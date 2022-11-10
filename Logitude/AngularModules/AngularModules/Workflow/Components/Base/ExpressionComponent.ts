import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { BooleanValuesList } from "Workflow/Models/BooleanValuesList";
import { ListItem } from "Workflow/Models/ListItem";
import { ObjectTables } from "Workflow/Models/ObjectTables";

@Component({
    selector: "Expression",
    templateUrl: "./ExpressionComponent.html"
})

export class ExpressionComponent extends BaseComponent implements OnInit {

    @Input() CurrentValue: string;
    @Input() FlowObject: any;
    @Input() FlowObjectFields: ObjectFieldList[];
    @Input() CurrentNodeId: string;
    @Output() ValueChanged = new EventEmitter<string>();

    public DataContext: any = this;


    public ExpressionValue: string;

    constructor() {
        super();
    }

    ngOnInit() {
        this.ExpressionValue = this.CurrentValue
    }


    expressionHandle() {
        let propertiesComponentPath = "./Workflow/Components/Base/ExpressionLogicComponent";
        let propertiesWindow = this.buildPropertiesWindow();
        propertiesWindow.Show(propertiesComponentPath);
        propertiesWindow.WindowClosed.subscribe((data: any) => { this.handlePropertiesWindowClosed(data); });
    }

    handlePropertiesWindowClosed(data) {
        this.ExpressionValue = data
        this.ValueChanged.emit(data);
    }

    buildPropertiesWindow() {
        let propertiesWindow = new LogitudeWindow();
        let propertiesWindowArgs: any = {
            FlowObject: this.FlowObject,
            CurrentNodeId: this.CurrentNodeId,
            FlowObjectFields: this.FlowObjectFields,
            ExpressionValue : this.ExpressionValue
        };
        propertiesWindow.Height = 440;
        propertiesWindow.Width = 985;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = "Field Expression";
        propertiesWindow.WindowArgs = propertiesWindowArgs;
        return propertiesWindow;
    }

}