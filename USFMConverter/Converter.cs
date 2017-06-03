using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace USFMConverter
{
    public class Converter
    {
        public List<string> ApplyUSFMTagsToFiles(List<string> fileList)
        {
            List<string> newFileList = new List<string>();

            foreach (string fileName in fileList)
            {
                var newFileName = GetNewFileName(fileName);
                newFileList.Add(newFileName);
                ApplyUSFMTags(fileName, newFileName);
            }

            return newFileList;
        }

        private bool ApplyUSFMTags(string sourceFilename, string targetFileName)
        {
            File.WriteAllLines(targetFileName, File.ReadAllLines(sourceFilename).Select(line => GetProcessedLine(line)).ToArray());
            return true;
        }

        private string GetNewFileName(string fileName)
        {
            return fileName; // TODO
        }

        private string GetProcessedLine(string line)
        {
            return line; // TODO
        }
    }
}
