using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UnicodeConverter;

namespace InPageToAutographa
{
    public class ConverterViewModel : ObservableObject
    {
        string sourceFName;
        string targetFName;
        string targetFName_Sp;
        string openLastFile;
        string outPut = " ";
        string outPut_Sp = " ";
        byte[] binaryData;
        bool start_test = false;
        bool cancel_test = false;
        string[] inputFileNames;
        string fileListTitle = "";
        List<string> targetFileNames = new List<string>();

        /////  hot key function /////////////////// 

        public const int altKey = 0x1;
        public const int ctrlKey = 0x2;
        public const int hotKey = 0x312;

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int key);

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UnregisterHotKey(IntPtr hwnd, int id);

        private string btnConvertText;
        private bool isProgressBarVisible;
        private BackgroundWorker bgw;
        private BackgroundWorker bgw4_new;
        private int progressBarValue;
        private bool rbtnUrduChecked;

        public int ProgressBarValue
        {
            get
            {
                return progressBarValue;
            }
            set
            {
                progressBarValue = value;
                RaisePropertyChangedEvent("ProgressBarValue");
            }
        }

        public string BtnConvertText
        {
            get => btnConvertText;
            set
            {
                btnConvertText = value;
                RaisePropertyChangedEvent("BtnConvertText");
            }
        }

        public bool IsProgressBarVisible
        {
            get => isProgressBarVisible;
            set
            {
                isProgressBarVisible = value;
                RaisePropertyChangedEvent("IsProgressBarVisible");
            }
        }
        private bool checkboxAdditionalBaariYieChecked;
        private bool checkboxRemoveKashidaChecked;
        private bool checkboxChangePChecked;
        private bool checkboxChangeCCChecked;
        private bool checkboxReverseSolidusSignChecked;
        private bool checkboxReverseThousSeparatorChecked;
        private bool checkboxReverseQuotMarksChecked;
        private bool checkboxCorrectWawHamzaChecked;
        private bool checkboxCorrectHehHamzaChecked;
        private bool checkboxReverseNumbersDigitsChecked;
        private bool checkboxCorrectYearSignChecked;
        private bool checkboxRemoveErabsChecked;
        private bool checkboxRemoveDoubleSpaceChecked;
        private bool checkboxCorrectBariYeeChecked;
        private bool checkboxAdditionalKashidaChecked;
        private string statusBarText;

        public string StatusBarText
        {
            get { return statusBarText; }
            set { statusBarText = value;
                RaisePropertyChangedEvent("StatusBarText");
            }
        }

        
        public string FileListTitle
        {
            get { return fileListTitle; }
            set
            {
                fileListTitle = value;
                RaisePropertyChangedEvent("FileListTitle");
            }
        }

        public bool CheckboxAdditionalKashidaChecked
        {
            get
            {
                return checkboxAdditionalKashidaChecked;
            }

            set
            {
                checkboxAdditionalKashidaChecked = value;
                RaisePropertyChangedEvent("CheckboxAdditionalKashidaChecked");
            }
        }

        public bool CheckboxAdditionalBaariYieChecked
        {
            get
            {
                return checkboxAdditionalBaariYieChecked;
            }

            set
            {
                checkboxAdditionalBaariYieChecked = value;
                RaisePropertyChangedEvent("CheckboxAdditionalBaariYieChecked");
            }
        }

        public bool CheckboxCorrectYearSignChecked { get { return checkboxCorrectYearSignChecked; } set { checkboxCorrectYearSignChecked = value; RaisePropertyChangedEvent("CheckboxCorrectYearSignChecked"); } }
        public bool CheckboxCorrectHehHamzaChecked { get { return checkboxCorrectHehHamzaChecked; } set { checkboxCorrectHehHamzaChecked = value; RaisePropertyChangedEvent("CheckboxCorrectHehHamzaChecked"); } }
        public bool CheckboxCorrectWawHamzaChecked { get { return checkboxCorrectWawHamzaChecked; } set { checkboxCorrectWawHamzaChecked = value; RaisePropertyChangedEvent("CheckboxCorrectWawHamzaChecked"); } }
        public bool CheckboxCorrectBariYeeChecked { get { return checkboxCorrectBariYeeChecked; } set { checkboxCorrectBariYeeChecked = value; RaisePropertyChangedEvent("CheckboxCorrectBariYeeChecked"); } }

        public bool CheckboxRemoveDoubleSpaceChecked { get { return checkboxRemoveDoubleSpaceChecked; } set { checkboxRemoveDoubleSpaceChecked = value; RaisePropertyChangedEvent("CheckboxRemoveDoubleSpaceChecked"); } }
        public bool CheckboxRemoveKashidaChecked { get { return checkboxRemoveKashidaChecked; } set { checkboxRemoveKashidaChecked = value; RaisePropertyChangedEvent("CheckboxRemoveKashidaChecked"); } }
        public bool CheckboxRemoveErabsChecked { get { return checkboxRemoveErabsChecked; } set { checkboxRemoveErabsChecked = value; RaisePropertyChangedEvent("CheckboxRemoveErabsChecked"); } }

        public bool CheckboxReverseNumbersDigitsChecked { get { return checkboxReverseNumbersDigitsChecked; } set { checkboxReverseNumbersDigitsChecked = value; RaisePropertyChangedEvent("CheckboxReverseNumbersDigitsChecked"); } }
        public bool CheckboxReverseSolidusSignChecked { get { return checkboxReverseSolidusSignChecked; } set { checkboxReverseSolidusSignChecked = value; RaisePropertyChangedEvent("CheckboxReverseSolidusSignChecked"); } }
        public bool CheckboxReverseThousSeparatorChecked { get { return checkboxReverseThousSeparatorChecked; } set { checkboxReverseThousSeparatorChecked = value; RaisePropertyChangedEvent("CheckboxReverseThousSeparatorChecked"); } }
        public bool CheckboxReverseQuotMarksChecked { get { return checkboxReverseQuotMarksChecked; } set { checkboxReverseQuotMarksChecked = value; RaisePropertyChangedEvent("CheckboxReverseQuotMarksChecked"); } }
        
        public bool CheckboxChangePChecked { get { return checkboxChangePChecked; } set { checkboxChangePChecked = value; RaisePropertyChangedEvent("CheckboxChangePChecked"); } }
        public bool CheckboxChangeCCChecked { get { return checkboxChangeCCChecked; } set { checkboxChangeCCChecked = value; RaisePropertyChangedEvent("CheckboxChangeCCChecked"); } }

        public string[] SourceFileNames
        {
            get
            {
                return inputFileNames;
            }
            set
            {
                inputFileNames = value;
                RaisePropertyChangedEvent("SourceFileNames");
            }
        }

        public ConverterViewModel()
        {
            CharacterMap.initIp2ucCharacter();
            CharacterMap.init_cpinpage2unicode();
            CharacterMap.init_cpunicode2inpage();

            LoadSettings();
            bgw = new BackgroundWorker();
        }

        private void LoadSettings()
        {

            CheckboxCorrectYearSignChecked = Properties.Settings.Default.CheckboxCorrectYearSignChecked;
            CheckboxCorrectHehHamzaChecked= Properties.Settings.Default.CheckboxCorrectHehHamzaChecked;
            CheckboxCorrectWawHamzaChecked= Properties.Settings.Default.CheckboxCorrectWawHamzaChecked;
            CheckboxCorrectBariYeeChecked= Properties.Settings.Default.CheckboxCorrectBariYeeChecked;

            CheckboxRemoveDoubleSpaceChecked = Properties.Settings.Default.CheckboxRemoveDoubleSpaceChecked;
            CheckboxRemoveKashidaChecked= Properties.Settings.Default.CheckboxRemoveKashidaChecked ;
            CheckboxRemoveErabsChecked= Properties.Settings.Default.CheckboxRemoveErabsChecked;

            CheckboxReverseNumbersDigitsChecked = Properties.Settings.Default.CheckboxReverseNumbersDigitsChecked;
            CheckboxReverseSolidusSignChecked= Properties.Settings.Default.CheckboxReverseSolidusSignChecked ;
            CheckboxReverseThousSeparatorChecked= Properties.Settings.Default.CheckboxReverseThousSeparatorChecked;
            CheckboxReverseQuotMarksChecked= Properties.Settings.Default.CheckboxReverseQuotMarksChecked;

            CheckboxChangePChecked = Properties.Settings.Default.CheckboxChangePChecked;
            CheckboxChangeCCChecked= Properties.Settings.Default.CheckboxChangeCCChecked;

            CheckboxAdditionalBaariYieChecked = Properties.Settings.Default.CheckboxAdditionalBaariYieChecked;
            CheckboxAdditionalKashidaChecked = Properties.Settings.Default.CheckboxAdditionalKashidaChecked;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.CheckboxCorrectYearSignChecked = CheckboxCorrectYearSignChecked;
            Properties.Settings.Default.CheckboxCorrectHehHamzaChecked = CheckboxCorrectHehHamzaChecked;
            Properties.Settings.Default.CheckboxCorrectWawHamzaChecked = CheckboxCorrectWawHamzaChecked;
            Properties.Settings.Default.CheckboxCorrectBariYeeChecked = CheckboxCorrectBariYeeChecked;

            Properties.Settings.Default.CheckboxRemoveDoubleSpaceChecked = CheckboxRemoveDoubleSpaceChecked;
            Properties.Settings.Default.CheckboxRemoveKashidaChecked = CheckboxRemoveKashidaChecked;
            Properties.Settings.Default.CheckboxRemoveErabsChecked = CheckboxRemoveErabsChecked;

            Properties.Settings.Default.CheckboxReverseNumbersDigitsChecked = CheckboxReverseNumbersDigitsChecked;
            Properties.Settings.Default.CheckboxReverseSolidusSignChecked = CheckboxReverseSolidusSignChecked;
            Properties.Settings.Default.CheckboxReverseThousSeparatorChecked = CheckboxReverseThousSeparatorChecked;
            Properties.Settings.Default.CheckboxReverseQuotMarksChecked = CheckboxReverseQuotMarksChecked;

            Properties.Settings.Default.CheckboxChangePChecked = CheckboxChangePChecked;
            Properties.Settings.Default.CheckboxChangeCCChecked = CheckboxChangeCCChecked;

            Properties.Settings.Default.CheckboxAdditionalBaariYieChecked = CheckboxAdditionalBaariYieChecked;
            Properties.Settings.Default.CheckboxAdditionalKashidaChecked = CheckboxAdditionalKashidaChecked;

            Properties.Settings.Default.Save();
        }

