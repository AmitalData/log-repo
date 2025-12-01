import {EventEmitter} from '@angular/core';


export class ObservableCollection {
    public Collection: any[];
    public Changed = new EventEmitter();
    constructor(ArrayData: any[]) {
        this.Collection = ArrayData;
        this.UpdateLength();
    }

    public Insert(Item: any, FocusFirstCell: boolean = true) {
        this.Collection.push(Item);
        this.UpdateLength();
        this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell, Operation: "insert", Item: Item });
    }

    public InsertCollection(myCollection: any[], preventScroll: boolean = false) {
        if (myCollection) {
            this.Collection = myCollection;
            this.UpdateLength();
            this.Changed.emit({ IsCollection: true, PreventScroll: preventScroll, Items: myCollection});
        }
    }

    public AppendCollection(myCollection: any[]) {
        if (myCollection) {

            myCollection.forEach(item => {
                this.Collection.push(item);
            });

            this.UpdateLength();
            this.Changed.emit({ IsCollection: true });
        }
    }

    public InsertAtIndex(Index: number, Item: any, FocusFirstCell: boolean = true) {
        this.Collection.splice(Index, 0, Item);
        this.UpdateLength();
        this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell, RowIndex: Index });
    }

    public Update(OldItem: any, UpdatedItem: any, FocusFirstCell: boolean = true) {
        var index = this.Collection.indexOf(OldItem, 0);
        if (index > -1) {
            this.Collection[index] = UpdatedItem;
            this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell });
        }
    }

    public UpdateWithIndex(index: number, UpdatedItem: any, FocusFirstCell: boolean = true) {
        if (index > -1) {
            this.Collection[index] = UpdatedItem;
            this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell });
        }
    }

    public GetIndex(Item: any) {
        var index = this.Collection.indexOf(Item, 0);
        return index;
    }

    public Remove(Item: any, FocusFirstCell: boolean = true) {
        var index = this.Collection.indexOf(Item, 0);
        if (index > -1) {
            this.Collection.splice(index, 1);
            this.UpdateLength();
            this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell, PreventScroll: true, Operation: "remove", Item: Item });
        }
    }

    public RemoveFromIndex(index: number, FocusFirstCell: boolean = true) {
        // var index = this.Collection.indexOf(Item, 0);
        //if (index > -1) {
        this.Collection.splice(index, 1);
        this.UpdateLength();
        this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell });
        //}
    }

    public Clear() {
        if (this.Collection) {
            this.Collection = [];
            this.UpdateLength();
            this.Changed.emit(this.Collection);
        }
    }

    public Length: number = 0;
    private UpdateLength() {
        var myResult = 0;

        if (this.Collection) {
            myResult = this.Collection.length;
        }

        this.Length = myResult;
    }
    public FindIndex(code: string) {
        var index =  this.Collection.findIndex(x => x.code === code) ;
        return index
    }

}
