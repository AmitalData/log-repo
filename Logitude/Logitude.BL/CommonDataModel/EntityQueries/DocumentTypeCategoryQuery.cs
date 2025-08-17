using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
   public class DocumentTypeCategoryQuery
    {
       DocumentTypeCategoryRepository repository;



        public DocumentTypeCategoryQuery(int tenant)
        {
            repository = new DocumentTypeCategoryRepository(tenant);
        }

        public DocumentTypeCategoryQuery(DocumentTypeCategoryRepository repository)
        {
            this.repository = repository;
        }

        public DocumentTypeCategoryPM GetSinglePM(string code)
        {
            return (from a in repository.context.DocumentTypeCategories
                    where a.Code == code
                    select new DocumentTypeCategoryPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();
        }

        public IQueryable<DocumentTypeCategoryPM> GetDocumentTypeCategoryPMs()
        {
            return from a in repository.context.DocumentTypeCategories
                   select new DocumentTypeCategoryPM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }

        public IQueryable<DocumentTypeCategoryList> GetIQueryableEntityList(IQueryable<DocumentTypeCategory> iQueryable)
        {
            IQueryable<DocumentTypeCategoryList> result = from entity in iQueryable
                                               select new DocumentTypeCategoryList()
                                               {
                                                   Name = entity.Name,
                                                   Code = entity.Code,
                                                   SearchFields = entity.SearchFields,
                                               };
            return result;
        }
    }
}
