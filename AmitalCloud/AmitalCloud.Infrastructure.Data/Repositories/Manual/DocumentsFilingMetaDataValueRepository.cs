using AmitalCloud.Infrastructure.Data.Context;using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class DocumentsFilingMetaDataValueRepository : Repository<DocumentsFilingMetaDataValue>, IRepository<DocumentsFilingMetaDataValue>
    {
        IAmitalCloudContext currentContext;

        public DocumentsFilingMetaDataValueRepository() :this(0)
        {
        }

        public DocumentsFilingMetaDataValueRepository(int tenant) : this(AmitalCloudContext.GetContext(tenant))
        {
        }

        public DocumentsFilingMetaDataValueRepository(IAmitalCloudContext context) : base(context)
        {
            currentContext = context;
        }
        public DocumentsFilingMetaDataValueRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public IQueryable<DocumentsFilingMetaDataValue> GetDocumentsFilingMetaDataValues(int tenant)
        {
            return (from record in context.DocumentsFilingMetaDataValues
                    where record.Tenant == tenant
                    select record);
        }

        public DocumentsFilingMetaDataValue GetSingleDocumentsFilingMetaDataValue(string id, int tenant)
        {
            DocumentsFilingMetaDataValue d = (from a in context.DocumentsFilingMetaDataValues
                                              where a.Id == id && a.Tenant == tenant
                                              select a).FirstOrDefault();
            return d;
        }



        public IAmitalCloudContext context
        {

            get { return currentContext; }
        }


        public IQueryable<DocumentsFilingMetaDataValue> GetDocumentsFilingMetaDataValuesByTenantDocFilingId(int tenant, string DocumentFilingId)
        {
            return (from record in context.DocumentsFilingMetaDataValues
                    where record.Tenant == tenant && record.DocumentsFilingId == DocumentFilingId
                    select record);
        }
    }
}
