using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.EntityQueries
{
    public class AWBAdditionalHandlingInfoQuery
    {
        AWBAdditionalHandlingInfoRepository repository;

        public AWBAdditionalHandlingInfoQuery(int tenant)
        {
            this.repository = new AWBAdditionalHandlingInfoRepository(tenant);
        }

        public AWBAdditionalHandlingInfoQuery(AWBAdditionalHandlingInfoRepository repository)
        {
            this.repository = repository;
        }

        public AWBAdditionalHandlingInfoPM GetSingleAWBInformationPM(string id)
        {
            return (from a in repository.Context.AWBAdditionalHandlingInfos
                    where a.Id == id
                    select new AWBAdditionalHandlingInfoPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        PrintDescription = a.PrintDescription,
                        SearchFields = a.SearchFields
                    }).FirstOrDefault();
        }

        public IQueryable<AWBAdditionalHandlingInfoPM> GetAWBInformationPMs()
        {
            return (from a in repository.Context.AWBAdditionalHandlingInfos
                    select new AWBAdditionalHandlingInfoPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        Code = a.Code,
                        Name = a.Name,
                        PrintDescription = a.PrintDescription,
                        SearchFields = a.SearchFields
                    });
        }

        public IQueryable<AWBAdditionalHandlingInfoList> GetIQueryableEntityList(IQueryable<AWBAdditionalHandlingInfo> iQueryable)
        {
            IQueryable<AWBAdditionalHandlingInfoList> result = from entity in iQueryable
                                                               select new AWBAdditionalHandlingInfoList()
                                                               {
                                                                   Id = entity.Id,
                                                                   Tenant = entity.Tenant,
                                                                   Code = entity.Code,
                                                                   Name = entity.Name,
                                                                   PrintDescription = entity.PrintDescription,
                                                                   SearchFields = entity.SearchFields
                                                               };
            return result;
        }
    }
}
