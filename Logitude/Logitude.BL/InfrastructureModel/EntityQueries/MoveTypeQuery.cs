using System.Linq;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using System.Collections.Generic;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class MoveTypeQuery
    {
        MoveTypeRepository repository;

        public MoveTypeQuery()
        {
            repository = new MoveTypeRepository();
        }

        public MoveTypeQuery(int tenant)
        {
            repository = new MoveTypeRepository(tenant);
        }

        public MoveTypeQuery(MoveTypeRepository moveTypeRepository)
        {
            repository = moveTypeRepository;
        }

        public MoveTypePM GetSingleMoveTypePM(string id, int tenant)
        {
            return (from a in repository.context.MoveTypes
                    where a.Id == id && a.Tenant == tenant
                    select new MoveTypePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        MoveTypeEnglishName = a.MoveTypeEnglishName,
                        MoveTypeLocalName = a.MoveTypeLocalName,
                        AddedManually = a.AddedManually,
                        InActive = a.InActive,
                        TransportModeId = a.TransportModeId,
                        Code = a.Code, 
                        SearchFields = a.SearchFields,
                        IsAir = a.TransportModeId == "A" ? true : false,
                        IsInland = a.TransportModeId == "I" ? true : false,
                        IsOcean = a.TransportModeId == "O" ? true : false,
                    }).FirstOrDefault();
        }


        public IQueryable<MoveTypePM> GetMoveTypePMsByTenant(int tenant)
        {
            return (from a in repository.context.MoveTypes
                    where a.Tenant == tenant
                        select new MoveTypePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        MoveTypeEnglishName = a.MoveTypeEnglishName,
                        MoveTypeLocalName = a.MoveTypeLocalName,
                        AddedManually = a.AddedManually,
                        InActive = a.InActive,
                        TransportModeId = a.TransportModeId,
                        Code = a.Code,
                        SearchFields = a.SearchFields,
                        IsAir = a.TransportModeId == "A" ? true : false,
                        IsInland = a.TransportModeId == "I" ? true : false,
                        IsOcean = a.TransportModeId == "O" ? true : false,
                    }
                  );
        }

        public MoveTypePM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.MoveTypes
                    where a.Id == id && a.Tenant == tenant
                    select new MoveTypePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        MoveTypeEnglishName = a.MoveTypeEnglishName,
                        MoveTypeLocalName = a.MoveTypeLocalName,
                        AddedManually = a.AddedManually,
                        InActive = a.InActive,
                        TransportModeId = a.TransportModeId,
                        Code = a.Code,
                        SearchFields = a.SearchFields,
                        IsAir = a.TransportModeId == "A" ? true : false,
                        IsInland = a.TransportModeId == "I" ? true : false,
                        IsOcean = a.TransportModeId == "O" ? true : false,
                    }).FirstOrDefault();
        }

        public IQueryable<MoveTypePM> GetMoveTypePMs(int tenant)
        {
            return (from a in repository.context.MoveTypes
                    where a.Tenant == tenant
                    select new MoveTypePM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        MoveTypeEnglishName = a.MoveTypeEnglishName,
                        MoveTypeLocalName = a.MoveTypeLocalName,
                        AddedManually = a.AddedManually,
                        InActive = a.InActive,
                        TransportModeId = a.TransportModeId,
                        Code = a.Code,
                        SearchFields = a.SearchFields,
                        IsAir = a.TransportModeId == "A" ? true : false,
                        IsInland = a.TransportModeId == "I" ? true : false,
                        IsOcean = a.TransportModeId == "O" ? true : false,
                    });
        }

        public IQueryable<MoveTypeList> GetIQueryableEntityList(IQueryable<MoveType> iQueryable)
        {
            IQueryable<MoveTypeList> result = from a in iQueryable
                                              select new MoveTypeList()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         MoveTypeEnglishName = a.MoveTypeEnglishName,
                                                         MoveTypeLocalName = a.MoveTypeLocalName,
                                                         AddedManually = a.AddedManually,
                                                         InActive = a.InActive,
                                                         TransportModeId = a.TransportModeId,
                                                         Code = a.Code,
                                                         SearchFields = a.SearchFields,
                                                         IsAir = a.TransportModeId == "A" ? true : false,
                                                         IsInland = a.TransportModeId == "I" ? true : false,
                                                         IsOcean = a.TransportModeId == "O" ? true : false,
                                                     };

            return result;
        }

        public string GetMoveTypeIdByCode(string code, int tenant)
        {
            return (from a in repository.context.MoveTypes
                    where a.Tenant == tenant && a.Code == code
                    select a.Id).FirstOrDefault();


        }

        public string GetMoveTypeNameById(string id, int tenant)
        {
            return (from a in repository.context.MoveTypes
                    where a.Tenant == tenant && a.Id == id
                    select a.MoveTypeEnglishName).FirstOrDefault();


        }

    }
}