import {Component, ChangeDetectorRef} from '@angular/core';
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

@Component({
    moduleId: module.id,
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

    public IsDisplayOnly: boolean = false;
    public color: string;
    constructor(private CD: ChangeDetectorRef, private _declarationWebService: DeclarationWebService) {
        
    }

    setVariables(DeclarationListRecord: DeclarationList, fieldName: string, additionalData: any)
    {
          this.rowData = DeclarationListRecord;
        this.fieldName = fieldName;
        this.entityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM as DeclarationPM;
          this.CD.detectChanges();
    }
 

    ChangeAmendment(id:string) {
        this.OpenNewAmendment(id);
    }


    public OpenNewAmendment(id:string) {


        var searchParams: GenericRequestParams = new GenericRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = id;
        searchParams.LoggingEntityReference = this.rowData.DeclarationNumber;
        searchParams.LoggingObjectTableId = this.ObjectTableName;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
        searchParams.RequestName = "Declaration Request";
        searchParams.ResponseName = "Declaration Response";
        searchParams.RequestVIA = SendRequestVIA.DCABatch;
        searchParams.ForcePersonalSign = false;
         this._declarationWebService
            .GetNewAmendmentDeclaration(searchParams)
            .subscribe((response: any) => {

                if (response) {
                    if (!response.HasError) {
                        var entity = response.Result;
                        if (entity != null) {
                            // this.LoadDeclarationAmendmentsList();
                            //setTimeout(() => {
                            //    this.MenuHeaderchangeevent.emit({ Filters: this.filterAgrs, IgnoreFilter: false });
                            //}, 10);
                            this.CurrentSession.StopBusyIndicator();
                             this.openNewDeclaration(entity.Id);

                        }

                    }
                }
            });


    }


    openNewDeclaration(id: string) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Customs.Declaration', BackButtonLabel: TextCodeTranslator.Translate("General.MH.Declaration") });
                cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                    if (SessionLocator.SelectedSession != null && SessionLocator.SelectedSession.CurrentWindow != null) {
                        SessionLocator.SelectedSession.CurrentWindow.SuppressBusyIndicator = false;
                    }
                });

            });
    }

}
