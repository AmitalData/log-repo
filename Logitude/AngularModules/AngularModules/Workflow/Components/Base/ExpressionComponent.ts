import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { ObjectTablePM } from "Infrastructure/EntityPMs/ObjectTablePM";
import { FieldTypes } from "Workflow/Constants/FieldTypes";
import { ExpressionList } from "Workflow/EntityLists/ExpressionList";
import { BooleanValuesList } from "Workflow/Models/BooleanValuesList";
import { ListItem } from "Workflow/Models/ListItem";
import { ObjectTables } from "Workflow/Models/ObjectTables";
import { ExpressionListService } from "Workflow/Services/StandardLists/ExpressionListService";

@Component({
    selector: "Expression",
    templateUrl: "./ExpressionComponent.html"
})

export class ExpressionComponent extends BaseComponent implements OnInit {

    @Input() CurrentValue: string;
    @Input() FlowObject: any;
    @Input() FlowObjectFields: ObjectFieldList[];
    @Input() CurrentNodeId: string;
    @Input() IsDisabled: boolean = false;
    @Output() ValueChanged = new EventEmitter<string>();

    public DataContext: any = this;
    public ExpressionList: ExpressionList[] = [];
    public expressionListService: ExpressionListService = new ExpressionListService();


    public ExpressionValue: string;

    constructor() {
        super();
    }

    ngOnInit() {
        this.ExpressionValue = this.CurrentValue
        this.initializeExpressionData();

    }

    initializeExpressionData() {
        this.expressionListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ExpressionList = this.ExpressionList.concat(myResponse.Result);
            }
        });
    }

    expressionHandle() {
        if (!this.IsDisabled) {
            let propertiesComponentPath = "./Workflow/Components/Base/ExpressionLogicComponent";
            let propertiesWindow = this.buildPropertiesWindow();
            propertiesWindow.Show(propertiesComponentPath);
            propertiesWindow.WindowClosed.subscribe((data: any) => { this.handlePropertiesWindowClosed(data); });
        }
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
            ExpressionValue: this.ExpressionValue,
            ExpressionList: this.ExpressionList
        };
        propertiesWindow.Height = 440;
        propertiesWindow.Width = 985;
        propertiesWindow.RTL = false;
        propertiesWindow.Title = "Field Expression";
        propertiesWindow.WindowArgs = propertiesWindowArgs;
        return propertiesWindow;
    }

}