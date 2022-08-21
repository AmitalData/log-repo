import { Component } from '@angular/core';

@Component({
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public ObjectTableName: string = null;

    public StatusColor: string = null;

    constructor() {

    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];

        this.setStatusColor();
    }

    private setStatusColor() {
        if (this.ObjectTableName === "WorkFlow" && this.FieldName === "StatusName") {
            let statusCode = this.Entity["StatusCode"];
            switch (statusCode) {
                case "DRFT":
                    this.StatusColor = "#258bee";
                    break;
                case "PUED":
                    this.StatusColor = "#54aa41";
                    break;
                case "PAED":
                    this.StatusColor = "#000000";
                    break;
                default:
                    this.StatusColor = "#000000";
                    break;
            }
        }
    }
}