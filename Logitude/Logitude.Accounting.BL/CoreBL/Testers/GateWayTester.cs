using Logitude.Accounting.BL.CoreBL.Reports;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Testers
{
    public class GateWayTester
    {
        public GateWayTesterResult TestIt(string operationId,int tenant, string _TextBoxParam)
        {
            switch (operationId)
            {
                case "_ButtonReverseTotal_Click":
                    {
                       return _ButtonReverseTotal_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "_ButtonReverseTrans_Click":
                    {
                        return _ButtonReverseTrans_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "WorkWithoutQueue_Click":
                    {
                        return WorkWithoutQueue_Click(tenant, _TextBoxParam);
                    }
                    break;
                case "_TrailReport_Click":
                    {
                        return _TrailReport_Click(tenant, _TextBoxParam);
                    }
                    break;

                case "Aging_Click":
                    {
                        return Aging_Click(tenant, _TextBoxParam);
                    }
                    break;


                case "CardIndexNew_Click":
                    {
                        return CardIndexNew_Click(tenant, _TextBoxParam);
                    }
                    break;

                default:
                    return new GateWayTesterResult()
                    {
                        ExceptionMess =
                        $"No operationId  {operationId}"
                    };
                    break;
            }
        }

        private GateWayTesterResult Aging_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                var myAgingReportParam = LogitudeXmlSerializer.JsonConvertDeserializeTObject<AgingReportParam>(textBoxParam);
                //using (
                var agingReport = new AgingReportService(myAgingReportParam);
                var xml = agingReport.RunReport();
                //var MyPeriodList = agingReport.MyPeriodList;
                var xmlMyPeriodList = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<PeriodMExtended>>(agingReport.MyPeriodExtendedList);

                gateWayTesterResult.Log = xmlMyPeriodList;


                gateWayTesterResult.JsonOut = xml;


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }


        private GateWayTesterResult _TrailReport_Click(int tenant, string textBoxParam)         
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<TrailReportParam>(textBoxParam);
                //using (
                var trailReportService = TrailReportFactory.CreateNew(param);//)



                var res = trailReportService.Execute();

                gateWayTesterResult.Log = trailReportService.DbLog;
                

                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<TrailReportM>>(res);


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }

        
             
        private GateWayTesterResult CardIndexNew_Click(int tenant, string textBoxParam)
        {

            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<CardIndexReportParams>(textBoxParam);
                //using (
                var accountingContext = AccountingContext.GetContext(tenant);
                var CardIndexReportService = new CardIndexReportService(accountingContext, param);
                CardIndexReportService.Run();

                //gateWayTesterResult.Log = trailReportService.DbLog;


                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<LedgerTransactionBalanceResponse>>(CardIndexReportService.CardIndexs);


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {

                gateWayTesterResult.Log = gateWayTesterResult.Log ?? "";
                gateWayTesterResult.Log += LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }

        private GateWayTesterResult WorkWithoutQueue_Click(int tenant, string textBoxParam)
        
        {
            dynamic param = null;
            var gateWayTesterResult = new GateWayTesterResult();


            try
            {


                param = LogitudeXmlSerializer.JsonConvertDeserializeObject(textBoxParam);
                int tenantFrom = param.Tenant;
                string JournalId = param.JournalId;
                List<string> Last_journalBufferKeys = null;
                JournalApproveService.WorkWithoutQueue(tenantFrom, JournalId, ref Last_journalBufferKeys);

            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }

        private GateWayTesterResult _ButtonReverseTrans_Click(int tenant, string textBoxParam)
        {
         
            var gateWayTesterResult = new GateWayTesterResult();


            try
            {


                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(textBoxParam);
                var s = new ReverseEngineerLedgerTransactionService(param.MyDate, param.MyTenant);
                s.CheckDbIntegrity();

                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<JournalLineLedgerDTO>>(s.CompareReport.rows);


            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log = LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }

        GateWayTesterResult _ButtonReverseTotal_Click(int tenant ,string _TextBoxParam)
        {
            var gateWayTesterResult = new GateWayTesterResult();


            try
            {
                

                var param = LogitudeXmlSerializer.JsonConvertDeserializeTObject<ParamBasic>(_TextBoxParam);
                var s = new ReverseEngineerTotalByMonthService(param.MyDate, param.MyTenant, param.MyGLAccId);
                s.CheckDbIntegrity();
                
                gateWayTesterResult.JsonOut = LogitudeXmlSerializer.SerializeObjectToJosnStringMax<List<GLAccountTotalByMonthsDTO>>(s.CompareReport.GLAccountTotalByMonthsList);
                ///ReloadGrid(SerializeObjectByte);

            }
            catch (Exception eee)
            {
                //param = null;
                //throw;
                gateWayTesterResult.ExceptionMess = eee.ToString();
            }
            finally
            {


                gateWayTesterResult.Log= LogMessagingUtil.Instance.ToString();
            }
            return gateWayTesterResult;
        }
    }
    public class GateWayTesterResult
    {
        public string JsonOut { get; set; }
        public string Log { get; set; }
        public string ExceptionMess { get; set; }

    }
    public class ParamBasic
    {
        public int MyTenant { get; set; }
        public DateTime MyDate { get; set; }

        public string MyGLAccId { get; set; }
    }
}