        private void WriteStatusMessage(string message)
        {
            StatusBarText = message;
        }

        private bool CheckBookNamePrefixes(string[] fileNames)
        {
            bool isValid = true;

            foreach (string fileName in fileNames)
            {
                if(Path.GetFileNameWithoutExtension(fileName).IndexOf('_') != 3)
                {
                    isValid = false;
                    break;
                }
            }

            if (!isValid)
            {
                WriteStatusMessage("No book ID has been prefixed in the filename with an underscore as stated in the guidelines in the about section. Please modify file name(s).");
                return false;
            }

            return isValid;
        }

        private void btnOpenDlg_Click()
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Filter = "Inpage files (*.inp)|*.inp";
            OFD.Title = "Open inpage file";
            OFD.Multiselect = true;

            if (OFD.ShowDialog() == true)
            {
                if (OFD.FileNames.Length == 0 || !(Path.GetExtension(OFD.FileName) == ".inp" | Path.GetExtension(OFD.FileName) == ".INP"))
                {
                    WriteStatusMessage("Please select a valid inpage file");

                    //txtSourceLocation.Text = "";
                    //txtTatgetLocation.Text = "";
                    //ButtonConvertEnabled = false;
                    //ButtonOpenFileEnabled = false;
                    //btnFConvert.Enabled = false;
                }
                else if(CheckBookNamePrefixes(OFD.FileNames))
                {
                    FileListTitle = "Source files";
                    //txtSourceLocation.Text = OFD.FileName;
                    SourceFileNames = OFD.FileNames;
                    WriteStatusMessage("Ready to convert, please click proceed button");
                    //ButtonConvertEnabled = true;
                    //btnFConvert.Enabled = true;
                    //ButtonInPageFileEnabled = false;
                    //ButtonOpenFileEnabled = false;
                }
                
            }
            else
            {
                WriteStatusMessage("Ready");
            }
        }

        public ICommand ConvertFilesCommand
        {
            get { return new DelegateCommand(btnConvert_Click); }
        }

        public ICommand SelectFilesCommand
        {
            get { return new DelegateCommand(btnOpenDlg_Click); }
        }

        private void btnConvert_Click()
        {
            if (BtnConvertText == "Cancel")
            {
                MessageBoxResult msgResult = MessageBox.Show("Do you want cancel the converting?", "Cancel ?", MessageBoxButton.YesNo);
                if (msgResult == MessageBoxResult.Yes)
                {
                    cancel_test = true;
                    BtnConvertText = "Convert";
                }
            }
            else
            {
                try
                {

                    IsProgressBarVisible = true;
                    BtnConvertText = "Cancel";
                    bgw.WorkerReportsProgress = true;
                    bgw.DoWork += bgw_DoWork;
                    bgw.ProgressChanged += bgw_ProgressChanged;
                    bgw.RunWorkerCompleted += bgw_RunWorkerCompleted;
                    bgw.RunWorkerAsync();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void bgw_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int individualPercentage = 100 / inputFileNames.Length;
            targetFileNames.Clear();
            foreach (var fileName in inputFileNames)
            {
                sourceFName = fileName;
                targetFName = Path.GetDirectoryName(sourceFName) + "\\" + Path.GetFileNameWithoutExtension(sourceFName) + "_unicode.txt";
                targetFileNames.Add(targetFName);
                targetFName_Sp = Path.GetDirectoryName(sourceFName) + "\\" + Path.GetFileNameWithoutExtension(sourceFName) + "_with_out_spaces.txt";

                FileInfo finfo = new FileInfo(sourceFName);
                long numBytes = finfo.Length;
                FileStream fStream = new FileStream(sourceFName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);

                binaryData = br.ReadBytes(Convert.ToInt32(numBytes));
                br.Close();
                fStream.Close();
                WriteStatusMessage("Currently converting: " + fileName);
                int p_prog = binaryData.Length / 100;
                int startP = CharacterMap.FindStartPosition(binaryData);
                int endP = CharacterMap.FindEndPosition(binaryData, startP);

                string digitBuffer = "";

                for (int i = startP; i <= endP; i++)
                {
                    if ((binaryData[i] == 4))
                    {
                        // find hamza position

                        if (binaryData[i + 1] == 163 & FindHamzaPosition(i))
                        {
                            outPut += Convert.ToChar(1574).ToString();
                            // 1574 ? ya   shift + 4
                            i += 1;
                        }
                        else if (binaryData[i + 1] == 165 & ((168 > binaryData[i + 3] & binaryData[i + 3] > 128)
                            | binaryData[i + 3] == 170 | binaryData[i + 3] == 182 | binaryData[i + 3] == 184
                            | binaryData[i + 3] == 185 | binaryData[i + 3] == 200 | binaryData[i + 3] == 201))
                        {
                            if (CheckboxAdditionalBaariYieChecked == true)
                            {
                                outPut += Convert.ToChar(1740).ToString();
                                //bari-ya ? convert to  ? ?  
                                i += 1;
                            }
                            else
                            {
                                outPut += Convert.ToChar(1746).ToString();
                                i += 1;
                                if (binaryData[i + 3] == 32)
                                {
                                    i += 2;
                                }
                            }
                        }
                        else if (binaryData[i + 1] == 161 & !(binaryData[i + 3] == 32 | binaryData[i + 2] == 13 | (255 > binaryData[i + 3] & binaryData[i + 3] > 202) | binaryData[i + 2] == 9 | FindHamzaPosition(i + 3) == false | (198 > binaryData[i + 3] & binaryData[i + 3] > 167)))
                        {
                            outPut += Convert.ToChar(1722).ToString();
                            // add space after ?  noon guna
                            outPut += Convert.ToChar(32).ToString();
                            i += 1;
                        }
                        else if (((binaryData[i + 1] == 177) & (binaryData[i + 3] == 177)))
                        {
                            //  remove 1 jazam 
                            outPut += Convert.ToChar(CharacterMap.ip2uc[Convert.ToInt32(binaryData[i + 1])]).ToString();
                            i += 3;
                        }
                        else if (binaryData[i + 1] == 169 & !(binaryData[i + 3] == 169))
                        {
                            // //  extra character  " tatbeeq "  pass this character 
                            if (CheckboxAdditionalKashidaChecked)
                            {
                                outPut += Convert.ToChar(CharacterMap.ip2uc[Convert.ToInt32(binaryData[i + 1])]).ToString();
                                i += 1;
                            }
                            else
                            {
                                i += 1;
                            }
                        }
                        else if (binaryData[i + 1] == 166 & outPut[outPut.Length - 1].ToString() == Convert.ToChar(1574).ToString())
                        {
                            //remove ya-hamza  before hamza
                            outPut = outPut.Remove(outPut.Length - 1, 1);
                            outPut += Convert.ToChar(CharacterMap.ip2uc[166]).ToString();
                            outPut += Convert.ToChar(CharacterMap.ip2uc[191]).ToString();
                            i += 1;
                        }
                        else if (binaryData[i + 1] == 253 | binaryData[i + 1] == 254)
                        {
                            //  Change " 
                            if (CheckboxChangeCCChecked == true)
                            {
                                if (binaryData[i + 1] == 253)
                                {
                                    outPut += Convert.ToChar(CharacterMap.ip2uc[254]).ToString();
                                    i += 1;
                                }
                                else
                                {
                                    outPut += Convert.ToChar(CharacterMap.ip2uc[253]).ToString();
                                    i += 1;
                                }
                            }
                            else
                            {
                                outPut += Convert.ToChar(CharacterMap.ip2uc[Convert.ToInt32(binaryData[i + 1])]);
                                i += 1;
                            }

                        }
                        else if (binaryData[i + 1] == 224)
                        {
                            // ... double  
                            outPut += Convert.ToChar(CharacterMap.ip2uc[224]).ToString();
                            outPut += Convert.ToChar(CharacterMap.ip2uc[224]).ToString();
                            i += 1;
                        }
                        else if (binaryData[i + 1] == 184 & !(binaryData[i + 3] == 32 | binaryData[i + 2] == 13 | binaryData[i + 2] == 9 | (255 > binaryData[i + 3] & binaryData[i + 3] > 202) | (198 > binaryData[i + 3] & binaryData[i + 3] > 167)))
                        {
                            // add space after ?
                            outPut += Convert.ToChar(CharacterMap.ip2uc[184]).ToString();
                            outPut += Convert.ToChar(CharacterMap.ip2uc[32]).ToString();
                            i += 1;
                        }
                        else if (((binaryData[i + 1] == 162 | binaryData[i + 1] == 182) & outPut[outPut.Length - 1].ToString() == Convert.ToChar(1574).ToString()))
                        {
                            //remove ya-hamza befor wao-haza or wao
                            outPut = outPut.Remove(outPut.Length - 1, 1);
                            outPut += Convert.ToChar(CharacterMap.ip2uc[182]).ToString();
                            i += 1;
                        }
                        else if ((218 > binaryData[i + 1] & binaryData[i + 1] > 207) & ((218 > binaryData[i + 3] & binaryData[i + 3] > 207) | (binaryData[i + 3] == 223 | binaryData[i + 2] == 47)))
                        {
                            //ginti ;)                                                          

                            string temValue = "";
                            while (!(!((218 > binaryData[i + 1] & binaryData[i + 1] > 207) | (binaryData[i + 1] == 223 | binaryData[i] == 47))))
                            {
                                if (binaryData[i] == 47)
                                {
                                    temValue += Convert.ToChar(47).ToString();
                                }
                                else
                                {
                                    temValue += Convert.ToChar(CharacterMap.ip2uc[Convert.ToInt32(binaryData[i + 1])].ToString());
                                }

                                if (binaryData[i + 1] == 47 | binaryData[i] == 47)
                                {
                                    i += 1;
                                }
                                else
                                {
                                    i += 2;
                                }

                            }
                            if (CheckboxChangePChecked == false)
                            {
                                outPut += CharacterMap.ChangePositon(temValue);
                            }
                            else
                            {
                                outPut += temValue;
                            }
                            i -= 1;
                        }
                        else
                        {
                            outPut += Convert.ToChar(CharacterMap.ip2uc[Convert.ToInt32(binaryData[i + 1])]);
                            i += 1;
                        }


                    }
                    else if (binaryData[i] == 32)
                    {
                        outPut += Convert.ToChar(32);
                    }
                    else if (binaryData[i] == 13)
                    {
                        outPut += Convert.ToChar(13);
                        outPut += Convert.ToChar(10);
                        i += 3;
                    }
                    else if (binaryData[i] == 9)
                    {
                        outPut += Convert.ToChar(9);

                    }
                    else if (64 > binaryData[i] & binaryData[i] > 32)
                    {

                        if ((58 > binaryData[i] & binaryData[i] > 47) & binaryData[i + 1] == 32)
                        {
                            bool boolChkEnter = false;
                            string my_tempVar = "";
                            // Or my_binaryData[i] = 47)
                            while (!(!((58 > binaryData[i] & binaryData[i] > 47) | binaryData[i] == 32)))
                            {
                                boolChkEnter = true;
                                my_tempVar += Convert.ToChar(binaryData[i]).ToString();
                                i += 1;
                            }
                            if (CheckboxChangePChecked == false)
                            {
                                outPut += CharacterMap.ChangePositon(my_tempVar);
                            }
                            else
                            {
                                outPut += my_tempVar;
                            }


                            if (boolChkEnter)
                            {
                                i -= 1;
                            }

                        }
                        else
                        {
                            if (!(47 < binaryData[i] & binaryData[i] < 58) && string.IsNullOrEmpty(digitBuffer))
                            {
                                outPut += Convert.ToChar(binaryData[i]);
                            }
                            else

                            {
                                if ((47 < binaryData[i] & binaryData[i] < 58) & (47 > binaryData[i + 1] || binaryData[i + 1] > 58))
                                {
                                    // do processing
                                    if (outPut.LastIndexOf(".") == outPut.Length - 1)
                                    {
                                        outPut = outPut.Remove(outPut.Length - 1);
                                    }
                                    outPut += Convert.ToChar(13);
                                    outPut += Convert.ToChar(10);
                                    outPut += Convert.ToChar(9);
                                    outPut += digitBuffer + Convert.ToChar(binaryData[i]);
                                    // clear buffer and set digBuff
                                    digitBuffer = "";
                                }
                                else
                                {
                                    // store up
                                    digitBuffer += Convert.ToChar(binaryData[i]);
                                }
                            }
                        }
                    }
                    else if (256 > binaryData[i] & binaryData[i] > 32)
                    {
                        outPut += Convert.ToChar(binaryData[i]).ToString();
                    }
                    if (cancel_test)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }

                    bgw.ReportProgress(Convert.ToInt32((i / p_prog) * individualPercentage));
                }
                if (cancel_test == false)
                {
                    try
                    {

                        System.IO.File.WriteAllText(targetFName, outPut, System.Text.Encoding.UTF8);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    Remove_Spaces();

                    //MessageBox.Show("Done");
                    outPut = " ";
                    outPut_Sp = "";
                    start_test = false;
                }
            }
        }

        private int GetIndexOfNonDigit(int currentIndex)
        {
            int index = currentIndex;
            int relativeIndex = 0;
            while(47 < binaryData[index -1] & binaryData[index-1] < 58)
            {
                index--;
                relativeIndex++;
            }
            return relativeIndex;
        }
        //private bool IsPartOfANumber(int currentIndex, int numberOfIndexes)
        //{
        //    for (int index = currentIndex; index >numberOfIndexes; index++)
        //    {
        //        if (47 < binaryData[index] & binaryData[index] < 58)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private void bgw_ProgressChanged(System.Object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ProgressBarValue = e.ProgressPercentage;
        }

        private void bgw_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (cancel_test)
            {
                WriteStatusMessage("Process is canceled by the user");
                ProgressBarValue = 0;
                IsProgressBarVisible = false;
                outPut = " ";
                outPut_Sp = "";
                start_test = false;
                //end_test = True
                cancel_test = false;
            }
            else
            {
                ProgressBarValue = 0;
                //ButtonOpenFileEnabled = true;
                //ButtonInPageFileEnabled = true;
                BtnConvertText = "Convert";
                SourceFileNames = null;
                USFMConverter.Converter converter = new USFMConverter.Converter();
                string errorMessage = "";
                converter.ApplyUSFMTagsToFiles(new List<string>(targetFileNames), ref errorMessage);
                IsProgressBarVisible = false;
                if (string.IsNullOrEmpty(errorMessage))
                    WriteStatusMessage("All files are successfully converted and saved in the selected input file's directory.");
                else
                    WriteStatusMessage("Please read about section for guidelines - encountered errors while converting:: " + errorMessage);
            }
        }

        private bool FindHamzaPosition(Int32 i)
        {
            bool functionReturnValue = false;

            if (binaryData[i + 3] != 32 & !(binaryData[i + 2] == 13))
            {

                if ((binaryData[i + 3] == 170 | binaryData[i + 3] == 171 | binaryData[i + 3] == 172 | binaryData[i + 3] == 176 | binaryData[i + 3] == 177 | binaryData[i + 3] == 168 | binaryData[i + 3] == 181 | binaryData[i + 3] == 173 | binaryData[i + 3] == 180 | binaryData[i + 3] == 179 | binaryData[i + 3] == 169))
                {

                    if (!(binaryData[i + 4] == 13 | binaryData[i + 5] == 32))
                    {
                        return true;
                        return functionReturnValue;

                    }
                    else
                    {
                        return false;
                        return functionReturnValue;
                    }
                }
                else if ((186 < binaryData[i + 3] & binaryData[i + 3] < 255))
                {
                    return false;
                    return functionReturnValue;
                }
                else
                {
                    return true;
                    return functionReturnValue;
                }
            }
            else
            {
                return false;
                return functionReturnValue;
            }
            return functionReturnValue;
        }

        private bool FindHamzaPositionI2U(Int32[] ipByte, Int32 i)
        {
            bool functionReturnValue = false;
            try
            {

                if (ipByte[i + 3] != 32 & !(ipByte[i + 2] == 13))
                {

                    if ((ipByte[i + 3] == 170 | ipByte[i + 3] == 171 | ipByte[i + 3] == 172 | ipByte[i + 3] == 176 | ipByte[i + 3] == 177 | ipByte[i + 3] == 168 | ipByte[i + 3] == 181 | ipByte[i + 3] == 173 | ipByte[i + 3] == 180 | ipByte[i + 3] == 179 | ipByte[i + 3] == 169))
                    {

                        if (!(ipByte[i + 4] == 13 | ipByte[i + 5] == 32))
                        {
                            return true;
                            return functionReturnValue;

                        }
                        else
                        {
                            return false;
                            return functionReturnValue;
                        }
                    }
                    else if ((186 < ipByte[i + 3] & ipByte[i + 3] < 255))
                    {
                        return false;
                        return functionReturnValue;
                    }
                    else
                    {
                        return true;
                        return functionReturnValue;
                    }
                }
                else
                {
                    return false;
                    return functionReturnValue;
                }

            }
            catch (Exception ex)
            {
            }
            return functionReturnValue;

        }

        private void Remove_Spaces()
        {
            string[] removeSpaces = {
                   "?",
                   "?",
                   "?",
                   "?",
                   "?",
                   "?",
                   "?",
                   "?",
                   "?",
                   "?",
                   "?",
                   "?"
               };
            outPut_Sp = outPut;

            for (int i = 0; i <= removeSpaces.Length - 1; i++)
            {
                outPut_Sp = outPut_Sp.Replace(removeSpaces[i].ToString() + " ", removeSpaces[i].ToString());
            }
            outPut = Regex.Replace(outPut, "[ ]+[ ]", " ");
            try
            {
                System.IO.File.WriteAllText(targetFName_Sp, outPut_Sp, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //private void frmInPageConverter_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        //{
        //    //NotifyIcon1.Dispose();
        //    //UnregisterHotKey(this.Handle, 100);
        //    //UnregisterHotKey(this.Handle, 200);
        //    //UnregisterHotKey(this.Handle, 300);

        //}

        //private void frmInPageConverter_Load(System.Object sender, System.EventArgs e)
        //{

        //    try
        //    {

        //        Rectangle WArea = SystemInformation.WorkingArea;
        //        int x = (WArea.Width - this.Width) / 2;
        //        this.Location = new Point(x, 200);
        //        RegisterHotKey(this.Handle, 100, altKey + ctrlKey, Keys.I);
        //        RegisterHotKey(this.Handle, 200, altKey + ctrlKey, Keys.U);
        //        RegisterHotKey(this.Handle, 300, altKey + ctrlKey, Keys.A);
        //        CharacterMap.initCharacterMap.ip2ucCharacter();
        //        CharacterMap.init_cpinpage2unicode();
        //        CharacterMap.init_cpunicode2inpage();
        //        NotifyIcon1.Visible = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        //private void btnOpenFile_Click(System.Object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        Process.Start(targetFName);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + " : " + targetFName);
        //    }

        //}

        //private void btnRemoveSpacesFile_Click(System.Object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        Process.Start(targetFName_Sp);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + " : " + targetFName);
        //    }
        //}

        //private void btnInPageFile_Click(System.Object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        Process.Start(sourceFName);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + " : " + targetFName);
        //    }
        //}

        private void Inpage2Unicode()
        {
            string ipString = Clipboard.GetText() + "   ";
            char[] ipChar = ipString.ToCharArray();
            int ipLength = ipString.Length - 1;
            Int32[] ipByte = new Int32[ipLength + 1];
            string ucOutput = " ";

            for (int i = 0; i <= ipLength; i += 1)
            {
                ipByte[i] = Convert.ToInt32(ipChar[i]);
            }

            try
            {

                for (int i = 0; i <= ipByte.Length - 1; i++)
                {
                    if ((ipByte[i] == 4))
                    {
                        // find hamza position

                        if (ipByte[i + 1] == 163 & FindHamzaPositionI2U(ipByte, i))
                        {
                            ucOutput += Convert.ToChar(1574).ToString();
                            // 1574 ? ya   shift + 4
                            i += 1;
                        }
                        else if (ipByte[i + 1] == 165 & ((167 > ipByte[i + 3] & ipByte[i + 3] > 128) 
                            | (750 > ipByte[i + 3] & ipByte[i + 3] > 338) 
                            | (8485 > ipByte[i + 3] & ipByte[i + 3] > 8210) 
                            | ipByte[i + 3] == 170 | ipByte[i + 3] == 182 
                            | ipByte[i + 3] == 184 | ipByte[i + 3] == 185 
                            | ipByte[i + 3] == 200 | ipByte[i + 3] == 201))
                        {
                            if (CheckboxAdditionalBaariYieChecked == true)
                            {
                                ucOutput += Convert.ToChar(1740).ToString();
                                //bari-ya ? convert to  ? ?  
                                i += 1;
                            }
                            else
                            {
                                ucOutput += Convert.ToChar(1746).ToString();
                                i += 1;
                                if (ipByte[i + 3] == 32)
                                {
                                    i += 2;
                                }
                            }
                        }
                        else if (ipByte[i + 1] == 161 & !(ipByte[i + 3] == 32 | ipByte[i + 2] == 13 | (255 > ipByte[i + 3] & ipByte[i + 3] > 202) | ipByte[i + 2] == 9 | (198 > ipByte[i + 3] & ipByte[i + 3] > 167)))
                        {
                            ucOutput += Convert.ToChar(1722).ToString();
                            // add space after ?  noon guna
                            ucOutput += Convert.ToChar(32).ToString();
                            i += 1;
                        }
                        else if (((ipByte[i + 1] == 177) & (ipByte[i + 3] == 177)))
                        {
                            //  remove 1 jazam 
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[Convert.ToInt32(ipByte[i + 1])]).ToString();
                            i += 3;
                        }
                        else if (ipByte[i + 1] == 166 & ucOutput[ucOutput.Length -1].ToString() == Convert.ToChar(1574).ToString())
                        {
                            //remove ya-hamza  before hamza
                            ucOutput = ucOutput.Remove(ucOutput.Length - 1, 1);
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[166]).ToString();
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[191]).ToString();
                            i += 1;
                        }
                        else if (ipByte[i + 1] == 253 | ipByte[i + 1] == 254)
                        {
                            //  Change " 
                            if (CheckboxChangeCCChecked == true)
                            {
                                if (ipByte[i + 1] == 253)
                                {
                                    ucOutput += Convert.ToChar(CharacterMap.cpi2u[254]).ToString();
                                    i += 1;
                                }
                                else
                                {
                                    ucOutput += Convert.ToChar(CharacterMap.cpi2u[253]).ToString();
                                    i += 1;
                                }
                            }
                            else
                            {
                                ucOutput += Convert.ToChar(CharacterMap.cpi2u[Convert.ToInt32(ipByte[i + 1])].ToString());
                                i += 1;
                            }
                        }
                        else if (ipByte[i + 1] == 224)
                        {
                            // ... double  
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[224]).ToString();
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[224]).ToString();
                            i += 1;

                        }
                        else if (ipByte[i + 1] == 184 & !(ipByte[i + 3] == 32 | ipByte[i + 2] == 13 | ipByte[i + 2] == 9 | (255 > ipByte[i + 3] & ipByte[i + 3] > 202) | (198 > ipByte[i + 3] & ipByte[i + 3] > 167)))
                        {
                            // add space after ?
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[184]).ToString();
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[32]).ToString();
                            i += 1;
                        }
                        else if (((ipByte[i + 1] == 162 | ipByte[i + 1] == 182) & ucOutput[ucOutput.Length - 1].ToString() == Convert.ToChar(1574).ToString()))
                        {
                            //remove ya-hamza befor wao-haza or wao
                            ucOutput = ucOutput.Remove(ucOutput.Length - 1, 1);
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[182]).ToString();
                            i += 1;
                        }
                        else if ((218 > ipByte[i + 1] & ipByte[i + 1] > 207) & ((218 > ipByte[i + 3] & ipByte[i + 3] > 207) | (ipByte[i + 3] == 223 | ipByte[i + 2] == 47)))
                        {
                            //ginti ;)                                                          

                            string temValue = "";
                            while (!(!((218 > ipByte[i + 1] & ipByte[i + 1] > 207) | (ipByte[i + 1] == 223 | ipByte[i] == 47))))
                            {
                                if (ipByte[i] == 47)
                                {
                                    temValue += Convert.ToChar(47).ToString();
                                }
                                else
                                {
                                    temValue += Convert.ToChar(CharacterMap.cpi2u[Convert.ToInt32(ipByte[i + 1])].ToString());
                                }

                                if (ipByte[i + 1] == 47 | ipByte[i] == 47)
                                {
                                    i += 1;
                                }
                                else
                                {
                                    i += 2;
                                }

                            }
                            if (CheckboxChangePChecked == false)
                            {
                                ucOutput += CharacterMap.ChangePositon(temValue);
                            }
                            else
                            {
                                ucOutput += temValue;
                            }
                            i -= 1;
                        }
                        else
                        {
                            ucOutput += Convert.ToChar(CharacterMap.cpi2u[Convert.ToInt32(ipByte[i + 1])].ToString());
                            i += 1;
                        }


                    }
                    else if (ipByte[i] == 32)
                    {
                        ucOutput += Convert.ToChar(32);
                    }
                    else if (ipByte[i] == 13)
                    {
                        ucOutput += Convert.ToChar(13);
                        ucOutput += Convert.ToChar(10);
                        i += 1;
                    }
                    else if (ipByte[i] == 9)
                    {
                        ucOutput += Convert.ToChar(9);

                    }
                    else if (64 > ipByte[i] & ipByte[i] > 32)
                    {

                        if ((58 > ipByte[i] & ipByte[i] > 47) & ipByte[i + 1] == 32)
                        {
                            bool boolChkEnter = false;
                            string my_tempVar = "";
                            // Or ipByte[i] = 47)
                            while (!(!((58 > ipByte[i] & ipByte[i] > 47) | ipByte[i] == 32)))
                            {
                                boolChkEnter = true;
                                my_tempVar += Convert.ToChar(ipByte[i]).ToString();
                                i += 1;
                            }

                            if (CheckboxChangePChecked == false)
                            {
                                ucOutput += CharacterMap.ChangePositon(my_tempVar);
                            }
                            else
                            {
                                ucOutput += my_tempVar;
                            }


                            if (boolChkEnter)
                            {
                                i -= 1;
                            }

                        }
                        else
                        {
                            ucOutput += Convert.ToChar(ipByte[i]).ToString();
                        }

                    }
                    else if (127 > ipByte[i] & ipByte[i] > 32)
                    {
                        ucOutput += Convert.ToChar(ipByte[i]).ToString();
                    }
                }

            }
            catch (Exception ex)
            {
            }
            ucOutput = Regex.Replace(ucOutput, "[ ]+[ ]", " ");
            Clipboard.SetText(ucOutput);

            //if (NotifyIcon1.Visible == true)
            //{
            //    NotifyIcon1.ShowBalloonTip(2000, "Pak Inpage to Unicode", "Converted into unicode formate", ToolTipIcon.Info);
            //}
            //else
            //{
            //    MessageBox.Show("Converted into unicode formate", MsgBoxStyle.Information);
            //}
            ucOutput = " ";
        }

        //protected override void WndProc(ref System.Windows.Forms.Message m)
        //{
        //    if (m.Msg == hotKey)
        //    {
        //        IntPtr id = m.WParam;
        //        if (id.ToString() == "200")
        //        {
        //            try
        //            {
        //                Inpage2Unicode();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //        }
        //        else if (id.ToString() == "100")
        //        {
        //            try
        //            {
        //                // Unicode2Inpage();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //        }
        //        else if (id.ToString() == "300")
        //        {
        //            try
        //            {
        //                //Unicode2Inpage();
        //                Inpage2Unicode();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message);
        //            }
        //        }
        //    }
        //    base.WndProc(m);
        //}

        private void InpageToUnicodeToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Inpage2Unicode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void UnicodeToInpageToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                //Unicode2Inpage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //private void frmInPageConverter_Resize(object sender, System.EventArgs e)
        //{
        //    if (this.WindowState == FormWindowState.Minimized)
        //    {
        //        this.Hide();
        //        NotifyIcon1.Visible = true;
        //    }

        //}

        //private void NotifyIcon1_DoubleClick(System.Object sender, System.EventArgs e)
        //{
        //    this.Show();
        //    NotifyIcon1.Visible = false;
        //}

        //private void ExitToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        //{
        //    Application.Exit();
        //}

        //private void ShowToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        //{
        //    this.Show();
        //    NotifyIcon1.Visible = false;
        //}

        //private void btnExit_Click(System.Object sender, System.EventArgs e)
        //{
        //    Application.Exit();
        //}

        //private void DisableToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        //{
        //    if (DisableToolStripMenuItem.Text == "Disable")
        //    {
        //        DisableToolStripMenuItem.Text = "Enable";
        //        UnregisterHotKey(this.Handle, 100);
        //        UnregisterHotKey(this.Handle, 200);
        //        UnregisterHotKey(this.Handle, 300);
        //    }
        //    else
        //    {
        //        DisableToolStripMenuItem.Text = "Disable";
        //        RegisterHotKey(this.Handle, 100, altKey + ctrlKey, Keys.I);
        //        RegisterHotKey(this.Handle, 200, altKey + ctrlKey, Keys.U);
        //        RegisterHotKey(this.Handle, 300, altKey + ctrlKey, Keys.A);
        //    }
        //}

        private void btnFConvert_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                WriteStatusMessage("Converting ... ");
                IsProgressBarVisible = true;
                bgw4_new.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void bgw4_new_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            FileInfo finfo = new FileInfo(sourceFName);
            long numBytes = finfo.Length;
            string myEnter = Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString();
            string myTab = Convert.ToChar(9).ToString();
            //   Dim Zair As String = "?"
            //Dim regReverseEngWSpace As String = "[0-9A-Za-z]+[ ][0-9A-Za-z ?]+[0-9A-Za-z]"
            string regReverseEngWSpace = "[0-9]+[ ][0-9 ?]+[0-9]";
            string regEnter = "-0D-[^-]+-[^-]+-[^-]+-[^-]+[^-]";
            string regRemoveAhrab = "[ ????????????????]";
            //  Dim regUDigits As String = "[??????????][/,+=%.÷()×-??????????"]+' & Convert.ToChar(1748) & "]+"
            string regUDigits = "[??????????][??????????/+×÷%,]+";
            // & "[]$"
            string regADigits = "[??????????][??????????/+×÷%,]+";

            // only digits
            string regOnlyUDigits = "[??????????][??????????]+";
            // & "[]$"
            string regOnlyADigits = "[??????????][??????????]+";

            // Dim regUDigitsWS As String = "([??????????]+)([\])([^??????????])"
            string regUrduAlfabat = "([??????????????????????????????????????????????????])";
            // Dim regUrduAlfabat As String = "([??????????????????????????????????????????????????])"
            // Dim regArabiAlfabat As String = "([?????????????????????????????????])" 
            string regAhrab = "([?????????????????])";
            // zair is inside 
            string regNoonGuna = "(?)" + regUrduAlfabat;
            FileStream fStream = new FileStream(sourceFName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            string regHamza = null;
            string regHamzaWAhrab = null;

            // If rbtnUrduChecked Then
            regHamza = "(?)" + regUrduAlfabat;
            regHamzaWAhrab = "(?)" + regAhrab + regUrduAlfabat;
            //ElseIf rbtnArabicChecked Then
            //regHamza = "(?)" & regArabiAlfabat
            //regHamzaWAhrab = "(?)" & regAhrab & regArabiAlfabat
            //End If


            binaryData = br.ReadBytes(Convert.ToInt32(numBytes));
            br.Close();
            fStream.Close();

            int startP = CharacterMap.FindStartPosition(binaryData);
            int endP = CharacterMap.FindEndPosition(binaryData, startP);
            int slength = endP - startP;


            bgw4_new.ReportProgress(10);

            // newOutputBox("start index = " & startP & " end index = " & endP & " length = " & endP - startP)
            // Dim newOutput As String = BitConverter.ToString()(my_binaryData)
            string newOutput = "-" + BitConverter.ToString(binaryData, startP, slength);
            //     newOutput = Replace(newOutput, "-", "")
            //  Dim sample As String = newOutput

            ///'''''''''''''''''''''''''
            // Dim test As String = "-0D-[^-]+-[^-]+-[^-]+-[^-]+[^-]"
            // Dim matches As MatchCollection = Regex.Matches(newOutput, test)?

            //   Regex.Replace(input,
            // Dim str1 As String = ""

            //        For Each mach As Match In matches
            //str1 += "match value = " & mach.Value
            //str += " match index = " & mach.Index & vbNewLine
            //Next

            //        Form1.RichTextBox1.Text = str
            //       Form1.Show()
            //      Exit Sub
            ///''''''''''''''''''''''''''
            newOutput = Regex.Replace(newOutput, regEnter, myEnter);
            newOutput = newOutput.Replace("-09",myTab);
            newOutput = newOutput.Replace("-04-AA","?");
            // ZEIR
            ///'''''''''''''''''''''''''''''''''''''''''''''''''''''

            newOutput = newOutput.Replace("-04-20"," ");
            newOutput = newOutput.Replace("-04-81-04-B3","?");
            newOutput = newOutput.Replace("-04-81-04-BF","?");
            newOutput = newOutput.Replace("-04-81","?");
            newOutput = newOutput.Replace("-04-82","?");
            newOutput = newOutput.Replace("-04-83","?");
            newOutput = newOutput.Replace("-04-84","?");
            newOutput = newOutput.Replace("-04-85","?");
            newOutput = newOutput.Replace("-04-86","?");
            newOutput = newOutput.Replace("-04-87","?");
            newOutput = newOutput.Replace("-04-88","?");
            newOutput = newOutput.Replace("-04-89","?");
            newOutput = newOutput.Replace("-04-8A","?");
            newOutput = newOutput.Replace("-04-8B","?");
            newOutput = newOutput.Replace("-04-8C","?");
            newOutput = newOutput.Replace("-04-8D","?");
            newOutput = newOutput.Replace("-04-8E","?");
            newOutput = newOutput.Replace("-04-8F","?");
            newOutput = newOutput.Replace("-04-90","?");
            newOutput = newOutput.Replace("-04-91","?");
            newOutput = newOutput.Replace("-04-92","?");
            newOutput = newOutput.Replace("-04-93","?");
            newOutput = newOutput.Replace("-04-94","?");
            newOutput = newOutput.Replace("-04-95","?");
            newOutput = newOutput.Replace("-04-96","?");
            newOutput = newOutput.Replace("-04-97","?");
            newOutput = newOutput.Replace("-04-98","?");
            newOutput = newOutput.Replace("-04-99","?");
            newOutput = newOutput.Replace("-04-9A","?");
            newOutput = newOutput.Replace("-04-9B","?");
            if (rbtnUrduChecked)
            {
                newOutput = newOutput.Replace("-04-9C","?");
            }
            else
            {
                newOutput = newOutput.Replace("-04-9C","?");
            }
            newOutput = newOutput.Replace("-04-9D","?");
            newOutput = newOutput.Replace("-04-9E","?");
            newOutput = newOutput.Replace("-04-9F","?");
            newOutput = newOutput.Replace("-04-A0","?");
            newOutput = newOutput.Replace("-04-A1","?");
            //  newOutput = Replace(newOutput, "-04-A3-04-A2", "?")
            if (CheckboxCorrectHehHamzaChecked)
            {
                newOutput = newOutput.Replace("-04-A3-04-A2","?");
                newOutput = newOutput.Replace("-04-BF-04-A2","?");
                // testing
            }
            else
            {
                newOutput = newOutput.Replace("-04-A3-04-A2","??");
            }

            newOutput = newOutput.Replace("-04-A2-04-BF","?");
            // urdu arabic
            if (rbtnUrduChecked)
            {
                // urdu
                if (CheckboxCorrectHehHamzaChecked)
                {
                    newOutput = newOutput.Replace("-04-BF-04-A6","?");
                    // testing
                    newOutput = newOutput.Replace("-04-A3-04-A6","?");
                    // testing
                }
                newOutput = newOutput.Replace("-04-A6-04-BF","?");
            }
            else
            {
                newOutput = newOutput.Replace("-04-A6-04-BF","?");
            }
            //  newOutput = Replace(newOutput, "-04-BF-04-A6", "?")  ' testing
            newOutput = newOutput.Replace("-04-A3-04-A6","??");
            newOutput = newOutput.Replace("-04-A2","?");
            newOutput = newOutput.Replace("-04-A3","?");
            //     newOutput = Replace(newOutput, "-44-A3", "?")  ' modified
            newOutput = newOutput.Replace("-04-A4-04-BF","?");
            if (rbtnUrduChecked)
            {
                newOutput = newOutput.Replace("-04-A4","?");
            }
            else
            {
                newOutput = newOutput.Replace("-04-A4","?");
            }
            newOutput = newOutput.Replace("-04-A5","?");
            // newOutput = Replace(newOutput, "-04-A6", "?")
            if (rbtnUrduChecked)
            {
                newOutput = newOutput.Replace("-04-A6","?");
                newOutput = newOutput.Replace("-04-A7","?");
            }
            else
            {
                newOutput = newOutput.Replace("-04-A6","?");
                newOutput = newOutput.Replace("-04-A7","?");
            }


            //'/////////////////////////////////////////////



            //'/////////////////////////////////////////////

            newOutput = newOutput.Replace("-04-A8","?");
            // 2 ZEIR NICHE  
            //remove kashida
            if (CheckboxRemoveKashidaChecked)
            {
                newOutput = newOutput.Replace("-04-A9","");
                // 169 TATWEEL
            }
            else
            {
                newOutput = newOutput.Replace("-04-A9","?");
                // 169 TATWEEL
            }
            newOutput = newOutput.Replace("-04-AA","?");
            // ZEIR
            newOutput = newOutput.Replace("-04-AB","?");
            // ZABAR
            newOutput = newOutput.Replace("-04-AC","?");
            // PAESH
            newOutput = newOutput.Replace("-04-AD","?");
            // 0651 UNICODE VALUE
            newOutput = newOutput.Replace("-04-AE","?");
            // ALAH ISLAM SH

            newOutput = newOutput.Replace("-04-B0","?");
            // KHARI ZEIR
            if (rbtnUrduChecked)
            {
                newOutput = newOutput.Replace("-04-B1-04-B1","?");
                // SAKIN
                newOutput = newOutput.Replace("-04-B1","?");
                // SAKIN
            }
            else
            {
                newOutput = newOutput.Replace("-04-B1-04-B1","?");
                // SAKIN
                newOutput = newOutput.Replace("-04-B1","?");
                // SAKIN
            }
            newOutput = newOutput.Replace("-04-B3","?");
            // MAD
            newOutput = newOutput.Replace("-04-B4","?");
            // SAKIN
            newOutput = newOutput.Replace("-04-B5","?");
            //  PAESH TYPE PHOOL
            newOutput = newOutput.Replace("-04-B6","?");
            newOutput = newOutput.Replace("-04-B7","?");
            newOutput = newOutput.Replace("-04-B8","?");
            if (rbtnUrduChecked)
            {
                newOutput = newOutput.Replace("-04-B9","?");
            }
            else
            {
                newOutput = newOutput.Replace("-04-B9","?");
            }
            newOutput = newOutput.Replace("-04-BD","?");
            // KHARI ZABAR
            newOutput = newOutput.Replace("-04-BE","?");
            // ULTA PAESH
            newOutput = newOutput.Replace("-04-BF","?");
            // HAMZA OOPER

            newOutput = newOutput.Replace("-04-C7","?");
            // 2 ZABAR OOPAR
            newOutput = newOutput.Replace("-04-C8","?");
            newOutput = newOutput.Replace("-04-C9","?");
            newOutput = newOutput.Replace("-04-CA","?");
            newOutput = newOutput.Replace("-04-CB","?");
            // ALLAH
            newOutput = newOutput.Replace("-04-CF","?");
            // 207

            if (rbtnUrduChecked)
            {
                newOutput = newOutput.Replace("-04-D0","?");
                newOutput = newOutput.Replace("-04-D1","?");
                newOutput = newOutput.Replace("-04-D2","?");
                newOutput = newOutput.Replace("-04-D3","?");
                newOutput = newOutput.Replace("-04-D4","?");
                newOutput = newOutput.Replace("-04-D5","?");
                newOutput = newOutput.Replace("-04-D6","?");
                newOutput = newOutput.Replace("-04-D7","?");
                newOutput = newOutput.Replace("-04-D8","?");
                newOutput = newOutput.Replace("-04-D9","?");
            }
            else
            {
                newOutput = newOutput.Replace("-04-D0","?");
                newOutput = newOutput.Replace("-04-D1","?");
                newOutput = newOutput.Replace("-04-D2","?");
                newOutput = newOutput.Replace("-04-D3","?");
                newOutput = newOutput.Replace("-04-D4","?");
                newOutput = newOutput.Replace("-04-D5","?");
                newOutput = newOutput.Replace("-04-D6","?");
                newOutput = newOutput.Replace("-04-D7","?");
                newOutput = newOutput.Replace("-04-D8","?");
                newOutput = newOutput.Replace("-04-D9","?");
            }
            newOutput = newOutput.Replace("-04-DA","!");
            //218
            newOutput = newOutput.Replace("-04-DB","?");
            // SP B
            newOutput = newOutput.Replace("-04-DC","?");
            newOutput = newOutput.Replace("-04-DE","%");
            newOutput = newOutput.Replace("-04-DF","/");

            newOutput = newOutput.Replace("-04-E0","……");
            // ... DBL
            newOutput = newOutput.Replace("-04-E1",")");
            //N B
            newOutput = newOutput.Replace("-04-E2","(");
            //N B 
            newOutput = newOutput.Replace("-04-E4","+");
            newOutput = newOutput.Replace("-04-E6","?");
            // RAZI ALLAH SH
            newOutput = newOutput.Replace("-04-E7","?");
            // RAHMATU ALLAH SH
            newOutput = newOutput.Replace("-04-E8","?");
            newOutput = newOutput.Replace("-04-E9",":");
            //233
            newOutput = newOutput.Replace("-04-EA","?");
            newOutput = newOutput.Replace("-04-EB","×");
            newOutput = newOutput.Replace("-04-EC","=");
            newOutput = newOutput.Replace("-04-ED","?");
            newOutput = newOutput.Replace("-04-EE","?");
            newOutput = newOutput.Replace("-04-EF","÷");

            newOutput = newOutput.Replace("-04-F1","?");
            //241
            newOutput = newOutput.Replace("-04-F2","?");
            if (rbtnUrduChecked)
            {
                newOutput = newOutput.Replace("-04-F3","?");
            }
            else
            {
                newOutput = newOutput.Replace("-04-F3",".");
            }
            newOutput = newOutput.Replace("-04-F5","-");
            //245  /
            newOutput = newOutput.Replace("-04-F6","?");
            // PBUH
            newOutput = newOutput.Replace("-04-F7","?");
            // 247
            newOutput = newOutput.Replace("-04-F8","?");
            // PBUH SHORT
            newOutput = newOutput.Replace("-04-F9",",");
            newOutput = newOutput.Replace("-04-FA","]");
            newOutput = newOutput.Replace("-04-FB","[");
            newOutput = newOutput.Replace("-04-FC",".");
            //.

            bgw4_new.ReportProgress(20);
            if (CheckboxReverseQuotMarksChecked)
            {
                newOutput = newOutput.Replace("-04-FE","’");
                //254
                newOutput = newOutput.Replace("-04-FD","‘");
                //253
            }
            else
            {
                newOutput = newOutput.Replace("-04-FD","’");
                //253
                newOutput = newOutput.Replace("-04-FE","‘");
                //254
            }
            newOutput = newOutput.Replace("-04-3A","");
            newOutput = newOutput.Replace("-04-3B","");
            newOutput = newOutput.Replace("-09",myTab);

            newOutput = newOutput.Replace("-20"," ");
            newOutput = newOutput.Replace("-21","!");
            newOutput = newOutput.Replace("-22",Convert.ToChar(34).ToString());
            newOutput = newOutput.Replace("-23","#");
            newOutput = newOutput.Replace("-24","$");
            newOutput = newOutput.Replace("-25","%");
            newOutput = newOutput.Replace("-26","&");
            newOutput = newOutput.Replace("-27","'");
            newOutput = newOutput.Replace("-28","(");
            newOutput = newOutput.Replace("-29",")");
            newOutput = newOutput.Replace("-2A","*");
            newOutput = newOutput.Replace("-2B","+");
            newOutput = newOutput.Replace("-2C",",");
            newOutput = newOutput.Replace("-2D","-");
            newOutput = newOutput.Replace("-2E",".");
            newOutput = newOutput.Replace("-2F","/");

            newOutput = newOutput.Replace("-3A",":");
            newOutput = newOutput.Replace("-3B",";");
            newOutput = newOutput.Replace("-3C","<");
            newOutput = newOutput.Replace("-3D","=");
            newOutput = newOutput.Replace("-3E",">");
            newOutput = newOutput.Replace("-3F","?");

            newOutput = newOutput.Replace("-40","@");
            newOutput = newOutput.Replace("-41","A");
            newOutput = newOutput.Replace("-42","B");
            newOutput = newOutput.Replace("-43","C");
            newOutput = newOutput.Replace("-44","D");
            newOutput = newOutput.Replace("-45","E");
            newOutput = newOutput.Replace("-46","F");
            newOutput = newOutput.Replace("-47","G");
            newOutput = newOutput.Replace("-48","H");
            newOutput = newOutput.Replace("-49","I");
            newOutput = newOutput.Replace("-4A","J");
            newOutput = newOutput.Replace("-4B","K");
            newOutput = newOutput.Replace("-4C","L");
            newOutput = newOutput.Replace("-4D","M");
            newOutput = newOutput.Replace("-4E","N");
            newOutput = newOutput.Replace("-4F","O");

            newOutput = newOutput.Replace("-50","P");
            newOutput = newOutput.Replace("-51","Q");
            newOutput = newOutput.Replace("-52","R");
            newOutput = newOutput.Replace("-53","S");
            newOutput = newOutput.Replace("-54","T");
            newOutput = newOutput.Replace("-55","U");
            newOutput = newOutput.Replace("-56","V");
            newOutput = newOutput.Replace("-57","W");
            newOutput = newOutput.Replace("-58","X");
            newOutput = newOutput.Replace("-59","Y");
            newOutput = newOutput.Replace("-5A","Z");
            newOutput = newOutput.Replace("-5B","[");
            newOutput = newOutput.Replace("-5C","\\");
            newOutput = newOutput.Replace("-5D","]");
            newOutput = newOutput.Replace("-5E","^");
            newOutput = newOutput.Replace("-5F","_");

            newOutput = newOutput.Replace("-60","`");
            newOutput = newOutput.Replace("-61","a");
            newOutput = newOutput.Replace("-62","b");
            newOutput = newOutput.Replace("-63","c");
            newOutput = newOutput.Replace("-64","d");
            newOutput = newOutput.Replace("-65","e");
            newOutput = newOutput.Replace("-66","f");
            newOutput = newOutput.Replace("-67","g");
            newOutput = newOutput.Replace("-68","h");
            newOutput = newOutput.Replace("-69","i");
            newOutput = newOutput.Replace("-6A","j");
            newOutput = newOutput.Replace("-6B","k");
            newOutput = newOutput.Replace("-6C","l");
            newOutput = newOutput.Replace("-6D","m");
            newOutput = newOutput.Replace("-6E","n");
            newOutput = newOutput.Replace("-6F","o");

            newOutput = newOutput.Replace("-70","p");
            newOutput = newOutput.Replace("-71","q");
            newOutput = newOutput.Replace("-72","r");
            newOutput = newOutput.Replace("-73","s");
            newOutput = newOutput.Replace("-74","t");
            newOutput = newOutput.Replace("-75","u");
            newOutput = newOutput.Replace("-76","v");
            newOutput = newOutput.Replace("-77","w");
            newOutput = newOutput.Replace("-78","x");
            newOutput = newOutput.Replace("-79","y");
            newOutput = newOutput.Replace("-7A","z");
            newOutput = newOutput.Replace("-7B","{");
            newOutput = newOutput.Replace("-7C","|");
            newOutput = newOutput.Replace("-7D","}");
            newOutput = newOutput.Replace("-7E","~");
            newOutput = newOutput.Replace("-7F","");

            newOutput = newOutput.Replace("-30","0");
            newOutput = newOutput.Replace("-31","1");
            newOutput = newOutput.Replace("-32","2");
            // newOutput = Replace(newOutput, "-33", "3")
            newOutput = newOutput.Replace("-34","4");
            newOutput = newOutput.Replace("-35","5");
            newOutput = newOutput.Replace("-36","6");
            newOutput = newOutput.Replace("-37","7");
            newOutput = newOutput.Replace("-38","8");
            newOutput = newOutput.Replace("-39","9");

            newOutput = newOutput.Replace("-33","3");
            // @ last

            bgw4_new.ReportProgress(30);

            //newOutput = Replace(newOutput, "-", "")
            //'===========================================================================
            // ElseIf my_binaryData[i + 1] = 166 And my_OutPut.Last = Convert.ToChar(1574).ToString() Then
            //'remove ya-hamza  before hamza
            //my_OutPut = my_OutPut.Remove(my_OutPut.Length - 1, 1)
            //my_OutPut += Convert.ToChar(CharacterMap.ip2uc[166]).ToString()
            //my_OutPut += Convert.ToChar(CharacterMap.ip2uc[191]).ToString()
            //i += 1


            // Dim searchIndex As Integer = 1


            if (CheckboxReverseNumbersDigitsChecked & CheckboxReverseSolidusSignChecked & CheckboxReverseThousSeparatorChecked)
            {
                MatchCollection digitMatches = default(MatchCollection);

                if (rbtnUrduChecked)
                {
                    digitMatches = Regex.Matches(newOutput, regUDigits);
                }
                else
                {
                    digitMatches = Regex.Matches(newOutput, regADigits);
                }

                foreach (Match mach in digitMatches)
                {
                    if (mach.Value[mach.Value.Length-1].ToString() == "/")
                    {
                        string machValue = mach.Value;
                        machValue = machValue.Remove(machValue.Length - 1, 1);
                        machValue = machValue.Reverse() + "/";
                        newOutput = newOutput.Remove(mach.Index, mach.Length);
                        newOutput = newOutput.Insert(mach.Index, machValue);
                    }
                    else
                    {
                        newOutput = newOutput.Remove(mach.Index, mach.Length);
                        newOutput = newOutput.Insert(mach.Index, mach.Value.Reverse().ToString());
                    }
                }
            }
            else
            {
                if (CheckboxReverseNumbersDigitsChecked & !CheckboxReverseThousSeparatorChecked)
                {
                    MatchCollection digitMatches = default(MatchCollection);

                    if (rbtnUrduChecked)
                    {
                        digitMatches = Regex.Matches(newOutput, regOnlyUDigits);
                    }
                    else
                    {
                        digitMatches = Regex.Matches(newOutput, regOnlyADigits);
                    }

                    foreach (Match mach in digitMatches)
                    {
                        newOutput = newOutput.Remove(mach.Index, mach.Length);
                        newOutput = newOutput.Insert(mach.Index, mach.Value.Reverse().ToString());
                    }
                }
            }
            newOutput = Regex.Replace(newOutput, "(/)(=)", "$2$1");
            bgw4_new.ReportProgress(50);
            newOutput = Regex.Replace(newOutput, regNoonGuna, "$1 $2");
            bgw4_new.ReportProgress(60);
            newOutput = Regex.Replace(newOutput, "(?)(?)", "?$2");
            newOutput = Regex.Replace(newOutput, regHamza, "?$2");
            bgw4_new.ReportProgress(66);
            //  newOutput = Regex.Replace(newOutput, regUDigitsWS, "2$1$3$")
            newOutput = Regex.Replace(newOutput, regHamzaWAhrab, "?$2$3");
            bgw4_new.ReportProgress(72);

            if (CheckboxCorrectBariYeeChecked)
            {
                newOutput = Regex.Replace(newOutput, "(?)" + regUrduAlfabat, "?$2");
            }

            bgw4_new.ReportProgress(80);
            if (rbtnUrduChecked)
            {
                newOutput = Regex.Replace(newOutput, "(?)" + regUrduAlfabat, "$1 $2");
            }
            bgw4_new.ReportProgress(90);

            //'////////////////////////////////////
            // optional setting
            // remove Double Space
            if (CheckboxRemoveDoubleSpaceChecked)
            {
                newOutput = Regex.Replace(newOutput, "[ ]+[ ]", " ");
            }

            if (CheckboxRemoveErabsChecked)
            {
                newOutput = Regex.Replace(newOutput, regRemoveAhrab, "");
            }
            if (CheckboxCorrectYearSignChecked)
            {
                newOutput = Regex.Replace(newOutput, "(?)(?)", "$2$1");
                newOutput = Regex.Replace(newOutput, "(?)(?)", "$2$1");
            }
            //    Dim RevDigitMatches As MatchCollection = Regex.Matches(newOutput, regReverseEngWSpace)
            //   For Each myMatch As Match In RevDigitMatches
            //newOutput = newOutput.Remove(myMatch.Index, myMatch.Length)
            //newOutput = newOutput.Insert(myMatch.Index, RevDig(myMatch.Value))
            //Next

            //'==========================================================================

            try
            {
                System.IO.File.WriteAllText(targetFName, newOutput, System.Text.Encoding.UTF8);
                bgw4_new.ReportProgress(100);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //newOutput = Regex.Replace(newOutput, "[ ]+[ ]", " ")
            //Try
            //    System.IO.File.WriteAllText(my_targetFName_Sp, newOutput, System.Text.Encoding.UTF8)
            //    bgw4_new.ReportProgress(100)
            //Catch ex As Exception
            //    MsgBox(ex.Message)
            //End Try
            // Form1.RichTextBox1.Text = sample
            //Form1.Show()
            //MsgBox(" Words count = " & slength / 2 & " Digits Reverse  = " & digitMatches.Count)
            //MessageBox.Show("Done");
        }

        private void bgw4_new_ProgressChanged(System.Object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ProgressBarValue = e.ProgressPercentage;
        }

        private void bgw4_new_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            WriteStatusMessage("File is successfully converted");
            ProgressBarValue = 0;
            IsProgressBarVisible = false;
            //ButtonOpenFileEnabled = true;
            //ButtonInPageFileEnabled = true;
        }

        private void chkReverseSSign_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (CheckboxReverseSolidusSignChecked)
            {
                CheckboxReverseThousSeparatorChecked = true;
            }
            else
            {
                CheckboxReverseThousSeparatorChecked = false;
            }
        }

        private void chkThousSeparator_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (CheckboxReverseThousSeparatorChecked)
            {
                CheckboxReverseSolidusSignChecked = true;
            }
            else
            {
                CheckboxReverseSolidusSignChecked = false;
            }
        }

        //public frmInPageConverter()
        //{
        //    Resize += frmInPageConverter_Resize;
        //    Load += frmInPageConverter_Load;
        //    FormClosing += frmInPageConverter_FormClosing;
        //}



    }
}


