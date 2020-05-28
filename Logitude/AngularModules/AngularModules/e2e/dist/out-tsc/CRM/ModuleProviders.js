"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ActivityListService_1 = require("./Services/StandardLists/ActivityListService");
var ActivityOwnerHistoryListService_1 = require("./Services/StandardLists/ActivityOwnerHistoryListService");
var ActivityPriorityListService_1 = require("./Services/StandardLists/ActivityPriorityListService");
var ActivityStatusListService_1 = require("./Services/StandardLists/ActivityStatusListService");
var ActivityTimeTypeListService_1 = require("./Services/StandardLists/ActivityTimeTypeListService");
var ActivityTypeListService_1 = require("./Services/StandardLists/ActivityTypeListService");
var CallTypeListService_1 = require("./Services/StandardLists/CallTypeListService");
var CorrespondenceListService_1 = require("./Services/StandardLists/CorrespondenceListService");
var CorrespondencesAttachmentListService_1 = require("./Services/StandardLists/CorrespondencesAttachmentListService");
var CRMFilterSettingListService_1 = require("./Services/StandardLists/CRMFilterSettingListService");
var EmployeeGroupListService_1 = require("./Services/StandardLists/EmployeeGroupListService");
var EscalationActionTimeIndicatorListService_1 = require("./Services/StandardLists/EscalationActionTimeIndicatorListService");
var EscalationPreDefinitionListService_1 = require("./Services/StandardLists/EscalationPreDefinitionListService");
var OpportunityClosingReasonListService_1 = require("./Services/StandardLists/OpportunityClosingReasonListService");
var OpportunityListService_1 = require("./Services/StandardLists/OpportunityListService");
var OpportunityStageListService_1 = require("./Services/StandardLists/OpportunityStageListService");
var OpportunityTypeListService_1 = require("./Services/StandardLists/OpportunityTypeListService");
var QuestionnaireAnswerListService_1 = require("./Services/StandardLists/QuestionnaireAnswerListService");
var QuestionnaireListService_1 = require("./Services/StandardLists/QuestionnaireListService");
var RatingListService_1 = require("./Services/StandardLists/RatingListService");
var SLAHeaderListService_1 = require("./Services/StandardLists/SLAHeaderListService");
var StageListService_1 = require("./Services/StandardLists/StageListService");
var TicketClassificationListService_1 = require("./Services/StandardLists/TicketClassificationListService");
var TicketCreatedByTypeListService_1 = require("./Services/StandardLists/TicketCreatedByTypeListService");
var TicketEscalationListService_1 = require("./Services/StandardLists/TicketEscalationListService");
var TicketListService_1 = require("./Services/StandardLists/TicketListService");
var TicketSeverityListService_1 = require("./Services/StandardLists/TicketSeverityListService");
var TicketSourceListService_1 = require("./Services/StandardLists/TicketSourceListService");
var TicketStageListService_1 = require("./Services/StandardLists/TicketStageListService");
var TicketTypeListService_1 = require("./Services/StandardLists/TicketTypeListService");
var TimeUnitListService_1 = require("./Services/StandardLists/TimeUnitListService");
var OccasionListService_1 = require("./Services/StandardLists/OccasionListService");
var OccasionTypeListService_1 = require("./Services/StandardLists/OccasionTypeListService");
var OccasionStatusListService_1 = require("./Services/StandardLists/OccasionStatusListService");
var ActivityOwnerHistoryPMService_1 = require("./Services/StandardPMs/ActivityOwnerHistoryPMService");
var ActivityPMService_1 = require("./Services/StandardPMs/ActivityPMService");
var CorrespondencePMService_1 = require("./Services/StandardPMs/CorrespondencePMService");
var CorrespondencesAttachmentPMService_1 = require("./Services/StandardPMs/CorrespondencesAttachmentPMService");
var EmployeeGroupPMService_1 = require("./Services/StandardPMs/EmployeeGroupPMService");
var OpportunityClosingReasonPMService_1 = require("./Services/StandardPMs/OpportunityClosingReasonPMService");
var OpportunityPMService_1 = require("./Services/StandardPMs/OpportunityPMService");
var OpportunityStagePMService_1 = require("./Services/StandardPMs/OpportunityStagePMService");
var OpportunityTypePMService_1 = require("./Services/StandardPMs/OpportunityTypePMService");
var QuestionnaireAnswerPMService_1 = require("./Services/StandardPMs/QuestionnaireAnswerPMService");
var QuestionnairePMService_1 = require("./Services/StandardPMs/QuestionnairePMService");
var SLAHeaderPMService_1 = require("./Services/StandardPMs/SLAHeaderPMService");
var StagePMService_1 = require("./Services/StandardPMs/StagePMService");
var TicketClassificationPMService_1 = require("./Services/StandardPMs/TicketClassificationPMService");
var TicketEscalationPMService_1 = require("./Services/StandardPMs/TicketEscalationPMService");
var TicketPMService_1 = require("./Services/StandardPMs/TicketPMService");
var TicketSeverityPMService_1 = require("./Services/StandardPMs/TicketSeverityPMService");
var TicketStagePMService_1 = require("./Services/StandardPMs/TicketStagePMService");
var TicketTypePMService_1 = require("./Services/StandardPMs/TicketTypePMService");
var OccasionTypePMService_1 = require("./Services/StandardPMs/OccasionTypePMService");
var OccasionPMService_1 = require("./Services/StandardPMs/OccasionPMService");
// Menu Buttons 
var TicketMenuButtonsHandler_1 = require("./Components/MenuButtons/TicketMenuButtonsHandler");
var ActivityMenuButtonsHandler_1 = require("./Components/MenuButtons/ActivityMenuButtonsHandler");
var OpportunityMenuButtonsHandler_1 = require("./Components/MenuButtons/OpportunityMenuButtonsHandler");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            // List
            case "ActivityListService": {
                myResult = new ActivityListService_1.ActivityListService();
                break;
            }
            case "ActivityOwnerHistoryListService": {
                myResult = new ActivityOwnerHistoryListService_1.ActivityOwnerHistoryListService();
                break;
            }
            case "ActivityPriorityListService": {
                myResult = new ActivityPriorityListService_1.ActivityPriorityListService();
                break;
            }
            case "ActivityStatusListService": {
                myResult = new ActivityStatusListService_1.ActivityStatusListService();
                break;
            }
            case "ActivityTimeTypeListService": {
                myResult = new ActivityTimeTypeListService_1.ActivityTimeTypeListService();
                break;
            }
            case "ActivityTypeListService": {
                myResult = new ActivityTypeListService_1.ActivityTypeListService();
                break;
            }
            case "CallTypeListService": {
                myResult = new CallTypeListService_1.CallTypeListService();
                break;
            }
            case "CorrespondenceListService": {
                myResult = new CorrespondenceListService_1.CorrespondenceListService();
                break;
            }
            case "CorrespondencesAttachmentListService": {
                myResult = new CorrespondencesAttachmentListService_1.CorrespondencesAttachmentListService();
                break;
            }
            case "CRMFilterSettingListService": {
                myResult = new CRMFilterSettingListService_1.CRMFilterSettingListService();
                break;
            }
            case "EmployeeGroupListService": {
                myResult = new EmployeeGroupListService_1.EmployeeGroupListService();
                break;
            }
            case "EscalationActionTimeIndicatorListService": {
                myResult = new EscalationActionTimeIndicatorListService_1.EscalationActionTimeIndicatorListService();
                break;
            }
            case "EscalationPreDefinitionListService": {
                myResult = new EscalationPreDefinitionListService_1.EscalationPreDefinitionListService();
                break;
            }
            case "OpportunityClosingReasonListService": {
                myResult = new OpportunityClosingReasonListService_1.OpportunityClosingReasonListService();
                break;
            }
            case "OpportunityListService": {
                myResult = new OpportunityListService_1.OpportunityListService();
                break;
            }
            case "OpportunityStageListService": {
                myResult = new OpportunityStageListService_1.OpportunityStageListService();
                break;
            }
            case "OpportunityTypeListService": {
                myResult = new OpportunityTypeListService_1.OpportunityTypeListService();
                break;
            }
            case "QuestionnaireAnswerListService": {
                myResult = new QuestionnaireAnswerListService_1.QuestionnaireAnswerListService();
                break;
            }
            case "QuestionnaireListService": {
                myResult = new QuestionnaireListService_1.QuestionnaireListService();
                break;
            }
            case "RatingListService": {
                myResult = new RatingListService_1.RatingListService();
                break;
            }
            case "SLAHeaderListService": {
                myResult = new SLAHeaderListService_1.SLAHeaderListService();
                break;
            }
            case "StageListService": {
                myResult = new StageListService_1.StageListService();
                break;
            }
            case "TicketClassificationListService": {
                myResult = new TicketClassificationListService_1.TicketClassificationListService();
                break;
            }
            case "TicketCreatedByTypeListService": {
                myResult = new TicketCreatedByTypeListService_1.TicketCreatedByTypeListService();
                break;
            }
            case "TicketEscalationListService": {
                myResult = new TicketEscalationListService_1.TicketEscalationListService();
                break;
            }
            case "TicketListService": {
                myResult = new TicketListService_1.TicketListService();
                break;
            }
            case "TicketSeverityListService": {
                myResult = new TicketSeverityListService_1.TicketSeverityListService();
                break;
            }
            case "TicketSourceListService": {
                myResult = new TicketSourceListService_1.TicketSourceListService();
                break;
            }
            case "TicketStageListService": {
                myResult = new TicketStageListService_1.TicketStageListService();
                break;
            }
            case "TicketTypeListService": {
                myResult = new TicketTypeListService_1.TicketTypeListService();
                break;
            }
            case "TimeUnitListService": {
                myResult = new TimeUnitListService_1.TimeUnitListService();
                break;
            }
            case "OccasionTypeListService": {
                myResult = new OccasionTypeListService_1.OccasionTypeListService();
                break;
            }
            case "OccasionStatusListService": {
                myResult = new OccasionStatusListService_1.OccasionStatusListService();
                break;
            }
            case "OccasionListService": {
                myResult = new OccasionListService_1.OccasionListService();
                break;
            }
            // PM
            case "ActivityOwnerHistoryPMService": {
                myResult = new ActivityOwnerHistoryPMService_1.ActivityOwnerHistoryPMService();
                break;
            }
            case "ActivityPMService": {
                myResult = new ActivityPMService_1.ActivityPMService();
                break;
            }
            case "CorrespondencePMService": {
                myResult = new CorrespondencePMService_1.CorrespondencePMService();
                break;
            }
            case "CorrespondencesAttachmentPMService": {
                myResult = new CorrespondencesAttachmentPMService_1.CorrespondencesAttachmentPMService();
                break;
            }
            case "EmployeeGroupPMService": {
                myResult = new EmployeeGroupPMService_1.EmployeeGroupPMService();
                break;
            }
            case "OpportunityClosingReasonPMService": {
                myResult = new OpportunityClosingReasonPMService_1.OpportunityClosingReasonPMService();
                break;
            }
            case "OpportunityPMService": {
                myResult = new OpportunityPMService_1.OpportunityPMService();
                break;
            }
            case "OpportunityStagePMService": {
                myResult = new OpportunityStagePMService_1.OpportunityStagePMService();
                break;
            }
            case "OpportunityTypePMService": {
                myResult = new OpportunityTypePMService_1.OpportunityTypePMService();
                break;
            }
            case "QuestionnaireAnswerPMService": {
                myResult = new QuestionnaireAnswerPMService_1.QuestionnaireAnswerPMService();
                break;
            }
            case "QuestionnairePMService": {
                myResult = new QuestionnairePMService_1.QuestionnairePMService();
                break;
            }
            case "SLAHeaderPMService": {
                myResult = new SLAHeaderPMService_1.SLAHeaderPMService();
                break;
            }
            case "StagePMService": {
                myResult = new StagePMService_1.StagePMService();
                break;
            }
            case "TicketClassificationPMService": {
                myResult = new TicketClassificationPMService_1.TicketClassificationPMService();
                break;
            }
            case "TicketEscalationPMService": {
                myResult = new TicketEscalationPMService_1.TicketEscalationPMService();
                break;
            }
            case "TicketPMService": {
                myResult = new TicketPMService_1.TicketPMService();
                break;
            }
            case "TicketSeverityPMService": {
                myResult = new TicketSeverityPMService_1.TicketSeverityPMService();
                break;
            }
            case "TicketStagePMService": {
                myResult = new TicketStagePMService_1.TicketStagePMService();
                break;
            }
            case "TicketTypePMService": {
                myResult = new TicketTypePMService_1.TicketTypePMService();
                break;
            }
            case "OccasionTypePMService": {
                myResult = new OccasionTypePMService_1.OccasionTypePMService();
                break;
            }
            case "OccasionPMService": {
                myResult = new OccasionPMService_1.OccasionPMService();
                break;
            }
            //Menu Buttons
            case "TicketMenuButtonsHandler": {
                myResult = new TicketMenuButtonsHandler_1.TicketMenuButtonsHandler();
                break;
            }
            case "ActivityMenuButtonsHandler": {
                myResult = new ActivityMenuButtonsHandler_1.ActivityMenuButtonsHandler();
                break;
            }
            case "OpportunityMenuButtonsHandler": {
                myResult = new OpportunityMenuButtonsHandler_1.OpportunityMenuButtonsHandler();
                break;
            }
            case "OccasionTypeListService": {
                myResult = new OccasionTypeListService_1.OccasionTypeListService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map