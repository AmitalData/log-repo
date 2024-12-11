using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TemplateFormatRepository:IRepository<TemplateFormat>
    {
        ICommonDataContext commonDataContext;

        public TemplateFormatRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TemplateFormatRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TemplateFormatRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<TemplateFormat> GetTemplateFormats()
        {
            return from a in context.TemplateFormats
                   select a;
        }

        public IQueryable<TemplateFormat> GetAll()
        {
            return from a in context.TemplateFormats
                   select a;
        }

        public TemplateFormat GetSingleTemplateFormat(string code)
        {
            return (from a in context.TemplateFormats
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(TemplateFormat entity)
        {
            context.TemplateFormats.Add(entity);
        }

        public void Remove(TemplateFormat entity)
        {
            context.TemplateFormats.Attach(entity);
            context.TemplateFormats.Remove(entity);
        }

        public void Update(TemplateFormat entity)
        {
            context.TemplateFormats.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TemplateFormat> All()
        {
            return context.TemplateFormats.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TemplateFormat> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TemplateFormat GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}