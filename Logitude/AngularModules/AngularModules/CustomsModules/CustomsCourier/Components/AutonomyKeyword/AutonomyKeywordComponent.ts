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


@Component({
    moduleId: module.id,
    templateUrl: './AutonomyKeywordComponent.html',
})

export class AutonomyKeywordComponent
    extends BaseComponent
    implements OnInit{

    public DataContext: any = this;
    public ObjectTableName: string = "Customs.CustomsAutonomyKeyword";
    public EntityPM: CustomsAutonomyKeywordPM;
    
    isNewRecord: boolean = false;
    ValidationErrorsList: any[] = [];
    private _EntityResourceService: EntityResourceService = new EntityResourceService();

    _CustomsAutonomyKeywordPMService: CustomsAutonomyKeywordPMService = new CustomsAutonomyKeywordPMService();
    _CustomsAutonomyKeywordExtendedPMService: CustomsAutonomyKeywordExtendedPMService = new CustomsAutonomyKeywordExtendedPMService();
    private _CustomsAutonomyKeywordListService: CustomsAutonomyKeywordListService = new CustomsAutonomyKeywordListService();
    _KeywordtypeCodes: KeyValuePair[] = [];
    _SelectKeywordtypeCode_Key: string;
    Loaded: boolean = false;
    constructor(public entityArgs: EntityArgs ) {
        super();
        
        this._KeywordtypeCodes.push(new KeyValuePair("1", "עיר"));
        this._KeywordtypeCodes.push(new KeyValuePair("2", "טלפון"));
        this.UIProperties.SetEnabled("KeywordsList", this.ObjectTableName, false);
        SessionLocator.SelectedSession.StartBusyIndicator("");
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this.Loaded = true;
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.WarningMessage = "יש לבחור קוד מילות מפתחיש לבחור קוד מילות מפתח ולאחר להזין רשימת מילות מפתח מופרדות";
        });
    }

    
    ngOnInit() {
        //SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        //this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
        //    SessionLocator.SelectedSession.StopBusyIndicator();
        //    this.Loaded = true;
        //});
    }

    SetWindowArgs(args: any) {
        //if (!AppTool.IsNullOrEmpty(args)) {
        //    this.isWindowMode = true;
        //    this.isNewRecord = true;
        //    //this.EntityPM = new CustomsAutonomyKeywordPM();
        //    //this.EntityPM.Tenant = SessionLocator.Tenant;

        //}
    }


    //#region Properties
    private _WarningMessage: string;
    public get WarningMessage() { return this._WarningMessage; }
    public set WarningMessage(newValue: string) {
        this._WarningMessage = newValue;
    }



    public get KeywordsList() {
        if (this.EntityPM == null) {
            return "";
        }
        return this.EntityPM.KeywordsList;
    }
    public set KeywordsList(newValue: string) {
        this.EntityPM.KeywordsList = newValue;
    }


    _SelectedKeywordtypeCode: String;
    KeywordtypeCodeClicked(SelectKeywordtypeCode_Key) {
        this._SelectKeywordtypeCode_Key = SelectKeywordtypeCode_Key;
        SessionLocator.SelectedSession.StartBusyIndicator("");
        this._CustomsAutonomyKeywordExtendedPMService.getByKeywordtypeCode(SelectKeywordtypeCode_Key, SessionLocator.Tenant)
            .subscribe((serviceResponse: ServiceResponse) => {

                SessionLocator.SelectedSession.StopBusyIndicator();
                this.EntityPM = serviceResponse.Result as CustomsAutonomyKeywordPM;
                if (this.EntityPM == null) {
                    this.isNewRecord = true;
                    this.EntityPM = new CustomsAutonomyKeywordPM();
                    this.EntityPM.Id = "new";
                    this.EntityPM.Tenant = SessionLocator.Tenant;
                    this.EntityPM.MarkAsDirty();
                } else {
                    this.isNewRecord = false;
                }
                
                this.EntityPM.KeywordtypeCode = this._SelectKeywordtypeCode_Key;
                this.UIProperties.SetEnabled("KeywordsList", this.ObjectTableName, true);


            });
    }


    //#endregion\

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
