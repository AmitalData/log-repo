import { NewClaimComponent } from './Components/NewEntity/NewClaimComponent';
import { PointersFromClaimRelatedEntitiesSelectionComponent } from './Components/Documents/PointersFromClaimRelatedEntitiesSelectionComponent';
import { ClaimGeneralTabComponent } from './Components/EditTabs/General/ClaimGeneralTabComponent';
import { ClaimRelatedEntityTabComponent } from './Components/EditTabs/RelatedEntity/ClaimRelatedEntityTabComponent';
import { ClaimRelatedEntityGeneralTabComponent } from './Components/EditTabs/RelatedEntity/ClaimRelatedEntityGeneralTabComponent';
import { ClaimRelatedEntityAdditionalDataTabComponent } from './Components/EditTabs/RelatedEntity/ClaimRelatedEntityAdditionalDataTabComponent';
import { ClaimRelatedEntReasonExpComponent } from './Components/EditTabs/RelatedEntity/ClaimRelatedEntReasonExpComponent';
import { ClaimRelatedEntityReasonsTabComponent } from './Components/EditTabs/RelatedEntity/ClaimRelatedEntityReasonsTabComponent';
import { ClaimRelatedEntityCustomAnswerTabComponent } from './Components/EditTabs/RelatedEntity/ClaimRelatedEntityCustomAnswerTabComponent';
import { ClaimImporterDeclATabComponent } from './Components/EditTabs/ImporterDeclaration/ClaimImporterDeclATabComponent';
import { ClaimImporterDeclBCTabComponent } from './Components/EditTabs/ImporterDeclaration/ClaimImporterDeclBCTabComponent';
import { ClaimRefundDetailsTabComponent } from './Components/EditTabs/Refund/ClaimRefundDetailsTabComponent';
import { SendClaimComponent } from './Components/SendClaim/SendClaimComponent';
import { ClaimImporterDeclAP3LoisComponent } from './Components/EditTabs/ImporterDeclaration/ClaimImporterDeclAP3LoisComponent';
import { ClaimRelatedEntityClaimDecisionTabComponent } from './Components/EditTabs/RelatedEntity/ClaimRelatedEntityClaimDecisionTabComponent';
import { ClaimRelatedEntityCancelOrObjectionTabComponent } from './Components/EditTabs/RelatedEntity/ClaimRelatedEntityCancelOrObjectionTabComponent';
import { PassportDetailsComponent } from './Components/EditTabs/General/PassportDetails/PassportDetailsComponent';

export const Components =
    [
        NewClaimComponent,
        PointersFromClaimRelatedEntitiesSelectionComponent,
        ClaimGeneralTabComponent,
        ClaimRelatedEntityTabComponent,
        ClaimRelatedEntityGeneralTabComponent,
        ClaimRelatedEntityAdditionalDataTabComponent,
        ClaimRelatedEntReasonExpComponent,
        ClaimRelatedEntityReasonsTabComponent,
        ClaimRelatedEntityCustomAnswerTabComponent,
        ClaimImporterDeclATabComponent,
        ClaimImporterDeclBCTabComponent,
        ClaimRefundDetailsTabComponent,
        SendClaimComponent,
        ClaimImporterDeclAP3LoisComponent,
        ClaimRelatedEntityClaimDecisionTabComponent,
        ClaimRelatedEntityCancelOrObjectionTabComponent,
        PassportDetailsComponent,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "NewClaimComponent": { myResult = NewClaimComponent; break; }
            case "PointersFromClaimRelatedEntitiesSelectionComponent": { myResult = PointersFromClaimRelatedEntitiesSelectionComponent; break; }
            case "ClaimGeneralTabComponent": { myResult = ClaimGeneralTabComponent; break; }
            case "ClaimRelatedEntityTabComponent": { myResult = ClaimRelatedEntityTabComponent; break; }
            case "ClaimRelatedEntityGeneralTabComponent": { myResult = ClaimRelatedEntityGeneralTabComponent; break; }
            case "ClaimRelatedEntityAdditionalDataTabComponent": { myResult = ClaimRelatedEntityAdditionalDataTabComponent; break; }
            case "ClaimRelatedEntReasonExpComponent": { myResult = ClaimRelatedEntReasonExpComponent; break; }
            case "ClaimRelatedEntityReasonsTabComponent": { myResult = ClaimRelatedEntityReasonsTabComponent; break; }
            case "ClaimRelatedEntityCustomAnswerTabComponent": { myResult = ClaimRelatedEntityCustomAnswerTabComponent; break; }
            case "ClaimImporterDeclATabComponent": { myResult = ClaimImporterDeclATabComponent; break; }
            case "ClaimImporterDeclBCTabComponent": { myResult = ClaimImporterDeclBCTabComponent; break; }
            case "ClaimRefundDetailsTabComponent": { myResult = ClaimRefundDetailsTabComponent; break; }
            case "SendClaimComponent": { myResult = SendClaimComponent; break; }
            case "ClaimImporterDeclAP3LoisComponent": { myResult = ClaimImporterDeclAP3LoisComponent; break; }
            case "ClaimRelatedEntityClaimDecisionTabComponent": { myResult = ClaimRelatedEntityClaimDecisionTabComponent; break; }
            case "ClaimRelatedEntityCancelOrObjectionTabComponent": { myResult = ClaimRelatedEntityCancelOrObjectionTabComponent; break; }
            case "PassportDetailsComponent": { myResult = PassportDetailsComponent; break; }
        }

        return myResult;
    }
}
