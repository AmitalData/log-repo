
import { Component, EventEmitter, Output, Input, OnInit, ViewChild, AfterViewInit, AfterContentInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import {ServiceHelper} from       '../../../Infrastructure/Utilities/ServiceHelper';
import { RequestParamsBase, CustomSendOptionsArgs} from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ResponseDataBase } from                    '../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { CustomsMenuItem, RequestSheetState } from  '../../../Customs/DataContract/CustomsMenuItem';
import { CustomSendOptionsComponent } from './CustomSendOptionsComponent'
//import { CustomSendOptionsComponent } from './CustomSendOptionsComponent'

import { CommunicationLogStepListService } from '../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
import { Guid } from '../../../Infrastructure/Utilities/Guid';



export interface IMassagingViewModel {

    SimplogWindowWidth: number;
    SimplogWindowHeight: number;
    SimplogWindowTitle: string;
    Dispose(): void;
}
@Component({
    selector: 'custom-message-wrapper',
    
    templateUrl: '././CustomMessageWrapperComponent.html',
})

export class CustomMessageWrapperComponent
    extends BaseComponent
//implements OnInit, AfterContentInit, AfterViewInit,IRequestsSheetMassagingView 
    implements AfterContentInit
{
    
    private _ValidationErrorsList: string[] = [];
    public MyGuid: string;
    @Input() get ValidationErrorsList() { return this._ValidationErrorsList; }
    set ValidationErrorsList(newValue: string[]) {
        this._ValidationErrorsList = newValue;

    }
    @Input()
    public CustomSendOptionsButtonCanForcePersonalSign: boolean = false;

    @Input()
    get CustomSendOptionsButtonAvoidDoubleClick() { return this.MyCustomSendOptionsComponent ? this.MyCustomSendOptionsComponent.AvoidDoubleClick : null; }
    set CustomSendOptionsButtonAvoidDoubleClick(newValue: boolean) {
        this.MyCustomSendOptionsComponent.AvoidDoubleClick = newValue;
    }

    @Input()
    public IsShowCustomResponseContent: boolean = true;
    @Input()
    public IsShowCustomToolBar: boolean = true;

    //@Input()
    //public CanExportExcel: boolean = false;
    
    
    get CustomSendOptionsButtonIsDisable() { return this.MyCustomSendOptionsComponent ? this.MyCustomSendOptionsComponent.IsDisabled : null; }
    set CustomSendOptionsButtonIsDisable(newValue: boolean) {
        this.MyCustomSendOptionsComponent.IsDisabled= newValue;
    }

    @Input()
    public  CustomRequestContentIsDisable: boolean;
    
    public  CustomResponseContentIsDisable: boolean;


    private _RequestParams: RequestParamsBase;
    private _ResponseData: ResponseDataBase;
    protected _RequestParamsXml: string;
    protected _ResponseDataXml: string;
    

    //@Output()
    //private CustomSendOptionsClick: EventEmitter<CustomSendOptionsArgs> = new EventEmitter<CustomSendOptionsArgs>();
    
    
    

    @ViewChild(CustomSendOptionsComponent)
    public MyCustomSendOptionsComponent: CustomSendOptionsComponent = new CustomSendOptionsComponent(null,null);
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.MyGuid = Guid.newGuid();
        console.log("CustomMessageWrapperComponent:" + this.MyGuid);
    }
    _AfterContentInit: boolean = false;
    ngAfterContentInit() {
        //////alert(this.MyCustomSendOptionsComponent);
        this._AfterContentInit = true;
    }
    public get FormTitle() {
        if (this.CurrentSession.CurrentWindow) {
            return this.CurrentSession.CurrentWindow.Title;
        } else {
            return "";
        }
    }


    private _IsDisableToggle: boolean = false;
    MessageDisplayIsDisableToggle() {
        this._IsDisableToggle = !this._IsDisableToggle;

        this.CustomRequestContentIsDisable = this.CustomResponseContentIsDisable = this.CustomSendOptionsButtonIsDisable = this._IsDisableToggle;

    }
    
    MyLastCustomsRequestSheetId: string = null;
    ExportExcel() {
        //this._IsDisableToggle = !this._IsDisableToggle;
        console.log(this.MyLastCustomsRequestSheetId);
        var communicationLogStepListService: CommunicationLogStepListService = new CommunicationLogStepListService();
        var mainInterfaceCode: string, RequestId: string, tenant: number

        //communicationLogStepListService.GetExportExcelByRequestId("8305", this.MyLastCustomsRequestSheetId, SessionLocator.Tenant);
        //http://localhost:9996/api/CommunicationLogStep/GetExportExcelByRequestId/?mainInterfaceCode=8305&requestId=3333&tenant=2
        var url = "";
        if (!AppTool.IsNullOrEmpty(this.MyCommunicationLogId)) {
            url = ServiceHelper.GetLogitudeURL() + 'api/CommunicationLogStep/GetExportExcelByLogId/?mainInterfaceCode=' + this.MyCustomsMenuItem.MainInterfaceCode + '&logId=' + this.MyCommunicationLogId + '&tenant=' + SessionLocator.Tenant.toString();
        }
        if (!AppTool.IsNullOrEmpty(this.MyLastCustomsRequestSheetId)) {
            url = ServiceHelper.GetLogitudeURL() + 'api/CommunicationLogStep/GetExportExcelByRequestId/?mainInterfaceCode=' + this.MyCustomsMenuItem.MainInterfaceCode + '&requestId=' + this.MyLastCustomsRequestSheetId + '&tenant=' + SessionLocator.Tenant.toString();
        }


        window.open(url);

    }
    

    public get RequestParams() { return this._RequestParams; }
    public set RequestParams(newValue: any) {
        this._RequestParams = newValue;;
        
    }


    public get ResponseData() { return this._ResponseData; }
    public set ResponseData(newValue: any) {
        this._ResponseData = newValue;

    }

    public get ShowCustomResponseContent() { return this.IsShowCustomResponseContent; }
    public set ShowCustomResponseContent(newValue: boolean) {
        if (this.IsShowCustomResponseContent != newValue) {
            this.IsShowCustomResponseContent = newValue;
        }
    }

    public MyCustomsMenuItem: CustomsMenuItem = null;
    public MyCommunicationLogId: string = null;

    public get ShowCustomToolBar() { return this.IsShowCustomToolBar; }
    public set ShowCustomToolBar(newValue: boolean) {
        if (this.IsShowCustomToolBar!= newValue) {
            this.IsShowCustomToolBar= newValue;
        }
    }
    

    DisposeMyState(): void {
        this._RequestParams = null;
        this._ResponseData = null;
        this.MyCustomSendOptionsComponent = null;
    }

    

    CancelButtonClickedBase() {
        this.CurrentSession.CloseCurrentWindow();
    }
   

    //OnCustomSendOptionsClick(customSendOptionsArgs: CustomSendOptionsArgs) {
    //    this.CustomSendOptionsClick.emit(customSendOptionsArgs);
    //}

}
