using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
   public class HybridPartnerQuery
    {
       HybridPartnerRepository repository;

        public HybridPartnerQuery()
        {
            repository = new HybridPartnerRepository();
        }

        public HybridPartnerQuery(int tenant)
        {
            repository = new HybridPartnerRepository(tenant);
        }

        public HybridPartnerQuery(HybridPartnerRepository HybridPartnerRepository)
        {
            repository = HybridPartnerRepository;
        }

        public IQueryable<HybridPartnerList> GetIQueryableEntityList(IQueryable<HybridPartner> iQueryable)
        {
            IQueryable<HybridPartnerList> entity = from a in iQueryable
                                                   select new HybridPartnerList()
                                                   {
                                                       Id = a.Id,
                                                       PartnerTenant = a.PartnerTenant,
                                                       Name = a.Name,
                                                       LocalName = a.LocalName,
                                                       LogoId = a.LogoId,
                                                       SearchFields = a.SearchFields,
                                                       SmallLogoId = a.SmallLogoId,
                                                       IsMislakaActivated = a.IsMislakaActivated,
                                                       IsExternalPartner = a.IsExternalPartner,
                                                       ReceiveAllStatuses = a.ReceiveAllStatuses,
                                                       AllowSendingDocsToAgent = a.AllowSendingDocsToAgent,
                                                       InActive = a.InActive

                                                   };


            return entity;

        }

        public HybridPartnerPM GetSinglePM(string id,int Tenant)
        {
            HybridPartnerPM entity = (from a in repository.context.HybridPartners
                                      where a.Id == id
                                      select new HybridPartnerPM()
                                      {
                                          Id = a.Id,
                                          PartnerTenant = a.PartnerTenant,
                                          Name = a.Name,
                                          LocalName = a.LocalName,
                                          LogoId = a.LogoId,
                                          SearchFields = a.SearchFields,
                                          SmallLogoId = a.SmallLogoId,
                                          IsMislakaActivated = a.IsMislakaActivated,
                                          IsExternalPartner = a.IsExternalPartner,
                                          ReceiveAllStatuses = a.ReceiveAllStatuses,
                                          AllowSendingDocsToAgent = a.AllowSendingDocsToAgent,
                                          InActive = a.InActive

                                      }).FirstOrDefault();


            return entity;


        }

        public HybridPartnerPM GetSinglePM(string id)
        {
            HybridPartnerPM entity = (from a in repository.context.HybridPartners
                                             where a.Id == id 
                                             select new HybridPartnerPM()
                                             {
                                                 Id = a.Id,
                                                 PartnerTenant = a.PartnerTenant,
                                                 Name = a.Name,
                                                 LocalName = a.LocalName,
                                                 LogoId = a.LogoId,
                                                 SearchFields = a.SearchFields,
                                                 SmallLogoId = a.SmallLogoId,
                                                 IsMislakaActivated = a.IsMislakaActivated,
                                                 IsExternalPartner = a.IsExternalPartner,
                                                 ReceiveAllStatuses = a.ReceiveAllStatuses,
                                                 AllowSendingDocsToAgent = a.AllowSendingDocsToAgent,
                                                 InActive = a.InActive
                                             }).FirstOrDefault();


            return entity;


        }

        public int GetPartnerTenantById(string id)
        {
            int partnerTenant = (from a in repository.context.HybridPartners
                                      where a.Id == id
                                      select a.PartnerTenant).FirstOrDefault();

            return partnerTenant;


        }

        public IQueryable<HybridPartnerPM> GetHybridPartnerPMsByTenant()
        {

            IQueryable<HybridPartnerPM> entity = from a in repository.context.HybridPartners.Include("ImageDetail")
                                                  
                                                        select new HybridPartnerPM()

                                                        {
                                                            Id = a.Id,
                                                            PartnerTenant = a.PartnerTenant,
                                                            Name = a.Name,
                                                            LocalName = a.LocalName,
                                                            LogoId = a.ImageDetail.Id,
                                                            SearchFields = a.SearchFields,
                                                            SmallLogoId = a.SmallLogoId,
                                                            IsMislakaActivated = a.IsMislakaActivated,
                                                            IsExternalPartner = a.IsExternalPartner,
                                                            ReceiveAllStatuses = a.ReceiveAllStatuses,
                                                            AllowSendingDocsToAgent = a.AllowSendingDocsToAgent,
                                                            InActive = a.InActive
                                                        };





            return entity;
        }

        public HybridPartnerPM GetSinglePMByPartnerTenant(int PartnerTenant)
        {
            string key = $"GetSinglePMByPartnerTenant({PartnerTenant})";
            return Simplog.Server.Infrastructure.Helpers.CacheManager.GetOrInsertNewObject<HybridPartnerPM>(key, () =>
            {
                return GetSinglePMByPartnerTenantReal(PartnerTenant);
            });

        }
        HybridPartnerPM GetSinglePMByPartnerTenantReal(int PartnerTenant)
        {
            HybridPartnerPM entity = (from a in repository.context.HybridPartners
                                      where a.PartnerTenant == PartnerTenant
                                      select new HybridPartnerPM()
                                      {
                                          Id = a.Id,
                                          PartnerTenant = a.PartnerTenant,
                                          Name = a.Name,
                                          LocalName = a.LocalName,
                                          LogoId = a.LogoId,
                                          SearchFields = a.SearchFields,
                                          SmallLogoId = a.SmallLogoId,
                                          IsMislakaActivated = a.IsMislakaActivated,
                                          IsExternalPartner = a.IsExternalPartner,
                                          ReceiveAllStatuses = a.ReceiveAllStatuses,
                                          AllowSendingDocsToAgent = a.AllowSendingDocsToAgent,
                                          InActive = a.InActive
                                      }).FirstOrDefault();


            return entity;


        }

        public List<HybridPartnerList> GetHybridPartnerLists(int tenant)
        { 
            //var hybridPartnerRepository = new HybridPartnerRepository(tenant);

            var requestsList = (from a in repository.context.CustomerTenantAccessRequests.Include("RequestStatusCode")
                                join b in repository.context.HybridPartners on a.ForwarderId equals b.Id
                                where a.ForwarderId == b.Id && a.Tenant == tenant
                                select new HybridPartnerList
                                {
                                    Id = b.Id,
                                    IsHasRequest = a.RequestStatusCode.Code != "N",
                                    LocalName = b.LocalName,
                                    LogoId = b.LogoId,
                                    Name = b.Name,
                                    PartnerTenant = b.PartnerTenant,
                                    SearchFields = b.SearchFields,
                                    SmallLogoId = b.SmallLogoId,
                                    StatusName = a.RequestStatusCode.EnglishName,
                                    ReqId = a.Id,
                                    IsMislakaActivated = b.IsMislakaActivated,
                                    IsExternalPartner = b.IsExternalPartner,
                                    ReceiveAllStatuses = b.ReceiveAllStatuses,
                                    AllowSendingDocsToAgent = b.AllowSendingDocsToAgent,
                                    InActive = b.InActive
                                }).OrderByDescending(a => a.StatusName).ToList();
             
            return requestsList;
        }

        public List<HybridPartnerList> GetHybridPartnerListWithNoRequest(int tenant)
        {

            var requestsList = (from a in repository.context.CustomerTenantAccessRequests.Include("RequestStatusCode")
                                where a.Tenant == tenant
                                select a.ForwarderId).ToList();

            var hybridList = (from a in repository.context.HybridPartners
                              where !requestsList.Contains(a.Id)
                              select new HybridPartnerList
                              {
                                  Id = a.Id,
                                  IsHasRequest = false,
                                  LocalName = a.LocalName,
                                  LogoId = a.LogoId,
                                  Name = a.Name,
                                  PartnerTenant = a.PartnerTenant,
                                  SearchFields = a.SearchFields,
                                  SmallLogoId = a.SmallLogoId,
                                  StatusName = "New",
                                  IsMislakaActivated = a.IsMislakaActivated,
                                  IsExternalPartner = a.IsExternalPartner,
                                  ReceiveAllStatuses = a.ReceiveAllStatuses,
                                  AllowSendingDocsToAgent = a.AllowSendingDocsToAgent,
                                  InActive = a.InActive
                              }).OrderByDescending(a => a.StatusName).ToList();



            return hybridList;
        }

        public List<HybridPartnerList> GetAllowdHybridPartnerLists(string hybridPartnerId, int tenant)
        {
            List<HybridPartnerList> result = new List<HybridPartnerList>();
            HybridPartnersPermissionQuery hybridPartnersPermissionQuery = new HybridPartnersPermissionQuery(tenant);
            List<string> ids = hybridPartnersPermissionQuery.GetAllowedByHybridPartnerIdsByHybridPartnerId(hybridPartnerId);

            if (ids.Count > 0)
            {
                result = (from b in repository.context.HybridPartners
                          where ids.Contains(b.Id)
                          select new HybridPartnerList
                          {
                              Id = b.Id,
                              PartnerTenant = b.PartnerTenant,
                              LocalName = b.LocalName,
                              LogoId = b.LogoId,
                              Name = b.Name,
                              InActive = b.InActive
                          }).ToList();
            }

            return result;
        }


        public List<HybridPartnerList> GetAllowingHybridPartnerLists(string hybridPartnerId , int tenant)
        {
            List<HybridPartnerList> result = new List<HybridPartnerList>();
            HybridPartnersPermissionQuery hybridPartnersPermissionQuery = new HybridPartnersPermissionQuery(tenant);
            List<string> ids = hybridPartnersPermissionQuery.GetHybridPartnerIdsByAllowedByHybridPartnerId(hybridPartnerId);
            if (ids.Count > 0)
            {
                result = (from b in repository.context.HybridPartners
                          where ids.Contains(b.Id)
                          select new HybridPartnerList
                          {
                              Id = b.Id,
                              PartnerTenant = b.PartnerTenant,
                              LocalName = b.LocalName,
                              LogoId = b.LogoId,
                              Name = b.Name,
                              InActive = b.InActive
                          }).ToList();

            }
            return result;
        }


        public string GetPartnerNameByPartnerTenant(int PartnerTenant)
        {
            string partnerName = (from a in repository.context.HybridPartners
                                      where a.PartnerTenant == PartnerTenant
                                    select a.Name).FirstOrDefault();


            return partnerName;


        }

        public List<string> GetPartnersForRequest(int PartnerTenant, List<string> Ids)
        {
            List<string> partners = (from a in repository.context.HybridPartners
                                  where a.PartnerTenant == PartnerTenant && Ids.Contains(a.Id)
                                        select a.Id).ToList();
             
            return partners;


        }

       



    }
}
