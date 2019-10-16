using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WebFreight.Web.Helpers.TimeClock
{
    public class TMClockAnalyze
    {
        private AnalyzeQueue myAnalyzeQueue;
        private AnalyzeQueueRepository analyzeQueueRepository;
        int Tenant;
        CommunicationLogRepository myCommunicationLogRepository;

        public TMClockAnalyze(AnalyzeQueue analyzeQueue, AnalyzeQueueRepository analyzeQueueRepository)
        {
            if (analyzeQueue != null)
            {
                this.Tenant = analyzeQueue.Tenant;
                this.myAnalyzeQueue = analyzeQueue;
                this.analyzeQueueRepository = analyzeQueueRepository;
                this.myCommunicationLogRepository = new CommunicationLogRepository(this.Tenant);

            }
        }

        public void Run()
        {
            if (myAnalyzeQueue != null)
            {
                this.Deserialize();
            }
        }

        private void Deserialize()
        {
            try
            {
                this.AnalyzeData(myAnalyzeQueue.From);

            }
            catch (Exception ex)
            {
                myAnalyzeQueue.Status = "F";
                myAnalyzeQueue.ErrorMessage = "TMC Page Load failed: " + ex.Message;
                myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
                return;
            }

        }

        private void AnalyzeData(string from)
        {
            try
            {
                this.ConnectAnalyzeQueue();
            }
            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }

        private void ConnectAnalyzeQueue()
        {
            try
            {
                if (!myAnalyzeQueue.ConnectedToTenant)
                {
                    myAnalyzeQueue.ConnectedToTenant = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                if (!myAnalyzeQueue.ConnectedToEntity)
                {
                    myAnalyzeQueue.ConnectedToEntity = true;
                    analyzeQueueRepository.Update(myAnalyzeQueue);
                    analyzeQueueRepository.SubmitChanges();
                }

                ///WORK 
                if (this.myAnalyzeQueue.MessageBody != null)
                    ImportClockTimeData(this.myAnalyzeQueue.MessageBody);

                myAnalyzeQueue.Status = "D";
                analyzeQueueRepository.Update(myAnalyzeQueue);
                analyzeQueueRepository.SubmitChanges();
            }

            catch (Exception ex)
            {
                this.OnCatchAnalyzingError(ex);
            }
        }

        private Dictionary<string, List<string>> BuildIndex(byte[] data)
        {
            string datastring = Encoding.ASCII.GetString(data);
            Dictionary<string, List<string>> Dictionay = new Dictionary<string, List<string>>();
            using (StringReader reader = new StringReader(datastring))
            {
                string line;
                int tenant = this.myAnalyzeQueue.Tenant;
                ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
                ITimeManagementContext TMContext = TimeManagementContext.GetContext(tenant);
                ObjectTableRepository objectTableRepository = new ObjectTableRepository(tenant);
                ObjectTable UserTable = objectTableRepository.GetObjectTableByName("User", tenant, true);
             
                ComputingPartner ComputingPartner = myCommonContext.ComputingPartners.Where(d => (d.Tenant == tenant && d.Code == "CTM") ||(d.Tenant==0&& d.Code== "G-CTM")).FirstOrDefault();
                if (ComputingPartner != null)
                {
                    ComputingPartnerTable Table = myCommonContext.ComputingPartnerTables.Where(d => d.Tenant == ComputingPartner.Tenant && d.ComputingPartnerId == ComputingPartner.Id && d.ObjectTable.Name == "User").FirstOrDefault();
                    if (Table != null)
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] linedata = line.Split(',');

                            string ComputingPartnerId = linedata[0];
                            string EntryTime = linedata[1];
                            if (!string.IsNullOrEmpty(ComputingPartnerId) && !string.IsNullOrEmpty(EntryTime))
                            {
                                ComputingPartnerTranslation Entity = myCommonContext.ComputingPartnerTranslations.Where(d => d.Tenant == tenant && d.ComputingPartnerId == ComputingPartner.Id && d.ObjectTableId == UserTable.Id && d.PartnerCode == ComputingPartnerId).FirstOrDefault();
                                if (Entity != null)
                                {
                                    string UserEmail = Entity.OurCode;
                                    string userId = myCommonContext.Contacts.Where(d => d.Email == UserEmail && d.Tenant ==tenant).FirstOrDefault().Id;
                                    List<string> DateWithTime = EntryTime.Split(' ').ToList();
                                    List<string> ReversedDate = DateWithTime[0].Split('/').ToList();
                                    String ValidDate = ReversedDate[2] + "/" + ReversedDate[0] + "/" + ReversedDate[1] + " " + DateWithTime[1];
                                    DateTime EntryDate = DateTime.Parse(ValidDate);
                                    DateTime ExactDate = new DateTime(EntryDate.Year, EntryDate.Month, EntryDate.Day, 0, 0, 0);
                                    List<string> retValue;
                                    if (!Dictionay.TryGetValue(userId, out retValue))
                                    {
                                        List<string> PostingList = new List<string>();
                                        PostingList.Add(EntryDate + "," + ExactDate);
                                        Dictionay.Add(userId, PostingList);
                                    }
                                    else
                                    {
                                        Dictionay[userId].Add(EntryDate + "," + ExactDate);
                                    }


                                }

                            }

                        }
                    }
                }
            }


            return Dictionay;
        }

        public string ImportClockTimeData(byte[] data)
        {
          
            int tenant = this.myAnalyzeQueue.Tenant;
            int Counter = 0;
            Dictionary<string, List<string>> InvertedIndex = BuildIndex(data);
            string ErrorMessage = "";
            ITimeManagementContext TMContext = TimeManagementContext.GetContext(tenant);
            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRepository = new ContactRepository(myCommonContext);
            Contact LoggedContact = myCommonContext.Contacts.Where(d => d.Email == "system@tenant0.com" && d.Tenant == 0).FirstOrDefault();

            for (int i = 0; i < InvertedIndex.Keys.Count; i++)
            {
                List<string> linedata = InvertedIndex.ElementAt(i).Value;
                TMOfficeHour OfficeHour = null;
                bool flag = true;
                List<TMOfficeHour> ListOfOfficeHour = new List<TMOfficeHour>();
                for (int j = 0; j < linedata.Count; j++)
                {
                    DateTime EntryDate = DateTime.Parse(linedata[j].Split(',')[0]);
                    DateTime ExactDate = DateTime.Parse(linedata[j].Split(',')[1]);

                    if (flag)
                    {
                        OfficeHour = new TMOfficeHour();

                        string entityId = IdCounter.GetNumber("TMOfficeHour", tenant).ToString();
                        OfficeHour.Id = entityId;
                        OfficeHour.UserId = InvertedIndex.ElementAt(i).Key;
                        OfficeHour.WorkDate = ExactDate;
                        OfficeHour.RecordedEntryTime = EntryDate;
                        OfficeHour.EntryTime = EntryDate;
                        OfficeHour.CreateDate = EntryDate;
                        OfficeHour.UpdateDate = ExactDate;
                        OfficeHour.Tenant = tenant;
                        OfficeHour.CreatedByUserId = LoggedContact.Id;
                        OfficeHour.UpdatedByUserId = LoggedContact.Id;
                        ListOfOfficeHour.Add(OfficeHour);
                    }
                    else
                    {

                        if (ListOfOfficeHour[ListOfOfficeHour.Count - 1].WorkDate == ExactDate)
                        {
                            ListOfOfficeHour[ListOfOfficeHour.Count - 1].RecordedExitTime = EntryDate;
                            ListOfOfficeHour[ListOfOfficeHour.Count - 1].ExitTime = EntryDate;
                        }
                        else
                        {
                            flag = false;
                            j--;
                        }


                    }
                    flag = !flag;
                }

                foreach (TMOfficeHour item in ListOfOfficeHour)
                {
                    TMOfficeHour DBRecord = TMContext.TMOfficeHours.Where(d => d.UserId == item.UserId && d.Tenant == tenant && d.RecordedEntryTime == item.RecordedEntryTime && d.WorkDate == item.WorkDate && d.RecordedExitTime == item.RecordedExitTime).FirstOrDefault();
                    if (DBRecord == null)
                    {
                        TMOfficeHour DBRecord2 = TMContext.TMOfficeHours.Where(d => d.UserId == item.UserId && d.Tenant == tenant && d.RecordedEntryTime == item.RecordedEntryTime && d.WorkDate == item.WorkDate).FirstOrDefault();
                        if (DBRecord2 != null)
                        {
                           DBRecord2.RecordedExitTime= DBRecord2.RecordedExitTime!=null? DBRecord2.RecordedExitTime:item.RecordedExitTime;
                            DBRecord2.ExitTime = DBRecord2.ExitTime!=null? DBRecord2.ExitTime: item.ExitTime;
                            TMContext.TMOfficeHours.Attach(DBRecord2);
                            TMContext.SetAsModified(DBRecord2);
                            TMContext.SaveChanges();
                        }
                        else
                        {
                            TMContext.TMOfficeHours.Add(item);
                        }
                    }
                    Counter++;
                    if (Counter >= 500)
                    {
                        Counter = 0;
                        TMContext.SaveChanges();
                    }

                }

                TMContext.SaveChanges();
            }

            return ErrorMessage;

        }


        private void OnCatchAnalyzingError(Exception ex)
        {
            myAnalyzeQueue.ErrorMessage = ex.Message + (ex.InnerException != null ? Environment.NewLine + "InnerException: " + ex.InnerException.Message : "");
            myAnalyzeQueue.StackTrace = (ex.StackTrace != null ? Environment.NewLine + "Stack Trace: " + ex.StackTrace : "");
            myAnalyzeQueue.ErrorMessage = myAnalyzeQueue.ErrorMessage.Length > 7950 ? myAnalyzeQueue.ErrorMessage.Substring(0, 7950) : myAnalyzeQueue.ErrorMessage;
            myAnalyzeQueue.StackTrace = myAnalyzeQueue.StackTrace.Length > 7950 ? myAnalyzeQueue.StackTrace.Substring(0, 7950) : myAnalyzeQueue.StackTrace;

            if (ex.Message.StartsWith("--"))
            {
                myAnalyzeQueue.Status = "F";
            }

            else
            {
                myAnalyzeQueue.Retries++;

                if (myAnalyzeQueue.Retries >= 5)
                {
                    myAnalyzeQueue.Status = "F";

                    if (myAnalyzeQueue.ConnectedToTenant)
                    {
                        CommunicationLog commLog = myCommunicationLogRepository.GetSingleCommunicationLog(myAnalyzeQueue.CommunicationLogId, Tenant);
                        if (commLog != null)
                        {
                            commLog.CommunicationStatusTypeCode = "F";
                            commLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(Tenant);
                            commLog.LastStatusDateUTC = DateTime.UtcNow;
                            commLog.ExceptionMessage = myAnalyzeQueue.ErrorMessage;

                            if (myAnalyzeQueue.StackTrace != null)
                            {
                                commLog.ExceptionMessage = commLog.ExceptionMessage + Environment.NewLine + "Stack Trace: " + myAnalyzeQueue.StackTrace;
                            }

                            myCommunicationLogRepository.Update(commLog);
                            myCommunicationLogRepository.SubmitChanges();
                        }
                    }
                }
            }
            myAnalyzeQueue.DoneDate = TenantServerConfigration.GetCurrentDateTime(myAnalyzeQueue.Tenant);
            analyzeQueueRepository.Update(myAnalyzeQueue);
            analyzeQueueRepository.SubmitChanges();
        }

    }
}