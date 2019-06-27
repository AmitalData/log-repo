import {ActivityListService} from './Services/StandardLists/ActivityListService';
import {ActivityOwnerHistoryListService} from './Services/StandardLists/ActivityOwnerHistoryListService';
import {ActivityPriorityListService} from './Services/StandardLists/ActivityPriorityListService';
import {ActivityStatusListService} from './Services/StandardLists/ActivityStatusListService';
import {ActivityTimeTypeListService} from './Services/StandardLists/ActivityTimeTypeListService';
import {ActivityTypeListService} from './Services/StandardLists/ActivityTypeListService';
import {CallTypeListService} from './Services/StandardLists/CallTypeListService';
import {CorrespondenceListService} from './Services/StandardLists/CorrespondenceListService';
import {CorrespondencesAttachmentListService} from './Services/StandardLists/CorrespondencesAttachmentListService';
import {CRMFilterSettingListService} from './Services/StandardLists/CRMFilterSettingListService';
import {EmployeeGroupListService} from './Services/StandardLists/EmployeeGroupListService';
import {EscalationActionTimeIndicatorListService} from './Services/StandardLists/EscalationActionTimeIndicatorListService';
import {EscalationPreDefinitionListService} from './Services/StandardLists/EscalationPreDefinitionListService';
import {OpportunityClosingReasonListService} from './Services/StandardLists/OpportunityClosingReasonListService';
import {OpportunityListService} from './Services/StandardLists/OpportunityListService';
import {OpportunityStageListService} from './Services/StandardLists/OpportunityStageListService';
import {OpportunityTypeListService} from './Services/StandardLists/OpportunityTypeListService';
import {QuestionnaireAnswerListService} from './Services/StandardLists/QuestionnaireAnswerListService';
import {QuestionnaireListService} from './Services/StandardLists/QuestionnaireListService';
import {RatingListService} from './Services/StandardLists/RatingListService';
import {SLAHeaderListService} from './Services/StandardLists/SLAHeaderListService';
import {StageListService} from './Services/StandardLists/StageListService';
import {TicketClassificationListService} from './Services/StandardLists/TicketClassificationListService';
import {TicketCreatedByTypeListService} from './Services/StandardLists/TicketCreatedByTypeListService';
import {TicketEscalationListService} from './Services/StandardLists/TicketEscalationListService';
import {TicketListService} from './Services/StandardLists/TicketListService';
import {TicketSeverityListService} from './Services/StandardLists/TicketSeverityListService';
import {TicketSourceListService} from './Services/StandardLists/TicketSourceListService';
import {TicketStageListService} from './Services/StandardLists/TicketStageListService';
import {TicketTypeListService} from './Services/StandardLists/TicketTypeListService';
import {TimeUnitListService} from './Services/StandardLists/TimeUnitListService';

import {ActivityOwnerHistoryPMService} from './Services/StandardPMs/ActivityOwnerHistoryPMService';
import {ActivityPMService} from './Services/StandardPMs/ActivityPMService';
import {CorrespondencePMService} from './Services/StandardPMs/CorrespondencePMService';
import {CorrespondencesAttachmentPMService} from './Services/StandardPMs/CorrespondencesAttachmentPMService';
import {EmployeeGroupPMService} from './Services/StandardPMs/EmployeeGroupPMService';
import {OpportunityClosingReasonPMService} from './Services/StandardPMs/OpportunityClosingReasonPMService';
import {OpportunityPMService} from './Services/StandardPMs/OpportunityPMService';
import {OpportunityStagePMService} from './Services/StandardPMs/OpportunityStagePMService';
import {OpportunityTypePMService} from './Services/StandardPMs/OpportunityTypePMService';
import {QuestionnaireAnswerPMService} from './Services/StandardPMs/QuestionnaireAnswerPMService';
import {QuestionnairePMService} from './Services/StandardPMs/QuestionnairePMService';
import {SLAHeaderPMService} from './Services/StandardPMs/SLAHeaderPMService';
import {StagePMService} from './Services/StandardPMs/StagePMService';
import {TicketClassificationPMService} from './Services/StandardPMs/TicketClassificationPMService';
import {TicketEscalationPMService} from './Services/StandardPMs/TicketEscalationPMService';
import {TicketPMService} from './Services/StandardPMs/TicketPMService';
import {TicketSeverityPMService} from './Services/StandardPMs/TicketSeverityPMService';
import {TicketStagePMService} from './Services/StandardPMs/TicketStagePMService';
import { TicketTypePMService } from './Services/StandardPMs/TicketTypePMService';
import { OccasionTypeListService } from './Services/StandardLists/OccasionTypeListService';
import { OccasionStatusListService } from './Services/StandardLists/OccasionStatusListService';
import { OccasionTypePMService } from './Services/StandardPMs/OccasionTypePMService';

// Menu Buttons 
import {TicketMenuButtonsHandler} from './Components/MenuButtons/TicketMenuButtonsHandler';
import {ActivityMenuButtonsHandler} from './Components/MenuButtons/ActivityMenuButtonsHandler';
import {OpportunityMenuButtonsHandler} from './Components/MenuButtons/OpportunityMenuButtonsHandler';


