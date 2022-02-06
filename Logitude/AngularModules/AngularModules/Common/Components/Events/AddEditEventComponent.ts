import {Component} from '@angular/core';
import {EventItemClass} from './EventsTabComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TraceEventPM} from '../../../Infrastructure/EntityPMs/TraceEventPM';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TraceEventPMService} from '../../../Infrastructure/Services/StandardPMs/TraceEventPMService';
import {WebFreightDomainService, NewTraceEventResult} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    
    templateUrl: './AddEditEventComponent.html',
})

export class AddEditEventComponent {
    public ValidationErrorsList: string[] = [];
    public EntityPM: TraceEventPM = null;
    public ObjectTableName = "TraceEvent";
    public DataContext: EventItemClass;
    private myPMService: TraceEventPMService = null;
    private myDomainService: WebFreightDomainService = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetWindowArgs(args: EventItemClass) {
        this.DataContext = args;
        this.EntityPM = args.EntityPM;
        this.EntityPM.CloneMe();
    }

    CancelButtonClicked() {
        this.EntityPM.RejectChanges();
        this.CurrentSession.CurrentWindow.Close("Cancel");
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

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
                                if (this.CurrentSession.CurrentEditComponent) {
                                    if (this.CurrentSession.CurrentEditComponent.EntityId == myResult.EntityId) {
                                        switch (this.CurrentSession.CurrentEditComponent.ObjectTableName) {
                                            case "Shipment":
                                            case "Master": {
                                                var isEntityDirty = this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.StatusId = myResult.StatusId;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.StatusName = myResult.StatusName;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.StatusDate = myResult.StatusDate;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.StatusLocation = myResult.StatusLocation;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.LastStatusLogDate = myResult.LastStatusLogDate;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventId = myResult.LastSharedEventId;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventLocation = myResult.LastSharedEventLocation;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventNotes = myResult.LastSharedEventNotes;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.LastSharedEventDate = myResult.LastSharedEventDate;
                                                this.CurrentSession.CurrentEditComponent.EntityPM.IsDirty = isEntityDirty;
                                                this.CurrentSession.CurrentEditComponent.BuildHeaderScreen();
                                                this.CurrentSession.CurrentEditComponent.LoadCompleted.emit(true);
                                                break;
                                            }

                                        }
                                    }
                                }
                            }

                            this.CurrentSession.CloseCurrentWindow();
                            this.DataContext.father.LoadData();
                        }
                    }

                    this.CurrentSession.StopBusyIndicator();
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
                            this.CurrentSession.CloseCurrentWindow();
                            this.DataContext.father.LoadData();
                        }
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }
        }
    }
}
