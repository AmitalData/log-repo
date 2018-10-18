using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Def.ClosedTable
{
    public class InterfaceSendOptionsDetails : InterfaceSendOption, ICloseTable<InterfaceSendOption, InterfaceSendOptionsDetails>
    {
        public List<InterfaceSendOptionsDetails> GetAll()
        {
            var all = new List<InterfaceSendOptionsDetails>();
            all.Add(new InterfaceSendOptionsDetails()
            {
                Code = InterfaceSendOptionEnum.D.ToString(),
                EnglishName ="DCA Out",
                LocalName = "כספת",
            });

            all.Add(new InterfaceSendOptionsDetails()
            {
                Code = InterfaceSendOptionEnum.WB.ToString(),
                EnglishName = "Asynchronous Web Service",
                LocalName = "שליחה ברקע",
            });
            all.Add(new InterfaceSendOptionsDetails()
            {
                Code = InterfaceSendOptionEnum.WI.ToString(),
                EnglishName = "Interactive Web Service",
                LocalName = "אינטרקטיבי",
            });

            all.ForEach(rec => rec.SearchFields = GetSearchFields(rec));
            return all;
        }

        
        public enum InterfaceSendOptionEnum
        {
            D,WB,WI
        }
        

        public void MapPoco(InterfaceSendOption newPoco)
        {
            newPoco.Code = this.Code;
            newPoco.EnglishName = this.EnglishName;
            newPoco.LocalName = this.LocalName;
            newPoco.SearchFields = GetSearchFields(this);
        }


        public string GetSearchFields(InterfaceSendOption rec)
        {
            return string.Concat(rec.Code + "," + rec.EnglishName + ",", rec.LocalName).ToLower();
        }
    }


    public enum InteractiveMode
    {
        none = 0,
        WebServiceInteractive,
        WebServiceBatch,
        DCABatchOutIn,
        DCABatchIn,
    }
    public enum InOutType
    {
        none = 0,
        In = 1,
        Out = 2
    }
}
