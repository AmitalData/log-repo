
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
    public class ITBPCKTYDataMapping : IMapping<ITBPCKTYPM, ITBPCKTY>
    {
        public void PMToPOCO(ITBPCKTYPM entityPM, ITBPCKTY entityPOCO)
        {
            entityPOCO.PACKTYPEID = entityPM.PACKTYPEID;
            entityPOCO.NAMEHEB = entityPM.NAMEHEB;
            entityPOCO.NAMEENG = entityPM.NAMEENG;
            entityPOCO.SEPARPRC = entityPM.SEPARPRC;
            entityPOCO.BLOCKRECORD = entityPM.BLOCKRECORD;
            entityPOCO.SEARCHENG = entityPM.SEARCHENG;
            entityPOCO.CONTSIZE = entityPM.CONTSIZE;
            entityPOCO.CONTTYPE = entityPM.CONTTYPE;
            entityPOCO.REFRI = entityPM.REFRI;
            entityPOCO.VENTY = entityPM.VENTY;
            entityPOCO.TEU = entityPM.TEU;
            entityPOCO.MODEOFTRANSP = entityPM.MODEOFTRANSP;
            entityPOCO.DEFTARA = entityPM.DEFTARA;
            entityPOCO.DEFVOLUME = entityPM.DEFVOLUME;
        }

        public void POCOToPM(ITBPCKTYPM entityPM, ITBPCKTY entityPOCO)
        {
            entityPM.PACKTYPEID = entityPOCO.PACKTYPEID;
            entityPM.NAMEHEB = entityPOCO.NAMEHEB;
            entityPM.NAMEENG = entityPOCO.NAMEENG;
            entityPM.SEPARPRC = entityPOCO.SEPARPRC;
            entityPM.BLOCKRECORD = entityPOCO.BLOCKRECORD;
            entityPM.SEARCHENG = entityPOCO.SEARCHENG;
            entityPM.CONTSIZE = entityPOCO.CONTSIZE;
            entityPM.CONTTYPE = entityPOCO.CONTTYPE;
            entityPM.REFRI = entityPOCO.REFRI;
            entityPM.VENTY = entityPOCO.VENTY;
            entityPM.TEU = entityPOCO.TEU;
            entityPM.MODEOFTRANSP = entityPOCO.MODEOFTRANSP;
            entityPM.DEFTARA = entityPOCO.DEFTARA;
            entityPM.DEFVOLUME = entityPOCO.DEFVOLUME;
        }

        public void CustomPMToPOCO(ITBPCKTYPM entityPM, ITBPCKTY entityPOCO)
        {
            //throw new NotImplementedException();
        }

        public void CustomPOCOToPM(ITBPCKTYPM entityPM, ITBPCKTY entityPOCO)
        {
            ///throw new NotImplementedException();
        }


        public void PMToOldPM(ITBPCKTYPM entityPM, ITBPCKTYPM oldEntityPM)
        {
            //    throw new System.NotImplementedException();
        }
    }
}

