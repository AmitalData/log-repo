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
var SessionLocator_1 = require("../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../Infrastructure/Tools");
var ObjectsLocator_1 = require("../Infrastructure/Locators/ObjectsLocator");
var SearchTextBox = /** @class */ (function () {
    function SearchTextBox() {
        this.searchText = null;
        this.SearchTextChangeEvent = new core_1.EventEmitter();
        this.TextChanged = new core_1.EventEmitter();
        this.LayoutDirection = 'ltr';
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isDisabled = false;
        if (this.CurrentSession == null) {
            this.SearchFieldsId = "SearchFieldsId_-1_-1";
        }
        else {
            this.SearchFieldsId = "SearchFieldsId_" + this.CurrentSession.GetNewId("SearchFieldsId");
        }
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        this.SetStyles();
    }
    Object.defineProperty(SearchTextBox.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            var _this = this;
            if (this.searchText != newValue) {
                this.searchText = !Tools_1.AppTool.IsNullOrEmpty(newValue) ? newValue : null;
                //this.SetControlDisplay();
                if (this.searchText != null) {
                    this.ClearPlaceHolder();
                }
                if (this.timerToken) {
                    clearTimeout(this.timerToken);
                }
                //if (AppTool.IsNullOrEmpty(newValue)) {
                //    this.OnDataLoaded();
                //}
                //else {
                this.timerToken = setTimeout(function () { return _this.LoadData(); }, 500);
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    SearchTextBox.prototype.LoadData = function () {
        this.SearchTextChangeEvent.emit(this.SearchText);
        this.TextChanged.emit(this.SearchText);
    };
    SearchTextBox.prototype.ngOnInit = function () {
        if (!this.PlaceHolder) {
            if (this.ObjectTableName == null) {
                this.PlaceHolder = "Search ...";
            }
            else {
                var ObjectTable = this.GetObjectTableName(this.ObjectTableName);
                var textCode = ObjectTable + ".F.SearchFields";
                var waterMark = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode);
                if (!Tools_1.AppTool.IsNullOrEmpty(waterMark)) {
                    this.PlaceHolder = waterMark;
                }
                else {
                    this.PlaceHolder = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Search");
                }
                //var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
                //this.ObjectTableId = ObjectTable.Id;
                //this.ObjectField = window.ObjectFields.filter(d => d.ObjectTableId === this.ObjectTableId && d.FieldName === "SearchFields")[0];
                //if (this.ObjectField) {
                //    if (this.ObjectField.FullNameTextCodeDefaultText) {
                //        this.PlaceHolder = this.ObjectField.FullNameTextCodeDefaultText;
                //    }
                //    else {
                //        this.PlaceHolder = "Search ...";
                //    }
                //}
            }
        }
        //this.SearchTextValue = new FormControl();
        //this.SearchTextValue.valueChanges
        //    .debounceTime(500)
        //    .distinctUntilChanged()
        //    .subscribe((search: string): any => {
        //        this.SearchText = search != "" ? search : null;
        //        this.SearchTextChangeEvent.emit(this.SearchText);
        //        this.TextChanged.emit(this.SearchText);
        //    });
    };
    Object.defineProperty(SearchTextBox.prototype, "IsDisabled", {
        get: function () { return this.isDisabled; },
        set: function (value) {
            if (this.isDisabled != value) {
                this.isDisabled = value;
                //if (value == true) {
                //    this.SearchText = null;
                //    this.IsControlFocused = false;
                //    this.SetControlDisplay();
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    SearchTextBox.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        if (temp) {
            temp.placeholder = "";
            temp.style.background = "rgba(0, 0, 0, 0)";
            temp.style.backgroundColor = "white";
        }
    };
    SearchTextBox.prototype.FillPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = this.PlaceHolder;
        if (!this.SearchText) {
            temp.style.background = "url(Images/Search.png) 6px 2px  no-repeat scroll";
            temp.style.backgroundColor = "white";
            temp.style.backgroundPosition = this.LayoutDirection == "rtl" ? '6px 2px' : "right center";
        }
        //temp.style.backgroundPosition = "right center";
        //temp.style.paddingRight = "30px";
        this.SetStyles();
    };
    SearchTextBox.prototype.OnDeleteValue = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.value = null;
        this.SearchText = null;
        temp.focus();
        this.SearchTextChangeEvent.emit("");
    };
    SearchTextBox.prototype.GetObjectTableName = function (theObjectTableName) {
        var cardTables = ["customer", "agent", "shippingagent", "customagent", "vendor", "airline", "trucker", "shippingline", "warehouse"];
        if (cardTables.indexOf(theObjectTableName.toLowerCase()) > -1) {
            return "Card";
        }
        else {
            return theObjectTableName;
        }
    };
    SearchTextBox.prototype.SetStyles = function () {
        if (this.LayoutDirection == "rtl") {
            this.textValueStyle = {
                'background-image': 'url(Images/Search.png)',
                'background-repeat': 'no-repeat',
                'background-color': 'white',
                'background-attachment': 'scroll',
                'background-position': '6px 2px',
                'padding-left': '30px',
                'font-style': 'italic',
            };
        }
        else {
            this.textValueStyle = {
                'background-image': 'url(Images/Search.png)',
                'background-repeat': 'no-repeat',
                'background-color': 'white',
                'background-attachment': 'scroll',
                'background-position': 'right center',
                'padding-right': '30px',
                'font-style': 'italic',
            };
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SearchTextBox.prototype, "SearchTextChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SearchTextBox.prototype, "TextChanged", void 0);
    SearchTextBox = __decorate([
        core_1.Component({
            selector: 'SearchTextBox',
            template: "<input type=\"text\" [disabled]=\"IsDisabled\" [id]=\"SearchFieldsId\" placeholder=\"{{PlaceHolder}}\" (focus)=\"ClearPlaceHolder();\" (blur)=\"FillPlaceHolder();\" [ngStyle]=\"textValueStyle\" [(ngModel)]=\"SearchText\" style=\"background: url(Images/Search.png) no-repeat scroll;background-color: white;background-position: right center;font-style: italic;\" [ngStyle]=\"LayoutDirection == 'rtl' ? {'padding-left': '30px'} : {'padding-right': '30px'}\" />\n               <img *ngIf=\"SearchText\" [className]=\"LayoutDirection == 'rtl' ? 'DeleteButton LeftCenter' : 'DeleteButton RightCenter'\" [ngStyle]=\"LayoutDirection == 'rtl' ? {'left': '15px'} : {'right': '15px'}\" src=\"Images/RedX.png\" (click)=\"OnDeleteValue()\" />\n              ",
            inputs: ['ObjectTableName', 'PlaceHolder', 'SearchText', 'IsDisabled'],
        }),
        __metadata("design:paramtypes", [])
    ], SearchTextBox);
    return SearchTextBox;
}());
exports.SearchTextBox = SearchTextBox;
//# sourceMappingURL=SearchTextBox.js.map