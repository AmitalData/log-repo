
declare var System: any;
declare var window: any;
import {ServiceResponse} from '../DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../DataContracts/ApiQueryFilters';
import {DocumentTypeListService} from '../../Common/Services/StandardLists/DocumentTypeListService';
import {DocumentTypeList} from '../../Common/EntityLists/DocumentTypeList';
import {SessionInfo} from '../Utilities/SessionInfo';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {DocumentOutPMService} from '../../Common/Services/ExtendedPMs/DocumentOutPMService';
import {EventTypeExtendedPMService} from '../../Infrastructure/Services/ExtendedPMs/EventTypeExtendedPMService';
import {EventTypePM} from '../../Infrastructure/EntityPMs/EventTypePM';
import {ShipmentFollowUpPM} from '../../Shipment/EntityPMs/ShipmentFollowUpPM';
import {ShipmentPM} from '../../Shipment/EntityPMs/ShipmentPM';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {DocumentsFilingExtendedPMService} from '../../Common/Services/ExtendedPMs/DocumentsFilingExtendedPMService';
import {FollowUpPM} from '../../Infrastructure/EntityPMs/FollowUpPM';
import {FollowUpPMExtendedService} from '../Services/ExtendedPMs/FollowUpPMExtendedService';
import {ServiceLocator} from '../Locators/ServiceLocator';

export class GeneralDocumentFollowUpHelper {



    public _documentOutPMService: DocumentOutPMService;
    public _eventTypeExtendedPMService: EventTypeExtendedPMService;
    public _documentsFilingExtendedPMService: DocumentsFilingExtendedPMService

    public ObjectTableName: string;
    public CurrentObjectTableId: string;
    EventType: EventTypePM;

    public ChildEntityId: string;
    public CurrentEntityId: string;
    public ChildEntityReference: string;
    MessageTotango: string = "";
    TabName: string;
    ParentViewModel: any;
    EntityPM: any;
    EventTypeCode: string = "";
    followUpPMExtendedService: FollowUpPMExtendedService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(objecttablename: string, entityId: string, childEntityId: string, childEntityReference: string, tabName: string ,parentViewModel:any,entityPM:any) {


        var table = window.ObjectTables.filter(d => d.Name == objecttablename)[0];

        if (table) {
            this.CurrentObjectTableId = table.Id;
            this.ObjectTableName = table.Name;

        }

        this.CurrentEntityId = entityId;
        this.ChildEntityId = childEntityId;
        this.ChildEntityReference = childEntityReference;

        if (tabName == "DocOut") {
            this.EventTypeCode = "DOCO";
            this.MessageTotango = "Docs Out Adding FollowUp";
        }
        if (tabName == "DocIn") {
            this.EventTypeCode = "DOCI";
            this.MessageTotango = "Docs In Adding FollowUp";
        }

     
        this.TabName = tabName;

        this.ParentViewModel = parentViewModel;
        this.EntityPM = entityPM ;

        this._documentOutPMService = new DocumentOutPMService();
        this._eventTypeExtendedPMService = new EventTypeExtendedPMService();
        this._documentsFilingExtendedPMService = new DocumentsFilingExtendedPMService();
        this.followUpPMExtendedService = new FollowUpPMExtendedService();

    }

    AddFollowUp() {


        if (this.ParentViewModel && this.EntityPM && this.EntityPM.FollowUps) {
            ServiceLocator.SendTotangoUserActivity(this.ObjectTableName, this.MessageTotango);
             //Add FollowUp
            if (!this.ParentViewModel.HasFollowUp) {
                this.CurrentSession.StartBusyIndicator("Please Wait...");
                this.GetEventTypeByCodeQuery();
            }

             //DeleteFollowUp
            else {
            
                var followup = null;
              
                if (this.EntityPM.FollowUps) {
                    this.CurrentSession.StartBusyIndicator("Saving...");
                    followup = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == this.ParentViewModel.DocumentTypeId && d.Done == false)[0];
                    
                    if (followup) {

                        var items = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == this.ParentViewModel.DocumentTypeId && d.Done == false);
                        if (this.TabName != "DocOut" && items.length > 1 && this.ParentViewModel.CurrentDocument) {

                            followup = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == this.ParentViewModel.DocumentTypeId && d.Done == false && d.ExternalDocumentId == this.ParentViewModel.CurrentDocument.Id)[0];
                            if (!followup) {
                                followup = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == this.ParentViewModel.DocumentTypeId && d.Done == false && AppTool.IsNullOrEmpty(d.ExternalDocumentId))[0];
                            }
                        }

                        if (followup) {
                            if (this.ObjectTableName == "Shipment") {
                                this.EntityPM.RemoveShipmentFollowUp(followup);
                            }
                            else if (this.ObjectTableName == "Quote") {
                                this.EntityPM.RemoveQuoteFollowUpPM(followup);
                            }
                        }
                        
                        this.ParentViewModel.HasFollowUp = false;
                    }

