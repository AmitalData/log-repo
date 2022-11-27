import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core";
import { LogitudeWindow } from "Controls/Windows/LogitudeWindow";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { ExpressionValue } from "Workflow/Models/Types";

@Component({
    selector: "Expression",
    templateUrl: "./ExpressionComponent.html"
})

export class ExpressionComponent extends BaseComponent implements OnInit {

    @Input() CurrentValue: ExpressionValue;
    @Input() FlowObject: any;
    @Input() FlowObjectFields: ObjectFieldList[];
    @Input() CurrentNodeId: string;
    @Input() IsDisabled: boolean = false;
    @Output() ValueChanged = new EventEmitter<string>();

    public ExpressionValue: ExpressionValue;

    constructor() {
        super();
    }

    ngOnInit() {
        let defaultExpressionValue: ExpressionValue = {
            expression: null,
            variables: []
        };
        this.ExpressionValue = this.CurrentValue || defaultExpressionValue;
    }

    openExpressionBuilder() {
        if (!this.IsDisabled) {
            let expressionBuilderComponentPath = "./Workflow/Components/Base/ExpressionBuilderComponent";
            let expressionBuilderWindow = this.buildExpressionBuilderWindow();
            expressionBuilderWindow.Show(expressionBuilderComponentPath);
            expressionBuilderWindow.WindowClosed.subscribe((data: any) => { this.handleExpressionBuilderWindowClosed(data); });
        }
    }

    handleExpressionBuilderWindowClosed(data: any) {
        if (data.Action === "save") {
            this.ExpressionValue = data.ExpressionValue;
            this.ValueChanged.emit(data.ExpressionValue);
        }
    }

    buildExpressionBuilderWindow() {
        let expressionBuilderWindow = new LogitudeWindow();
        let expressionBuilderWindowArgs: any = {
            FlowObject: this.FlowObject,
            CurrentNodeId: this.CurrentNodeId,
            FlowObjectFields: this.FlowObjectFields,
            ExpressionValue: this.ExpressionValue
        };
        expressionBuilderWindow.Width = 900;
        expressionBuilderWindow.Height = 420;
        expressionBuilderWindow.RTL = false;
        expressionBuilderWindow.Title = "Build Expression";
        expressionBuilderWindow.WindowArgs = expressionBuilderWindowArgs;
        return expressionBuilderWindow;
    }
}