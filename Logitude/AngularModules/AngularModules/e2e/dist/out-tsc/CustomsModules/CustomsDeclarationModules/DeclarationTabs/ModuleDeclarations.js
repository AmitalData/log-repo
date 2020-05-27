"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DeclarationGeneralComponent_1 = require("./Components/General/DeclarationGeneralComponent");
var ConsigmentTabContentComponent_1 = require("./Components/General/ConsigmentTabContent/ConsigmentTabContentComponent");
var CustomsAnswersComponent_1 = require("./Components/CustomsAnswers/CustomsAnswersComponent");
var DeclarationCorrectionsComponent_1 = require("./Components/Corrections/DeclarationCorrectionsComponent");
var CertificateTabComponent_1 = require("./Components/Certificate/CertificateTabComponent");
var ConstraintsDetailsComponent_1 = require("./Components/CustomsAnswers/ConstraintsDetailsComponent");
var AgentObjectionComponent_1 = require("./Components/CustomsAnswers/AgentObjectionComponent");
var ConstraintDetailWithCollateralComponent_1 = require("./Components/CustomsAnswers/ConstraintDetailWithCollateralComponent");
var DeclarationDocsInTabComponent_1 = require("./Components/DocsIn/DeclarationDocsInTabComponent");
var NotificationReplyTabComponent_1 = require("./Components/NotificationReply/NotificationReplyTabComponent");
var DeclarationPaymentOrderTabComponent_1 = require("./Components/PaymentOrder/DeclarationPaymentOrderTabComponent");
var DeclarationTaxesTabComponent_1 = require("./Components/Taxes/DeclarationTaxesTabComponent");
var ItemTaxesMoreFieldsComponent_1 = require("./Components/Taxes/ItemTaxesMoreFieldsComponent");
var DeclarationTapagTabComponent_1 = require("./Components/Tapag/DeclarationTapagTabComponent");
var DeclarationCollateralsComponent_1 = require("./Components/Collateral/DeclarationCollateralsComponent");
var DeclarationPhysicalCheckTabComponent_1 = require("./Components/PhysicalCheck/DeclarationPhysicalCheckTabComponent");
var CreateEditTicketComponent_1 = require("./Components/Certificate/CreateEditTicketComponent");
var CertificateSelectionComponent_1 = require("./Components/Certificate/CertificateSelectionComponent");
var DeclarationCargoSplitTabComponent_1 = require("./Components/CargoSplit/DeclarationCargoSplitTabComponent");
var ImporterDetailsComponent_1 = require("./Components/General/ImporterDetails/ImporterDetailsComponent");
var GuaranteeDataComponent_1 = require("./Components/Tapag/GuaranteeDataComponent");
var DeclarationClassificationComponent_1 = require("./Components/Classification/DeclarationClassificationComponent");
var SInvoiceClassificationTabComponent_1 = require("./Components/Classification/SInvoiceClassificationTabComponent");
exports.Components = [
    DeclarationGeneralComponent_1.DeclarationGeneralComponent,
    ConsigmentTabContentComponent_1.ConsigmentTabContentComponent,
    CustomsAnswersComponent_1.CustomsAnswersComponent,
    DeclarationCorrectionsComponent_1.DeclarationCorrectionsComponent,
    CertificateTabComponent_1.CertificateTabComponent,
    ConstraintsDetailsComponent_1.ConstraintsDetailsComponent,
    AgentObjectionComponent_1.AgentObjectionComponent,
    ConstraintDetailWithCollateralComponent_1.ConstraintDetailWithCollateralComponent,
    DeclarationDocsInTabComponent_1.DeclarationDocsInTabComponent,
    NotificationReplyTabComponent_1.NotificationReplyTabComponent,
    DeclarationPaymentOrderTabComponent_1.DeclarationPaymentOrderTabComponent,
    DeclarationTaxesTabComponent_1.DeclarationTaxesTabComponent,
    ItemTaxesMoreFieldsComponent_1.ItemTaxesMoreFieldsComponent,
    DeclarationPhysicalCheckTabComponent_1.DeclarationPhysicalCheckTabComponent,
    CreateEditTicketComponent_1.CreateEditTicketComponent,
    CertificateSelectionComponent_1.CertificateSelectionComponent,
    DeclarationTapagTabComponent_1.DeclarationTapagTabComponent,
    DeclarationCollateralsComponent_1.DeclarationCollateralsComponent,
    DeclarationCargoSplitTabComponent_1.DeclarationCargoSplitTabComponent,
    ImporterDetailsComponent_1.ImporterDetailsComponent,
    GuaranteeDataComponent_1.GuaranteeDataComponent,
    DeclarationClassificationComponent_1.DeclarationClassificationComponent,
    SInvoiceClassificationTabComponent_1.SInvoiceClassificationTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "DeclarationGeneralComponent": {
                myResult = DeclarationGeneralComponent_1.DeclarationGeneralComponent;
                break;
            }
            case "ConsigmentTabContentComponent": {
                myResult = ConsigmentTabContentComponent_1.ConsigmentTabContentComponent;
                break;
            }
            case "CustomsAnswersComponent": {
                myResult = CustomsAnswersComponent_1.CustomsAnswersComponent;
                break;
            }
            case "DeclarationCorrectionsComponent": {
                myResult = DeclarationCorrectionsComponent_1.DeclarationCorrectionsComponent;
                break;
            }
            case "CertificateTabComponent": {
                myResult = CertificateTabComponent_1.CertificateTabComponent;
                break;
            }
            case "CreateEditTicketComponent": {
                myResult = CreateEditTicketComponent_1.CreateEditTicketComponent;
                break;
            }
            case "ConstraintsDetailsComponent": {
                myResult = ConstraintsDetailsComponent_1.ConstraintsDetailsComponent;
                break;
            }
            case "AgentObjectionComponent": {
                myResult = AgentObjectionComponent_1.AgentObjectionComponent;
                break;
            }
            case "ConstraintDetailWithCollateralComponent": {
                myResult = ConstraintDetailWithCollateralComponent_1.ConstraintDetailWithCollateralComponent;
                break;
            }
            case "DeclarationDocsInTabComponent": {
                myResult = DeclarationDocsInTabComponent_1.DeclarationDocsInTabComponent;
                break;
            }
            case "CertificateSelectionComponent": {
                myResult = CertificateSelectionComponent_1.CertificateSelectionComponent;
                break;
            }
            case "NotificationReplyTabComponent": {
                myResult = NotificationReplyTabComponent_1.NotificationReplyTabComponent;
                break;
            }
            case "DeclarationPaymentOrderTabComponent": {
                myResult = DeclarationPaymentOrderTabComponent_1.DeclarationPaymentOrderTabComponent;
                break;
            }
            case "DeclarationPhysicalCheckTabComponent": {
                myResult = DeclarationPhysicalCheckTabComponent_1.DeclarationPhysicalCheckTabComponent;
                break;
            }
            case "DeclarationTaxesTabComponent": {
                myResult = DeclarationTaxesTabComponent_1.DeclarationTaxesTabComponent;
                break;
            }
            case "ItemTaxesMoreFieldsComponent": {
                myResult = ItemTaxesMoreFieldsComponent_1.ItemTaxesMoreFieldsComponent;
                break;
            }
            case "DeclarationTapagTabComponent": {
                myResult = DeclarationTapagTabComponent_1.DeclarationTapagTabComponent;
                break;
            }
            case "DeclarationCollateralsComponent": {
                myResult = DeclarationCollateralsComponent_1.DeclarationCollateralsComponent;
                break;
            }
            case "DeclarationCargoSplitTabComponent": {
                myResult = DeclarationCargoSplitTabComponent_1.DeclarationCargoSplitTabComponent;
                break;
            }
            case "ImporterDetailsComponent": {
                myResult = ImporterDetailsComponent_1.ImporterDetailsComponent;
                break;
            }
            case "GuaranteeDataComponent": {
                myResult = GuaranteeDataComponent_1.GuaranteeDataComponent;
                break;
            }
            case "DeclarationClassificationComponent": {
                myResult = DeclarationClassificationComponent_1.DeclarationClassificationComponent;
                break;
            }
            case "SInvoiceClassificationTabComponent": {
                myResult = SInvoiceClassificationTabComponent_1.SInvoiceClassificationTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map