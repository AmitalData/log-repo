using Logitude.Server.Tools.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace Logitude.Server.Tools.AnalyticTableServices
{
    public abstract class AnalyticTableService<TEntity, TAnalyticTable> where TAnalyticTable : class
    {
        private readonly DbContext context;

        protected AnalyticTableService(DbContext context)
        {
            this.context = context;
        }

        public void AddUpdate(TEntity entity, int tenant)
        {
            if (!FeatureToggleHelper.HasFeatureToggle("DBA", tenant)) return;
            var analyticTable = AutoMapToAnalyticTable<TAnalyticTable>(entity);
            CustomMap(entity, analyticTable);
            SubmitChanges(analyticTable);
        }

        protected S AutoMapToAnalyticTable<S>(TEntity from)
        {
            var toPropes = typeof(S).GetProperties();
            var fromPropes = from.GetType().GetProperties().ToDictionary(e => e.Name, e => e);
            var to = (S)Activator.CreateInstance(typeof(S));
            foreach (var item in toPropes)
            {
                if (fromPropes.ContainsKey(item.Name))
                {
                    item.SetValue(to, fromPropes[item.Name].GetValue(from));
                }
            }
            return to;
        }

        protected abstract void CustomMap(TEntity entity, TAnalyticTable analyticTable);

        private void SubmitChanges(TAnalyticTable analyticTable)
        {
            var key = analyticTable.GetType().GetProperties().FirstOrDefault(x => x.GetCustomAttributes(typeof(KeyAttribute), false).Length != 0)?.GetValue(analyticTable);
            var existEntity = context.Set<TAnalyticTable>().Find(key);
            if (existEntity != null)
            {
                context.Entry(existEntity).State = EntityState.Detached;
                context.Set<TAnalyticTable>().Attach(analyticTable);
                context.Entry(analyticTable).State = EntityState.Modified;
            }
            else context.Set<TAnalyticTable>().Add(analyticTable);

            context.SaveChanges();
        }

    }
}
