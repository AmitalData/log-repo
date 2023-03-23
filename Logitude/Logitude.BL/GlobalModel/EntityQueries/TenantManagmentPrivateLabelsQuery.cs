using System;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.GlobalModel.EntityLists;
using System.Collections.Generic;
using System.Transactions;

namespace Logitude.BL.GlobalModel.EntityQueries
{
    public class TenantManagmentPrivateLabelsQuery
    {
        private TenantManagmentPrivateLabelsRepository repository;
        public TenantManagmentPrivateLabelsQuery()
        {
            repository = new TenantManagmentPrivateLabelsRepository();
        }
        public TenantManagmentPrivateLabelsQuery(int tenant)
        {
            repository = new TenantManagmentPrivateLabelsRepository();
        }
        public TenantManagmentPrivateLabelsQuery(TenantManagmentPrivateLabelsRepository repository)
        {
            this.repository = repository;
        }

        public TenantManagmentPrivateLabelsPM GetSinglePM_Cache(string id)
        {
            string entityKeyString = $"GetSinglePM_Cache({id})";
            var res = CacheManager
                .GetOrInsertNewObject<TenantManagmentPrivateLabelsPM>(entityKeyString,
                () => { return this.GetSinglePM(id); });
            return res;

        }

        public TenantManagmentPrivateLabelsPM GetSinglePM(string id)
        {
            TenantManagmentPrivateLabelsPM entity = (from a in repository.context.TenantManagmentPrivateLabels
                                                     where a.Id == id
                                                     select new TenantManagmentPrivateLabelsPM()
                                         {
                                             Id = a.Id,
                                             PrivateLabelName = a.PrivateLabelName,
                                             PrivateLabelShortName = a.PrivateLabelShortName,
                                             PrivateLabelUrl = a.PrivateLabelUrl,
                                             PrivateLabelDomain = a.PrivateLabelDomain,
                                             ReceiveAllStatuses = a.ReceiveAllStatuses,
                                             MainLogo = a.MainLogo,
                                             InActive = a.InActive,
                                             HybridPartnerId = a.HybridPartnerId,
                                             ContactUsEmail = a.ContactUsEmail,
                                             SearchFields = a.SearchFields,
                                             SmallLogo = a.SmallLogo,
                                             BackgroundImageId = a.BackgroundImageId,
                                             LoginImageId = a.LoginImageId,
                                             MainColor = a.MainColor,
                                             LoginProgressImageId = a.LoginProgressImageId,
                                             ForgetPasswordImageId = a.ForgetPasswordImageId,
                                             SecondaryColor = a.SecondaryColor,
                                             HasLogboxAccess = a.HasLogboxAccess,
                                         }).FirstOrDefault();

            return entity;
        }

        public TenantManagmentPrivateLabelsPM GetSingleActivePMByUrl_Cache(string url)
        {
            string entityKeyString = $"GetSingleActivePMByUrl_Cache({url})";
            var res = CacheManager
                .GetOrInsertNewObject<TenantManagmentPrivateLabelsPM>(entityKeyString,
                () => { return this.GetSingleActivePMByUrl(url); });
            return res;

        }
        public TenantManagmentPrivateLabelsPM GetSingleActivePMByUrl(string url)
        {
            TenantManagmentPrivateLabelsPM entity = (from a in repository.context.TenantManagmentPrivateLabels
                                                     where a.PrivateLabelUrl == url && a.InActive == false
                                                     select new TenantManagmentPrivateLabelsPM()
                                                     {
                                                         Id = a.Id,
                                                         PrivateLabelName = a.PrivateLabelName,
                                                         PrivateLabelShortName = a.PrivateLabelShortName,
                                                         PrivateLabelUrl = a.PrivateLabelUrl,
                                                         PrivateLabelDomain = a.PrivateLabelDomain,
                                                         ReceiveAllStatuses = a.ReceiveAllStatuses,
                                                         MainLogo = a.MainLogo,
                                                         InActive = a.InActive,
                                                         HybridPartnerId = a.HybridPartnerId,
                                                         ContactUsEmail = a.ContactUsEmail,
                                                         SearchFields = a.SearchFields,
                                                         SmallLogo = a.SmallLogo,
                                                         BackgroundImageId = a.BackgroundImageId,
                                                         LoginImageId = a.LoginImageId,
                                                         MainColor = a.MainColor,
                                                         LoginProgressImageId = a.LoginProgressImageId,
                                                         ForgetPasswordImageId = a.ForgetPasswordImageId,
                                                         SecondaryColor = a.SecondaryColor,
                                                         HasLogboxAccess = a.HasLogboxAccess,

                                                     }).FirstOrDefault();

            return entity;
        }

