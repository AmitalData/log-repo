using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cloud.Sign.App.Properties;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using Cloud.Sign.App.Helpers;
using Microsoft.AspNet.SignalR.Client;
using iTextSharp.text.pdf.security;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters;
using System.Configuration;
using System.Xml;
using System.Deployment.Application;
using Microsoft.Win32;
using System.Xml.Linq;
using iTextSharp.text;

namespace Cloud.Sign.App
{
    public partial class MainWindow : Form
    {
        public string Version = "2.34";
        //RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        object _Obj = new object();
        public string Token = "";
        public string Email = "";
        public DateTime LastSigned;
        string URI = ConfigurationManager.AppSettings["SystemUrl"].ToString();//"http://localhost:9996";
        string Environment = ConfigurationManager.AppSettings["Environment"].ToString();
        public X509Certificate2 selected;
        public int Tenant;
        public string Company;
        InternalWindow InternalForm;
        public Timer MyTimer = new Timer();
        public Timer MyCheckRequestTimer = new Timer();
        BackgroundWorker MyTimerwork = new BackgroundWorker();
        BackgroundWorker MyCheckRequestTimerwork = new BackgroundWorker();
        BackgroundWorker VersionTimerwork = new BackgroundWorker();
        BackgroundWorker LogOutTimerwork = new BackgroundWorker();


        private bool allowVisible;
        #region Chunck Props
        int buffersize = 100000; // 100k
        int counter = -1;
        long sentBytes = 0;
        int position = 0;
        byte[] currentData;
        string blockId2;
        double value;
        List<string> blockIdsArray;
        double blocksNumber;
        string encodedFileName;
        FileInformation FileInfo;
        #endregion

        #region Connection Props

        private IHubProxy HubProxy { get; set; }
        private HubConnection MyConnection { get; set; }

        #endregion

