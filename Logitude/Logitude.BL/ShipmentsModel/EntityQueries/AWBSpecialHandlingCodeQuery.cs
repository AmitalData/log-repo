using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.ShipmentsModel.Repositories;

using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class AWBSpecialHandlingCodeQuery
    {

        AWBSpecialHandlingCodeRepository repository;
         
        public AWBSpecialHandlingCodeQuery(int tenant)
        {
            
            repository = new AWBSpecialHandlingCodeRepository(tenant);
        }

        public AWBSpecialHandlingCodeQuery(AWBSpecialHandlingCodeRepository awbSpecialHandlingCodeRepository)
        {
            repository = awbSpecialHandlingCodeRepository;
        }

        public AWBSpecialHandlingCodePM GetSinglePM(string id)
        {
            return (from a in repository.context.AWBHandlingCodes
                    where a.Id == id
                    select new AWBSpecialHandlingCodePM()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        IsIATA = a.IsIATA,
                        InActive = a.InActive,
                        AirlineId = a.AirlineId,
                        SearchFields = a.SearchFields
                    }).FirstOrDefault();
        }

        public AWBSpecialHandlingCodePM GetSingleAWBHandlingCodePM(string id)
        {
            return (from a in repository.context.AWBHandlingCodes
                    where a.Id == id
                    select new AWBSpecialHandlingCodePM()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        IsIATA = a.IsIATA,
                        InActive = a.InActive,
                        AirlineId = a.AirlineId,
                        SearchFields = a.SearchFields
                    }).FirstOrDefault();
        }


        public IQueryable<AWBSpecialHandlingCodePM> GetAWBHandlingCodePMs()
        {
            return (from a in repository.context.AWBHandlingCodes
                    select new AWBSpecialHandlingCodePM()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        IsIATA = a.IsIATA,
                        InActive = a.InActive,
                        AirlineId = a.AirlineId,
                        SearchFields = a.SearchFields
                    });
        }


        public IQueryable<AWBSpecialHandlingCodeList> GetIQueryableEntityList(IQueryable<AWBSpecialHandlingCode> pocos)
        {
            return (from a in pocos
                    select new AWBSpecialHandlingCodeList()
                    {
                        Id = a.Id,
                        Code = a.Code,
                        Name = a.Name,
                        IsIATA = a.IsIATA,
                        InActive = a.InActive,
                        AirlineId = a.AirlineId,
                        SearchFields = a.SearchFields
                    });
        }
    }
}