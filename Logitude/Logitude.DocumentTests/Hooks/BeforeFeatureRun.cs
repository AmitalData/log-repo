using Logitude.DocumentTests.Services.Preparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Logitude.Logitude.FullAccounting.Test.Hooks
{
    [Binding]
    public class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare-DocumentType")]
        public static void PreprepareDocumentType()
        {
            new DocumentTypePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-UploadFilewithChunks")]
        public static void PrePrepareUploadFilewithChunks()
        {
            new ShipmentDataPreparation().Prepare();
            new DocumentTypePreparation().Prepare();
            new DocumentTypeTemplatePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-ViewDocument")]
        public static void PrePrepareViewDocument()
        {
            new ShipmentDataPreparation().Prepare();
            new DocumentTypePreparation().Prepare();
            new DocumentTypeTemplatePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-CreateReceivedDocument")]
        public static void PrePrepareCreateReceivedDocument()
        {
            new ShipmentDataPreparation().Prepare();
            new DocumentTypePreparation().Prepare();
            new DocumentTypeTemplatePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-CreateOutDocument")]
        public static void PrePrepareCreateOutDocument()
        {
            new ShipmentDataPreparation().Prepare();
            new DocumentTypePreparation().Prepare();
            new DocumentTypeTemplatePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-PrintDocumentOut")]
        public static void PrePreparePrintDocumentOut()
        {
            new ShipmentDataPreparation().Prepare();
            new DocumentTypePreparation().Prepare();
            new DocumentTypeTemplatePreparation().Prepare();
        }
        [BeforeFeature("Pre-Prepare-SendDocumentOut")]
        public static void PrePrepareSendDocumentOut()
        {
            new ShipmentDataPreparation().Prepare();
            new DocumentTypePreparation().Prepare();
            new DocumentTypeTemplatePreparation().Prepare();
        }

    }
}
