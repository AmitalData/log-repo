import {Component, ViewChild, ViewContainerRef, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FontTool} from '../../../Infrastructure/Tools';
@Component({
    moduleId: module.id,
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;


    public ObjectTableName: string = null;
    public IsHeaderScreenTemplate: boolean = false;
    public BackgroudColor: string = "";
    public WarehouseDateType: string = "";

    constructor() {

    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsHeaderScreenTemplate = args['IsHeaderScreenTemplate'];

        if (this.Entity != null) {
            if (this.FieldName != null) {
                this.FieldValue = this.Entity[this.FieldName];

                if (this.ObjectTableName == "WarehouseEntry" && this.FieldName == "ActualEntryDate") {
                    this.BackgroudColor = this.transform(this.FieldValue, this.Entity['ExpectedEntryDate']);
                }

                //if (this.ObjectTableName == "WarehouseRelease" && this.FieldName == "ActualReleaseDate") {
                //    this.BackgroudColor = this.transform(this.FieldValue, this.Entity['ExpectedReleaseDate']);
                //}

                if (this.ObjectTableName == "WarehouseRelease" && this.FieldName == "ReleaseDate") {
                    this.BackgroudColor = this.transform(this.Entity['ActualReleaseDate'], this.Entity['ExpectedReleaseDate']);
                }
            }
        }
    }

    transform(actual: Date, expected: Date ): string {

        var myResult = FontTool.Black;

         if (actual != null) {
             myResult = FontTool.Green;
             this.WarehouseDateType = " (actual)";


        } else   if(expected != null) {
             myResult = FontTool.Red;
             this.WarehouseDateType = " (expected)";
             var name = this.FieldName.replace("Actual", "Expected");
             this.FieldValue = this.Entity[name];
        }



        return myResult;
    }





}
