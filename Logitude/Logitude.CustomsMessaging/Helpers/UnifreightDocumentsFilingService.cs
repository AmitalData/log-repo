using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;

namespace Logitude.CustomsMessaging.Helpers
{
    public class UnifreightDocumentsFilingService : DocumentsFilingService
    {

        public bool FeatureIsOn = true;

        public UnifreightDocumentsFilingService(ICommonDataContext objectContext, int tenant,string DeclarationNumVersionId = null)
            :base(objectContext, tenant)
        {
            base.DeclarationNumVersionId = DeclarationNumVersionId;
        }
        new public void Update(DocumentsFilingPM theEntityPm, byte[] fileData = null, string loggedUserId = null, bool FromService = false)
        {

            
            var uniGDMFILINGQueryService = new GDMFILINGQueryService(AmitalContext.GetContext(theEntityPm.Tenant));
            
            var IsUnifreightFillingMode = BlobFileInfoExt.IsUnifreightFillingModeBase(theEntityPm.Tenant,
                "docsin" //-- must call from CostomMessage that create "docsin" !!!
                );
            if (FeatureIsOn && IsUnifreightFillingMode )
            {

                var gdmfiling = uniGDMFILINGQueryService.GetSingle(theEntityPm.Id, true);
                if (gdmfiling != null)
                {


                    var lst = gdmfiling.LASTVERSION;
                    var lstGDMFILEVER = gdmfiling.GDMFILEVERs
                        //maybe latter .FirstOrDefault
                        .First
                        (r => r.VERSION == lst);
                    base.MyUniFileVerM = new UniFileVerM()
                    {
                        COMID = lstGDMFILEVER.COMID,
                        VERSION = lstGDMFILEVER.VERSION,
                        MD5HASH = lstGDMFILEVER.MD5HASH,
                        EXTENSION = lstGDMFILEVER.EXTENSION,
                    };
                }
            }
            
            base.Update(theEntityPm, fileData, loggedUserId, FromService);
        }
         
    }
}
