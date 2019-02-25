using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.ILOVS;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Utils;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.Maman
{
    public class Send2MasofIfNeededService
    {
        public void Send2Masof(DeclarationPM drityEntityPM,bool pHaveChange=false)
        {
            try
            {

                List<string> listStorageDefault = GetlistStorageDefault(drityEntityPM);

                if (!drityEntityPM.IsCourierDeclaration || drityEntityPM.Consignments == null && drityEntityPM.ChangeSetOp == ChangeSetOperation.Delete)
                {
                    return;
                }
                if (listStorageDefault.Count == 0)
                {
                    return;
                }
                bool dataHaveChangeSendIt = false;
                var myStorageSiteCode = drityEntityPM.Consignments
                    .Where(r => !string.IsNullOrWhiteSpace(r.StorageSiteCode))
                    .Where(r => listStorageDefault.Contains(r.StorageSiteCode))
                    .Select(r => r.StorageSiteCode)
                    .FirstOrDefault();
                var qs = new DeclarationQueryService(drityEntityPM.Tenant);
                var dbPM = qs.GetSingle(drityEntityPM.Id, true, false);
                if (drityEntityPM.ChangeSetOp == ChangeSetOperation.Insert)
                {
                    dataHaveChangeSendIt = true;
                }
                if (pHaveChange)
                {
                    dataHaveChangeSendIt = true;
                }
                string drityMessage = "";
                string dbMessage = "";

                if (listStorageDefault.Contains("ILMMN") && myStorageSiteCode == "ILMMN") // Maman
                {

                    var courierGWMessageECTHRDataMamanService = new CourierGWMessageECTHRDataMamanRequestService();
                    drityMessage = courierGWMessageECTHRDataMamanService.GetMessage2Maman(drityEntityPM.Id, drityEntityPM.Tenant, drityEntityPM, null);
                    if (!dataHaveChangeSendIt && dbPM != null)
                    {
                        
                        dbMessage = courierGWMessageECTHRDataMamanService.GetMessage2Maman(dbPM.Id, dbPM.Tenant, dbPM, null);
                        
                        if (!ProxyUtil.ArrayJsonAreEqual(drityMessage, dbMessage,
                            new List<string>() {
                                "BaldarMessageTime"
                            }))
                        {
                            dataHaveChangeSendIt = true;
                        }
                    }
                    if (dataHaveChangeSendIt)
                    {

                        List<string> requiredField = courierGWMessageECTHRDataMamanService.GetRequiredField(drityMessage);
                        if (requiredField.Count > 0)
                        {
                            Debug.WriteLine($"חסרים שדות חובה :{String.Join(",", requiredField)}");
                            return;// $"חסרים שדות חובה :{String.Join(",", requiredField)}";
                        }
                        var res = courierGWMessageECTHRDataMamanService.BuildComm2Maman(drityEntityPM.Id, drityEntityPM.Tenant, drityMessage);
                        Debug.WriteLine(res);
                    }



                }
                else if (listStorageDefault.Contains("ILOVL") && myStorageSiteCode == "ILOVL") // OVS
                {
                    var courierGWMessageECTHRDataMamanService = new CourierOVSECTHMessageRequestService();
                    drityMessage = courierGWMessageECTHRDataMamanService.GetMessageUpdateHawbStatus(drityEntityPM.Id, drityEntityPM.Tenant, drityEntityPM, null);
                    if (!dataHaveChangeSendIt && dbPM != null)
                    {

                        dbMessage = courierGWMessageECTHRDataMamanService.GetMessageUpdateHawbStatus(dbPM.Id, dbPM.Tenant, dbPM, null);
                        
                        if (dbMessage != drityMessage)
                        {
                            dataHaveChangeSendIt = true;
                        }
                    }
                    if (dataHaveChangeSendIt)
                    {

                        List<string> requiredField = courierGWMessageECTHRDataMamanService.GetRequiredField(drityMessage);
                        if (requiredField.Count > 0)
                        {
                            Debug.WriteLine($"חסרים שדות חובה :{String.Join(",", requiredField)}");
                            return;// $"חסרים שדות חובה :{String.Join(",", requiredField)}";
                        }
                        var res = courierGWMessageECTHRDataMamanService.BuildUpdateHawbStatus(drityEntityPM.Id, drityEntityPM.Tenant, drityMessage);
                        Debug.WriteLine(res);
                    }

                }

            }
            catch (Exception e)
            {
                //e.SetMess
                //throw;
            }



        }

        private static List<string> GetlistStorageDefault(DeclarationPM drityEntityPM)
        {
            var amitalContext = AmitalContext.GetContext(drityEntityPM.Tenant);
            var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);
            var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", false, true);
            def.DEFDATA = def.DEFDATA ?? "";

            var listStorageDefault = new List<string>();//&& declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL"
            if (def.DEFDATA.Contains("ILMMN")) // Maman
            {
                listStorageDefault.Add("ILMMN");
            }
            if (def.DEFDATA.Contains("ILOVL")) // OVS
            {
                listStorageDefault.Add("ILOVL");
            }

            return listStorageDefault;
        }
    }
}
