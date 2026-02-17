using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.LogitudeClient.DeclarationErrorPointer.DBWCO
{
    public class WCOErrorPointerModel
    {
        public enum LogitudeEntityEnum
        { 
            None,
            Declaration,
            DeclarationTaxes,
            Consignment,
            ConsignmentPackages,
            ConsignmentInternalTransitions,
            GoodsShipment,
            AdditionalDocument,
            CustomsValuation,
            GoodsItem,
        }
        public int IndexSeq { get; set; } //My
        public int Key { get; set; } // A=1
        public int Level { get; set; } //D =4
        public string  WCOID { get; set; }//E=5
        public string  XmlTag { get; set; } // H=8

        //public string LogitudeEntityID { get; set; }
        public LogitudeEntityEnum LogitudeEntity { get; set; }
        public string LogitudeFieldID { get; set; } 

        public override string ToString()
        {
            return new StringBuilder().Append(IndexSeq).Append("L").Append(Level).Append(XmlTag)
                .Append("+").Append(WCOID)
                .Append("/E/").Append(LogitudeEntity.ToString() ).Append(".").Append(LogitudeFieldID)
                .ToString();
        }

        internal WCOErrorPointerModel CreateNew()
        {
            return new WCOErrorPointerModel()
            {
                IndexSeq = this.IndexSeq,
                Key = this.Key,
                Level = this.Level,
                WCOID = this.WCOID,
                XmlTag = this.XmlTag,
                LogitudeEntity = this.LogitudeEntity,
                LogitudeFieldID = this.LogitudeFieldID ,
                
            };
        }
    }
}
