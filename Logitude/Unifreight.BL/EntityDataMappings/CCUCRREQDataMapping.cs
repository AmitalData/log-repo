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
using Unifreight.Data.AmitalModel.EntityPOCOs;

namespace Unifreight.BL.EntityDataMappings
{
    public class CCUCRREQDataMapping : IMapping<CCUCRREQPM, CCUCRREQ>
    {
        public void PMToPOCO(CCUCRREQPM entityPM, CCUCRREQ entityPOCO)
        {
            entityPOCO.ENTNAME = entityPM.ENTNAME;
            entityPOCO.FILENO = entityPM.FILENO;
            entityPOCO.ACCLINENO = entityPM.ACCLINENO;
            entityPOCO.ITEMLINE = entityPM.ITEMLINE;
            entityPOCO.LINENO = entityPM.LINENO;
            entityPOCO.APPROVCODE = entityPM.APPROVCODE;
            entityPOCO.APPROVTYPE = entityPM.APPROVTYPE;
            entityPOCO.CERTIFICATENO = entityPM.CERTIFICATENO;
            entityPOCO.FENTNAME = entityPM.FENTNAME;
            entityPOCO.GCRCRTFCLOSE = entityPM.GCRCRTFCLOSE;
            entityPOCO.GCRCRTFID = entityPM.GCRCRTFID;
            entityPOCO.ITEMNO = entityPM.ITEMNO;
            entityPOCO.PRATMEHES = entityPM.PRATMEHES;
            entityPOCO.REMARKS = entityPM.REMARKS;
            entityPOCO.REQCERTID = entityPM.REQCERTID;
            entityPOCO.SINUMBER = entityPM.SINUMBER;
            entityPOCO.SUPPLIERCOUNTRY = entityPM.SUPPLIERCOUNTRY;
            entityPOCO.SUPPLIERID = entityPM.SUPPLIERID;
            entityPOCO.REQUESTNO = entityPM.REQUESTNO;
        }

        public void POCOToPM(CCUCRREQPM entityPM, CCUCRREQ entityPOCO)
        {
            entityPM.ENTNAME = entityPOCO.ENTNAME;
            entityPM.FILENO = entityPOCO.FILENO;
            entityPM.ACCLINENO = entityPOCO.ACCLINENO;
            entityPM.ITEMLINE = entityPOCO.ITEMLINE;
            entityPM.LINENO = entityPOCO.LINENO;
            entityPM.APPROVCODE = entityPOCO.APPROVCODE;
            entityPM.APPROVTYPE = entityPOCO.APPROVTYPE;
            entityPM.CERTIFICATENO = entityPOCO.CERTIFICATENO;
            entityPM.FENTNAME = entityPOCO.FENTNAME;
            entityPM.GCRCRTFCLOSE = entityPOCO.GCRCRTFCLOSE;
            entityPM.GCRCRTFID = entityPOCO.GCRCRTFID;
            entityPM.ITEMNO = entityPOCO.ITEMNO;
            entityPM.PRATMEHES = entityPOCO.PRATMEHES;
            entityPM.REMARKS = entityPOCO.REMARKS;
            entityPM.REQCERTID = entityPOCO.REQCERTID;
            entityPM.SINUMBER = entityPOCO.SINUMBER;
            entityPM.SUPPLIERCOUNTRY = entityPOCO.SUPPLIERCOUNTRY;
            entityPM.SUPPLIERID = entityPOCO.SUPPLIERID;
            entityPM.REQUESTNO = entityPOCO.REQUESTNO;
        }

        public void CustomPMToPOCO(CCUCRREQPM entityPM, CCUCRREQ entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(CCUCRREQPM entityPM, CCUCRREQ entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(CCUCRREQPM entityPM, CCUCRREQPM oldEntityPM)
        {
            //throw new System.NotImplementedException();
        }
    }
}
