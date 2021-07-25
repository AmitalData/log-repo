import {Component, ChangeDetectorRef, Output, EventEmitter} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { CourierMasterPM } from '../../../Customs/EntityPMs/CourierMasterPM';
import { CourierMasterValidator } from '../../../Customs/Validators/CourierMasterValidator';
import { CustomsRequestsSheetPM } from '../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { GenericRequestParams } from '../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { SendRequestVIA } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { DeclarationWebService } from '../../../Customs/Services/WebServices/DeclarationWebService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationAmendmentComponent } from '../../CustomsDeclarationModules/DeclarationTabs/Components/DeclarationAmendment/DeclarationAmendmentComponent';
import { DeclarationPMService } from '../../../Customs/Services/StandardPMs/DeclarationPMService';
 import { DeclarationEventManager } from '../../../Customs/Utilities/DeclarationEventManager';
import { DeclarationAmendmentSharedDataService } from '../../../Customs/Services/DataChange/DeclarationAmendmentSharedDataService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';

@Component({
    templateUrl: './DeclarationAmendmentListTemplate.html',
 })

export class DeclarationAmendmentListTemplate {

    public rowData: DeclarationList;
    public fieldName: any;
    fontcolor: string;
    entityPM: DeclarationPM;
    IsConnectedDeclarationChecked: boolean = true;
    IsNotConnectedDeclarationChecked: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public ObjectTableName: string = "Customs.Declaration";
    filterAgrs: ApiQueryFilters;
    @Output() MenuHeaderchangeevent = new EventEmitter();
   // declarationAmendmentSharedDataService: DeclarationAmendmentSharedDataService = new DeclarationAmendmentSharedDataService();

    public IsDisplayOnly: boolean = false;
    public color: string;
    public allowCancel: boolean;
    public CanOpenNewAmendment: boolean;


    constructor(private CD: ChangeDetectorRef, 
        private _declarationWebService: DeclarationWebService,
        private EntityResourceService: EntityResourceService,
        private comp: DeclarationAmendmentComponent, public declarationAmendmentSharedDataService: DeclarationAmendmentSharedDataService
          ) {
        
    }

    setVariables(DeclarationListRecord: DeclarationList, fieldName: string, additionalData: any )
    {
        this.EntityResourceService.getEntityResourceByTableName("General").subscribe(response => {
             this.rowData = DeclarationListRecord;
            this.fieldName = fieldName;
            this.entityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM as DeclarationPM;
            this.allowCancel = AppTool.IsNullOrEmpty(this.rowData.AmendmentStatus) && this.rowData.IsAmendment == true;
             this.CanOpenNewAmendment = this.declarationAmendmentSharedDataService.CanOpenNewAmendment;

            this.CD.detectChanges();
        });
    }

    Cancel() {
        var declarationPMService: DeclarationPMService = new DeclarationPMService();

        var dec;
        declarationPMService.get(this.rowData.Id).subscribe(
            data => {
                 dec = data.Result;
                dec.AmendmentStatus = "5";
                this.CurrentSession.StartBusyIndicatorSaving();

                declarationPMService.update(dec).subscribe(
                    data => {
                        this.CurrentSession.StopBusyIndicator();
                        DeclarationEventManager.DeclarationAmendmentCancelled.emit(null);
                      });

        });

    
    }

    ChangeAmendment(id: string) {
        this.comp.OpenNewAmendment(id, this.rowData.DeclarationNumber);
 
    }
 

    CopyAmendment(id: string) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show("נא‌ ‌אשר‌ ‌פתיחת‌ ‌תיקון‌ ‌והעתקת‌ ‌נתונים‌ ‌מתיקון‌ ‌הצהרה");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.comp.OpenNewAmendment(id, this.rowData.DeclarationNumber, true);

            }
        });

     }

  

    openNewDeclaration(id: string) {
         SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Customs.Declaration', BackButtonLabel: "תיקוני הצהרה" });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    DeclarationEventManager.DeclarationAmendmentCancelled.emit(null);
                    if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
                        SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = false;
                        DeclarationEventManager.DeclarationAmendmentCancelled.emit(null);
                     }
                });

            });
   }

}
