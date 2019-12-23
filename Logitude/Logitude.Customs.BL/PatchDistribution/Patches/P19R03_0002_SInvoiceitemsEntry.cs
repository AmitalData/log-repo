using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.PatchDistribution.Patches
{
    internal class P19R03_0002_SInvoiceitemsEntry : PatchDistributionBase
    {
        public P19R03_0002_SInvoiceitemsEntry()
            : base("פריסה מלאה מהצהרת כנמ", new DateTime(2019, 11, 20))
        {

        }

        public override void CreateUpScripts()
        {
            this.AddUpSqlScript("CREATE INDEX IX_DECLARATIONS_CUSTOMFILENO ON DECLARATIONS (CUSTOMFILENO ASC)");
            this.AddUpSqlScript("ALTER TABLE supplierinvoiceitems ADD (marksandnumbers VARCHAR2(512 CHAR),packagequantity NUMBER(10),weight NUMBER(18,2)) "); 
            this.AddUpSqlScript("ALTER PROCEDURE usp_updateparentinvoiceitemseq COMPILE ");
            this.AddUpSqlScript("ALTER PROCEDURE usp_updateinvoiceitemssequence COMPILE ");
        }

        public override void CreateDownScripts()
        {
            throw new NotImplementedException();
        }
    }
}
