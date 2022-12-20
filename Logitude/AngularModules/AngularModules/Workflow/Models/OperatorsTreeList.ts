import { OperatorCategoryList } from "Workflow/EntityLists/OperatorCategoryList";
import { OperatorList } from "Workflow/EntityLists/OperatorList";
import { TreeSelectItem } from "./TreeSelectItem";

export class OperatorsTreeList {
    private OperatorCategories: OperatorCategoryList[];
    private Operators: OperatorList[];

    public Items: TreeSelectItem[] = [];


    constructor(operatorCategories: OperatorCategoryList[], operators: OperatorList[]) {
        this.initialize(operatorCategories, operators);
        this.initializeTreeItems();
    }

    private initialize(operatorCategories, operators) {
        this.OperatorCategories = operatorCategories;
        this.Operators = operators;
    }

    private initializeTreeItems() {
        this.initializeOperatorTreeSelectItems();
    }

    private initializeOperatorTreeSelectItems() {
        this.OperatorCategories.forEach((operatorCategory) => {
            let childrenItems = this.getOperatorTreeSelectItemChildren(operatorCategory.Code);
            let treeSelectItem = new TreeSelectItem(operatorCategory.Code, operatorCategory.Name, false, false, false, false, childrenItems);
            this.Items.push(treeSelectItem);
        })
    }

    private getOperatorTreeSelectItemChildren(operatoryCategoryCode: string) {
        let OperatorTreeSelectItemChildren = [];
        this.Operators.filter(o => o.CategoryCode == operatoryCategoryCode).forEach(operator => {
            let treeSelectItem = new TreeSelectItem(operator.Code, operator.Sign + ' ' + operator.Name, true, true, false, false, [], operator);
            OperatorTreeSelectItemChildren.push(treeSelectItem);
        })
        return OperatorTreeSelectItemChildren;
    }
}