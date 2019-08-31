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
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var CommonDomainService_1 = require("../../../Common/Services/CommonDomainService");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var ClientListService_1 = require("../../../Customs/Services/StandardLists/ClientListService");
var ControlsIdCounter_1 = require("../../../Infrastructure/Utilities/ControlsIdCounter");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var KeyCode_1 = require("../../../Infrastructure/DataContracts/KeyCode");
var IdGeneratorPipe_1 = require("../../../Controls/Pipes/IdGeneratorPipe");
var SearchBox = /** @class */ (function () {
    function SearchBox() {
        this.Watermark = null;
        this.ObjectTableName = null;
        this.UseTimer = false;
        this.IsControlFocused = false;
        this.IsQuickSearch = false;
        this.IsQuickSearchOpened = false;
        this.IsQuickSearchLoading = false;
        this.IsQuickSearchNoResult = false;
        this.IsSearchIconVisible = false;
        this.IsDeleteIconVisible = false;
        this.AutoClearSearchText = false;
        this.DisplayMember = null;
        this.HideBox = false;
        this.HideIcons = false;
        this.ShowViewAll = false;
        this.DropDownHeight = 75;
        this.DropDownWidth = 300;
        this.ItemHeight = 25;
        this.Header = null;
        this.Background = "white";
        this.IsSimilarPartner = false;
        this.PartnerTypeId = null;
        this.myService = null;
        this.myCardListService = null;
        this.clientListService = null;
        this.QuickSearchItems = [];
        this.emitText = true;
        this.LayoutDirection = 'ltr';
        this.HasFilters = false;
        this.IsImporterDetails = false;
        this.TextChanged = new core_1.EventEmitter();
        this.ItemClicked = new core_1.EventEmitter();
        this.ViewAllClicked = new core_1.EventEmitter();
        this.LostFocus = new core_1.EventEmitter();
        this.ShowErrorPopup = false;
        this.isRTL = false;
        this.displayText = null;
        this.isDisabled = false;
        this.searchText = null;
        this.selectedItem = null;
        this.highlightedItem = null;
        this.selectFirst = false;
        this.SetControlDisplay();
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        var SearchBoxDivId_counter = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter("SearchBoxDivId");
        this.SearchBoxDivId = "SearchBoxDivId" + SearchBoxDivId_counter;
        var ErrorPopUpId_counter = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter("ErrorPopUpId");
        this.ErrorPopUpId = "ErrorPopUpId" + ErrorPopUpId_counter;
    }
    Object.defineProperty(SearchBox.prototype, "DisplayText", {
        get: function () { return this.displayText; },
        set: function (value) {
            if (this.displayText != value) {
                this.displayText = value;
                this.searchText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SearchBox.prototype, "IsDisabled", {
        get: function () { return this.isDisabled; },
        set: function (value) {
            if (this.isDisabled != value) {
                this.isDisabled = value;
                if (value == true) {
                    //this.SearchText = null;
                    this.IsControlFocused = false;
                    this.SetControlDisplay();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    SearchBox.prototype.ngOnInit = function () {
        var pipe = new IdGeneratorPipe_1.IdGeneratorPipe();
        this.NewId = pipe.transform(this.ObjectTableName + '_Search');
        if (this.Watermark == null) {
            var myWatermark = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Search");
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableName)) {
                var textCode = this.ObjectTableName + ".F.SearchFields";
                if (this.ObjectTableName == "CardGLAccount" || this.ObjectTableName == "CustomerGLAccount" || this.ObjectTableName == "VendorGLAccount") {
                    textCode = "GLAccount.F.SearchFields";
                }
                var waterMark = TextCodeTranslator_1.TextCodeTranslator.Translate(textCode);
                if (!Tools_1.AppTool.IsNullOrEmpty(waterMark)) {
                    myWatermark = waterMark;
                }
            }
            this.Watermark = myWatermark;
        }
        if (this.IsQuickSearch) {
            this.UseTimer = true;
            this.myService = new CommonDomainService_1.CommonDomainService();
            this.myCardListService = new CardListService_1.CardListService();
            this.clientListService = new ClientListService_1.ClientListService();
        }
    };
    SearchBox.prototype.OnFucos = function () {
        this.IsControlFocused = true;
        if (this.searchText)
            this.emitText = false;
        this.SetControlDisplay();
    };
    SearchBox.prototype.OnLostFucos = function () {
        this.LostFocus.emit(this.SearchText);
        if (this.AutoClearSearchText) {
            this.SearchText = null;
        }
        this.IsControlFocused = false;
        this.SetControlDisplay();
    };
    SearchBox.prototype.SetControlDisplay = function () {
        var isSearchIconVisible = false;
        var isDeleteIconVisible = false;
        var isQuickSearchOpened = false;
        if (Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
            if (!this.IsControlFocused) {
                isSearchIconVisible = true;
            }
            isQuickSearchOpened = false;
        }
        else {
            isDeleteIconVisible = true;
            if (this.IsQuickSearch) {
                if (this.HideBox) {
                    isQuickSearchOpened = true;
                }
                else {
                    if (this.IsControlFocused) {
                        isQuickSearchOpened = true;
                    }
                    else {
                        isQuickSearchOpened = false;
                    }
                }
            }
        }
        this.IsSearchIconVisible = isSearchIconVisible;
        this.IsDeleteIconVisible = isDeleteIconVisible;
        this.IsQuickSearchOpened = isQuickSearchOpened;
    };
    SearchBox.prototype.DeleteClicked = function () {
        this.SearchText = null;
    };
    Object.defineProperty(SearchBox.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
                this.SetControlDisplay();
                this.OnSearchTextChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    SearchBox.prototype.OnSearchTextChanged = function () {
        //console.log("# SearchTextChanged", this.SelectedItem);
        var _this = this;
        if (this.SelectedItem) {
            if (this.SelectedItem[this.DisplayMember] != this.SearchText) {
                this.SelectedItem = null;
                this.ItemClicked.emit(null);
            }
        }
        if (this.IsQuickSearch) {
            this.QuickSearchItems = [];
            this.DropDownHeight = 75;
            this.IsQuickSearchLoading = true;
            this.IsQuickSearchNoResult = false;
        }
        if (this.UseTimer) {
            if (this.timerToken) {
                clearTimeout(this.timerToken);
            }
            this.timerToken = setTimeout(function () { return _this.RunSearch(); }, 400);
        }
        else {
            this.RunSearch();
        }
    };
    SearchBox.prototype.RunSearch = function () {
        var _this = this;
        this.highlightedItemIndex = null;
        this.HighlightedItem = null;
        if (this.emitText == true) {
            this.TextChanged.emit(this.SearchText);
        }
        this.emitText = true;
        if (this.IsQuickSearch) {
            if (this.SearchText == "\"") {
                this.SearchText = null;
            }
            if (this.IsSimilarPartner && !Tools_1.AppTool.IsNullOrEmpty(this.PartnerTypeId)) {
                var filters = new ApiQueryFilters_1.ApiQueryFilters();
                filters.PageIndex = 0;
                filters.PageSize = 10;
                filters.SortBy = "EnglishName";
                filters.SortDirection = "Ascending";
                if (this.ObjectTableName == "Customer") {
                    filters.addAdditionalFilter("PartnerTypeId", "CS,PO", null, null, "InList", false, true, false, "string");
                }
                else {
                    filters.addAdditionalFilter("PartnerTypeId", this.PartnerTypeId, null, null, "Equals", false, false, false, "string");
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                    filters.addAdditionalFilter("EnglishName", this.SearchText, null, null, "Contains", false, false, false, "string");
                }
                this.myCardListService.getByFilters(filters).subscribe(function (myResponse) {
                    if (_this.MyCallTime == null || myResponse.CallTime > _this.MyCallTime) {
                        _this.MyCallTime = myResponse.CallTime;
                        _this.IsQuickSearchLoading = false;
                        _this.QuickSearchItems = [];
                        var itemsCount = 0;
                        if (myResponse != null) {
                            itemsCount = myResponse.Result.length;
                            _this.QuickSearchItems = myResponse.Result;
                            _this.highlightedItemIndex = 0;
                            _this.HighlightedItem = _this.QuickSearchItems[0];
                        }
                        _this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
                        _this.SetDropDownheight(itemsCount);
                    }
                });
            }
            else if (this.HasFilters && this.ObjectTableName == "Client") {
                var filters = new ApiQueryFilters_1.ApiQueryFilters();
                filters.PageIndex = 0;
                filters.PageSize = 10;
                filters.addAdditionalFilter("PassportNumber", "", null, null, "NotEqual", false, false, false, "string");
                filters.addAdditionalFilter("Code", "", null, null, "NotEqual", false, false, false, "string");
                filters.addAdditionalFilter("SearchFields", this.SearchText, null, null, "Contains", false, false, false, "string");
                this.clientListService.getByFilters(filters).subscribe(function (myResponse) {
                    _this.IsQuickSearchLoading = false;
                    _this.QuickSearchItems = [];
                    var itemsCount = 0;
                    if (myResponse != null) {
                        itemsCount = myResponse.Result.length;
                        _this.QuickSearchItems = myResponse.Result;
                        _this.highlightedItemIndex = 0;
                        _this.HighlightedItem = _this.QuickSearchItems[0];
                    }
                    _this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
                    _this.SetDropDownheight(itemsCount);
                });
            }
            else if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectTableName) && !Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                this.myService.GetQuickSearch(this.ObjectTableName, this.SearchText).subscribe(function (myResponse) {
                    if (_this.MyCallTime == null || myResponse.CallTime > _this.MyCallTime) {
                        _this.MyCallTime = myResponse.CallTime;
                        _this.IsQuickSearchLoading = false;
                        _this.QuickSearchItems = [];
                        var itemsCount = 0;
                        if (myResponse.Result != null) {
                            itemsCount = myResponse.Result.length;
                            _this.QuickSearchItems = myResponse.Result;
                            _this.highlightedItemIndex = 0;
                            _this.HighlightedItem = _this.QuickSearchItems[0];
                        }
                        _this.IsQuickSearchNoResult = itemsCount == 0 ? true : false;
                        _this.SetDropDownheight(itemsCount);
                    }
                });
            }
            else {
                this.QuickSearchItems = [];
            }
        }
    };
    SearchBox.prototype.SetDropDownheight = function (itemsCount) {
        if (this.MaxPopupItemsCount)
            itemsCount = this.MaxPopupItemsCount;
        var myHeight = 75;
        if (itemsCount > 0) {
            var itemsHeight = ((itemsCount * this.ItemHeight) + 2);
            if (itemsHeight > myHeight) {
                myHeight = itemsHeight;
            }
        }
        if (this.Header) {
            myHeight += 25;
        }
        if (this.ShowViewAll) {
            myHeight += 20;
        }
        this.DropDownHeight = myHeight;
    };
    SearchBox.prototype.ItemSelected = function (item) {
        this.SearchText = null;
        if (item != null) {
            this.SelectedItem = item;
            if (this.DisplayMember != null) {
                this.emitText = false; // disable emitting textChanged event when setting display member value
                this.SearchText = item[this.DisplayMember];
            }
        }
        this.ItemClicked.emit(item);
    };
    SearchBox.prototype.OnViewAllClicked = function () {
        this.ViewAllClicked.emit(true);
    };
    Object.defineProperty(SearchBox.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SearchBox.prototype, "HighlightedItem", {
        get: function () { return this.highlightedItem; },
        set: function (value) {
            if (this.highlightedItem != value) {
                this.highlightedItem = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SearchBox.prototype.OnSearchIputKeyDown = function (event) {
        //this.selectFirst = false;
        var TAB = 9;
        var ENTERKEY = 13;
        var DOWNKEY = 40;
        var UPKEY = 38;
        var ESC = 27;
        var CTRL = 17;
        var SHIFT = 16;
        //this.timerToken = setTimeout(()=>{
        //    if ($event.keyCode == TABKEY) {
        //        if (this.IsQuickSearchOpened) {
        //            this.ItemSelected(this.QuickSearchItems[0]);
        //        } else {
        //            this.selectFirst = true;
        //        }
        //    }
        //}, 300);
        var key = event.keyCode;
        var keyChar = event.key;
        switch (key) {
            case KeyCode_1.KeyCode.DOWN: // ↓
                {
                    // navigate to items
                    if (Tools_1.AppTool.IsNullOrEmpty(this.highlightedItemIndex))
                        this.highlightedItemIndex = 0;
                    else
                        this.highlightedItemIndex = ((this.highlightedItemIndex == this.QuickSearchItems.length - 1) ? 0 : this.highlightedItemIndex + 1); // increment index
                    this.HighlightedItem = this.QuickSearchItems[this.highlightedItemIndex];
                    break;
                }
            case KeyCode_1.KeyCode.UP: // ↑
                {
                    // navigate to items
                    this.highlightedItemIndex = ((this.highlightedItemIndex == 0) ? this.QuickSearchItems.length - 1 : this.highlightedItemIndex - 1); // decrement index
                    this.HighlightedItem = this.QuickSearchItems[this.highlightedItemIndex];
                    break;
                }
            case KeyCode_1.KeyCode.ENTER:
                {
                    this.ItemSelected(this.HighlightedItem);
                    this.IsQuickSearchOpened = false;
                    break;
                }
            case KeyCode_1.KeyCode.TAB:
                {
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText)) {
                        if (this.HighlightedItem) {
                            if (this.SelectedItem) {
                                if (this.SelectedItem != this.HighlightedItem)
                                    this.ItemSelected(this.HighlightedItem);
                            }
                            else
                                this.ItemSelected(this.HighlightedItem);
                        }
                        else {
                        }
                    }
                    break;
                }
            case KeyCode_1.KeyCode.ESCAPE:
                {
                    //if (this.IsOpened) {
                    //    this.CloseDropDown();
                    //}
                    break;
                }
            case KeyCode_1.KeyCode.BACK_SPACE:
            case KeyCode_1.KeyCode.DELETE:
                {
                    this.SelectedItem = null;
                    this.ItemClicked.emit(null);
                    break;
                }
            default:
                {
                    // Check key type
                    if (this.InputType) {
                        var result = this.CheckKey(key, keyChar);
                        if (key == SHIFT) {
                            //this.isShiftKeyDown = false;
                        }
                        if (key == CTRL) {
                            //this.isCtrlKeyDown = true;
                        }
                        if (result != null) {
                            return key;
                        }
                        else {
                            return false;
                        }
                    }
                    break;
                }
        }
    };
    SearchBox.prototype.CheckKey = function (key, keyChar) {
        var BACKSPACE = 8;
        var SHIFT = 16;
        var DASH = 189;
        var SUBTRACT = 109;
        var DELETE = 46;
        var HOME = 36;
        var END = 35;
        var PAGEUP = 33;
        var PAGEDOWN = 34;
        var LEFT = 37;
        var UP = 38;
        var RIGHT = 39;
        var DOWN = 40;
        var DECIMALPT = 110;
        var PERIOD = 190;
        var ADD = 107;
        var TAB = 9;
        var SPACEBAR = 32;
        var EQUALSIGN = 187;
        var GRAVEACCENT = 192;
        var BACKSLASH = 220;
        var CLOSEBRACKET = 221;
        var OPENBRACKET = 219;
        var SINGLEQOUTE = 222;
        var ENTER = 13;
        var FORWARDSLASH = 191;
        var COMMA = 188;
        var ESC = 27;
        var SEMICOLON = 186;
        var CTRL = 17;
        var EQUAL = 187;
        //(key >= 48 && key <= 57) ARE THE NUMBERS ON TOP || (key >= 96 && key <= 105) ARE THE NUMBERS ON NUMPAD
        //if (key == TAB) {
        //    this.keydown = false;
        //}
        //if (key == SHIFT) {
        //    this.keydown = false;//this.isShiftKeyDown = true; 
        //}
        //if (key == CTRL) {
        //    this.isCtrlKeyDown = true;
        //}
        //if (key == 67 || key == 65 || key == 86 || key == 88) {// ctrl+a,v,a,x
        //    if (this.isCtrlKeyDown) {
        //        return key;
        //    }
        //}
        if (this.InputType) {
            var numChars = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];
            switch (this.InputType.toLowerCase()) {
                case 'integertext':
                    {
                        if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == BACKSPACE || key == TAB || key == DELETE || key == END
                            || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == SUBTRACT || key == DASH) {
                            if (key == SUBTRACT || key == DASH) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchText) && this.SearchText.toString().indexOf('-') > -1) {
                                    return null;
                                }
                                else {
                                    var input = document.getElementById(this.NewId);
                                    if (input != null) {
                                        if (selectionStart(input) == 0) {
                                            return key;
                                        }
                                        else {
                                            return null;
                                        }
                                    }
                                }
                            }
                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            return key;
                        }
                        return null;
                    }
                default:
                    return key;
            }
        }
        else {
            return key;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SearchBox.prototype, "TextChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SearchBox.prototype, "ItemClicked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SearchBox.prototype, "ViewAllClicked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], SearchBox.prototype, "LostFocus", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], SearchBox.prototype, "ErrorMessage", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], SearchBox.prototype, "MaxPopupItemsCount", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], SearchBox.prototype, "InputType", void 0);
    SearchBox = __decorate([
        core_1.Component({
            selector: "SearchBox",
            moduleId: module.id,
            templateUrl: './SearchBox.html',
            inputs: [
                'Watermark',
                'ObjectTableName',
                'UseTimer',
                'IsQuickSearch',
                'DisplayMember',
                'DisplayText',
                'HideIcons',
                'IsDisabled',
                'ShowViewAll',
                'AutoClearSearchText',
                'ItemHeight',
                'DropDownWidth',
                'IsSimilarPartner',
                'PartnerTypeId',
                'Background',
                'HideBox',
                'SearchText',
                'Header',
                'HasFilters',
                'IsImporterDetails',
            ],
        }),
        __metadata("design:paramtypes", [])
    ], SearchBox);
    return SearchBox;
}());
exports.SearchBox = SearchBox;
//# sourceMappingURL=SearchBox.js.map