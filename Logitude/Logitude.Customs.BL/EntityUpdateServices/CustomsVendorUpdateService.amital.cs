using Logitude.AmitalMessaging.Infrastructure.SystemTable;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.Amital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class CustomsVendorUpdateService
    {
        private void UpdateUnifreight(CustomsVendorPM dirtyCustomsVendorPM)
        {
            //if (dirtyCustomsVendorPM.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            //{
            //    return;
            //}
            ////todo:
            //return;
            Send2Amital("TableID",dirtyCustomsVendorPM);
        }
        private static void Send2Amital(string TableID,CustomsVendorPM dirtyCustomsVendorPM)
        {
            var setting = CustomsSettingQueryService.GetSettingByTenant(dirtyCustomsVendorPM.Tenant);
            if (setting != null)
            {
                if (!setting.IsConnectedToUniFreight)
                {
                    return;
                }
            }

            var myCUSTOMS_TABLE = new CUSTOMS_TABLE();
            //myCUSTOMS_TABLE.TABLECODE = TableID;
            myCUSTOMS_TABLE.TABLECODE = new TABLECODE[] { new TABLECODE { TABLECODE_ID = "CTBCUSTSUP" } }; ;
            var myTABLEDATAList = new List<TABLEDATA>();
            myTABLEDATAList.Add(
                new TABLEDATA()
                {
                    TABLEDATA_ID = dirtyCustomsVendorPM.VendorNumber,
                    TABLEDATA_NAME_ENG = dirtyCustomsVendorPM.VendorName,
                    TABLEDATA_NAME_HEB = dirtyCustomsVendorPM.VendorName,
                    TABLEDATA_ADDITIONALCODE1 = dirtyCustomsVendorPM.CountryCode,
                    TABLEDATA_ADDITIONALCODE2 = "",
                    //TABLEDATA_BLOCKED = "",
                    TABLEDATA_BLOCKED = dirtyCustomsVendorPM.InActive == true ? "T" : "F",
                    TABLEDATA_REMARKS = ""
                }
            );
            myCUSTOMS_TABLE.TABLECODE[0].TABLEDATA = myTABLEDATAList.ToArray();


            var myAmitalCommunicationModel = new AmitalCommunicationModelBase(
                Server.Tools.Models.AmitalStandardCommunicationModel.OperationMethod.DataAccess, "YTBFLOGITABLE", "UpdateTable")
            {
                //Tenant = 0,
                Tenant = dirtyCustomsVendorPM.Tenant,
                objectTableName = null,


                //CommunicationLoggingEntityReference = dirtyDeclarationPM.DeclarationNumber,
                //EntityId = dirtyDeclarationPM.Id,
                //UserId = loggingUserId,
                CommunicationSubject = "IIG Table arrived from Logitude ",

            };
            var myUServerCommunicationService = new UServerCommunicationService<AmitalCommunicationModelBase, CUSTOMS_TABLE>(myAmitalCommunicationModel, myCUSTOMS_TABLE);
            myUServerCommunicationService.Send();

        }
    }
}