        protected override void SetVisibleCore(bool value)
        {
            if (!allowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }
        bool FromLogOutBtn = false;
        public MainWindow(bool FromLogOut)
        {
            //reg.SetValue("Cloud.Sign.App", Application.ExecutablePath.ToString());
            this.allowVisible = FromLogOut;
            FromLogOutBtn = FromLogOut;
            InitializeComponent();
            if (FromLogOut)
            {
                this.WindowState = FormWindowState.Normal;
                UpdateAppStatus(false, false, false);
                MyTimerwork.Dispose();//.Stop();
                MyCheckRequestTimerwork.Dispose();//.Stop();
                this.selected = null;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                CallMe();
            }
        }

        public async void CallMe()
        {
            await LoginUsingToken();
        }
        bool IgnoreMinimize = false;
        public MainWindow()
        {
            //reg.SetValue("Cloud.Sign.App", Application.ExecutablePath.ToString());
            InitializeComponent();
            //this.WindowState = FormWindowState.Minimized;
            Minimize(true);
            this.Hide();
            LoginUsingToken();
            if (!LogOutTimerwork.IsBusy)
            {
                LogOutTimerwork.DoWork += LogOutTimerwork_DoWork;
                LogOutTimerwork.RunWorkerAsync();
            }
            if (!VersionTimerwork.IsBusy)
            {
                //MessageBox.Show("MyTimer_Tick Timer");
                VersionTimerwork.DoWork += VersionTimerwork_DoWork;
                VersionTimerwork.RunWorkerAsync();
            }
            //Minimize();
            //this.Hide();
            //cobTenants.Hide();
            //txtEmail.Text = "angular@fnarsoft.com";
            //txtPassword.Text = "1";
        }

        private void LogOutTimerwork_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!StopTimer && DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 58 && DateTime.Now.Minute <= 59)
                {
                    
                    try
                    {
                        if (!string.IsNullOrEmpty(this.Token))
                        {
                            if (Application.OpenForms.Count > 0)
                            {
                                foreach (Form frm in Application.OpenForms)
                                {
                                    if (frm.Name == "InternalWindow")
                                    {
                                        var temp = frm as InternalWindow;
                                        temp.HideMe();
                                        temp.LogOutAction(true);
                                        this.Invoke((MethodInvoker)delegate
                                        {
                                            // close the form on the forms thread
                                            //this.WindowState = FormWindowState.Maximized;
                                            //this.TopMost = true;

                                            this.Show();
                                            this.WindowState = FormWindowState.Normal;
                                            this.Focus();
                                            this.txtEmail.Text = "";
                                            this.txtPassword.Text = "";
                                            ni.BalloonTipText = "Application disconnected , please login again";
                                            ni.ShowBalloonTip(2500);
                                        });
                                        var myTimeSpan = new TimeSpan(24, 0, 0);
                                        System.Threading.Thread.Sleep(myTimeSpan);  // Wait 24 Hours.
                                    }
                                }
                            } 
                        }
                    }
                    catch (Exception ex)
                    {
                        //InternalForm.ConnectionStateChanged(true);
                        string errorMessage = ex.Message;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                        }

                        errorMessage = errorMessage + ex.StackTrace;
                        LogFileUtil.Log("VersionTimerwork_DoWork" + errorMessage, LogFileUtil.LogLevel.Debug);
                    }

                }
                if (DateTime.Now.Hour == 23 && DateTime.Now.Minute >= 58 && DateTime.Now.Minute <= 59)
                {
                    var myTimeSpan = new TimeSpan(24, 0, 0);
                    System.Threading.Thread.Sleep(myTimeSpan);  // Wait 24 Hours.
                }
                else
                {
                    var myTimeSpan = new TimeSpan(0, 1, 0);
                    System.Threading.Thread.Sleep(myTimeSpan);  // Wait 1 Min.
                }
               
            }
        }

        private void VersionTimerwork_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                if (!StopTimer)
                {
                    var GetURI = URI + "/api/LogBoxSignatureClient/GetSignAppLastVersion?Tenant=" + Tenant;
                    
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var result = client.GetAsync(GetURI);
                            result.Wait();

                            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var Data = result.Result.Content.ReadAsStringAsync().Result.Trim('"');
                                if (Data != Version)
                                {
                                    MessageBox.Show("You don't have the last version of sign app, please close the app and open it again to get the last version.");
                                }
                            }
                            //else
                            //{
                            //    //InternalForm.ConnectionStateChanged(true);
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        //InternalForm.ConnectionStateChanged(true);
                        string errorMessage = ex.Message;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                        }

                        errorMessage = errorMessage + ex.StackTrace;
                        LogFileUtil.Log("VersionTimerwork_DoWork" + errorMessage, LogFileUtil.LogLevel.Debug);
                    } 

                }
                System.Threading.Thread.Sleep(1800000);  // Wait 30 min.
            }
        }

        public string mystatus = "";
        public string mymessage = "";
        private void MyTimer_Tick(object sender, EventArgs e)
        {
            //MessageBox.Show("Inside Timer 5555");
            while (true)
            {
                if (!StopTimer)
                { 
                    var GetURI = URI + "/api/LogBoxSignatureClient/GetServerStatus?Tenant=" + Tenant;

                    //Uploader.Uploader up = new Uploader.Uploader();
                    //var temp = up.DownloadFile("1-20680","pdf","",203);
                    //string strMore = "", status = "", message = "";
                    //SignPdf(temp, strMore, selected, out status, out message);
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            var result = client.GetAsync(GetURI);
                            result.Wait();

                            if (result.Result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                InternalForm.ConnectionStateChanged(true, true);
                                MyMessage = DialogResult.None;
                                if (Environment == "DSV")
                                {
                                    ni.Icon = Resources.dsv;
                                    ni.BalloonTipText = "You can access DSV sign application from here.";
                                }
                                else
                                {
                                    ni.Icon = Resources.logboxicon1;
                                    ni.BalloonTipText = "You can access LogBox sign application from here.";
                                }
                            }
                            else
                            {
                                InternalForm.ConnectionStateChanged(true);
                            }


                        }
                    }
                    catch (Exception ex)
                    {
                        //InternalForm.ConnectionStateChanged(true);
                        string errorMessage = ex.Message;

                        if (ex.InnerException != null)
                        {

                            errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                        }

                        errorMessage = errorMessage + ex.StackTrace;
                        LogFileUtil.Log("MyTimer_Tick" + errorMessage, LogFileUtil.LogLevel.Debug);
                    }
                    if (Token == null)
                    {
                        UpdateAppStatus(true, false, true);
                    }
                    else
                    {
                        var IsValidCert = CheckValidCert(StoreName.My, StoreLocation.CurrentUser, "Available Certificates", "");
                        if (!IsValidCert)
                        {
                            //MessageBox.Show("!IsValidCert");
                            selected = null;
                            //MyTimer.Stop();
                            UpdateAppStatus(true, true, false);
                            foreach (Form frm in Application.OpenForms)
                            {
                                if (frm.Name == "InternalWindow")
                                {
                                    var temp = frm as InternalWindow;
                                    //temp.ShowMe();

                                    temp.ConnectionStateChanged();
                                }
                                //if (frm.Name == "MainWindow")
                                //{
                                //    var temp = frm as MainWindow;
                                //    temp.ShowDisconnectMessage();
                                //}
                            }
                        }
                        else
                        {
                            UpdateAppStatus(true, true, true);
                            foreach (Form frm in Application.OpenForms)
                            {
                                if (frm.Name == "InternalWindow")
                                {
                                    var temp = frm as InternalWindow;
                                    //temp.ShowMe();

                                    temp.CardStateChanged(selected);
                                }
                                //if (frm.Name == "MainWindow")
                                //{
                                //    var temp = frm as MainWindow;
                                //    temp.ShowDisconnectMessage();
                                //}
                            }
                            //MessageBox.Show("before SignSamplePdf");

                            if (mymessage != "PIN Code Is Required")
                            {
                                CheckStatus();
                            }
                            else
                            {
                                foreach (Form frm in Application.OpenForms)
                                {
                                    if (frm.Name == "InternalWindow")
                                    {
                                        var temp = frm as InternalWindow;
                                        //temp.ShowMe(); 
                                        temp.PinCodeStatus(false);
                                        //SignSamplePdf(out mystatus, out mymessage);
                                    }
                                }
                            }
                        }
                    }

                }
                System.Threading.Thread.Sleep(10000);  // Wait one min.
            }
        }

         

        public void CheckStatus()
        {
            SignSamplePdf(out mystatus, out mymessage);
            if (mymessage == "PIN Code Is Required")
            {
                selected = null;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "InternalWindow")
                    {
                        var temp = frm as InternalWindow;
                        //temp.ShowMe();

                        temp.PinCodeStatus(false);
                    }
                }
            }
            else if (mymessage == "InActive Cert" || mymessage.Contains("The chain context handle is invalid"))
            {
                StopTimer = true;
                selected = null;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "InternalWindow")
                    {
                        var temp = frm as InternalWindow;
                        //temp.ShowMe();

                        temp.PinCodeStatus(false);
                    }
                }
            }
            if (mymessage == "service down")
            {
                MessageBox.Show("The smart card service is down", "IDProtect Manager");
            }
            else
            {
                if (!MyTimerwork.IsBusy && selected != null)
                {
                    //MessageBox.Show("MyTimer_Tick Timer");
                    MyTimerwork.DoWork += new DoWorkEventHandler(MyTimer_Tick);
                    MyTimerwork.RunWorkerAsync();
                }
                if (!MyCheckRequestTimerwork.IsBusy && !IsWorkerActive)
                {
                    //MessageBox.Show("MyCheckRequestTimer_Tick Timer");
                    MyCheckRequestTimerwork.DoWork += new DoWorkEventHandler(MyCheckRequestTimer_Tick);
                    MyCheckRequestTimerwork.RunWorkerAsync();
                    IsWorkerActive = true;
                }
                if (selected != null)
                {
                    StopTimer = false;
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "InternalWindow")
                        {
                            var temp = frm as InternalWindow;
                            //temp.ShowMe();
                            if (mymessage.Contains("Access denied"))
                            {
                                temp.ConnectionStateChanged();
                                UpdateAppStatus(true, true, false);
                                selected = null;
                            }
                            else
                            {
                                temp.PinCodeStatus(true);
                            }
                        }
                    }
                }
            }
        }

        private static bool ShowBalloon = true;
        public void UpdateAppStatus(bool IsActive, bool IsLogged, bool IsValidCert)
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    IsLogged = false;
                }
                if (!IsActive || !IsValidCert || !IsLogged)
                {
                    if (Environment == "DSV")
                    {
                        ni.Icon = Resources.dsvInActive;
                        ni.BalloonTipTitle = "DSV Sign Application";
                    }
                    else
                    {
                        ni.Icon = Resources.logboxiconInActive;
                        ni.BalloonTipTitle = "LogBox Sign Application";
                    }
                    if (IsLogged == false)
                    {
                        ni.BalloonTipText = "Application disconnected , please login again.";
                        ni.ShowBalloonTip(2500);
                        ShowBalloon = false;
                    }
                    else if (!IsValidCert && ni.BalloonTipText != "Your Card Disconnected." && ShowBalloon)
                    {
                        ni.BalloonTipText = "Your Card Disconnected.";
                        ni.ShowBalloonTip(1000);
                        ShowBalloon = false;
                    }
                }
                else
                {
                    ShowBalloon = true;
                    if (Environment == "DSV")
                    {
                        ni.Icon = Resources.dsv;
                    }
                    else
                    {
                        ni.Icon = Resources.logboxicon1;
                    }

                }
                if (string.IsNullOrEmpty(Token))
                {
                    var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                    XmlDocument xmldoc = new XmlDocument();
                    if (File.Exists(MyPath + @"\Settings\Settings.xml"))
                    {
                        xmldoc.Load(MyPath + @"\Settings\Settings.xml");

                        XmlNodeList TokenNode = xmldoc.GetElementsByTagName("Token");
                        if (TokenNode != null && TokenNode[0] != null)
                        {
                            Token = TokenNode[0].Attributes["Value"].Value;
                        }
                    }
                }
                if (Token != null)
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Token", Token);

                        StatusData Data = new StatusData()
                        {
                            IsActive = IsActive,
                            LastStatusDate = DateTime.Now,
                            Tenant = Tenant,
                            LoggedByUserEmail = Email,
                            IsLogged = IsLogged,
                            IsValidCert = IsValidCert
                        };
                        var serializedObj = JsonConvert.SerializeObject(Data);
                        var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                        var resultData = client.PostAsync(URI + "/api/LogBoxSignatureClient" + "?Email=" + Email, contentData);
                        resultData.Wait();
                        if (resultData.Result.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                        }
                        else
                        {

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                }

                errorMessage = errorMessage + ex.StackTrace;
                LogFileUtil.Log("UpdateAppStatus" + errorMessage, LogFileUtil.LogLevel.All);
                if (Environment == "DSV")
                {
                    ni.Icon = Resources.dsvInActive;
                }
                else
                {
                    ni.Icon = Resources.logboxiconInActive;
                }

                ni.BalloonTipText = "The Server is not reachable.";
                ni.ShowBalloonTip(1000);
                //if (MyMessage != DialogResult.OK)
                //{

                //    MyMessage = MessageBox.Show("The Server You are trying to connect is not reachable, please call your system administrator");
                //}
            }
        }
        DialogResult MyMessage;
        private void MainWindow_Resize(object sender, EventArgs e)
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
        }
        public void Minimize(bool InActive = false)
        {
            Maximized = false;
            MaximizeDelegate = null;
            //this.Invoke((MethodInvoker)delegate
            //{
            // Put the icon in the system tray and allow it react to mouse clicks.
            if (Environment == "DSV")
            {
                ni.BalloonTipTitle = "DSV Sign Application";
                ni.BalloonTipText = "You can access DSV sign application from here.";
            }
            else
            {
                ni.BalloonTipTitle = "LogBox Sign Application";
                ni.BalloonTipText = "You can access LogBox sign application from here.";
            }
            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            if (InActive || selected == null)
            {
                if (Environment == "DSV")
                {
                    ni.Icon = Resources.dsvInActive;
                }
                else
                {
                    ni.Icon = Resources.logboxiconInActive;
                }

            }
            else
            {
                if (Environment == "DSV")
                {
                    ni.Icon = Resources.dsv;
                }
                else
                {
                    ni.Icon = Resources.logboxicon1;
                }
            }
            if (Environment == "DSV")
            {
                ni.Text = "DSV Sign Application";
            }
            else
            {
                ni.Text = "LogBox Sign Application";
            }
            ni.Visible = true;
            ni.ShowBalloonTip(500);

            // Attach a context menu.
            ni.ContextMenuStrip = new ContextMenus().Create();
            //});
        }

        async void ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.allowVisible = true;
                if (!Maximized)
                {
                    await Maximize();
                }
                else
                {
                    Minimize();
                }
            }
        }

        public object MaximizeDelegate;
        public bool Maximized = false;
        public async Task Maximize()
        {

            //MaximizeDelegate = this.Invoke((MethodInvoker)delegate
            //{
            Maximized = true;
            StopTimer = false;
            if (string.IsNullOrEmpty(Token))
            {
                var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                XmlDocument xmldoc = new XmlDocument();
                if (File.Exists(MyPath + @"\Settings\Settings.xml"))
                {
                    xmldoc.Load(MyPath + @"\Settings\Settings.xml");

                    XmlNodeList TokenNode = xmldoc.GetElementsByTagName("Token");
                    if (TokenNode != null && TokenNode[0] != null)
                    {
                        Token = TokenNode[0].Attributes["Value"].Value;
                    }
                }

            }
            if (!string.IsNullOrEmpty(Token) && !FromLogOutBtn)//&& Tenant != -1 
            {
                //MessageBox.Show("Maximize");
                await ContinueLoginProcess(Token, false);
                //if (selected != null)
                //{
                //    Maximized = true;
                //}
            }
            else
            {
                ni.Visible = false;
                this.TopMost = true;
                this.Show();
                if (Token == null)
                {
                    UpdateAppStatus(true, false, false);
                }
                else
                {
                    var IsValidCert = CheckValidCert(StoreName.My, StoreLocation.CurrentUser, "Available Certificates", "");
                    UpdateAppStatus(true, true, IsValidCert);
                }
                //if (selected != null)
                //{
                //    Maximized = true;
                //}
            }
            //if (!MyTimerwork.IsBusy && selected != null)
            //{
            //    MyTimerwork.DoWork += new DoWorkEventHandler(MyTimer_Tick);
            //    MyTimerwork.RunWorkerAsync();
            //}
            //if (!MyCheckRequestTimerwork.IsBusy)
            //{
            //    MyCheckRequestTimerwork.DoWork += new DoWorkEventHandler(MyCheckRequestTimer_Tick);
            //    MyCheckRequestTimerwork.RunWorkerAsync();
            //}
            //});
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var LoginParam = new LoginParameters();
                    LoginParam.ByToken = false;
                    LoginParam.GetToken = true;
                    LoginParam.Email = txtEmail.Text;
                    LoginParam.Password = txtPassword.Text;
                    Email = txtEmail.Text;
                    var PostURI = URI + "/api/Authentication";
                    //client.DefaultRequestHeaders.Add("X-Real-IP", "192.168.1.180");
                    var serializedObj = JsonConvert.SerializeObject(LoginParam);
                    var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                    var resultData = await client.PostAsync(PostURI, contentData);
                    //resultData.Wait();
                    if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var Data = resultData.Content.ReadAsStringAsync().Result;
                        var MyResult = JsonConvert.DeserializeObject<UserData>(Data);
                        if (MyResult.CompanyLogins != null)
                        {
                            if (Environment == "DSV")
                            {
                                MyResult.CompanyLogins = MyResult.CompanyLogins.Where(a => a.PrivateLabelId != null).ToList();
                            }
                            else
                            {
                                MyResult.CompanyLogins = MyResult.CompanyLogins.Where(a => a.PrivateLabelId == null || a.HasLogboxAccess).ToList();
                            }
                        }
                        if (MyResult.IsLocked)
                        {
                            lblError.Text = "Your account has been locked out!";
                            lblError.Visible = true;
                            return;
                        }
                        else if (MyResult.CompanyLogins == null || MyResult.CompanyLogins.Count == 0)
                        {
                            lblError.Text = "Wrong user name or password !!";
                            lblError.Visible = true;
                            return;
                        }
                        else
                        {
                            lblError.Visible = false;
                        }
                        if (MyResult.CompanyLogins.Count == 1)
                        {
                            Tenant = MyResult.CompanyLogins.FirstOrDefault().Tenant;
                            Company = MyResult.CompanyLogins.FirstOrDefault().CompanyName;
                            var myserializedObj = JsonConvert.SerializeObject(LoginParam);
                            var mycontentData = new StringContent(myserializedObj, Encoding.UTF8, "application/json");
                            resultData = await client.PostAsync(PostURI + "?tenant=" + Tenant, mycontentData);
                            //resultData.Wait();
                            if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var MyData = resultData.Content.ReadAsStringAsync().Result;
                                var SubResult = JsonConvert.DeserializeObject<UserData>(Data);
                                Token = SubResult.Token;
                                await ContinueLoginProcess(Token, false);
                            }
                            else
                            {
                                lblError.Text = "Wrong user name or password !!";
                                lblError.Visible = true;
                            }
                        }
                        else
                        {
                            cobTenants.DataSource = MyResult.CompanyLogins;
                            cobTenants.DisplayMember = "CompanyName";
                            cobTenants.ValueMember = "Tenant";
                            var tempo = new AutoCompleteStringCollection();
                            foreach (var item in MyResult.CompanyLogins)
                            {
                                tempo.Add(item.CompanyName);
                            }
                            cobTenants.AutoCompleteCustomSource = tempo;
                            PNLLoginInfo.Hide();
                            PNLTenantInfo.Show();
                            btnLogin.Hide();
                            btnContinue.Show();
                        }

                    }
                    else
                    {
                        lblError.Text = "Wrong user name or password !!";
                        lblError.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                }

                errorMessage = errorMessage + ex.StackTrace;
                LogFileUtil.Log("btnLogin_Click" + errorMessage, LogFileUtil.LogLevel.All);
                if (Environment == "DSV")
                {
                    ni.Icon = Resources.dsvInActive;
                }
                else
                {
                    ni.Icon = Resources.logboxiconInActive;
                }

                ni.BalloonTipText = "The Server is not reachable.";
                if (MyMessage != DialogResult.OK)
                {
                    ni.ShowBalloonTip(1000);
                    //MyMessage = MessageBox.Show("The Server You are trying to connect is not reachable, please call your system administrator");
                }
            }
        }
        bool IsWorkerActive = false;
        private async Task ContinueLoginProcess(string token, bool ShowCerts = true)
        {
            PNLLoginInfo.Show();
            PNLTenantInfo.Hide();
            btnLogin.Show();
            btnContinue.Hide();
            var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            System.IO.Directory.CreateDirectory(MyPath + @"\Settings\");
            using (XmlWriter writer = XmlWriter.Create(MyPath + @"\Settings\Settings.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Settings");
                writer.WriteStartElement("Token");
                writer.WriteAttributeString("Value", token);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            if (MyConnection == null || MyConnection.State != Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            {
                ConnectAsync();
            }

            if (selected == null)
            {
                UpdateAppStatus(true, true, false);
                this.Hide();
                //MessageBox.Show("ContinueLoginProcess");
                //selected = selectCert(StoreName.My, StoreLocation.CurrentUser, "Available Certificates", "");
                //SignSamplePdf(out mystatus, out mymessage);
                if (ShowCerts)
                {
                    CheckStatus();
                }
                //

                if (selected == null)//|| mystatus == "-1"
                {
                    selected = null;
                    bool isExist = false;
                    foreach (Form frm in Application.OpenForms)
                    {
                        if (frm.Name == "InternalWindow")
                        {
                            isExist = true;
                            //var temp = frm as InternalWindow;

                            //temp.ShowMe(LastSigned);

                        }
                    }
                    if (!isExist)
                    {
                        InternalForm = new InternalWindow(selected, Email, this);
                        InternalForm.Show();
                    }
                    else
                    {
                        if (!MyTimerwork.IsBusy && selected != null)
                        {
                            //MessageBox.Show("MyTimer_Tick Timer");
                            MyTimerwork.DoWork += new DoWorkEventHandler(MyTimer_Tick);
                            MyTimerwork.RunWorkerAsync();
                        }
                        InternalForm.ShowMe(LastSigned, Email);
                    }
                }
            }

            if (selected != null)
            {
                UpdateAppStatus(true, true, true);
                bool isExist = false;
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "InternalWindow")
                    {
                        isExist = true;
                        //var temp = frm as InternalWindow;

                        //temp.ShowMe(LastSigned);

                    }
                }
                if (!isExist)
                {
                    InternalForm = new InternalWindow(selected, Email, this);
                    InternalForm.Show();
                }
                else
                {
                    InternalForm.ShowMe(LastSigned, Email);
                }


                this.Hide();

                //MessageBox.Show("before Timer");
                if (!MyCheckRequestTimerwork.IsBusy && !IsWorkerActive)
                {
                    //MessageBox.Show("MyCheckRequestTimer_Tick Timer");
                    MyCheckRequestTimerwork.DoWork += new DoWorkEventHandler(MyCheckRequestTimer_Tick);
                    MyCheckRequestTimerwork.RunWorkerAsync();
                    IsWorkerActive = true;
                }


                //MyTimer.Interval = (30000);
                //MyTimer.Tick += MyTimer_Tick;
                //MyTimer.Start();
                //MyCheckRequestTimer.Interval = 5000;
                //MyCheckRequestTimer.Tick += MyCheckRequestTimer_Tick;
                //MyCheckRequestTimer.Start();
            }
            if (!MyTimerwork.IsBusy)
            {
                //MessageBox.Show("MyTimer_Tick Timer");
                MyTimerwork.DoWork += new DoWorkEventHandler(MyTimer_Tick);
                MyTimerwork.RunWorkerAsync();
            }
        }

        private async void MyCheckRequestTimer_Tick(object sender, EventArgs e)
        {
            while (true)
            {
                var GetURI = URI + "/api/LogBoxSignatureClient/GetIfThereIsSignRequestByUserEmail?Email=" + Email + "&myTenant=" + Tenant;
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Token", Token);
                        if (selected != null)
                        {
                            var result = await client.GetAsync(GetURI);


                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                var Data = result.Content.ReadAsStringAsync().Result.Trim('"');
                                if (Data != "false")
                                {
                                    //if (DocsQueue.Count == 0 || DocsQueue.Where(a => a != Data).Count() > 0)
                                    //{
                                    await SignRequestRecieved();
                                    //}
                                }
                            }
                            else
                            {
                                var Data = result.Content.ReadAsStringAsync().Result;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message;

                    if (ex.InnerException != null)
                    {

                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                    }

                    errorMessage = errorMessage + ex.StackTrace;
                    LogFileUtil.Log("MyCheckRequestTimer_Tick" + errorMessage, LogFileUtil.LogLevel.All);
                    if (Environment == "DSV")
                    {
                        ni.Icon = Resources.dsvInActive;
                    }
                    else
                    {
                        ni.Icon = Resources.logboxiconInActive;
                    }

                    ni.BalloonTipText = "The Server is not reachable.";
                    if (MyMessage != DialogResult.OK)
                    {
                        ni.ShowBalloonTip(1000);
                        //MyMessage = MessageBox.Show("The Server You are trying to connect is not reachable, please call your system administrator");
                    }
                }
                System.Threading.Thread.Sleep(10000);  // Wait five minutes
            }
        }

        List<string> DocsQueue = new List<string>();

        public async Task SignRequestRecieved()
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    return;
                }
                if (selected == null)
                {
                    //MessageBox.Show("Please connect your smart card and try again ...");
                    return;
                }
                var GetURI = URI + "/api/LogBoxSignatureClient/GetDocumentDataBySignRequestUserEmail?Email=" + Email + "&myTenant=" + Tenant;

                //Uploader.Uploader up = new Uploader.Uploader();
                //var temp = up.DownloadFile("1-20680","pdf","",203);
                //string strMore = "", status = "", message = "";
                //SignPdf(temp, strMore, selected, out status, out message);
                using (var client = new HttpClient())
                {
                    if (string.IsNullOrEmpty(Token))
                    {
                        var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                        //XmlTextReader reader = new XmlTextReader(MyPath + @"\Settings\Settings.xml");
                        //var temp = reader.ReadContentAsString();
                        XmlDocument xmldoc = new XmlDocument();
                        xmldoc.Load(MyPath + @"\Settings\Settings.xml");

                        XmlNodeList TokenNode = xmldoc.GetElementsByTagName("Token");
                        Token = TokenNode[0].Attributes["Value"].Value;
                    }
                    client.DefaultRequestHeaders.Add("Token", Token);

                    var result = await client.GetAsync(GetURI);
                    //result.Wait(30000);
                    bool ThereAreMore = false;
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var Data = result.Content.ReadAsStringAsync().Result;

                        var jsonSettings = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects,
                            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
                        };

                        var MyResult = JsonConvert.DeserializeObject<DocumentData>(Data, jsonSettings);
                        do
                        {
                            string strMore = "", status = "", message = "";
                            if (MyResult.BinarryFile != null)
                            {
                                LastSigned = DateTime.Now;
                                //XmlDocument xmldoc = new XmlDocument();
                                //xmldoc.Load(MyPath + @"\Settings\Settings.xml");

                                //XmlNodeList TokenNode = xmldoc.GetElementsByTagName("Token");
                                //if (TokenNode.Count > 0)
                                //{
                                //    var tempToken = TokenNode[0].Attributes["Value"].Value;
                                var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                                System.IO.Directory.CreateDirectory(MyPath + @"\Settings\");
                                var temp = LastSigned.ToShortDateString() + " " + LastSigned.ToShortTimeString();
                                using (XmlWriter writer = XmlWriter.Create(MyPath + @"\Settings\StatusSettings.xml"))
                                {
                                    writer.WriteStartDocument();
                                    writer.WriteStartElement("StatusSettings");
                                    writer.WriteStartElement("LastSigned");
                                    writer.WriteAttributeString("Value", temp);
                                    writer.WriteEndElement();
                                    writer.WriteEndElement();
                                    writer.WriteEndDocument();
                                }
                                //XDocument xDocument = XDocument.Load(MyPath + @"\Settings\Settings.xml");
                                //XElement root = xDocument.Element("Settings");
                                //IEnumerable<XElement> rows = root.Descendants("Token");
                                //XElement firstRow = rows.First();
                                //var temp = LastSigned.ToShortDateString() + " " + LastSigned.ToShortTimeString();
                                //firstRow.AddBeforeSelf(
                                //   new XElement("LastSigned", temp));
                                //xDocument.Save(MyPath + @"\Settings\Settings.xml");
                                byte[] signedDocBinarryData = null;
                                //if (DocsQueue.Where(a => a == MyResult.DocumentFilingId).Count() == 0)
                                //{
                                //    DocsQueue.Add(MyResult.DocumentFilingId);
                                signedDocBinarryData = SignPdf(MyResult.BinarryFile, strMore, selected, out status, out message);
                                //}


                                #region SignLogic
                                if (signedDocBinarryData != null)
                                {
                                    int counter = -1;
                                    long sentBytes = 0;
                                    int position = 0;
                                    FileInfo = new FileInformation();
                                    FileInfo.DocumentId = MyResult.DocumentId;
                                    FileInfo.DocumentsFilingId = MyResult.DocumentFilingId;
                                    FileInfo.FileName = MyResult.FileName + "." + MyResult.Extention;
                                    FileInfo.Tenant = Tenant;
                                    FileInfo.FileSize = signedDocBinarryData.Length;
                                    blocksNumber = Math.Ceiling(Convert.ToDouble(signedDocBinarryData.Length) / buffersize);
                                    blockIdsArray = new List<string>();
                                    encodedFileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10).Replace('/', 'A').ToLower();
                                    while (sentBytes < signedDocBinarryData.Length)
                                    {

                                        counter++;
                                        int byteDifference2 = signedDocBinarryData.Length - Convert.ToInt32(sentBytes);
                                        if (byteDifference2 > buffersize)
                                        {
                                            currentData = new byte[buffersize];
                                            Buffer.BlockCopy(signedDocBinarryData, position, currentData, 0, buffersize);
                                        }
                                        else
                                        {
                                            currentData = new byte[byteDifference2];
                                            Buffer.BlockCopy(signedDocBinarryData, position, currentData, 0, byteDifference2);
                                        }

                                        blockId2 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                                        blockIdsArray.Add(blockId2);
                                        sentBytes += currentData.Length;
                                        position = Convert.ToInt32(sentBytes);
                                        value = (Convert.ToDouble(sentBytes) / Convert.ToDouble(signedDocBinarryData.Length)) * 100;
                                        FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                        FileInfo.buffer = currentData;
                                        FileInfo.SentSize = sentBytes;
                                        FileInfo.BufferNumber = counter;
                                        FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                        FileInfo.BlockIdsList = blockIdsArray.ToArray();
                                        var serializedObj = JsonConvert.SerializeObject(FileInfo);
                                        var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                        var resultData = await client.PostAsync(URI + "/api/LogBoxSignatureClient", contentData);
                                        //resultData.Wait();
                                        if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                                        {
                                            //string temp1 = resultData.Content.ReadAsStringAsync().Result;
                                            //NewDocumentFilingAM.DocumentId = JsonConvert.DeserializeObject<string>(temp1);
                                            //NewDocumentFilingAM.FileInfo.DocumentId = NewDocumentFilingAM.DocumentId;
                                        }
                                        else
                                        {
                                            //var temp1 = resultData.Content.ReadAsStringAsync().Result;
                                            //APIException EXC = JsonConvert.DeserializeObject<APIException>(temp1);
                                            //if (EXC != null)
                                            //{
                                            //    var Failmsg = EXC.ErrorType + " Fail To Send New Document To Importer " + DateTime.Now;
                                            //    APILogsUtility.UpdateAPILogStatus(LogPM.Id, tenant, "F", response.RetryNumber + 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(EXC), null, "");
                                            //    throw new Exception(EXC.ErrorType, new Exception(EXC.ErrorMessage));
                                            //}
                                        }

                                    }
                                    sentBytes = 0;
                                    position = 0;
                                    counter = -1;
                                    //DocsQueue.Remove(MyResult.DocumentFilingId);
                                } 
                                else
                                {
                                    //if (message == "PIN Code Is Required")
                                    //{
                                    //    UpdateAppStatus(true, true, false);
                                    //    selected = null;
                                    //    foreach (Form frm in Application.OpenForms)
                                    //    {
                                    //        if (frm.Name == "InternalWindow")
                                    //        {
                                    //            var mytemp = frm as InternalWindow;
                                    //            //temp.ShowMe();

                                    //            mytemp.PinCodeStatus(true,false,selected);
                                    //        }
                                    //    }
                                    //}
                                    //else //if (message == "InActive Cert" || mymessage.Contains("The chain context handle is invalid"))
                                    //{
                                    if (message == "PDF header signature not found")
                                    {
                                        if (Environment == "DSV")
                                        {
                                            ni.Icon = Resources.dsvInActive;
                                        }
                                        else
                                        {
                                            ni.Icon = Resources.logboxiconInActive;
                                        }

                                        ni.BalloonTipText = "The file you are trying to sign is corrupted.";
                                        ni.ShowBalloonTip(1500);
                                        LogFileUtil.Log("SignRequestRecieved-CurruptedFile " + MyResult.DocumentFilingId, LogFileUtil.LogLevel.All);
                                        if (!string.IsNullOrEmpty(MyResult.DocumentFilingId))
                                        {
                                            FileInfo = new FileInformation();
                                            FileInfo.DocumentId = MyResult.DocumentId;
                                            FileInfo.DocumentsFilingId = MyResult.DocumentFilingId;
                                            FileInfo.FileName = MyResult.FileName + "." + MyResult.Extention;
                                            FileInfo.Tenant = Tenant;
                                            FileInfo.FileSize = 0;
                                            var serializedObj = JsonConvert.SerializeObject(FileInfo);
                                            var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                            var resultData = client.PostAsync(URI + "/api/LogBoxSignatureClient", contentData);
                                            resultData.Wait();
                                            if (resultData.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                            {

                                            }
                                            else
                                            {

                                            }
                                        }
                                    }
                                    else if (message == "Rebuild failed")
                                    {
                                        if (!string.IsNullOrEmpty(MyResult.DocumentFilingId))
                                        {
                                            FileInfo = new FileInformation();
                                            FileInfo.DocumentId = MyResult.DocumentId;
                                            FileInfo.DocumentsFilingId = MyResult.DocumentFilingId;
                                            FileInfo.FileName = MyResult.FileName + "." + MyResult.Extention;
                                            FileInfo.Tenant = Tenant;
                                            FileInfo.FileSize = 0;
                                            var serializedObj = JsonConvert.SerializeObject(FileInfo);
                                            var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                            var resultData = client.PostAsync(URI + "/api/LogBoxSignatureClient", contentData);
                                            resultData.Wait();
                                            if (resultData.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                            {

                                            }
                                            else
                                            {

                                            }
                                        }
                                        MessageBox.Show("The PDF you are trying to sign is invalid.","Invalid PDF");
                                    }
                                    else if (message == "service down")
                                    {
                                        MessageBox.Show("The smart card service is down", "IDProtect Manager");
                                    }
                                    else
                                    {
                                        //UpdateAppStatus(true, true, false);
                                        if (!string.IsNullOrEmpty(MyResult.DocumentFilingId))
                                        {
                                            FileInfo = new FileInformation();
                                            FileInfo.DocumentId = MyResult.DocumentId;
                                            FileInfo.DocumentsFilingId = MyResult.DocumentFilingId;
                                            FileInfo.FileName = MyResult.FileName + "." + MyResult.Extention;
                                            FileInfo.Tenant = Tenant;
                                            FileInfo.FileSize = 0;
                                            var serializedObj = JsonConvert.SerializeObject(FileInfo);
                                            var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                            var resultData = client.PostAsync(URI + "/api/LogBoxSignatureClient", contentData);
                                            resultData.Wait();
                                            if (resultData.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                            {

                                            }
                                            else
                                            {

                                            }
                                        }
                                        if (Environment == "DSV")
                                        {
                                            ni.Icon = Resources.dsvInActive;
                                        }
                                        else
                                        {
                                            ni.Icon = Resources.logboxiconInActive;
                                        }

                                        ni.BalloonTipText = "An error occured during the sign process";
                                        ni.ShowBalloonTip(1500);
                                        MessageBox.Show("An error occured during the sign process, please try to disconnect the card and reconnect it again.", "Sign Faild");
                                        //foreach (Form frm in Application.OpenForms)
                                        //{
                                        //    if (frm.Name == "InternalWindow")
                                        //    {
                                        //        var mytemp = frm as InternalWindow;
                                        //        //temp.ShowMe();

                                        //        mytemp.PinCodeStatus(true, false);
                                        //    }
                                        //}
                                    }
                                    
                                    //}
                                    //else
                                    //{
                                    //    UpdateAppStatus(true, true, false);
                                    //    if (!string.IsNullOrEmpty(MyResult.DocumentFilingId))
                                    //    {
                                    //        FileInfo = new FileInformation();
                                    //        FileInfo.DocumentId = MyResult.DocumentId;
                                    //        FileInfo.DocumentsFilingId = MyResult.DocumentFilingId;
                                    //        FileInfo.FileName = MyResult.FileName + "." + MyResult.Extention;
                                    //        FileInfo.Tenant = Tenant;
                                    //        FileInfo.FileSize = 0;
                                    //        var serializedObj = JsonConvert.SerializeObject(FileInfo);
                                    //        var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                    //        var resultData = client.PostAsync(URI + "/api/LogBoxSignatureClient", contentData);
                                    //        resultData.Wait();
                                    //        if (resultData.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                    //        {

                                    //        }
                                    //        else
                                    //        {

                                    //        }
                                    //    }
                                    //}
                                }
                                #endregion

                                result = await client.GetAsync(GetURI);
                                //result.Wait();
                                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    Data = result.Content.ReadAsStringAsync().Result;
                                    MyResult = JsonConvert.DeserializeObject<DocumentData>(Data, jsonSettings);
                                    if ((MyResult.BinarryFile != null && selected != null))//&& DocsQueue.Where(a => a != MyResult.DocumentFilingId).Count() > 0) || DocsQueue.Count > 0)
                                    {
                                        ThereAreMore = true;
                                    }
                                    else
                                    {
                                        ThereAreMore = false;
                                    }
                                }
                            }
                            else
                            {
                                LogFileUtil.Log("SignRequestRecieved-Else " + MyResult.DocumentFilingId, LogFileUtil.LogLevel.All);
                                if (!string.IsNullOrEmpty(MyResult.DocumentFilingId))
                                {
                                    if (Environment == "DSV")
                                    {
                                        ni.Icon = Resources.dsvInActive;
                                    }
                                    else
                                    {
                                        ni.Icon = Resources.logboxiconInActive;
                                    }

                                    ni.BalloonTipText = "The file you are trying to sign is corrupted.";
                                    ni.ShowBalloonTip(1500);
                                    FileInfo = new FileInformation();
                                    FileInfo.DocumentId = MyResult.DocumentId;
                                    FileInfo.DocumentsFilingId = MyResult.DocumentFilingId;
                                    FileInfo.FileName = MyResult.FileName + "." + MyResult.Extention;
                                    FileInfo.Tenant = Tenant;
                                    FileInfo.FileSize = 0;
                                    var serializedObj = JsonConvert.SerializeObject(FileInfo);
                                    var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                                    var resultData = client.PostAsync(URI + "/api/LogBoxSignatureClient", contentData);
                                    resultData.Wait();
                                    if (resultData.Result.StatusCode == System.Net.HttpStatusCode.OK)
                                    {

                                    }
                                    else
                                    {

                                    }
                                }
                            }


                        } while (ThereAreMore);
                    }
                    else
                    {
                        //var resultXmal = result.Content.ReadAsStringAsync().Result;
                        //var apiException = JsonConvert.DeserializeObject<APIException>(resultXmal);
                        //lblMessage.Text = "Authentication Failed!";
                        //MessageBox.Show(apiException.ShortErrorMessage);

                    }


                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                }

                errorMessage = errorMessage + ex.StackTrace;
                LogFileUtil.Log("SignRequestRecieved" + errorMessage, LogFileUtil.LogLevel.All);
                if (Environment == "DSV")
                {
                    ni.Icon = Resources.dsvInActive;
                }
                else
                {
                    ni.Icon = Resources.logboxiconInActive;
                }

                ni.BalloonTipText = "The Server is not reachable.";
                ni.ShowBalloonTip(1000);
                //if (MyMessage != DialogResult.OK)
                //{

                //    MyMessage = MessageBox.Show("The Server You are trying to connect is not reachable, please call your system administrator");
                //}
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Minimize();
            this.Hide();
            //this.Close();
        }
        static bool IsCertOpened = false;// { get; set; }
        private X509Certificate2 selectCert(StoreName store, StoreLocation location, string windowTitle, string windowMsg)
        {
            try
            {

                X509Certificate2 certSelected = null;
                X509Store x509Store = new X509Store(store, location);
                x509Store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection col = x509Store.Certificates;//.Find(X509FindType.FindByIssuerName, "CN=ComSign Corporations CA, O=ComSign Ltd., C=IL", validOnly: true);
                X509Certificate2Collection col2 = new X509Certificate2Collection();
                foreach (var cert in col)
                {
                    if (!cert.Subject.ToLower().Contains("fiddler") && (cert.Issuer.ToLower().Contains("personal") || cert.Issuer.ToLower().Contains("comsign") || cert.Issuer.ToLower().Contains("rabaia")))// || cert.Issuer.ToLower().Contains("rabaia")
                    {
                        Debug.WriteLine(cert.Subject);
                        Debug.WriteLine(cert.Issuer + ";" + cert.IssuerName.Name + ";" + cert.GetExpirationDateString());
                        col2.Add(cert);
                        //cert.re
                        //MessageBox.Show(cert.GetCertHashString());
                    }

                }
                for (int i = 0; i < col.Count; i++)
                {
                    if (!col[i].Subject.ToLower().Contains("fiddler") && !col[i].Subject.ToLower().Contains("dropbox"))
                    {
                        Debug.WriteLine(col[i].Subject);
                        Debug.WriteLine(col[i].Issuer + ";" + col[i].IssuerName.Name + ";" + col[i].GetExpirationDateString());
                    }
                    else
                    {
                        col.Remove(col[i]);
                    }
                }
                //           if (col2.Count == 0)
                //           {
                //               DialogResult result2 = MessageBox.Show("Is Dot Net Perls awesome?",
                //"Important Query",
                //MessageBoxButtons.YesNoCancel,
                //MessageBoxIcon.Question);
                //           }
                //           else
                //           {

                //           }
                X509Certificate2Collection sel = null;
                CertCount = col2.Count;
                if (IsCertOpened == false)// && col2.Count > 0
                {
                    IsCertOpened = true;
                    sel = X509Certificate2UI.SelectFromCollection(col2, windowTitle, windowMsg, X509SelectionFlag.SingleSelection);
                }
                else
                {
                    return null;
                }
                IsCertOpened = false;

                if (sel.Count > 0)
                {
                    X509Certificate2Enumerator en = sel.GetEnumerator();
                    en.MoveNext();
                    certSelected = en.Current;
                }
                //X509Certificate2UI.DisplayCertificate(certSelected);

                x509Store.Close();
                //if (File.Exists(textDest.Text))
                //    File.Delete(textDest.Text);
                //string strMore = "", status = "", message = "";
                //SignPdf(textSource.Text, textDest.Text, strMore, certSelected, out status, out message);
                return certSelected;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                }

                errorMessage = errorMessage + ex.StackTrace;
                LogFileUtil.Log("selectCert" + errorMessage, LogFileUtil.LogLevel.All);
                return null;
            }


        }
        public static bool StopTimer = false;
        private static int CertCount = 0;
        private bool CheckValidCert(StoreName store, StoreLocation location, string windowTitle, string windowMsg)
        {
            try
            {
                X509Store x509Store = new X509Store(store, location);
                x509Store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection col = x509Store.Certificates;
                X509Certificate2Collection col2 = new X509Certificate2Collection();
                foreach (var cert in col)
                {
                    if (!cert.Subject.ToLower().Contains("fiddler") && (cert.Issuer.ToLower().Contains("personal") || cert.Issuer.ToLower().Contains("comsign") || cert.Issuer.ToLower().Contains("rabaia")))// || cert.Issuer.ToLower().Contains("rabaia")
                    {
                        Debug.WriteLine(cert.Subject);
                        Debug.WriteLine(cert.Issuer + ";" + cert.IssuerName.Name + ";" + cert.GetExpirationDateString());
                        col2.Add(cert);
                    }

                }
                for (int i = 0; i < col.Count; i++)
                {
                    if (!col[i].Subject.ToLower().Contains("fiddler") && !col[i].Subject.ToLower().Contains("dropbox"))
                    {
                        Debug.WriteLine(col[i].Subject);
                        Debug.WriteLine(col[i].Issuer + ";" + col[i].IssuerName.Name + ";" + col[i].GetExpirationDateString());
                    }
                    else
                    {
                        col.Remove(col[i]);
                    }
                }
                if (selected == null || col2.Count == 0 || (selected != null ? col2.Count < CertCount && !col2.Contains(selected) : false))//col2.Count == 0 || (selected != null ? col2.Count < CertCount && !col2.Contains(selected) : false)
                {
                    //selected = null; 
                    CertCount = col2.Count;
                    StopTimer = true;
                    return false;
                }
                else
                {
                    var status = false;
                    if (col2.Count > CertCount && selected == null && InternalForm != null && IsCertOpened == false)//col2.Count > CertCount && 
                    {
                        InternalForm.Minimize();
                        InternalForm.HideMe();
                        StopTimer = true;
                        selected = selectCert(StoreName.My, StoreLocation.CurrentUser, "Available Certificates", "");
                        InternalForm.ShowMe(null);
                        StopTimer = false;
                        if (!MyCheckRequestTimerwork.IsBusy && !IsWorkerActive)
                        {
                            MyCheckRequestTimerwork.DoWork += new DoWorkEventHandler(MyCheckRequestTimer_Tick);
                            MyCheckRequestTimerwork.RunWorkerAsync();
                            IsWorkerActive = true;
                        }
                    }
                    if (selected != null)
                    {
                        status = true;

                    }
                    CertCount = col2.Count;
                    return status;
                }

                //X509Certificate2Collection sel = X509Certificate2UI.SelectFromCollection(col2, windowTitle, windowMsg, X509SelectionFlag.SingleSelection);

                //if (sel.Count > 0)
                //{
                //    X509Certificate2Enumerator en = sel.GetEnumerator();
                //    en.MoveNext();
                //    certSelected = en.Current;
                //}
                ////X509Certificate2UI.DisplayCertificate(certSelected);

                //x509Store.Close();
                ////if (File.Exists(textDest.Text))
                ////    File.Delete(textDest.Text);
                ////string strMore = "", status = "", message = "";
                ////SignPdf(textSource.Text, textDest.Text, strMore, certSelected, out status, out message);
                //return certSelected;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                }

                errorMessage = errorMessage + ex.StackTrace;
                LogFileUtil.Log("CheckValidCert" + errorMessage, LogFileUtil.LogLevel.All);
                return false;
            }


        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to close the Sign app?", "Confirm",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question);

            //e.Cancel = (result == DialogResult.No);
            if (result == DialogResult.Yes)
            {
                var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                XmlDocument doc = new XmlDocument();
                doc.Load(MyPath + @"\Settings\Settings.xml");

                // find a node - here the one with name='abc'
                XmlNode node = doc.SelectSingleNode("/Settings/Token[@Value='" + Token + "']");

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
                UpdateAppStatus(false, false, false);
                PerformClosing = false;
                Application.Exit();
            }

        }
        bool PerformClosing = true;
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (PerformClosing == true)
            {
                //var result = MessageBox.Show("Are you suer you want to close the Sign app?", "Confirm",
                //            MessageBoxButtons.YesNo,
                //            MessageBoxIcon.Question);

                ////e.Cancel = (result == DialogResult.No);
                //if (result == DialogResult.Yes)
                //{
                base.OnFormClosing(e);

                if (e.CloseReason == CloseReason.WindowsShutDown || e.CloseReason == CloseReason.ApplicationExitCall) return;
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                Minimize();
                this.Hide();
                //}
            }
        }

        private byte[] SignPdf(byte[] SourceFile, string strMore, X509Certificate2 cert, out string status, out string message)
        {
            try
            {
                //throw new NotImplementedException();
                //' http://itextpdf.com/book/digitalsignatures20130304.pdf P51
                status = "";
                message = "";
                //'Dim pathToSignatureImage As String = "Sign6.png"
                var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                string pathToSignatureImage = MyPath + @"\Signature128.png";
                Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                IList<Org.BouncyCastle.X509.X509Certificate> chain = new List<Org.BouncyCastle.X509.X509Certificate>();
                X509Chain x509chain = new X509Chain();
                //x509chain.Build(cert)
                x509chain.Build(cert);
                foreach (X509ChainElement x509ChainElement in x509chain.ChainElements)
                {
                    chain.Add(cp.ReadCertificate(x509ChainElement.Certificate.RawData));
                }
                IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");
                //var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                System.IO.Directory.CreateDirectory(MyPath + @"\temp\");
                File.WriteAllBytes(MyPath + @"\temp\NotSigned.pdf", SourceFile);
                PdfReader pdfReader = new PdfReader(MyPath + @"\temp\NotSigned.pdf");
                PdfReader.unethicalreading = true;
                string strTargetSignedFilePath = (MyPath) + @"\temp\Signed.pdf";
                if ((File.Exists(strTargetSignedFilePath)))
                {
                    File.Delete(strTargetSignedFilePath);
                }
                FileStream signedPdf = new FileStream(strTargetSignedFilePath, FileMode.Create);

                //'Dim pdfStamper As PdfStamper = pdfStamper.CreateSignature(pdfReader, signedPdf, "\0")
                PdfStamper _pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0', null, true);

                PdfSignatureAppearance signatureAppearance = _pdfStamper.SignatureAppearance;
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    try
                    {

                        signatureAppearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(ApplicationDeployment.CurrentDeployment.DataDirectory + @"\Signature128.png");//MessageBox.Show(sr.ReadToEnd());

                    }
                    catch (Exception exx)
                    {
                        LogFileUtil.Log("SignPdf" + exx.Message, LogFileUtil.LogLevel.All);
                        MessageBox.Show("Could not read file. Error message: " + exx.Message);
                    }
                }
                else
                {
                    signatureAppearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(pathToSignatureImage);
                }
                List<string> _list = _pdfStamper.AcroFields.GetSignatureNames();
                int totalSign = 0;
                foreach (string item in _list)
                {
                    int signNum = 0;
                    if ((item.Contains("Signature-")))
                    {
                        int.TryParse(item.Remove(0, 10), out signNum);
                        if ((signNum > totalSign))
                        {
                            totalSign = signNum;
                        }
                    }
                }
                totalSign = totalSign + 1;
                //signatureAppearance.SetVisibleSignature(New Rectangle(100, 100, 250, 150), pdfReader.NumberOfPages, "Signature")
                //Dim rec As New iTextSharp.text.Rectangle(100, 100, 250, 150)
                float hSize = pdfReader.GetPageSize(1).Height;
                float wSize = pdfReader.GetPageSize(1).Width;
                //'Dim rec As New iTextSharp.text.Rectangle(wSize - 105, hSize - 125, wSize - 25, hSize - 25)
                iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(wSize - 350, hSize - 64, wSize - 250, hSize);
                iTextSharp.text.Rectangle currentPageRectangle = pdfReader.GetPageSizeWithRotation(1);
                if (currentPageRectangle.Width > currentPageRectangle.Height)
                {
                    //page is landscape
                    rec = new iTextSharp.text.Rectangle(wSize - 350, hSize - 420, wSize - 250, hSize);
                    //rec = new iTextSharp.text.Rectangle(wSize - 150, hSize - 150, wSize - 200, hSize - 200);
                    //rec = new iTextSharp.text.Rectangle(currentPageRectangle.Width - 150, currentPageRectangle.Height - 150, currentPageRectangle.Width - 200, currentPageRectangle.Height - 200);
                }
                //else
                //{
                //    //page is portrait
                //    rec = new iTextSharp.text.Rectangle(wSize - 350, hSize - 420, wSize - 250, hSize);
                //}
                //'Dim rec As New iTextSharp.text.Rectangle(wSize - 75, hSize - 75, wSize - 25, hSize - 25)
                signatureAppearance.SetVisibleSignature(rec, 1, "Signature-" + totalSign.ToString());
                //Dim v1, v2, v3, v4 As Integer
                //v1 = 100
                //v2 = 100
                //v3 = 250
                //v4 = 150

                //v3 = 100
                //v4 = 100
                //v2 = CType(pdfReader.GetPageSize(1).Height, Integer)
                //signatureAppearance.SetVisibleSignature(New Rectangle(v1, v2, v3, v4), pdfReader.NumberOfPages, "Signature")
                //signatureAppearance.SetVisibleSignature(New Rectangle(0, 0), pdfReader.NumberOfPages, "Signature")
                //'Dim aaa As Rectangle = pdfReader.GetPageSize(1)

                //'signatureAppearance.SetVisibleSignature(New Rectangle(0, aaa.Height + 100, 100, 200), pdfReader.NumberOfPages, "Signature")


                //'signatureAppearance.SetVisibleSignature(New Rectangle(0, 2575, 64, 64), 1, "Signature")


                //'signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION
                signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
                lock (_Obj)
                {
                    MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
                }
                var tempo = File.ReadAllBytes(MyPath + @"\temp\Signed.pdf");
                return tempo;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The action was cancelled by the user"))
                {
                    message = "PIN Code Is Required";
                }
                else if (ex.Message.Contains("Keyset does not exist"))
                {
                    message = "InActive Cert";
                }
                else if (ex.Message.Contains("PDF header signature not found"))
                {
                    message = "PDF header signature not found";
                }
                else if (ex.Message.Contains("Rebuild failed"))
                {
                    message = "Rebuild failed";
                }
                else if (ex.Message.Contains("Provider DLL failed to initialize correctly"))
                {
                    message = "service down";
                }
                else
                {
                    message = ex.Message;// "SignPdf:Exception:" + ex.Message + " " + ex.ToString();
                }
                status = "-1";
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                }

                errorMessage = errorMessage + ex.StackTrace;
                LogFileUtil.Log("SignPdf 1" + errorMessage, LogFileUtil.LogLevel.All);
                //message = "SignPdf:Exception:" + ex.Message + " " + ex.ToString();
                //status = "-1";
                return null;
            }
        }
        public void ShowDisconnectMessage()
        {
            this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                MessageBox.Show(this, "You have been disconnected.");

            });
        }
        private void ConnectAsync()
        {
            //MyConnection = new HubConnection(URI, new Dictionary<string, string> { { "UserName", Email } });
            //MyConnection.Closed += //Connection_Closed;
            //    () =>
            //    {
            //        InternalForm.CloseMe();
            //        ShowDisconnectMessage();
            //        foreach (Form frm in Application.OpenForms)
            //        {
            //            if (frm.Name == "InternalWindow")
            //            {
            //                var temp = frm as InternalWindow;
            //                //temp.ShowMe();
            //                temp.TopMost = true;
            //                temp.ConnectionStateChanged();
            //                //temp.Show();
            //                //temp.Close();
            //            }
            //            //if (frm.Name == "MainWindow")
            //            //{
            //            //    var temp = frm as MainWindow;
            //            //    temp.ShowDisconnectMessage();
            //            //}
            //        }
            //    };
            //HubProxy = MyConnection.CreateHubProxy("LogBoxSignatureHub");
            //MyConnection.StateChanged += MyConnection_StateChanged;
            //HubProxy.On("NewSignRequestReceived", x => SignRequestRecieved());
            //HubProxy.On("Disconnected", x => Disconnected());
            //try
            //{
            //    MyConnection.Start().Wait();
            //}
            //catch (HttpRequestException)
            //{
            //    MessageBox.Show("Unable to connect to server: Start server before connecting clients.");
            //    MyConnection = null;
            //    return;
            //}
        }

        private void MyConnection_StateChanged(StateChange obj)
        {

        }

        public void Disconnected()
        {
        }

        private void cobTenants_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Tenant = (int)cobTenants.SelectedValue;
                Company = ((CompanyLogin)cobTenants.SelectedItem).CompanyName;

            }
            catch (Exception)
            {
                Tenant = ((CompanyLogin)cobTenants.SelectedValue).Tenant;
                Company = ((CompanyLogin)cobTenants.SelectedValue).CompanyName;
            }

        }
        public void ClearLoginCred()
        {
            txtPassword.Text = "";
            txtEmail.Text = "";
        }
        private async void btnContinue_Click(object sender, EventArgs e)
        {

            using (var client = new HttpClient())
            {
                var LoginParam = new LoginParameters();
                LoginParam.ByToken = false;
                LoginParam.GetToken = true;
                LoginParam.Email = Email;
                LoginParam.Password = txtPassword.Text;
                LoginParam.IsUser = true;
                var PostURI = URI + "/api/Authentication";
                //client.DefaultRequestHeaders.Add("X-Real-IP", "192.168.1.180");
                var serializedObj = JsonConvert.SerializeObject(LoginParam);
                var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                var resultData = client.PostAsync(PostURI + "?tenant=" + Tenant, contentData);
                resultData.Wait();
                if (resultData.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var Data = resultData.Result.Content.ReadAsStringAsync().Result;
                    var MyResult = JsonConvert.DeserializeObject<UserData>(Data);
                    Token = MyResult.Token;
                    await ContinueLoginProcess(Token);
                }
                else
                {
                    lblError.Text = "Wrong user name or password !!";
                    lblError.Visible = true;
                }
            }
        }
        private void ClearToken()
        {
            var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            XmlDocument doc = new XmlDocument();
            doc.Load(MyPath + @"\Settings\Settings.xml");
            XmlNodeList TokenNode = doc.GetElementsByTagName("Token");
            string tempToken = "";
            if (TokenNode.Count > 0)
            {
                tempToken = TokenNode[0].Attributes["Value"].Value;
            }
            // find a node - here the one with name='abc'
            XmlNode node = doc.SelectSingleNode("/Settings/Token[@Value='" + tempToken + "']");

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
        }
        private async Task LoginUsingToken()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(15);
                    var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                    XmlDocument xmldoc = new XmlDocument();
                    try
                    {
                        xmldoc.Load(MyPath + @"\Settings\Settings.xml");
                    }
                    catch (Exception)
                    {
                        using (XmlWriter writer = XmlWriter.Create(MyPath + @"\Settings\Settings.xml"))
                        {
                            writer.WriteStartDocument();
                            writer.WriteStartElement("Settings");
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                        }
                    }


                    XmlNodeList TokenNode = xmldoc.GetElementsByTagName("Token");
                    if (TokenNode.Count > 0)
                    {
                        var tempToken = TokenNode[0].Attributes["Value"].Value;
                        var LoginParam = new LoginTokenParameter();
                        LoginParam.IsMobileLogin = false;
                        LoginParam.Token = tempToken;
                        var PostURI = URI + "/api/Authentication";
                        //client.DefaultRequestHeaders.Add("X-Real-IP", "192.168.1.180");
                        var serializedObj = JsonConvert.SerializeObject(LoginParam);
                        var contentData = new StringContent(serializedObj, Encoding.UTF8, "application/json");
                        var resultData = await client.PostAsync(PostURI + "/PostTrayLoginUsingAuthenticaionToken?fromTray=true&useTenant=true", contentData);
                        //resultData.Wait(50);
                        if (resultData.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var Data = resultData.Content.ReadAsStringAsync().Result;
                            var MyResult = JsonConvert.DeserializeObject<UserData>(Data);
                            if (!MyResult.HasError)
                            {
                                Tenant = MyResult.CurrentTenant;
                                Email = MyResult.UserName;
                                Token = tempToken;
                                Company = (MyResult.CompanyLogins != null) ? MyResult.CompanyLogins.Where(a => a.Tenant == Tenant).FirstOrDefault().CompanyName : "";
                            }
                            else
                            {
                                Tenant = -1;
                                Token = null;
                                ClearToken();
                                Token = null;
                                //await Maximize();
                            }

                        }
                        else
                        {
                            Tenant = -1;
                            Token = null;
                            ClearToken();
                            //await Maximize();
                        }
                    }
                    else
                    {
                        Tenant = -1;
                        Token = null;
                        ClearToken();
                        //await Maximize();

                    }

                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {

                    errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                }

                errorMessage = errorMessage + ex.StackTrace;
                LogFileUtil.Log("LoginUsingToken " + errorMessage, LogFileUtil.LogLevel.All);
                Tenant = -1;
                Token = null;
                ClearToken();
                this.WindowState = FormWindowState.Normal;
                //UpdateAppStatus(false, false, false);
                //MyTimerwork.Dispose();//.Stop();
                //MyCheckRequestTimerwork.Dispose();//.Stop();
                this.selected = null;

            }
        }

        public void SignSamplePdf(out string status, out string message)
        {
            try
            {
                X509Certificate2 cert;
                if (selected == null)
                {
                    UpdateAppStatus(true, true, false);
                    //MessageBox.Show("SignSamplePdf");
                    selected = selectCert(StoreName.My, StoreLocation.CurrentUser, "Available Certificates", "");
                }

                cert = selected;
                status = "";
                message = "";

                var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                string pathToSignatureImage = MyPath + @"\Signature128.png";
                Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
                IList<Org.BouncyCastle.X509.X509Certificate> chain = new List<Org.BouncyCastle.X509.X509Certificate>();
                X509Chain x509chain = new X509Chain();

                x509chain.Build(cert);
                foreach (X509ChainElement x509ChainElement in x509chain.ChainElements)
                {
                    chain.Add(cp.ReadCertificate(x509ChainElement.Certificate.RawData));
                }

                IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");
                //var MyPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                //System.IO.Directory.CreateDirectory(MyPath + @"\temp\");
                string SourcePath = MyPath + @"\temp\samplepdf.pdf";
                if (!File.Exists(SourcePath))
                {
                    //MessageBox.Show("Not");
                    //Create document

                    Document doc = new Document();
                    //Create PDF Table

                    PdfPTable tableLayout = new PdfPTable(4);



                    //Create a PDF file in specific path

                    PdfWriter.GetInstance(doc, new FileStream(SourcePath, FileMode.Create));



                    //Open the PDF document

                    doc.Open();



                    //Add Content to PDF

                    doc.Add(Add_Content_To_PDF(tableLayout));



                    // Closing the document

                    doc.Close();
                }
                PdfReader pdfReader = new PdfReader(SourcePath);
                string strTargetSignedFilePath = (MyPath) + @"\temp\samplesignedpdf.pdf";
                if ((File.Exists(strTargetSignedFilePath)))
                {
                    try
                    {
                        File.Delete(strTargetSignedFilePath);
                    }
                    catch (Exception)
                    {

                    }
                }

                //FileStream signedPdf = new FileStream(strTargetSignedFilePath, FileMode.Create);
                //FileStream signedPdf;
                using (FileStream signedPdf = new FileStream(strTargetSignedFilePath, FileMode.Create))
                {
                    PdfStamper _pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0', null, true);

                    PdfSignatureAppearance signatureAppearance = _pdfStamper.SignatureAppearance;
                    if (ApplicationDeployment.IsNetworkDeployed)
                    {
                        try
                        {
                            signatureAppearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(ApplicationDeployment.CurrentDeployment.DataDirectory + @"\Signature128.png");//MessageBox.Show(sr.ReadToEnd());

                        }
                        catch (Exception exx)
                        {
                            MessageBox.Show("Could not read file. Error message: " + exx.Message);
                        }
                    }
                    else
                    {
                        signatureAppearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(pathToSignatureImage);
                    }
                    List<string> _list = _pdfStamper.AcroFields.GetSignatureNames();
                    int totalSign = 0;
                    foreach (string item in _list)
                    {
                        int signNum = 0;
                        if ((item.Contains("Signature-")))
                        {
                            int.TryParse(item.Remove(0, 10), out signNum);
                            if ((signNum > totalSign))
                            {
                                totalSign = signNum;
                            }
                        }
                    }
                    totalSign = totalSign + 1;

                    float hSize = pdfReader.GetPageSize(1).Height;
                    float wSize = pdfReader.GetPageSize(1).Width;

                    iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(wSize - 89, hSize - 89, wSize - 25, hSize - 25);

                    signatureAppearance.SetVisibleSignature(rec, 1, "Signature-" + totalSign.ToString());

                    signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC;
                    lock (_Obj)
                    {
                        MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
                    }
                    //if (MyTimerwork..Enabled == false)
                    //{
                    //    MyTimer.Start();
                    //}
                    if (!MyTimerwork.IsBusy)
                    {
                        MyTimerwork.DoWork += new DoWorkEventHandler(MyTimer_Tick);
                        MyTimerwork.RunWorkerAsync();
                    }
                }
                //MessageBox.Show("Finish SignSamplePdf");

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The action was cancelled by the user"))
                {
                    message = "PIN Code Is Required";
                }
                else if (ex.Message.Contains("Keyset does not exist"))
                {
                    message = "InActive Cert";
                }
                else if (ex.Message.Contains("Provider DLL failed to initialize correctly"))
                {
                    message = "service down";
                }
                else
                {
                    string errorMessage = ex.Message;

                    if (ex.InnerException != null)
                    {

                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")";

                    }

                    errorMessage = errorMessage + ex.StackTrace;
                    LogFileUtil.Log("SignSamplePdf " + errorMessage, LogFileUtil.LogLevel.All);
                    message = ex.Message;// "SignPdf:Exception:" + ex.Message + " " + ex.ToString();
                }
                status = "-1";
                //MessageBox.Show("EX SignSamplePdf" + ex.Message);

            }
        }

        private PdfPTable Add_Content_To_PDF(PdfPTable tableLayout)

        {

            float[] headers = { 20, 20, 30, 30 };  //Header Widths

            tableLayout.SetWidths(headers);        //Set the pdf headers

            tableLayout.WidthPercentage = 80;       //Set the PDF File witdh percentage



            //Add Title to the PDF file at the top

            tableLayout.AddCell(new PdfPCell(new Phrase("Creating PDF file using iTextsharp", new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 13, 1, new BaseColor(153, 51, 0)))) { Colspan = 4, Border = 0, PaddingBottom = 20, HorizontalAlignment = Element.ALIGN_CENTER });



            //Add header

            AddCellToHeader(tableLayout, "Cricketer Name");

            AddCellToHeader(tableLayout, "Height");

            AddCellToHeader(tableLayout, "Born On");

            AddCellToHeader(tableLayout, "Parents");



            //Add body

            AddCellToBody(tableLayout, "Sachin Tendulkar");

            AddCellToBody(tableLayout, "1.65 m");

            AddCellToBody(tableLayout, "April 24, 1973");

            AddCellToBody(tableLayout, "Ramesh Tendulkar, Rajni Tendulkar");



            AddCellToBody(tableLayout, "Mahendra Singh Dhoni");

            AddCellToBody(tableLayout, "1.75 m");

            AddCellToBody(tableLayout, "July 7, 1981");

            AddCellToBody(tableLayout, "Devki Devi, Pan Singh");



            AddCellToBody(tableLayout, "Virender Sehwag");

            AddCellToBody(tableLayout, "1.70 m");

            AddCellToBody(tableLayout, "October 20, 1978");

            AddCellToBody(tableLayout, "Aryavir Sehwag, Vedant Sehwag");



            AddCellToBody(tableLayout, "Virat Kohli");

            AddCellToBody(tableLayout, "1.75 m");

            AddCellToBody(tableLayout, "November 5, 1988");

            AddCellToBody(tableLayout, "Saroj Kohli, Prem Kohli");



            return tableLayout;

        }



        // Method to add single cell to the header

        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 8, 1, BaseColor.WHITE))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = new BaseColor(0, 51, 102) });

        }



        // Method to add single cell to the body

        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {

            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 8, 1, BaseColor.BLACK))) { HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5, BackgroundColor = BaseColor.WHITE });

        }

    }

    public class DocumentData
    {
        public byte[] BinarryFile { get; set; }
        public string DocumentId { get; set; }
        public string Extention { get; set; }
        public string DocumentFilingId { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string FileName { get; set; }
    }
    public class FileInformation
    {
        public string ShipmentNumber { get; set; }
        public string Key { get; set; }
        public int FileSize { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string[] BlockIdsList { get; set; }
        public byte[] buffer { get; set; }
        public int BufferNumber { get; set; }
        public long SentSize { get; set; }
        public string FileName { get; set; }
        public string ObjectTableId { get; set; }
        public string DocumentTypeId { get; set; }
        public string ShipmentId { get; set; }
        public string UserId { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentId { get; set; }

    }
}
