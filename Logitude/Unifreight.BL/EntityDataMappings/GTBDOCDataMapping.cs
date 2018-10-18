
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
    public class GTBDOCDataMapping : IMapping<GTBDOCPM, GTBDOC>
    {
        public void PMToPOCO(GTBDOCPM entityPM, GTBDOC entityPOCO)
        {
            entityPOCO.DOCID = entityPM.DOCID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.USERDOC = entityPM.USERDOC;
            entityPOCO.SEARCHENG = entityPM.SEARCHENG;
            entityPOCO.DISCLIENT = entityPM.DISCLIENT;
            entityPOCO.DISAGENT = entityPM.DISAGENT;
            entityPOCO.COPYDESC = entityPM.COPYDESC;
            entityPOCO.FOLDERCODE = entityPM.FOLDERCODE;
            entityPOCO.DECLARATIONMAPPING = entityPM.DECLARATIONMAPPING;
            entityPOCO.OCR = entityPM.OCR;
        }

        public void POCOToPM(GTBDOCPM entityPM, GTBDOC entityPOCO)
        {
            entityPM.DOCID = entityPOCO.DOCID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.USERDOC = entityPOCO.USERDOC;
            entityPM.SEARCHENG = entityPOCO.SEARCHENG;
            entityPM.DISCLIENT = entityPOCO.DISCLIENT;
            entityPM.DISAGENT = entityPOCO.DISAGENT;
            entityPM.COPYDESC = entityPOCO.COPYDESC;
            entityPM.FOLDERCODE = entityPOCO.FOLDERCODE;
            entityPM.DECLARATIONMAPPING = entityPOCO.DECLARATIONMAPPING;
            entityPM.OCR = entityPOCO.OCR;
        }

        public void CustomPMToPOCO(GTBDOCPM entityPM, GTBDOC entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(GTBDOCPM entityPM, GTBDOC entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(GTBDOCPM entityPM, GTBDOCPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}
