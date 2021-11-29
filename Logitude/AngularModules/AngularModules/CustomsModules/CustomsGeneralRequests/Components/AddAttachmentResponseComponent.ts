import { Component, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DeclarationRestoreArgs } from '../../../Customs/Args';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';
import { IIGGeneralMessagesService } from '../../../Customs/Services/WebServices/IIGGeneralMessagesService';

import { CustomSendOptionsArgs } from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { AddAttachmentResponseData } from '../../../Customs/DataContract/ResponseData/AddAttachmentResponseData';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { BaseRequestsSheetMassaging, IRequestsSheetMassagingComponent } from '../../../CustomsModules/CustomsRequests/Components/BaseRequestsSheetMassaging';
import { CustomMessageProgressComponent } from '../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';


import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
@Component({
    selector: 'AddAttachmentResponseComponent',
    
    templateUrl: './AddAttachmentResponseComponent.html',
    styles: [
        `        
            :host ::ng-deep input{
                color: black;
            }

            :host ::ng-deep LogLabel  label {
                color: black !important;
            }
        `
    ]
})


export class AddAttachmentResponseComponent
    extends BaseRequestsSheetMassaging
    implements AfterViewInit,IRequestsSheetMassagingComponent, OnInit {
    public DataContext: AddAttachmentResponseComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();
    ///public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        
    }

    ngOnInit() {
        ///alert("AddAttachmentResponseComponent:ngOnInit")
        //super.ngOnInit();
    }

    @ViewChild(CustomMessageWrapperComponent)
    SuperCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    ngAfterViewInit() {
        if (this.SuperCustomMessageWrapperComponent == null) {
            console.warn("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("SuperCustomMessageWrapperComponent.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.MyCustomMessageWrapperComponent = this.SuperCustomMessageWrapperComponent;
        this.subscribeWrapperComponent()
    }

    OnMassageDisplayMethod() {
        if (this.RequestParams == null) {
            
        }

        if (this.ResponseData ) {

        } else {
            ///console.error("this.ResponseData == null !?!?!?")
        }

        let tst = false;
        if (tst) {
            let rsp: AddAttachmentResponseData = this.ResponseData;
            rsp.HasException = true;;
            rsp.DocumentNumber = "DocumentNumber";
            rsp.ErrorCode = "ErrorCode";
            rsp.ErrorRemarks = `ErrorRemarks
ErrorRemarks
ErrorRemarks`;

        }

    }                                                      
   
    get DocumentNumber() { return this.ResponseData ? this.ResponseData.DocumentNumber : null; }
    set DocumentNumber(value: string) {
        if (this.ResponseData.DocumentNumber != value) {
            this.ResponseData.DocumentNumber = value;
        }
    }

    get CustomDocument() { return this.ResponseData ? this.ResponseData.CustomDocument : null; }
    set CustomDocument(value: string) {
        if (this.ResponseData.CustomDocument != value) {
            this.ResponseData.CustomDocument = value;
        }
    }

    get CustomRecievedDate() { return this.ResponseData ? this.ResponseData.CustomRecievedDate : null; }
    set CustomRecievedDate(value: string) {
        if (this.ResponseData.CustomRecievedDate != value) {
            this.ResponseData.CustomRecievedDate = value;
        }
    }

    get Remarks() { return this.ResponseData ? this.ResponseData.Remarks : null; }
    set Remarks(value: string) {
        if (this.ResponseData.Remarks != value) {
            this.ResponseData.Remarks = value;
        }
    }

    get ErrorCode() { return this.ResponseData ? this.ResponseData.ErrorCode : null; }
    set ErrorCode(value: string) {
        if (this.ResponseData.ErrorCode != value) {
            this.ResponseData.ErrorCode = value;
        }
    }


    get ErrorRemarks() { return this.ResponseData ? this.ResponseData.ErrorRemarks : null; }
    set ErrorRemarks(value: string) {
        if (this.ResponseData.ErrorRemarks!= value) {
            this.ResponseData.ErrorRemarks= value;
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs) { }

 

}        
