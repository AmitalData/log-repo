import { Component } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    templateUrl: './SIIRequestComponent.html',
    providers: [EntityArgs],
})


export class SIIRequestComponent extends BaseComponent {
    constructor(public entityArgs: EntityArgs) {
        super();
    }
}
