import { Component, OnDestroy} from '@angular/core';
import { OccasionPM } from '../../../../CRM/EntityPMs/OccasionPM';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './OccasionGeneralTabComponent.html',
})

export class OccasionGeneralTabComponent extends BaseComponent implements OnDestroy {
    public EntityPM: OccasionPM = null;
    public ObjectTableName: string = "Occasion";
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("OccasionTypeId", this.ObjectTableName, AppTool.IsNullOrEmpty(this.OccasionTypeId));
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) { if (this.EntityPM.Name != value) this.EntityPM.Name = value; }

    public get StartDateTime() { return this.EntityPM.StartDateTime; }
    public set StartDateTime(value: Date) { if (this.EntityPM.StartDateTime != value) this.EntityPM.StartDateTime = value; }

    public get EndDateTime() { return this.EntityPM.EndDateTime; }
    public set EndDateTime(value: Date) { if (this.EntityPM.EndDateTime != value) this.EntityPM.EndDateTime = value; }

    public get Location() { return this.EntityPM.Location; }
    public set Location(value: string) { if (this.EntityPM.Location != value) this.EntityPM.Location = value; }

    public get Goal() { return this.EntityPM.Goal; }
    public set Goal(value: string) { if (this.EntityPM.Goal != value) this.EntityPM.Goal = value; }

    public get IndustryId() { return this.EntityPM.IndustryId; }
    public set IndustryId(value: string) { if (this.EntityPM.IndustryId != value) this.EntityPM.IndustryId = value; }

    public get OwnerId() { return this.EntityPM.OwnerId; }
    public set OwnerId(value: string) { if (this.EntityPM.OwnerId != value) this.EntityPM.OwnerId = value; }

    public get OccasionStatusId() { return this.EntityPM.OccasionStatusId; }
    public set OccasionStatusId(value: string) { if (this.EntityPM.OccasionStatusId != value) this.EntityPM.OccasionStatusId = value; }

    public get OccasionTypeId() { return this.EntityPM.OccasionTypeId; }
    public set OccasionTypeId(value: string) {
        if (this.EntityPM.OccasionTypeId != value) {
            this.EntityPM.OccasionTypeId = value;
            this.SetUIProperties();
        }
    }
}
