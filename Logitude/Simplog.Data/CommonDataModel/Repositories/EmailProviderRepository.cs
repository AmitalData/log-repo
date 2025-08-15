using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class EmailProviderRepository : IRepository<EmailProvider>
    {
        ICommonDataContext commonDataContext;


        public EmailProviderRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }
        public EmailProviderRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public IQueryable<EmailProvider> GetEmailProviders()
        {
            return (from record in context.EmailProviders select record);
        }

        public IQueryable<EmailProvider> GetActiveEmailProviders()
        {
            return (from record in context.EmailProviders where record.Status=="active" select record);
        }


        public EmailProvider GetSingleEmailProvider(string providerNumber)
        {
            return (from record in context.EmailProviders where record.ProviderNumber == providerNumber select record).FirstOrDefault();
        }

        public EmailProvider GetSingleByName(string name)
        {
            var EmailProvider = (from a in context.EmailProviders
                        where a.UserName == name
                        select a).FirstOrDefault();
            return EmailProvider;
        }

        
        public void Add(EmailProvider entity)
        {
            this.context.EmailProviders.Add(entity);
        }

        public void Remove(EmailProvider entity)
        {
            this.context.EmailProviders.Attach(entity);
            this.context.EmailProviders.Remove(entity);
        }

        public void Update(EmailProvider entity)
        {
            try
            {
                this.context.EmailProviders.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<EmailProvider> All()
        {
            return this.context.EmailProviders.ToList<EmailProvider>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<EmailProvider> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public EmailProvider GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}