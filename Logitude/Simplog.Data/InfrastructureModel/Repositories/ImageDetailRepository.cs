using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ImageDetailRepository:IRepository<ImageDetail>
    {
        IWebFreightContext webFreightContext;
        public ImageDetailRepository()
        {
            webFreightContext = new WebFreightContext();

        }
        public ImageDetailRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public ImageDetailRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public ImageDetail GetSingleImageDetail(string id, int tenant)
        {
            return (from a in context.ImageDetails
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public void Add(ImageDetail entity)
        {
            context.ImageDetails.Add(entity);
        }

        public void Remove(ImageDetail entity)
        {
            context.ImageDetails.Attach(entity);
            context.ImageDetails.Remove(entity);
        }

        public void Update(ImageDetail entity)
        {
            context.ImageDetails.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ImageDetail> All()
        {
            return context.ImageDetails.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public string GetImageExtensionbyId(int tenant, string id)
        {
            string extension = (from a in context.ImageDetails
                                where a.Id == id && a.Tenant == tenant
                                select a.Extension).FirstOrDefault();
            return extension;
        }

        public string GetImageExtensionbyIdForDigital(string id)
        {
            string extension = (from a in context.ImageDetails
                                where a.Id == id
                                select a.Extension).FirstOrDefault();
            return extension;
        }

        public List<ImageDetail> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ImageDetail GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}