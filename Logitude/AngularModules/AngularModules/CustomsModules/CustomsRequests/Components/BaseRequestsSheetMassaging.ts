import { Component, EventEmitter, Output, Input, OnInit, ViewChild, AfterViewInit, AfterContentInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';

import { RequestParamsBase, CustomSendOptionsArgs} from '../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ResponseDataBase } from                    '../../../Customs/DataContract/ResponseData/ResponseDataBase';
import { CustomsMenuItem, RequestSheetState } from  '../../../Customs/DataContract/CustomsMenuItem';
import { CustomMessageWrapperComponent} from '../../../CustomsModules/CustomsControls/Components/CustomMessageWrapperComponent'

export interface IRequestsSheetMassagingComponent {
    OnMassageDisplayMethod();
    ///ValidationErrorsList: string[];
    OnCustomSendOptionsButtonClick(customSendOptionsArgs: CustomSendOptionsArgs);
}


export class BaseRequestsSheetMassaging
    extends BaseComponent
    implements OnInit, AfterContentInit, AfterViewInit
//, AfterContentInit, AfterViewInit, IRequestsSheetMassagingView 
{




    private _RequestParams: RequestParamsBase;
    private _ResponseData: ResponseDataBase;
    protected _RequestParamsXml: string;
    protected _ResponseDataXml: string;

    _MyCustomMessageWrapperComponent: CustomMessageWrapperComponent = new CustomMessageWrapperComponent();
    @ViewChild(CustomMessageWrapperComponent)
    public get MyCustomMessageWrapperComponent() { return this._MyCustomMessageWrapperComponent; }
    public set MyCustomMessageWrapperComponent(val: CustomMessageWrapperComponent) {
        console.log("MyCustomMessageWrapperComponent is settt!!!!");
        this._MyCustomMessageWrapperComponent = val;
    }
    //private _callBackOnMassageDisplay: () => void;

    constructor() {
        super();
        //this._callBackOnMassageDisplay = callBackOnMassageDisplay;
        this.CallOnMassageDisplayMethod();
    }

    ngOnInit() {
        //alert("BaseRequestsSheetMassaging:ngOnInit")
        //this.CallOnMassageDisplayMethod();
        ///alert(this.MyCustomMessageWrapperComponent);
     

    }

    ngAfterContentInit() {
        if (this._MyCustomMessageWrapperComponent == null) {
            console.warn("BaseRequestsSheetMassaging.ngAfterContentInit this.MyCustomMessageWrapperComponent == null");
        }
        else {
            console.log("BaseRequestsSheetMassaging.ngAfterContentInit this.MyCustomMessageWrapperComponent != null");
        }
        //this.subscribeWrapperComponent()
    }

    ngAfterViewInit() {
        if (this._MyCustomMessageWrapperComponent == null) {
            console.warn("BaseRequestsSheetMassaging.ngAfterViewInit MyCustomMessageWrapperComponent == null");
        } else {
            console.log("BaseRequestsSheetMassaging.ngAfterViewInit MyCustomMessageWrapperComponent != null");
        }
        this.subscribeWrapperComponent()
    }

    subscribeWrapperComponent() {
        this._MyCustomMessageWrapperComponent.MyCustomSendOptionsComponent
            .SendButtonClicked.subscribe(
            (myCustomSendOptionsArgs: CustomSendOptionsArgs) => {
                this.CallOnCustomSendOptionsButtonClick(myCustomSendOptionsArgs);
            }
            );
    }
    private CallOnCustomSendOptionsButtonClick(myCustomSendOptionsArgs: CustomSendOptionsArgs) {
        var myIRequestsSheetMassagingComponent: IRequestsSheetMassagingComponent = this as any;
        if (myIRequestsSheetMassagingComponent) {
            myIRequestsSheetMassagingComponent.OnCustomSendOptionsButtonClick(myCustomSendOptionsArgs);
            this.OnMassageDisplayBase()
        } else {
            console.warn("Please implements IRequestsSheetMassagingComponent");
        }
    }

    public MassageDisplay(RequestParamsXml: string, ResponseDataXml: string): void {


        this._RequestParamsXml = RequestParamsXml;
        this._ResponseDataXml = ResponseDataXml;
        if (AppTool.IsNullOrEmpty(this._RequestParamsXml)) {
            return;
        }
        if (AppTool.IsNullOrEmpty(this._ResponseDataXml)) {
            return;
        }
        this.RequestParams = JSON.parse(RequestParamsXml); //XmlGenericUtil<TRequestParams>.DeSerializeObject(_RequestParamsXml);
        this.ResponseData = JSON.parse(ResponseDataXml); // DeSerializeResponse(_ResponseDataXml);

        //this.OnMassageDisplay.emit();
        this.CallOnMassageDisplayMethod();


    }
    private CallOnMassageDisplayMethod()
    {
        console.log("CallOnMassageDisplayMethod()");
        var myIRequestsSheetMassagingComponent: IRequestsSheetMassagingComponent = this as any;
        if (myIRequestsSheetMassagingComponent) {
            myIRequestsSheetMassagingComponent.OnMassageDisplayMethod();
            this.OnMassageDisplayBase()
        } else {
            console.warn("Please implements IRequestsSheetMassagingComponent");
        }
    }

    private OnMassageDisplayBase() {

    }

    public get CustomResponseContentIsDisable() { return this._MyCustomMessageWrapperComponent.CustomResponseContentIsDisable;}
    public set CustomResponseContentIsDisable(val: boolean) {
        this._MyCustomMessageWrapperComponent.CustomResponseContentIsDisable= val;
    }

    public get CustomRequestContentIsDisable() { return this._MyCustomMessageWrapperComponent.CustomRequestContentIsDisable; }
    public set CustomRequestContentIsDisable(val: boolean) {
        this._MyCustomMessageWrapperComponent.CustomRequestContentIsDisable= val;
    }
    
    

    public get CustomSendOptionsButtonIsDisable() { return this._MyCustomMessageWrapperComponent.CustomSendOptionsButtonIsDisable; }
    public set CustomSendOptionsButtonIsDisable(val: boolean) {
        this._MyCustomMessageWrapperComponent.CustomSendOptionsButtonIsDisable= val;
    }
    
    public get ValidationErrorsList() { return this._MyCustomMessageWrapperComponent.ValidationErrorsList; }
    public set ValidationErrorsList(val: string[]) {
        this._MyCustomMessageWrapperComponent.ValidationErrorsList= val;
    }

    public get CustomSendOptionsButtonCanForcePersonalSign() { return this._MyCustomMessageWrapperComponent.CustomSendOptionsButtonCanForcePersonalSign; }
    public set CustomSendOptionsButtonCanForcePersonalSign(newValue: boolean) {
        this._MyCustomMessageWrapperComponent.CustomSendOptionsButtonCanForcePersonalSign= newValue;;
    }

    public get IsShowCustomResponseContent() { return this._MyCustomMessageWrapperComponent.IsShowCustomResponseContent; }
    public set IsShowCustomResponseContent(newValue: boolean) {
        if (this._MyCustomMessageWrapperComponent.IsShowCustomResponseContent != newValue) {
            this._MyCustomMessageWrapperComponent.IsShowCustomResponseContent = newValue;
        }
    }

    public get MyCustomsMenuItem() { return this._MyCustomMessageWrapperComponent.MyCustomsMenuItem; }
    public set MyCustomsMenuItem(newValue: CustomsMenuItem) {
        if (this._MyCustomMessageWrapperComponent.MyCustomsMenuItem != newValue) {
            this._MyCustomMessageWrapperComponent.MyCustomsMenuItem = newValue;
        }
    }

    public get MyCommunicationLogId() { return this._MyCustomMessageWrapperComponent.MyCommunicationLogId; }
    public set MyCommunicationLogId(newValue: string) {
        if (this._MyCustomMessageWrapperComponent.MyCommunicationLogId != newValue) {
            this._MyCustomMessageWrapperComponent.MyCommunicationLogId = newValue;
        }
    }
    

    public get MyLastCustomsRequestSheetId() { return this._MyCustomMessageWrapperComponent.MyLastCustomsRequestSheetId; }
    public set MyLastCustomsRequestSheetId(newValue: string) {
        if (this._MyCustomMessageWrapperComponent.MyLastCustomsRequestSheetId!= newValue) {
            this._MyCustomMessageWrapperComponent.MyLastCustomsRequestSheetId= newValue;
        }
    }

    public get RequestParams() { return this._RequestParams; }
    public set RequestParams(newValue: any) {
        this._RequestParams = newValue;;

    }
    
    public get ResponseData() { return this._ResponseData; }
    public set ResponseData(newValue: any) {
        this._ResponseData = newValue;

    }
    DisposeMyState(): void {
        this._RequestParams = null;
        this._ResponseData = null;

    }

    

    
    


}
