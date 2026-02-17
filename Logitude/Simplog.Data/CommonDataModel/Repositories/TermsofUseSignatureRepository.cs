using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
	public class TermsofUseSignatureRepository : IRepository<TermsofUseSignature>
	{
		ICommonDataContext commonDataContext;

		public TermsofUseSignatureRepository()
		{
			commonDataContext = new CommonDataContext();
		}

		public TermsofUseSignatureRepository(ICommonDataContext context)
		{
			commonDataContext = context;
		}

		public TermsofUseSignatureRepository(int version)
		{
			commonDataContext = CommonDataContext.GetContext(version);
		}

		public void Add(TermsofUseSignature entity)
		{
			context.TermsofUseSignatures.Add(entity);
		}

		public void Remove(TermsofUseSignature entity)
		{
			context.TermsofUseSignatures.Attach(entity);
			context.TermsofUseSignatures.Remove(entity);
		}
		
		public void Update(TermsofUseSignature entity)
		{
			context.TermsofUseSignatures.Attach(entity);
			context.SetAsModified(entity);
		}

		public List<TermsofUseSignature> All()
		{
			return context.TermsofUseSignatures.ToList();
		}

		public ICommonDataContext context
		{
			get { return commonDataContext; }
		}

		public void SubmitChanges()
		{
			context.SaveChanges();
		}
		
		public TermsofUseSignature GetSingleTermsofUseSignature(string id, int tenant)
		{
			return (from record in context.TermsofUseSignatures where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();

		}

		public IQueryable<TermsofUseSignature> GetTermsofUseSignatures(int tenant)
		{
			return context.TermsofUseSignatures.Where(t=>t.Tenant == tenant);
		}
        public IQueryable<TermsofUseSignature> GetTermsofUseSignatures()
        {
            return context.TermsofUseSignatures;
        }


        public List<TermsofUseSignature> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TermsofUseSignature GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
