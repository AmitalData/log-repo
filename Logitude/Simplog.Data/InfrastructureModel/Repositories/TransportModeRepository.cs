using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TransportModeRepository:IRepository<TransportMode>
    {
        IWebFreightContext webFreightContext;

        public TransportModeRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public TransportModeRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public TransportModeRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public TransportMode GetSingleTransportMode(string id)
        {
            return (from a in context.TransportModes where a.Id == id select a).FirstOrDefault();
        }

        public IQueryable<TransportMode> GetTransportModes()
        {
            return context.TransportModes;
        }

        #region IRepository<Direction> Members

        public void Add(TransportMode entity)
        {
            context.TransportModes.Add(entity);
        }

        public void Remove(TransportMode entity)
        {
            context.TransportModes.Attach(entity);
            context.TransportModes.Remove(entity);
        }

        public void Update(TransportMode entity)
        {
            context.TransportModes.Attach(entity);
            context.SetAsModified(entity);


        }

        public List<TransportMode> All()
        {
            return context.TransportModes.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        #endregion

        public List<TransportMode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TransportMode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}