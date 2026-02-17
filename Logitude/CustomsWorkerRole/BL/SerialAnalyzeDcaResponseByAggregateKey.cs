
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CustomsWorkerRole.Queue;
using UnifreightIIG.Common.Utils;
using Logitude.Server.Tools;
using Logitude.CustomsMessaging.MessagingServices;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.BL.Messaging.Customs;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using CustomsWorkerRole.Utils;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.Customs.BL.EntityQueryServices;
using CustomsWorkerRole.BL;
using Logitude.Customs.Data;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.Helpers;
using System.Globalization;
using Logitude.Customs.Def.ClosedTable;

namespace CustomsWorkerRole.BL
{
    public class SerialAnalyzeDcaResponseByAggregateKey
    {
        //private Func<BrokeredMessage, OverrideControllerModel, bool> _baseProcessMessage;
        private int _Tenant;
        //private string _SourceReqSheetId;
        private ICommonDataContext _CommonContext;
        private ICustomContext _CustomContext;
        private CustomsRequestsSheetQueryService _CustomsRequestsSheetQueryService;
        private string _DcaAnalyzeAggregateKey;
        //private BrokeredMessage _BrokeredMessage;
        private StringBuilder _logger;
        List<DcaLogEntry> _DCAEntryList;

        internal List<DcaLogEntry> DCAEntryList
        {
            get { return _DCAEntryList; }
            set
            {
                _DCAEntryList = value;
                if (_DCAEntryList == null) return;
                var list = _DCAEntryList.Where(r => r.AnalyzeAt.HasValue).ToList();
                int i = 0;
                if (list.Count > 0)
                {
                    i = list.Max(r => r.Seq);
                }
                
                foreach (var item in _DCAEntryList.Where(r => !r.AnalyzeAt.HasValue))
                {
                    item.Seq = i++;
                }

            }
        }

        //private List<string> _CorrelationWait4Analyze;

        private SerialAnalyzeDcaResponseByAggregateKey(
            //Func<BrokeredMessage, OverrideControllerModel, bool> baseProcessMessage, 
            BrokeredMessage brokeredMessage)
        {
           // _baseProcessMessage = baseProcessMessage;
            //if (_baseProcessMessage == null)
            //{
            //    throw new System.ArgumentNullException("baseProcessMessage");
            //}
            var _BrokeredMessage = brokeredMessage;
            if (_BrokeredMessage == null)
            {
                throw new System.ArgumentNullException("message");
            }
            _DcaAnalyzeAggregateKey = _BrokeredMessage.GetProperty<string>(QueueExt.QueuePropertyNames.DcaAnalyzeAggregateKey, "");//, 
            
            _Tenant = _BrokeredMessage.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
            //_SourceReqSheetId = _BrokeredMessage.CorrelationId;

            Init();
        }
        public SerialAnalyzeDcaResponseByAggregateKey(int tenant, string dcaAnalyzeAggregateKey)
        {

            _DcaAnalyzeAggregateKey = dcaAnalyzeAggregateKey;// _BrokeredMessage.GetProperty<string>(QueueExt.QueuePropertyNames.DcaAnalyzeAggregateKey, "");//, 

            _Tenant = tenant;// _BrokeredMessage.GetProperty<int>(QueueExt.QueuePropertyNames.Tenant, -1);
            //_SourceReqSheetId = sourceReqSheetId;// _BrokeredMessage.CorrelationId;

            Init();
        }

        private void Init()
        {
            if (String.IsNullOrWhiteSpace(_DcaAnalyzeAggregateKey))
            {
                throw new System.ArgumentNullException("_DcaAnalyzeAggregateKey");
            }
            if (_Tenant < 1)
            {
                throw new System.ArgumentNullException("_Tenant<1");
            }
            _logger = new StringBuilder();

            _CommonContext = CommonDataContext.GetContext(_Tenant);
            _CustomContext = CustomContext.GetContext(_Tenant);
            _CustomsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(_CustomContext);
        }



