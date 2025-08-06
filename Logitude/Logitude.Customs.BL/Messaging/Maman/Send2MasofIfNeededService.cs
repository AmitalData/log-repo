using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.BL.Messaging.ILSWS;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class Send2MasofIfNeededService
    {

        [ThreadStatic]
        public static bool SuppressSend=false;
        [ThreadStatic]
        public static bool IsNewFromU2L=false;
         public void Send2Masof(DeclarationPM drityEntityPM,bool pHaveChange, DeclarationPM dbPM,bool forceSend=false, CourierMasterPM courierMasterPM=null)
         {
            var sb=new StringBuilder();
            try
            {
                sb.Append($"{drityEntityPM.Id};CustomFileNo:{drityEntityPM.CustomFileNo};SuppressSend == {SuppressSend}")
                    .Append(drityEntityPM.Consignments == null ? "NoConsignments" : "HaveConsignments")
                    .Append(";").Append(drityEntityPM.ChangeSetOp.ToString()).Append(";")
                    ;
                if (SuppressSend == true)
                {
                    
                    return;
                }
                if (!drityEntityPM.IsCourierDeclaration)
                {
                    return;
                }
                if (drityEntityPM.Consignments == null)
                {
                    return;
                }
                if (drityEntityPM.ChangeSetOp == ChangeSetOperation.Delete)
                {
                    return;
                }
                List<string> listStorageDefault = GetlistStorageDefault(drityEntityPM);
                sb.Append($";listStorageDefault={listStorageDefault.Count};");
                if (listStorageDefault.Count == 0)
                {
                    return;
                }
                sb.Append($";{string.Join(",",listStorageDefault.ToArray())};");
                bool dataHaveChangeSendIt = false;
                var myStorageSiteCode = drityEntityPM.Consignments
                    .Where(r => !string.IsNullOrWhiteSpace(r.StorageSiteCode))
                    .Where(r => listStorageDefault.Contains(r.StorageSiteCode))
                    .Select(r => r.StorageSiteCode)
                    .FirstOrDefault();
                sb.Append($"myStorageSiteCode={myStorageSiteCode} IS NULL ???");
                if (dbPM==null)
                {
                    var qs = new DeclarationQueryService(drityEntityPM.Tenant);
                    dbPM = qs.GetSingle(drityEntityPM.Id, true, false);

                }
                if (drityEntityPM.ChangeSetOp == ChangeSetOperation.Insert)
                {
                    dataHaveChangeSendIt = true;
                }
                if (pHaveChange)
                {
                    dataHaveChangeSendIt = true;
                }
                if (dbPM != null)
                {
                    var prev_site = dbPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().StorageSiteCode;
                    var current_site = drityEntityPM.Consignments.DefaultIfEmpty(new ConsignmentPM()).First().StorageSiteCode;
                    if(prev_site != current_site)
                    {
                        dataHaveChangeSendIt = true;
                        sb.AppendLine("site change-SEND!!!");
                    }
                }
                
                string drityMessage = "";
                string dbMessage = "";

                if (listStorageDefault.Contains("ILMMN") && myStorageSiteCode == "ILMMN") // Maman
                {
                    sb.AppendLine("ILMMN!!!");
                    var courierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanRequestService();
                    drityMessage = courierGWMessageECTHRDataMamanService.GetMessage2Maman(drityEntityPM.Id, drityEntityPM.Tenant, drityEntityPM, courierMasterPM);
                    if(drityMessage == null)
                    {
                        return;
                    }
                    if (!dataHaveChangeSendIt && dbPM != null)
                    {
                        
                        dbMessage = courierGWMessageECTHRDataMamanService.GetMessage2Maman(dbPM.Id, dbPM.Tenant, dbPM, null);
                        
                        if(dbMessage == null)
                        {
                            return;
                        }
                        if (!ProxyUtil.ArrayJsonAreEqual(drityMessage, dbMessage,
                            new List<string>() {
                                "BaldarMessageTime"
                            }))
                        {
                            dataHaveChangeSendIt = true;
                            sb.AppendLine("Message:Changed-SEND!!!");
                        }
                        bool forceDueEcomUpsert = !string.IsNullOrWhiteSpace(drityEntityPM?.MyEcomInsert?.MyDeclarationCourierStatusPM?.DeclarationId);


                        if (forceSend || forceDueEcomUpsert)
                        {
                            dataHaveChangeSendIt = true;
                            sb.AppendLine("forceSend || forceDueEcomUpsert-SEND!!!");

                        }
                        else
                        {
                            
                            sb.AppendLine($"{Environment.MachineName}+;{drityEntityPM?.MyEcomInsert};-;{drityEntityPM?.MyEcomInsert?.MyDeclarationCourierStatusPM}+;{drityEntityPM?.MyEcomInsert?.MyCourierMasterPM}+;{drityEntityPM?.MyEcomInsert?.MyDeclarationCourierStatusPM?.DeclarationId}");

                            sb.AppendLine(Environment.StackTrace);
                        }
                    }
                    if (dataHaveChangeSendIt)
                    {

                        List<string> requiredField = courierGWMessageECTHRDataMamanService.GetRequiredField(drityMessage);
                        if (requiredField.Count > 0)
                        {
                           NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"חסרים שדות חובה :{String.Join(",", requiredField)}");
                            sb.AppendLine($"חסרים שדות חובה :{String.Join(",", requiredField)}");
                            return;// $"חסרים שדות חובה :{String.Join(",", requiredField)}";
                        }
                        var res = courierGWMessageECTHRDataMamanService.BuildComm2Maman(drityEntityPM.Id, drityEntityPM.Tenant, drityMessage);
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug(res);
                        sb.AppendLine(res);
                    }



                }
                else if (listStorageDefault.Contains("ILOVL") && myStorageSiteCode == "ILOVL") // OVS
                {
                    sb.AppendLine("ILOVL!!!");
                    var courierGWMessageECTHRDataMamanService = new CourierOVSECTHMessageRequestService();
                    drityMessage = courierGWMessageECTHRDataMamanService.GetMessageUpdateHawbStatus(drityEntityPM.Id, drityEntityPM.Tenant, drityEntityPM, courierMasterPM);
                    if (!dataHaveChangeSendIt && dbPM != null)
                    {

                        dbMessage = courierGWMessageECTHRDataMamanService.GetMessageUpdateHawbStatus(dbPM.Id, dbPM.Tenant, dbPM, courierMasterPM);
                        
                        if (dbMessage != drityMessage)
                        {
                            dataHaveChangeSendIt = true;
                            sb.AppendLine("dbMessage != drityMessage-SEND!!!");

                        }
                        bool forceDueEcomUpsert = !string.IsNullOrWhiteSpace(drityEntityPM?.MyEcomInsert?.MyDeclarationCourierStatusPM?.CrateNumber);
                        if (forceSend || forceDueEcomUpsert )
                        {
                            sb.AppendLine("forceSend || forceDueEcomUpsert-SEND!!!");
                            dataHaveChangeSendIt = true;

                        }
                    }
                    if (dataHaveChangeSendIt)
                    {

                        List<string> requiredField = courierGWMessageECTHRDataMamanService.GetRequiredField(drityMessage);
                        if (requiredField.Count > 0)
                        {
                           NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"חסרים שדות חובה :{String.Join(",", requiredField)}");
                            return;// $"חסרים שדות חובה :{String.Join(",", requiredField)}";
                        }
                        var res = courierGWMessageECTHRDataMamanService.BuildUpdateHawbStatus(drityEntityPM.Id, drityEntityPM.Tenant, drityMessage);
                       NetCommonHelper.Logger.DevLog.Instance.WriteDebug(res);
                        
                        sb.AppendLine(res);
                        
                    }
                    else
                    {

                        sb.AppendLine(Environment.StackTrace);
                    }

                }

              else if (listStorageDefault.Contains("ILSWS") && myStorageSiteCode == "ILSWS")
                {
                    
                        
                    sb.AppendLine("ILSWS!!!");
                    
                    if (drityEntityPM.CourierCustomStatusCode != dbPM.CourierCustomStatusCode || drityEntityPM.StorageSiteCode != dbPM.StorageSiteCode || IsNewFromU2L)
                    {
                         var courierECSWSTHRMessageRequestService = new CourierECSWSTHRMessageRequestService();
                        if (IsNewFromU2L) dataHaveChangeSendIt = true;
                        drityMessage = courierECSWSTHRMessageRequestService.GetMessageUpdateHawbStatus(drityEntityPM.Id, drityEntityPM.Tenant, drityEntityPM, courierMasterPM);

                        if (!dataHaveChangeSendIt && dbPM != null)
                        {

                            dbMessage = courierECSWSTHRMessageRequestService.GetMessageUpdateHawbStatus(dbPM.Id, dbPM.Tenant, dbPM, null);
                            
                            if (dbMessage != null && dbMessage != drityMessage)
                            {
                                sb.AppendLine("dbMessage != null && dbMessage != drityMessage-SEND!!!");
                                dataHaveChangeSendIt = true;
                            }
                        }
                        if (dataHaveChangeSendIt)
                        {

                            List<string> requiredField = courierECSWSTHRMessageRequestService.GetRequiredField(drityMessage);
                            if (requiredField.Count > 0)
                            {
                               NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"חסרים שדות חובה :{String.Join(",", requiredField)}");
                                return;// $"חסרים שדות חובה :{String.Join(",", requiredField)}";
                            }
                            var XMLdrityMessage = courierECSWSTHRMessageRequestService.DeserializeXmlNode(drityMessage);
                            var res = courierECSWSTHRMessageRequestService.BuildUpdateHawbStatus(drityEntityPM.Id, drityEntityPM.Tenant, XMLdrityMessage);
                           NetCommonHelper.Logger.DevLog.Instance.WriteDebug(res);
                            sb.AppendLine(res);
                        }
                        else
                        {

                            sb.AppendLine(Environment.StackTrace);
                        }
                    }
            }

        }
            catch (Exception e)
            {
                sb.AppendLine(e.ToString());
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                //e.SetMess
                //throw;
            }
            finally
            {
                //if (Logger.ToLogUntilDateyyyyMMdd("20230112HDCall409236.LogUntilDateyyyyMMdd"))
                //{
                    NetCommonHelper.Logger.DevLog.Instance.WriteInfo(sb.ToString());
                //}

            }



        }
        public static DateTime GetStopLogAt()
        {
            DateTime stopLogAt = new DateTime(2023, 02, 01);
            try
            {
                string UntilDateyyyyMMdd = System.Configuration.ConfigurationManager.AppSettings["20230112HDCall409236.LogUntilDateyyyyMMdd"];
                if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
                {
                    stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
                                                        "yyyyMMdd",
                                                        CultureInfo.InvariantCulture,
                                                        style: DateTimeStyles.None);
                }
            }
            catch (Exception)
            {

            }
            return stopLogAt;

        }
        private static List<string> GetlistStorageDefault(DeclarationPM drityEntityPM)
        {
            DefaultValueQueryService defaultValueQueryService = new DefaultValueQueryService(drityEntityPM.Tenant);

            string def = defaultValueQueryService.GetDefault("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", drityEntityPM.Tenant);


     
            def = def ?? "";

            var listStorageDefault = new List<string>();//&& declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL"
            if (def.Contains("ILMMN")) // Maman
            {
                listStorageDefault.Add("ILMMN");
            }
            if (def.Contains("ILOVL")) // OVS
            {
                listStorageDefault.Add("ILOVL");
            }
            if (def.Contains("ILSWS")) // OVS
            {
                listStorageDefault.Add("ILSWS");
            }

            return listStorageDefault;
        }
    }
}
