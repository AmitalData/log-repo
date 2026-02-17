import {Component, AfterViewInit, ChangeDetectorRef, ViewChildren, QueryList } from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {LocationDirective} from '../../../../Infrastructure/Utilities/LocationDirective';
import {ApiQueryFilters, FilterItem} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, ArrayTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { CustomSendOptionsArgs, SendRequestVIA} from '../../../../Customs/DataContract/RequestParams/RequestParamsBase';

import {CouriersVatPM} from '../../../../Customs/EntityPMs/CouriersVatPM';
import {CouriersVatPMService} from '../../../../Customs/Services/StandardPMs/CouriersVatPMService';


@Component({
    moduleId: module.id,
    templateUrl: './AddEditCouriersVatComponent.html',
})

export class AddEditCouriersVatComponent extends BaseComponent {
    public DataContext: any = this;
    public ObjectTableName: string = "Customs.CouriersVat";
    public EntityPM: CouriersVatPM;
    isWindowMode: boolean = false;
    ValidationErrorsList: any[] = [];
    _CouriersVatPMService: CouriersVatPMService = new CouriersVatPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (AppTool.IsNullOrEmpty(entityArgs.EntityPM)) {
            this.EntityPM = new CouriersVatPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.isWindowMode = true;
        } else {
            this.EntityPM = this.entityArgs.EntityPM;
        }
        
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
        }
    }

    //#region Properties

    public get VatNumber() { return this.EntityPM.VatNumber; }
    public set VatNumber(newValue: string) {
        this.EntityPM.VatNumber = newValue;
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

    //#endregion\

    OkButtonClicked() {

        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (errors.length > 0) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList = errors;
        } else {
            this._CouriersVatPMService.insert(this.EntityPM).subscribe(myResult => {

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
