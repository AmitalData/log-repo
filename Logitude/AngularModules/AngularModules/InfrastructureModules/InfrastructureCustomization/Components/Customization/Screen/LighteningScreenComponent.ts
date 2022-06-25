import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenLayoutComponent } from '../ScreenLayoutComponent';
@Component({
    selector: 'LighteningScreenComponent',
    templateUrl: './LighteningScreenComponent.html',
    inputs: [''],

})

export class LighteningScreenComponent extends BaseComponent implements OnInit {
    public ScreenLayoutComponent: ScreenLayoutComponent;
    constructor() {
        super();
    }

    ngOnInit(): void {

    }




    Run(screenLayoutComponent: ScreenLayoutComponent) {
        this.ScreenLayoutComponent = screenLayoutComponent;
    }

}
