import {Component, Output, EventEmitter} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

@Component({
    moduleId: module.id,
    templateUrl: './CustomsShipperFiltersMenuComponent.html',

})

export class CustomsShipperFiltersMenuComponent {

    constructor() {
        
    }

    NewDepositionFormClcik() {
        ServiceLocator.SendTotangoUserActivity("CustomsShipper", "Deposition Link");
        var link = "https://forms.gov.il/globaldata/getsequence/getHtmlForm.aspx?formType=SOVE01_hasava@taxes.gov.il";
        var win = window.open(link, '_blank');
        win.focus();
    }

}