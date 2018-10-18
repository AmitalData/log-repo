using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.BL.EntityQueryServices;
using Unifreight.BL.Mevaker.EntityPMs;
using Unifreight.Data.AmitalModel;

namespace Unifreight.BL.Mevaker.EntityQueryService
{
    public class MevakerQueryService
    {
        public MevakerScreenPM GetMevakerScreen(int fileNo)
        {
            var mevakerScreenPM =new MevakerScreenPM();
            using (var cntxt = new AmitalContext())
            {
                var myCCUFILEMQueryService = new CCUFILEMQueryService(cntxt);
                var myGAQFILEDATAQueryService= new GAQFILEDATAQueryService(cntxt);

                var myCCUFILEMPM= myCCUFILEMQueryService .GetSingle(fileNo,true ,false);
                var myGAQFILEDATAList= myGAQFILEDATAQueryService.GetXXX("CCUFILEM",fileNo.ToString());;


                mevakerScreenPM.CustomerTab = new CustomerTabPM();
                mevakerScreenPM.CustomerTab.AccNum = new GenralFieldPM() { MyValue = myCCUFILEMPM.CUSTOMERID };



                BuildPath(mevakerScreenPM);

                return mevakerScreenPM;
            }
        }

        private void  BuildPath(MevakerScreenPM mevakerScreenPM)
        {
            //mevakerScreenPM.
        }
    }
}
