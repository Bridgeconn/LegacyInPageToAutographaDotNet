using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace USFMConverter
{
    public class Converter
    {
        private bool IsIDAdded;
        private short chapterCounter=0;
        private short verseCounter=0;
        private bool isTitleContainsChapter;
        private Regex digitsWithDotOrHyphen = new Regex(@"([ \.\-]*(\d+)[ \.\-]*)");

        public List<string> ApplyUSFMTagsToFiles(List<string> fileList, ref string errorMessage)
        {
            List<string> newFileList = new List<string>();

            foreach (string fileName in fileList)
            {
                var newFileName = GetNewFileName(fileName);
                newFileList.Add(newFileName);
                if(!ApplyUSFMTags(fileName, newFileName))
                {
                    errorMessage += " " + "Book id error: " + fileName  + " ";
                }
                ResetCounters();
            }


            return newFileList;
        }

        private void ResetCounters()
        {
            IsIDAdded=false;
            chapterCounter=0;
            verseCounter=0;
        }

        private bool ApplyUSFMTags(string sourceFilename, string targetFileName)
        {
            var id = GetFileID(targetFileName);
            if (id.Length > 3)
            {
                return false;
            }

            File.WriteAllLines(targetFileName, File.ReadAllLines(sourceFilename).Select(line => GetProcessedLine(line, id)).ToArray(), Encoding.UTF8);
            return true;
        }

        private string GetFileID(string targetFileName)
        {
            string id = "No book ID has been prefixed in the filename with an underscore";
            var fileName = Path.GetFileName(targetFileName);
            if (fileName.IndexOf('_') == 3)
            {
                id = fileName.Substring(0, 3);
            }

            return id;
        }

        private string GetNewFileName(string fileName)
        {
            return Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + "_usfm.usfm";
        }

        private string GetProcessedLine(string line, string id)
        {
            line = line.Trim();
            line = line.Replace("\t", " ");

            if (line.IndexOf("۔")==0)
            {
                line = line.Substring(1) + "۔";
            }

            if (line.Trim() == string.Empty)
            {
                return line;
            }

            if (!IsIDAdded)
            {
                string newline="";
                if (AddIDUSFMTag(ref newline, id))
                {
                    AddChapterUSFMTag(ref line);
                    return newline + Environment.NewLine + line;
                }
            }
            else
            {
                if (AddChapterUSFMTag(ref line))
                {
                    return line;
                }
                else if (AddVerseUSFMTag(ref line))
                {
                    return line;
                }
            }

            return line; 
        }

        private bool AddIDUSFMTag(ref string line, string id)
        {
            if (IsIDAdded)
            {
                return false;
            }
            else
            {
                line = "\\id " + id;
                //if (digitsWithDotOrHyphen.IsMatch(line))
                //{
                //        var matches = digitsWithDotOrHyphen.Matches(line);
                //        if (matches.Count > 0)
                //        {
                //            line = digitsWithDotOrHyphen.Replace(line, "");
                //        }
                //        line = "\\id " + line + Environment.NewLine + "\\c " + ++chapterCounter;

                //}
                //else
                //{
                //    line = "\\id " + line;
                //}

                IsIDAdded = true;

                return true;
            }
        }

        private bool AddChapterUSFMTag(ref string line)
        {
            if (verseCounter == 0 && chapterCounter == 0)
            {
                if (digitsWithDotOrHyphen.IsMatch(line))
                {
                    line = digitsWithDotOrHyphen.Replace(line, "");

                    line = "\\c " + ++chapterCounter + " " + line;

                    verseCounter = 0;

                    return true;
                }
            }
            else
            {
                // number found and compare with above
                if (digitsWithDotOrHyphen.IsMatch(line))
                {
                    Match matches = Regex.Match(line, digitsWithDotOrHyphen.ToString());
                    if (matches.Success)
                    {
                        // TODO: special case if (groups.Count > 1)

                        var chapterNumber = ushort.Parse(matches.Groups[2].Value);

                        if (chapterNumber == chapterCounter + 1 && chapterNumber != verseCounter + 1)
                        {
                            line = digitsWithDotOrHyphen.Replace(line, "");
                            line = "\\c " + ++chapterCounter + " " + line;
                            verseCounter = 0;
                            return true;
                        }
                    }
                    
                }
            }

            return false;
        }

        private bool AddVerseUSFMTag(ref string line)
        {
            if(IsIDAdded && chapterCounter>0)
            {
                // number found and compare with above
                Match matches = Regex.Match(line, digitsWithDotOrHyphen.ToString());

                if (matches.Success)
                {
                    // TODO: special case if (groups.Count > 1) if two are found break them
                    foreach(Capture capturedNumber in matches.Groups[2].Captures)
                    {
                        var verseNumber = ushort.Parse(capturedNumber.Value);

                        if (verseNumber >= verseCounter + 1)
                        {
                            line = digitsWithDotOrHyphen.Replace(line, "");
                            line = "\\v " + ++verseCounter + " " + line;
                            
                        }
                        else
                        {
                            // TODO: Special case;
                        }
                    }
                    return true;
                }
            }
            return false;
        }
    }
}
