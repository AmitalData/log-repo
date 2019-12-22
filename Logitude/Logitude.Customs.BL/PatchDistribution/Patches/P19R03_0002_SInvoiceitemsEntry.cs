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
        public override List<ScriptDTO> GetDownScripts()
        {
            throw new NotImplementedException();
        }

        public override List<ScriptDTO> GetUpScripts()
        {
            int ScriptCount = 0;
            return new List<ScriptDTO>()
            {
                new ScriptDTO()
                {
                    ScriptCounter = ScriptCount++,
                    SqlScript = "CREATE INDEX IX_DECLARATIONS_CUSTOMFILENO ON DECLARATIONS (CUSTOMFILENO ASC)"
                },
                new ScriptDTO()
                {
                    ScriptCounter = ScriptCount++,
                    SqlScript = "ALTER TABLE supplierinvoiceitems ADD (marksandnumbers VARCHAR2(512 CHAR),packagequantity NUMBER(10),weight NUMBER(18,2)) "
                },

                new ScriptDTO()
                {
                    ScriptCounter = ScriptCount++,
                    SqlScript = "ALTER PROCEDURE usp_updateparentinvoiceitemseq COMPILE "
                },
                new ScriptDTO()
                {
                    ScriptCounter = ScriptCount++,
                    SqlScript = "ALTER PROCEDURE usp_updateinvoiceitemssequence COMPILE "
                }

            };
        }
    }
}
