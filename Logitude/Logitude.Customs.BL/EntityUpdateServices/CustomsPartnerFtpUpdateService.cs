using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Utils;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsPartnerFtpUpdateService : EntityUpdateService<CustomsPartnerFtp, CustomsPartnerFtpPM, EntityPM>
    {
        protected override void OnCreating(CustomsPartnerFtpPM entityPM, EntityPM entityParentPM)
        {
            entityPM.Id = IdCounter.GetNumber("Customs.CustomsPartnerFtp", entityPM.Tenant).ToString();
            base.OnCreating(entityPM, entityParentPM);
        }

        protected override void AfterUpdating(CustomsPartnerFtpPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Delete)
            {
                if (!String.IsNullOrWhiteSpace(entityPM.FtpDetailsId))
                {
                    var ftpRepo = new FTPDetailRepository(entityPM.Tenant);
                    var pocoFTP = ftpRepo.GetSingleFTPDetail(entityPM.FtpDetailsId, entityPM.Tenant);
                    ftpRepo.Remove(pocoFTP);
                    ftpRepo.SubmitChanges();
                }
            }

            base.AfterUpdating(entityPM, entityParentPM);

        }
        protected override void OnUpdating(CustomsPartnerFtpPM entityPM)
        {

            
            //entityPM.FileName = entityPM.FileName.ToUpper() 
            entityPM.InterfaceName = entityPM.InterfaceName ?? "";
            entityPM.InterfaceName = entityPM.InterfaceName.ToUpper();


            entityPM.TypeCode = entityPM.TypeCode ?? "";
            entityPM.TypeCode = entityPM.TypeCode.ToUpper();

            entityPM.PartnerCode = entityPM.PartnerCode ?? "";
            entityPM.PartnerCode = entityPM.PartnerCode.ToUpper();
            if (!string.IsNullOrWhiteSpace( entityPM.InterfaceName))
            {
                var myCustomsPartnerFtpDetails = new CustomsPartnerFtpDetails();
                var myinterfaceDef =myCustomsPartnerFtpDetails.GetAllInterfaceName().FirstOrDefault(r=>r.Key== entityPM.InterfaceName);
                if (string.IsNullOrWhiteSpace(myinterfaceDef.Key))
                {
                    //maybe to clear all ??!?!
                }
                else
                {
                    var def=ProxyUtil.JsonConvertDeserializeTyped<InterfaceDetails>(myinterfaceDef.Value) ;
                    switch (def.ViaMethod)
                    {
                        case "WEBAPI":
                            {
                                
                            }
                            break;
                        case "FTP":
                        default:
                            {
                                entityPM.CommunicationDetails = null;
                            }
                            break;
                    }
                }

            }
            base.OnUpdating(entityPM);
        }
    }
}
