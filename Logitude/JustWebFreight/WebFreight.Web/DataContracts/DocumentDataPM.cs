using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.DataContracts
{
    [DataContract]
    public class DocumentsFilingDataPM : DocumentDataPM
    {
        private List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuePMs;
        [DataMember]
        public List<DocumentsFilingMetaDataValuePM> DocumentsFilingMetaDataValuePMs
        {
            get
            {
                if (documentsFilingMetaDataValuePMs == null)
                {
                    documentsFilingMetaDataValuePMs = new List<DocumentsFilingMetaDataValuePM>();
                }
                return documentsFilingMetaDataValuePMs;
            }
            set { documentsFilingMetaDataValuePMs = value; }
        }
    }
}