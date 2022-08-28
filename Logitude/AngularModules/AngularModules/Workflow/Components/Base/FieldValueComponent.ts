import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { ObjectTablePMService } from "Infrastructure/Services/StandardPMs/ObjectTablePMService";

@Component({
    selector: "FieldValue",
    templateUrl: "./FieldValueComponent.html"
})

export class FieldValueComponent extends BaseComponent implements OnInit {

    @Input() ObjectField: ObjectFieldPM;
    @Input() Name: string;
    @Input() CurrentValue: any;

    @Output() ValueChanged = new EventEmitter<any>();

    public LookupTable: ObjectTablePM;

    public ObjectTablePMService = new ObjectTablePMService();

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {
        if (this.ObjectField && this.ObjectField.DataTypeCode === "LookUp") {
            this.setLookupTable();
        }
    }

    setLookupTable() {
        this.LookupTable = (window as any).ObjectTables.filter((o: any) => o.Id === this.ObjectField.LookUpTableId)[0];

        if (!this.LookupTable) {
            this.ObjectTablePMService.get(this.ObjectField.LookUpTableId).subscribe((response: any) => { this.handleGetLookupTableResponse(response); });
        }
    }

    handleGetLookupTableResponse(response: any) {
        if (!response.HasError) {
            this.LookupTable = response.Result;
        }
    }

    updateValue(value: string) {
        this.ValueChanged.emit(value);
    }
}