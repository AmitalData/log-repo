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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class AutomaticReconcileMethodListQueryService
    {
	    private IQueryable<AutomaticReconcileMethodList> GetIqueryableList(IQueryable<AutomaticReconcileMethod> iQueryable)
        {
            IQueryable<AutomaticReconcileMethodList> query = (from a in iQueryable
                                                              select new AutomaticReconcileMethodList()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            Code = a.Code,
                                                            AutomaticReconcile1 = a.AutomaticReconcileField1.Code,
                                                            AutomaticReconcile2 = a.AutomaticReconcileField2.Code,
                                                            AutomaticReconcile3 = a.AutomaticReconcileField3.Code,
                                                            AutomaticReconcileName1 = a.AutomaticReconcileField1 != null ? a.AutomaticReconcileField1.EnglishName : null,
                                                            AutomaticReconcileName2 = a.AutomaticReconcileField2 != null ? a.AutomaticReconcileField2.EnglishName : null,
                                                            AutomaticReconcileName3 = a.AutomaticReconcileField3 != null ? a.AutomaticReconcileField3.EnglishName : null,
                                                            Name = a.AutomaticReconcileField1 != null ?
                                                            a.AutomaticReconcileField2 != null ?
                                                                a.AutomaticReconcileField3 != null ?
                                                                    a.AutomaticReconcileField1.EnglishName + "+" + a.AutomaticReconcileField2.EnglishName + "+" + a.AutomaticReconcileField3.EnglishName
                                                                    : a.AutomaticReconcileField1.EnglishName + "+" + a.AutomaticReconcileField2.EnglishName
                                                                : a.AutomaticReconcileField1.EnglishName
                                                            : null,
                                                            LocalName = a.AutomaticReconcileField1 != null ?
                                                            a.AutomaticReconcileField2 != null ?
                                                                a.AutomaticReconcileField3 != null ?
                                                                    a.AutomaticReconcileField1.LocalName + "+" + a.AutomaticReconcileField2.LocalName + "+" + a.AutomaticReconcileField3.LocalName
                                                                    : a.AutomaticReconcileField1.LocalName + "+" + a.AutomaticReconcileField2.LocalName
                                                                : a.AutomaticReconcileField1.LocalName
                                                            : null,
                                                            Inactive = a.Inactive,
                                                            SearchFields = a.SearchFields,
                                                        });
            return query;
        }

		private IQueryable<AutomaticReconcileMethod> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AutomaticReconcileMethod> iQueryable,int tenant)
        {
            return iQueryable;
        }

		private IQueryable<AutomaticReconcileMethod> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AutomaticReconcileMethod> iQueryable,int tenant)
        {
			return iQueryable;
		}

        public AutomaticReconcileMethodList GetById(string id, int tenant)
        {
            
            IQueryable<AutomaticReconcileMethod> automaticReconcileMethodQuery = (from a in context.AutomaticReconcileMethods
                                                                      where a.Tenant == tenant && a.Id == id
                                                                      select a);

            IQueryable<AutomaticReconcileMethodList> automaticReconcileMethodListQuery = GetIqueryableList(automaticReconcileMethodQuery);
            var myList = automaticReconcileMethodListQuery.FirstOrDefault();
            return myList;
        }
    }


}
	