using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ImageLibraryRepository : IRepository<ImageLibrary>
    {
        IWebFreightContext webFreightContext;
        public ImageLibraryRepository()
        {
            webFreightContext = new WebFreightContext();

        }
        public ImageLibraryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public ImageLibraryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public ImageLibrary GetSingleImageLibrary(string id)
        {
            return (from a in context.ImageLibrarys
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public string GetDocumentIdByImageLibraryId(string id)
        {
            return (from a in context.ImageLibrarys
                    where a.Id == id
                    select a.DocumentId).FirstOrDefault();
        }


        public List<ImageLibrary> GetImageLibraryListsByTenant(int tenant)
        {
            return (from a in context.ImageLibrarys
                    where a.Tenant == tenant
                    select a).ToList();
        }




        public void Add(ImageLibrary entity)
        {
            context.ImageLibrarys.Add(entity);
        }

        public void Remove(ImageLibrary entity)
        {
            context.ImageLibrarys.Attach(entity);
            context.ImageLibrarys.Remove(entity);
        }

        public void Update(ImageLibrary entity)
        {
            context.ImageLibrarys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ImageLibrary> All()
        {
            return context.ImageLibrarys.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ImageLibrary> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ImageLibrary GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}