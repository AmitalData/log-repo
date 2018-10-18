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
    public class AWBCustomsInformationQuery
    {
        AWBCustomsInformationRepository repository;

        public AWBCustomsInformationQuery(int tenant)
        {
            repository = new AWBCustomsInformationRepository(tenant);
        }

        public AWBCustomsInformationQuery(AWBCustomsInformationRepository repository)
        {
            this.repository = repository;
        }

        public AWBCustomsInformationPM GetSingleAWBCustomsInformationPM(string code)
        {
            return (from a in repository.context.AWBCustomsInformations
                    where a.Code == code
                    select new AWBCustomsInformationPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields
                    }).FirstOrDefault();
        }

        public AWBCustomsInformationPM GetSinglePM(string code)
        {
            return (from a in repository.context.AWBCustomsInformations
                    where a.Code == code
                    select new AWBCustomsInformationPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields
                    }).FirstOrDefault();
        }
        

        public IQueryable<AWBCustomsInformationPM> GetAWBCustomsInformationPMs()
        {
            return (from a in repository.context.AWBCustomsInformations
                    select new AWBCustomsInformationPM()
                    {
                        Code = a.Code,
                        Name = a.Name,
                        SearchFields = a.SearchFields
                    });
        }

        public IQueryable<AWBCustomsInformationList> GetIQueryableEntityList(IQueryable<AWBCustomsInformation> iQueryable)
        {
            IQueryable<AWBCustomsInformationList> result = from entity in iQueryable
                                                           select new AWBCustomsInformationList()
                                                    {
                                                        Code = entity.Code,
                                                        Name = entity.Name,
                                                        SearchFields = entity.SearchFields,
                                                    };
            return result;
        }

    }
}