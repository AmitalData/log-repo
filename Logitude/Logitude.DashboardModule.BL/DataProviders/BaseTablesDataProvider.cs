using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.DashboardModule.BL.APIDataContract;
using Logitude.DashboardModule.BL.DataProviders.Models;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public abstract class BaseTablesDataProvider
    {
        protected WidgetPM _Widget;
        protected AnalyticsFactsMetaData _Entity;
        protected int _Tenant;

        protected BaseTablesDataProvider(WidgetPM widget, AnalyticsFactsMetaData entity, int tenant)
        {
            this._Widget = widget;
            this._Entity = entity;
            this._Tenant = tenant;
        }

        public abstract List<SeriesMeasure> GetChartData();
        public abstract AnalyticData GeChartDataPart(WidgetArguments widgetPartArguments);
        public abstract KpiChart GetKpiData();

        protected IQueryable<T> AddUserBranchRestrictionFilters<T>(IQueryable<T> query)
        {
            ContactPM contact = new ContactQuery(this._Tenant).GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedWorkWebUser(), this._Tenant, false);
            UserPM user = new UserQuery(this._Tenant).GetSinglePM(contact.Id, this._Tenant);
            if (user == null || !user.IsBranchRestricted) return query;
            var branchIds = user.UserPermittedBranches.Select(x => x.BranchId).ToList();
            return query.Where(BuildInListQuery<T>(branchIds, "BranchId"));
        }


        public Expression<Func<T, bool>> BuildInListQuery<T>(IEnumerable<string> list, string propertyName)
        {
            var parent = Expression.Parameter(typeof(T));
            var property = Expression.Property(parent, propertyName);

            var anyMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
              .Single(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Length == 2)
              .MakeGenericMethod(new[] { typeof(string) });

            var c = Expression.Parameter(typeof(string), "[C]");
            var containsCall = Expression.Call(
              property,
              typeof(string).GetMethod("Contains", new[] { typeof(string) }), c);

            var anyCall = Expression.Call(
              anyMethod,
              Expression.Constant(list),
              Expression.Lambda(containsCall, c)
            );
            var notNull = Expression.NotEqual(property, Expression.Constant(null));
            return Expression.Lambda<Func<T, bool>>(Expression.And(notNull, anyCall), parent);
        }

    }
}
