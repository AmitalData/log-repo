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
    public class AWBChargesCodeQuery
    {
        AWBChargesCodeRepository repository;
         
        public AWBChargesCodeQuery(int tenant)
        {
            repository = new AWBChargesCodeRepository(tenant);
        }

        public AWBChargesCodeQuery(AWBChargesCodeRepository awbChargeCodesRepository)
        {
            repository = awbChargeCodesRepository;
        }

        public AWBChargesCodePM GetSinglePM(string code)
        {
            return (from a in repository.context.AWBChargeCodes
                    where a.Code == code
                    select new AWBChargesCodePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields }).FirstOrDefault();
        }

        public AWBChargesCodePM GetSingleAWBChargeCodePM(string code)
        {
            return (from a in repository.context.AWBChargeCodes
                    where a.Code == code
                    select new AWBChargesCodePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields }).FirstOrDefault();
        }


        public IQueryable<AWBChargesCodePM> GetAWBChargeCodePMs()
        {
            return (from a in repository.context.AWBChargeCodes
                    select new AWBChargesCodePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields });
        }

        public IQueryable<AWBChargesCodeList> GetIQueryableEntityList(IQueryable<AWBChargesCode> pocos)
        {
            return (from a in pocos
                    select new AWBChargesCodeList() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields });
        }


        
    }
}