using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Logitude.BL.GlobalModel.Tools.EntityService
{
    public class TenantManagmentPrivateLablesService
    {
        const string privateLablesImageFolder = "PrivateLablesImages";
       // const string privateLablesImageExtensionType = "png";
        bool isNewEntity;
        private int tenant;
        public TenantManagmentPrivateLabels Poco { get; set; }
        public IGlobalContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private TenantManagmentPrivateLabelsPM entityPm;
        private IGlobalContext objectContext;
        private TenantManagmentPrivateLabelsRepository entityRepository;

        public TenantManagmentPrivateLablesService(IGlobalContext objectContext)
        {
            this.ObjectContext = objectContext;
            this.entityRepository = new TenantManagmentPrivateLabelsRepository(objectContext);
        }

        public void Create(TenantManagmentPrivateLabelsPM entityPM)
        {
                TenantManagmentPrivateLabels ModsPoco = new TenantManagmentPrivateLabels();
                this.isNewEntity = true;
                this.entityPm = entityPM;



                this.isNewEntity = true;
                this.entityPm = entityPM;
                this.Poco = new TenantManagmentPrivateLabels();


                this.Poco.Id = IdCounter.GetNumber("TenantManagmentPrivateLabels", 0);
                TenantManagmentPrivateLablesMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
               

        }



        public void Update(TenantManagmentPrivateLabelsPM entityPM)
        { 
                TenantManagmentPrivateLabels ModsPoco = new TenantManagmentPrivateLabels();
                
                this.isNewEntity = false;
                this.entityPm = entityPM;

                this.Poco = entityRepository.GetSingleTenantManagmentPrivateLabels(entityPM.Id);



                TenantManagmentPrivateLablesMapping.MapEntity(entityPM, Poco, isNewEntity);
                this.DeleteOldImages();
                entityRepository.Update(Poco); 
                entityRepository.SubmitChanges();
                
        }


        private void DeleteOldImages()
        {
            if (this.entityPm.BackgroundImageId != this.Poco.BackgroundImageId)
            {
                DeleteImage(Poco.BackgroundImageId);
            }
            if (this.entityPm.MainImageId != this.Poco.MainImageId)
            {
                DeleteImage(Poco.MainImageId);
            }
            if (this.entityPm.LoginProgressImageId != this.Poco.LoginProgressImageId)
            {
                DeleteImage(Poco.LoginProgressImageId);
            }
            if (this.entityPm.ForgetPasswordImageId != this.Poco.ForgetPasswordImageId)
            {
                DeleteImage(Poco.ForgetPasswordImageId);
            }
          
        }

        public void DeleteImage(string imgId)
        {
            string imagePath = GetFilePath(GetFileNameWithExtension(imgId));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

        }
        private string GetFilePath(string fileName)
        {
            string folderPath = System.Web.HttpContext.Current.Server.MapPath("~/" + privateLablesImageFolder +  "/");
            string filePath = folderPath + fileName;
            return filePath;
        }

        private string GetFileNameWithExtension(string imgName)
        {
            return imgName; //+ "." + privateLablesImageExtensionType;
        }

    }
}
