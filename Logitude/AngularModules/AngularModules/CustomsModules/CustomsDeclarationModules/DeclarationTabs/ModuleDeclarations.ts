
import { DeclarationGeneralComponent } from './Components/General/DeclarationGeneralComponent';
import { ConsigmentTabContentComponent } from './Components/General/ConsigmentTabContent/ConsigmentTabContentComponent';
import { CustomsAnswersComponent } from './Components/CustomsAnswers/CustomsAnswersComponent';
import { DeclarationCorrectionsComponent } from './Components/Corrections/DeclarationCorrectionsComponent';
import { CertificateTabComponent } from './Components/Certificate/CertificateTabComponent';
import { ConstraintsDetailsComponent } from './Components/CustomsAnswers/ConstraintsDetailsComponent';
import { AgentObjectionComponent } from './Components/CustomsAnswers/AgentObjectionComponent';
import { ConstraintDetailWithCollateralComponent } from './Components/CustomsAnswers/ConstraintDetailWithCollateralComponent';
import { DeclarationDocsInTabComponent } from './Components/DocsIn/DeclarationDocsInTabComponent';
import { NotificationReplyTabComponent } from './Components/NotificationReply/NotificationReplyTabComponent';
import { DeclarationPaymentOrderTabComponent } from './Components/PaymentOrder/DeclarationPaymentOrderTabComponent';
import { DeclarationTaxesTabComponent } from './Components/Taxes/DeclarationTaxesTabComponent';
import { ItemTaxesMoreFieldsComponent } from './Components/Taxes/ItemTaxesMoreFieldsComponent';
import { DeclarationTapagTabComponent } from './Components/Tapag/DeclarationTapagTabComponent';
import { DeclarationCollateralsComponent } from './Components/Collateral/DeclarationCollateralsComponent';
import { DeclarationPhysicalCheckTabComponent } from './Components/PhysicalCheck/DeclarationPhysicalCheckTabComponent';
import { CreateEditTicketComponent } from './Components/Certificate/CreateEditTicketComponent';
import { NewCertificateGoodsItemsSelectionComponent } from './Components/Certificate/NewCertificateGoodsItemsSelectionComponent';
import { CertificateSelectionComponent } from './Components/Certificate/CertificateSelectionComponent';
import { DeclarationCargoSplitTabComponent } from './Components/CargoSplit/DeclarationCargoSplitTabComponent';
import { ImporterDetailsComponent } from './Components/General/ImporterDetails/ImporterDetailsComponent';
import { GuaranteeDataComponent } from './Components/Tapag/GuaranteeDataComponent';
import { ConsigmentPackagesDangerComponent } from './Components/General/ConsigmentTabContent/ConsigmentPackagesDanger/ConsigmentPackagesDangerComponent';
import { DeclarationClassificationComponent } from './Components/Classification/DeclarationClassificationComponent';
import { SInvoiceClassificationTabComponent } from './Components/Classification/SInvoiceClassificationTabComponent';
import { CasualSupplierDetailsComponent } from './Components/Classification/CasualSupplierDetailsComponent';
import { DeclarationAmendmentComponent } from './Components/DeclarationAmendment/DeclarationAmendmentComponent';
import { DeclarationCargoSealTabComponent } from './Components/CargoSeal/DeclarationCargoSealTabComponent';
import { ExportDeclarationComponent } from './Components/General/ExportDeclarationComponent';
import { ExportConsigmentContentComponent } from './Components/General/ConsigmentTabContent/ExportConsigmentContentComponent';
import { ExportDeclarationClosureComponent } from './Components/Closure/ExportDeclarationClosureComponent';
import { DeclarationExportStorageComponent } from './Components/ExportStorageList/DeclarationExportStorageComponent';
import { DeclarationPendingsGeneralComponent } from './Components/Classification/DeclarationPendingsGeneralComponent';
import { ComprehensiveUpdateConsignment } from './Components/General/ConsigmentTabContent/ComprehensiveUpdateConsignment';
import { DigitalCertificateOfOriginTabComponent } from './Components/DigitalCertificateOfOrigin/DigitalCertificateOfOriginTabComponent';
import { CertificateOfOriginComponent } from './Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/CertificateOfOriginComponent';
import { CertificateOfOriginGeneralTabComponent } from './Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/General/CertificateOfOriginGeneralTabComponent';
import { CertificateOfOriginMoreDetailsTabComponent } from './Components/DigitalCertificateOfOrigin/CertificateOfOriginTabs/CertificateOfOriginMoreDetailsTabComponent';


