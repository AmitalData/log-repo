using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class FormCustomFieldRepository:IRepository<FormCustomField>
    {
        ICommonDataContext commonDataContext;

        public FormCustomFieldRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public FormCustomFieldRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public FormCustomFieldRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<FormCustomField> GetFormCustomFields(int tenant)
        {
            return (from record in context.FormCustomFields where record.Tenant == tenant select record);
        }

        public FormCustomField GetSingleFormCustomField(string id, int tenant)
        {
            return (from record in context.FormCustomFields where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public IQueryable<FormCustomField> GetFormCusotmFieldsByDocumentTypeId(string documentTypeId, int tenant)
        {
            var formCustom = from a in context.FormCustomFields
                                     where a.Tenant == tenant && a.DocumentTypeId == documentTypeId
                                     select a;
            return formCustom;
        }

        public void Add(FormCustomField entity)
        {
            context.FormCustomFields.Add(entity);
        }

        public void Remove(FormCustomField entity)
        {
            context.FormCustomFields.Attach(entity);
            context.FormCustomFields.Remove(entity);
        }

        public void Update(FormCustomField entity)
        {
            context.FormCustomFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FormCustomField> All()
        {
            return context.FormCustomFields.ToList();
        }

        public ICommonDataContext context
        {
            get {return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<FormCustomField> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public FormCustomField GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}