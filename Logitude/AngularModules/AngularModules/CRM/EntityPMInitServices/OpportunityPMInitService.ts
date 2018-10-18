import {OpportunityPM} from '../EntityPMs/OpportunityPM';
import {OpportunityList} from '../EntityLists/OpportunityList';
import {OpportunityListService} from '../Services/StandardLists/OpportunityListService';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {OpportunityTypeListService} from '../Services/StandardLists/OpportunityTypeListService';
import {OpportunityTypeList} from '../EntityLists/OpportunityTypeList';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {StageList} from '../EntityLists/StageList';
import {StageListService} from '../Services/StandardLists/StageListService';
import {CardPM} from '../../Common/EntityPMs/CardPM';
import {UserService} from '../../Common/Services/ExtendedLists/UserService';
export class OpportunityPMInitService {

    public static InitValues(entityPM: OpportunityPM, isNew: boolean) {

        if (isNew) {

            this.NewOpportunitySetData(entityPM, isNew);

        }
        else {
            this.ApplyUIPoperties(entityPM, false);

        }
    }

    public static NewOpportunitySetData(entityPM: OpportunityPM, isNew: boolean) {
            entityPM.Tenant = SessionLocator.Tenant;
            entityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            entityPM.OwnerId = SessionLocator.LoggedUserId;
            entityPM.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
            entityPM.RatingCode = "N";

            var stageListService: StageListService = new StageListService();
            stageListService.getAllFromCache().subscribe(result => {
                var myStage: StageList = result.Result.filter(d => d.Code == "QUA" && d.Tenant == SessionLocator.Tenant)[0];

                if (myStage != null) {
                    entityPM.StageId = myStage.Id;
                    entityPM.StageName = myStage.Name;
                    entityPM.Probability = myStage.Probability;

                    if (myStage.MaxDays != null) {
                        var date: Date = DateTool.GetCurrentDateTimeAsUtc();
                        date.setDate(date.getDate() + myStage.MaxDays);
                        entityPM.StageDueDate = date;
                    }
                }
            });     
        

    }

    public static ApplyUIPoperties(entityPM: OpportunityPM, isNew: boolean) {
        
        var oppTypeListService: OpportunityTypeListService = new OpportunityTypeListService();
        oppTypeListService.getAllFromCache().subscribe(result => {

            var typeList: OpportunityTypeList = result.Result.filter(d => d.Id == entityPM.OpportunityTypeId)[0];
            var typeCode: string = typeList == null ? null : typeList.Code;


            if (typeCode == "T") {
                entityPM.UIProperties.SetVisibility("AgentId","Opportunity", true);
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

            entityPM.UIProperties.SetEnabled("ContactId", "Opportunity", !AppTool.IsNullOrEmpty(entityPM.CustomerId));

        });
    }

}