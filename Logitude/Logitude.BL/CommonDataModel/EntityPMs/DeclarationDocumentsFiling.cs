using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [DataContract]
    public class DeclarationDocumentsFiling
    {
        [DataMember]
        public string documentsFilingId;
        [DataMember]
        public DocumentsFilingPM documentsFilingPM = new DocumentsFilingPM();
        [DataMember]
        public CustomsDocumentPM customsDocumentPM = new CustomsDocumentPM();
        [DataMember]
        public CustomsDocumentsTicketPM customsDocumentsTicketPM = new CustomsDocumentsTicketPM();
        [DataMember]
        public List<CustomsDocumentMetaDataValuePM> customsDocumentMetaDataValuePMList = new List<CustomsDocumentMetaDataValuePM>();
        [DataMember]
        public string pointerChild1EntityCode;
        [DataMember]
        public string pointerChild1EntityId;
    }
}