/*
         * private void Unicode2Inpage()
               {
                   string ucString = Clipboard.GetText();
                   ucString = Regex.Replace(ucString, "[ ]+[ ]", " ");
                   char[] ucChar = ucString + "   ";
                   int ucLength = ucString.Length;
                   Int32[] ucByte = new Int32[ucLength + 1];
                   string ipOutput = "";

                   for (i = 0; i <= ucLength; i += 1)
                   {
                       try
                       {
                           ucByte(i) = Convert.ToUInt32(ucChar(i));
                       }
                       catch (Exception ex)
                       {
                           MessageBox.Show(Convert.ToInt32(ucChar(i)).ToString() + ": i = " + i + " charcter =  " + ucChar(i) + "  " + ex.Message);
                       }
                   }

                   try
                   {
                       for (j = 0; j <= ucLength; j++)
                       {
                           if (ucByte(j) == 1574 & (ucByte(j + 1) == 32 | ucByte(j + 1) == 13 | ucByte(j + 1) == 9))
                           {
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[1574]).ToString();
                           }
                           else if (ucByte(j) == 1574)
                           {
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[1569]).ToString();
                           }
                           else if (ucByte(j) == 8204)
                           {
                               // "" nothing
                           }
                           else if (ucByte(j) == 10)
                           {
                               // "" nothing
                           }
                           else if (ucByte(j) == 1607 & (ucByte(j + 1) == 32 | ucByte(j + 1) == 13 | ucByte(j + 1) == 9))
                           {
                               ipOutput += Convert.ToChar(4);
                               //arbi ?
                               ipOutput += Convert.ToChar(cpu2i.Item[1749]).ToString();
                           }
                           else if (ucByte(j) == 13)
                           {
                               ipOutput += Convert.ToChar(13);
                               ipOutput += Convert.ToChar(10);
                           }
                           else if (ucByte(j) == 32)
                           {

                               if (j > 0)
                               {
                                   if (127 > ucByte(j - 1) & ucByte(j - 1) > 47)
                                   {
                                       ipOutput += Convert.ToChar(cpu2i.Item[32]).ToString();
                                   }
                                   else
                                   {
                                       ipOutput += Convert.ToChar(4);
                                       ipOutput += Convert.ToChar(cpu2i.Item[32]).ToString();
                                   }
                               }
                               else
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[32]).ToString();
                               }
                           }
                           else if (ucByte(j) == 8217 | ucByte(j) == 8216)
                           {
                               if (CheckboxChangeCCChecked == false)
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[Convert.ToInt32(ucByte(j)))].ToString();
                               }
                               else if (ucByte(j) == 8217 & CheckboxChangeCCChecked == true)
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[8216]).ToString();
                               }
                               else if (ucByte(j) == 8216 & CheckboxChangeCCChecked == true)
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[8217]).ToString();
                               }
                           }
                           else if (ucByte(j) == 1571)
                           {
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[1575]).ToString();
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[1620]).ToString();
                           }
                           else if (ucByte(j) == 1746 & !(ucByte(j + 1) == 32 | ucByte(j + 1) == 13 | ucByte(j + 1) == 9))
                           {
                               if (CheckboxBYieChecked)
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[1746]).ToString();
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[32]).ToString();
                               }
                               else
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[1746]).ToString();
                               }
                           }
                           else if (ucByte(j) == 8230 & ucByte(j + 1) == 8230)
                           {
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[8230]).ToString();
                               j += 1;
                           }
                           else if (ucByte(j) == 1728 | ucByte(j) == 1730)
                           {
                               // arbi value ?+?  ?
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[1569]).ToString();
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[1729]).ToString();
                           }
                           else if (ucByte(j) == 1747)
                           {
                               // arbi ?+?  ? 
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[1569]).ToString();
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[1746]).ToString();
                           }
                           else if (ucByte(j) == 1610)
                           {
                               if (ucByte(j + 1) == 32 | ucByte(j + 1) == 13)
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[1610]).ToString();
                               }
                               else
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[1740]).ToString();
                               }

                           }
                           else if (((ucByte(j) == 33 | ucByte(j) == 37 | ucByte(j) == 40 | ucByte(j) == 41 | ucByte(j) == 43 | ucByte(j) == 45 | ucByte(j) == 44 | ucByte(j) == 47 | ucByte(j) == 58 | ucByte(j) == 61 | ucByte(j) == 91 | ucByte(j) == 93) & !((91 > ucByte(j + 1) & ucByte(j + 1) > 64) | (123 > ucByte(j + 1) & ucByte(j + 1) > 96) | (58 > ucByte(j + 1) & ucByte(j + 1) > 47))))
                           {
                               ipOutput += Convert.ToChar(4);
                               ipOutput += Convert.ToChar(cpu2i.Item[Convert.ToInt32(ucByte(j)))].ToString();

                           }
                           else if (127 > ucByte(j) & ucByte(j) > 8)
                           {

                               if (58 > ucByte(j) & ucByte(j) > 47 & ((58 > ucByte(j + 1) & ucByte(j + 1) > 47) | (ucByte(j + 1) == 32 | ucByte(j + 1) == 47)))
                               {
                                   string temValue = "";
                                   try
                                   {
                                       while (!(!((58 > ucByte(j) & ucByte(j) > 46) | ucByte(j) == 32 | ucByte(j + 1) == 47)))
                                       {
                                           temValue += Convert.ToChar(ucByte(j));
                                           j += 1;
                                       }

                                   }
                                   catch (Exception ex)
                                   {
                                   }
                                   if (CheckboxChangePChecked == true)
                                   {
                                       ipOutput += CharacterMap.ChangePositon(temValue);
                                   }
                                   else
                                   {
                                       ipOutput += temValue;
                                   }
                                   j -= 1;

                               }
                               else
                               {
                                   while (!(!((127 > ucByte(j) & ucByte(j) > 31) | ucByte(j) == 9)))
                                   {
                                       ipOutput += Convert.ToChar(ucByte(j));
                                       j += 1;
                                   }
                                   j -= 1;
                               }
                           }
                           else if ((1786 > ucByte(j) & ucByte(j) > 1775) & ((1786 > ucByte(j + 1) & ucByte(j + 1) > 1775) | ucByte(j + 1) == 47))
                           {
                               //ginti ;)                                                          
                               Int16 k = 0;
                               //     Dim l As Int16
                               while (!(!((1786 > ucByte(j) & ucByte(j) > 1775) | ucByte(j) == 47)))
                               {
                                   k += 1;
                                   j += 1;
                               }


                               for (l = 1; l <= k; l += 1)
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[Convert.ToInt32(ucByte(j - l)))].ToString();
                               }

                               j -= 1;
                           }
                           else
                           {
                               if (cpu2i.Item[Convert.ToInt32(ucByte(j)))]
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(cpu2i.Item[Convert.ToInt32(ucByte(j)))].ToString();
                               }
                               else
                               {
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(251).ToString();
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(238).ToString();
                                   ipOutput += Convert.ToChar(4);
                                   ipOutput += Convert.ToChar(250).ToString();
                               }
                           }
                       }


                   }
                   catch (Exception ex)
                   {
                   }
                   Clipboard.SetText(ipOutput);
                   if (NotifyIcon1.Visible == true)
                   {
                       NotifyIcon1.ShowBalloonTip(2000, "Pak Inpage to Unicode", "Converted into inpage formate", ToolTipIcon.Info);
                   }
                   else
                   {
                       MessageBox.Show("Converted into inpage formate", MsgBoxStyle.Information);
                   }
                   ucString = " ";
               }
         */
