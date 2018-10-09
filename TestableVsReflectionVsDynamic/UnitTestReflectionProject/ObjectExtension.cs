using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestReflectionProject
{
    public static class ObjectExtensions
    {
        public static TV SetPrivateStaticField<T, TV>(this object obj, string fieldName, TV fieldValue)
        {
            var oldValue = default(TV);
            var configFieldInfo = typeof(T).GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic);
            if (configFieldInfo != null)
            {
                oldValue = (TV)configFieldInfo.GetValue(null);
                configFieldInfo.SetValue(null, fieldValue);
            }

            return oldValue;
        }

        public static TV SetPrivateField<T, TV>(this object obj, string fieldName, TV fieldValue)
        {
            var oldValue = default(TV);
            var configFieldInfo = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (configFieldInfo != null)
            {
                oldValue = (TV)configFieldInfo.GetValue(obj);
                configFieldInfo.SetValue(obj, fieldValue);
            }

            return oldValue;
        }

        public static TV SetPrivateProperty<T, TV>(this object obj, string propertyName, TV propertyValue)
        {
            var oldValue = default(TV);
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyInfo != null)
            {
                oldValue = (TV)propertyInfo.GetValue(obj, null);
                propertyInfo.SetValue(obj, propertyValue, null);
            }

            return oldValue;
        }

        public static TV SetPrivatePropertyWithPublicGet<T, TV>(this object obj, string propertyName, TV propertyValue)
        {
            var oldValue = default(TV);
            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                oldValue = (TV)propertyInfo.GetValue(obj, null);
                propertyInfo.SetValue(obj, propertyValue, null);
            }

            return oldValue;
        }

        public static TPt GetPrivateProperty<T, TPt>(this object obj, string propertyName)
            where T : class
        {
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (propertyInfo == null)
            {
                return default(TPt);
            }

            return (TPt)propertyInfo.GetValue(obj);
        }

        public static TV InvokeNonPublicMethod<TV>(this object obj, string methodName, params object[] args)
        {
            var mi = obj.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi != null)
            {
                return (TV)mi.Invoke(obj, args);
            }

            return default(TV);
        }

        public static TV InvokeBaseNonPublicMethod<TV>(this object obj, string methodName, params object[] args)
        {
            var mi = obj.GetType().BaseType.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mi != null)
            {
                return (TV)mi.Invoke(obj, args);
            }

            return default(TV);
        }
    }
}
