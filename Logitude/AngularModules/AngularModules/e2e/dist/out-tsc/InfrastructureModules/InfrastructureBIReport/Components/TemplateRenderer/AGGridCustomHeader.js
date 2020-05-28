"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var AGGridCustomHeader = /** @class */ (function () {
    function AGGridCustomHeader() {
        this.sortNumber = 0;
    }
    AGGridCustomHeader.prototype.agInit = function (params) {
        this.params = params;
        params.column.addEventListener('sortChanged', this.onSortChanged.bind(this));
        this.onSortChanged();
        this.colId = this.params.column.getColId().replace(/\s/g, '');
    };
    AGGridCustomHeader.prototype.refresh = function () {
        return false;
    };
    AGGridCustomHeader.prototype.onMenuClicked = function () {
        //this.params.showColumnMenu(this.menuButton.nativeElement);
    };
    ;
    AGGridCustomHeader.prototype.onSortChanged = function () {
        this.checkSortOrder();
        this.ascSort = this.descSort = this.noSort = 'inactive';
        if (this.params.column.isSortAscending()) {
            this.ascSort = 'active';
        }
        else if (this.params.column.isSortDescending()) {
            this.descSort = 'active';
        }
        else {
            this.noSort = 'active';
        }
    };
    /**
    * Check for sort Order
    */
    AGGridCustomHeader.prototype.checkSortOrder = function () {
        var _this = this;
        var sortingArray = this.params.api.sortController.getColumnsWithSortingOrdered();
        var j;
        if (sortingArray.length > 0) {
            setTimeout(function () {
                for (j = 0; j < sortingArray.length; j++) {
                    _this.sortNumber = j + 1;
                    var sortingDom = document.getElementById('sortingOrder' + sortingArray[j].colId.replace(/\s/g, ''));
                    sortingDom.innerHTML = _this.sortNumber.toString();
                }
            });
        }
    };
    AGGridCustomHeader.prototype.onSortRequested = function (order, event) {
        this.params.setSort(order, true);
        this.checkSortOrder();
    };
    __decorate([
        core_1.ViewChild('menuButton', { read: core_1.ElementRef }),
        __metadata("design:type", Object)
    ], AGGridCustomHeader.prototype, "menuButton", void 0);
    AGGridCustomHeader = __decorate([
        core_1.Component({
            selector: 'app-loading-overlay',
            template: "\n        <div class=\"MediaFill\">\n            <div *ngIf=\"params.enableMenu && params.menuIcon !='fa-list-ol'\" #menuButton class=\"customHeaderMenuButton\" (click)=\"onMenuClicked($event)\">\n                <i class=\"fa {{params.menuIcon}}\"></i>\n            </div>\n            <div *ngIf=\"params.enableMenu && params.menuIcon =='fa-list-ol'\" #menuButton class=\"customHeaderMenuButton\" (click)=\"onMenuClicked($event)\">\n                <img  src=\"./Images/numberIcon.png\" [className]=\"'LeftCenter'\"/>\n            </div>\n            <div class=\"customHeaderLabel TextTrimming\">{{params.displayName}}</div>\n            <div class=\"action-holders__sort-number action-holders__sorting\">\n                <p id=\"sortingOrder{{colId}}\"></p>\n            </div>\n            <div *ngIf=\"params.enableSorting\" (click)=\"onSortRequested('asc', $event)\" [ngClass]=\"ascSort\" class=\"customSortDownLabel\"><i class=\"fa fa-long-arrow-up\"></i></div> \n            <div *ngIf=\"params.enableSorting\" (click)=\"onSortRequested('desc', $event)\" [ngClass]=\"descSort\" class=\"customSortUpLabel\"><i class=\"fa fa-long-arrow-down\"></i></div> \n            <div *ngIf=\"params.enableSorting\" (click)=\"onSortRequested('', $event)\" [ngClass]=\"noSort\" class=\"customSortRemoveLabel\"><i class=\"fa fa-times\"></i></div>\n        </div>\n    ",
            styles: [
                '../_Resources/Froala/css/font-awesome.min.css',
                "\n        .action-holders__sort-number.action-holders__sorting {\n            position: relative;\n            float: left;\n            width: 7px;\n            height: 32px;\n            margin-right: 3px;\n            margin-left: 3px;\n        }\n        .customHeaderLabel {\n            float: left;\n            min-width: 70px;   \n            margin: 0 0 0 15px;\n         }\n        .customHeaderMenuButton, \n        .customSortDownLabel, \n        .customSortUpLabel, \n        .customSortRemoveLabel \n        {\n            float: left;\n            margin: 0 0 0 5px;\n        }\n            .customSortUpLabel {\n                margin: 0;\n            }\n\n            .customSortRemoveLabel {\n                font-size: 11px;\n            }\n\n            .active {\n                color: cornflowerblue;\n            }\n    "
            ]
        })
    ], AGGridCustomHeader);
    return AGGridCustomHeader;
}());
exports.AGGridCustomHeader = AGGridCustomHeader;
//# sourceMappingURL=AGGridCustomHeader.js.map