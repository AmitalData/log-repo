using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class AWBInformationQuery
    {
        AWBInformationRepository repository;

        public AWBInformationQuery(int tenant)
        {
            repository = new AWBInformationRepository(tenant);
        }

        public AWBInformationQuery(AWBInformationRepository repository)
        {
            this.repository = repository;
        }

        public AWBInformationPM GetSingleAWBInformationPM(string code)
        {
            return (from a in repository.context.AWBInformations
                    where a.Code == code
                    select new AWBInformationPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields
                    }).FirstOrDefault();
        }

        public AWBInformationPM GetSinglePM(string code)
        {
            return (from a in repository.context.AWBInformations
                    where a.Code == code
                    select new AWBInformationPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields
                    }).FirstOrDefault();
        }
        

        public IQueryable<AWBInformationPM> GetAWBInformationPMs()
        {
            return (from a in repository.context.AWBInformations
                    select new AWBInformationPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields
                    });
        }

        public IQueryable<AWBInformationList> GetIQueryableEntityList(IQueryable<AWBInformation> iQueryable)
        {
            IQueryable<AWBInformationList> result = from entity in iQueryable
                                                    select new AWBInformationList()
                                                 {
                                                     Code = entity.Code,
                                                     Name = entity.Name,
                                                     SearchFields = entity.SearchFields,
                                                 };
            return result;
        }

    }
}