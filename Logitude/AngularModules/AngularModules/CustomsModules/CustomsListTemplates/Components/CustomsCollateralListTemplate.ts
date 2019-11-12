declare var window: any;
import { Component, ChangeDetectorRef } from '@angular/core';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { OnInit, Output, EventEmitter, ComponentRef, QueryList } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';

import { ListComponentArgs } from '../../../Infrastructure/Args';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { CustomsCollateralList } from '../../../Customs/EntityLists/CustomsCollateralList';


@Component({
    moduleId: module.id,
    templateUrl: 'CustomsCollateralListTemplate.html',
})

export class CustomsCollateralListTemplate {

    _CustomsCollateralRecord: CustomsCollateralList;
    public fieldName: any;
    TableUpdateButtonIsEnabled: boolean = false;
    UpdateButtonVisibility: boolean = false;
    TableUpdateButtonOpacity: string = "1";
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        //        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;


    }

    setVariables(CustomsCollateralRecord: CustomsCollateralList, fieldName: string) {
        ///console.log(rowData);
        this._CustomsCollateralRecord = CustomsCollateralRecord;

        this.fieldName = fieldName;

    }


}
