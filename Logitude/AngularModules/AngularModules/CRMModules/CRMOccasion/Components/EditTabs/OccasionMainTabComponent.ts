import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';


@Component({
    selector: 'OccasionMainTabComponent',
    moduleId: module.id,
    templateUrl: './OccasionMainTabComponent.html',
})

export class OccasionMainTabComponent extends BaseComponent {


    constructor(public entityArgs: EntityArgs) {
        super();
        
    }


}
