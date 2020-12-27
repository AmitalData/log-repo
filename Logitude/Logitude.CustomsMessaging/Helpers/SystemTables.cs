using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using UnifreightIIG.Common.ClientSdk;
using UnifreightIIG.Common.Faults;
using UnifreightIIG.Common.SystemTableServiceReference;
using UnifreightIIG.Common.TheGateway;
using Logitude.AmitalMessaging.Infrastructure.SystemTable;
using Logitude.Customs.BL.Messaging.Amital;
using Logitude.Server.Tools.Models;
using System.Data;
using System.Text;
using UnifreightIIG.Common.CommonIIGInterface;
using Logitude.Customs.BL.EntityQueryServices;
using System.Diagnostics;
using Logitude.CustomsMessaging.MessagingServices;

namespace Logitude.CustomsMessaging.Helpers
{
    public class SystemTables
    {
        private static List<Customs.Def.EntityPMs.CustomsSettingPM> _AllSettingInDBZero;

        static SystemTables()
        {
            var qs = new Logitude.Customs.BL.EntityQueryServices.CustomsSettingQueryService(0);
            _AllSettingInDBZero = qs.GetAll();
        }
        public static void SpeedTest(string IIGServiceAddress, int tenant, string tableID= "1136")
        {

            var defaultSetting = _AllSettingInDBZero.FirstOrDefault(rec =>
                //!String.IsNullOrWhiteSpace( rec.IIGServiceAddress)
                    rec.Tenant == tenant
                    );
            if (defaultSetting == null)
            {
                throw new System.Exception("SystemTables:no default IIGServiceAddress");
            }
            var sw = Stopwatch.StartNew();
            var srvLog = "";
            MoreParams mySystemTablesMoreParams = new MoreParams();
            using (var myUnifreightSdkGateway = new UnifreightSdkGateway(IIGServiceAddress))
            {
                //var aa = myUnifreightSdkGateway.GetChannel<ISpeedTest>().SpeedTest("1", 10);
                var myResponseTableData = myUnifreightSdkGateway.GetChannel<IGatewayServiceSystemTableService>().
                      GetSystemTables(
                              "510120041"  //exist in mt computer and in 10.10.25.4 
                              //defaultSetting.CustomsAgentId
                              , tableID,
                              ref mySystemTablesMoreParams,
                              out srvLog).ToList();

            }
            
            
            sw.Stop();
            if (sw.Elapsed > TimeSpan.FromSeconds(12))
            {
                Debug.Write("***" + sw.Elapsed.ToString());
            }
        }
        public List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> GetTableData(
            string TableID,
            int tenant,
            MoreParams mySystemTablesMoreParams = null,
            bool? sendMehesTable2Amital = null)
        {
            var res  =SYSTBL_NG_9000_MSG_SystemTableRequestMessageService.SendIt(TableID, tenant, true);
            var mylist=res.MySystemTableResponse as List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData>;
            return mylist;





            var TableFromCache = ReadFromDisk(TableID);
            if (TableFromCache != null && TableFromCache.Count() > 0) return TableFromCache.OrderBy(rec => rec.id).ToList();


            if (mySystemTablesMoreParams == null)
            {
                mySystemTablesMoreParams = new MoreParams();
                mySystemTablesMoreParams.MyOption = MoreParams.Options.None;
                //if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["TestMode"]))
                //{
                //    mySystemTablesMoreParams.MyOption = MoreParams.Options.TestMode;
                //}
            }
            if (sendMehesTable2Amital == null)
            {
                sendMehesTable2Amital = SyncUnifreight(TableID);
            }
            List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> myResponseTableData;

            var srvLog = "";
            try
            {

                var defaultSetting = _AllSettingInDBZero.FirstOrDefault(rec =>
                    //!String.IsNullOrWhiteSpace( rec.IIGServiceAddress)
                    rec.Tenant == tenant
                    );
                if (defaultSetting == null)
                {
                    throw new System.Exception("SystemTables:no default IIGServiceAddress");
                }
                using (var myUnifreightSdkGateway = new UnifreightSdkGateway(defaultSetting.IIGServiceAddress))
                {
                //    var aa = myUnifreightSdkGateway.GetChannel<ISpeedTest>().SpeedTest("1", 10);
                    myResponseTableData = myUnifreightSdkGateway.GetChannel<IGatewayServiceSystemTableService>().
                        GetSystemTables(
                                defaultSetting.CustomsAgentId, TableID,
                                ref mySystemTablesMoreParams,
                                out srvLog).ToList();
                }

                if (sendMehesTable2Amital.Value)
                {
                    Send2Amital(TableID, myResponseTableData, tenant);
                }

                return myResponseTableData;
            }
            catch (FaultException<UnifreightIIGFault> ex)
            {
                switch (ex.Detail.LoggerLevel)
                {
                    case UnifreightIIGFault.LoggerLevelEnum.Debug:
                        break;
                    case UnifreightIIGFault.LoggerLevelEnum.Error:
                    case UnifreightIIGFault.LoggerLevelEnum.Fatal:
                    case UnifreightIIGFault.LoggerLevelEnum.Warn:
                        //MessageBox.Show(ex.Detail.Message);
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "WebRole", "system tables class", null);
                        break;
                    default:
                        break;
                }
                // only if a fault contract was specified 
                return null;
            }

