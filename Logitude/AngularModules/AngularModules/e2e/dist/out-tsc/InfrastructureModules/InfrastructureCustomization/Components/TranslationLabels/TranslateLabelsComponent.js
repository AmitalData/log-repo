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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var GeneralDomainService_1 = require("../../../../Infrastructure/Services/GeneralDomainService");
var TranslateLablesService_1 = require("../../../../Infrastructure/Services/TranslateLablesService");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var CachedDataManager_1 = require("../../../../Infrastructure/Utilities/CachedDataManager");
var TranslateLabelsComponent = /** @class */ (function (_super) {
    __extends(TranslateLabelsComponent, _super);
    function TranslateLabelsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.HasChanges = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.SkipDigit = 0;
        _this.TakeDigit = 20;
        _this.TextCodesCount = null;
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
        _this.PreviousButtonIsEnabled = false;
        _this.NextButtonIsEnabled = false;
        _this.ComponentId = "TranslateLabels_" + _this.CurrentSession.GetNewId("TranslateLabels");
        _this.TranslateLablesService = new TranslateLablesService_1.TranslateLablesService();
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.BuildFilters();
        return _this;
    }
    TranslateLabelsComponent.prototype.SetWindowArgs = function (args) {
        this.selectedLanguageCode = args;
        this.allTranslationsList = [];
        this.LoadAllTranslationMethod();
    };
    TranslateLabelsComponent.prototype.LoadAllTranslationMethod = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var myServiceHelper = new TranslateLablesService_1.TranslateLabelsAPIHelper();
        myServiceHelper.Language = this.selectedLanguageCode;
        myServiceHelper.ObjectTableId = this.ObjectTableId;
        myServiceHelper.SpellCheckedFilterCode = this.SpellCheckedSelectedItem == null ? null : this.SpellCheckedSelectedItem.Code;
        myServiceHelper.CheckDateFilerCode = this.DateCheckedSelectedItem == null ? null : this.DateCheckedSelectedItem.Code;
        myServiceHelper.TextCodeTypeCode = this.TextCodeTypeSelectedItem == null ? null : this.TextCodeTypeSelectedItem.Code;
        myServiceHelper.SelectedCheckDate = this.SelectedCheckDate;
        myServiceHelper.SearchText = this.SearchText;
        myServiceHelper.SkipDigit = this.SkipDigit;
        myServiceHelper.TakeDigit = this.TakeDigit;
        this.TranslateLablesService.Post(myServiceHelper).subscribe(function (myResult) {
            if (myResult.HasError) {
                _this.ValidationErrorsList = myResult.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                var list = myResult.Result;
                if (list != null) {
                    _this.allTranslationsList = list.Translations;
                    _this.TextCodesCount = list.CountAll;
                    _this.GetFilterdData();
                }
            }
        });
    };
    TranslateLabelsComponent.prototype.GetFilterdData = function () {
        if (this.SkipDigit < 0) {
            this.SkipDigit = 0;
        }
        if (this.TextCodesCount != null) {
            if (this.SkipDigit > this.TextCodesCount) {
                this.SkipDigit = this.TextCodesCount;
            }
        }
        this.SetButons();
        this.BuildData();
    };
    TranslateLabelsComponent.prototype.BuildData = function () {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        this.itemsCollection = [];
        this.filterdranslationsList = this.allTranslationsList;
        this.StartSearchFilter();
    };
    TranslateLabelsComponent.prototype.StartSearchFilter = function () {
        var _this = this;
        if (this.filterdranslationsList == null) {
            return;
        }
        this.filterdranslationsList.forEach(function (field) {
            _this.itemsCollection.push(new TranslateLabelsItem(field, _this));
        });
        this.ItemsSource.InsertCollection(this.itemsCollection);
        var resultStart = 0;
        if (this.TextCodesCount != null) {
            if (this.TextCodesCount > (this.SkipDigit + this.TakeDigit)) {
                resultStart = this.SkipDigit + this.TakeDigit;
            }
            else {
                resultStart = this.TextCodesCount;
            }
        }
        this.CountText = resultStart + " of " + this.TextCodesCount;
        this.CurrentSession.StopBusyIndicator();
        this.HasChanges = false;
    };
    TranslateLabelsComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    Object.defineProperty(TranslateLabelsComponent.prototype, "CountText", {
        get: function () { return this.countText; },
        set: function (value) {
            if (this.countText != value) {
                this.countText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TranslateLabelsComponent.prototype.OnSearchTextChanged = function (mySearchText) {
        this.SearchText = mySearchText;
        this.LoadAllTranslationMethod();
    };
    TranslateLabelsComponent.prototype.BuildFilters = function () {
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
        this.StartSearchFilter();
    };
    Object.defineProperty(TranslateLabelsComponent.prototype, "ObjectTableId", {
        get: function () { return this.objectTableId; },
        set: function (value) {
            if (this.objectTableId != value) {
                this.objectTableId = value;
                this.LoadAllTranslationMethod();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsComponent.prototype, "SpellCheckedSelectedItem", {
        get: function () { return this.spellCheckedSelectedItem; },
        set: function (value) {
            if (this.spellCheckedSelectedItem != value) {
                this.spellCheckedSelectedItem = value;
                this.LoadAllTranslationMethod();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsComponent.prototype, "DateCheckedSelectedItem", {
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
                this.LoadAllTranslationMethod();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsComponent.prototype, "TextCodeTypeSelectedItem", {
        get: function () { return this.textCodeTypeSelectedItem; },
        set: function (value) {
            if (this.textCodeTypeSelectedItem != value) {
                this.textCodeTypeSelectedItem = value;
                this.LoadAllTranslationMethod();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsComponent.prototype, "SelectedCheckDate", {
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
                this.LoadAllTranslationMethod();
            }
        },
        enumerable: true,
        configurable: true
    });
    TranslateLabelsComponent.prototype.SetButons = function () {
        this.PreviousButtonIsEnabled = this.SkipDigit > 0 ? true : false;
        this.NextButtonIsEnabled = (this.SkipDigit + this.TakeDigit) < this.TextCodesCount ? true : false;
    };
    TranslateLabelsComponent.prototype.SaveAndPreviousClicked = function () {
        if (this.HasChanges) {
            this.SkipDigit = this.SkipDigit - this.TakeDigit;
            this.SaveChanges();
        }
        else {
            this.SkipDigit = this.SkipDigit - this.TakeDigit;
            this.LoadAllTranslationMethod();
        }
    };
    TranslateLabelsComponent.prototype.SaveAndNextClicked = function () {
        if (this.HasChanges) {
            this.SkipDigit = this.SkipDigit + this.TakeDigit;
            this.SaveChanges();
        }
        else {
            this.SkipDigit = this.SkipDigit + this.TakeDigit;
            this.LoadAllTranslationMethod();
        }
    };
    TranslateLabelsComponent.prototype.CloseClicked = function () {
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
                    _this.SaveChanges(true);
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
    TranslateLabelsComponent.prototype.CloseWindow = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    TranslateLabelsComponent.prototype.SaveChanges = function (isClosing) {
        var _this = this;
        if (isClosing === void 0) { isClosing = false; }
        var list = this.GetDirtyFieldsTranslations();
        if (list.length > 0) {
            this.CurrentSession.StartBusyIndicatorSaving();
            var myServiceHelper = new GeneralDomainService_1.FieldsUpdateHelper();
            myServiceHelper.Tenant = SessionLocator_1.SessionLocator.Tenant;
            myServiceHelper.Items = list;
            var generalService = new GeneralDomainService_1.GeneralDomainService();
            generalService.UpdateFieldsTranslations(myServiceHelper).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    CachedDataManager_1.CachedDataManager.RefreshTenantTextCodes().subscribe(function (response) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                    });
                }
            });
        }
    };
    TranslateLabelsComponent.prototype.UpdateHasChanges = function () {
        var list = this.GetDirtyFieldsTranslations();
        if (list.length == 0) {
            this.HasChanges = false;
        }
        else {
            this.HasChanges = true;
        }
    };
    TranslateLabelsComponent.prototype.GetDirtyFieldsTranslations = function () {
        var myResult = [];
        this.ItemsSource.Collection.forEach(function (item) {
            if (item.fieldsTranslations.IsDirty) {
                myResult.push(item.fieldsTranslations);
            }
        });
        return myResult;
    };
    TranslateLabelsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TranslateLabelsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TranslateLabelsComponent);
    return TranslateLabelsComponent;
}(BaseComponent_1.BaseComponent));
exports.TranslateLabelsComponent = TranslateLabelsComponent;
var TranslateLabelsItem = /** @class */ (function (_super) {
    __extends(TranslateLabelsItem, _super);
    function TranslateLabelsItem(item, fatherComponent) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.CellBackgroundColor = "#E6E7E8";
        _this.fieldsTranslations = item;
        _this.isTranslatedOrigin = item.IsTranslated;
        _this.defaultTextOrigin = item.DefaultText;
        _this.translatedTextOrigin = item.TranslatedText;
        _this.translateDateOrigin = item.TranslateDate;
        _this.translatedByUserIdOrigin = item.TranslatedByUserId;
        _this.TranslatedBy = _this.translatedByUserIdOrigin;
        return _this;
    }
    Object.defineProperty(TranslateLabelsItem.prototype, "Code", {
        get: function () { return this.fieldsTranslations.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsItem.prototype, "DefaultText", {
        get: function () { return this.fieldsTranslations.DefaultText; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsItem.prototype, "TranslatedText", {
        get: function () { return this.fieldsTranslations.TranslatedText; },
        set: function (value) {
            if (this.fieldsTranslations.TranslatedText != value) {
                this.fieldsTranslations.TranslatedText = value;
                if (value == this.defaultTextOrigin) {
                    this.IsTranslated = false;
                    this.TranslateDate = null;
                    this.TranslatedBy = null;
                }
                else if (value == this.translatedTextOrigin) {
                    this.IsTranslated = this.isTranslatedOrigin;
                    this.TranslateDate = this.translateDateOrigin;
                    this.TranslatedBy = this.translatedByUserIdOrigin;
                }
                else {
                    this.IsTranslated = true;
                    this.TranslateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
                    this.TranslatedBy = SessionLocator_1.SessionLocator.LoggedUserId;
                }
                this.OnDataChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsItem.prototype, "IsTranslated", {
        get: function () { return this.fieldsTranslations.IsTranslated; },
        set: function (value) {
            if (this.fieldsTranslations.IsTranslated != value) {
                this.fieldsTranslations.IsTranslated = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsItem.prototype, "TranslateDate", {
        get: function () { return this.fieldsTranslations.TranslateDate; },
        set: function (value) {
            if (this.fieldsTranslations.TranslateDate != value) {
                this.fieldsTranslations.TranslateDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TranslateLabelsItem.prototype, "TranslatedBy", {
        get: function () { return this.translatedBy; },
        set: function (value) {
            this.fieldsTranslations.TranslatedByUserId = value;
            this.translatedBy = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(this.fieldsTranslations.TranslatedByUserId)) {
                //UserList user = UserDataProvider.GetCachedList<UserList>().Where(d => d.Id == fieldsTranslations.TranslatedByUserId).FirstOrDefault();
                //if (user != null) {
                //    translatedBy = user.EnglishName;
                //}
            }
        },
        enumerable: true,
        configurable: true
    });
    TranslateLabelsItem.prototype.OnDataChanged = function () {
        var isDirty = false;
        if (this.TranslatedText != this.translatedTextOrigin) {
            isDirty = true;
        }
        if (this.fieldsTranslations.IsDirty != isDirty) {
            this.fieldsTranslations.IsDirty = isDirty;
        }
        this.fatherComponent.UpdateHasChanges();
    };
    return TranslateLabelsItem;
}(BaseComponent_1.BaseComponent));
exports.TranslateLabelsItem = TranslateLabelsItem;
//# sourceMappingURL=TranslateLabelsComponent.js.map