import { Component } from "@angular/core";
import { MentionConfig } from "angular-mentions";
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { ExpressionList } from "Workflow/EntityLists/ExpressionList";
import { OperatorCategoryList } from "Workflow/EntityLists/OperatorCategoryList";
import { OperatorList } from "Workflow/EntityLists/OperatorList";
import { ExpressionsTreeList } from "Workflow/TreeLists/ExpressionsTreeList";
import { FlowVariablesTreeList } from "Workflow/TreeLists/FlowVariablesTreeList";
import { ListItem } from "Workflow/Models/ListItem";
import { OperatorsTreeList } from "Workflow/TreeLists/OperatorsTreeList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";
import { ExpressionValue } from "Workflow/Types";
import { ExpressionCategoryListService } from "Workflow/Services/StandardLists/ExpressionCategoryListService";
import { ExpressionListService } from "Workflow/Services/StandardLists/ExpressionListService";
import { OperatorCategoryListService } from "Workflow/Services/StandardLists/OperatorCategoryListService";
import { OperatorListService } from "Workflow/Services/StandardLists/OperatorListService";

@Component({
    templateUrl: "./ExpressionBuilderComponent.html"
})

export class ExpressionBuilderComponent extends BaseComponent {

    public DataContext: any = this;
    public ValidationErrorsList: string[];
    public CurrentSession = SessionLocator.SelectedSession;

    public FlowObject: any;
    public CurrentNodeId: string;
    public ExpressionValue: ExpressionValue;

    public CursorStartPoint: number = 0;
    public CursorEndPoint: number = 0;

    public ExpressionCategories: ListItem[];
    public DefaultExpressionCategory = new ListItem("ALL", "All Functions");
    public ExpressionCategory: ListItem = this.DefaultExpressionCategory;

    public Operators: OperatorList[];
    public OperatorCategories: OperatorCategoryList[];
    public OperatorsTreeItems: TreeSelectItem[];

    public FlowVariablesTreeList: FlowVariablesTreeList;
    public FlowVariablesTreeItems: TreeSelectItem[];
    public ExpressionsTreeItems: TreeSelectItem[];

    public ExpressionCategoryChanged: boolean = false;

    public IsSaving: boolean = false;

    public IsSuggestionsPanelOpened: boolean = false;
    public SuggestionsConfig: MentionConfig;
    public FunctionPlaceHolder: string = "Insert a function...";
    public VariablePlaceHolder: string = "Insert a variable...";
    public OperatorPlaceHolder: string = "Select an operator...";

    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        this.FlowObject = args.FlowObject ? args.FlowObject : null;
        this.CurrentNodeId = args.CurrentNodeId ? args.CurrentNodeId : null;
        let defaultExpressionValue: ExpressionValue = {
            expression: null,
            variables: []
        };
        this.ExpressionValue = args.ExpressionValue ? JSON.parse(JSON.stringify(args.ExpressionValue)) : defaultExpressionValue;