        public TenantManagmentPrivateLabelsList GetSingleList_Cache(string id)
        {
            string entityKeyString = $"GetSingleList_Cache({id})";
            var res = CacheManager
                .GetOrInsertNewObject<TenantManagmentPrivateLabelsList>(entityKeyString,
                () => { return this.GetSingleList(id); });
            return res;

        }
        public TenantManagmentPrivateLabelsList GetSingleList(string id)
        {
            TenantManagmentPrivateLabelsList entity = (from a in repository.context.TenantManagmentPrivateLabels
                                                       where a.Id == id
                                                       select new TenantManagmentPrivateLabelsList()
                                                       {
                                                           Id = a.Id,
                                                           PrivateLabelName = a.PrivateLabelName,
                                                           PrivateLabelShortName = a.PrivateLabelShortName,
                                                           PrivateLabelUrl = a.PrivateLabelUrl,
                                                           PrivateLabelDomain = a.PrivateLabelDomain,
                                                           ReceiveAllStatuses = a.ReceiveAllStatuses,
                                                           MainLogo = a.MainLogo,
                                                           InActive = a.InActive,
                                                           HybridPartnerId = a.HybridPartnerId,
                                                           ContactUsEmail = a.ContactUsEmail,
                                                           SearchFields = a.SearchFields,
                                                           SmallLogo = a.SmallLogo,
                                                           BackgroundImageId = a.BackgroundImageId,
                                                           LoginImageId = a.LoginImageId,
                                                           MainColor = a.MainColor,
                                                           LoginProgressImageId = a.LoginProgressImageId,
                                                           ForgetPasswordImageId = a.ForgetPasswordImageId,
                                                           SecondaryColor = a.SecondaryColor,
                                                           HasLogboxAccess = a.HasLogboxAccess,
                                                       }).FirstOrDefault();

            return entity;
        }

        public IQueryable<TenantManagmentPrivateLabelsPM> GetTenantManagmentPrivateLablesPMs_Cache()
        {
            string entityKeyString = $"GetTenantManagmentPrivateLablesPMs_Cache()";
            var res = CacheManager
                .GetOrInsertNewObject<IQueryable<TenantManagmentPrivateLabelsPM>>(entityKeyString,
                () => { return this.GetTenantManagmentPrivateLablesPMs(); });
            return res;

        }
        public IQueryable<TenantManagmentPrivateLabelsPM> GetTenantManagmentPrivateLablesPMs()
        {
            return (from a in repository.context.TenantManagmentPrivateLabels
                    select new TenantManagmentPrivateLabelsPM()
                    {
                        Id = a.Id,
                        PrivateLabelName = a.PrivateLabelName,
                        PrivateLabelShortName = a.PrivateLabelShortName,
                        PrivateLabelUrl = a.PrivateLabelUrl,
                        PrivateLabelDomain = a.PrivateLabelDomain,
                        ReceiveAllStatuses = a.ReceiveAllStatuses,
                        MainLogo = a.MainLogo,
                        InActive = a.InActive,
                        HybridPartnerId = a.HybridPartnerId,
                        ContactUsEmail = a.ContactUsEmail,
                        SearchFields = a.SearchFields,
                        SmallLogo = a.SmallLogo,
                        BackgroundImageId = a.BackgroundImageId,
                        LoginImageId = a.LoginImageId,
                        MainColor = a.MainColor,
                        LoginProgressImageId = a.LoginProgressImageId,
                        ForgetPasswordImageId = a.ForgetPasswordImageId,
                        SecondaryColor = a.SecondaryColor,
                        HasLogboxAccess = a.HasLogboxAccess,
                    });
        }