export class ModuleProviders {
    public static GetInstance(name: string) {

        var myResult: any = null;

        switch (name) {

            // List
            case "ActivityListService": { myResult = new ActivityListService(); break; }
            case "ActivityOwnerHistoryListService": { myResult = new ActivityOwnerHistoryListService(); break; }
            case "ActivityPriorityListService": { myResult = new ActivityPriorityListService(); break; }
            case "ActivityStatusListService": { myResult = new ActivityStatusListService(); break; }
            case "ActivityTimeTypeListService": { myResult = new ActivityTimeTypeListService(); break; }
            case "ActivityTypeListService": { myResult = new ActivityTypeListService(); break; }
            case "CallTypeListService": { myResult = new CallTypeListService(); break; }
            case "CorrespondenceListService": { myResult = new CorrespondenceListService(); break; }
            case "CorrespondencesAttachmentListService": { myResult = new CorrespondencesAttachmentListService(); break; }
            case "CRMFilterSettingListService": { myResult = new CRMFilterSettingListService(); break; }
            case "EmployeeGroupListService": { myResult = new EmployeeGroupListService(); break; }
            case "EscalationActionTimeIndicatorListService": { myResult = new EscalationActionTimeIndicatorListService(); break; }
            case "EscalationPreDefinitionListService": { myResult = new EscalationPreDefinitionListService(); break; }
            case "OpportunityClosingReasonListService": { myResult = new OpportunityClosingReasonListService(); break; }
            case "OpportunityListService": { myResult = new OpportunityListService(); break; }
            case "OpportunityStageListService": { myResult = new OpportunityStageListService(); break; }
            case "OpportunityTypeListService": { myResult = new OpportunityTypeListService(); break; }
            case "QuestionnaireAnswerListService": { myResult = new QuestionnaireAnswerListService(); break; }
            case "QuestionnaireListService": { myResult = new QuestionnaireListService(); break; }
            case "RatingListService": { myResult = new RatingListService(); break; }
            case "SLAHeaderListService": { myResult = new SLAHeaderListService(); break; }
            case "StageListService": { myResult = new StageListService(); break; }
            case "TicketClassificationListService": { myResult = new TicketClassificationListService(); break; }
            case "TicketCreatedByTypeListService": { myResult = new TicketCreatedByTypeListService(); break; }
            case "TicketEscalationListService": { myResult = new TicketEscalationListService(); break; }
            case "TicketListService": { myResult = new TicketListService(); break; }
            case "TicketSeverityListService": { myResult = new TicketSeverityListService(); break; }
            case "TicketSourceListService": { myResult = new TicketSourceListService(); break; }
            case "TicketStageListService": { myResult = new TicketStageListService(); break; }
            case "TicketTypeListService": { myResult = new TicketTypeListService(); break; }
            case "TimeUnitListService": { myResult = new TimeUnitListService(); break; }

            // PM
            case "ActivityOwnerHistoryPMService": { myResult = new ActivityOwnerHistoryPMService(); break; }
            case "ActivityPMService": { myResult = new ActivityPMService(); break; }
            case "CorrespondencePMService": { myResult = new CorrespondencePMService(); break; }
            case "CorrespondencesAttachmentPMService": { myResult = new CorrespondencesAttachmentPMService(); break; }
            case "EmployeeGroupPMService": { myResult = new EmployeeGroupPMService(); break; }
            case "OpportunityClosingReasonPMService": { myResult = new OpportunityClosingReasonPMService(); break; }
            case "OpportunityPMService": { myResult = new OpportunityPMService(); break; }
            case "OpportunityStagePMService": { myResult = new OpportunityStagePMService(); break; }
            case "OpportunityTypePMService": { myResult = new OpportunityTypePMService(); break; }
            case "QuestionnaireAnswerPMService": { myResult = new QuestionnaireAnswerPMService(); break; }
            case "QuestionnairePMService": { myResult = new QuestionnairePMService(); break; }
            case "SLAHeaderPMService": { myResult = new SLAHeaderPMService(); break; }
            case "StagePMService": { myResult = new StagePMService(); break; }
            case "TicketClassificationPMService": { myResult = new TicketClassificationPMService(); break; }
            case "TicketEscalationPMService": { myResult = new TicketEscalationPMService(); break; }
            case "TicketPMService": { myResult = new TicketPMService(); break; }
            case "TicketSeverityPMService": { myResult = new TicketSeverityPMService(); break; }
            case "TicketStagePMService": { myResult = new TicketStagePMService(); break; }
            case "TicketTypePMService": { myResult = new TicketTypePMService(); break; }
 
            case "OccasionTypeListService": { myResult = new OccasionTypeListService(); break; }
            case "OccasionStatusListService": { myResult = new OccasionStatusListService(); break; }
            case "OccasionTypePMService": { myResult = new OccasionTypePMService(); break; }

            //Menu Buttons
            case "TicketMenuButtonsHandler": { myResult = new TicketMenuButtonsHandler(); break; }
            case "ActivityMenuButtonsHandler": { myResult = new ActivityMenuButtonsHandler(); break; }
            case "OpportunityMenuButtonsHandler": { myResult = new OpportunityMenuButtonsHandler(); break; }
            case "OccasionTypeListService": { myResult = new OccasionTypeListService(); break; }
                
        }

        return myResult;
    }
}
