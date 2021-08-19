import { Component} from '@angular/core';


@Component({
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent  {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;


    constructor() {

    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];

        if (this.Entity != null) {
            if (this.FieldName != null) {
                this.FieldValue = this.Entity[this.FieldName];
     
            }

        }
    }

 
    
}
