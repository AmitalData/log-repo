"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var ObservableCollection = /** @class */ (function () {
    function ObservableCollection(ArrayData) {
        this.Changed = new core_1.EventEmitter();
        this.Length = 0;
        this.Collection = ArrayData;
        this.UpdateLength();
    }
    ObservableCollection.prototype.Insert = function (Item, FocusFirstCell) {
        if (FocusFirstCell === void 0) { FocusFirstCell = true; }
        this.Collection.push(Item);
        this.UpdateLength();
        this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell, Operation: "insert", Item: Item });
    };
    ObservableCollection.prototype.InsertCollection = function (myCollection, preventScroll) {
        if (preventScroll === void 0) { preventScroll = false; }
        if (myCollection) {
            this.Collection = myCollection;
            this.UpdateLength();
            this.Changed.emit({ IsCollection: true, PreventScroll: preventScroll });
        }
    };
    ObservableCollection.prototype.AppendCollection = function (myCollection) {
        var _this = this;
        if (myCollection) {
            myCollection.forEach(function (item) {
                _this.Collection.push(item);
            });
            this.UpdateLength();
            this.Changed.emit({ IsCollection: true });
        }
    };
    ObservableCollection.prototype.InsertAtIndex = function (Index, Item, FocusFirstCell) {
        if (FocusFirstCell === void 0) { FocusFirstCell = true; }
        this.Collection.splice(Index, 0, Item);
        this.UpdateLength();
        this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell, RowIndex: Index });
    };
    ObservableCollection.prototype.Update = function (OldItem, UpdatedItem, FocusFirstCell) {
        if (FocusFirstCell === void 0) { FocusFirstCell = true; }
        var index = this.Collection.indexOf(OldItem, 0);
        if (index > -1) {
            this.Collection[index] = UpdatedItem;
            this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell });
        }
    };
    ObservableCollection.prototype.UpdateWithIndex = function (index, UpdatedItem, FocusFirstCell) {
        if (FocusFirstCell === void 0) { FocusFirstCell = true; }
        if (index > -1) {
            this.Collection[index] = UpdatedItem;
            this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell });
        }
    };
    ObservableCollection.prototype.GetIndex = function (Item) {
        var index = this.Collection.indexOf(Item, 0);
        return index;
    };
    ObservableCollection.prototype.Remove = function (Item, FocusFirstCell) {
        if (FocusFirstCell === void 0) { FocusFirstCell = true; }
        var index = this.Collection.indexOf(Item, 0);
        if (index > -1) {
            this.Collection.splice(index, 1);
            this.UpdateLength();
            this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell, PreventScroll: true, Operation: "remove", Item: Item });
        }
    };
    ObservableCollection.prototype.RemoveFromIndex = function (index, FocusFirstCell) {
        if (FocusFirstCell === void 0) { FocusFirstCell = true; }
        // var index = this.Collection.indexOf(Item, 0);
        //if (index > -1) {
        this.Collection.splice(index, 1);
        this.UpdateLength();
        this.Changed.emit({ IsCollection: false, FocusFirstCell: FocusFirstCell });
        //}
    };
    ObservableCollection.prototype.Clear = function () {
        if (this.Collection) {
            this.Collection = [];
            this.UpdateLength();
            this.Changed.emit(this.Collection);
        }
    };
    ObservableCollection.prototype.UpdateLength = function () {
        var myResult = 0;
        if (this.Collection) {
            myResult = this.Collection.length;
        }
        this.Length = myResult;
    };
    return ObservableCollection;
}());
exports.ObservableCollection = ObservableCollection;
//# sourceMappingURL=ObservableCollection.js.map