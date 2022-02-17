using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.DataMapping;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Linq;
using System.Web;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ImageLibraryService
    {
        private readonly int tenant;
        private readonly IWebFreightContext objectContext;
        private readonly ImageLibraryRepository entityRepository;
        private readonly ContactRepository contactRepository;
        private bool isNewEntity;
        private ImageLibraryPM entityPM;
        private ImageLibrary poco;
        private Contact loggedContact;

        public ImageLibraryService(IWebFreightContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new ImageLibraryRepository(objectContext);
            this.contactRepository = new ContactRepository(tenant);
            GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            if (HttpContext.Current != null)
            {
                this.loggedContact = contactRepository.GetSingleContactByEmail(HttpContext.Current.User.Identity.Name, tenant);
                return;
            }
            string systemContactEmail = "system@tenant" + tenant.ToString() + ".com";
            this.loggedContact = contactRepository.GetSingleContactByEmail(systemContactEmail, tenant);
        }

        public void Create(ImageLibraryPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            this.entityPM.UpdatedByUserId = this.loggedContact.Id;
            this.entityPM.CreatedByUserId = this.loggedContact.Id;
            this.entityPM.Tenant = this.tenant;
            this.entityPM.SecurityId = entityPM.Id + GenerateRandomString(10);

            this.poco = new ImageLibrary();
            ImageLibraryMapping.MapEntity(entityPM, poco, isNewEntity);
            entityRepository.Add(poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ImageLibraryPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.entityPM.UpdatedByUserId = loggedContact.Id;
            this.entityPM.Tenant = this.tenant;

            this.poco = entityRepository.GetSingleImageLibrary(this.entityPM.Id, this.tenant);
            ImageLibraryMapping.MapEntity(entityPM, poco, isNewEntity);

            entityRepository.Update(poco);
            entityRepository.SubmitChanges();
        }

        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
