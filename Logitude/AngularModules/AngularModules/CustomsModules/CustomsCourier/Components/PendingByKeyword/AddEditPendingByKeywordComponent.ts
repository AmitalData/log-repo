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
import { PendingByKeywordPM } from '../../../../Customs/EntityPMs/PendingByKeywordPM';
import { PendingByKeywordPMService } from '../../../../Customs/Services/StandardPMs/PendingByKeywordPMService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { PendingByKeywordListService } from '../../../../Customs/Services/StandardLists/PendingByKeywordListService';



import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { KeyValuePair } from '../CourierWorkSheet/CourierWorksheetComponent';
import { CourierPendingReasonExtendedListService } from 'Customs/Services/ExtendedLists/CourierPendingReasonExtendedListService';
import { CourierPendingReasonList } from 'Customs/EntityLists/CourierPendingReasonList';


@Component({
    templateUrl: './AddEditPendingByKeywordComponent.html',
})

export class AddEditPendingByKeywordComponent
    extends BaseComponent
    implements OnInit{
    SearchByFieldCodes: KeyValuePair[] = [];
    SearchTypes: KeyValuePair[] = [];
    SelectedItemSearchByField: KeyValuePair;
    SelectedItemSearchType: KeyValuePair;


    public DataContext: any = this;
    public ObjectTableName: string = "Customs.PendingByKeyword";
    public EntityPM: PendingByKeywordPM;
    isWindowMode: boolean = false;
    isNewRecord: boolean = false;
    ValidationErrorsList: any[] = [];
    private _EntityResourceService: EntityResourceService = new EntityResourceService();

    _PendingByKeywordPMService: PendingByKeywordPMService = new PendingByKeywordPMService();
    private _PendingByKeywordListService: PendingByKeywordListService = new PendingByKeywordListService();

    constructor(public entityArgs: EntityArgs) {
        super();

        this.SearchByFieldCodes.push(new KeyValuePair("1", "תאור טובין"));
        this.SearchByFieldCodes.push(new KeyValuePair("2", "שם יבואן"));

        this.SearchTypes.push(new KeyValuePair("1", "מילה"));
        this.SearchTypes.push(new KeyValuePair("2", "חלק ממילה"));

        SessionLocator.SelectedSession.StartBusyIndicator("");
        
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            
            SessionLocator.SelectedSession.StopBusyIndicator();
            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
                this.EntityPM = new PendingByKeywordPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.isWindowMode = true;
                this.isNewRecord = true;
                this.UIProperties.SetRequired("SearchByFieldCode", this.ObjectTableName, true);
                this.UIProperties.SetRequired("SearchType", this.ObjectTableName, true);
                
            } else {
                this.EntityPM = this.entityArgs.EntityPM;
                this.SelectedItemSearchByField = this.SearchByFieldCodes.filter(r => r.Key == this.EntityPM.SearchByFieldCode)[0];
                this.UIProperties.SetRequired("SearchByFieldCode", this.ObjectTableName, false);
                this.SelectedItemSearchType = this.SearchTypes.filter(r => r.Key == this.EntityPM.SearchType)[0];
                this.UIProperties.SetRequired("SearchType", this.ObjectTableName, false);
            }
            
            this.UIProperties.SetRequired("KeywordsList", this.ObjectTableName, !this.EntityPM.KeywordsList);

        });
    }
    public _SearchByFieldCode: string;
    SearchByFieldCodeClicked(evKey) {
        this._SearchByFieldCode = evKey;
        this.EntityPM.SearchByFieldCode = this._SearchByFieldCode;
        this.UIProperties.SetRequired("SearchByFieldCode", this.ObjectTableName, false);
        
    }

    public _SearchType: string; 
    SearchTypeClicked(evKey) {
        this._SearchType = evKey;
        this.EntityPM.SearchType = this._SearchType;
        this.UIProperties.SetRequired("SearchType", this.ObjectTableName, false);
        
    }

    Loaded: boolean = false;
    ngOnInit() {
        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            SessionLocator.SelectedSession.StopBusyIndicator();
            this.Loaded = true;
        });
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
            this.isNewRecord = true;
            this.EntityPM = new PendingByKeywordPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;

        }
    }


    //#region Properties
    private _WarningMessage: string;
    public get WarningMessage() { return this._WarningMessage; }
    public set WarningMessage(newValue: string) {
        this._WarningMessage = newValue;
    }

    public get CourierPendingReasonCode() 
    {
        var _CourierPendingReasonExtendedListService = new CourierPendingReasonExtendedListService();
        var CourierPendingReason: CourierPendingReasonList;

        _CourierPendingReasonExtendedListService.GetSingleFromCacheByCode(this.EntityPM.CourierPendingReasonCode)
            .subscribe(serviceResponse => {
                CourierPendingReason = serviceResponse.Result;
            });
           if(CourierPendingReason == null){
                return null;
           }
           return CourierPendingReason.Id

    }
    public set CourierPendingReasonCode(newValue: string) {
        this.EntityPM.CourierPendingReasonCode = newValue;
    }

    public get CourierPendingReasonName() { return this.EntityPM.CourierPendingReasonName; }
    public set CourierPendingReasonName(newValue: string) {
        this.EntityPM.CourierPendingReasonName = newValue;
    }

    public get KeywordsList() { return this.EntityPM.KeywordsList; }
    public set KeywordsList(newValue: string) {
        this.UIProperties.SetRequired("KeywordsList", this.ObjectTableName, !newValue);
        
        this.EntityPM.KeywordsList = newValue;
    }
    public get ExceptKeywords() { return this.EntityPM.ExceptKeywords; }
    public set ExceptKeywords(newValue: string) {
        this.EntityPM.ExceptKeywords = newValue;
    }

    //#endregion\

    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.isNewRecord) {
            
        }

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            if (this.EntityPM == null || (this.EntityPM != null && AppTool.IsNullOrEmpty(this.EntityPM.CourierPendingReasonCode))){
                this.CancelButtonClicked();
            }
            if (this.isNewRecord) {
                this._PendingByKeywordPMService.insert(this.EntityPM).subscribe(myResult => {
                    if (myResult.HasError) {
                        this.ValidationErrorsList = [];
                        this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                        return;
                    }
                    this.CancelButtonClicked();
                });
            }
            else {
                this._PendingByKeywordPMService.update(this.EntityPM).subscribe(myResult => {
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
