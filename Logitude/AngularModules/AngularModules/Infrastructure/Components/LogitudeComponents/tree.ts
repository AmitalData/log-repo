import {Component, EventEmitter, Input, Output} from '@angular/core';

@Component({
  selector: 'slv-tree',
  styleUrls: ['./tree.css'],
  template: `
    <div class="sa-tree-view tree">
      <ul>
        <ng-container *ngTemplateOutlet="branch; context: {items: items}"></ng-container>
      </ul>
    </div>
   
    <!-- branch -->
    <ng-template let-items="items" #branch>
      <ng-container *ngFor="let item of items">
        <ng-container *ngTemplateOutlet="leaf; context: {item: item}"></ng-container>
      </ng-container>
    </ng-template>
   
    <!-- leaf -->
    <ng-template let-item="item" #leaf>
      <li [class.parent_li]="item?.children" [class.selected]="item?.selected">
        <span (click)="onClick(item)" [innerHtml]="item?.content" [ngStyle]="item?.children.length>0 ? {'background-color': '#c1c1c1'} : {'background-color': 'whitesmoke'}">
       
        </span>
        <img (click)="onClick(item)" *ngIf="item?.children?.length>0 && item.expanded" src="./Images/Icons/plus.png" style="margin-left:70px;padding:8px" />
        <img (click)="onClick(item)" *ngIf="item?.children?.length>0 && !item.expanded" src="./Images/Buttons/minus.png" style="margin-left:70px;padding:8px" />

        <ul *ngIf="item?.children" [class.hidden]="item.expanded"  >
        

          <ng-container *ngTemplateOutlet="branch; context: {items: item.children}"></ng-container>
        </ul>
      </li>
    </ng-template>
  `

})

export class TreeComponent {
  @Input() items: Object[];
  @Output() change = new EventEmitter<any>();
  
  onClick(item) {
    if(item.children.length>0)
      item.expanded = !item.expanded;
    this.change.emit(item);
  }

}
export class ISlvLeaf {
  content: string; // example: <span>Child</span>
  expanded: boolean;
  children?: Array<ISlvLeaf>;
}




export class Tree implements ISlvTree {

  treeData = [];
  selectedLeaf: Leaf = null;

  // hint: bind callback to component context (this)
  constructor(private clickCallback: {(data: Object, isVisited: boolean, leaf: Leaf)}) {}


  // ISlvTree's field
  get items() {
    
    return this.treeData;
  }

  onClick(leaf) {
    this.selectLeaf(leaf);
    this.clickCallback(leaf.data, leaf.visited, leaf);
    leaf.visited = true;
  }


  insertLeaves(leaves: Array<Leaf> | Leaf, parent: Leaf = null): void {

    // Note: NEED TO BUILD NEW OBJECT (REFERENCE) TO REDRAW(!!!) IT BY ANGULAR!!!
    // AFTER THAT NEW DOM WILL BE BUILT!!!
    // this.treeData = this.treeData.slice(0);
    
    // 1. add back link to a parent leaf and indexes of path to the leaf
    const initTreeLeaf = (leaf: Leaf, idx: number) => {
      if (parent) {
        leaf.parent = parent;
        leaf.indexes = parent.indexes.slice();
        leaf.indexes.push(idx);
      }
      else {
        leaf.indexes = [idx];
      }
    };

    if (leaves instanceof Leaf) {
      initTreeLeaf(leaves, 0);
    }
    else {
      (leaves as Array<Leaf>).forEach((leaf, idx) => initTreeLeaf(leaf, idx));
    }

    // 2. bind parent leaf with new provided children.
    // N-th level of leaves
    if (parent) {
      if (!parent.children) {
        parent.children = [];
      }
      parent.children = parent.children.concat(leaves);
    }
    // 1st level of leaves
    // Note: allows to append 1st level branches and to have it several
    else {
      this.treeData = this.treeData.concat(leaves);
    }

  }

  getSelectedLeafData() {
    const selLeaf = this.getSelectedLeaf();
    return selLeaf ? selLeaf.getData() : null;
  }

  setSelectedLeafData(data: any) {
    const selLeaf = this.getSelectedLeaf();
    if (!selLeaf) {
      return;
    }
    selLeaf.setData(data);
  }

  // indexes is path to a leaf
  getLeaf(indexes) {
    let leaf = this.treeData[indexes[0]];
    for (let i = 1, len = indexes.length; i < len; i++) {
      leaf = leaf.children[indexes[i]];
    }
    return leaf;
  }

  selectLeaf(leaf: Leaf) {
    if (this.selectedLeaf) {
      this.selectedLeaf.selected = false;
    }

    this.selectedLeaf = leaf;
    this.selectedLeaf.selected = true;
  }

  getSelectedLeaf() {
    return this.selectedLeaf;
  }

  getLeafPath(leaf) {
    const path = [];
    path.push(leaf.name);
    while (leaf.parent) {
      leaf = leaf.parent;
      path.push(leaf.name);
    }
    return path.reverse();
  }

  getSelectedLeafPath() {
    if (!this.selectedLeaf) {
      return '';
    }
    return this.getLeafPath(this.selectedLeaf);
  }
}


export class Leaf implements ISlvLeaf, ILeafHandle {

  // ISlvLeaf's fields
  content;
  expanded = false;
  children: Array<Leaf> = null;

  // ILeafHandle's fields. Initiated in insertLeaves of Tree
  parent: Leaf = null;
  indexes: Array<number> = null; // path to a leaf in a tree
  selected = false;
  visited = false;

  labelFieldName: string = null;

  constructor(labelOrFieldName: string, private data: Object, isParent?: boolean, icon?: string) {
    // A lief with / without children displayed differently.
    if (isParent) {
      this.children = new Array<Leaf>();
    }

    let content = '';
    if (icon) {
      content = `<i class='fa fa-fw fa-lg ${icon}'></i>&nbsp;`;
    }

    if (typeof data[labelOrFieldName] !== 'undefined') {
      content += data[labelOrFieldName];
      this.labelFieldName = labelOrFieldName;
    }
    else {
      content += labelOrFieldName;
    }

    this.content = content;
  }

  isEmpty() {
    return this.children.length === 0;
  }

  isParent() {
    return typeof this.children === 'object';
  }

  isOpen() {
    return this.expanded;
  }

  getLevel() {
    return this.indexes.length - 1;
  }

  getData() {
    return this.data;
  }

  setData(data) {
    if (this.labelFieldName) {
      this.content = this.content.replace(this.data[this.labelFieldName], data[this.labelFieldName]);
      // this.content = data[this.labelFieldName];
    }
    Object.assign(this.data, data);
  }

  getIndexesString() {
    return this.indexes.join(',');
  }
}
interface ISlvTree {
  items: Array<ISlvLeaf>;
  onClick: Function;
}


export interface ILeafHandle {
  parent: ILeafHandle;
  indexes: Array<number>; // path to a leaf in a tree on levels
  selected: boolean;
  visited: boolean;
}
