	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;
using Logitude.CRM.Data.CustomFilters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 
    public partial class CRMFilterSettingListQueryService
    {
	    private IQueryable<CRMFilterSettingList> GetIqueryableList(IQueryable<CRMFilterSetting> iQueryable)
        {
            if (iQueryable.Count() > 0)
            {
                int tenant = iQueryable.First().Tenant;

                 string loggedUserEmail = null;
                 if (HttpContext.Current != null && HttpContext.Current.User != null)
                 {
                     loggedUserEmail = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRep = new ContactRepository(tenant);
                Contact loggedUser = contactRep.GetSingleContactByEmail(loggedUserEmail, tenant);

                if (loggedUser != null)
                {
                    iQueryable = iQueryable.Where(d => d.UserId == loggedUser.Id);
                }
            }
            }


            IQueryable<CRMFilterSettingList> query = (from a in iQueryable
                                                      select new CRMFilterSettingList()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          UserId = a.UserId,
                                                          ControlNameSpace = a.ControlNameSpace,
                                                          FilterName = a.FilterName,
                                                          FilterValue = a.FilterValue
                                                      });

            return query;
		}

        private IQueryable<CRMFilterSetting> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CRMFilterSetting> iQueryable, int tenant)
        {
            if (iQueryable.Count() > 0)
            {                
                string loggedUserEmail = null;
                if (HttpContext.Current != null && HttpContext.Current.User != null)
                {
                    loggedUserEmail = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRep = new ContactRepository(tenant);
                Contact loggedUser = contactRep.GetSingleContactByEmail(loggedUserEmail, tenant);

                if (loggedUser != null)
                {
                    iQueryable = iQueryable.Where(d => d.UserId == loggedUser.Id);
                }
            }
            }

            return iQueryable;
		}

        private IQueryable<CRMFilterSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<CRMFilterSetting> iQueryable, int tenant)
        {
            if (iQueryable.Count() > 0)
            {
                string loggedUserEmail = null;
                if (HttpContext.Current != null && HttpContext.Current.User != null)
                {
                    loggedUserEmail = HttpContext.Current.User.Identity.Name;
                ContactRepository contactRep = new ContactRepository(tenant);
                Contact loggedUser = contactRep.GetSingleContactByEmail(loggedUserEmail, tenant);

                if (loggedUser != null)
                {
                    iQueryable = iQueryable.Where(d => d.UserId == loggedUser.Id);
                }
            }
            }

			return iQueryable;
		}
	}


}
	