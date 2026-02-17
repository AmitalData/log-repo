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
    public class DocumentTypeMetaDataTypePropertiesMapping
    {
        public static string GetDocumentTypeMetaDataTypeProperties(int ImporterTenant, CodeProperties DocumentTypeMetaDataTypeProperties)
        {

            if (!string.IsNullOrEmpty(DocumentTypeMetaDataTypeProperties.Id))
            {
                return DocumentTypeMetaDataTypeProperties.Id;
            }
            else if (!string.IsNullOrEmpty(DocumentTypeMetaDataTypeProperties.Code))
            {
                ICommonDataContext commoncontext = CommonDataContext.GetContext(ImporterTenant);
                DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(commoncontext);
                var DocumentMDType = DocumentsMetaDataTypeRepository.GetSingleDocumentsMetaDataTypeByCode(DocumentTypeMetaDataTypeProperties.Code, ImporterTenant);
                if (DocumentMDType != null)
                {
                    return DocumentMDType.Id;
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
