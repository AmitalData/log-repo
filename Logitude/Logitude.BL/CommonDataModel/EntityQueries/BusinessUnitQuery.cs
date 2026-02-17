using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class BusinessUnitQuery
    {
        BusinessUnitRepository repository;

        public BusinessUnitQuery()
        {
            repository = new BusinessUnitRepository(); 
        }

        public BusinessUnitQuery(int tenant)
        {
            repository = new BusinessUnitRepository(tenant);
        }

        public BusinessUnitQuery(BusinessUnitRepository myRepository)
        {
            repository = myRepository;
        }

        public BusinessUnitPM GetSinglePM(string id, int tenant)
        {
            BusinessUnitPM result = null;
            BusinessUnit entityPoco = repository.GetSingleBusinessUnit(id, tenant);

            if (entityPoco != null)
            {
                result = new BusinessUnitPM()
                {
                    Id = entityPoco.Id,
                    Tenant = entityPoco.Tenant,
                    Name = entityPoco.Name,
                    InActive = entityPoco.InActive,
                    ParentId = entityPoco.ParentId,
                    SearchFields = entityPoco.SearchFields,
                };

                if (!string.IsNullOrEmpty(entityPoco.ParentId))
                {
                    BusinessUnit myParent = repository.GetSingleBusinessUnit(entityPoco.ParentId, tenant);
                    if (myParent != null)
                    {
                        result.ParentName = myParent.Name;
                    }
                }
            }

            return result;
        }

        public IQueryable<BusinessUnitList> GetIQueryableEntityList(IQueryable<BusinessUnit> iQueryable)
        {

            IQueryable<BusinessUnitList> myResult = null;
       
            if (iQueryable!=null && iQueryable.Count() == 1)
            {
                var entityPOCO = iQueryable.FirstOrDefault() ;

                if (entityPOCO != null)
                {
                 var   entityList = new BusinessUnitList()
                    {
                        Id = entityPOCO.Id,
                        Tenant = entityPOCO.Tenant,
                        Name = entityPOCO.Name,
                        InActive = entityPOCO.InActive,
                        ParentId = entityPOCO.ParentId,
                        SearchFields = entityPOCO.SearchFields,
                    };

                    if (!string.IsNullOrEmpty(entityList.ParentId))
                    {

                        BusinessUnitRepository businessUnitRepository = new BusinessUnitRepository(entityList.Tenant);
                        BusinessUnit myParent = businessUnitRepository.GetSingleBusinessUnit(entityList.ParentId, entityList.Tenant);
                        if (myParent != null)
                        {
                            entityList.ParentName = myParent.Name;
                        }
                    }

                  var  result = new List<BusinessUnitList>();

                     result.Add(entityList);
                     myResult = result.AsQueryable();
             
                }
       

            }
            else
            {
                myResult = (from entityPoco in iQueryable
                            join db_BusinessUnits in repository.context.BusinessUnits on entityPoco.ParentId equals db_BusinessUnits.Id into UnitsUnits
                            from myBusinessUnit in UnitsUnits.DefaultIfEmpty()
                            select new BusinessUnitList()
                            {
                                Id = entityPoco.Id,
                                Tenant = entityPoco.Tenant,
                                Name = entityPoco.Name,
                                InActive = entityPoco.InActive,
                                ParentId = entityPoco.ParentId,
                                SearchFields = entityPoco.SearchFields,
                                ParentName = myBusinessUnit != null ? myBusinessUnit.Name : "",
                            });
                                                        

            }


            return myResult;
        }
    }
}
