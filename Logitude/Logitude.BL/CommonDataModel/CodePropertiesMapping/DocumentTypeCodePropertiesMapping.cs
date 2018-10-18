using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CodePropertiesMapping
{
    public class DocumentTypeCodePropertiesMapping
    {
        public static string GetDocumentTypeFromDocumentTypeProperties(int ImporterTenant, CodeProperties DocumentTypeProperties)
        {

            if (!string.IsNullOrEmpty(DocumentTypeProperties.Id))
            {
                return DocumentTypeProperties.Id;
            }
            else if (!string.IsNullOrEmpty(DocumentTypeProperties.Code))
            {
                ICommonDataContext commoncontext = CommonDataContext.GetContext(ImporterTenant);
                DocumentTypeRepository DocumentTypeRepository = new DocumentTypeRepository(commoncontext);
                DocumentType DocumentType = DocumentTypeRepository.GetSingleDocumentTypeByCode(DocumentTypeProperties.Code, ImporterTenant);
                if (DocumentType != null)
                {
                    return DocumentType.Id;
                }
                else
                {
                    return "";
                }
            }
            else// if (!string.IsNullOrEmpty(CardProperties.Code))
            {
                throw new NotImplementedException();
            }
        }
    }
}
