
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
    public class CFIMSVSTATLDataMapping : IMapping<CFIMSVSTATLPM, CFIMSVSTATL>
    {
        public void PMToPOCO(CFIMSVSTATLPM entityPM, CFIMSVSTATL entityPOCO)
        {
            entityPOCO.GUID = entityPM.GUID;
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.CREATEDATE = entityPM.CREATEDATE;
            entityPOCO.PAGENUM = entityPM.PAGENUM;
            entityPOCO.LINENUM = entityPM.LINENUM;
            entityPOCO.LINECOUNTER = entityPM.LINECOUNTER;
            entityPOCO.PRATMEHES = entityPM.PRATMEHES;
            entityPOCO.TARIFFCODE = entityPM.TARIFFCODE;
            entityPOCO.CUSTOMSSUPPLIERID = entityPM.CUSTOMSSUPPLIERID;
            entityPOCO.CUSTOMERID = entityPM.CUSTOMERID;
            entityPOCO.CATALOGID = entityPM.CATALOGID;
            entityPOCO.CATALOGNAME = entityPM.CATALOGNAME;
            entityPOCO.ITEMPRICE = entityPM.ITEMPRICE;
            entityPOCO.OCRQUANTITY = entityPM.OCRQUANTITY;
            entityPOCO.OCRQUANTITYTYPE = entityPM.OCRQUANTITYTYPE;
            entityPOCO.AMOUNT = entityPM.AMOUNT;
            entityPOCO.ORIGINID = entityPM.ORIGINID;

        }

        public void POCOToPM(CFIMSVSTATLPM entityPM, CFIMSVSTATL entityPOCO)
        {
            entityPM.GUID = entityPOCO.GUID;
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.COMID = entityPOCO.COMID;
            entityPM.CREATEDATE = entityPOCO.CREATEDATE;
            entityPM.PAGENUM = entityPOCO.PAGENUM;
            entityPM.LINENUM = entityPOCO.LINENUM;
            entityPM.LINECOUNTER = entityPOCO.LINECOUNTER;
            entityPM.PRATMEHES = entityPOCO.PRATMEHES;
            entityPM.TARIFFCODE = entityPOCO.TARIFFCODE;
            entityPM.CUSTOMSSUPPLIERID = entityPOCO.CUSTOMSSUPPLIERID;
            entityPM.CUSTOMERID = entityPOCO.CUSTOMERID;
            entityPM.CATALOGID = entityPOCO.CATALOGID;
            entityPM.CATALOGNAME = entityPOCO.CATALOGNAME;
            entityPM.ITEMPRICE = entityPOCO.ITEMPRICE;
            entityPM.OCRQUANTITY = entityPOCO.OCRQUANTITY;
            entityPM.OCRQUANTITYTYPE = entityPOCO.OCRQUANTITYTYPE;
            entityPM.AMOUNT = entityPOCO.AMOUNT;
            entityPM.ORIGINID = entityPOCO.ORIGINID;

        }

        public void CustomPMToPOCO(CFIMSVSTATLPM entityPM, CFIMSVSTATL entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIMSVSTATLPM entityPM, CFIMSVSTATL entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIMSVSTATLPM entityPM, CFIMSVSTATLPM oldEntityPM)
        {
           // throw new System.NotImplementedException();
        }
    }
}
