using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityLists;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class TenantManagmentPrivateLabelsQuery
    {
        private IRepository<TenantManagmentPrivateLabels> repository;
        IGlobalContext context;

        public TenantManagmentPrivateLabelsQuery(int tenant) : this(GlobalContext.GetContext(tenant))
        {
        }
        public TenantManagmentPrivateLabelsQuery(IGlobalContext context) : this(new Repository<TenantManagmentPrivateLabels>(context))
        {
            this.context = context;
        }


        public TenantManagmentPrivateLabelsQuery(IRepository<TenantManagmentPrivateLabels> repository)
        {
            this.repository = repository;
        }


        public TenantManagmentPrivateLabelsPM GetSinglePM(string id)
        {
            TenantManagmentPrivateLabelsPM entity = (from a in context.TenantManagmentPrivateLabels
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
                                                         MainTabHighlightColor = a.MainTabHighlightColor,
                                                         DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                                                         IsCustomsActivated = a.IsCustomsActivated,
                                                         IsExportActivated = a.IsExportActivated,
                                                         QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                                                         CreateShipmentsWithoutDocs = a.CreateShipmentsWithoutDocs,
                                                         CreateOShipmentsWithoutDocs = a.CreateOShipmentsWithoutDocs,
                                                         FilingInboxDomain = a.FilingInboxDomain,
                                                         DistributorCode = a.DistributorCode
                                                     }).FirstOrDefault();

            return entity;
        }
        public TenantManagmentPrivateLabelsPM GetSingleActivePMByUrl_Cache(string url)
        {
            string entityKeyString = $"GetSingleActivePMByUrl_Cache({url})";
            var res = CacheManager.GetOrInsertNewObject<TenantManagmentPrivateLabelsPM>(entityKeyString,
                () => { return this.GetSingleActivePMByUrl(url); });
            return res;
        }
        public TenantManagmentPrivateLabelsPM GetSingleActivePMByUrl(string url)
        {
            TenantManagmentPrivateLabelsPM entity = (from a in context.TenantManagmentPrivateLabels
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
                                                         MainTabHighlightColor = a.MainTabHighlightColor,
                                                         DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                                                         IsCustomsActivated = a.IsCustomsActivated,
                                                         IsExportActivated = a.IsExportActivated,
                                                         QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                                                         CreateShipmentsWithoutDocs = a.CreateShipmentsWithoutDocs,
                                                         CreateOShipmentsWithoutDocs = a.CreateOShipmentsWithoutDocs,
                                                         FilingInboxDomain = a.FilingInboxDomain,
                                                         DistributorCode = a.DistributorCode
                                                     }).FirstOrDefault();

            return entity;
        }
        public TenantManagmentPrivateLabelsList GetSingleList(string id)
        {
            TenantManagmentPrivateLabelsList entity = (from a in context.TenantManagmentPrivateLabels
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
                                                           MainTabHighlightColor = a.MainTabHighlightColor,
                                                           DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                                                           IsCustomsActivated = a.IsCustomsActivated,
                                                           IsExportActivated = a.IsExportActivated,
                                                           QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                                                           CreateShipmentsWithoutDocs = a.CreateShipmentsWithoutDocs,
                                                           CreateOShipmentsWithoutDocs = a.CreateOShipmentsWithoutDocs,
                                                           FilingInboxDomain = a.FilingInboxDomain,
                                                           DistributorCode = a.DistributorCode
                                                       }).FirstOrDefault();

            return entity;
        }
        public IQueryable<TenantManagmentPrivateLabelsPM> GetTenantManagmentPrivateLablesPMs()
        {
            return (from a in context.TenantManagmentPrivateLabels
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
                        MainTabHighlightColor = a.MainTabHighlightColor,
                        DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                        IsCustomsActivated = a.IsCustomsActivated,
                        IsExportActivated = a.IsExportActivated,
                        QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                        CreateShipmentsWithoutDocs = a.CreateShipmentsWithoutDocs,
                        CreateOShipmentsWithoutDocs = a.CreateOShipmentsWithoutDocs,
                        FilingInboxDomain = a.FilingInboxDomain,
                        DistributorCode = a.DistributorCode
                    });
        }
        public IQueryable<TenantManagmentPrivateLabelsPM> GetByHybridPartnerId(string hybridPartnerId)
        {
            return (from a in context.TenantManagmentPrivateLabels
                    where a.HybridPartnerId == hybridPartnerId
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
                        MainTabHighlightColor = a.MainTabHighlightColor,
                        DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                        IsCustomsActivated = a.IsCustomsActivated,
                        IsExportActivated = a.IsExportActivated,
                        QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                        CreateShipmentsWithoutDocs = a.CreateShipmentsWithoutDocs,
                        CreateOShipmentsWithoutDocs = a.CreateOShipmentsWithoutDocs,
                        FilingInboxDomain = a.FilingInboxDomain,
                        DistributorCode = a.DistributorCode
                    });
        }

        public IQueryable<TenantManagmentPrivateLabelsList> GetTenantManagmentPrivateLablesLists()
        {
            return (from a in context.TenantManagmentPrivateLabels
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
                        MainTabHighlightColor = a.MainTabHighlightColor,
                        DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                        IsCustomsActivated = a.IsCustomsActivated,
                        IsExportActivated = a.IsExportActivated,
                        QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                        CreateShipmentsWithoutDocs = a.CreateShipmentsWithoutDocs,
                        CreateOShipmentsWithoutDocs = a.CreateOShipmentsWithoutDocs,
                        FilingInboxDomain = a.FilingInboxDomain,
                        DistributorCode = a.DistributorCode
                    });
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
                       MainTabHighlightColor = a.MainTabHighlightColor,
                       DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                       IsCustomsActivated = a.IsCustomsActivated,
                       IsExportActivated = a.IsExportActivated,
                       QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                       CreateShipmentsWithoutDocs = a.CreateShipmentsWithoutDocs,
                       CreateOShipmentsWithoutDocs = a.CreateOShipmentsWithoutDocs,
                       FilingInboxDomain = a.FilingInboxDomain,
                       DistributorCode = a.DistributorCode
                   };

        }

        public List<string> GetLogboxAccessibleTenantManagmentPrivateLabelsIds()
        {
            List<string> entity = (from a in context.TenantManagmentPrivateLabels
                                   where a.InActive == false && a.HasLogboxAccess
                                   select a.Id).ToList();

            return entity;
        }

    }
}