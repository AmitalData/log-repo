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
                                             MainTabHighlightColor = a.MainTabHighlightColor,
                                             DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                                             IsCustomsActivated = a.IsCustomsActivated,
                                             IsExportActivated = a.IsExportActivated,
                                             QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                                         }).FirstOrDefault();

            return entity;
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
                                                         MainTabHighlightColor = a.MainTabHighlightColor,
                                                         DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                                                         IsCustomsActivated = a.IsCustomsActivated,
                                                         IsExportActivated = a.IsExportActivated,
                                                         QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                                                     }).FirstOrDefault();

            return entity;
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
                                                           MainTabHighlightColor = a.MainTabHighlightColor,
                                                           DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                                                           IsCustomsActivated = a.IsCustomsActivated,
                                                           IsExportActivated = a.IsExportActivated,
                                                           QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                                                       }).FirstOrDefault();

            return entity;
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
                        MainTabHighlightColor = a.MainTabHighlightColor,
                        DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                        IsCustomsActivated = a.IsCustomsActivated,
                        IsExportActivated = a.IsExportActivated,
                        QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
                    });
        }

        public IQueryable<TenantManagmentPrivateLabelsPM> GetByHybridPartnerId(string hybridPartnerId)
        {
            return (from a in repository.context.TenantManagmentPrivateLabels
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
                    });
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
                        MainTabHighlightColor = a.MainTabHighlightColor,
                        DocumentTypeHighlightColor = a.DocumentTypeHighlightColor,
                        IsCustomsActivated = a.IsCustomsActivated,
                        IsExportActivated = a.IsExportActivated,
                        QueryFiltersHighlightColor = a.QueryFiltersHighlightColor,
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
                   };

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