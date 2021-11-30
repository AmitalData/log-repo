using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.CoreBL.ReverseEngineer;
using System.Xml.Serialization;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class AccountingIntegrityCheckQueryService
    {

        public AccountingIntegrityResult GetAccountingIntegrityResultByIdAndTenant(string id, int tenant)
        {
            AccountingIntegrityCheckPM accountingIntegrityCheckPM = GetAccountingIntegrityCheck(id, tenant);

            System.IO.StringReader stringReader = new System.IO.StringReader(accountingIntegrityCheckPM.ResultXML);
            XmlSerializer serializer = new XmlSerializer(typeof(AccountingIntegrityResult));
            return serializer.Deserialize(stringReader) as AccountingIntegrityResult;
        }

        private static AccountingIntegrityCheckPM GetAccountingIntegrityCheck(string id, int tenant)
        {
            AccountingIntegrityCheckQueryService accountingIntegrityCheckQuery = new AccountingIntegrityCheckQueryService(tenant);
            accountingIntegrityCheckQuery.InitializeSettings();
            AccountingIntegrityCheckPM accountingIntegrityCheckPM = accountingIntegrityCheckQuery.GetSingle(id, true, false);
            return accountingIntegrityCheckPM;
        }
    }
}
