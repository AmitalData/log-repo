import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EscalationArgs} from './NewSLAComponent';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {SLAEscalationPM} from '../../../../CRM/EntityPMs/SLAEscalationPM';
import {SLAEscalationRecepientPM} from '../../../../CRM/EntityPMs/SLAEscalationRecepientPM';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {AppTool} from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditEscalationComponent.html',
})

export class AddEditEscalationComponent extends BaseComponent {

    public ObjectTableName: string = "SLAEscalation";
    public DataContext: EscalationArgs;
    public ValidationErrorsList: string[] = [];
    public EntityPM: SLAEscalationPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetDataContext(dataContext: EscalationArgs) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.entityPM;
        this.Clone();
    }

    // Commands
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var isTimeRequired = this.DataContext.EscalationActionTimeIndicator == "IM" ? false : true;
        if (isTimeRequired) {
            if (this.DataContext.EscalationTime == null || AppTool.IsNullOrEmpty(this.DataContext.EscalationTimeUnit)) {
                errors.push("Time field is required");
            }
        }
        var isPredefinitionEmpty = this.DataContext.PreDefinitionList.filter(a => a.IsChecked == true)[0];
        if ((this.DataContext.UserSelectedList == null || this.DataContext.UserSelectedList.length == 0) && (!isPredefinitionEmpty)) {
            errors.push("User or Recipient is required");
        }

        // User Component work not done
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            if (this.DataContext.isNew) {
                this.DataContext.isNew = false;

                if (this.DataContext.UserSelectedList != null && this.DataContext.UserSelectedList.length > 0) {
                    this.DataContext.UserSelectedList.forEach(item => {
                        var escalationLine = new SLAEscalationRecepientPM(this.EntityPM);
                        escalationLine.Tenant = SessionLocator.Tenant;
                        escalationLine.SLAEscalationId = this.EntityPM.Id;
                        escalationLine.UserId = item.Id;

                        if (this.EntityPM.SLAEscalationRecepients.indexOf(escalationLine) == -1) {
                            this.EntityPM.AddSLAEscalationRecepient(escalationLine);
                        }
                    });
                }

                if (this.DataContext.PreDefinitionList != null && this.DataContext.PreDefinitionList.length > 0) {
                    this.DataContext.PreDefinitionList.forEach(item => {
                        if (item.IsChecked) {
                            var escalationLine = new SLAEscalationRecepientPM(this.EntityPM);
                            escalationLine.Tenant = SessionLocator.Tenant;
                            escalationLine.SLAEscalationId = this.EntityPM.Id;
                            escalationLine.PreDefinitionId = item.Code;

                            if (this.EntityPM.SLAEscalationRecepients.indexOf(escalationLine) == -1) {
                                this.EntityPM.AddSLAEscalationRecepient(escalationLine);
                            }
                        }
                    });
                }

                if (this.DataContext.EscalationFor == "FR") {
                    if (this.DataContext.trigger.FirstResponseEscalationDataList.indexOf(this.DataContext) == -1) {
                        this.DataContext.trigger.FirstResponseEscalationDataList.push(this.DataContext);
                    }

                    if (this.DataContext.trigger.entityPM.SLAEscalations.filter(a => a.EscalationFor == "RW").indexOf(this.EntityPM) == -1) {
                        this.DataContext.trigger.entityPM.AddSLAEscalation(this.EntityPM);
                    }
                }

                else {
                    if (this.DataContext.trigger.ResolveEscalationDataList.indexOf(this.DataContext) == -1) {
                        this.DataContext.trigger.ResolveEscalationDataList.push(this.DataContext);
                    }

                    if (this.DataContext.trigger.entityPM.SLAEscalations.filter(a => a.EscalationFor == "FR").indexOf(this.EntityPM) == -1) {
                        this.DataContext.trigger.entityPM.AddSLAEscalation(this.EntityPM);
                    }
                }
            }

            else {
                this.DataContext.BuildEscalationRecepients();
            }

            if (this.DataContext.EscalationFor == "FR") {
                this.DataContext.trigger.FillResponseEscalationList();
            }

            if (this.DataContext.EscalationFor == "RW") {
                this.DataContext.trigger.FillResolveEscalationList();
            }
            this.CurrentSession.CloseCurrentWindow();
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('IsChecked');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('EscalationActionTimeIndicator');
        this.myCloner.AddField('EscalationTime');
        this.myCloner.AddField('EscalationTimeUnit');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.entityPM);
        this.myCloner.AddEntity(this.DataContext.trigger.entityPM);
        this.DataContext.PreDefinitionList.forEach(item => {
            this.myCloner.AddEntity(item);
        });
        this.myCloner.AddEntity(this.DataContext.PreDefinitionList);
        this.myCloner.AddEntity(this.DataContext.UserSelectedList);
    }
    private RejectChanges() {
        this.DataContext.FillPredefinitionList();
        this.myCloner.RejectChanges();
    }
}
