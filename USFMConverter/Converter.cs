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
        private Regex digits = new Regex(@"((\d+))");
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

            StringBuilder sb = new StringBuilder();
            StreamReader file = new StreamReader(sourceFilename);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                GetProcessedLine(sb, line, id);
            }

            file.Close();
            
            File.WriteAllText(targetFileName, sb.ToString(), Encoding.UTF8);
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

        private void GetProcessedLine(StringBuilder sb, string line, string id)
        {
            line = line.Trim();
            line = line.Replace("\t", " ");

            if (line.IndexOf("۔")==0)
            {
                line = line.Substring(1) + "۔";
            }

            if (line.Trim() == string.Empty)
            {
                return;
            }

            if (!IsIDAdded)
            {
                string newline="";
                if (AddIDUSFMTag(ref newline, id))
                {
                    AddChapterUSFMTag(ref line);
                    sb.AppendLine(newline + Environment.NewLine + line);
                }
            }
            else
            {
                if (AddChapterUSFMTag(ref line))
                {
                    sb.AppendLine(line);
                }
                else if (AddVerseUSFMTag(ref line))
                {
                    line = Regex.Replace(line, @"\t|\n|\r", "");
                    line = line.Replace(Environment.NewLine, "");
                    sb.Append(Environment.NewLine + line);
                }
                else
                {
                    line = Regex.Replace(line, @"\t|\n|\r", "");
                    line = line.Replace(Environment.NewLine, "");
                    sb.Append(line);
                }
            }
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
                if (digits.IsMatch(line))
                {
                    line = digits.Replace(line, "");

                    line = "\\c " + ++chapterCounter + " " + line;

                    verseCounter = 0;

                    return true;
                }
            }
            else
            {
                // number found and compare with above
                if (digits.IsMatch(line))
                {
                    Match matches = Regex.Match(line, digits.ToString());
                    if (matches.Success)
                    {
                        // TODO: special case if (groups.Count > 1)

                        var chapterNumber = ushort.Parse(matches.Groups[2].Value);

                        if (chapterNumber == chapterCounter + 1 && chapterNumber != verseCounter + 1)
                        {
                            line = digits.Replace(line, "");
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
                Match matches = Regex.Match(line, digits.ToString());

                if (matches.Success)
                {
                    // TODO: special case if (groups.Count > 1) if two are found break them
                    foreach(Capture capturedNumber in matches.Groups[2].Captures)
                    {
                        var verseNumber = ushort.Parse(capturedNumber.Value);

                        if (verseNumber >= verseCounter + 1)
                        {
                            line = digits.Replace(line, "");
                            line = Regex.Replace(line, @"\t|\n|\r", "");
                            line = line.Replace(Environment.NewLine, "");
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
