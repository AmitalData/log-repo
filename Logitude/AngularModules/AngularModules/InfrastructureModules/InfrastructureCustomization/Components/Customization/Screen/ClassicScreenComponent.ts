import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ScreenLayoutComponent } from '../ScreenLayoutComponent';
@Component({
    selector: 'ClassicScreenComponent',
    templateUrl: './ClassicScreenComponent.html',
    inputs: [''],

})

export class ClassicScreenComponent extends BaseComponent implements OnInit {
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
