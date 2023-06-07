import { Component, } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';

@Component({
    templateUrl: './CourierWorksheetFromExcelComponent.html',
    selector: 'CourierWorksheetFromExcelComponent'})


export class CourierWorksheetFromExcelComponent extends BaseComponent  {
    constructor(){
        super();
    }
}