                    this.CurrentSession.FireEvent("FollowupsChanged");
                    this.CurrentSession.StopBusyIndicator();
                }
            }

        }
    }

    MarkFollowUpAsDone() {

        if (this.EntityPM && this.EntityPM.FollowUps && this.ParentViewModel && this.ParentViewModel.HasFollowUp) {

            var followup = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == this.ParentViewModel.DocumentTypeId && d.Done == false)[0];
            if (followup) {

                var items = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == this.ParentViewModel.DocumentTypeId && d.Done == false);

                if (this.TabName != "DocOut" && items.length > 1 && this.ParentViewModel.CurrentDocument) {
                    followup = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == this.ParentViewModel.DocumentTypeId && d.Done == false && d.ExternalDocumentId == this.ParentViewModel.CurrentDocument.Id)[0];
                    if (!followup) {
                        followup = this.EntityPM.FollowUps.filter(d => d.DocumentTypeId == this.ParentViewModel.DocumentTypeId && d.Done == false && AppTool.IsNullOrEmpty(d.ExternalDocumentId))[0];
                    }

                }
            

                if (followup) {

                    this.followUpPMExtendedService.RemoveFollowUpById(followup.Id).subscribe((res: ServiceResponse) => {
                        var pmResponse: ServiceResponse = res;
                        this.CurrentSession.StopBusyIndicator();
                        if (!pmResponse.HasError) {
                            this.ParentViewModel.HasFollowUp = false;
                            this.EntityPM.FollowUps = this.EntityPM.FollowUps.filter(d => d.Id != followup.Id);
                            this.CurrentSession.FireEvent("FollowupsChanged");
                      
                        }
         
                    });

                }

               
            }
        }

    }


 
    GetEventTypeByCodeQuery() {

        if (!AppTool.IsNullOrEmpty(this.EventTypeCode)) {
            this.CurrentSession.StartBusyIndicator("Please Wait...");
            this._eventTypeExtendedPMService.GetEventTypeByCode(this.EventTypeCode, SessionInfo.LoggedUserTenant).subscribe((res: ServiceResponse) => {
                var pmResponse: ServiceResponse = res;
                this.CurrentSession.StopBusyIndicator();
                if (!pmResponse.HasError) {
                    this.EventType = pmResponse.Result;
                    if (this.EventType) {

                        this.ShowFollowUpControl();
                    }
                }
            });
        }
        else this.CurrentSession.StopBusyIndicator();

    }

    GetNewInStanceFromFollowUp() {


       var newFollowUp: FollowUpPM = new FollowUpPM();
        if (this.EventType && this.ParentViewModel ) {
            if (newFollowUp) {
              
                newFollowUp.DocumentTypeId = this.ParentViewModel.DocumentTypeId;
                newFollowUp.Area =  this.TabName;
                newFollowUp.Notes = this.ParentViewModel.DocumentTypeName;
                newFollowUp.EventTypeId = this.EventType.Id;
                newFollowUp.IsNew = true;
                newFollowUp.Tenant = SessionInfo.LoggedUserTenant;
                newFollowUp.Deleted = false;
                newFollowUp.EventTypeFollowUpName = this.EventType.FollowUpEnglishName;
                newFollowUp.OwnerUserId = SessionInfo.LoggedUserId;
                newFollowUp.Date = DateTool.GetCurrentDateAsUtc();
                newFollowUp.Done = false;
                if (this.ParentViewModel.CurrentDocument && this.TabName == "DocIn") {
                    newFollowUp.ExternalDocumentId = this.ParentViewModel.CurrentDocument.Id;
                }


            }
        }

        return newFollowUp;
    
    } 


    ShowFollowUpControl() {

        var newFollowUp = this.GetNewInStanceFromFollowUp();
        if (newFollowUp) {
            var windowArgs: any = {};
            windowArgs.ObjectTableName = this.ObjectTableName;
            windowArgs.CurrentFollowUp = newFollowUp;
            windowArgs.EntityPM = this.EntityPM;
            var logWindow = new LogitudeWindow();
            logWindow.Width = 350;
            logWindow.Height = 400;
            logWindow.Title = "New Follow Up";
            logWindow.WindowArgs = windowArgs;

            logWindow.Show("./Infrastructure/Components/LogitudeComponents/Followups/AddDocumentFollowupComponent");
            logWindow.WindowClosed.subscribe(($event: any) => {
                if ($event == "AddFollowUpSucceeded") {
                    this.ParentViewModel.HasFollowUp = true;
                }
                else this.ParentViewModel.HasFollowUp = false;

     

            });
        }

    }


}
