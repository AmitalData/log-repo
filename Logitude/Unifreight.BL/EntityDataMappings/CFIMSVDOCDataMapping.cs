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
    public class CFIMSVDOCDataMapping : IMapping<CFIMSVDOCPM, CFIMSVDOC>
    {
        public void PMToPOCO(CFIMSVDOCPM entityPM, CFIMSVDOC entityPOCO)
        {
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.COMID = entityPM.COMID;
            entityPOCO.CREATEDATE = entityPM.CREATEDATE;
            entityPOCO.CREATEBY = entityPM.CREATEBY;
            entityPOCO.UPDATEBY = entityPM.UPDATEBY;
            entityPOCO.STATUS = entityPM.STATUS;
            entityPOCO.REMARK = entityPM.REMARK;
            entityPOCO.TOTALPAGES = entityPM.TOTALPAGES;
            entityPOCO.CUSTOMERID = entityPM.CUSTOMERID;
            entityPOCO.HASCHANGED = entityPM.HASCHANGED;
            entityPOCO.QUETYPE = entityPM.QUETYPE;
            entityPOCO.GSTRING1 = entityPM.GSTRING1;
            entityPOCO.GSTRING2 = entityPM.GSTRING2;
            entityPOCO.GSTRING3 = entityPM.GSTRING3;


            entityPOCO.INVOICEDATE = entityPM.INVOICEDATE;
            entityPOCO.INVOICEAMOUNT = entityPM.INVOICEAMOUNT;
            entityPOCO.CURRENCYID = entityPM.CURRENCYID;
            entityPOCO.CUSTOMSSUPPLIERID = entityPM.CUSTOMSSUPPLIERID;
            entityPOCO.INCOTERMS = entityPM.INCOTERMS;
            entityPOCO.TAX = entityPM.TAX;
            entityPOCO.DISCOUNT = entityPM.DISCOUNT;
            entityPOCO.ORIGINID = entityPM.ORIGINID;
            entityPOCO.MORE = entityPM.MORE;
            entityPOCO.ADDITIONAL = entityPM.ADDITIONAL;
            entityPOCO.INVOICENO = entityPM.INVOICENO;
    }

        public void POCOToPM(CFIMSVDOCPM entityPM, CFIMSVDOC entityPOCO)
        {
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.COMID = entityPOCO.COMID;
            entityPM.CREATEDATE = entityPOCO.CREATEDATE;
            entityPM.CREATEBY = entityPOCO.CREATEBY;
            entityPM.UPDATEBY = entityPOCO.UPDATEBY;
            entityPM.STATUS = entityPOCO.STATUS;
            entityPM.REMARK = entityPOCO.REMARK;
            entityPM.TOTALPAGES = entityPOCO.TOTALPAGES;
            entityPM.CUSTOMERID = entityPOCO.CUSTOMERID;
            entityPM.HASCHANGED = entityPOCO.HASCHANGED;
            entityPM.QUETYPE = entityPOCO.QUETYPE;
            entityPM.GSTRING1 = entityPOCO.GSTRING1;
            entityPM.GSTRING2 = entityPOCO.GSTRING2;
            entityPM.GSTRING3 = entityPOCO.GSTRING3;
            entityPM.INVOICEDATE = entityPOCO.INVOICEDATE;
            entityPM.INVOICEAMOUNT = entityPOCO.INVOICEAMOUNT;
            entityPM.CURRENCYID = entityPOCO.CURRENCYID;
            entityPM.CUSTOMSSUPPLIERID = entityPOCO.CUSTOMSSUPPLIERID;
            entityPM.INCOTERMS = entityPOCO.INCOTERMS;
            entityPM.TAX = entityPOCO.TAX;
            entityPM.DISCOUNT = entityPOCO.DISCOUNT;
            entityPM.ORIGINID = entityPOCO.ORIGINID;
            entityPM.MORE = entityPOCO.MORE;
            entityPM.ADDITIONAL = entityPOCO.ADDITIONAL;
            entityPM.INVOICENO = entityPOCO.INVOICENO;
        }

        public void CustomPMToPOCO(CFIMSVDOCPM entityPM, CFIMSVDOC entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CFIMSVDOCPM entityPM, CFIMSVDOC entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CFIMSVDOCPM entityPM, CFIMSVDOCPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
