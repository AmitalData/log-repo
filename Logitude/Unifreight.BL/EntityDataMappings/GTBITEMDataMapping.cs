using System;
using System.Collections.Generic;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;

using Unifreight.Data.AmitalModel.EntityPOCOs;
using Unifreight.BL.EntityPMs;

namespace Unifreight.BL.EntityDataMappings
{
    public class GTBITEMDataMapping : IMapping<GTBITEMPM, GTBITEM>
    {
        public void PMToPOCO(GTBITEMPM entityPM, GTBITEM entityPOCO)
        {
            entityPOCO.PARTNERID = entityPM.PARTNERID;
            entityPOCO.ITEMID = entityPM.ITEMID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.SEARCHENG = entityPM.SEARCHENG;
            entityPOCO.UNITID = entityPM.UNITID;
            entityPOCO.PRICEQTY = entityPM.PRICEQTY;
            entityPOCO.COINID = entityPM.COINID;
            entityPOCO.PRICE = entityPM.PRICE;
            entityPOCO.HARMONIZEID = entityPM.HARMONIZEID;
            entityPOCO.PRATID = entityPM.PRATID;
            entityPOCO.WEIGHT = entityPM.WEIGHT;
            entityPOCO.WEIGHTUM = entityPM.WEIGHTUM;
            entityPOCO.SERIAL = entityPM.SERIAL;
            entityPOCO.ITEMTYPE = entityPM.ITEMTYPE;
            entityPOCO.CHARGE = entityPM.CHARGE;
            entityPOCO.NOOVERHEAD = entityPM.NOOVERHEAD;
            entityPOCO.ORIGINCOUNTRY = entityPM.ORIGINCOUNTRY;
            entityPOCO.STANDARTSIV = entityPM.STANDARTSIV;
            entityPOCO.LICENSENO = entityPM.LICENSENO;
            entityPOCO.DESCRIPTION = entityPM.DESCRIPTION;
            entityPOCO.LICENCESIV = entityPM.LICENCESIV;
            entityPOCO.DESCRIPTIONHEB = entityPM.DESCRIPTIONHEB;
            entityPOCO.NOSTANDART = entityPM.NOSTANDART;
            entityPOCO.APPROVTYPEID = entityPM.APPROVTYPEID;
        }

        public void POCOToPM(GTBITEMPM entityPM, GTBITEM entityPOCO)
        {
            entityPM.PARTNERID = entityPOCO.PARTNERID;
            entityPM.ITEMID = entityPOCO.ITEMID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.SEARCHENG = entityPOCO.SEARCHENG;
            entityPM.UNITID = entityPOCO.UNITID;
            entityPM.PRICEQTY = entityPOCO.PRICEQTY;
            entityPM.COINID = entityPOCO.COINID;
            entityPM.PRICE = entityPOCO.PRICE;
            entityPM.HARMONIZEID = entityPOCO.HARMONIZEID;
            entityPM.PRATID = entityPOCO.PRATID;
            entityPM.WEIGHT = entityPOCO.WEIGHT;
            entityPM.WEIGHTUM = entityPOCO.WEIGHTUM;
            entityPM.SERIAL = entityPOCO.SERIAL;
            entityPM.ITEMTYPE = entityPOCO.ITEMTYPE;
            entityPM.CHARGE = entityPOCO.CHARGE;
            entityPM.NOOVERHEAD = entityPOCO.NOOVERHEAD;
            entityPM.ORIGINCOUNTRY = entityPOCO.ORIGINCOUNTRY;
            entityPM.STANDARTSIV = entityPOCO.STANDARTSIV;
            entityPM.LICENSENO = entityPOCO.LICENSENO;
            entityPM.DESCRIPTION = entityPOCO.DESCRIPTION;
            entityPM.LICENCESIV = entityPOCO.LICENCESIV;
            entityPM.DESCRIPTIONHEB = entityPOCO.DESCRIPTIONHEB;
            entityPM.NOSTANDART = entityPOCO.NOSTANDART;
            entityPM.APPROVTYPEID = entityPOCO.APPROVTYPEID;
        }

        public void CustomPMToPOCO(GTBITEMPM entityPM, GTBITEM entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTBITEMPM entityPM, GTBITEM entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTBITEMPM entityPM, GTBITEMPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}

