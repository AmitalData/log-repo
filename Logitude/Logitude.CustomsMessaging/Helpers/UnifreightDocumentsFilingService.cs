using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
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
            base.MetaDataVersionValue = DeclarationNumVersionId;
        }
        public bool OnlyIfChangeUpdateAndAddVersion { get; set; }
        
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
                if (OnlyIfChangeUpdateAndAddVersion && fileData!=null && base.MyUniFileVerM!=null)
                {
                    if (MD5HashUtil.GetMD5Hash(fileData)== base.MyUniFileVerM.MD5HASH)
                    {
                        LogMessagingUtil.Instance.AppendLine("UnifreightDocumentsFilingService.update() OnlyIfChangeUpdate=true but  MD5HashUtil.GetMD5Hash(fileData)== base.MyUniFileVerM.MD5HASH nothing change => stop DocumentsFilingService.update !!== dont add task/queue hybrid message");
                        LogMessagingUtil.Instance.AppendLine("אם אין הבדל - לא ליצור בכלל ממשק ליוניפרייט");
                        return;
                    }
                    int nextVer = base.MyUniFileVerM.VERSION + 1;
                    base.MetaDataVersionValue = $"{theEntityPm.Id}-{nextVer}";
                    LogMessagingUtil.Instance.AppendLine("UnifreightDocumentsFilingService.update() AddVersion2MetaDataVersionValue=true base.MetaDataVersionValue =" + base.MetaDataVersionValue);
                    LogMessagingUtil.Instance.AppendLine("c.	אם יש הבדל, יש ליצור ממשק שיצור גרסה חדשה למסמך הקיים");

                }
            }
            
            base.Update(theEntityPm, fileData, loggedUserId, FromService);
        }
         
    }
}
