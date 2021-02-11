import {Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList } from '@angular/core';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../Infrastructure/Utilities/LocationDirective';
import {ApiQueryFilters, FilterItem} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, ArrayTool} from '../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import { CustomSendOptionsArgs, SendRequestVIA} from '../../../Customs/DataContract/RequestParams/RequestParamsBase';

import {CustomsAirlinePM} from '../../../Customs/EntityPMs/CustomsAirlinePM';
import {CustomsAirlinePMService} from '../../../Customs/Services/StandardPMs/CustomsAirlinePMService';
import { CustomsAirlineList } from '../../../Customs/EntityLists/CustomsAirlineList';
import { CustomsAirlineExtendedPMService } from '../../../Customs/Services/ExtendedPMs/CustomsAirlineExtendedPMService';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsAirlineListService } from '../../../Customs/Services/StandardLists/CustomsAirlineListService';


@Component({
    
    templateUrl: './AddEditCustomsAirlineComponent.html',
})

export class AddEditCustomsAirlineComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.CustomsAirline";
    public EntityPM: CustomsAirlinePM;
    isWindowMode: boolean = false;
    ValidationErrorsList: any[] = [];
    _CustomsAirlinePMService: CustomsAirlinePMService = new CustomsAirlinePMService();
    private _CustomsAirlineListService: CustomsAirlineListService = new CustomsAirlineListService();
    _CustomsAirlineExtendedPMService: CustomsAirlineExtendedPMService = new CustomsAirlineExtendedPMService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    _CustomsAirlineList: CustomsAirlineList;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
            this.EntityPM = new CustomsAirlinePM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.isWindowMode = true;
        } else {
            this.EntityPM = this.entityArgs.EntityPM;
        }
        
    }

    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.CustomsAirline").subscribe((response:any) => {
            });


            //this.RefreshBtnClick()
        });

    }
    SetWindowArgs(WinArg) {
        ;
        if (!AppTool.IsNullOrEmpty(WinArg)) {
            this.isWindowMode = true;
        }
        this._CustomsAirlineList = WinArg.SelectedItem;
        this.CurrentSession.StartBusyIndicatorLoading();

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.CustomsAirline").subscribe((response:any) => {
                this._CustomsAirlineExtendedPMService
                    .GetSingleCustomsAirlineByCodeAndPrefix
                    (this._CustomsAirlineList.AirlineCode, this._CustomsAirlineList.AirlinePrefix)
                    .subscribe((rsp:any) => {
                        this.EntityPM = rsp.Result;
                        this.CurrentSession.StopBusyIndicator();
                    });

            });
        });

    }

   

    //#region Properties

    public get AirlinePrefix() { return this.EntityPM.AirlinePrefix; }
    public set AirlinePrefix(newValue: string) {
        this.EntityPM.AirlinePrefix = newValue;
    }

    public get AirlineCode() { return this.EntityPM.AirlineCode; }
    public set AirlineCode(newValue: string) {
        this.EntityPM.AirlineCode = newValue;
    }

    public get ICAO() { return this.EntityPM.ICAO; }
    public set ICAO(newValue: string) {
        this.EntityPM.ICAO = newValue;
    }

    public get EnglishName() { return this.EntityPM.EnglishName; }
    public set EnglishName(newValue: string) {
        this.EntityPM.EnglishName = newValue;
    }

    public get LocalName() { return this.EntityPM.LocalName; }
    public set LocalName(newValue: string) {
        this.EntityPM.LocalName = newValue;
    }

    public get InActive() { return this.EntityPM.InActive; }
    public set InActive(newValue: boolean) {
        this.EntityPM.InActive = newValue;
    }

    public get UnloadPortCode() { return this.EntityPM.UnloadPortCode; }
    public set UnloadPortCode(newValue: string) {
        this.EntityPM.UnloadPortCode = newValue;
    }
    //#endregion\

    OkButtonClicked() {

        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            this._CustomsAirlinePMService.insert(this.EntityPM).subscribe((myResult:any) => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;
                    this.CurrentSession.CloseCurrentWindowEmit("ok");

                }
                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    

}
