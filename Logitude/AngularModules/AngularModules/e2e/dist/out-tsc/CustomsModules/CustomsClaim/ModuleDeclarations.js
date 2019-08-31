"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewClaimComponent_1 = require("./Components/NewEntity/NewClaimComponent");
var PointersFromClaimRelatedEntitiesSelectionComponent_1 = require("./Components/Documents/PointersFromClaimRelatedEntitiesSelectionComponent");
var ClaimGeneralTabComponent_1 = require("./Components/EditTabs/General/ClaimGeneralTabComponent");
var ClaimRelatedEntityTabComponent_1 = require("./Components/EditTabs/RelatedEntity/ClaimRelatedEntityTabComponent");
var ClaimRelatedEntityGeneralTabComponent_1 = require("./Components/EditTabs/RelatedEntity/ClaimRelatedEntityGeneralTabComponent");
var ClaimRelatedEntityAdditionalDataTabComponent_1 = require("./Components/EditTabs/RelatedEntity/ClaimRelatedEntityAdditionalDataTabComponent");
var ClaimRelatedEntReasonExpComponent_1 = require("./Components/EditTabs/RelatedEntity/ClaimRelatedEntReasonExpComponent");
var ClaimRelatedEntityReasonsTabComponent_1 = require("./Components/EditTabs/RelatedEntity/ClaimRelatedEntityReasonsTabComponent");
var ClaimRelatedEntityCustomAnswerTabComponent_1 = require("./Components/EditTabs/RelatedEntity/ClaimRelatedEntityCustomAnswerTabComponent");
var ClaimImporterDeclATabComponent_1 = require("./Components/EditTabs/ImporterDeclaration/ClaimImporterDeclATabComponent");
var ClaimImporterDeclBCTabComponent_1 = require("./Components/EditTabs/ImporterDeclaration/ClaimImporterDeclBCTabComponent");
var ClaimRefundDetailsTabComponent_1 = require("./Components/EditTabs/Refund/ClaimRefundDetailsTabComponent");
var SendClaimComponent_1 = require("./Components/SendClaim/SendClaimComponent");
var ClaimImporterDeclAP3LoisComponent_1 = require("./Components/EditTabs/ImporterDeclaration/ClaimImporterDeclAP3LoisComponent");
var ClaimRelatedEntityClaimDecisionTabComponent_1 = require("./Components/EditTabs/RelatedEntity/ClaimRelatedEntityClaimDecisionTabComponent");
exports.Components = [
    NewClaimComponent_1.NewClaimComponent,
    PointersFromClaimRelatedEntitiesSelectionComponent_1.PointersFromClaimRelatedEntitiesSelectionComponent,
    ClaimGeneralTabComponent_1.ClaimGeneralTabComponent,
    ClaimRelatedEntityTabComponent_1.ClaimRelatedEntityTabComponent,
    ClaimRelatedEntityGeneralTabComponent_1.ClaimRelatedEntityGeneralTabComponent,
    ClaimRelatedEntityAdditionalDataTabComponent_1.ClaimRelatedEntityAdditionalDataTabComponent,
    ClaimRelatedEntReasonExpComponent_1.ClaimRelatedEntReasonExpComponent,
    ClaimRelatedEntityReasonsTabComponent_1.ClaimRelatedEntityReasonsTabComponent,
    ClaimRelatedEntityCustomAnswerTabComponent_1.ClaimRelatedEntityCustomAnswerTabComponent,
    ClaimImporterDeclATabComponent_1.ClaimImporterDeclATabComponent,
    ClaimImporterDeclBCTabComponent_1.ClaimImporterDeclBCTabComponent,
    ClaimRefundDetailsTabComponent_1.ClaimRefundDetailsTabComponent,
    SendClaimComponent_1.SendClaimComponent,
    ClaimImporterDeclAP3LoisComponent_1.ClaimImporterDeclAP3LoisComponent,
    ClaimRelatedEntityClaimDecisionTabComponent_1.ClaimRelatedEntityClaimDecisionTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewClaimComponent": {
                myResult = NewClaimComponent_1.NewClaimComponent;
                break;
            }
            case "PointersFromClaimRelatedEntitiesSelectionComponent": {
                myResult = PointersFromClaimRelatedEntitiesSelectionComponent_1.PointersFromClaimRelatedEntitiesSelectionComponent;
                break;
            }
            case "ClaimGeneralTabComponent": {
                myResult = ClaimGeneralTabComponent_1.ClaimGeneralTabComponent;
                break;
            }
            case "ClaimRelatedEntityTabComponent": {
                myResult = ClaimRelatedEntityTabComponent_1.ClaimRelatedEntityTabComponent;
                break;
            }
            case "ClaimRelatedEntityGeneralTabComponent": {
                myResult = ClaimRelatedEntityGeneralTabComponent_1.ClaimRelatedEntityGeneralTabComponent;
                break;
            }
            case "ClaimRelatedEntityAdditionalDataTabComponent": {
                myResult = ClaimRelatedEntityAdditionalDataTabComponent_1.ClaimRelatedEntityAdditionalDataTabComponent;
                break;
            }
            case "ClaimRelatedEntReasonExpComponent": {
                myResult = ClaimRelatedEntReasonExpComponent_1.ClaimRelatedEntReasonExpComponent;
                break;
            }
            case "ClaimRelatedEntityReasonsTabComponent": {
                myResult = ClaimRelatedEntityReasonsTabComponent_1.ClaimRelatedEntityReasonsTabComponent;
                break;
            }
            case "ClaimRelatedEntityCustomAnswerTabComponent": {
                myResult = ClaimRelatedEntityCustomAnswerTabComponent_1.ClaimRelatedEntityCustomAnswerTabComponent;
                break;
            }
            case "ClaimImporterDeclATabComponent": {
                myResult = ClaimImporterDeclATabComponent_1.ClaimImporterDeclATabComponent;
                break;
            }
            case "ClaimImporterDeclBCTabComponent": {
                myResult = ClaimImporterDeclBCTabComponent_1.ClaimImporterDeclBCTabComponent;
                break;
            }
            case "ClaimRefundDetailsTabComponent": {
                myResult = ClaimRefundDetailsTabComponent_1.ClaimRefundDetailsTabComponent;
                break;
            }
            case "SendClaimComponent": {
                myResult = SendClaimComponent_1.SendClaimComponent;
                break;
            }
            case "ClaimImporterDeclAP3LoisComponent": {
                myResult = ClaimImporterDeclAP3LoisComponent_1.ClaimImporterDeclAP3LoisComponent;
                break;
            }
            case "ClaimRelatedEntityClaimDecisionTabComponent": {
                myResult = ClaimRelatedEntityClaimDecisionTabComponent_1.ClaimRelatedEntityClaimDecisionTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map