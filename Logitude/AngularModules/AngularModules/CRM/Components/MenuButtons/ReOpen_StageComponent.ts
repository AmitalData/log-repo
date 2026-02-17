import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {OpportunityPM} from '../../EntityPMs/OpportunityPM';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {StageListService} from '../../Services/StandardLists/StageListService';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {StageList} from '../../EntityLists/StageList';
@Component({
    moduleId: module.id,
    templateUrl: './ReOpen_StageComponent.html',
})

export class ReOpen_StageComponent extends BaseComponent {
    public entityPM: OpportunityPM;
    public ObjectTableName: string = "Opportunity";
    public DataContext: ReOpen_StageComponent = this;
    public ValidationErrorsList: Array<string> = [];
    private isSelectable: boolean = true;
    public get IsSelectable() { return this.isSelectable; }
    public set IsSelectable(value: boolean) { this.isSelectable = value; }
    public get StageId() { return this.entityPM.StageId; }
    private CurrentSession = SessionLocator.SelectedSession;

    SetWindowArgs(args: OpportunityPM) {
        this.entityPM = args;
    }
    public set StageId(value: string) {
        this.entityPM.StageId = value;
        if (!AppTool.IsNullOrEmpty(value)) {
            this.UIProperties.SetRequired("StageId", "Opportunity", false);
        }
        else {
            this.UIProperties.SetRequired("StageId", "Opportunity", true);
        }


    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("cancle");
    }

    OkButtonClicked()
    {
        this.ValidationErrorsList = [];

        if (AppTool.IsNullOrEmpty(this.StageId)) {
            this.ValidationErrorsList.push("Stage Field is required !");
        }


        if (this.ValidationErrorsList.length == 0) {
            var listService: StageListService = new StageListService();

            listService.getAllFromCache().subscribe(result => {
                var stage: StageList = result.Result.filter(d => d.Id == this.StageId)[0];
                if (stage != null) {
                    this.entityPM.Probability = stage.Probability;
                    this.entityPM.StageName = stage.Name;
                }

                this.CurrentSession.CurrentEditComponent.SaveChanges("ReOpening Opportunity...");
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            });

          }


    }

}
