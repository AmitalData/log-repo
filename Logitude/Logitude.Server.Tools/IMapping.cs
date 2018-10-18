using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools
{
    public interface IMapping<TEntityPM,TEntityPOCO>
    {
        void PMToPOCO(TEntityPM entityPM,TEntityPOCO entityPOCO);
        void POCOToPM(TEntityPM entityPM, TEntityPOCO entityPOCO);
        void CustomPMToPOCO(TEntityPM entityPM, TEntityPOCO entityPOCO);
        void CustomPOCOToPM(TEntityPM entityPM, TEntityPOCO entityPOCO);
        void PMToOldPM(TEntityPM entityPM, TEntityPM oldEntityPM);     
    }
    public interface IMappingEncodeBase64NVARCHARFields<TEntityPM>
    {
        void EncodeBase64NVARCHARFields(TEntityPM entityPM);
    }
}
