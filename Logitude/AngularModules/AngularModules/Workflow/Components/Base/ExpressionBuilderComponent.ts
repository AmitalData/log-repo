import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ExpressionList } from "Workflow/EntityLists/ExpressionList";
import { ExpressionsTreeList } from "Workflow/Models/ExpressionsTreeList";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { ListItem } from "Workflow/Models/ListItem";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ExpressionCategoryListService } from "Workflow/Services/StandardLists/ExpressionCategoryListService";
import { ExpressionListService } from "Workflow/Services/StandardLists/ExpressionListService";

@Component({
    templateUrl: "./ExpressionBuilderComponent.html"
})

export class ExpressionBuilderComponent extends BaseComponent {

    public DataContext: any = this;
    public ValidationErrorsList: string[];
    public CurrentSession = SessionLocator.SelectedSession;

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];
    public ExpressionValue: string;

    public CursorStartPoint: number = 0;
    public CursorEndPoint: number = 0;

    public ExpressionCategories: ListItem[];
    public DefaultExpressionCategory = new ListItem("ALL", "All Functions");
    public ExpressionCategory: ListItem = this.DefaultExpressionCategory;

    public FlowVariablesTreeItems: TreeSelectItem[];
    public ExpressionsTreeItems: TreeSelectItem[];

    public ExpressionCategoryChanged: boolean = false;

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        this.ExpressionValue = args.ExpressionValue || null;
    }

    ngOnInit() {
        this.initializeExpressionCategories();
        this.initializeExpressions();
        this.initializeFlowVariablesTree();
    }

    initializeExpressionCategories() {
        let expressionCategoryListService = new ExpressionCategoryListService();
        expressionCategoryListService.getAll().subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.ExpressionCategories = [];
                this.ExpressionCategories.push(this.DefaultExpressionCategory);
                this.ExpressionCategories = this.ExpressionCategories.concat(serviceResponse.Result.map((e: ExpressionList) => { return new ListItem(e.Code, e.Name); }));
            }
        });
    }

    initializeExpressions() {
        let expressionListService = new ExpressionListService();
        expressionListService.getAll().subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.ExpressionsTreeItems = new ExpressionsTreeList(serviceResponse.Result).Items;
            }
        });
    }

    initializeFlowVariablesTree() {
        let showVariables = {
            ShowRecordsVariables: true,
            ShowDeclaredVariables: true,
            ShowRecordsCollectionVariables: true,
            ShowDeclaredCollectionVariables: true
        };
        this.FlowVariablesTreeItems = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId, showVariables).Items;
    }

    updateExpressionCategory(expressionCategory: ListItem) {
        this.ExpressionCategory = expressionCategory;
        this.ExpressionCategoryChanged = !this.ExpressionCategoryChanged;
    }

    updateExpression(expressionItem: TreeSelectItem) {
        let expression: ExpressionList = expressionItem?.data;
        if (expression) {
            this.ExpressionValue = expression.Name + expression.Body;
            let expressionValueLength = this.ExpressionValue.length;
            this.updateCursorPointer(expressionValueLength, expressionValueLength);
        }
    }

    updateExpressionValue(value: string) {
        this.ExpressionValue = value;
    }

    updateCursorPointer(startPoint: number, endPoint: number) {
        if (startPoint && endPoint) {
            this.CursorStartPoint = startPoint;
            this.CursorEndPoint = endPoint;
        }
    }

    updateCursorPointerFromEvent(event: any) {
        if (event && (event.key === "Delete" || event.key === "Backspace")) {
            this.CursorStartPoint = event.target.selectionStart;
            this.CursorEndPoint = event.target.selectionEnd;
        } else if (this.isCursorPointerChanged(event)) {
            this.CursorStartPoint += 1;
            this.CursorEndPoint = this.CursorStartPoint;
        }
    }

    isCursorPointerChanged(event: any) {
        if (event) {
            return event.target.selectionStart !== this.CursorStartPoint && event.target.selectionEnd !== this.CursorEndPoint;
        }
        return false;
    }

    selectVariable(variable: string) {
        this.setVariableInExpression(variable);
    }

    setVariableInExpression(variable: string) {
        if (variable) {
            let variableWithBrackets = "{" + variable + "}";
            let currentExpressionValue = this.ExpressionValue;
            if (currentExpressionValue) {
                let newExpressionValue = currentExpressionValue.slice(0, this.CursorStartPoint) + variableWithBrackets + currentExpressionValue.slice(this.CursorEndPoint);
                this.ExpressionValue = newExpressionValue;
            } else {
                this.ExpressionValue = variableWithBrackets;
            }
        }
    }

    checkExpressionSyntax() {
        console.log("true");
    }

    cancelButtonClicked() {
        this.closeExpressionBuilder();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.ExpressionValue) {
            this.closeExpressionBuilder(true);
        }
        else {
            this.ValidationErrorsList.push("Expression is required");
        }
    }

    closeExpressionBuilder(isSaved: boolean = false) {
        let data: any = {
            Action: (isSaved ? "save" : "cancel"),
            ExpressionValue: this.ExpressionValue
        };
        this.CurrentSession.CurrentWindow.Close(data);
    }
}