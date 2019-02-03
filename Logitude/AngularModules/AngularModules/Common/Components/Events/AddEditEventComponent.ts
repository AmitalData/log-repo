import {Component} from '@angular/core';
import {EventItemClass} from './EventsTabComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TraceEventPM} from '../../../Infrastructure/EntityPMs/TraceEventPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TraceEventPMService} from '../../../Infrastructure/Services/StandardPMs/TraceEventPMService';
import {WebFreightDomainService, NewTraceEventResult} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditEventComponent.html',
})

export class AddEditEventComponent {
    public ValidationErrorsList: string[] = [];
    public EntityPM: TraceEventPM = null;
    public ObjectTableName = "TraceEvent";
    public DataContext: EventItemClass;
    private myPMService: TraceEventPMService = null;
    private myDomainService: WebFreightDomainService = null;
    constructor() {

    }

    SetWindowArgs(args: EventItemClass) {
        this.DataContext = args;
        this.EntityPM = args.EntityPM;
        this.EntityPM.CloneMe();
    }

    CancelButtonClicked() {
        this.EntityPM.RejectChanges();
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            SessionLocator.CurrentSession.StartBusyIndicatorSaving();

            if (this.DataContext.IsNewEntity) {
                if (this.myDomainService == null) {
                    this.myDomainService = new WebFreightDomainService();
                }

                this.myDomainService.InsertTraceEvent(this.EntityPM.EntityId, this.EntityPM.ObjectTableId, this.EntityPM.EventTypeId, this.EntityPM.EventDateTime, this.EntityPM.Notes).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            var myResult: NewTraceEventResult = myResponse.Result;

                            if (myResult.StatusChanged) {
                                if (SessionLocator.CurrentSession.CurrentEditComponent) {
                                    if (SessionLocator.CurrentSession.CurrentEditComponent.EntityId == myResult.EntityId) {
                                        switch (SessionLocator.CurrentSession.CurrentEditComponent.ObjectTableName) {
                                            case "Shipment":
                                            case "Master": {
                                                var isEntityDirty = SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.IsDirty;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.StatusId = myResult.StatusId;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.StatusName = myResult.StatusName;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.StatusDate = myResult.StatusDate;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.StatusLocation = myResult.StatusLocation;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.LastStatusLogDate = myResult.LastStatusLogDate;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventId = myResult.LastSharedEventId;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventLocation = myResult.LastSharedEventLocation;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventNotes = myResult.LastSharedEventNotes;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventDate = myResult.LastSharedEventDate;
                                                SessionLocator.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = isEntityDirty;
                                                SessionLocator.CurrentSession.CurrentEditComponent.BuildHeaderScreen();
                                                SessionLocator.CurrentSession.CurrentEditComponent.LoadCompleted.emit(true);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            SessionLocator.CurrentSession.CloseCurrentWindow();
                            this.DataContext.father.LoadData();
                        }
                    }

                    SessionLocator.CurrentSession.StopBusyIndicator();
                });
            }

            else {
                if (this.myPMService == null) {
                    this.myPMService = new TraceEventPMService();
                }

                this.myPMService.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    if (myResponse != null) {
                        if (myResponse.HasError) {
                            this.ValidationErrorsList = myResponse.ErrorsArray;
                        }

                        else {
                            SessionLocator.CurrentSession.CloseCurrentWindow();
                            this.DataContext.father.LoadData();
                        }
                    }

                    SessionLocator.CurrentSession.StopBusyIndicator();
                });
            }
        }
    }
}