export const Components =
    [
        DeclarationGeneralComponent,
        ConsigmentTabContentComponent,
        ExportConsigmentContentComponent,
        CustomsAnswersComponent,
        DeclarationCorrectionsComponent,
        CertificateTabComponent,
        ConstraintsDetailsComponent,
        AgentObjectionComponent,
        ConstraintDetailWithCollateralComponent,
        DeclarationDocsInTabComponent,
        NotificationReplyTabComponent,
        DeclarationPaymentOrderTabComponent,
        DeclarationTaxesTabComponent,
        ItemTaxesMoreFieldsComponent,
        DeclarationPhysicalCheckTabComponent,
        CreateEditTicketComponent,
        NewCertificateGoodsItemsSelectionComponent,
        CertificateSelectionComponent,
        DeclarationTapagTabComponent,
        DeclarationCollateralsComponent,
        DeclarationCargoSplitTabComponent,
        ImporterDetailsComponent,
        GuaranteeDataComponent,
        ConsigmentPackagesDangerComponent,
        DeclarationClassificationComponent,
        SInvoiceClassificationTabComponent,
        CasualSupplierDetailsComponent,
        DeclarationAmendmentComponent,
        DeclarationCargoSealTabComponent,
        ExportDeclarationComponent,
        ExportDeclarationClosureComponent,
        DeclarationPendingsGeneralComponent,
        DeclarationExportStorageComponent,
        ComprehensiveUpdateConsignment,
        
        DigitalCertificateOfOriginTabComponent,
        CertificateOfOriginComponent,
        CertificateOfOriginGeneralTabComponent,
        CertificateOfOriginMoreDetailsTabComponent

    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "DeclarationGeneralComponent": { myResult = DeclarationGeneralComponent; break; }
            case "ConsigmentTabContentComponent": { myResult = ConsigmentTabContentComponent; break; }
            case "ExportConsigmentContentComponent": { myResult = ExportConsigmentContentComponent; break; }

            case "CustomsAnswersComponent": { myResult = CustomsAnswersComponent; break; }
            case "DeclarationCorrectionsComponent": { myResult = DeclarationCorrectionsComponent; break; }
            case "CertificateTabComponent": { myResult = CertificateTabComponent; break; }
            case "NewCertificateGoodsItemsSelectionComponent": { myResult = NewCertificateGoodsItemsSelectionComponent; break; }

            case "ConstraintsDetailsComponent": { myResult = ConstraintsDetailsComponent; break; }
            case "AgentObjectionComponent": { myResult = AgentObjectionComponent; break; }
            case "ConstraintDetailWithCollateralComponent": { myResult = ConstraintDetailWithCollateralComponent; break; }
            case "DeclarationDocsInTabComponent": { myResult = DeclarationDocsInTabComponent; break; }
            case "CertificateSelectionComponent": { myResult = CertificateSelectionComponent; break; }
            case "NotificationReplyTabComponent": { myResult = NotificationReplyTabComponent; break; }
            case "DeclarationPaymentOrderTabComponent": { myResult = DeclarationPaymentOrderTabComponent; break; }
            case "DeclarationPhysicalCheckTabComponent": { myResult = DeclarationPhysicalCheckTabComponent; break; }
            case "DeclarationTaxesTabComponent": { myResult = DeclarationTaxesTabComponent; break; }
            case "ItemTaxesMoreFieldsComponent": { myResult = ItemTaxesMoreFieldsComponent; break; }
            case "DeclarationTapagTabComponent": { myResult = DeclarationTapagTabComponent; break; }
            case "DeclarationCollateralsComponent": { myResult = DeclarationCollateralsComponent; break; }
            case "DeclarationCargoSplitTabComponent": { myResult = DeclarationCargoSplitTabComponent; break; }
            case "DeclarationExportStorageComponent": { myResult = DeclarationExportStorageComponent; break; }
            case "ImporterDetailsComponent": { myResult = ImporterDetailsComponent; break; }
            case "GuaranteeDataComponent": { myResult = GuaranteeDataComponent; break; }
            case "DeclarationClassificationComponent": { myResult = DeclarationClassificationComponent; break; }
            case "SInvoiceClassificationTabComponent": { myResult = SInvoiceClassificationTabComponent; break; }
            case "CasualSupplierDetailsComponent": { myResult = CasualSupplierDetailsComponent; break; }
            case "ConsigmentPackagesDangerComponent": { myResult = ConsigmentPackagesDangerComponent; break; }
            case "DeclarationAmendmentComponent": { myResult = DeclarationAmendmentComponent; break; }
            case "DeclarationCargoSealTabComponent": { myResult = DeclarationCargoSealTabComponent; break; }
            case "ExportDeclarationComponent": { myResult = ExportDeclarationComponent; break; }
            case "ExportDeclarationClosureComponent": { myResult = ExportDeclarationClosureComponent; break; }
            case "DeclarationPendingsGeneralComponent": { myResult = DeclarationPendingsGeneralComponent; break; }
            case "ComprehensiveUpdateConsignment": { myResult = ComprehensiveUpdateConsignment; break; }
            
            case "DigitalCertificateOfOriginTabComponent": { myResult = DigitalCertificateOfOriginTabComponent; break; }
            case "CertificateOfOriginComponent": { myResult = CertificateOfOriginComponent; break; }
            case "CertificateOfOriginGeneralTabComponent": { myResult = CertificateOfOriginGeneralTabComponent; break; }
            case "CertificateOfOriginMoreDetailsTabComponent": { myResult = CertificateOfOriginMoreDetailsTabComponent; break; }

        }

        return myResult;
    }
}
