export class TreeSelectItem {
    public key: string;
    public title: string;
    public icon: string;
    public isLeaf: boolean;
    public checked: boolean;
    public selected: boolean;
    public selectable: boolean;
    public disabled: boolean;
    public disableCheckbox: boolean;
    public expanded: boolean;
    public children: TreeSelectItem[];
    public data: any;

    constructor(
        key: string,
        title: string,
        isLeaf: boolean,
        selectable: boolean,
        expanded: boolean,
        disabled: boolean,
        children: TreeSelectItem[],
        data: any = {}
    ) {
        this.key = key || Date.now().toString();
        this.title = title || "Unknown";
        this.icon = null;
        this.isLeaf = isLeaf;
        this.checked = false;
        this.selected = false;
        this.selectable = selectable;
        this.disabled = disabled;
        this.disableCheckbox = false;
        this.expanded = expanded;
        this.children = children || [];
        this.data = data;
    }
}