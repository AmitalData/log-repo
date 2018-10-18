 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class TaxWithholdingAssessOfficeRepository:IRepository<TaxWithholdingAssessOffice>
   {
        
		public List<TaxWithholdingAssessOffice> GetMulti(EntityKeyFields entityKeys)
        {

            return null;
        }

        public TaxWithholdingAssessOffice GetSingleTaxWithholdingAssessOffice(string Code, int tenant)
        {
            return (from a in context.TaxWithholdingAssessOffices
                    where a.Code == Code && a.Tenant== tenant
                   
                    select a).FirstOrDefault();
        }

        public TaxWithholdingAssessOffice GetSingleTaxWithholdingAssessOfficeByTenant(int tenant)
        {
            TaxWithholdingAssessOffice taxWithholdingAssessOffice = (from a in context.TaxWithholdingAssessOffices
                                                                     where a.Tenant == tenant
                                                             select a).FirstOrDefault();
            return taxWithholdingAssessOffice;
        }

    }

}
   