        internal bool DoSerialAnalyze()
        {

            var sw = Stopwatch.StartNew();


            ///var analyzeClass = _BrokeredMessage.GetProperty<string>(QueueExt.QueuePropertyNames.InterfaceTypeCode, "");//, 



            _logger.Append("StartAggregrateAnalyze:").Append(
                //Thread.CurrentThread.GetHashCode()
                Guid.NewGuid().ToString()
                ).Append("At:").AppendLine(DateTime.Now.ToString()); ;


            using (var scope = TransactionFactory.GetNewTransaction(TimeSpan.FromMinutes(20)))
            {
                try
                {
                    LockIt();
                }
                catch (Exception eee)
                {
                    AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Information);

                    if (eee.ToString().Contains("ORA-00054"))
                    {
                        throw new CustomsRequestsSheetDomainModelServiceException(CustomsRequestsSheetDomainModelServiceException.WhereEnum.AggregateDCAAnalyzerLockIt, CustomsRequestsSheetDomainModelServiceException.What2DoEnum.RetryQueue, "GeneralLock is locked in another thread", eee);
                    }
                    //ActivityLogger
                    throw;
                }

                //if (list == null) //All Executed
                //{
                //    _BrokeredMessage.SafeComplete();
                //    return true;
                //}
                //if (list.Count() == 0) //All Executed
                //{
                //    TryDeleteLockRow();
                //    _BrokeredMessage.SafeComplete();
                //    return true;
                //}

                //foreach (var item in list)
                CustomsRequestsSheetPM item = null;
                while (
                    (item = GetAllDcaStageAnalyze().FirstOrDefault())
                    != null)
                {


                    


                    var sb = new StringBuilder();
                    sb.AppendLine(DcaLogEntry.HeaderToString());
                    foreach (var itemDCAEntry in DCAEntryList)
                    {
                        sb.AppendLine(itemDCAEntry.ToString());
                    }
                    //_logger.Append("Start Analyze CorrelationId=").Append(item.CorrelationId);
                    var controller = new OverrideControllerModel()
                    {
                        IsAggregateDCAAnalyzer = true,
                        AggregateDCAAnalyzerLogger = _logger.ToString() + Environment.NewLine + sb.ToString()
                    };
                    
                    try
                    {

                        using (var scope1 = TransactionFactory.GetNewTransactionSuppress()) //Hope the Down will open new transc
                        {

                            MessagingServiceFactoryHelper.ResolveAndExecute(
                                item.InterfaceTypeCode,
                                item.Tenant,
                                item.Id,
                                CustomsCommandEnum.CustomsCommandAnalyzeResponseWR,
                                controller);

                        }
                    }
                    catch (Exception)
                    {
                        
                        throw;
                    }





                    DCAEntryList.First(r => r.Correlation == item.CorrelationId).AnalyzeAt = DateTime.Now;
                    if (sw.Elapsed > TimeSpan.FromMinutes(15))
                    {
                        return false;//timeout lock to big
                    }
                }
                TryDeleteLockRow();
                scope.Complete();
                return true;
            }
        }

        



        private void TryDeleteLockRow()
        {
            //GeneralLock
            //using (var scope = TransactionFactory.GetNewTransaction())
            {

                var repo = new GeneralLockRepository(_Tenant);

                //var repo = new GeneralLockRepository(_Tenant);
                repo.FastDelete(_DcaAnalyzeAggregateKey, _Tenant);
                //  scope.Complete();
            }
        }
        private void LockIt()
        {
            var repo = new GeneralLockRepository(_Tenant);
            var lockPoco = repo.GetSingleGeneralLockNOWAIT(_DcaAnalyzeAggregateKey, _Tenant);
            if (lockPoco == null)
            {
                using (var scope = TransactionFactory.GetNewTransaction())
                {
                    repo.Add(new GeneralLock()
                    {
                        Tenant = _Tenant,
                        GeneralKey = _DcaAnalyzeAggregateKey,
                        CreatedAt = TenantServerConfigration.GetCurrentDateTime(_Tenant)
                    });
                    _logger.AppendLine("add GeneralLock");
                    repo.SubmitChanges();
                    scope.Complete();
                }
                lockPoco = repo.GetSingleGeneralLockNOWAIT(_DcaAnalyzeAggregateKey, _Tenant);    
                //poco = AddKeyAndLock(dcaAnalyzeAggregateKey);
            }
            
            if (lockPoco == null)
            {
                throw new Exception("lockPoco ==null");
            }
            else
            {
                _logger.AppendLine("Lock it ");
            }

        }





