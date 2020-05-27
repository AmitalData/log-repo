//import { Component, OnInit } from '@angular/core';
//import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
//import { RequestParamsBase } from '../../DataContract/RequestParams/RequestParamsBase';
//import { ResponseDataBase } from '../../DataContract/ResponseData/ResponseDataBase';
//import { AppTool, DateTool } from '../../../Infrastructure/Tools';
//////////////////////////////
//export interface IRequestsSheetMassagingView {
//    //InitMassagingViewModel(InDisplayMode: Boolean, RequestParamsXml: string, ResponseDataXml: string): void;
//    MassageDisplay(InDisplayMode: boolean,RequestParamsXml: string, ResponseDataXml: string): void;
//    DisposeMyState(): void;
//}
//export interface IMassagingViewModel {
//    SimplogWindowWidth: number;
//    SimplogWindowHeight: number;
//    SimplogWindowTitle: string;
//    Dispose(): void;
//}
//export abstract class RequestsSheetMassagingBase
//    <TRequestParams extends RequestParamsBase, TResponseData extends ResponseDataBase>
//    extends BaseComponent 
//    implements OnInit,IRequestsSheetMassagingView {
//    private _RequestParams: TRequestParams;
//    private  _ResponseData: TResponseData;
//    protected  _RequestParamsXml: string;
//    protected _ResponseDataXml: string;
//    constructor() {
//        super();
//    }
//    ngOnInit() {
//    }
//    public MassageDisplay(InDisplayMode: boolean,RequestParamsXml: string, ResponseDataXml: string): void {
//        this.MessageDisplayIsEnable = !InDisplayMode;
//        this._RequestParamsXml = RequestParamsXml;
//        this._ResponseDataXml = ResponseDataXml;
//        if (AppTool.IsNullOrEmpty(this._RequestParamsXml)) {
//            return;
//        }
//        if (AppTool.IsNullOrEmpty(this._ResponseDataXml)) {
//            return;
//        }
//        this.RequestParams = JSON.parse(RequestParamsXml); //XmlGenericUtil<TRequestParams>.DeSerializeObject(_RequestParamsXml);
//        this.ResponseData = JSON.parse(ResponseDataXml); // DeSerializeResponse(_ResponseDataXml);
//        this.OnMassageDisplay();
//    }
//OnMassageDisplayBase(): void{
//}
//    abstract OnMassageDisplay(): void;
//    // public SimplogWindowWidth: number;
//    // public SimplogWindowHeight: number;
//    // public SimplogWindowTitle: string;
//    public MessageDisplayIsEnable: boolean;
//    public get RequestParams() { return this._RequestParams; }
//    public set RequestParams(newValue: TRequestParams) {
//        this._RequestParams = newValue;
//    }
//    public get ResponseData() { return this._ResponseData; }
//    public set ResponseData(newValue: TResponseData) {
//        this._ResponseData = newValue;
//    }
//    DisposeMyState(): void{
//        this._RequestParams=null;
//        this._ResponseData=null;
//    }
//}
//# sourceMappingURL=RequestsSheetMassagingBase.js.map