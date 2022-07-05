
using System;
using System.Collections.Generic;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityPMs;
using Unifreight.Data.AmitalModel;
using Unifreight.BL.EntityPMs.UGenerated;
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class ETBVENDDataMapping : IMapping<ETBVENDPM, ETBVEND>
    {
        public void PMToPOCO(ETBVENDPM entityPM, ETBVEND entityPOCO)
        {
            entityPOCO.VENDORID = entityPM.VENDORID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.STATE = entityPM.STATE;
            entityPOCO.FAX = entityPM.FAX;
            entityPOCO.ADDRESS = entityPM.ADDRESS;
            entityPOCO.SEARCHENG = entityPM.SEARCHENG;
            entityPOCO.ISHANDAGNT = entityPM.ISHANDAGNT;
            entityPOCO.CITY = entityPM.CITY;
            entityPOCO.ADDRESS2 = entityPM.ADDRESS2;
            entityPOCO.COUNTRY = entityPM.COUNTRY;
            entityPOCO.ZIPCODE = entityPM.ZIPCODE;
            entityPOCO.VENDORPREFIX = entityPM.VENDORPREFIX;
            entityPOCO.CARDID = entityPM.CARDID;
            entityPOCO.CHECKDIGIT = entityPM.CHECKDIGIT;
            entityPOCO.PRINTRATE = entityPM.PRINTRATE;
            entityPOCO.TMPACCOUNTNO = entityPM.TMPACCOUNTNO;
            entityPOCO.CHARACTERS = entityPM.CHARACTERS;
            entityPOCO.TAM = entityPM.TAM;
            entityPOCO.TEL = entityPM.TEL;
            entityPOCO.CONTACT = entityPM.CONTACT;
            entityPOCO.EMAIL = entityPM.EMAIL;
            entityPOCO.BILLTO = entityPM.BILLTO;
            entityPOCO.FILLERB1 = entityPM.FILLERB1;
            entityPOCO.FILLERB2 = entityPM.FILLERB2;
        }

        public void POCOToPM(ETBVENDPM entityPM, ETBVEND entityPOCO)
        {
            entityPM.VENDORID = entityPOCO.VENDORID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.ADDRESS = entityPOCO.ADDRESS;
            entityPM.SEARCHENG = entityPOCO.SEARCHENG;
            entityPM.ISHANDAGNT = entityPOCO.ISHANDAGNT;
            entityPM.CITY = entityPOCO.CITY;
            entityPM.ADDRESS2 = entityPOCO.ADDRESS2;
            entityPM.STATE = entityPOCO.STATE;
            entityPM.COUNTRY = entityPOCO.COUNTRY;
            entityPM.ZIPCODE = entityPOCO.ZIPCODE;
            entityPM.VENDORPREFIX = entityPOCO.VENDORPREFIX;
            entityPM.CARDID = entityPOCO.CARDID;
            entityPM.CHECKDIGIT = entityPOCO.CHECKDIGIT;
            entityPM.PRINTRATE = entityPOCO.PRINTRATE;
            entityPM.TMPACCOUNTNO = entityPOCO.TMPACCOUNTNO;
            entityPM.CHARACTERS = entityPOCO.CHARACTERS;
            entityPM.TAM = entityPOCO.TAM;
            entityPM.TEL = entityPOCO.TEL;
            entityPM.FAX = entityPOCO.FAX;
            entityPM.CONTACT = entityPOCO.CONTACT;
            entityPM.EMAIL = entityPOCO.EMAIL;
            entityPM.BILLTO = entityPOCO.BILLTO;
            entityPM.FILLERB1 = entityPOCO.FILLERB1;
            entityPM.FILLERB2 = entityPOCO.FILLERB2;


        }

        public void CustomPMToPOCO(ETBVENDPM entityPM, ETBVEND entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ETBVENDPM entityPM, ETBVEND entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ETBVENDPM entityPM, ETBVENDPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
