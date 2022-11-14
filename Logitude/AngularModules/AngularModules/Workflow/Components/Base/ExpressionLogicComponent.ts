import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ExpressionCategoryList } from "Workflow/EntityLists/ExpressionCategoryList";
import { ExpressionList } from "Workflow/EntityLists/ExpressionList";
import { ExpressionFunctionTreeList } from "Workflow/Models/ExpressionFunctionTreeList";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ExpressionCategoryListService } from "Workflow/Services/StandardLists/ExpressionCategoryListService";
import { ExpressionListService } from "Workflow/Services/StandardLists/ExpressionListService";

@Component({
    templateUrl: "./ExpressionLogicComponent.html"
})

export class ExpressionLogicComponent extends BaseComponent {

    public DataContext: any = this;
    public ValidationErrorsList: string[];
    public CurrentSession = SessionLocator.SelectedSession;

    public ExpressionList: ExpressionList[] = [];
    public ExpressionCategoryList: ExpressionCategoryList[] = [];

    public expressionListService: ExpressionListService = new ExpressionListService();
    public expressionCategoryListService: ExpressionCategoryListService = new ExpressionCategoryListService();

    public Resources: any
    public ExpressionTextArea: string
    public SelectedCategory: ExpressionCategoryList
    public SelectedCategoryCode: string = "ALL";

    public startPoint: number = 0
    public endPoint: number = 0

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];

    public FlowVariablesTreeList: FlowVariablesTreeList;
    public FlowVariablesTreeItems: TreeSelectItem[];

    public ExpressionFunctionTreeList: ExpressionFunctionTreeList;
    public ExpressionFunctionTreeItems: TreeSelectItem[];

    public SelectedCategoryChanged: boolean = false;

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        this.ExpressionTextArea = args.ExpressionValue;
        this.ExpressionList = args.ExpressionList ? args.ExpressionList : [];
    }

    ngOnInit() {
        this.initializeExpressionCategoryData();
        this.initializeFlowVariablesTree();
        this.initializeExpressionFunctionTree();
    }

    initializeExpressionCategoryData() {
        this.expressionCategoryListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                const result = myResponse.Result;
                let allCategoryType = { Code: "ALL", Name: "All Functions", SearchFields: "All,All Functions" };
                this.ExpressionCategoryList.push(allCategoryType);
                this.ExpressionCategoryList = this.ExpressionCategoryList.concat(result);
                this.SelectedCategory = allCategoryType;
            }
        });
    }

    initializeFlowVariablesTree() {
        this.FlowVariablesTreeList = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId);
        this.FlowVariablesTreeItems = this.FlowVariablesTreeList.Items;
    }

    initializeExpressionFunctionTree() {
        this.ExpressionFunctionTreeList = new ExpressionFunctionTreeList(this.ExpressionList);
        this.ExpressionFunctionTreeItems = this.ExpressionFunctionTreeList.Items;
    }

    updateExpressionCategory(value: any) {
        if (value != null) {
            this.SelectedCategory = value;
            this.SelectedCategoryCode = value.Code
            this.SelectedCategoryChanged = !this.SelectedCategoryChanged;
        }
    }

    updateExpression(value: any) {
        let v: ExpressionList = value.data;
        if (v != null) {
            this.ExpressionTextArea = v.Name + v.Body;
            this.updateCurserPoint(this.ExpressionTextArea)
        }
    }

    updateExpressionTextArea(value: string) {
        this.ExpressionTextArea = value;
    }

    updateCurserPoint(value: string) {
        if (value) {
            this.startPoint = value.length
            this.endPoint = value.length
        }
    }

    updateResources(value: string) {
        this.Resources = value;
        if (value != null) {
            this.setResourcesInFunction(value)
        }
    }

    setResourcesInFunction = (selectedResource: string) => {
        let resource = '{' + selectedResource + '}'
        let currentValue = this.ExpressionTextArea;
        if (currentValue) {
            let patchedValue = currentValue.substr(0, this.startPoint) + resource + currentValue.substr(this.endPoint);
            this.ExpressionTextArea = patchedValue;
        } else {
            this.ExpressionTextArea = resource;
        }
    }

    setCurserPoints(event: any) {
        this.startPoint = event.target.selectionStart;
        this.endPoint = event.target.selectionEnd;
    }

    checkSyntax() {
        console.log("true")
    }

    cancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.ExpressionTextArea) {
            this.CurrentSession.CurrentWindow.Close(this.ExpressionTextArea);
        }
        else {
            this.ValidationErrorsList.push("Expression is required");
        }

    }
}