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
                    this.StatusColor = "blue";
                    break;
                case "PUED":
                    this.StatusColor = "green";
                    break;
                case "PAED":
                    this.StatusColor = "black";
                    break;
                default:
                    this.StatusColor = "black";
                    break;
            }
        }
    }
}