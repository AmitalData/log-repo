using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
using System.Threading.Tasks;
using Logitude.Server.Tools;
using Logitude.Customs.Def.Messaging.LogitudeClient.DeclarationErrorPointer;
using System.Runtime.Serialization;
using Logitude.Customs.Def.Validators;

namespace Logitude.Customs.Def.EntityPMs
{
    [CustomValidation(typeof(DeclarationValidator), "IsTransportModeValid")]
    [CustomValidation(typeof(DeclarationValidator), "IsImporterCodeValid")]

    
    public partial class DeclarationPM : EntityPM
    {
        //public bool DeclarationPaymentChanged { get; set; }

        //public string CustomsRequestsSheetId { get; set; }

        private List<DeclarationConsignmentPM> declarationConsignmentPM;

        [Include]
        [Association("DeclarationDeclarationConsignments2", "Id", "DeclarationId")]
        [Composition]
        [DataMember]
        public virtual List<DeclarationConsignmentPM> DeclarationConsignments
        {
            get
            {
                if (declarationConsignmentPM == null)
                {
                    declarationConsignmentPM = new List<DeclarationConsignmentPM>();
                }
                return declarationConsignmentPM;
            }

            set { declarationConsignmentPM = value; }
        }

        private List<DeclarationErrorView> declarationErrorViews;

        [Include]
        [Association("DeclarationDeclarationXmlErrors", "Id", "DeclarationId")]
        [DataMember]
        public virtual List<DeclarationErrorView> DeclarationErrorViews
        {
            get
            {
                if (declarationErrorViews == null)
                {
                    declarationErrorViews = new List<DeclarationErrorView>();
                }
                return declarationErrorViews;
            }

            set { declarationErrorViews = value; }
        }

        //private List<DeclarationCorrectionView> declarationCorrectionViews;
        //[Include]
        //[Association("DeclarationDeclarationXmlCorrections", "Id", "DeclarationId")]
        //public virtual List<DeclarationCorrectionView> DeclaratioCorrectionViews
        //{
        //    get
        //    {
        //        if (declarationCorrectionViews == null)
        //        {
        //            declarationCorrectionViews = new List<DeclarationCorrectionView>();
        //        }
        //        return declarationCorrectionViews;
        //    }

        //    set { declarationCorrectionViews = value; }
        //}

        [DataMember]
        public string DeclarationConstraintsActiveIds { get; set; }
        [DataMember]
        public string DeclarationErrorViewsActiveIds { get; set; }
        [DataMember]
        public string DeclarationTaxesActiveIds { get; set; }


        //[DataMember]
        //public string DocumentDeclarationId { get; set; }

        private List<DeclarationCourierStatusPM> declarationCourierStatusPM;

        [Include]
        [Association("DeclarationDclarationCourierStatus", "Id", "DeclarationId")]
        [Composition]
        [DataMember]
        public virtual List<DeclarationCourierStatusPM> DclarationCourierStatus
        {
            get
            {
                if (declarationCourierStatusPM == null)
                {
                    declarationCourierStatusPM = new List<DeclarationCourierStatusPM>();
                }
                return declarationCourierStatusPM;
            }

            set { declarationCourierStatusPM = value; }
        }


        public DeclarationCourierStatusPM MyInsertDeclarationCourierStatusPM { get; set; }
        public CourierMasterPM MyCourierMasterPM { get; set; }
        
    }
}