            catch (FaultException ex)
            {

                // any other faults 
                throw;
            }
            catch (TimeoutException timeProblem)
            {
                Console.WriteLine("The service operation timed out. " + timeProblem.Message);
                // any communication errors? 
                throw;
            }

            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message);
                // any communication errors? 
                throw;
            }


            catch (System.Exception)
            {

                throw;
            }

        }

        public static bool SyncUnifreight(string TableID)
        {
            /*
             * אלו רשימת הטבלאות שצריכות לעורר את הטריגר לבצע ממשק ליוניפרייט.
1136 - מדינות
1426 – תנאי מכר
1091 – סוגי אריזה
1120 -  סעיפי תשלום
2009 – הסכם סחר
1144 – סוג מטבע
2012 – אתר אחסון 
2013 – מעברים פנימי
2014 – מחסן רישוי
2011 - תחנות מכס
1354 - מזהה מטען      
1259 – סוג רשימון
1422 – תהליכים לסחורה
            */

            switch (TableID)
            {
                case "1136":
                case "1426":
                case "1091":
                case "1120":
                case "2009":
                case "1144":
                case "2012":
                case "2013":
                case "2014":
                case "2011":
                case "1354":
                //<--- Yuval Chalup 16.07.2015 TASK-13872
                case "1604": 
                case "1404":
                case "1585":
                case "1423":
                //Yuval Chalup 16.07.2015 TASK-13872 --->
                    //case "1259": Removed by Yuval Chalup 27.04.2015 TASK-12921
                case "1930": //Yuval Chalup 24.01.2016 AMI-55745
                case "13": //Yuval Chalup 05.07.2016 TASK-21102
                case "1385":
                case "1345":
                case "1416":
                case "1422":
                case "2192":
                    return true;
                    break;
                default:
                    return false;
                    break;
            }



        }


        public DataSet GetAsTableData(string TableID, int pageNumber, int pageSize,
          MoreParams mySystemTablesMoreParams = null,
          bool sendMehesTable2Amital = false)
        {

            var ExternalId = Guid.NewGuid().ToString();
            SYSTBL_NG_9001_MSG_SystemTablesResponse myResponse = null;

            try
            {


                if (mySystemTablesMoreParams == null)
                {
                    mySystemTablesMoreParams = new MoreParams();
                    mySystemTablesMoreParams.MyOption = MoreParams.Options.None;
                }

                var myRequest = new UnifreightIIG.Common.SystemTableServiceReference.SYSTBL_NG_9000_MSG_SystemTableRequest()
                {
                    RequestContentHeader = new RequestContentHeader() { SenderID = 1, RecieverID = new int[] { 1 } },
                    tableName = TableID,
                    SelectOptions = new SYSTBL_NG_9000_MSG_SystemTableRequestSelectOptions() { GetAsDataTable = true, PageNumber = pageNumber, PageSize = pageSize }

                };


                var defaultSetting = _AllSettingInDBZero.FirstOrDefault(rec => !string.IsNullOrWhiteSpace(rec.IIGServiceAddress));
                if (defaultSetting == null)
                {
                    throw new ArgumentNullException("IIGServiceAddress");
                }
                using (var myUnifreightSdkGateway = new UnifreightSdkGateway(defaultSetting.IIGServiceAddress))
                {
                    var myResponseHeader = myUnifreightSdkGateway.GetChannel<IGatewayServiceSystemTableService>().
                        SystemTablesDetails(ExternalId, defaultSetting.CustomsAgentId, myRequest,
                        ref mySystemTablesMoreParams,
                        out myResponse);
                    UnifreightIIGFault.ThrowIIGBLException(
                            myResponseHeader,
                            myResponse.GetResponseContentHeader() as IResponseContentHeader
                            );
                }
                if (String.IsNullOrWhiteSpace(myResponse.TableAsDataSetTableData))
                {
                    return null;
                }
                var ds = DataSetReadXML(myResponse.TableAsDataSetTableData);

                if (!sendMehesTable2Amital)
                {
                    if (Environment.UserName.Equals("itzik", StringComparison.OrdinalIgnoreCase) && DateTime.Now < new DateTime(2013, 10, 24))
                    {
                        sendMehesTable2Amital = true;
                    }
                }
                if (sendMehesTable2Amital)
                {
                    Send2Amital(TableID, ds);
                }

                return ds;
            }
            catch (FaultException<UnifreightIIGFault> ex)
            {
                switch (ex.Detail.LoggerLevel)
                {
                    case UnifreightIIGFault.LoggerLevelEnum.Debug:
                        break;
                    case UnifreightIIGFault.LoggerLevelEnum.Error:
                    case UnifreightIIGFault.LoggerLevelEnum.Fatal:
                    case UnifreightIIGFault.LoggerLevelEnum.Warn:
                        //MessageBox.Show(ex.Detail.Message);
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "WebRole", "system tables class", null);
                        break;
                    default:
                        break;
                }
                // only if a fault contract was specified 
                return null;
            }

            catch (FaultException ex)
            {

                // any other faults 
                throw;
            }
            catch (TimeoutException timeProblem)
            {
                Console.WriteLine("The service operation timed out. " + timeProblem.Message);
                // any communication errors? 
                throw;
            }

            catch (CommunicationException commProblem)
            {
                Console.WriteLine("There was a communication problem. " + commProblem.Message);
                // any communication errors? 
                throw;
            }


            catch (System.Exception)
            {

                throw;
            }

        }

        private void Send2Amital(string TableID, DataSet ds)
        {
            throw new NotImplementedException();
        }
        public static DataSet DataSetReadXML(string MyXml)
        {
            DataSet DataSet1 = null;
            UTF8Encoding myEncoder = null;
            //ASCIIEncoding myEncoder=null;
            try
            {

                //myEncoder = new ASCIIEncoding();
                myEncoder = new UTF8Encoding();
                if (MyXml == "") return null;
                Byte[] bytes = myEncoder.GetBytes(MyXml);
                DataSet1 = new DataSet();
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    //this.AutoGenerateColumns = true;

                    DataSet1.ReadXml(ms as Stream);

                }
                return DataSet1;
            }
            catch
            {

                throw;
                return null;
            }
            finally
            {
                DataSet1 = null;
                myEncoder = null;
            }
        }



        public static void Send2Amital(string TableID, List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> myResponseTableData, int Tenant)
        {
            var myCUSTOMS_TABLE = new CUSTOMS_TABLE();
            //myCUSTOMS_TABLE.TABLECODE = TableID;
            myCUSTOMS_TABLE.TABLECODE = new TABLECODE[] { new TABLECODE { TABLECODE_ID = TableID } }; ;
            var myTABLEDATAList = new List<TABLEDATA>();
            myResponseTableData.ForEach(recMehes =>
            {
                TABLEDATA newTABLEDATA = null;
                newTABLEDATA = DefaultInerface(recMehes);
                newTABLEDATA = SpecialMapping(newTABLEDATA, recMehes, TableID, Tenant);

                myTABLEDATAList.Add(newTABLEDATA);
            }
            );
            myCUSTOMS_TABLE.TABLECODE[0].TABLEDATA = myTABLEDATAList.ToArray();


            //for (int curTenant = 1; curTenant < 2; curTenant++)// by mohammad i added the tenant and saw this code commented so i uncommented it to make the project build please do you adjustment.
            //{
            if (_AllSettingInDBZero == null)
            {
                return;
            }
            foreach (var item in _AllSettingInDBZero)
            {
                var setting = CustomsSettingQueryService.GetSettingByTenant(item.Tenant);
                if (setting != null)
                {
                    if (setting.IsConnectedToUniFreight || !String.IsNullOrWhiteSpace(setting.UnfConnectionString))
                    {
                        UServerCommunication.SendUpdateTableToUnifreight(item.Tenant, TableID, myCUSTOMS_TABLE, false, true);
                    }
                }
            }

        }

        private static TABLEDATA SpecialMapping(TABLEDATA newTABLEDATA, SYSTBL_NG_9001_MSG_SystemTablesResponseTableData recMehes, string TableID, int Tenant)
        {
            if (newTABLEDATA == null)
            {
                return newTABLEDATA;
            }
            switch (TableID)
            {
                case "2011":
                    newTABLEDATA.TABLEDATA_ADDITIONALCODE1 = GetModeOfTransport(recMehes, Tenant);
                    break;
                default:
                    break;
            }
            return newTABLEDATA;
        }


        private static string GetModeOfTransport(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData recMehes, int Tenant)
        {
            string transportMode = "";
            if (recMehes == null)
            {
                return transportMode;
            }

            var defTenat = _AllSettingInDBZero.FirstOrDefault();
            //var tenant = defTenat.Tenant;

            var repo = new Logitude.Customs.Data.Repsitories.CustomsHouseTypeAdditionalRepository(Tenant);
            var poko = repo.GetAll(Tenant).Where(rec => rec.Code == recMehes.id).FirstOrDefault();
            if (poko != null)
            {
                transportMode = poko.TransportModeId;
            }
            return transportMode;
        }



        private static TABLEDATA DefaultInerface(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData recMehes)
        {
            var newTABLEDATA = new TABLEDATA()
            {
                TABLEDATA_ID = recMehes.id,
                TABLEDATA_NAME_ENG = recMehes.extraStringData,
                TABLEDATA_NAME_HEB = recMehes.name,
                TABLEDATA_ADDITIONALCODE1 = recMehes.malamID.ToString(),
                TABLEDATA_ADDITIONALCODE2 = recMehes.extraNumericData.ToString(),
                TABLEDATA_BLOCKED = IsBlock(recMehes.state),
                TABLEDATA_REMARKS = (recMehes.updateDate.HasValue ? recMehes.updateDate.Value.ToString() : "")
            };
            return newTABLEDATA;
        }

        private static string IsBlock(int? state)
        {
            if (!state.HasValue) return "F";
            if (state.Value == 1)
            {
                return "F";
            }
            return "T";
        }



        SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[] ReadFromDisk(string TableId)
        {

            if (String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Cache2Disk"])) return null;
            SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[] myTableData;
            if (!File.Exists(TableId + ".Xml")) return null;

            using (var my = new System.IO.StreamReader(TableId + ".Xml"))
            {

                var x = new System.Xml.Serialization.XmlSerializer(typeof(SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[]));
                myTableData = (x.Deserialize(my) as SYSTBL_NG_9001_MSG_SystemTablesResponseTableData[]);


            }

            return myTableData;


        }

        private static SYSTBL_NG_9000_MSG_SystemTableRequest GetReqObject()
        {
            var mySystemTableRequest = new SYSTBL_NG_9000_MSG_SystemTableRequest();
            // Create New Request

            // Initialize
            mySystemTableRequest.RequestContentHeader = new RequestContentHeader();
            mySystemTableRequest.RequestContentHeader.RecieverID = new int[2];
            mySystemTableRequest.RequestContentHeader.RecieverID[0] = 123;
            mySystemTableRequest.RequestContentHeader.RecieverID[1] = 222;
            mySystemTableRequest.RequestContentHeader.SenderID = 456;
            mySystemTableRequest.RequestContentHeader.TransmitionDateTime = System.DateTime.Now;
            mySystemTableRequest.tableName = "TableConfiguration"; // "CC_TA_M_007";
            return mySystemTableRequest;
        }

        internal List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> GetChangingTimeRequestTypeList()
        {

            //הבאת רשימת תורים פנויים או שינוי מועד בדיקה 
            return GetTableData("1156", 208);
        }


        internal List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> GetChangingTimeSitelookupList()
        {
            return GetTableData("1339", 208);
        }

        internal List<SYSTBL_NG_9001_MSG_SystemTablesResponseTableData> GetChangingTimeCheckQueueTypeLookup()
        {

            return GetTableData("1518", 208);
        }


        //internal static void UpdateDB()
        //{



        //    var myGenDbLoader1 = new UnifreightIIG.Client.BL.SystemTable.DbLoader.GenDbLoader<CheckQueueType>();
        //    myGenDbLoader1.LoadToDB(new CheckQueueType());

        //    var myGenDbLoaderCheckSiteNumber = new UnifreightIIG.Client.BL.SystemTable.DbLoader.GenDbLoader<CheckSiteNumber>();
        //    myGenDbLoaderCheckSiteNumber.LoadToDB(new CheckSiteNumber());

        //    var myGenDbLoaderTableConfiguration = new UnifreightIIG.Client.BL.SystemTable.DbLoader.GenDbLoader<TableConfiguration>();
        //    myGenDbLoaderTableConfiguration.LoadToDB(new TableConfiguration());


        //}


        //private static GovTableState GetInt16(int? nullable)
        //{
        //    if (nullable.HasValue)
        //    {
        //        Int16 myGovRowId = -1;
        //        Int16.TryParse(nullable.Value.ToString(), out myGovRowId);
        //        return (GovTableState)myGovRowId;
        //    }
        //    else
        //    {
        //        return GovTableState.Active;
        //    }
        //}




    }
}
