using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Logitude.Accounting.BL.EntityQueryServices
{
   public partial class JournalAdditionalDataQueryService
    {
		public List<JournalAdditionalDataPM> GetJournalAdditionalDataPMsByTaxReportId(string taxReportId, int tenant)
		{
			List<JournalAdditionalData> JournalAdditionalDataPocos = GetJournalAdditionalDataByTaxReportId(taxReportId, tenant);
			List<JournalAdditionalDataPM> journalAdditionalDataPMs = GetJournalAdditionalDataPMs(JournalAdditionalDataPocos);
			return journalAdditionalDataPMs;
		}

		private List<JournalAdditionalData> GetJournalAdditionalDataByTaxReportId(string taxReportId, int tenant)
		{
			return (from a in context.JournalAdditionalDatas
					where a.TaxReportId == taxReportId && a.Tenant == tenant
					select a).ToList();

		}
		private List<JournalAdditionalDataPM> GetJournalAdditionalDataPMs(List<JournalAdditionalData> JournalAdditionalDataPocos)
		{
			return (from a in JournalAdditionalDataPocos
					select new JournalAdditionalDataPM()
					{
						Tenant = a.Tenant,
						JournalId = a.JournalId,
						TaxReportId = a.TaxReportId,
						TaxReportTransmitStatusCode = a.TaxReportTransmitStatusCode,
						JournalLineNumber = a.JournalLineNumber
					}).ToList();

		}
		public JournalAdditionalDataPM GetJournalAdditionalData(string journalId,int journalLineNumber, int tenant)
		{
			var query = (from a in context.JournalAdditionalDatas
					where a.JournalId == journalId && a.JournalLineNumber == journalLineNumber && a.Tenant == tenant
					select a).FirstOrDefault();


			return GetEntityPM(query);

		}
        public bool CheckIfJournalAdditionalDataExist(string journalId, int journalLineNumber, int tenant)
        {
            var exists = context.JournalAdditionalDatas
                        .Any(a => a.JournalId == journalId &&
                                  a.JournalLineNumber == journalLineNumber &&
                                  a.Tenant == tenant);

            return exists;
        }

    }
}
