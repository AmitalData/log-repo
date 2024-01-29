 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CertificateOfOriginRepository:IRepository<CertificateOfOrigin>
   {
        
		public List<CertificateOfOrigin> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
		public CertificateOfOrigin GetCertificateOfOriginByCounter(string Counter)
		{
			return (from a in context.CertificateOfOrigins
					where a.Counter == Counter
					select a).FirstOrDefault();

		}

		public List<CertificateOfOrigin> GetCertificateOfOriginsByDeclarationId(string declarationId, int tenant)
		{
			return (from a in context.CertificateOfOrigins
					where a.Tenant == tenant && a.DeclarationId == declarationId 
					select a).ToList();

		}
		public CertificateOfOrigin GetCertificateOfOriginsByDeclarationIdIncludeChildrens(string certificateId, string declarationId, int tenant)
		{
			return (from a in context.CertificateOfOrigins
					where a.Tenant == tenant && a.DeclarationId == declarationId && a.Id == certificateId
                    select a).FirstOrDefault();
		}

	}

}
   