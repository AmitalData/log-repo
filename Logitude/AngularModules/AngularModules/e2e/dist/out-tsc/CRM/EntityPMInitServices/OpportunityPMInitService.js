"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var OpportunityTypeListService_1 = require("../Services/StandardLists/OpportunityTypeListService");
var Tools_1 = require("../../Infrastructure/Tools");
var StageListService_1 = require("../Services/StandardLists/StageListService");
var OpportunityPMInitService = /** @class */ (function () {
    function OpportunityPMInitService() {
    }
    OpportunityPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            this.NewOpportunitySetData(entityPM, isNew);
        }
        else {
            this.ApplyUIPoperties(entityPM, false);
        }
    };
    OpportunityPMInitService.NewOpportunitySetData = function (entityPM, isNew) {
        entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        entityPM.CreateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        entityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        entityPM.OwnerId = SessionLocator_1.SessionLocator.LoggedUserId;
        entityPM.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
        entityPM.RatingCode = "N";
        var stageListService = new StageListService_1.StageListService();
        stageListService.getAllFromCache().subscribe(function (result) {
            var myStage = result.Result.filter(function (d) { return d.Code == "QUA" && d.Tenant == SessionLocator_1.SessionLocator.Tenant; })[0];
            if (myStage != null) {
                entityPM.StageId = myStage.Id;
                entityPM.StageName = myStage.Name;
                entityPM.Probability = myStage.Probability;
                if (myStage.MaxDays != null) {
                    var date = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    date.setDate(date.getDate() + myStage.MaxDays);
                    entityPM.StageDueDate = date;
                }
            }
        });
    };
    OpportunityPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        var oppTypeListService = new OpportunityTypeListService_1.OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe(function (result) {
            var typeList = result.Result.filter(function (d) { return d.Id == entityPM.OpportunityTypeId; })[0];
            var typeCode = typeList == null ? null : typeList.Code;
            if (typeCode == "T") {
                entityPM.UIProperties.SetVisibility("AgentId", "Opportunity", true);
                entityPM.UIProperties.SetVisibility("ForeignClientId", "Opportunity", true);
                entityPM.UIProperties.SetVisibility("LeadUserId", "Opportunity", false);
                entityPM.UIProperties.SetVisibility("LeadSourceId", "Opportunity", false);
                entityPM.UIProperties.SetVisibility("LeadPartnerId", "Opportunity", false);
                entityPM.UIProperties.SetVisibility("LeadDescription", "Opportunity", false);
            }
            else {
                entityPM.UIProperties.SetVisibility("AgentId", "Opportunity", false);
                entityPM.UIProperties.SetVisibility("ForeignClientId", "Opportunity", false);
                entityPM.UIProperties.SetVisibility("LeadUserId", "Opportunity", true);
                entityPM.UIProperties.SetVisibility("LeadSourceId", "Opportunity", true);
                entityPM.UIProperties.SetVisibility("LeadPartnerId", "Opportunity", true);
                entityPM.UIProperties.SetVisibility("LeadDescription", "Opportunity", true);
            }
            entityPM.UIProperties.SetEnabled("ContactId", "Opportunity", !Tools_1.AppTool.IsNullOrEmpty(entityPM.CustomerId));
        });
    };
    return OpportunityPMInitService;
}());
exports.OpportunityPMInitService = OpportunityPMInitService;
//# sourceMappingURL=OpportunityPMInitService.js.map