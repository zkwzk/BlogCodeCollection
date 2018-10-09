using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace ConsoleApp1
{
    class Program
    {
        private static int FILECOUNT = 4000;
        static void Main(string[] args)
        {
            var testableText = string.Empty;
            var reflectionText = string.Empty;
            var wrapperText = string.Empty;
            var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../");
            var reflectionFilePath = Path.Combine(basePath, "template/Reflection.txt");
            var testableFilePath = Path.Combine(basePath, "template/Testable.txt");
            var wrapperFilePath = Path.Combine(basePath, "template/Wrapper.txt");
            var reflectionTargetPath = Path.Combine(basePath, "../UnitTestReflectionProject/testfiles");
            var testableTargetPath = Path.Combine(basePath, "../UnitTestProject1/testfiles");
            var wrapperTargetPath = Path.Combine(basePath, "../UnitTestWrapperProject/testfiles");
          
            reflectionText = File.ReadAllText(reflectionFilePath);
            testableText = File.ReadAllText(testableFilePath);
            wrapperText = File.ReadAllText(wrapperFilePath);
            GenerateFile(reflectionTargetPath, reflectionText, "AClassTest", FILECOUNT);
            GenerateFile(testableTargetPath, testableText, "AClassTest", FILECOUNT);
            GenerateFile(wrapperTargetPath, wrapperText, "AClassTest", FILECOUNT);
        }

        static void GenerateFile(string folderPath, string content, string fileName, int fileCount)
        {
            for (var index = 1; index <= fileCount; index++)
            {
                var fn = fileName + index + ".cs";
                var fileContent = content.Replace("{{number}}", index.ToString());
                File.WriteAllText(Path.Combine(folderPath,fn), fileContent);
            }
        }
    }
}
