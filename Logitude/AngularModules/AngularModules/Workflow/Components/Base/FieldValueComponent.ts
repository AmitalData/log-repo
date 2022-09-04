import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldPM } from "Infrastructure/EntityPMs/ObjectFieldPM";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { ObjectTablePMService } from "Infrastructure/Services/StandardPMs/ObjectTablePMService";
import { BooleanItems } from "Workflow/Constants/BooleanItems";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanItemsList } from "Workflow/Models/BooleanItemsList";
import { ListItem } from "Workflow/Models/ListItem";

@Component({
    selector: "FieldValue",
    templateUrl: "./FieldValueComponent.html"
})

export class FieldValueComponent extends BaseComponent implements OnInit {

    @Input() ObjectField: ObjectFieldPM;
    @Input() Name: string;
    @Input() CurrentValue: string;

    @Output() ValueChanged = new EventEmitter<string>();

    public LookupTable: ObjectTablePM;

    public ObjectTablePMService = new ObjectTablePMService();

    public BooleanItems: ListItem[] = new BooleanItemsList().BooleanItems;

    public ListItem = (itemCode: string) => { return new ListItem(itemCode) };

    public FieldTypes = FieldTypes;

    DataContext: any = this;

    constructor() {
        super();
    }

    ngOnInit() {
        if (this.ObjectField && this.ObjectField.DataTypeCode === FieldTypes.LookUp) {
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