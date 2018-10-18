
import {Component, ChangeDetectorRef} from '@angular/core';
import { DateTool} from '../../../Infrastructure/Tools';


@Component({
    moduleId: module.id,
    templateUrl: './EndDateComponent.html',
})

export class EndDateComponent {

    public rowData: any;
    public fieldName: any;
    fontcolor: string;
    constructor(private CD: ChangeDetectorRef) {
    }
    setVariables(rowData: any, fieldName: string, additionalData: any) {
       
      
        this.rowData = rowData;
        this.fieldName = fieldName;
        if (rowData.EndDate) {
            var valueDate = new Date(rowData.EndDate.valueOf()).valueOf();
        }
        var today = DateTool.GetCurrentDateAsUtc().valueOf();
        if (valueDate != null && valueDate < today) {
        
            this.fontcolor = "red";
        }

        else {
          
            this.fontcolor = "limegreen";
        }
        this.CD.detectChanges();

    }

}