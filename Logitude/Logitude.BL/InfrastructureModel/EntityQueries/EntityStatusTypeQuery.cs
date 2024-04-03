using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class EntityStatusTypeQuery
    {
        EntityStatusTypeRepository repository;
        public EntityStatusTypeQuery()
        {
            repository = new EntityStatusTypeRepository(); 
        }

        public EntityStatusTypeQuery(int tenant)
        {
            repository = new EntityStatusTypeRepository(tenant);
        }

        public EntityStatusTypeQuery(EntityStatusTypeRepository EntityStatusTypeRepository)
        {
            repository = EntityStatusTypeRepository;
        }

        public EntityStatusTypePM GetSingleEntityStatusTypePM(string code)
        {
            return (from a in repository.context.EntityStatusTypes
                    where a.Code == code
                    select new EntityStatusTypePM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields,
                    }).FirstOrDefault();

        }
        public IQueryable<EntityStatusTypePM> GetEntityStatusTypePMs()
        {
            return from a in repository.context.EntityStatusTypes
                   select new EntityStatusTypePM()
                   {
                       Code = a.Code,
                       Name = a.Name,
                       SearchFields = a.SearchFields,
                   };
        }


        public IQueryable<EntityStatusTypeList> GetIQueryableEntityList(IQueryable<EntityStatusType> iQueryable)
        {
            IQueryable<EntityStatusTypeList> result = from entity in iQueryable
                                                select new EntityStatusTypeList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }
    }
}