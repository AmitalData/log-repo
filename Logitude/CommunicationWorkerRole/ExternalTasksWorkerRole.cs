using System.Linq;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Net;
using System;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using Logitude.Server.Tools;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using Logitude.BL.ShipmentsModel.EntityAMs;
using WebFreight.Web.Helpers;
using System.Net.Http;
using System.Text;
using WebFreight.Web.DataContracts;
using System.Security.Cryptography;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace CommunicationWorkerRole
{
    class ExternalTasksWorkerRole : WorkerEntryPoint
    {
        string Token = "";
        public override async void AsyncRun()
        {
            Token = Login("admin@fnarsoft.com","1");
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        LastActivity = DateTime.UtcNow;
                        //string url = LogitudeSettings.LogitudeURL;//ConfigurationManager.AppSettings.Get("LogitudeURL");
                        //url = url + "/wcfapi/ExternalTasksQueueWcfService.svc";
                        //url = url.Replace("https", "http");
                        Envelope result;
                        using (var Service = new ExternalTasksQueueWcfService.ExternalTasksQueueWcfServiceClient())
                        { 
                            //Service.InnerChannel.h.Add("Token", Token);
                            var xx = await Service.GetTaskFromQueueAsync(1, 1);
                            XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
                            using (TextReader reader = new StringReader(xx))
                            {
                                result = (Envelope)serializer.Deserialize(reader);
                            }
                            var temp = result.Tasks.Where(a => a.Action == "NewImporterShipment").FirstOrDefault();
                            if (temp != null)
                            {
                                var param = temp.Parameters.Where(a => a.Name == "ImporterShipment").FirstOrDefault();
                                if (param != null)
                                {
                                    ShipmentAM ship = JsonConvert.DeserializeObject<ShipmentAM>(param.Value);
                                    ShipmentWcfServiceReference.ShipmentWcfServiceClient shipmentservice = new ShipmentWcfServiceReference.ShipmentWcfServiceClient();
                                    using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)shipmentservice.InnerChannel))
                                    {
                                        System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", Token);
                                        string shipmentnumber = "myship7712";//"CUSTOMF1";//"MYHYBRIDSHIP3";//Guid.NewGuid().ToString().Substring(0, 15);
                                        ShipmentWcfServiceReference.ShipmentPM pm = new ShipmentWcfServiceReference.ShipmentPM()
                                        {
                                            DirectionId = ship.DirectionId,//"C",
                                            ShipmentLevelCode = ship.ShipmentLevelCode,//"A",
                                            ShipmentTypeId = "Cons",//ship.ShipmentTypeId,//null, 
                                            Tenant = 1,
                                            AccessDate = DateTime.Now,
                                            AWBCurrencyId = "USD",
                                            BranchId = string.IsNullOrEmpty(ship.Branch.Code) ? "HybridB1" : "HybridB1",//ship.Branch.Code,
                                            DepartmentId = string.IsNullOrEmpty(ship.Department.Code) ? "HybridD1" : ship.Department.Code,
                                            ChargeableWeightUnitCode = "KG",
                                            ConsigneeId = string.IsNullOrEmpty(ship.Consignee.Code) ? "70000" : ship.Consignee.Code, 
                                            ShipperId = string.IsNullOrEmpty(ship.Shipper.Code) ? "70000" : ship.Shipper.Code,
                                            //ship.Shipper.Code,//"70000",
                                            ConsigneeReference1 = ship.CustomerReference1,//"PO35104",
                                            CreateDateTime = DateTime.Now,
                                            CreatedByUserId = "HybridU1",
                                            CutoffDate = DateTime.Now,
                                            DimensionsUnitCode = "CM",

                                            FinalDistenationPortId = ship.ToPort.Code,//"TLV",
                                            FreightPrepaidCollectId = "C",
                                            FromPortId = ship.FromPort.Code,//"JFK",
                                            GrossWeightUnitCode = "KG",
                                            House = ship.House,//"4545",
                                            IncotermId = "CIF",
                                            MainCarriageCarrierId = "LY",
                                            MainCarriageFinalDestinationPortId = ship.ToPort.Code,//"TLV",
                                            MainCarriageFromPortId = ship.FromPort.Code,
                                            MainCarriageToPortId = ship.ToPort.Code,
                                            OtherPrepaidCollectId = "C",
                                            ProfitCurrencyId = "USD",
                                            ShipmentCustomerTypeCode = "SHI",

                                            ShipmentNumber = shipmentnumber,

                                            StatusId = null,//ship.StatusCode,//.Code"SHOR",
                                            ToPortId = ship.ToPort.Code,//,"TLV",
                                            TransportModeId = ship.TransportModeId,//"A",
                                            VolumeUnitCode = "CBM",
                                            ChargeableWeight = 0.9999984133,
                                            GrossWeight = 0.9999984133,
                                            Master = "45454",
                                            MainCarriageATA = ship.MainCarriageATA,//DateTime.Now,
                                            MainCarriageETA = ship.MainCarriageETA,//DateTime.Now,
                                            MainCarriageETD = ship.MainCarriageETD,//DateTime.Now,
                                            MainCarriageATD = ship.MainCarriageATD,//DateTime.Now,

                                            AccountedPayablesInLocalCurrency = 100,
                                            AccountedPayablesInProfitCurrency = 200,
                                            AccountedReceivablesInLocalCurrency = 300,
                                            AccountedReceivablesInProfitCurrency = 400,
                                            CustomerShipmentNumber = ship.CustomerShipmentNumber
                                            //CustomFileNumber = "CUSTOMF1",

                                        };
                                        //CUSTOMF1


                                        ShipmentWcfServiceReference.Response response = shipmentservice.Upsert(pm, false);
                                       
                                    }
                                }
                            }
                            //List<QueueTask>
                        }
                        //ExternalTasksQueueWcfService.ExternalTasksQueueWcfServiceClient Service = new ExternalTasksQueueWcfService.ExternalTasksQueueWcfServiceClient();

                        //Service.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);



                        Thread.Sleep(3000);
                        //warmService.LoadMetaData();
                        //Thread.Sleep(300000); // 5 minutes300000
                        LogDoneItemInMemory();
                    }
                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ExternalTasksWorkerRole", null);
                        Thread.Sleep(10000);
                    }
                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ExternalTasksWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            //RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }

        public string Login(string email, string password)
        {
            string Result = "";
           
            try
            {
                string hashedPassword = GetHashedPassword(email, password);
                IGlobalContext globalObjectContext = GlobalContext.GetContext();
                ContactPassword contactPassword = AuthenticationUtil.VerifyContactPassword(email, password, globalObjectContext);
                if (contactPassword != null)
                {
                    hashedPassword = contactPassword.Password;
                    if (contactPassword.IsLocked) contactPassword = null;

                }

                GlobalContact contact = globalObjectContext.GlobalContacts.Where(m => m.Email == email && m.InActive == false).FirstOrDefault();
                if (contactPassword != null && contact != null && contact.InActive == false)
                {
                    string token = AuthenticationUtil.GenerateToken();// Guid.NewGuid().ToString();
                    AuthenticationTokenRepository authenticationTokenRepository = new AuthenticationTokenRepository(0);
                    AuthenticationToken authentication = new AuthenticationToken() { CreateDate = DateTime.Now, Email = email, Password = hashedPassword, Token = token };
                    authenticationTokenRepository.Add(authentication);
                    authenticationTokenRepository.SubmitChanges();

                    Result = token;

                }
               
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return Result;
        }

        public static string GetHashedPassword(string email, string password)
        {
            email = email.ToLower();
            byte[] byteRepresentation = UnicodeEncoding.UTF8.GetBytes(password + email);
            byte[] hashedTextInBytes = null;
            MD5CryptoServiceProvider myMd5 = new MD5CryptoServiceProvider();
            hashedTextInBytes = myMd5.ComputeHash(byteRepresentation);
            string hashedText = Convert.ToBase64String(hashedTextInBytes);

            return hashedText;
            //ICy5YqxZB1uWSwcVLSNLcA==
        }
    }
}
