import { Component} from '@angular/core';
import { OccasionPM } from '../../../../CRM/EntityPMs/OccasionPM';
import { OccasionPMService } from '../../../../CRM/Services/StandardPMs/OccasionPMService';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';


@Component({
    selector: 'NewOccasionComponent',
    moduleId: module.id,
    templateUrl: './NewOccasionComponent.html',
})

export class NewOccasionComponent extends BaseComponent {

    public ObjectTableName: string = "Occasion";
    public DataContext: NewOccasionComponent = this;
    public EntityPM: OccasionPM = new OccasionPM();
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList = [];

    constructor() {
        super(); 
        this.InitiateOccasion();
        this.SetUIProperties();
    }

    InitiateOccasion() {
        var todayDate: Date = DateTool.GetCurrentDateAsUtc();
        this.EntityPM = new OccasionPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = todayDate;
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = todayDate;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.OwnerId = SessionLocator.LoggedUserId;
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

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(this.OccasionTypeId)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("Occasion.F.OccasionTypeId")));
        }

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorCreating();
            var service = new OccasionPMService();
            service.insert(this.EntityPM).subscribe(myResult => {
                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit(mm.Result.Id);
                    this.CurrentSession.StopBusyIndicator();
                }
                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }
}
