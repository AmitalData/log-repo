import { ExpressionList } from "Workflow/EntityLists/ExpressionList";
import { TreeSelectItem } from "Workflow/Models/TreeSelectItem";

export class ExpressionsTreeList {
    private Expressions: ExpressionList[];
    public Items: TreeSelectItem[] = [];

    constructor(expressions: ExpressionList[]) {
        this.initialize(expressions);
        this.initializeTreeItems();
    }

    private initialize(expressions: ExpressionList[]) {
        this.Expressions = expressions ? expressions : [];
    }

    private initializeTreeItems() {
        this.Expressions.forEach((expression) => {
            let treeSelectItem = new TreeSelectItem(expression.Code, expression.Title, false, true, true, false, [], expression);
            this.Items.push(treeSelectItem);
        });
    }
}