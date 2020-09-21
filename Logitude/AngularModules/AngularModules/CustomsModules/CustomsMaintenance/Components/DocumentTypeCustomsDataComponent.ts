declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from       '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from  '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';

import { ApiQueryFilters } from  '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from  '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from   '../../../Infrastructure/Services/EntityListService';





import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { DocumentTypeCustomsDataPM } from '../../../Customs/EntityPMs/DocumentTypeCustomsDataPM';
import { DocumentTypeCustomsDataList } from '../../../Customs/EntityLists/DocumentTypeCustomsDataList';

//C: \LW\Customs\AngularModules\AngularModules\Customs\Services\StandardPMs\DocumentTypeCustomsDataPMService.ts
import { DocumentTypeCustomsDataPMService } from '../../../Customs/Services/StandardPMs/DocumentTypeCustomsDataPMService';
import { DocumentTypeCustomsDataListService } from '../../../Customs/Services/StandardLists/DocumentTypeCustomsDataListService';
import { DocumentTypeCustomsDataExtendPMService } from '../../../Customs/Services/ExtendedPMs/DocumentTypeCustomsDataExtendPMService';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import { DocumentTypePM } from '../../../Common/EntityPMs/DocumentTypePM';


@Component({
    
    templateUrl: './DocumentTypeCustomsDataComponent.html',
})





export class DocumentTypeCustomsDataComponent
    extends BaseComponent
    implements OnInit {

    public DataContext: DocumentTypeCustomsDataComponent = this;
    public ObjectTableName: string = "Customs.DocumentTypeCustomsData";
    public columns: any[] = null;



    private _DocumentTypeCustomsDataPMService: DocumentTypeCustomsDataPMService = new DocumentTypeCustomsDataPMService();
    private _DocumentTypeCustomsDataListService: DocumentTypeCustomsDataListService = new DocumentTypeCustomsDataListService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    

    
    entityPM: DocumentTypeCustomsDataPM;

    ValidationErrorsList: string[] = [];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
    }
    Loaded: boolean = false;
    EntityResource: boolean = false;
    fromLog: boolean = false;
    ngOnInit() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
          var   docType: DocumentTypePM = this.entityArgs.EntityPM;
            this.DocumentTypeId = docType.Code;
            this.UnifaceNAME_HEB = docType.Name;
            this._DocumentTypeCustomsDataPMService
                .get(this.DocumentTypeId)
                .subscribe((res: any) => {
                    this.entityPM = res.Result;
                    if (this.entityPM == null) {
                        this.IsNew = true;
                        this.entityPM = new DocumentTypeCustomsDataPM();
                        this.entityPM.DocumentTypeId = this.DocumentTypeId;
                        this.entityPM.Tenant = SessionLocator.Tenant;
                       

                    }
                    this.fromLog = true;

                    if (this.entityPM) {
                        //  this.DocumentTypeId = this.entityPM.;
                        //  this.UnifaceNAME_HEB = this.entityPM.u;
                        this.Loaded = true;
                    }
                    this.EntityResource = true;
                    if (this.EntityResource && this.Loaded) {
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
         
            //this.RefreshBtnClick()
        });

    }
    DocumentTypeId: string;
    UnifaceNAME_HEB: string;
    
    SetWindowArgs(arg) {
        this.DocumentTypeId = arg.UnifaceDOC_ID;
        this.UnifaceNAME_HEB = arg.UnifaceNAME_HEB;
        
        this._DocumentTypeCustomsDataPMService
            .get(this.DocumentTypeId)
            .subscribe((res:any) => {
                this.entityPM = res.Result;
                if (this.entityPM == null) {
                    this.IsNew = true;
                    this.entityPM = new DocumentTypeCustomsDataPM();
                    this.entityPM.DocumentTypeId = this.DocumentTypeId;
                    this.entityPM.Tenant = SessionLocator.Tenant;

                    
                }
                this.CurrentSession.StopBusyIndicator();
                this.Loaded = true;
                if (this.EntityResource && this.Loaded) {
                    this.CurrentSession.StopBusyIndicator();
                }
            });
    }
    ///#region Properties

    //IsUnifreightCertificateActivatedEnabled: boolean = true;


    get CustomsDoucumentTypeCode() { return this.entityPM.CustomsDoucumentTypeCode }
    set CustomsDoucumentTypeCode(value: string) { this.entityPM.CustomsDoucumentTypeCode = value; }
                             

    

         

    //#endregion

    DeleteRow() {
        var documentTypeCustomsDataExtendPMService: DocumentTypeCustomsDataExtendPMService = new DocumentTypeCustomsDataExtendPMService();
        documentTypeCustomsDataExtendPMService.DeleteRecord(this.entityPM.DocumentTypeId)
            .subscribe((resp:any) => {
                if (resp.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                    return;
                }
                this.CancelButtonClicked();
            });
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
        IsNew: boolean = false;//itzik : there is a row that come with defualt DB !!!
    OkButtonClicked() {
        
        if (this.IsNew) {
            this._DocumentTypeCustomsDataPMService.insert(this.entityPM)
                .subscribe((resp:any) => {
                    if (resp.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                    if (this.fromLog)  this.IsNew = false;
                });
        } else {
            this._DocumentTypeCustomsDataPMService.update(this.entityPM)
                .subscribe((resp:any) => {
                    if (resp.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();

                });
        }
    }
}
