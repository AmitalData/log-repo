import { ExpressionList } from "Workflow/EntityLists/ExpressionList";
import { TreeSelectItem } from "./TreeSelectItem";

export class ExpressionFunctionTreeList {
    private FunctionList: ExpressionList[];
    public Items: TreeSelectItem[] = [];

    constructor(functionList: ExpressionList[]) {
        this.initialize(functionList);
        this.initializeTreeItems();
    }

    private initialize(functionList) {
        this.FunctionList = functionList ? functionList : [];
    }

    private initializeTreeItems() {
        this.FunctionList.forEach((expressionFunction,index) => {
            let treeSelectItem = new TreeSelectItem(expressionFunction.Code, expressionFunction.Name + expressionFunction.Body, false, true, true, false, [], expressionFunction);
            this.Items.push(treeSelectItem);
        });
    }
}