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
    public class CFIMSVLINEDataMapping : IMapping<CFIMSVLINEPM, CFIMSVLINE>
    {
        public void PMToPOCO(CFIMSVLINEPM entityPM, CFIMSVLINE entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.PAGENUM = entityPM.PAGENUM;
            entityPOCO.LINENUM = entityPM.LINENUM;
            entityPOCO.PRATMEHES = entityPM.PRATMEHES;
            entityPOCO.TARIFFCODE = entityPM.TARIFFCODE;
            entityPOCO.TOP = entityPM.TOP;
            entityPOCO.HEIGHT = entityPM.HEIGHT;
            entityPOCO.GROUPNUM = entityPM.GROUPNUM;
            entityPOCO.REMARK = entityPM.REMARK;
            entityPOCO.YEVU = entityPM.YEVU;
            entityPOCO.SUGGESTM = entityPM.SUGGESTM;
            entityPOCO.SUGGESTI = entityPM.SUGGESTI;
            entityPOCO.SUGGESTDET1 = entityPM.SUGGESTDET;
            entityPOCO.STATUS = entityPM.STATUS;
            entityPOCO.TAXEXEMPT = entityPM.TAXEXEMPT;
            entityPOCO.INVOICEQUANTITY = entityPM.INVOICEQUANTITY;
            entityPOCO.INVOICEQUANTITYTYPE = entityPM.INVOICEQUANTITYTYPE;
            entityPOCO.QUETYPE = entityPM.QUETYPE;
            entityPOCO.CATALOGID = entityPM.CATALOGID;
            entityPOCO.CATALOGNAME = entityPM.CATALOGNAME;
            entityPOCO.AMOUNT = entityPM.AMOUNT;
            entityPOCO.STATAMOUNT = entityPM.STATAMOUNT;
            entityPOCO.STATTYPE = entityPM.STATTYPE;
        }

        public void POCOToPM(CFIMSVLINEPM entityPM, CFIMSVLINE entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.COMID = entityPOCO.COMID;
            entityPM.PAGENUM = entityPOCO.PAGENUM;
            entityPM.LINENUM = entityPOCO.LINENUM;
            entityPM.PRATMEHES = entityPOCO.PRATMEHES;
            entityPM.TARIFFCODE = entityPOCO.TARIFFCODE;
            entityPM.TOP = entityPOCO.TOP;
            entityPM.HEIGHT = entityPOCO.HEIGHT;
            entityPM.GROUPNUM = entityPOCO.GROUPNUM;
            entityPM.REMARK = entityPOCO.REMARK;
            entityPM.YEVU = entityPOCO.YEVU;
            entityPM.SUGGESTM = entityPOCO.SUGGESTM;
            entityPM.SUGGESTI = entityPOCO.SUGGESTI;
            entityPM.SUGGESTDET = entityPOCO.SUGGESTDET1;
            entityPM.STATUS = entityPOCO.STATUS;
            entityPM.TAXEXEMPT = entityPOCO.TAXEXEMPT;
            entityPM.INVOICEQUANTITY = entityPOCO.INVOICEQUANTITY;
            entityPM.INVOICEQUANTITYTYPE = entityPOCO.INVOICEQUANTITYTYPE;
            entityPM.QUETYPE = entityPOCO.QUETYPE;
            entityPM.CATALOGID = entityPOCO.CATALOGID;
            entityPM.CATALOGNAME = entityPOCO.CATALOGNAME;
            entityPM.AMOUNT = entityPOCO.AMOUNT;
            entityPM.STATAMOUNT = entityPOCO.STATAMOUNT;
            entityPM.STATTYPE = entityPOCO.STATTYPE;
        }

        public void CustomPMToPOCO(CFIMSVLINEPM entityPM, CFIMSVLINE entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIMSVLINEPM entityPM, CFIMSVLINE entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIMSVLINEPM entityPM, CFIMSVLINEPM oldEntityPM)
        {
        //    throw new System.NotImplementedException();
        }
    }
}
