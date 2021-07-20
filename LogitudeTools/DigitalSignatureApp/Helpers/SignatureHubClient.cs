
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cloud.Sign.App.Helpers
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
            _Instance = new SignatureHubClient(ServerURI, message4U);
            try
            {
                _Instance.WakeUp(true);
            }
            catch (Exception)
            {
                //ExceptionHandler.HandleException(eee, DateTime.Now, 0, "", "WebRole", "SignalRStartup : Configuration", null);
                ///throw;
            }


        }


        private String UserName { get; set; }
        private IHubProxy HubProxy { get; set; }
        string _ServerURI;
        Action<string, string> _Message4U;
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

        //private async void ConnectAsync()
        //{
           
        //    MyConnection = new HubConnection(_ServerURI);
        //    MyConnection.Closed += //Connection_Closed;
        //        () =>
        //        {
        //            //Deactivate chat UI; show login UI. 

        //            //this.Invoke((Action)(() => StatusText.Text = "You have been disconnected."));
        //            LastError = "You have been disconnected.";
        //            MyConnection = null;
        //            IsActive = false;
        //        };
        //    HubProxy = MyConnection.CreateHubProxy(
        //        //"MyHub"
        //        "SignatureHub"
        //        );
        //    //Handle incoming event from server: use Invoke to write to console from SignalR's thread
        //    HubProxy.On<string, string>("Message4U", (name, tenant) =>
        //    {
        //        if (_Message4U != null)
        //        {
        //            _Message4U(name, tenant);
        //        }
        //    }

        //   );
        //    try
        //    {
        //        await MyConnection.Start();
        //        IsActive = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        LastError = "Unable to connect to server: Start server before connecting clients.";
        //        IsActive = false;
        //        MyConnection = null;
        //        //No connection: Don't enable Send button or show chat UI
        //        return;
        //    }

        //    //Activate UI

        //    //RichTextBoxConsole.AppendText("Connected to server at " + ServerURI + Environment.NewLine);
        //}

        public void WakeUp(bool forceReconnect = false)
        {

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
                    //ConnectAsync();
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
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void SafeSend(string LogitudeURL, string WorkEnvironment, SignQueueByType requestMessageType, string requestMessageData)
        {
            if (!IsSafe(LogitudeURL, WorkEnvironment)) return;
            Instance.Send(requestMessageType, requestMessageData);
        }

        private static bool IsSafe(string LogitudeURL, string WorkEnvironment)
        {
            string host = GetHost(LogitudeURL);
            return !string.IsNullOrWhiteSpace(host);
        }
        public static void SafeWakeUp(string LogitudeURL, string WorkEnvironment)
        {
            if (!IsSafe(LogitudeURL, WorkEnvironment)) return;
            Instance.WakeUp();
        }

        public static string GetHost(string LogitudeURL)
        {

            //if (!LogitudeSettings.IsCostomsDeploy)
            //{
            //    return "";
            //}
            var host = "";

            var itzikhost = "";
            if (DateTime.Now < new DateTime(2016, 03, 28) &&
                //Debugger.IsAttached && 
                Environment.MachineName.StartsWith("itzik", StringComparison.OrdinalIgnoreCase))
            {
                itzikhost = //uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                    @"http://localhost:62619/";
            }
            else
            {
                if (!Environment.CommandLine.ToLower().Contains("w3wp.exe")) return "";
            }

            // Any connection or hub wire up and configuration should go here


            host = LogitudeURL;
            var uri = new Uri(host);
            host = host.Replace(uri.Host, "localhost");
            if (!string.IsNullOrWhiteSpace(itzikhost))
            {
                host = itzikhost;
            }
            return host;
        }

    }
    public enum SignQueueByType
    {
        None = 0,
        SignQueueByCustomsAgentId,
        SignQueueByPersonId
    }
}