        public IQueryable<TenantManagmentPrivateLabelsList> GetTenantManagmentPrivateLablesLists_Cache()
        {
            string entityKeyString = $"GetTenantManagmentPrivateLablesLists_Cache()";
            var res = CacheManager
                .GetOrInsertNewObject<IQueryable<TenantManagmentPrivateLabelsList>>(entityKeyString,
                () => { return this.GetTenantManagmentPrivateLablesLists(); });
            return res;

        }
        public IQueryable<TenantManagmentPrivateLabelsList> GetTenantManagmentPrivateLablesLists()
        {
            return (from a in repository.context.TenantManagmentPrivateLabels
                    select new TenantManagmentPrivateLabelsList()
                    {
                        Id = a.Id,
                        PrivateLabelName = a.PrivateLabelName,
                        PrivateLabelShortName = a.PrivateLabelShortName,
                        PrivateLabelUrl = a.PrivateLabelUrl,
                        PrivateLabelDomain = a.PrivateLabelDomain,
                        ReceiveAllStatuses = a.ReceiveAllStatuses,
                        MainLogo = a.MainLogo,
                        InActive = a.InActive,
                        HybridPartnerId = a.HybridPartnerId,
                        ContactUsEmail = a.ContactUsEmail,
                        SearchFields = a.SearchFields,
                        SmallLogo = a.SmallLogo,
                        BackgroundImageId = a.BackgroundImageId,
                        LoginImageId = a.LoginImageId,
                        MainColor = a.MainColor,
                        LoginProgressImageId = a.LoginProgressImageId,
                        ForgetPasswordImageId = a.ForgetPasswordImageId,
                        SecondaryColor = a.SecondaryColor,
                        HasLogboxAccess = a.HasLogboxAccess,
                    });
        }

        public IQueryable<TenantManagmentPrivateLabelsList> GetIQueryableEntityList_Cache(IQueryable<TenantManagmentPrivateLabels> iQueryable)
        {
            string entityKeyString = $"GetIQueryableEntityList_Cache({iQueryable})";
            var res = CacheManager
                .GetOrInsertNewObject<IQueryable<TenantManagmentPrivateLabelsList>>(entityKeyString,
                () => { return this.GetIQueryableEntityList(iQueryable); });
            return res;

        }
        public IQueryable<TenantManagmentPrivateLabelsList> GetIQueryableEntityList(IQueryable<TenantManagmentPrivateLabels> iQueryable)
        {
            return from a in iQueryable
                   select new TenantManagmentPrivateLabelsList()
                   {
                       Id = a.Id,
                       PrivateLabelName = a.PrivateLabelName,
                       PrivateLabelShortName = a.PrivateLabelShortName,
                       PrivateLabelUrl = a.PrivateLabelUrl,
                       PrivateLabelDomain = a.PrivateLabelDomain,
                       ReceiveAllStatuses = a.ReceiveAllStatuses,
                       MainLogo = a.MainLogo,
                       InActive = a.InActive,
                       HybridPartnerId = a.HybridPartnerId,
                       ContactUsEmail = a.ContactUsEmail,
                       SearchFields = a.SearchFields,
                       SmallLogo = a.SmallLogo,
                       BackgroundImageId = a.BackgroundImageId,
                       LoginImageId = a.LoginImageId,
                       MainColor = a.MainColor,
                       LoginProgressImageId = a.LoginProgressImageId,
                       ForgetPasswordImageId = a.ForgetPasswordImageId,
                       SecondaryColor = a.SecondaryColor,
                       HasLogboxAccess = a.HasLogboxAccess,
                   };

        }

        public List<string> GetLogboxAccessibleTenantManagmentPrivateLabelsIds_Cache()
        {
            string entityKeyString = $"GetLogboxAccessibleTenantManagmentPrivateLabelsIds_Cache()";
            var res = CacheManager
                .GetOrInsertNewObject<List<string>>(entityKeyString,
                () => { return this.GetLogboxAccessibleTenantManagmentPrivateLabelsIds(); });
            return res;

        }

        public List<string> GetLogboxAccessibleTenantManagmentPrivateLabelsIds()
        {
            List<string> entity = (from a in repository.context.TenantManagmentPrivateLabels
                                                     where a.InActive == false && a.HasLogboxAccess
                                                     select a.Id).ToList();

            return entity;
        }

    }
}