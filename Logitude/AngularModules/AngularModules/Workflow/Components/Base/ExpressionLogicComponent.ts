import { Component } from "@angular/core";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ApiQueryFilters } from "Infrastructure/DataContracts/ApiQueryFilters";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ObjectFieldList } from "Infrastructure/EntityLists/ObjectFieldList";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ExpressionCategoryList } from "Workflow/EntityLists/ExpressionCategoryList";
import { ExpressionList } from "Workflow/EntityLists/ExpressionList";
import { ApiQueryFiltersBuilder } from "Workflow/Models/ApiQueryFiltersBuilder";
import { FlowVariablesTreeList } from "Workflow/Models/FlowVariablesTreeList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ExpressionCategoryListService } from "Workflow/Services/StandardLists/ExpressionCategoryListService";
import { ExpressionListService } from "Workflow/Services/StandardLists/ExpressionListService";
import { WorkFlowInstanceListService } from "Workflow/Services/StandardLists/WorkFlowInstanceListService";

@Component({
    templateUrl: "./ExpressionLogicComponent.html"
})

export class ExpressionLogicComponent extends BaseComponent {

    public DataContext: any = this;
    public ValidationErrorsList: string[];

    public CurrentSession = SessionLocator.SelectedSession;
    public ExpressionList: ExpressionList[] = [];
    public ExpressionListByCategory: ExpressionList[] = [];
    public ExpressionCategoryList: ExpressionCategoryList[] = [];

    public expressionListService: ExpressionListService = new ExpressionListService();
    public expressionCategoryListService: ExpressionCategoryListService = new ExpressionCategoryListService();

    public Expression: string
    public ExpressionCategory: string
    public Resources: any
    public ExpressionTextArea: string

    public startPoint: number = 0
    public endPoint: number = 0

    public FlowObject: any;
    public CurrentNodeId: string;
    public FlowObjectFields: ObjectFieldList[];

    public FlowVariablesTreeList: FlowVariablesTreeList;
    public FlowVariablesTreeItems: TreeSelectItem[];

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        this.FlowObjectFields = args.FlowObjectFields ? args.FlowObjectFields : [];
        this.ExpressionTextArea = args.ExpressionValue
    }

    ngOnInit() {
        this.initializeExpressionData();
        this.initializeExpressionCategoryData();
        this.initializeFlowVariablesTree();
    }

    initializeExpressionCategoryData() {
        this.expressionCategoryListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                const result = myResponse.Result;
                this.ExpressionCategoryList = [{ Code: "ALL", Name: "All Functions", SearchFields: "All,All Functions" }];
                this.ExpressionCategoryList = this.ExpressionCategoryList.concat(result);

                var allCategory = this.ExpressionCategoryList.find(e => e.Code = "ALL")
                this.SelectedExpressionCategory = allCategory
            }
        });

    }

    initializeExpressionData() {
        this.expressionListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.ExpressionList = this.ExpressionList.concat(myResponse.Result);
            }
        });
    }

    initializeFlowVariablesTree() {
        this.FlowVariablesTreeList = new FlowVariablesTreeList(this.FlowObjectFields, this.FlowObject, this.CurrentNodeId);
        this.FlowVariablesTreeItems = this.FlowVariablesTreeList.Items;
    }

    updateExpressionTextArea(value: string) {
        this.ExpressionTextArea = value;
    }

    private _SelectedExpression: ExpressionList;
    public get SelectedExpression(): ExpressionList {
        return this._SelectedExpression;
    }
    public set SelectedExpression(v: ExpressionList) {
        this._SelectedExpression = v;
        this.ExpressionTextArea = v.Name + v.Body;
    }

    private _SelectedExpressionCategory: ExpressionCategoryList;
    public get SelectedExpressionCategory(): ExpressionCategoryList {
        return this._SelectedExpressionCategory;
    }
    public set SelectedExpressionCategory(v: ExpressionCategoryList) {
        this._SelectedExpressionCategory = v;
        this._SelectedExpression = null;
        // this.ExpressionTextArea = "";
        this.ExpressionListByCategory = [];
        this.LoadFilterdExpressionFunctions();
    }

    LoadFilterdExpressionFunctions() {
        var selectedCategory = this.SelectedExpressionCategory.Code
        if (!this.ExpressionList) {
            this.initializeExpressionData();
        }
        if (selectedCategory == 'ALL') {
            this.ExpressionListByCategory = this.ExpressionList
        } else {
            this.ExpressionListByCategory = this.ExpressionList.filter(e => e.CategoryCode == selectedCategory)
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
        let patchedValue = currentValue.substr(0, this.startPoint) + resource + currentValue.substr(this.endPoint);
        this.ExpressionTextArea = patchedValue;
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
        if(this.ExpressionTextArea){
            this.CurrentSession.CurrentWindow.Close(this.ExpressionTextArea);
        }
        else{
            this.ValidationErrorsList.push("Expression is required");
        }
        
    }
}