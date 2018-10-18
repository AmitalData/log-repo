using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class InterfaceTypeQueryService : EntityQueryService<InterfaceType, InterfaceTypeKeys, InterfaceTypePM, object, InterfaceTypeKeys>
    {
        public InterfaceTypePM GetOutInterfaceType(string currentDCAInInterfaceTypeCode)
        {


#if not_hardcodded
            List<InterfaceTypePM> list=null;
            list =repository.GetAll().Where(rec=> rec.ResponseCode==currentDCAInInterfaceTypeCode).ToList();
            if (list.Count >1)
            {
                throw new Exception("Exept ony 1 item , GetOutInterfaceType(string currentDCAInInterfaceTypeCode):" +currentDCAInInterfaceTypeCode); 
            }      
            return list .Fris
#else
            switch (currentDCAInInterfaceTypeCode)
            {
                case "103":
                    {
                        var pocos = repository.GetAll().Where(rec => rec.Code == "101").ToList();
                        if (pocos == null)
                        {
                            return null;
                        }
                        var poco = pocos.FirstOrDefault();
                        if (poco == null)
                        {
                            return null;
                        }
                        var newpm = new InterfaceTypePM();
                        mapping.POCOToPM(newpm, poco);
                        return newpm;
                    }
                    break;
                case "190":
                    return null;
                case "3053":
                    {
                        var pocos = repository.GetAll().Where(rec => rec.Code == "3050").ToList();
                        if (pocos == null)
                        {
                            return null;
                        }
                        var poco = pocos.FirstOrDefault();
                        if (poco == null)
                        {
                            return null;
                        }
                        var newpm = new InterfaceTypePM();
                        mapping.POCOToPM(newpm, poco);
                        return newpm;
                    }
                    break;
                default:
                    throw new Exception("Exept ony 1 item , GetOutInterfaceType(string currentDCAInInterfaceTypeCode):" + currentDCAInInterfaceTypeCode);
                    break;
            }
#endif


        }

        public List<InterfaceTypePM> GetAll()
        {

            //mapping = new InterfaceTypeDataMapping();

            var allPoco = repository.GetAll().ToList();
            var allPM = new List<InterfaceTypePM>();
            foreach (var poco in allPoco)
            {
                var newpm = new InterfaceTypePM();
                mapping.POCOToPM(newpm, poco);
                allPM.Add(newpm);
            }
            return allPM;
        }
    }
}
