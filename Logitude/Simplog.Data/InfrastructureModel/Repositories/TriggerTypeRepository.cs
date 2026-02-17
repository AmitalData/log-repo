using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class RuleNotificationTypeRepository: IRepository<RuleNotificationType>
    {

        IWebFreightContext webFreightContext;
        public RuleNotificationTypeRepository()
        {
            webFreightContext = new WebFreightContext();

        }
        public RuleNotificationTypeRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public RuleNotificationTypeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public IQueryable<RuleNotificationType> GetRuleNotificationTypes()
        {
            return webFreightContext.RuleNotificationTypes.OrderBy(d => d.Name);
        }

        public RuleNotificationType GetRuleNotificationTypeByCode(string code)
        {
            return webFreightContext.RuleNotificationTypes.Where(r=>r.Code == code).FirstOrDefault();
        }


        public void Add(RuleNotificationType entity)
        {
            webFreightContext.RuleNotificationTypes.Add(entity);
        }

        public void Remove(RuleNotificationType entity)
        {
            webFreightContext.RuleNotificationTypes.Remove(entity);
        }

        public void Update(RuleNotificationType entity)
        {
            webFreightContext.RuleNotificationTypes.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<RuleNotificationType> All()
        {
            return webFreightContext.RuleNotificationTypes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }


        public List<RuleNotificationType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public RuleNotificationType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}