declare var window: any;
import { Component, EventEmitter, Output, Input, OnInit, ElementRef, AfterViewInit } from '@angular/core';
import { ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { KeyValuePair } from '../../../CustomsCourier/Components/CourierWorkSheet/CourierWorksheetComponent';





@Component({
    selector: 'SendDeclarationTastCaseComponent',
    moduleId: module.id,
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
        this.ScenarioCodes.push(new KeyValuePair("1", "עיר"));
        this.ScenarioCodes.push(new KeyValuePair("2", "טלפון"));
    }
    SetWindowArgs(windowArgs) {
        this.Loaded = true;
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
    }
}
