using Cloud.Sign.App.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Cloud.Sign.App
{
    public partial class InternalWindow : Form
    {
        MainWindow MainForm = null;
        string Environment = ConfigurationManager.AppSettings["Environment"].ToString();
        public InternalWindow(X509Certificate2 SelectedCert, string LoggedEmail, MainWindow Window)
        {
            InitializeComponent();
            if (SelectedCert == null)
            {
                lblCertName.Text = "No Card Connected";
                lblCertDesc.Text = "insert your card and click activiate";
                pictureBoxConnected.Visible = false;
                pictureBoxDisconnected.Visible = true;
                btnChooseCert.Visible = true;
                lblCloudStatus.Visible = false;
                lblLastSigned.Visible = false;
                label4.Visible = false;
                label3.Visible = false;
                pictureBox2.Visible = false;
                pictureBoxCLoudStatus.Visible = false;
            }
            else
            {
                lblCertName.Text = "Connected ( " + SelectedCert.GetNameInfo(X509NameType.SimpleName, false) + " )";
                lblCertDesc.Text = "";
                pictureBoxConnected.Visible = true;
                pictureBoxDisconnected.Visible = false;
                btnChooseCert.Visible = false;
                lblCloudStatus.Visible = true;
                lblLastSigned.Visible = true;
                label4.Visible = true;
                label3.Visible = true;
                pictureBox2.Visible = false;
                pictureBoxCLoudStatus.Visible = true;
            }
            lblCloudStatus.Text = "Connected";
            lblLoggedEmail.Text = LoggedEmail;
            MainForm = Window;
            if (MainForm.Tenant != 0)
            {
                lblLoggedCompany.Text = MainForm.Company;
            } 
            SetLastSignDate();
        }

        private void InternalWindow_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                Minimize();
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                ni.Visible = false;
                this.Show();
            }
            //using (ProcessIcon pi = new ProcessIcon())
            //{
            //    pi.Display();
            //    this.Hide();
            //}
        }

        public void Minimize()
        {
            ShowMeInvoker = null;
            MainForm.MaximizeDelegate = null;
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "MainWindow")
                {
                    var temp = frm as MainWindow;

                    temp.Minimize();

                }
            }
            MainForm.Minimize();
            // Put the icon in the system tray and allow it react to mouse clicks.	
            //ni.BalloonTipTitle = "Digital Signiture App";
            //ni.BalloonTipText = "You can access Digital Signiture App from here.";
            //ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            //ni.Icon = Resources.signature;
            //ni.Text = "Digital Signiture Application";
            //ni.Visible = true;
            //ni.ShowBalloonTip(500);

            //// Attach a context menu.
            //ni.ContextMenuStrip = new ContextMenus().Create();
        }

        void ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Maximize();
            }
        }

        public void CloseMe()
        {
            this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                this.Close();

            });
        }
        object ConnectionStateChange;
        public void ConnectionStateChanged(bool IsCloud = false, bool IsCloudConnected = false)
        {
            ConnectionStateChange = this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                if (!pictureBoxWarning.Visible)
                {
                    if (IsCloud)
                    {
                        lblCloudStatus.Visible = true;
                        lblLastSigned.Visible = true;
                        label4.Visible = true;
                        label3.Visible = true;
                        pictureBox2.Visible = false;
                        //pictureBoxCLoudStatus.Visible = true;
                        if (IsCloudConnected == true)
                        {
                            lblCloudStatus.Text = "Connected";
                            pictureBoxCLoudStatus.Visible = true;
                            pictureBoxCLoudStatusDisConn.Visible = false;
                        }
                        else
                        {
                            lblCloudStatus.Text = "Disconnected";
                            pictureBoxCLoudStatus.Visible = false;
                            pictureBoxCLoudStatusDisConn.Visible = true;
                        }
                    }
                    else
                    {
                        lblCertName.Text = "No Card Connected";
                        lblCertDesc.Text = "insert your card and click activiate";
                        pictureBoxConnected.Visible = false;
                        pictureBoxDisconnected.Visible = true;
                        btnChooseCert.Visible = true;
                        pictureBoxCLoudStatus.Visible = false;
                        pictureBoxCLoudStatusDisConn.Visible = false;
                        lblCloudStatus.Visible = false;
                        lblLastSigned.Visible = false;
                        label4.Visible = false;
                        label3.Visible = false;
                        pictureBox2.Visible = false;
                    }
                }
                
                if (IsCloudConnected == false)
                {

                    //this.Maximize();
                }

            });
        }

        public void PinCodeStatus(bool Entered = true,bool ValidCert = true, X509Certificate2 SelectedCert = null)
        {
            ConnectionStateChange = this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                if (Entered && ValidCert)
                {

                    //lblCloudStatus.Text = "Disconnected";
                    if (SelectedCert != null)
                    {
                        lblCertName.Text = "Connected ( " + SelectedCert.GetNameInfo(X509NameType.SimpleName, false) + " )";
                    } 
                    lblCertDesc.Text = "";
                    pictureBoxConnected.Visible = true;
                    pictureBoxDisconnected.Visible = false;
                    pictureBoxWarning.Visible = false;
                    btnChooseCert.Visible = false;
                    lblCloudStatus.Visible = true;
                    lblLastSigned.Visible = true;
                    label4.Visible = true;
                    label3.Visible = true;
                    pictureBox2.Visible = false;
                    pictureBoxCLoudStatus.Visible = true;

                }
                else
                {
                    //lblCertName.Text = "Disconnected";
                    if (!Entered)
                    {
                        lblCertName.Text = "Password Required";
                        lblCertDesc.Text = "click activiate to enter your password";
                        pictureBoxConnected.Visible = false;
                        pictureBoxDisconnected.Visible = false;
                        pictureBoxWarning.Visible = true;
                        btnChooseCert.Visible = true;
                        pictureBoxCLoudStatus.Visible = false;
                        pictureBoxCLoudStatusDisConn.Visible = false;
                        lblCloudStatus.Visible = false;
                        lblLastSigned.Visible = false;
                        label4.Visible = false;
                        label3.Visible = false;
                        pictureBox2.Visible = false;
                    }
                    else
                    {
                        lblCertName.Text = "No Card Connected";
                        lblCertDesc.Text = "insert your card and click activiate";
                        pictureBoxConnected.Visible = false;
                        pictureBoxDisconnected.Visible = true;
                        btnChooseCert.Visible = true;
                        pictureBoxCLoudStatus.Visible = false;
                        pictureBoxCLoudStatusDisConn.Visible = false;
                        lblCloudStatus.Visible = false;
                        lblLastSigned.Visible = false;
                        label4.Visible = false;
                        label3.Visible = false;
                        pictureBox2.Visible = false;
                    }
                    
                }
                //if (IsCloudConnected == false)
                //{

                //    this.Maximize();
                //}

            });
        }

        object CardStateChange;
        public void CardStateChanged(X509Certificate2 SelectedCert)
        {
            CardStateChange = this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                if (SelectedCert != null && !pictureBoxWarning.Visible)
                {
                    lblCertName.Text = "Connected ( " + SelectedCert.GetNameInfo(X509NameType.SimpleName, false) + " )";
                    lblCertDesc.Text = "";
                    pictureBoxConnected.Visible = true;
                    pictureBoxDisconnected.Visible = false;
                    btnChooseCert.Visible = false;
                }
                //this.Maximize();

            });
        }

        public void Maximize()
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "MainWindow")
                {
                    var temp = frm as MainWindow;

                    temp.Maximize();

                }
            }
            //ni.Visible = false;
            //this.TopMost = true;
            //this.Show();
        }

        private void InternalWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            XmlDocument doc = new XmlDocument();
            doc.Load(MyPath + @"\Settings\Settings.xml");

            // find a node - here the one with name='abc'
            XmlNode node = doc.SelectSingleNode("/Settings/Token[@Value='" + MainForm.Token + "']");

            // if found....
            if (node != null)
            {
                // get its parent node
                XmlNode parent = node.ParentNode;

                // remove the child node
                parent.RemoveChild(node);

                // verify the new XML structure
                string newXML = doc.OuterXml;

                // save to file or whatever....
                doc.Save(MyPath + @"\Settings\Settings.xml");
            }
            MainForm.UpdateAppStatus(false, false, false);
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Minimize();
            this.Hide();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.ApplicationExitCall) return;
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            Minimize();
            this.Hide();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LogOutAction();
        }

        private void linkLabelPassWordRequired_LinkClicked(object sender, EventArgs e)
        {
            //string mystatus = "";
            //string mymessage = "";
            MainForm.SignSamplePdf(out MainForm.mystatus, out MainForm.mymessage);
            if (MainForm.mymessage == "PIN Code Is Required")
            {

                PinCodeStatus(false);

            }
            else
            {

                PinCodeStatus(true);
            }
        }
        private void btnChooseCert_Clicked(object sender, EventArgs e)
        {
            //MainForm.Minimize();
            this.Minimize();
            this.Hide();
            MainForm.CheckStatus();
            if (MainForm.mymessage == "InActive Cert" || MainForm.mymessage.Contains("The chain context handle is invalid"))
            {

                PinCodeStatus(true,false);

            }
            else if(MainForm.mymessage == "PIN Code Is Required")
            {

                PinCodeStatus(false); 
            }
            //if (MainForm.mymessage == "service down")
            //{
            //    MessageBox.Show("The smart card service is down", "IDProtect Manager");
            //}
            else
            {

                PinCodeStatus(true,true, MainForm.selected);
            }
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Focus();

        } 

        public void LogOutAction(bool ForceLogOut = false)
        {
            MainWindow.StopTimer = true;
            MainForm.UpdateAppStatus(false, false, false);
            if (ForceLogOut == false)
            {
                this.Hide();
            } 
            ConnectionStateChange = null;
            var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            XmlDocument doc = new XmlDocument();
            doc.Load(MyPath + @"\Settings\Settings.xml");

            // find a node - here the one with name='abc'
            XmlNode node = doc.SelectSingleNode("/Settings/Token[@Value='" + MainForm.Token + "']");

            // if found....
            if (node != null)
            {
                // get its parent node
                XmlNode parent = node.ParentNode;

                // remove the child node
                parent.RemoveChild(node);

                // verify the new XML structure
                string newXML = doc.OuterXml;

                // save to file or whatever....
                doc.Save(MyPath + @"\Settings\Settings.xml");
            }
            //var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            //System.IO.Directory.CreateDirectory(MyPath + @"\Settings\");
            //using (XmlWriter writer = XmlWriter.Create(MyPath + @"\Settings\Settings.xml"))
            //{
            //    writer.WriteStartDocument();
            //    writer.WriteStartElement("Settings");
            //    writer.WriteStartElement("Token");
            //    writer.WriteAttributeString("Value", token);
            //    writer.WriteEndElement();
            //    writer.WriteEndElement();
            //    writer.WriteEndDocument();
            //}
            MainForm.Token = null;
            MainForm.selected = null;
            
            //MainForm.MyTimer.Stop();
            //MainForm.MyCheckRequestTimer.Stop();
            if (ForceLogOut == false)
            {
                MainForm.ClearLoginCred();
                MainForm.Show();
            }
            
            //this.Close();
            //bool isExist = false;
            //foreach (Form frm in Application.OpenForms)
            //{
            //    if (frm.Name == "MainWindow")
            //    {
            //        isExist = true;
            //        var temp = frm as MainWindow;

            //        temp.Maximize();

            //    }
            //}
            //if (!isExist)
            //{
            //    var mainForm = new MainWindow();
            //    mainForm.Show();
            //}
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void InternalWindow_Load(object sender, EventArgs e)
        {

        }

        object ShowMeInvoker;
        public void ShowMe(DateTime? LastSigned, string Email = null)
        {
            if (LastSigned != null)
            {
                SetLastSignDate();
            }
            if (!string.IsNullOrEmpty(Email))
            {
                lblLoggedEmail.Text = Email;
                if (MainForm != null && MainForm.Tenant != 0)
                {
                    lblLoggedCompany.Text = MainForm.Company;
                }
            }
            //lblLastSigned.Text = LastSigned.ToShortDateString() + " " + LastSigned.ToShortTimeString();
            ShowMeInvoker = this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                //this.WindowState = FormWindowState.Maximized;
                //this.TopMost = true;

                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Focus();

            });
        }

        public void HideMe()
        {
            ShowMeInvoker = this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                this.Hide();

            });
        }

        private void SetLastSignDate()
        {
            XmlDocument xmldoc = new XmlDocument();
            var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            if (File.Exists(MyPath + @"\Settings\StatusSettings.xml"))
            {
                xmldoc.Load(MyPath + @"\Settings\StatusSettings.xml");

                XmlNodeList TokenNode = xmldoc.GetElementsByTagName("LastSigned");
                if (TokenNode.Count > 0)
                {
                    lblLastSigned.Text = TokenNode[0].Attributes["Value"].Value;
                }
            }

        }
    }
}
