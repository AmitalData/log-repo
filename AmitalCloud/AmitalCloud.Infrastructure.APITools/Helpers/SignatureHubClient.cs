using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using AmitalCloud.Infrastructure.Domain.Enums;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.APITools.Helpers
{
    public class SignatureHubClient
    {
        static SignatureHubClient _Instance;

        public static SignatureHubClient Instance
        {
            get
            {
                if (_Instance == null)
                {

                    throw new Exception("Object not created : use  SignatureHubClient.Create");
                }
                return _Instance;
            }

        }

        public static void Create(string ServerURI, Action<string, string> message4U)
        {
            if (_Instance != null)
            {
                throw new Exception("Object already created");
            }
            //NetCommonHelper.Logger.DevLog.Instance.WriteInfo("ServerURI:" + ServerURI);
            _Instance = new SignatureHubClient(ServerURI, message4U);
            try
            {
                _Instance.WakeUp(true);
            }
            catch (Exception eee)
            {
                ExceptionHandler.HandleException(eee, DateTime.Now, 0, "", "WebRole", "SignalRStartup : Configuration", null);
                ///throw;
            }


        }


        private String UserName { get; set; }
        private IHubProxy HubProxy { get; set; }
        string _ServerURI;
        Action<string, string> _Message4U;
        private DateTime? _LastCrashAt;

        private HubConnection MyConnection { get; set; }

        SignatureHubClient(string serverURI, Action<string, string> message4U)
        {

            _ServerURI = serverURI + "/signalr";
            _Message4U = message4U;
            //ConnectAsync();
            //return;
        }


        //"http://localhost:32651/signalr";
        //"http://localhost:8080/signalr";
        //"http://itzik7:5058/Unifright/DcaSignalr";
        //@"http://localhost:5058/Unifright/DcaSignalr";

        public bool IsActive { get; set; }
        public string LastError { get; set; }

        private async void ConnectAsync()
        {
            //if (MyConnection != null)
            //{
            //    MyConnection.Dispose();
            //    MyConnection = null;
            //}

            MyConnection = new HubConnection(_ServerURI);
            MyConnection.Closed += //Connection_Closed;
                () =>
                {
                    //Deactivate chat UI; show login UI. 

                    //this.Invoke((Action)(() => StatusText.Text = "You have been disconnected."));
                    LastError = "You have been disconnected.";
                    MyConnection = null;
                    IsActive = false;
                };
            HubProxy = MyConnection.CreateHubProxy(
                //"MyHub"
                "SignatureHub"
                );
            //Handle incoming event from server: use Invoke to write to console from SignalR's thread
            HubProxy.On<string, string>("Message4U", (name, tenant) =>
            {
                if (_Message4U != null)
                {
                    _Message4U(name, tenant);
                }
            }

           );
            try
            {
                await MyConnection.Start();
                IsActive = true;
            }
            catch (HttpRequestException)
            {
                LastError = "Unable to connect to server: Start server before connecting clients.";
                IsActive = false;
                MyConnection = null;
                //No connection: Don't enable Send button or show chat UI
                return;
            }

            //Activate UI

            //RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + Environment.NewLine);
        }

        public void WakeUp(bool forceReconnect = false)
        {
            bool RestartSignlarWhenCrash = ConfigurationManager.AppSettings["20181212.RestartSignlarWhenCrash"] == "1";//TFCSendSignCrushButSuccesssInBackground
            if (RestartSignlarWhenCrash)
            {
                if (_LastCrashAt.HasValue)
                {
                    if (DateTime.Now.Subtract(_LastCrashAt.GetValueOrDefault()) > TimeSpan.FromMinutes(15))
                    {
                        forceReconnect = true;
                        _LastCrashAt = null;
                    }
                }
            }
            if (!IsActive || forceReconnect)
            {

                var t = Task.Factory.StartNew(() =>
                {
                    if (MyConnection != null)
                    {
                        MyConnection.Dispose();
                        MyConnection = null;
                        IsActive = false;
                    }
                    ConnectAsync();
                });
                t.Wait();

            }

        }

        public void Send(SignQueueByType requestMessageType, string requestMessageData)
        {
            //QueueCode="
            try
            {
                if (String.IsNullOrWhiteSpace(requestMessageData))
                {
                    return;
                }
                if (HubProxy != null && MyConnection != null && MyConnection.State == ConnectionState.Connected)
                {

                    HubProxy.Invoke("Send", requestMessageType.ToString(), requestMessageData);
                }
                _LastCrashAt = null;
            }
            catch (Exception)
            {
                if (!_LastCrashAt.HasValue)
                {
                    _LastCrashAt = DateTime.Now;
                }
                throw;
            }
        }

        public static void SafeSend(string AmitalURL, string WorkEnvironment, SignQueueByType requestMessageType, string requestMessageData)
        {
            if (!IsSafe(AmitalURL, WorkEnvironment)) return;
            Instance.Send(requestMessageType, requestMessageData);
        }

        private static bool IsSafe(string AmitalURL, string WorkEnvironment)
        {
            string host = GetHost(AmitalURL);
            return !string.IsNullOrWhiteSpace(host);
        }
        public static void SafeWakeUp(string AmitalURL, string WorkEnvironment)
        {
            if (!IsSafe(AmitalURL, WorkEnvironment)) return;
            Instance.WakeUp();
        }

        public static string GetHost(string AmitalURL)
        {

            if (!AmitalCloudSettings.IsCostomsDeploy)
            {
                return "";
            }
            var host = "";

            var itzikhost = "";
            //string SuppresSignatureHubClient = System.Configuration.ConfigurationManager.AppSettings.Get("SuppresSignatureHubClient");
            //if (!string.IsNullOrWhiteSpace(SuppresSignatureHubClient))
            //{
            //    return "";
            //}
            var RabaiaHost = "";
            if (DateTime.Now < new DateTime(2017, 02, 28) &&
                //Debugger.IsAttached && 
                Environment.MachineName.StartsWith("itzik", StringComparison.OrdinalIgnoreCase))
            {
                itzikhost = //uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                    @"http://localhost:62619/";
                return "";
            }
            else if (DateTime.Now < new DateTime(2017, 06, 01) && Environment.MachineName.StartsWith("DEVELOPER-6-PC", StringComparison.OrdinalIgnoreCase))//Rabaia PC
            {
                RabaiaHost = @"http://localhost:9996/";
            }
            else
            {
                if (!Environment.CommandLine.ToLower().Contains("w3wp.exe")) return "";
            }

            // Any connection or hub wire up and configuration should go here


            host = AmitalURL;
            var uri = new Uri(host);
            host = host.Replace(uri.Host, "localhost");
            if (!string.IsNullOrWhiteSpace(itzikhost))
            {
                host = itzikhost;
            }
            if (!string.IsNullOrWhiteSpace(RabaiaHost))
            {
                host = RabaiaHost;
            }
            return host;
        }

    }

}
