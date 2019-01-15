import {Component, Output, EventEmitter} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';

@Component({
    moduleId: module.id,
    templateUrl: './CustomsShipperFiltersMenuComponent.html',

})

export class CustomsShipperFiltersMenuComponent {

    constructor() {
        
    }

    NewDepositionFormClcik() {

        var link = "https://forms.gov.il/globaldata/getsequence/getHtmlForm.aspx?formType=SOVE01_hasava@taxes.gov.il";
        var win = window.open(link, '_blank');
        win.focus();
    }

}