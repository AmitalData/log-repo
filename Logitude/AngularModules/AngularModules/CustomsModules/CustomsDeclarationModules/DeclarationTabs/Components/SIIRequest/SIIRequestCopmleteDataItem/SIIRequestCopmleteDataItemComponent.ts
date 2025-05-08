import { Component, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';


@Component({
    selector: 'SIIRequestCopmleteDataItemComponent',
    templateUrl: './SIIRequestCopmleteDataItemComponent.html',
    styleUrls: ['./SIIRequestCopmleteDataItemComponent.scss'],
    providers: [EntityArgs],
})


export class SIIRequestCopmleteDataItemComponent extends BaseComponent implements OnInit {


    constructor(public entityArgs: EntityArgs) {
        super();
    }

    ngOnInit(): void {
    }

 
}
