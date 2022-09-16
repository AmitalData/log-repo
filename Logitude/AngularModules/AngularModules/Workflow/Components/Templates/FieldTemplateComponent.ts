import { ChangeDetectorRef, Component } from '@angular/core';

@Component({
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {

    public Entity: any = null;
    public FieldName: string = null;
    public ObjectTableName: string = null;

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
    }
    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.Entity = rowData;
        this.FieldName = fieldName; 
        
        if (rowData[fieldName]) {
            var temp: boolean = rowData[fieldName];
          
        }
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        } 
    }
}