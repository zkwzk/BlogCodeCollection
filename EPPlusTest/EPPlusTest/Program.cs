using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace EPPlusTest
{
    class Program
    {
        private static ExcelPackage xlPackage;
        static void Main(string[] args)
        {
            using (var fileStream = File.Open("../../test.xlsx", FileMode.Open, FileAccess.Read))
            {
                xlPackage = new ExcelPackage(fileStream);
                var workBook = xlPackage.Workbook;
                var workSheet = workBook.Worksheets["sheet1"];
                var converter = new ModelConverter<TestModel>(workSheet, 3);
                var result = converter.Convert();
                var json = JsonConvert.SerializeObject(result);
            }
        }
    }

    public class TestModel
    {
        [WorkSheetColumn("A")]
        public string Name { get; set; }

        [WorkSheetColumn("B")]
        public int Age { get; set; }

        [WorkSheetColumn("C")]
        [TypeConverter(typeof(NoSpaceConverter))]
        public string FavoriteFruit { get; set; }

        [WorkSheetColumn("D")]
        [TypeConverter(typeof(ListStringConverter))]
        public List<string> Hobby { get; set; }
    }

    public class ListStringConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var strValue = value.ToString();
                if (string.IsNullOrEmpty(strValue) || strValue == "N/A")
                {
                    return new List<string>();
                }

                return value.ToString().Split(',').ToList();
            }

            return value;
        }
    }

    public class NoSpaceConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var strValue = value.ToString();
                if (strValue == "N/A")
                {
                    return string.Empty;
                }

                return value.ToString().Replace(" ", "_");
            }

            return value;
        }
    }


    public class ModelConverter<T>  where T : class, new()
    {
        private readonly ExcelRange _excelRange;

        private readonly int _startRow;

        private readonly int _endRow;

        public ModelConverter(ExcelWorksheet workSheet, int startRow)
        {
            _excelRange = workSheet.Cells;
            _startRow = startRow;
            _endRow = workSheet.Dimension.End.Row;
        }

        public IList<T> Convert()
        {
            var mappingDic = GetMappingDic();
            var result = new List<T>();
            for (var index = _startRow; index <= _endRow; index++)
            {
                var instance = new T();
                foreach (var mappingInfo in mappingDic)
                {
                    mappingInfo.PropertyInfo.SetValue(instance,
                        mappingInfo.TypeConverter.ConvertFrom(_excelRange[string.Format("{0}{1}", mappingInfo.ColumnName, index)].Text), (object[])null);
                }

                result.Add(instance);
            }

            return result;
        }

        private List<MappingInfo> GetMappingDic()
        {
            var properties = typeof(T).GetProperties();
            var result = new List<MappingInfo>();
            foreach (var propertyInfo in properties)
            {
                var workColumnAttribute =
                    (WorkSheetColumnAttribute)propertyInfo.GetCustomAttributes(typeof(WorkSheetColumnAttribute), false).FirstOrDefault();
                if (workColumnAttribute == null)
                {
                    continue;
                }


                var mappingInfo =  new MappingInfo()
                {
                    ColumnName = workColumnAttribute.ColumnName,
                    PropertyInfo = propertyInfo,
                };

                var propertyDescriptorCollection = TypeDescriptor.GetProperties(typeof(T));
                mappingInfo.TypeConverter = propertyDescriptorCollection.Find(propertyInfo.Name, false).Converter;
                result.Add(mappingInfo);
            }

            return result;
        }

        public class MappingInfo
        {
            public PropertyInfo PropertyInfo { get; set; }

            public string ColumnName { get; set; }

            public TypeConverter TypeConverter { get; set; }
        }
    }


    [AttributeUsage(AttributeTargets.Property)]
    public class WorkSheetColumnAttribute : Attribute
    {
        public string ColumnName { get; private set; }

        public WorkSheetColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}
