using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    public class BatchSystem1000FlatFileAnalyser : BatchTaskExecutionsService
    {
        public BatchSystem1000FlatFileAnalyser(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }
        public override void RunCode()
        {
            // Deserilaize parameters
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(System1000FlatFileAnalyserArgs)); // AccountingIntegrityInParam
            System1000FlatFileAnalyserArgs parameterArgs = serializer.Deserialize(stringReader) as System1000FlatFileAnalyserArgs;
            string winHebrewString = GetCommunicationsData
                (parameterArgs.Tenant, parameterArgs.CommunicationLogId);
            try
            {


                using (var scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(600)))
                {

                    var system1000FlatFileAnalyser = new System1000FlatFileAnalyser();
                    system1000FlatFileAnalyser.Analyse(parameterArgs.Tenant, winHebrewString, parameterArgs.LoggingUserId);
                    string responseText = system1000FlatFileAnalyser.MyResultLoadFlatFile.SuccessVendorList.ToString();
                    ChangeStatus("D", null, responseText);

                    scope.Complete();

                }

            }
            catch (Exception ex)
            {

                LogitudeSettings.HandleLogMe(ex.ToString(), true, "BatchSystem1000FlatFileAnalyser", new DateTime(2019, 10, 1));
                throw;

            }
        }

        string GetCommunicationsData(int tenant, string CommunicationLogId)
        {
            try
            {
                var _CommunicationLog = Communications.GetCommunicationLog(tenant, CommunicationLogId);
                if (_CommunicationLog == null)
                {
                    throw new Exception("Cannnot GetCommunicationLog");
                }
                var communicationsData = Communications.GetData(_CommunicationLog); ;
                if (string.IsNullOrWhiteSpace(communicationsData))
                {
                    throw new Exception("communicationsData is null");
                }
                return communicationsData;

            }
            catch (Exception ee)
            {

                throw;
            }

        }
    }
    public class System1000FlatFileAnalyserArgs
    {
        public string CommunicationLogId { get; set; }
        public string LoggingUserId { get; set; }
        public int Tenant { get; set; }
    }
}
