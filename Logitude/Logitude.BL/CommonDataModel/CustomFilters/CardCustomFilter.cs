using System;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.Data.Utils;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class CardCustomFilter
    {
        private int tenant;
        public CardCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public int Tenant
        {
            get { return tenant; }
            set { tenant = value; }
        }



        public IQueryable<Card> GetFilteredQuery(QueryOperations operations, IQueryable<Card> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;

            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "PartnerTypeId")
                    {
                        string filterFieldId = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(filterFieldId))
                        {
                            queryableData = queryableData.Where(c => c.PartnerTypeId == filterFieldId);
                        }
                    }

                    if (item.FieldName == "Name")
                    {
                        string tString = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(tString))
                        {
                            queryableData = queryableData.Where(c => c.EnglishName.StartsWith(tString) || c.LocalName.StartsWith(tString));
                        }
                    }

                    if (item.FieldName == "CodeOrName")
                    {
                        string value = item.FieldValue as string;

                        if (!string.IsNullOrEmpty(value))
                        {
                            queryableData = queryableData.Where(d => d.EnglishName.StartsWith(value) || d.Code.StartsWith(value) || d.LocalName.StartsWith(value));
                        }
                    }
                }
            }

            #region Freelancer

            User loggedUser = GetLoggedUser(tenant);
            if (loggedUser!=null && loggedUser.IsFreelancer)
                queryableData = GetFreelancerCards(queryableData, tenant);

            #endregion

            return queryableData;
        }

       

        public IQueryable<Card> GetFreelancerCards(IQueryable<Card> queryableData, int tenant)
        {
            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
            if (customersIds.Count > 0)
            {
                queryableData = queryableData.Where(d => customersIds.Contains(d.Id));
            }

            return queryableData;
        }


        private User GetLoggedUser(int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            UserRepository Repo = new UserRepository(commonContext);

            string email = "";
            email = (AuthenticationUtil.IsAuthenticatedUserExists() ? AuthenticationUtil.GetAuthenticatedUser() : ("system@tenant" + tenant + ".com"));
            User user = Repo.GetSingleUserByEmail(email, tenant, false);

            return user;
        }

    }
}
