"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DefaultTranslationService_1 = require("../../../../Infrastructure/Services/DefaultTranslationService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var DefaultTranslationComponent = /** @class */ (function (_super) {
    __extends(DefaultTranslationComponent, _super);
    function DefaultTranslationComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.HasChanges = false;
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SelectedRow = null;
        _this.SearchText = null;
        _this.SpellCheckedList = [];
        _this.DateCheckedList = [];
        _this.TextCodeTypesList = [];
        _this.objectTableId = null;
        _this.spellCheckedSelectedItem = null;
        _this.dateCheckedSelectedItem = null;
        _this.textCodeTypeSelectedItem = null;
        _this.selectedCheckDate = null;
        _this.SkipDigit = 0;
        _this.TakeDigit = 20;
        _this.TextCodesCount = null;
        _this.StartPagerDigit = null;
        _this.SearchResultCount = 0;
        _this.PageItemsCount = 0;
        _this.PreviousButtonIsEnabled = false;
        _this.NextButtonIsEnabled = false;
        _this.ComponentId = "DefaultTranslation_" + _this.CurrentSession.GetNewId("DefaultTranslation");
        _this.myService = new DefaultTranslationService_1.DefaultTranslationService();
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.BuildFilters();
        return _this;
    }
    DefaultTranslationComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    DefaultTranslationComponent.prototype.OnSearchTextChanged = function (mySearchText) {
        this.SearchText = mySearchText;
    };
    DefaultTranslationComponent.prototype.SearchButtonClicked = function () {
        this.StartNewSearch();
    };
    DefaultTranslationComponent.prototype.BuildFilters = function () {
        this.SpellCheckedList = [];
        this.SpellCheckedList.push(new CodeNameClass_1.CodeNameClass("None", "None"));
        this.SpellCheckedList.push(new CodeNameClass_1.CodeNameClass("True", "Yes"));
        this.SpellCheckedList.push(new CodeNameClass_1.CodeNameClass("False", "No"));
        this.spellCheckedSelectedItem = this.SpellCheckedList[0];
        this.DateCheckedList = [];
        this.DateCheckedList.push(new CodeNameClass_1.CodeNameClass("None", "None"));
        this.DateCheckedList.push(new CodeNameClass_1.CodeNameClass("Equals", "Equals"));
        this.DateCheckedList.push(new CodeNameClass_1.CodeNameClass("Bigger", "Bigger"));
        this.DateCheckedList.push(new CodeNameClass_1.CodeNameClass("Less", "Less"));
        this.dateCheckedSelectedItem = this.DateCheckedList[0];
        this.TextCodeTypesList = [];
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("All", "All"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("F", "Fields"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("CH", "Column Headers"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("B", "Buttons And Actions"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("H", "Help Text"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("M", "Messages"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("MH", "Menu Headers"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("MC", "Maintenance"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("L", "Links"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("O", "Others"));
        this.TextCodeTypesList.push(new CodeNameClass_1.CodeNameClass("G", "General"));
        this.textCodeTypeSelectedItem = this.TextCodeTypesList[0];
        this.StartNewSearch();
    };
    Object.defineProperty(DefaultTranslationComponent.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (value) {
            if (this.objectTableId != value) {
                this.objectTableId = value;
                this.StartNewSearch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationComponent.prototype, "SpellCheckedSelectedItem", {
        get: function () { return this.spellCheckedSelectedItem; },
        set: function (value) {
            if (this.spellCheckedSelectedItem != value) {
                this.spellCheckedSelectedItem = value;
                this.StartNewSearch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationComponent.prototype, "DateCheckedSelectedItem", {
        get: function () { return this.dateCheckedSelectedItem; },
        set: function (value) {
            if (this.dateCheckedSelectedItem != value) {
                this.dateCheckedSelectedItem = value;
                if (value == null) {
                    this.selectedCheckDate = null;
                }
                else if (value.Code == "None") {
                    this.selectedCheckDate = null;
                }
                else {
                    this.selectedCheckDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                }
                this.StartNewSearch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationComponent.prototype, "TextCodeTypeSelectedItem", {
        get: function () { return this.textCodeTypeSelectedItem; },
        set: function (value) {
            if (this.textCodeTypeSelectedItem != value) {
                this.textCodeTypeSelectedItem = value;
                this.StartNewSearch();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationComponent.prototype, "SelectedCheckDate", {
        get: function () { return this.selectedCheckDate; },
        set: function (value) {
            if (this.selectedCheckDate != value) {
                this.selectedCheckDate = value;
                if (value == null) {
                    this.dateCheckedSelectedItem = this.DateCheckedList.filter(function (d) { return d.Code == "None"; })[0];
                }
                else if (this.dateCheckedSelectedItem == null) {
                    this.dateCheckedSelectedItem = this.DateCheckedList.filter(function (d) { return d.Code == "Equals"; })[0];
                }
                else if (this.dateCheckedSelectedItem.Code == "None") {
                    this.dateCheckedSelectedItem = this.DateCheckedList.filter(function (d) { return d.Code == "Equals"; })[0];
                }
                this.StartNewSearch();
            }
        },
        enumerable: true,
        configurable: true
    });
    DefaultTranslationComponent.prototype.StartNewSearch = function () {
        this.SkipDigit = 0;
        this.LoadTextCodes(true);
    };
    DefaultTranslationComponent.prototype.LoadTextCodes = function (isNewSearching) {
        var _this = this;
        if (isNewSearching === void 0) { isNewSearching = false; }
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource.Clear();
        if (this.SkipDigit < 0) {
            this.SkipDigit = 0;
        }
        if (this.SkipDigit > this.SearchResultCount) {
            this.SkipDigit = this.SearchResultCount;
        }
        var myServiceHelper = new DefaultTranslationService_1.DefaultTranslationAPIHelper();
        myServiceHelper.ObjectTableId = this.ObjectTableId;
        myServiceHelper.SpellCheckedFilterCode = this.SpellCheckedSelectedItem == null ? null : this.SpellCheckedSelectedItem.Code;
        myServiceHelper.CheckDateFilerCode = this.DateCheckedSelectedItem == null ? null : this.DateCheckedSelectedItem.Code;
        myServiceHelper.TextCodeTypeCode = this.TextCodeTypeSelectedItem == null ? null : this.TextCodeTypeSelectedItem.Code;
        myServiceHelper.SelectedCheckDate = this.SelectedCheckDate;
        myServiceHelper.SearchText = this.SearchText;
        myServiceHelper.SkipDigit = this.SkipDigit;
        myServiceHelper.TakeDigit = this.TakeDigit;
        myServiceHelper.IsNewSearching = isNewSearching;
        if (!isNewSearching) {
            myServiceHelper.Count = this.SearchResultCount;
        }
        this.myService.Post(myServiceHelper).subscribe(function (myResponse) {
            _this.ItemsSource.Clear();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.OnDataLoaded(myResponse.Result);
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DefaultTranslationComponent.prototype.SaveChanges = function (isClosing) {
        var _this = this;
        if (isClosing === void 0) { isClosing = false; }
        this.CurrentSession.StartBusyIndicatorSaving();
        if (this.SkipDigit < 0) {
            this.SkipDigit = 0;
        }
        if (this.SkipDigit > this.SearchResultCount) {
            this.SkipDigit = this.SearchResultCount;
        }
        var myServiceHelper = new DefaultTranslationService_1.DefaultTranslationAPIHelper();
        myServiceHelper.ObjectTableId = this.ObjectTableId;
        myServiceHelper.SpellCheckedFilterCode = this.SpellCheckedSelectedItem == null ? null : this.SpellCheckedSelectedItem.Code;
        myServiceHelper.CheckDateFilerCode = this.DateCheckedSelectedItem == null ? null : this.DateCheckedSelectedItem.Code;
        myServiceHelper.TextCodeTypeCode = this.TextCodeTypeSelectedItem == null ? null : this.TextCodeTypeSelectedItem.Code;
        myServiceHelper.SelectedCheckDate = this.SelectedCheckDate;
        myServiceHelper.SearchText = this.SearchText;
        myServiceHelper.SkipDigit = this.SkipDigit;
        myServiceHelper.TakeDigit = this.TakeDigit;
        myServiceHelper.IsNewSearching = false;
        myServiceHelper.Count = this.SearchResultCount;
        myServiceHelper.UpdatedTextCodes = this.GetDirtyTextCodes();
        if (isClosing) {
            myServiceHelper.IsUpdatingOnly = true;
        }
        this.myService.Post(myServiceHelper).subscribe(function (myResponse) {
            _this.ItemsSource.Clear();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                if (isClosing) {
                    _this.CloseWindow();
                }
                else {
                    _this.OnDataLoaded(myResponse.Result);
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DefaultTranslationComponent.prototype.OnDataLoaded = function (myResultHelper) {
        var _this = this;
        if (myResultHelper) {
            this.SearchResultCount = myResultHelper.Count;
            var loadedItems = myResultHelper.TextCodes;
            var itemsCollection = [];
            loadedItems.forEach(function (item) {
                itemsCollection.push(new DefaultTranslationItem(item, _this));
            });
            this.ItemsSource.InsertCollection(itemsCollection);
        }
        var myPageItemsCount = 0;
        if (this.SearchResultCount) {
            if (this.SearchResultCount > (this.SkipDigit + this.TakeDigit)) {
                myPageItemsCount = this.SkipDigit + this.TakeDigit;
            }
            else {
                myPageItemsCount = this.SearchResultCount;
            }
        }
        this.PageItemsCount = myPageItemsCount;
        this.SetButons();
        this.HasChanges = false;
    };
    DefaultTranslationComponent.prototype.SetButons = function () {
        this.PreviousButtonIsEnabled = this.SkipDigit > 0 ? true : false;
        this.NextButtonIsEnabled = (this.SkipDigit + this.TakeDigit) < this.SearchResultCount ? true : false;
    };
    DefaultTranslationComponent.prototype.SaveAndPreviousClicked = function () {
        if (this.HasChanges) {
            var isDataValid = this.ValidateData();
            if (isDataValid) {
                this.SkipDigit = this.SkipDigit - this.TakeDigit;
                this.SaveChanges();
            }
        }
        else {
            this.SkipDigit = this.SkipDigit - this.TakeDigit;
            this.LoadTextCodes();
        }
    };
    DefaultTranslationComponent.prototype.SaveAndNextClicked = function () {
        if (this.HasChanges) {
            var isDataValid = this.ValidateData();
            if (isDataValid) {
                this.SkipDigit = this.SkipDigit + this.TakeDigit;
                this.SaveChanges();
            }
        }
        else {
            this.SkipDigit = this.SkipDigit + this.TakeDigit;
            this.LoadTextCodes();
        }
    };
    DefaultTranslationComponent.prototype.CloseClicked = function () {
        var _this = this;
        if (this.HasChanges) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Width = 450;
            confirmWindow.Height = 190;
            confirmWindow.ShowCancelButton = true;
            confirmWindow.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.DontSave");
            confirmWindow.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
            confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.UnSavedChanges");
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.ThisEntityhasunsavedchanges").replace("%Entity", "Data"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    var isDataValid = _this.ValidateData();
                    if (isDataValid) {
                        _this.SaveChanges(true);
                    }
                }
                else if (confirmWindow.No) {
                    _this.CloseWindow();
                }
            });
        }
        else {
            this.CloseWindow();
        }
    };
    DefaultTranslationComponent.prototype.CloseWindow = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DefaultTranslationComponent.prototype.GetDirtyTextCodes = function () {
        var myResult = [];
        this.ItemsSource.Collection.forEach(function (item) {
            if (item.EntityPM.IsDirty) {
                myResult.push(item.EntityPM);
            }
        });
        return myResult;
    };
    DefaultTranslationComponent.prototype.ValidateData = function () {
        var myResult = true;
        var errors = [];
        var list = this.GetDirtyTextCodes();
        list.forEach(function (item) {
            Validator_1.Validator.TryValidateObject(item, "TextCode", errors);
        });
        this.ValidationErrorsList = errors;
        if (errors.length > 0) {
            myResult = false;
        }
        return myResult;
    };
    DefaultTranslationComponent.prototype.UpdateHasChanges = function () {
        var list = this.GetDirtyTextCodes();
        if (list.length == 0) {
            this.HasChanges = false;
        }
        else {
            this.HasChanges = true;
        }
    };
    DefaultTranslationComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DefaultTranslationComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DefaultTranslationComponent);
    return DefaultTranslationComponent;
}(BaseComponent_1.BaseComponent));
exports.DefaultTranslationComponent = DefaultTranslationComponent;
var DefaultTranslationItem = /** @class */ (function (_super) {
    __extends(DefaultTranslationItem, _super);
    function DefaultTranslationItem(item, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.EntityPM = null;
        _this.ObjectTableName = "TextCode";
        _this.EntityPM = item;
        _this.CloneData();
        return _this;
    }
    DefaultTranslationItem.prototype.CloneData = function () {
        this.DefaultText_Cloned = this.EntityPM.DefaultText;
        this.DefaultTextPlural_Cloned = this.EntityPM.DefaultTextPlural;
        this.LocalDefaultText_Cloned = this.EntityPM.LocalDefaultText;
        this.IsSpellChecked_Cloned = this.EntityPM.IsSpellChecked;
        this.SpellCheckDate_Cloned = this.EntityPM.SpellCheckDate;
        this.SpellCheckedByUserId_Cloned = this.EntityPM.SpellCheckedByUserId;
        this.SpellCheckedByUserName_Cloned = this.EntityPM.SpellCheckedByUserName;
    };
    DefaultTranslationItem.prototype.OnDataChanged = function () {
        var isDirty = false;
        if (this.DefaultText != this.DefaultText_Cloned) {
            isDirty = true;
        }
        else if (this.DefaultTextPlural != this.DefaultTextPlural_Cloned) {
            isDirty = true;
        }
        else if (this.LocalDefaultText != this.LocalDefaultText_Cloned) {
            isDirty = true;
        }
        else if (this.IsSpellChecked != this.IsSpellChecked_Cloned) {
            isDirty = true;
        }
        if (isDirty == false) {
            if (this.SpellCheckDate != this.SpellCheckDate_Cloned) {
                this.SpellCheckDate = this.SpellCheckDate_Cloned;
            }
            if (this.SpellCheckedByUserId != this.SpellCheckedByUserId_Cloned) {
                this.SpellCheckedByUserId = this.SpellCheckedByUserId_Cloned;
            }
            if (this.SpellCheckedByUserName != this.SpellCheckedByUserName_Cloned) {
                this.SpellCheckedByUserName = this.SpellCheckedByUserName_Cloned;
            }
        }
        if (this.EntityPM.IsDirty != isDirty) {
            this.EntityPM.IsDirty = isDirty;
        }
        this.fatherComponent.UpdateHasChanges();
    };
    Object.defineProperty(DefaultTranslationItem.prototype, "Code", {
        get: function () { return this.EntityPM.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationItem.prototype, "DefaultText", {
        get: function () { return this.EntityPM.DefaultText; },
        set: function (value) {
            if (this.EntityPM.DefaultText != value) {
                this.EntityPM.DefaultText = value;
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationItem.prototype, "DefaultTextPlural", {
        get: function () { return this.EntityPM.DefaultTextPlural; },
        set: function (value) {
            if (this.EntityPM.DefaultTextPlural != value) {
                this.EntityPM.DefaultTextPlural = value;
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationItem.prototype, "LocalDefaultText", {
        get: function () { return this.EntityPM.LocalDefaultText; },
        set: function (value) {
            if (this.EntityPM.LocalDefaultText != value) {
                this.EntityPM.LocalDefaultText = value;
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationItem.prototype, "IsSpellChecked", {
        get: function () { return this.EntityPM.IsSpellChecked; },
        set: function (value) {
            if (this.EntityPM.IsSpellChecked != value) {
                this.EntityPM.IsSpellChecked = value;
                if (value) {
                    this.SpellCheckDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    this.SpellCheckedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                    this.SpellCheckedByUserName = SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName;
                }
                else {
                    this.SpellCheckDate = null;
                    this.SpellCheckedByUserId = null;
                    this.SpellCheckedByUserName = null;
                }
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationItem.prototype, "SpellCheckDate", {
        get: function () { return this.EntityPM.SpellCheckDate; },
        set: function (value) {
            if (this.EntityPM.SpellCheckDate != value) {
                this.EntityPM.SpellCheckDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationItem.prototype, "SpellCheckedByUserId", {
        get: function () { return this.EntityPM.SpellCheckedByUserId; },
        set: function (value) {
            if (this.EntityPM.SpellCheckedByUserId != value) {
                this.EntityPM.SpellCheckedByUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DefaultTranslationItem.prototype, "SpellCheckedByUserName", {
        get: function () { return this.EntityPM.SpellCheckedByUserName; },
        set: function (value) {
            if (this.EntityPM.SpellCheckedByUserName != value) {
                this.EntityPM.SpellCheckedByUserName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return DefaultTranslationItem;
}(BaseComponent_1.BaseComponent));
exports.DefaultTranslationItem = DefaultTranslationItem;
//# sourceMappingURL=DefaultTranslationComponent.js.map