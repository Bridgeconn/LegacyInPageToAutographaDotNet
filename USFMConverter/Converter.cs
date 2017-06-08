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
        private static short chapterCounter;
        private static short verseCounter;

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
            return Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + "_usfm.txt";
        }

        private string GetProcessedLine(string line)
        {
            if(ChapterNeedsToBeAdded())
            {
                line = "\\c " + chapterCounter + " " + line;
            }
            else if(VerseNeedsToBeAdded())
            {
                line = "\\v " + verseCounter + " " + line;
            }

            return line; 
        }


        private bool ChapterNeedsToBeAdded()
        {
            return false;
        }

        private bool VerseNeedsToBeAdded()
        {
            return false;
        }
    }
}