        let expressionLength = this.ExpressionValue.expression ? this.ExpressionValue.expression.length : 0;
        this.updateCursorPointer(expressionLength, expressionLength);
    }

    ngOnInit() {
        this.initializeSuggestionsConfig();
        this.initializeExpressionCategories();
        this.initializeExpressions();
        this.initializeOperators();
        this.initializeFlowVariablesTree();
    }

    initializeSuggestionsConfig() {
        this.SuggestionsConfig = {
            mentions: Array.from("abcdefghijklmnopqrstuvwxyz").map(triggerChar => {
                return {
                    triggerChar: triggerChar,
                    items: [],
                    labelKey: "code",
                    disableSort: true,
                    allowSpace: true,
                    mentionSelect: (mentionItem: any) => {
                        return mentionItem.value;
                    },
                    mentionFilter: (filterTerm: string, mentionItems: any) => {
                        let startsWithFilterTerm = (filterTerm && !filterTerm.startsWith(triggerChar) ? triggerChar : "") + (filterTerm ? filterTerm : "");
                        let filteredMentionItems = mentionItems.filter((i: any) => i.name.toString().toLowerCase().startsWith(startsWithFilterTerm.toLowerCase()));
                        return filteredMentionItems;
                    }
                };
            })
        };
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
                this.addExpressionsToSuggestionsConfig(this.ExpressionsTreeItems);
            }
        });
    }

    addExpressionsToSuggestionsConfig(expressionsTreeItems: TreeSelectItem[]) {
        this.SuggestionsConfig.mentions.forEach(m => {
            let items = expressionsTreeItems
                .filter(i => i.selectable && !i.disabled && i.title.toString().toLowerCase().startsWith(m.triggerChar))
                .map(i => {
                    let type = "function";
                    let functionName = i.data.Name || "";
                    let functionFullName = functionName + (i.data.Body || "");
                    return {
                        code: type + "_" + i.key,
                        name: functionName,
                        label: functionFullName,
                        value: functionFullName,
                        type: type
                    };
                });
            m.items = m.items.concat(items);
        });
    }

    initializeOperators() {
        let operatorListService = new OperatorListService();
        let operatorCategoryListService = new OperatorCategoryListService();
        operatorCategoryListService.getAll().subscribe((serviceResponse: ServiceResponse) => {
            if (!serviceResponse.HasError) {
                this.OperatorCategories = serviceResponse.Result
                operatorListService.getAll().subscribe((serviceResponse: ServiceResponse) => {
                    if (!serviceResponse.HasError) {
                        this.Operators = serviceResponse.Result;
                        let operatorsTreeList = new OperatorsTreeList(this.OperatorCategories, this.Operators);
                        this.OperatorsTreeItems = operatorsTreeList.Items;
                        this.addOperatorsToSuggestionsConfig(operatorsTreeList.ItemsList);
                    }
                });
            }
        });
    }

    addOperatorsToSuggestionsConfig(operatorsTreeItems: TreeSelectItem[]) {
        this.SuggestionsConfig.mentions.forEach(m => {
            let items = operatorsTreeItems
                .filter(i => i.selectable && !i.disabled && i.data.Name && i.data.Name.toString().toLowerCase().startsWith(m.triggerChar))
                .map(i => {
                    let type = "operator";
                    return {
                        code: type + "_" + i.key,
                        name: i.data.Name,
                        label: i.data.Name,
                        value: i.data.Sign,
                        type: type
                    };
                });
            m.items = m.items.concat(items);
        });
    }

    initializeFlowVariablesTree() {
        let props = {
            ShowRecordsVariables: true,
            ShowDeclaredVariables: true,
            ShowRecordsCollectionVariables: true,
            ShowDeclaredCollectionVariables: true,
            ShowGlobalVariables: true,
            IsObjectVariableSelectable: true
        };
        this.FlowVariablesTreeList = new FlowVariablesTreeList(this.FlowObject, this.CurrentNodeId, props);
        this.FlowVariablesTreeItems = this.FlowVariablesTreeList.Items;
        this.addFlowVariablesToSuggestionsConfig(this.FlowVariablesTreeList.ItemsList);
    }

    addFlowVariablesToSuggestionsConfig(flowVariablesTreeItems: TreeSelectItem[]) {
        this.SuggestionsConfig.mentions.forEach(m => {
            let items = flowVariablesTreeItems
                .filter(i => i.selectable && !i.disabled && i.title.toString().toLowerCase().startsWith(m.triggerChar))
                .map(i => {
                    let type = "variable";
                    return {
                        code: type + "_" + i.key,
                        name: i.title,
                        label: i.key.indexOf("_") === -1 ? i.title : (i.key.split("_").slice(0, -1).join(" - ") + " - " + i.title),
                        value: "{" + i.key + "}",
                        type: type
                    };
                });
            m.items = items.concat(m.items);
        });
    }

    updateExpressionCategory(expressionCategory: ListItem) {
        this.ExpressionCategory = expressionCategory;
        this.ExpressionCategoryChanged = !this.ExpressionCategoryChanged;
    }

    updateExpression(expressionItem: TreeSelectItem) {
        let expression: ExpressionList = expressionItem?.data;
        if (expression) {
            this.updateExpressionValue(expression.Name + expression.Body);
            let expressionLength = this.ExpressionValue.expression.length;
            this.updateCursorPointer(expressionLength, expressionLength);
        }
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

    selectOperator(operator: any) {
        if (operator.data) {
            this.setOperatorInExpression(operator.data.Sign)
        }
    }

    setOperatorInExpression(operatorSign: string) {
        if (operatorSign) {
            let currentExpression = this.ExpressionValue.expression;
            if (currentExpression) {
                let newExpression = currentExpression.slice(0, this.CursorStartPoint) + operatorSign + currentExpression.slice(this.CursorEndPoint);
                this.updateExpressionValue(newExpression);
            } else {
                this.updateExpressionValue(operatorSign);
            }
        }
    }

    selectVariable(variable: string) {
        this.setVariableInExpression(variable);
    }

    setVariableInExpression(variable: string) {
        if (variable) {
            let variableWithBrackets = "{" + variable + "}";
            let currentExpression = this.ExpressionValue.expression;
            if (currentExpression) {
                let newExpression = currentExpression.slice(0, this.CursorStartPoint) + variableWithBrackets + currentExpression.slice(this.CursorEndPoint);
                this.updateExpressionValue(newExpression);
            } else {
                this.updateExpressionValue(variableWithBrackets);
            }
        }
    }

    updateExpressionValue(expression: string) {
        this.setIsSuggestionsPanelOpened();
        this.handleMentionListElementStyle();

        if (expression && expression !== "") {
            this.ExpressionValue.expression = expression;
        } else {
            this.ExpressionValue.expression = null;
        }
        this.ExpressionValue.variables = [];

        let expressionLength = this.ExpressionValue.expression ? this.ExpressionValue.expression.length : 0;
        this.updateCursorPointer(expressionLength, expressionLength);
    }

    setIsSuggestionsPanelOpened() {
        let mentionMenuElement = document.querySelector(".mention-menu");
        if (mentionMenuElement) {
            if (mentionMenuElement.getAttribute("hidden") === "") {
                this.IsSuggestionsPanelOpened = false;
            } else {
                this.IsSuggestionsPanelOpened = true;
            }
        } else {
            this.IsSuggestionsPanelOpened = false;
        }
    }

    handleMentionListElementStyle() {
        setTimeout(() => {
            let mentionListElement = document.querySelector<HTMLElement>("mention-list");
            if (mentionListElement) {
                let left = Number(mentionListElement.style.left.replace("px", ""));
                let top = Number(mentionListElement.style.top.replace("px", ""));
                if (left > 570) {
                    mentionListElement.style.left = "570px";
                }
                if (top > 85) {
                    mentionListElement.style.top = "85px";
                }
            }
        }, 50);
    }

    setIsSuggestionsPanelOpenedAsClosed() {
        this.IsSuggestionsPanelOpened = false;
    }

    checkExpressionSyntax() {
        console.log("true");
    }

    cancelButtonClicked() {
        this.closeExpressionBuilder();
    }

    saveButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.ExpressionValue && this.ExpressionValue.expression && this.ExpressionValue.expression !== "") {
            this.closeExpressionBuilder(true);
        }
        else {
            this.ValidationErrorsList.push("Expression is required");
        }
    }

    closeExpressionBuilder(isSaved: boolean = false) {
        if (isSaved) {
            this.IsSaving = true;
            setTimeout(() => {
                new Promise((resolve, _reject) => {
                    this.setExpressionVariables();
                    resolve(null);
                }).then(() => {
                    let dataToReturn: any = {
                        Action: "save",
                        ExpressionValue: this.ExpressionValue
                    };
                    this.CurrentSession.CurrentWindow.Close(dataToReturn);
                });
            }, 50);
        } else {
            let dataToReturn: any = {
                Action: "cancel",
                ExpressionValue: null
            };
            this.CurrentSession.CurrentWindow.Close(dataToReturn);
        }
    }

    setExpressionVariables() {
        let expression = this.ExpressionValue.expression;
        if (expression && expression !== "") {
            this.ExpressionValue.variables = [];
            let expressionVariables = expression.match(/\{(.*?)\}/g);
            if (expressionVariables && expressionVariables.length > 0) {
                expressionVariables.forEach(expressionVariable => {
                    if (expressionVariable && expressionVariable !== "") {
                        let variableCode = expressionVariable.replace("{", "").replace("}", "");
                        let variableItem = this.FlowVariablesTreeList.getItem(variableCode);
                        if (variableItem && this.ExpressionValue.variables.filter(v => v.code === variableCode).length === 0) {
                            this.ExpressionValue.variables.push(
                                {
                                    code: variableCode,
                                    type: ((variableItem.data && variableItem.data.type) ? variableItem.data.type : null),
                                    variableUsedFrom: ((variableItem.data && variableItem.data.nodeId) ? variableItem.data.nodeId : null)
                                }
                            );
                        }
                    }
                });
            }
        } else {
            this.ExpressionValue.variables = [];
        }
    }
}