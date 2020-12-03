declare var window: any;
import { Component, EventEmitter, Output, Input, OnInit, ElementRef, AfterViewInit } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { KeyValuePair } from '../../../CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent';
import { CustomsSettingExtendedListService } from '../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';

@Component({
    selector: 'SendDeclarationTastCaseComponent',
    templateUrl: './SendDeclarationTastCaseComponent.html',
})

export class SendDeclarationTastCaseComponent extends BaseComponent{
    private CurrentSession = SessionLocator.SelectedSession;
    Loaded: boolean;
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.Declaration";
    ScenarioCodes: KeyValuePair[] = [];
    SelectedItemScenario: KeyValuePair;
    ValidationErrorsList: any[] = [];
    private _Param1: string;
    SincroTestCaseDetailList: SincroTestCaseDetail[];
    public get Param1() { return this._Param1; }
    public set Param1(newValue: string) {
        this._Param1 = newValue;
    }


    private _Param2: string;
    public get Param2() { return this._Param2; }
    public set Param2(newValue: string) {
        this._Param2 = newValue;
    }


    constructor() {
        super();
        //CurrentSession.StartBusyIndicator("");
    }
    SetWindowArgs(windowArgs) {

        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService();
        
        myCustomsSettingExtendedListService.GetSincroOption(SessionLocator.Tenant, windowArgs.SincroScreen)
            .subscribe(response => {
                //this.CurrentSession.StopBusyIndicator();
                if (!response.HasError && response.Result != null ) {
                    
                    this.SincroTestCaseDetailList = response.Result.SincroTestCaseDetailList;
                    if (this.SincroTestCaseDetailList != null) {
                        this.SincroTestCaseDetailList.forEach(r => {
                            this.ScenarioCodes.push(new KeyValuePair(r.Code, r.Name));
                        });
                    }
                    
                }
                this.Loaded = true;
            });
    }

    OkButtonClicked() {
        var errors = [];
        if (this._ScenarioCode == null) {
            errors.push("אנא בחר קוד תרחיש");
        }

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
            return;
        }
        let testCaseResult = {
            "ScenarioCode": this._ScenarioCode,
            Params1: this.Param1
        };
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    }

    

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindowEmit("");
    }
    public _ScenarioCode: String;
    ScenarioCodeClicked(evKey) {
        this._ScenarioCode = evKey;
        let detail = this.SincroTestCaseDetailList.filter(r => r.Code == this._ScenarioCode)[0];
        this.Param1 = detail.Param1;
        this.Param2 = detail.Param2;
    }
}

export class SincroTestCaseDetail {
    Code: string
    Name: string
    Entity: string
    IsDCA: boolean
    Param1: string
    Param2: string
}
