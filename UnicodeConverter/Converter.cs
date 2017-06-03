using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace UnicodeConverter
{
    /*
    public class Converter
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
        /////  hot key function /////////////////// 

        public const int altKey = 0x1;
        public const int ctrlKey = 0x2;
        public const int hotKey = 0x312;
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, int key);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int UnregisterHotKey(IntPtr hwnd, int id);


        // ////////////////////////////////////////////////////////

        private void btnOpenDlg_Click(System.Object sender, System.EventArgs e)
        {
            OFD.Filter = "Inpage files (*.inp)|*.inp|All files (*.*)|*.*";
            OFD.Title = "Open inpage file";

            if (OFD.ShowDialog == Windows.Forms.DialogResult.OK)
            {
                if (!(Path.GetExtension(OFD.FileName) == ".inp" | Path.GetExtension(OFD.FileName) == ".INP" | Path.GetExtension(OFD.FileName) == ".txt"))
                {
                    WriteStatusMessage("Error opening a file");
                    Interaction.MsgBox("Please select a inpage file");
                    txtSourceLocation.Text = "";
                    txtTatgetLocation.Text = "";
                    btnConvert.Enabled = false;
                    btnOpenFile.Enabled = false;
                    btnFConvert.Enabled = false;
                }
                else
                {
                    txtSourceLocation.Text = OFD.FileName;
                    sourceFName = OFD.FileName;
                    txtTatgetLocation.Text = Path.GetDirectoryName(sourceFName) + "\\" + Path.GetFileNameWithoutExtension(sourceFName) + "_convert.txt";
                    targetFName = txtTatgetLocation.Text;
                    targetFName_Sp = Path.GetDirectoryName(sourceFName) + "\\" + Path.GetFileNameWithoutExtension(sourceFName) + "_with_out_spaces.txt";
                    WriteStatusMessage("Ready to convert");
                    btnConvert.Enabled = true;
                    btnFConvert.Enabled = true;
                    btnInPageFile.Enabled = false;
                    btnOpenFile.Enabled = false;
                }
            }
            else
            {
                WriteStatusMessage("Ready");
            }
        }

        private void btnConvert_Click(System.Object sender, System.EventArgs e)
        {
            if (btnConvert.Text == "Cancel")
            {
                DialogResult msgResult = default(DialogResult);
                msgResult = Interaction.MsgBox("Do you want cancel the converting?", MsgBoxStyle.YesNo, "Cancel ?");
                if (msgResult == Windows.Forms.DialogResult.Yes)
                {
                    cancel_test = true;
                    btnConvert.Text = "Convert";
                }
            }
            else
            {
                try
                {
                    FileInfo finfo = new FileInfo(sourceFName);
                    long numBytes = finfo.Length;
                    FileStream fStream = new FileStream(sourceFName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fStream);

                    binaryData = br.ReadBytes(Convert.ToInt32(numBytes));
                    br.Close();
                    fStream.Close();
                    WriteStatusMessage("Converting ... ");
                    pb.Visible = true;
                    btnConvert.Text = "Cancel";
                    bgw.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox(ex.Message);
                }
            }
        }

        private void bgw_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int p_prog = binaryData.Length / 100;
            int startP = findStartPosition(binaryData);
            int endP = findEndPosition(binaryData, startP);


            for (i = startP; i <= endP; i++)
            {
                if ((binaryData(i) == 4))
                {
                    // find hamza position

                    if (binaryData(i + 1) == 163 & FindHamzaPosition(i))
                    {
                        outPut += Convert.ToChar(1574).ToString;
                        // 1574 ? ya   shift + 4
                        i += 1;
                    }
                    else if (binaryData(i + 1) == 165 & ((168 > binaryData(i + 3) & binaryData(i + 3) > 128) | binaryData(i + 3) == 170 | binaryData(i + 3) == 182 | binaryData(i + 3) == 184 | binaryData(i + 3) == 185 | binaryData(i + 3) == 200 | binaryData(i + 3) == 201))
                    {
                        if (chkBYie.Checked == true)
                        {
                            outPut += Convert.ToChar(1740).ToString;
                            //bari-ya ? convert to  ? ?  
                            i += 1;
                        }
                        else
                        {
                            outPut += Convert.ToChar(1746).ToString;
                            i += 1;
                            if (binaryData(i + 3) == 32)
                            {
                                i += 2;
                            }
                        }
                    }
                    else if (binaryData(i + 1) == 161 & !(binaryData(i + 3) == 32 | binaryData(i + 2) == 13 | (255 > binaryData(i + 3) & binaryData(i + 3) > 202) | binaryData(i + 2) == 9 | FindHamzaPosition(i + 3) == false | (198 > binaryData(i + 3) & binaryData(i + 3) > 167)))
                    {
                        outPut += Convert.ToChar(1722).ToString;
                        // add space after ?  noon guna
                        outPut += Convert.ToChar(32).ToString;
                        i += 1;
                    }
                    else if (((binaryData(i + 1) == 177) & (binaryData(i + 3) == 177)))
                    {
                        //  remove 1 jazam 
                        outPut += Convert.ToChar(ip2uc.Item(Convert.ToInt32(binaryData(i + 1)))).ToString;
                        i += 3;
                    }
                    else if (binaryData(i + 1) == 169 & !(binaryData(i + 3) == 169))
                    {
                        // //  extra character  " tatbeeq "  pass this character 
                        if (chkKashida.Checked)
                        {
                            outPut += Convert.ToChar(ip2uc.Item(Convert.ToInt32(binaryData(i + 1)))).ToString;
                            i += 1;
                        }
                        else
                        {
                            i += 1;
                        }
                    }
                    else if (binaryData(i + 1) == 166 & outPut.Last == Convert.ToChar(1574).ToString)
                    {
                        //remove ya-hamza  before hamza
                        outPut = outPut.Remove(outPut.Length - 1, 1);
                        outPut += Convert.ToChar(ip2uc.Item(166)).ToString;
                        outPut += Convert.ToChar(ip2uc.Item(191)).ToString;
                        i += 1;
                    }
                    else if (binaryData(i + 1) == 253 | binaryData(i + 1) == 254)
                    {
                        //  Change " 
                        if (chkChangeCC.Checked == true)
                        {
                            if (binaryData(i + 1) == 253)
                            {
                                outPut += Convert.ToChar(ip2uc.Item(254)).ToString;
                                i += 1;
                            }
                            else
                            {
                                outPut += Convert.ToChar(ip2uc.Item(253)).ToString;
                                i += 1;
                            }
                        }
                        else
                        {
                            outPut += Convert.ToChar(ip2uc.Item(Convert.ToInt32(binaryData(i + 1)))).ToString;
                            i += 1;
                        }

                    }
                    else if (binaryData(i + 1) == 224)
                    {
                        // ... double  
                        outPut += Convert.ToChar(ip2uc.Item(224)).ToString;
                        outPut += Convert.ToChar(ip2uc.Item(224)).ToString;
                        i += 1;
                    }
                    else if (binaryData(i + 1) == 184 & !(binaryData(i + 3) == 32 | binaryData(i + 2) == 13 | binaryData(i + 2) == 9 | (255 > binaryData(i + 3) & binaryData(i + 3) > 202) | (198 > binaryData(i + 3) & binaryData(i + 3) > 167)))
                    {
                        // add space after ?
                        outPut += Convert.ToChar(ip2uc.Item(184)).ToString;
                        outPut += Convert.ToChar(ip2uc.Item(32)).ToString;
                        i += 1;
                    }
                    else if (((binaryData(i + 1) == 162 | binaryData(i + 1) == 182) & outPut.Last == Convert.ToChar(1574).ToString))
                    {
                        //remove ya-hamza befor wao-haza or wao
                        outPut = outPut.Remove(outPut.Length - 1, 1);
                        outPut += Convert.ToChar(ip2uc.Item(182)).ToString;
                        i += 1;
                    }
                    else if ((218 > binaryData(i + 1) & binaryData(i + 1) > 207) & ((218 > binaryData(i + 3) & binaryData(i + 3) > 207) | (binaryData(i + 3) == 223 | binaryData(i + 2) == 47)))
                    {
                        //ginti ;)                                                          

                        string temValue = "";
                        while (!(!((218 > binaryData(i + 1) & binaryData(i + 1) > 207) | (binaryData(i + 1) == 223 | binaryData(i) == 47))))
                        {
                            if (binaryData(i) == 47)
                            {
                                temValue += Convert.ToChar(47).ToString;
                            }
                            else
                            {
                                temValue += Convert.ToChar(ip2uc.Item(Convert.ToInt32(binaryData(i + 1)))).ToString;
                            }

                            if (binaryData(i + 1) == 47 | binaryData(i) == 47)
                            {
                                i += 1;
                            }
                            else
                            {
                                i += 2;
                            }

                        }
                        if (chkChangeP.Checked == false)
                        {
                            outPut += ChangePositon(temValue);
                        }
                        else
                        {
                            outPut += temValue;
                        }
                        i -= 1;
                    }
                    else
                    {
                        outPut += Convert.ToChar(ip2uc.Item(Convert.ToInt32(binaryData(i + 1)))).ToString;
                        i += 1;
                    }


                }
                else if (binaryData(i) == 32)
                {
                    outPut += Convert.ToChar(32);
                }
                else if (binaryData(i) == 13)
                {
                    outPut += Convert.ToChar(13);
                    outPut += Convert.ToChar(10);
                    i += 3;
                }
                else if (binaryData(i) == 9)
                {
                    outPut += Convert.ToChar(9);

                }
                else if (64 > binaryData(i) & binaryData(i) > 32)
                {

                    if ((58 > binaryData(i) & binaryData(i) > 47) & binaryData(i + 1) == 32)
                    {
                        bool boolChkEnter = false;
                        string my_tempVar = "";
                        // Or my_binaryData(i) = 47)
                        while (!(!((58 > binaryData(i) & binaryData(i) > 47) | binaryData(i) == 32)))
                        {
                            boolChkEnter = true;
                            my_tempVar += Convert.ToChar(binaryData(i)).ToString;
                            i += 1;
                        }
                        if (chkChangeP.Checked == false)
                        {
                            outPut += ChangePositon(my_tempVar);
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
                        outPut += Convert.ToChar(binaryData(i)).ToString;
                    }

                }
                else if (256 > binaryData(i) & binaryData(i) > 32)
                {
                    outPut += Convert.ToChar(binaryData(i)).ToString;
                }
                if (cancel_test)
                {
                    break; // TODO: might not be correct. Was : Exit For
                }

                bgw.ReportProgress(Convert.ToInt32(i / p_prog));
            }
            if (cancel_test == false)
            {
                try
                {
                    System.IO.File.WriteAllText(targetFName, outPut, System.Text.Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Interaction.MsgBox(ex.Message);
                }

                Remove_Spaces();

                Interaction.MsgBox("Done");
                outPut = " ";
                outPut_Sp = "";
                start_test = false;
            }

        }

        private void bgw_ProgressChanged(System.Object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            pb.Value = e.ProgressPercentage;
        }

        private void bgw_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (cancel_test)
            {
                WriteStatusMessage("Process is canceled by the user");
                pb.Value = 0;
                pb.Visible = false;
                outPut = " ";
                outPut_Sp = "";
                start_test = false;
                //end_test = True
                cancel_test = false;
            }
            else
            {
                WriteStatusMessage("File is successfully converted");
                pb.Value = 0;
                pb.Visible = false;
                btnOpenFile.Enabled = true;
                btnInPageFile.Enabled = true;
                btnConvert.Text = "Convert";
            }
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

            for (i = 0; i <= removeSpaces.Length - 1; i++)
            {
                outPut_Sp = Strings.Replace(outPut_Sp, removeSpaces(i) + " ", removeSpaces(i));
            }
            outPut = Regex.Replace(outPut, "[ ]+[ ]", " ");
            try
            {
                System.IO.File.WriteAllText(targetFName_Sp, outPut_Sp, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }

        }

        private void frmInPageConverter_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            NotifyIcon1.Dispose();
            UnregisterHotKey(this.Handle, 100);
            UnregisterHotKey(this.Handle, 200);
            UnregisterHotKey(this.Handle, 300);

        }

        private void frmInPageConverter_Load(System.Object sender, System.EventArgs e)
        {

            try
            {

                Rectangle WArea = SystemInformation.WorkingArea;
                int x = (WArea.Width - this.Width) / 2;
                this.Location = new Point(x, 200);
                RegisterHotKey(this.Handle, 100, altKey + ctrlKey, Keys.I);
                RegisterHotKey(this.Handle, 200, altKey + ctrlKey, Keys.U);
                RegisterHotKey(this.Handle, 300, altKey + ctrlKey, Keys.A);
                initIp2ucCharacter();
                init_cpinpage2unicode();
                init_cpunicode2inpage();
                NotifyIcon1.Visible = false;
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }

        }

        private void btnOpenFile_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Process.Start(targetFName);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + " : " + targetFName);
            }

        }

        private void btnRemoveSpacesFile_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Process.Start(targetFName_Sp);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + " : " + targetFName);
            }
        }

        private void btnInPageFile_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Process.Start(sourceFName);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message + " : " + targetFName);
            }
        }

        private void Inpage2Unicode()
        {
            string ipString = Clipboard.GetText() + "   ";
            char[] ipChar = ipString;
            int ipLength = ipString.Length - 1;
            Int32[] ipByte = new Int32[ipLength + 1];
            string ucOutput = " ";

            for (i = 0; i <= ipLength; i += 1)
            {
                ipByte(i) = Convert.ToInt32(ipChar(i));
            }

            try
            {

                for (i = 0; i <= ipByte.Length - 1; i++)
                {
                    if ((ipByte(i) == 4))
                    {
                        // find hamza position

                        if (ipByte(i + 1) == 163 & FindHamzaPositionI2U(ipByte, i))
                        {
                            ucOutput += Convert.ToChar(1574).ToString;
                            // 1574 ? ya   shift + 4
                            i += 1;
                        }
                        else if (ipByte(i + 1) == 165 & ((167 > ipByte(i + 3) & ipByte(i + 3) > 128) | (750 > ipByte(i + 3) & ipByte(i + 3) > 338) | (8485 > ipByte(i + 3) & ipByte(i + 3) > 8210) | ipByte(i + 3) == 170 | ipByte(i + 3) == 182 | ipByte(i + 3) == 184 | ipByte(i + 3) == 185 | ipByte(i + 3) == 200 | ipByte(i + 3) == 201))
                        {
                            if (chkBYie.Checked == true)
                            {
                                ucOutput += Convert.ToChar(1740).ToString;
                                //bari-ya ? convert to  ? ?  
                                i += 1;
                            }
                            else
                            {
                                ucOutput += Convert.ToChar(1746).ToString;
                                i += 1;
                                if (ipByte(i + 3) == 32)
                                {
                                    i += 2;
                                }
                            }
                        }
                        else if (ipByte(i + 1) == 161 & !(ipByte(i + 3) == 32 | ipByte(i + 2) == 13 | (255 > ipByte(i + 3) & ipByte(i + 3) > 202) | ipByte(i + 2) == 9 | (198 > ipByte(i + 3) & ipByte(i + 3) > 167)))
                        {
                            ucOutput += Convert.ToChar(1722).ToString;
                            // add space after ?  noon guna
                            ucOutput += Convert.ToChar(32).ToString;
                            i += 1;
                        }
                        else if (((ipByte(i + 1) == 177) & (ipByte(i + 3) == 177)))
                        {
                            //  remove 1 jazam 
                            ucOutput += Convert.ToChar(cpi2u.Item(Convert.ToInt32(ipByte(i + 1)))).ToString;
                            i += 3;
                        }
                        else if (ipByte(i + 1) == 166 & ucOutput.Last == Convert.ToChar(1574).ToString)
                        {
                            //remove ya-hamza  before hamza
                            ucOutput = ucOutput.Remove(ucOutput.Length - 1, 1);
                            ucOutput += Convert.ToChar(cpi2u.Item(166)).ToString;
                            ucOutput += Convert.ToChar(cpi2u.Item(191)).ToString;
                            i += 1;
                        }
                        else if (ipByte(i + 1) == 253 | ipByte(i + 1) == 254)
                        {
                            //  Change " 
                            if (chkChangeCC.Checked == true)
                            {
                                if (ipByte(i + 1) == 253)
                                {
                                    ucOutput += Convert.ToChar(cpi2u.Item(254)).ToString;
                                    i += 1;
                                }
                                else
                                {
                                    ucOutput += Convert.ToChar(cpi2u.Item(253)).ToString;
                                    i += 1;
                                }
                            }
                            else
                            {
                                ucOutput += Convert.ToChar(cpi2u.Item(Convert.ToInt32(ipByte(i + 1)))).ToString;
                                i += 1;
                            }
                        }
                        else if (ipByte(i + 1) == 224)
                        {
                            // ... double  
                            ucOutput += Convert.ToChar(cpi2u.Item(224)).ToString;
                            ucOutput += Convert.ToChar(cpi2u.Item(224)).ToString;
                            i += 1;

                        }
                        else if (ipByte(i + 1) == 184 & !(ipByte(i + 3) == 32 | ipByte(i + 2) == 13 | ipByte(i + 2) == 9 | (255 > ipByte(i + 3) & ipByte(i + 3) > 202) | (198 > ipByte(i + 3) & ipByte(i + 3) > 167)))
                        {
                            // add space after ?
                            ucOutput += Convert.ToChar(cpi2u.Item(184)).ToString;
                            ucOutput += Convert.ToChar(cpi2u.Item(32)).ToString;
                            i += 1;
                        }
                        else if (((ipByte(i + 1) == 162 | ipByte(i + 1) == 182) & ucOutput.Last == Convert.ToChar(1574).ToString))
                        {
                            //remove ya-hamza befor wao-haza or wao
                            ucOutput = ucOutput.Remove(ucOutput.Length - 1, 1);
                            ucOutput += Convert.ToChar(cpi2u.Item(182)).ToString;
                            i += 1;
                        }
                        else if ((218 > ipByte(i + 1) & ipByte(i + 1) > 207) & ((218 > ipByte(i + 3) & ipByte(i + 3) > 207) | (ipByte(i + 3) == 223 | ipByte(i + 2) == 47)))
                        {
                            //ginti ;)                                                          

                            string temValue = "";
                            while (!(!((218 > ipByte(i + 1) & ipByte(i + 1) > 207) | (ipByte(i + 1) == 223 | ipByte(i) == 47))))
                            {
                                if (ipByte(i) == 47)
                                {
                                    temValue += Convert.ToChar(47).ToString;
                                }
                                else
                                {
                                    temValue += Convert.ToChar(cpi2u.Item(Convert.ToInt32(ipByte(i + 1)))).ToString;
                                }

                                if (ipByte(i + 1) == 47 | ipByte(i) == 47)
                                {
                                    i += 1;
                                }
                                else
                                {
                                    i += 2;
                                }

                            }
                            if (chkChangeP.Checked == false)
                            {
                                ucOutput += ChangePositon(temValue);
                            }
                            else
                            {
                                ucOutput += temValue;
                            }
                            i -= 1;
                        }
                        else
                        {
                            ucOutput += Convert.ToChar(cpi2u.Item(Convert.ToInt32(ipByte(i + 1)))).ToString;
                            i += 1;
                        }


                    }
                    else if (ipByte(i) == 32)
                    {
                        ucOutput += Convert.ToChar(32);
                    }
                    else if (ipByte(i) == 13)
                    {
                        ucOutput += Convert.ToChar(13);
                        ucOutput += Convert.ToChar(10);
                        i += 1;
                    }
                    else if (ipByte(i) == 9)
                    {
                        ucOutput += Convert.ToChar(9);

                    }
                    else if (64 > ipByte(i) & ipByte(i) > 32)
                    {

                        if ((58 > ipByte(i) & ipByte(i) > 47) & ipByte(i + 1) == 32)
                        {
                            bool boolChkEnter = false;
                            string my_tempVar = "";
                            // Or ipByte(i) = 47)
                            while (!(!((58 > ipByte(i) & ipByte(i) > 47) | ipByte(i) == 32)))
                            {
                                boolChkEnter = true;
                                my_tempVar += Convert.ToChar(ipByte(i)).ToString;
                                i += 1;
                            }

                            if (chkChangeP.Checked == false)
                            {
                                ucOutput += ChangePositon(my_tempVar);
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
                            ucOutput += Convert.ToChar(ipByte(i)).ToString;
                        }

                    }
                    else if (127 > ipByte(i) & ipByte(i) > 32)
                    {
                        ucOutput += Convert.ToChar(ipByte(i)).ToString;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            ucOutput = Regex.Replace(ucOutput, "[ ]+[ ]", " ");
            Clipboard.SetText(ucOutput);

            if (NotifyIcon1.Visible == true)
            {
                NotifyIcon1.ShowBalloonTip(2000, "Pak Inpage to Unicode", "Converted into unicode formate", ToolTipIcon.Info);
            }
            else
            {
                Interaction.MsgBox("Converted into unicode formate", MsgBoxStyle.Information);
            }
            ucOutput = " ";
        }

        private void Unicode2Inpage()
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
                    Interaction.MsgBox(Convert.ToInt32(ucChar(i)).ToString + ": i = " + i + " charcter =  " + ucChar(i) + "  " + ex.Message);
                }
            }

            try
            {
                for (j = 0; j <= ucLength; j++)
                {
                    if (ucByte(j) == 1574 & (ucByte(j + 1) == 32 | ucByte(j + 1) == 13 | ucByte(j + 1) == 9))
                    {
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(1574)).ToString;
                    }
                    else if (ucByte(j) == 1574)
                    {
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(1569)).ToString;
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
                        ipOutput += Convert.ToChar(cpu2i.Item(1749)).ToString;
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
                                ipOutput += Convert.ToChar(cpu2i.Item(32)).ToString;
                            }
                            else
                            {
                                ipOutput += Convert.ToChar(4);
                                ipOutput += Convert.ToChar(cpu2i.Item(32)).ToString;
                            }
                        }
                        else
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(32)).ToString;
                        }
                    }
                    else if (ucByte(j) == 8217 | ucByte(j) == 8216)
                    {
                        if (chkChangeCC.Checked == false)
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(Convert.ToInt32(ucByte(j)))).ToString;
                        }
                        else if (ucByte(j) == 8217 & chkChangeCC.Checked == true)
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(8216)).ToString;
                        }
                        else if (ucByte(j) == 8216 & chkChangeCC.Checked == true)
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(8217)).ToString;
                        }
                    }
                    else if (ucByte(j) == 1571)
                    {
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(1575)).ToString;
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(1620)).ToString;
                    }
                    else if (ucByte(j) == 1746 & !(ucByte(j + 1) == 32 | ucByte(j + 1) == 13 | ucByte(j + 1) == 9))
                    {
                        if (chkBYie.Checked)
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(1746)).ToString;
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(32)).ToString;
                        }
                        else
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(1746)).ToString;
                        }
                    }
                    else if (ucByte(j) == 8230 & ucByte(j + 1) == 8230)
                    {
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(8230)).ToString;
                        j += 1;
                    }
                    else if (ucByte(j) == 1728 | ucByte(j) == 1730)
                    {
                        // arbi value ?+?  ?
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(1569)).ToString;
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(1729)).ToString;
                    }
                    else if (ucByte(j) == 1747)
                    {
                        // arbi ?+?  ? 
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(1569)).ToString;
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(1746)).ToString;
                    }
                    else if (ucByte(j) == 1610)
                    {
                        if (ucByte(j + 1) == 32 | ucByte(j + 1) == 13)
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(1610)).ToString;
                        }
                        else
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(1740)).ToString;
                        }

                    }
                    else if (((ucByte(j) == 33 | ucByte(j) == 37 | ucByte(j) == 40 | ucByte(j) == 41 | ucByte(j) == 43 | ucByte(j) == 45 | ucByte(j) == 44 | ucByte(j) == 47 | ucByte(j) == 58 | ucByte(j) == 61 | ucByte(j) == 91 | ucByte(j) == 93) & !((91 > ucByte(j + 1) & ucByte(j + 1) > 64) | (123 > ucByte(j + 1) & ucByte(j + 1) > 96) | (58 > ucByte(j + 1) & ucByte(j + 1) > 47))))
                    {
                        ipOutput += Convert.ToChar(4);
                        ipOutput += Convert.ToChar(cpu2i.Item(Convert.ToInt32(ucByte(j)))).ToString;

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
                            if (chkChangeP.Checked == true)
                            {
                                ipOutput += ChangePositon(temValue);
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
                            ipOutput += Convert.ToChar(cpu2i.Item(Convert.ToInt32(ucByte(j - l)))).ToString;
                        }

                        j -= 1;
                    }
                    else
                    {
                        if (cpu2i.Item(Convert.ToInt32(ucByte(j))))
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(cpu2i.Item(Convert.ToInt32(ucByte(j)))).ToString;
                        }
                        else
                        {
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(251).ToString;
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(238).ToString;
                            ipOutput += Convert.ToChar(4);
                            ipOutput += Convert.ToChar(250).ToString;
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
                Interaction.MsgBox("Converted into inpage formate", MsgBoxStyle.Information);
            }
            ucString = " ";
        }

        private bool FindHamzaPosition(Int32 i)
        {
            bool functionReturnValue = false;

            if (binaryData(i + 3) != 32 & !(binaryData(i + 2) == 13))
            {

                if ((binaryData(i + 3) == 170 | binaryData(i + 3) == 171 | binaryData(i + 3) == 172 | binaryData(i + 3) == 176 | binaryData(i + 3) == 177 | binaryData(i + 3) == 168 | binaryData(i + 3) == 181 | binaryData(i + 3) == 173 | binaryData(i + 3) == 180 | binaryData(i + 3) == 179 | binaryData(i + 3) == 169))
                {

                    if (!(binaryData(i + 4) == 13 | binaryData(i + 5) == 32))
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
                else if ((186 < binaryData(i + 3) & binaryData(i + 3) < 255))
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

                if (ipByte(i + 3) != 32 & !(ipByte(i + 2) == 13))
                {

                    if ((ipByte(i + 3) == 170 | ipByte(i + 3) == 171 | ipByte(i + 3) == 172 | ipByte(i + 3) == 176 | ipByte(i + 3) == 177 | ipByte(i + 3) == 168 | ipByte(i + 3) == 181 | ipByte(i + 3) == 173 | ipByte(i + 3) == 180 | ipByte(i + 3) == 179 | ipByte(i + 3) == 169))
                    {

                        if (!(ipByte(i + 4) == 13 | ipByte(i + 5) == 32))
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
                    else if ((186 < ipByte(i + 3) & ipByte(i + 3) < 255))
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

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == hotKey)
            {
                IntPtr id = m.WParam;
                if (id.ToString == "200")
                {
                    try
                    {
                        Inpage2Unicode();
                    }
                    catch (Exception ex)
                    {
                        Interaction.MsgBox(ex.Message);
                    }
                }
                else if (id.ToString == "100")
                {
                    try
                    {
                        Unicode2Inpage();
                    }
                    catch (Exception ex)
                    {
                        Interaction.MsgBox(ex.Message);
                    }
                }
                else if (id.ToString == "300")
                {
                    try
                    {
                        Unicode2Inpage();
                        Inpage2Unicode();
                    }
                    catch (Exception ex)
                    {
                        Interaction.MsgBox(ex.Message);
                    }
                }
            }
            base.WndProc(m);
        }

        private void InpageToUnicodeToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Inpage2Unicode();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }

        }

        private void UnicodeToInpageToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                Unicode2Inpage();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }

        }

        private void frmInPageConverter_Resize(object sender, System.EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                NotifyIcon1.Visible = true;
            }

        }

        private void NotifyIcon1_DoubleClick(System.Object sender, System.EventArgs e)
        {
            this.Show();
            NotifyIcon1.Visible = false;
        }

        private void ExitToolStripMenuItem1_Click(System.Object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void ShowToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            this.Show();
            NotifyIcon1.Visible = false;
        }

        private void btnExit_Click(System.Object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        private void DisableToolStripMenuItem_Click(System.Object sender, System.EventArgs e)
        {
            if (DisableToolStripMenuItem.Text == "Disable")
            {
                DisableToolStripMenuItem.Text = "Enable";
                UnregisterHotKey(this.Handle, 100);
                UnregisterHotKey(this.Handle, 200);
                UnregisterHotKey(this.Handle, 300);
            }
            else
            {
                DisableToolStripMenuItem.Text = "Disable";
                RegisterHotKey(this.Handle, 100, altKey + ctrlKey, Keys.I);
                RegisterHotKey(this.Handle, 200, altKey + ctrlKey, Keys.U);
                RegisterHotKey(this.Handle, 300, altKey + ctrlKey, Keys.A);
            }
        }

        private void btnFConvert_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                WriteStatusMessage("Converting ... ");
                pb.Visible = true;
                bgw4_new.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message);
            }

        }

        private void bgw4_new_DoWork(System.Object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            FileInfo finfo = new FileInfo(sourceFName);
            long numBytes = finfo.Length;
            string myEnter = Convert.ToChar(13).ToString + Convert.ToChar(10).ToString;
            string myTab = Convert.ToChar(9).ToString;
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

            // If rbtnUrdu.Checked Then
            regHamza = "(?)" + regUrduAlfabat;
            regHamzaWAhrab = "(?)" + regAhrab + regUrduAlfabat;
            //ElseIf rbtnArabic.Checked Then
            //regHamza = "(?)" & regArabiAlfabat
            //regHamzaWAhrab = "(?)" & regAhrab & regArabiAlfabat
            //End If


            binaryData = br.ReadBytes(Convert.ToInt32(numBytes));
            br.Close();
            fStream.Close();

            int startP = findStartPosition(binaryData);
            int endP = findEndPosition(binaryData, startP);
            int slength = endP - startP;


            bgw4_new.ReportProgress(10);

            // newOutputBox("start index = " & startP & " end index = " & endP & " length = " & endP - startP)
            // Dim newOutput As String = BitConverter.ToString(my_binaryData)
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
            newOutput = Strings.Replace(newOutput, "-09", myTab);
            newOutput = Strings.Replace(newOutput, "-04-AA", "?");
            // ZEIR
            ///'''''''''''''''''''''''''''''''''''''''''''''''''''''

            newOutput = Strings.Replace(newOutput, "-04-20", " ");
            newOutput = Strings.Replace(newOutput, "-04-81-04-B3", "?");
            newOutput = Strings.Replace(newOutput, "-04-81-04-BF", "?");
            newOutput = Strings.Replace(newOutput, "-04-81", "?");
            newOutput = Strings.Replace(newOutput, "-04-82", "?");
            newOutput = Strings.Replace(newOutput, "-04-83", "?");
            newOutput = Strings.Replace(newOutput, "-04-84", "?");
            newOutput = Strings.Replace(newOutput, "-04-85", "?");
            newOutput = Strings.Replace(newOutput, "-04-86", "?");
            newOutput = Strings.Replace(newOutput, "-04-87", "?");
            newOutput = Strings.Replace(newOutput, "-04-88", "?");
            newOutput = Strings.Replace(newOutput, "-04-89", "?");
            newOutput = Strings.Replace(newOutput, "-04-8A", "?");
            newOutput = Strings.Replace(newOutput, "-04-8B", "?");
            newOutput = Strings.Replace(newOutput, "-04-8C", "?");
            newOutput = Strings.Replace(newOutput, "-04-8D", "?");
            newOutput = Strings.Replace(newOutput, "-04-8E", "?");
            newOutput = Strings.Replace(newOutput, "-04-8F", "?");
            newOutput = Strings.Replace(newOutput, "-04-90", "?");
            newOutput = Strings.Replace(newOutput, "-04-91", "?");
            newOutput = Strings.Replace(newOutput, "-04-92", "?");
            newOutput = Strings.Replace(newOutput, "-04-93", "?");
            newOutput = Strings.Replace(newOutput, "-04-94", "?");
            newOutput = Strings.Replace(newOutput, "-04-95", "?");
            newOutput = Strings.Replace(newOutput, "-04-96", "?");
            newOutput = Strings.Replace(newOutput, "-04-97", "?");
            newOutput = Strings.Replace(newOutput, "-04-98", "?");
            newOutput = Strings.Replace(newOutput, "-04-99", "?");
            newOutput = Strings.Replace(newOutput, "-04-9A", "?");
            newOutput = Strings.Replace(newOutput, "-04-9B", "?");
            if (rbtnUrdu.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-9C", "?");
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-9C", "?");
            }
            newOutput = Strings.Replace(newOutput, "-04-9D", "?");
            newOutput = Strings.Replace(newOutput, "-04-9E", "?");
            newOutput = Strings.Replace(newOutput, "-04-9F", "?");
            newOutput = Strings.Replace(newOutput, "-04-A0", "?");
            newOutput = Strings.Replace(newOutput, "-04-A1", "?");
            //  newOutput = Replace(newOutput, "-04-A3-04-A2", "?")
            if (chkHehHamza.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-A3-04-A2", "?");
                newOutput = Strings.Replace(newOutput, "-04-BF-04-A2", "?");
                // testing
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-A3-04-A2", "??");
            }

            newOutput = Strings.Replace(newOutput, "-04-A2-04-BF", "?");
            // urdu arabic
            if (rbtnUrdu.Checked)
            {
                // urdu
                if (chkHehHamza.Checked)
                {
                    newOutput = Strings.Replace(newOutput, "-04-BF-04-A6", "?");
                    // testing
                    newOutput = Strings.Replace(newOutput, "-04-A3-04-A6", "?");
                    // testing
                }
                newOutput = Strings.Replace(newOutput, "-04-A6-04-BF", "?");
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-A6-04-BF", "?");
            }
            //  newOutput = Replace(newOutput, "-04-BF-04-A6", "?")  ' testing
            newOutput = Strings.Replace(newOutput, "-04-A3-04-A6", "??");
            newOutput = Strings.Replace(newOutput, "-04-A2", "?");
            newOutput = Strings.Replace(newOutput, "-04-A3", "?");
            //     newOutput = Replace(newOutput, "-44-A3", "?")  ' modified
            newOutput = Strings.Replace(newOutput, "-04-A4-04-BF", "?");
            if (rbtnUrdu.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-A4", "?");
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-A4", "?");
            }
            newOutput = Strings.Replace(newOutput, "-04-A5", "?");
            // newOutput = Replace(newOutput, "-04-A6", "?")
            if (rbtnUrdu.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-A6", "?");
                newOutput = Strings.Replace(newOutput, "-04-A7", "?");
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-A6", "?");
                newOutput = Strings.Replace(newOutput, "-04-A7", "?");
            }


            //'/////////////////////////////////////////////



            //'/////////////////////////////////////////////

            newOutput = Strings.Replace(newOutput, "-04-A8", "?");
            // 2 ZEIR NICHE  
            //remove kashida
            if (chkRKashida.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-A9", "");
                // 169 TATWEEL
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-A9", "?");
                // 169 TATWEEL
            }
            newOutput = Strings.Replace(newOutput, "-04-AA", "?");
            // ZEIR
            newOutput = Strings.Replace(newOutput, "-04-AB", "?");
            // ZABAR
            newOutput = Strings.Replace(newOutput, "-04-AC", "?");
            // PAESH
            newOutput = Strings.Replace(newOutput, "-04-AD", "?");
            // 0651 UNICODE VALUE
            newOutput = Strings.Replace(newOutput, "-04-AE", "?");
            // ALAH ISLAM SH

            newOutput = Strings.Replace(newOutput, "-04-B0", "?");
            // KHARI ZEIR
            if (rbtnUrdu.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-B1-04-B1", "?");
                // SAKIN
                newOutput = Strings.Replace(newOutput, "-04-B1", "?");
                // SAKIN
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-B1-04-B1", "?");
                // SAKIN
                newOutput = Strings.Replace(newOutput, "-04-B1", "?");
                // SAKIN
            }
            newOutput = Strings.Replace(newOutput, "-04-B3", "?");
            // MAD
            newOutput = Strings.Replace(newOutput, "-04-B4", "?");
            // SAKIN
            newOutput = Strings.Replace(newOutput, "-04-B5", "?");
            //  PAESH TYPE PHOOL
            newOutput = Strings.Replace(newOutput, "-04-B6", "?");
            newOutput = Strings.Replace(newOutput, "-04-B7", "?");
            newOutput = Strings.Replace(newOutput, "-04-B8", "?");
            if (rbtnUrdu.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-B9", "?");
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-B9", "?");
            }
            newOutput = Strings.Replace(newOutput, "-04-BD", "?");
            // KHARI ZABAR
            newOutput = Strings.Replace(newOutput, "-04-BE", "?");
            // ULTA PAESH
            newOutput = Strings.Replace(newOutput, "-04-BF", "?");
            // HAMZA OOPER

            newOutput = Strings.Replace(newOutput, "-04-C7", "?");
            // 2 ZABAR OOPAR
            newOutput = Strings.Replace(newOutput, "-04-C8", "?");
            newOutput = Strings.Replace(newOutput, "-04-C9", "?");
            newOutput = Strings.Replace(newOutput, "-04-CA", "?");
            newOutput = Strings.Replace(newOutput, "-04-CB", "?");
            // ALLAH
            newOutput = Strings.Replace(newOutput, "-04-CF", "?");
            // 207

            if (rbtnUrdu.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-D0", "?");
                newOutput = Strings.Replace(newOutput, "-04-D1", "?");
                newOutput = Strings.Replace(newOutput, "-04-D2", "?");
                newOutput = Strings.Replace(newOutput, "-04-D3", "?");
                newOutput = Strings.Replace(newOutput, "-04-D4", "?");
                newOutput = Strings.Replace(newOutput, "-04-D5", "?");
                newOutput = Strings.Replace(newOutput, "-04-D6", "?");
                newOutput = Strings.Replace(newOutput, "-04-D7", "?");
                newOutput = Strings.Replace(newOutput, "-04-D8", "?");
                newOutput = Strings.Replace(newOutput, "-04-D9", "?");
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-D0", "?");
                newOutput = Strings.Replace(newOutput, "-04-D1", "?");
                newOutput = Strings.Replace(newOutput, "-04-D2", "?");
                newOutput = Strings.Replace(newOutput, "-04-D3", "?");
                newOutput = Strings.Replace(newOutput, "-04-D4", "?");
                newOutput = Strings.Replace(newOutput, "-04-D5", "?");
                newOutput = Strings.Replace(newOutput, "-04-D6", "?");
                newOutput = Strings.Replace(newOutput, "-04-D7", "?");
                newOutput = Strings.Replace(newOutput, "-04-D8", "?");
                newOutput = Strings.Replace(newOutput, "-04-D9", "?");
            }
            newOutput = Strings.Replace(newOutput, "-04-DA", "!");
            //218
            newOutput = Strings.Replace(newOutput, "-04-DB", "?");
            // SP B
            newOutput = Strings.Replace(newOutput, "-04-DC", "?");
            newOutput = Strings.Replace(newOutput, "-04-DE", "%");
            newOutput = Strings.Replace(newOutput, "-04-DF", "/");

            newOutput = Strings.Replace(newOutput, "-04-E0", "……");
            // ... DBL
            newOutput = Strings.Replace(newOutput, "-04-E1", ")");
            //N B
            newOutput = Strings.Replace(newOutput, "-04-E2", "(");
            //N B 
            newOutput = Strings.Replace(newOutput, "-04-E4", "+");
            newOutput = Strings.Replace(newOutput, "-04-E6", "?");
            // RAZI ALLAH SH
            newOutput = Strings.Replace(newOutput, "-04-E7", "?");
            // RAHMATU ALLAH SH
            newOutput = Strings.Replace(newOutput, "-04-E8", "?");
            newOutput = Strings.Replace(newOutput, "-04-E9", ":");
            //233
            newOutput = Strings.Replace(newOutput, "-04-EA", "?");
            newOutput = Strings.Replace(newOutput, "-04-EB", "×");
            newOutput = Strings.Replace(newOutput, "-04-EC", "=");
            newOutput = Strings.Replace(newOutput, "-04-ED", "?");
            newOutput = Strings.Replace(newOutput, "-04-EE", "?");
            newOutput = Strings.Replace(newOutput, "-04-EF", "÷");

            newOutput = Strings.Replace(newOutput, "-04-F1", "?");
            //241
            newOutput = Strings.Replace(newOutput, "-04-F2", "?");
            if (rbtnUrdu.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-F3", "?");
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-F3", ".");
            }
            newOutput = Strings.Replace(newOutput, "-04-F5", "-");
            //245  /
            newOutput = Strings.Replace(newOutput, "-04-F6", "?");
            // PBUH
            newOutput = Strings.Replace(newOutput, "-04-F7", "?");
            // 247
            newOutput = Strings.Replace(newOutput, "-04-F8", "?");
            // PBUH SHORT
            newOutput = Strings.Replace(newOutput, "-04-F9", ",");
            newOutput = Strings.Replace(newOutput, "-04-FA", "]");
            newOutput = Strings.Replace(newOutput, "-04-FB", "[");
            newOutput = Strings.Replace(newOutput, "-04-FC", ".");
            //.

            bgw4_new.ReportProgress(20);
            if (chkQuotMarks.Checked)
            {
                newOutput = Strings.Replace(newOutput, "-04-FE", "’");
                //254
                newOutput = Strings.Replace(newOutput, "-04-FD", "‘");
                //253
            }
            else
            {
                newOutput = Strings.Replace(newOutput, "-04-FD", "’");
                //253
                newOutput = Strings.Replace(newOutput, "-04-FE", "‘");
                //254
            }
            newOutput = Strings.Replace(newOutput, "-04-3A", "");
            newOutput = Strings.Replace(newOutput, "-04-3B", "");
            newOutput = Strings.Replace(newOutput, "-09", myTab);

            newOutput = Strings.Replace(newOutput, "-20", " ");
            newOutput = Strings.Replace(newOutput, "-21", "!");
            newOutput = Strings.Replace(newOutput, "-22", Convert.ToChar(34));
            newOutput = Strings.Replace(newOutput, "-23", "#");
            newOutput = Strings.Replace(newOutput, "-24", "$");
            newOutput = Strings.Replace(newOutput, "-25", "%");
            newOutput = Strings.Replace(newOutput, "-26", "&");
            newOutput = Strings.Replace(newOutput, "-27", "'");
            newOutput = Strings.Replace(newOutput, "-28", "(");
            newOutput = Strings.Replace(newOutput, "-29", ")");
            newOutput = Strings.Replace(newOutput, "-2A", "*");
            newOutput = Strings.Replace(newOutput, "-2B", "+");
            newOutput = Strings.Replace(newOutput, "-2C", ",");
            newOutput = Strings.Replace(newOutput, "-2D", "-");
            newOutput = Strings.Replace(newOutput, "-2E", ".");
            newOutput = Strings.Replace(newOutput, "-2F", "/");

            newOutput = Strings.Replace(newOutput, "-3A", ":");
            newOutput = Strings.Replace(newOutput, "-3B", ";");
            newOutput = Strings.Replace(newOutput, "-3C", "<");
            newOutput = Strings.Replace(newOutput, "-3D", "=");
            newOutput = Strings.Replace(newOutput, "-3E", ">");
            newOutput = Strings.Replace(newOutput, "-3F", "?");

            newOutput = Strings.Replace(newOutput, "-40", "@");
            newOutput = Strings.Replace(newOutput, "-41", "A");
            newOutput = Strings.Replace(newOutput, "-42", "B");
            newOutput = Strings.Replace(newOutput, "-43", "C");
            newOutput = Strings.Replace(newOutput, "-44", "D");
            newOutput = Strings.Replace(newOutput, "-45", "E");
            newOutput = Strings.Replace(newOutput, "-46", "F");
            newOutput = Strings.Replace(newOutput, "-47", "G");
            newOutput = Strings.Replace(newOutput, "-48", "H");
            newOutput = Strings.Replace(newOutput, "-49", "I");
            newOutput = Strings.Replace(newOutput, "-4A", "J");
            newOutput = Strings.Replace(newOutput, "-4B", "K");
            newOutput = Strings.Replace(newOutput, "-4C", "L");
            newOutput = Strings.Replace(newOutput, "-4D", "M");
            newOutput = Strings.Replace(newOutput, "-4E", "N");
            newOutput = Strings.Replace(newOutput, "-4F", "O");

            newOutput = Strings.Replace(newOutput, "-50", "P");
            newOutput = Strings.Replace(newOutput, "-51", "Q");
            newOutput = Strings.Replace(newOutput, "-52", "R");
            newOutput = Strings.Replace(newOutput, "-53", "S");
            newOutput = Strings.Replace(newOutput, "-54", "T");
            newOutput = Strings.Replace(newOutput, "-55", "U");
            newOutput = Strings.Replace(newOutput, "-56", "V");
            newOutput = Strings.Replace(newOutput, "-57", "W");
            newOutput = Strings.Replace(newOutput, "-58", "X");
            newOutput = Strings.Replace(newOutput, "-59", "Y");
            newOutput = Strings.Replace(newOutput, "-5A", "Z");
            newOutput = Strings.Replace(newOutput, "-5B", "[");
            newOutput = Strings.Replace(newOutput, "-5C", "\\");
            newOutput = Strings.Replace(newOutput, "-5D", "]");
            newOutput = Strings.Replace(newOutput, "-5E", "^");
            newOutput = Strings.Replace(newOutput, "-5F", "_");

            newOutput = Strings.Replace(newOutput, "-60", "`");
            newOutput = Strings.Replace(newOutput, "-61", "a");
            newOutput = Strings.Replace(newOutput, "-62", "b");
            newOutput = Strings.Replace(newOutput, "-63", "c");
            newOutput = Strings.Replace(newOutput, "-64", "d");
            newOutput = Strings.Replace(newOutput, "-65", "e");
            newOutput = Strings.Replace(newOutput, "-66", "f");
            newOutput = Strings.Replace(newOutput, "-67", "g");
            newOutput = Strings.Replace(newOutput, "-68", "h");
            newOutput = Strings.Replace(newOutput, "-69", "i");
            newOutput = Strings.Replace(newOutput, "-6A", "j");
            newOutput = Strings.Replace(newOutput, "-6B", "k");
            newOutput = Strings.Replace(newOutput, "-6C", "l");
            newOutput = Strings.Replace(newOutput, "-6D", "m");
            newOutput = Strings.Replace(newOutput, "-6E", "n");
            newOutput = Strings.Replace(newOutput, "-6F", "o");

            newOutput = Strings.Replace(newOutput, "-70", "p");
            newOutput = Strings.Replace(newOutput, "-71", "q");
            newOutput = Strings.Replace(newOutput, "-72", "r");
            newOutput = Strings.Replace(newOutput, "-73", "s");
            newOutput = Strings.Replace(newOutput, "-74", "t");
            newOutput = Strings.Replace(newOutput, "-75", "u");
            newOutput = Strings.Replace(newOutput, "-76", "v");
            newOutput = Strings.Replace(newOutput, "-77", "w");
            newOutput = Strings.Replace(newOutput, "-78", "x");
            newOutput = Strings.Replace(newOutput, "-79", "y");
            newOutput = Strings.Replace(newOutput, "-7A", "z");
            newOutput = Strings.Replace(newOutput, "-7B", "{");
            newOutput = Strings.Replace(newOutput, "-7C", "|");
            newOutput = Strings.Replace(newOutput, "-7D", "}");
            newOutput = Strings.Replace(newOutput, "-7E", "~");
            newOutput = Strings.Replace(newOutput, "-7F", "");

            newOutput = Strings.Replace(newOutput, "-30", "0");
            newOutput = Strings.Replace(newOutput, "-31", "1");
            newOutput = Strings.Replace(newOutput, "-32", "2");
            // newOutput = Replace(newOutput, "-33", "3")
            newOutput = Strings.Replace(newOutput, "-34", "4");
            newOutput = Strings.Replace(newOutput, "-35", "5");
            newOutput = Strings.Replace(newOutput, "-36", "6");
            newOutput = Strings.Replace(newOutput, "-37", "7");
            newOutput = Strings.Replace(newOutput, "-38", "8");
            newOutput = Strings.Replace(newOutput, "-39", "9");

            newOutput = Strings.Replace(newOutput, "-33", "3");
            // @ last

            bgw4_new.ReportProgress(30);

            //newOutput = Replace(newOutput, "-", "")
            //'===========================================================================
            // ElseIf my_binaryData(i + 1) = 166 And my_OutPut.Last = Convert.ToChar(1574).ToString Then
            //'remove ya-hamza  before hamza
            //my_OutPut = my_OutPut.Remove(my_OutPut.Length - 1, 1)
            //my_OutPut += Convert.ToChar(ip2uc.Item(166)).ToString
            //my_OutPut += Convert.ToChar(ip2uc.Item(191)).ToString
            //i += 1


            // Dim searchIndex As Integer = 1


            if (chkRDigits.Checked & chkReverseSSign.Checked & chkThousSeparator.Checked)
            {
                MatchCollection digitMatches = default(MatchCollection);

                if (rbtnUrdu.Checked)
                {
                    digitMatches = Regex.Matches(newOutput, regUDigits);
                }
                else
                {
                    digitMatches = Regex.Matches(newOutput, regADigits);
                }

                foreach (Match mach in digitMatches)
                {
                    if (mach.Value.Last == "/")
                    {
                        string machValue = mach.Value;
                        machValue = machValue.Remove(machValue.Length - 1, 1);
                        machValue = Strings.StrReverse(machValue) + "/";
                        newOutput = newOutput.Remove(mach.Index, mach.Length);
                        newOutput = newOutput.Insert(mach.Index, machValue);
                    }
                    else
                    {
                        newOutput = newOutput.Remove(mach.Index, mach.Length);
                        newOutput = newOutput.Insert(mach.Index, Strings.StrReverse(mach.Value));
                    }
                }
            }
            else
            {
                if (chkRDigits.Checked & !chkThousSeparator.Checked)
                {
                    MatchCollection digitMatches = default(MatchCollection);

                    if (rbtnUrdu.Checked)
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
                        newOutput = newOutput.Insert(mach.Index, Strings.StrReverse(mach.Value));
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

            if (chkBariYee.Checked)
            {
                newOutput = Regex.Replace(newOutput, "(?)" + regUrduAlfabat, "?$2");
            }

            bgw4_new.ReportProgress(80);
            if (rbtnUrdu.Checked)
            {
                newOutput = Regex.Replace(newOutput, "(?)" + regUrduAlfabat, "$1 $2");
            }
            bgw4_new.ReportProgress(90);

            //'////////////////////////////////////
            // optional setting
            // remove Double Space
            if (chkRDoubleSpace.Checked)
            {
                newOutput = Regex.Replace(newOutput, "[ ]+[ ]", " ");
            }

            if (chkRErabs.Checked)
            {
                newOutput = Regex.Replace(newOutput, regRemoveAhrab, "");
            }
            if (chkYearSign.Checked)
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
                Interaction.MsgBox(ex.Message);
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
            Interaction.MsgBox("Done");
        }

        private void bgw4_new_ProgressChanged(System.Object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            pb.Value = e.ProgressPercentage;
        }

        private void bgw4_new_RunWorkerCompleted(System.Object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            WriteStatusMessage("File is successfully converted");
            pb.Value = 0;
            pb.Visible = false;
            btnOpenFile.Enabled = true;
            btnInPageFile.Enabled = true;
        }

        private void chkReverseSSign_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (chkReverseSSign.Checked)
            {
                chkThousSeparator.Checked = true;
            }
            else
            {
                chkThousSeparator.Checked = false;
            }
        }

        private void chkThousSeparator_CheckedChanged(System.Object sender, System.EventArgs e)
        {
            if (chkThousSeparator.Checked)
            {
                chkReverseSSign.Checked = true;
            }
            else
            {
                chkReverseSSign.Checked = false;
            }
        }

        public frmInPageConverter()
        {
            Resize += frmInPageConverter_Resize;
            Load += frmInPageConverter_Load;
            FormClosing += frmInPageConverter_FormClosing;
        }
    }
    */
}
