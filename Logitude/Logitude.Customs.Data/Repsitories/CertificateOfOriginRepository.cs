 
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
		public CertificateOfOrigin GetCertificateOfOriginByCounter(string Counter, int tenant)
		{
			return (from a in context.CertificateOfOrigins
					where a.Counter == Counter && (a.Tenant== tenant || tenant==0)
					select a).FirstOrDefault();

		}

		public List<CertificateOfOrigin> GetCertificateOfOriginsByDeclarationId(string declarationId, int tenant)
		{
            //return (from a in context.CertificateOfOrigins
            //		where a.Tenant == tenant && a.DeclarationId == declarationId
            //		select a).ToList();

            //var matchingDeclarationIds = context.Declarations.Where(d => d.Id == declarationId || d.AmendmentOriginalDeclartation == declarationId).Select(d => d.Id).ToList();
            //return context.CertificateOfOrigins
            //              .Where(co => co.Tenant == tenant && matchingDeclarationIds.Contains(co.DeclarationId))
            //              .ToList();

            var matchingDeclarationIds = context.Declarations.Where(d => d.Id == declarationId || d.AmendmentOriginalDeclartation == declarationId).Select(d => d.Id).ToList();

            return context.CertificateOfOrigins.Where(co => co.Tenant == tenant && (co.DeclarationId == null ?
                  matchingDeclarationIds.Contains(null) : matchingDeclarationIds.Contains(co.DeclarationId))).ToList();
        }
        public CertificateOfOrigin GetCertificateOfOriginsByDeclarationIdIncludeChildrens(string certificateId, string declarationId, int tenant)
		{
			return (from a in context.CertificateOfOrigins
					where a.Tenant == tenant && a.DeclarationId == declarationId && a.Id == certificateId
                    select a).FirstOrDefault();
		}

	}

}
   