using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class HybridPartnersPermissionQuery
    {
        HybridPartnersPermissionRepository repository;

        public HybridPartnersPermissionQuery()
        {
            repository = new HybridPartnersPermissionRepository();
        }

        public HybridPartnersPermissionQuery(int tenant)
        {
            repository = new HybridPartnersPermissionRepository(tenant);
        }

        public HybridPartnersPermissionQuery(HybridPartnersPermissionRepository HybridPartnersPermissionRepository)
        {
            repository = HybridPartnersPermissionRepository;
        }

        public IQueryable<HybridPartnersPermissionList> GetIQueryableEntityList(IQueryable<HybridPartnersPermission> iQueryable)
        {
            IQueryable<HybridPartnersPermissionList> entity = from a in iQueryable
                                                   select new HybridPartnersPermissionList()
                                                   {
                                                      AllowedByHybridPartnerId = a.AllowedByHybridPartnerId,
                                                      HybridPartnerId = a.HybridPartnerId,
                                                      InActive = a.InActive,


                                                   };


            return entity;

        }




        public List<string> GetAllowedByHybridPartnerIdsByHybridPartnerId(string hybridPartnerId)
        {
            List<string> ids = (from a in repository.context.HybridPartnersPermissions
                                   where a.HybridPartnerId == hybridPartnerId
                                   select a.AllowedByHybridPartnerId).ToList();
                                                            
            return ids;

        }


        public List<string> GetHybridPartnerIdsByAllowedByHybridPartnerId(string hybridPartnerId)
        {
            List<string> ids = (from a in repository.context.HybridPartnersPermissions
                                   where a.AllowedByHybridPartnerId == hybridPartnerId
                                   select a.HybridPartnerId).ToList();
            
            return ids;

        }





        public HybridPartnersPermissionPM GetSinglePM(string id, int Tenant)
        {
            HybridPartnersPermissionPM entity = (from a in repository.context.HybridPartnersPermissions
                                      where a.HybridPartnerId == id
                                      select new HybridPartnersPermissionPM()
                                      {
                              
                                          AllowedByHybridPartnerId = a.AllowedByHybridPartnerId,
                                          HybridPartnerId = a.HybridPartnerId,
                                          InActive = a.InActive,

                                      }).FirstOrDefault();


            return entity;


        }

   



     

    }
}
