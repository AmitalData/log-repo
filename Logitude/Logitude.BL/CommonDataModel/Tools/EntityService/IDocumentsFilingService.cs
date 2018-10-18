using System;
namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public interface IDocumentsFilingService
    {
        void Create(Logitude.BL.CommonDataModel.EntityPMs.DocumentsFilingPM theEntityPm);
        Simplog.Data.CommonDataModel.ICommonDataContext ObjectContext { get; set; }
        Simplog.Data.CommonDataModel.EntityPOCOs.DocumentsFiling Poco { get; set; }
        void Update(Logitude.BL.CommonDataModel.EntityPMs.DocumentsFilingPM theEntityPm);
    }
}
