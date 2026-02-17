using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TranslationHeaderRepository:IRepository<TranslationHeader>
    {
         IWebFreightContext webFreightContext;
        public TranslationHeaderRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public TranslationHeaderRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public TranslationHeaderRepository()
        {
               webFreightContext=new WebFreightContext(); 
        }
        public IQueryable<TranslationHeader> GetTranslationHeaders()
        {
            return context.TranslationHeaders;
        }

        public IQueryable<TranslationHeader> GetTranslationHeadersByTenant(int tenant)
        {
            IQueryable<TranslationHeader> translationHeaders = from a in context.TranslationHeaders
                                                              
                                                               select a;
            return translationHeaders;
        }

        public void Add(TranslationHeader entity)
        {
            context.TranslationHeaders.Add(entity);
        }

        public void Remove(TranslationHeader entity)
        {
            context.TranslationHeaders.Attach(entity);
            context.TranslationHeaders.Remove(entity);
        }

        public void Update(TranslationHeader entity)
        {
            context.TranslationHeaders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TranslationHeader> All()
        {
            return context.TranslationHeaders.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TranslationHeader> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TranslationHeader GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}