        private List<CustomsRequestsSheetPM> GetAllDcaStageAnalyze()
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var dbList =
                _CustomsRequestsSheetQueryService
                .GetAllReceivedDca(_DcaAnalyzeAggregateKey, _Tenant)
                .OrderBy(rec => rec.RequestCreateDate)
                .ToList();
                var currentDbDcaEntryList = dbList.Select(rec => new DcaLogEntry()
                {
                    CreateAt = rec.RequestCreateDate.Value,
                    Correlation = rec.CorrelationId
                }).ToList();
                if (DCAEntryList == null)
                {
                    DCAEntryList = currentDbDcaEntryList;
                }
                else
                {
                    
                    var acurateWait2AnalyzeCorrelationList = currentDbDcaEntryList.OrderBy(r => r.CreateAt).Select(rec => rec.Correlation).ToList();
                    var threadCorrList = DCAEntryList.OrderBy(r => r.CreateAt).Select(rec => rec.Correlation).ToList();
                    
                    //var newEntryList = DCAEntryList.Where(rec => !wait2AnalyzeCorrelationList.Contains(rec.Correlation));
                    var newCorrelationList = acurateWait2AnalyzeCorrelationList.Where(correlation => !threadCorrList.Contains(correlation));
                    //!_DCAEntryList.Where( rec =>rec.AnalyzeAt==null ).SequenceEqual(correlationWait4AnalyzeDb))

                    if (newCorrelationList.Count() > 0)
                    {
                        
                        AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Critical);
                        _logger.AppendLine("Opps newEntryList !!!! " + newCorrelationList.Aggregate((i1, i2) => i1 + "," + i2));
                        foreach (var newEntry in currentDbDcaEntryList.Where(r => newCorrelationList.Contains(r.Correlation)).OrderBy(r => r.CreateAt))
                        {
                            if (DCAEntryList.Exists(r => r.Correlation == newEntry.Correlation))
                            {
                                throw new Exception("if (DCAEntryList.Exists(r => r.Correlation == newEntry.Correlation))"); 
                            }
                            else
                            {
                                DCAEntryList.Add(newEntry);
                            }
                            

                        }

                        DCAEntryList = DCAEntryList.OrderBy(r => r.CreateAt).ToList();
                        

                    }
                }
                ///_logger.AppendLine(correlationWait4Analyze.Aggregate((i1, i2) => i1 + "," + i2));
                
                return dbList;
                


            }
            finally
            {
                if (sw.Elapsed > TimeSpan.FromSeconds(25))
                {
                    throw new Exception("List<CustomsRequestsSheetPM> _CustomsRequestsSheetQueryService.GetAllReceivedDca(string dcaAnalyzeAggregateKey) timeout 15 sec!!!! Please Create index !!");
                }
            }


        }
    }
    class DcaLogEntry
    {
        public int Seq { get; set; }
        public DateTime CreateAt { get; set; }
        public string Correlation { get; set; }
        public DateTime? AnalyzeAt { get; set; }
        
        //public int SeqCorrection { get; set; }

        public override string ToString()
        {
            return (new StringBuilder())
                .Append(Seq.ToString()).Append('\t')
                .Append(CreateAt.ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture)).Append('\t')
                .Append(Correlation.ToString()).Append('\t')
                .Append(AnalyzeAt.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm:ss.fff",
                                            CultureInfo.InvariantCulture)).Append('\t')
                .ToString(); 
        }

        public static string HeaderToString()
        {
            return (new StringBuilder())
                .Append("Seq                 ").Append('\t')
                .Append("CreateAt            ").Append('\t')
                .Append("Correlation         ").Append('\t')
                .Append("AnalyzeAt           ").Append('\t')
                .ToString();
        }

    }
}
