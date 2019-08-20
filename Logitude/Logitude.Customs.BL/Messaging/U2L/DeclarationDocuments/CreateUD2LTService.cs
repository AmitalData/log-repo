using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Messaging.U2L.DeclarationDocuments
{
    class CreateUD2LTService ///: ICreateUD2LTService 
    {
        private DocumentsFilingPM _DocumentsFilingPM;

         void //JustDoIt(string DocumentsFilingId, int tenant)
            JustDoIt1(object documentsFilingPM)
        {
            DeclarationPM declarationPM;
            Debug.WriteLine("CreateUD2LTService");
            try
            {
                //var mappingService = new DocumentsFilingQueryService(tenant);
                //var entity = mappingService.GetDocumentsFilingById(DocumentsFilingId, tenant);
                //_DocumentsFilingPM = mappingService.DocumentsFilingCustomDataMappingAndValidating(entity, tenant, false);

                _DocumentsFilingPM = documentsFilingPM as DocumentsFilingPM;
                if (_DocumentsFilingPM == null)
                {
                    Debug.WriteLine("_DocumentsFilingPM == null");
                    return;
                }
                string DocumentsFilingId = _DocumentsFilingPM.Id;
                int tenant = _DocumentsFilingPM.Tenant;
                if (!IsConnected2Declaration())
                {
                    Debug.WriteLine("!IsConnected2Decalaration()");
                    return;
                }

                bool shouldCreateDCAComm = false;
                if (CourierENV() )
                {
                    Debug.WriteLine("CourierENV");
                    shouldCreateDCAComm = true;
                }
                else
                {
                    //CGG_DEC_DOC_CLT
                    var amitalContext = AmitalContext.GetContext(tenant);
                    var myGDFDATAQueryService = new GDFDATAQueryService(amitalContext);
                    var def = myGDFDATAQueryService.GetSingle("ISRAEL", "CGG_DEC_DOC_CLT", "NON", "NON", false, true);
                    def.DEFDATA = def.DEFDATA ?? "";
                    if (!String.IsNullOrWhiteSpace(def.DEFDATA))
                    {
                        var listStorageDefault = new List<string>();//&& declaration.Consignments.FirstOrDefault().StorageSiteCode == "ILOVL"
                        var decQS = new DeclarationQueryService(tenant);
                        declarationPM = decQS.GetSingle(this._DocumentsFilingPM.EntityId, false, false);

                        if (def.DEFDATA.Contains(declarationPM.CustomerId)) // Maman
                        {
                            Debug.WriteLine("def.DEFDATA.Contains(declarationPM.CustomerId)");
                            shouldCreateDCAComm = true;
                        }
                    }

                }
                if (!shouldCreateDCAComm)
                {
                    Debug.WriteLine("!shouldCreateDCAComm");
                    return;
                }

                Debug.WriteLine("CreateCRS");
                //var myDCAInUCBUD2LT_MsgMessagingService = new DCAInUCBUD2LT_MsgMessagingService();

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

            }

        }

       

        private bool CourierENV()
        {
            var qs = new CustomsSettingQueryService(_DocumentsFilingPM.Tenant);
            var pm=qs.GetSettingByTenantN(_DocumentsFilingPM.Tenant);
            return false;// pm.companytype=="B"
        }

        private bool IsConnected2Declaration()
        {
            return (this._DocumentsFilingPM.ObjectTableId == ObjectTableRepository.GetObjectTableByName("Customs.Declaration") && !String.IsNullOrWhiteSpace(this._DocumentsFilingPM.EntityId));
        }
    }
}
