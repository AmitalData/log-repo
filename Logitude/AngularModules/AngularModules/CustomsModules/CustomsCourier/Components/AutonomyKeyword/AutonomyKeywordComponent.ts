import { Component, OnInit } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { CustomsAutonomyKeywordPM } from '../../../../Customs/EntityPMs/CustomsAutonomyKeywordPM';
import { CustomsAutonomyKeywordPMService } from '../../../../Customs/Services/StandardPMs/CustomsAutonomyKeywordPMService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CustomsAutonomyKeywordListService } from '../../../../Customs/Services/StandardLists/CustomsAutonomyKeywordListService';



import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { KeyValuePair } from '../CourierWorkSheet/CourierWorksheetComponent';
import { CustomsAutonomyKeywordExtendedPMService } from '../../../../Customs/Services/ExtendedPMs/CustomsAutonomyKeywordExtendedPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { I18NHtmlParser } from '@angular/compiler';
import { ControlsIdCounter } from '../../../../Infrastructure/Utilities/ControlsIdCounter';


@Component({
    templateUrl: './AutonomyKeywordComponent.html',
})

export class AutonomyKeywordComponent
    extends BaseComponent
    implements OnInit {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.CustomsAutonomyKeyword";
    public EntityPM: CustomsAutonomyKeywordPM;
    isWindowMode: boolean = false;

    isNewRecord: boolean = false;
    ValidationErrorsList: any[] = [];
    private _EntityResourceService: EntityResourceService = new EntityResourceService();

    _CustomsAutonomyKeywordPMService: CustomsAutonomyKeywordPMService = new CustomsAutonomyKeywordPMService();
    private _CustomsAutonomyKeywordListService: CustomsAutonomyKeywordListService = new CustomsAutonomyKeywordListService();
    _KeywordtypeCodes: KeyValuePair[] = [];
    _SelectKeywordtypeCode_Key: string;
    Loaded: boolean = false;
    FromList: boolean;
    SelectedItemKeywordtypeCode: KeyValuePair;

    constructor(public entityArgs: EntityArgs) {
        super();
        this._KeywordtypeCodes.push(new KeyValuePair("1", "עיר"));
        this._KeywordtypeCodes.push(new KeyValuePair("2", "טלפון"));
        SessionLocator.SelectedSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this.Loaded = true;
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.WarningMessage = "יש לבחור קוד מילת מפתח ולאחר להזין  מילת מפתח ";
            this.Listen();
        });
        
    }
    private Listen() {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {

            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
                SessionLocator.SelectedSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess && SessionLocator.SelectedSession.CurrentEditComponent) {
                        this.EntityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
        }
    }
    ngOnInit(): void {
            if(this.entityArgs.EntityPM != null) {
            this.EntityPM = this.entityArgs.EntityPM;
            if (this.EntityPM.KeywordtypeCode == "1") {
                this.SelectedItemKeywordtypeCode = this._KeywordtypeCodes[0];
            } else {
                this.SelectedItemKeywordtypeCode = this._KeywordtypeCodes[1];
            }
        } else {
            this.isNewRecord = true;
            this.EntityPM = new CustomsAutonomyKeywordPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
                this.EntityPM.MarkAsDirty();
                this.isWindowMode = true;
        }
    }

    SetWindowArgs(args: any) {
    }


    //#region Properties
    private _WarningMessage: string;
    public get WarningMessage() { return this._WarningMessage; }
    public set WarningMessage(newValue: string) {
        this._WarningMessage = newValue;
    }

    public get KeywordtypeCode() {
        if (this.EntityPM == null) {
            return "";
        }
        return this.EntityPM.KeywordtypeCode;
    }
    public set KeywordtypeCode(newValue: string) { this.EntityPM.KeywordtypeCode = newValue; }

    public get KeywordsList() { return this.EntityPM.KeywordsList; }
    public set KeywordsList(newValue: string) {
        this.EntityPM.KeywordsList = newValue;
    }




    _SelectedKeywordtypeCode: String;
    KeywordtypeCodeClicked(SelectKeywordtypeCode_Key) {
        this._SelectKeywordtypeCode_Key = SelectKeywordtypeCode_Key;
        this.EntityPM.KeywordtypeCode = this._SelectKeywordtypeCode_Key;
    }

    OkButtonClicked() {
        var errors = [];
        if (this.EntityPM == null) {
            errors.push("אנא בחר קוד מילות מפתח");
        } else {
            
        }
        if (this.isNewRecord) {
            
        }

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            if (this.isNewRecord) {
                this._CustomsAutonomyKeywordPMService.insert(this.EntityPM).subscribe(myResult => {
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                });
            }
            else {
                this._CustomsAutonomyKeywordPMService.update(this.EntityPM).subscribe(myResult => {
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                });
            }
        }
    }

    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }


}
