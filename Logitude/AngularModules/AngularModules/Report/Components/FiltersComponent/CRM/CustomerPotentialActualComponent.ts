import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../../Components/Filters/QueryFilterItem';

@Component({
    moduleId: module.id,
    templateUrl: './CustomerPotentialActualComponent.html',
})

export class CustomerPotentialActualComponent extends BaseComponent   {

    constructor() {
        super();
    }


}