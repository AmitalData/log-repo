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
var DWObjectTablePMService_1 = require("../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService");
var DWObjectFieldExtendedPMService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService");
var DWQueryBuilderService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DWQueryData_1 = require("../../../../Common/DataContracts/DWQueryData");
var DWSubQueryPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService");
var DWSubQueryPM_1 = require("../../../../Infrastructure/EntityPMs/DWSubQueryPM");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var DWObjectTableListService_1 = require("../../../../Infrastructure/Services/StandardLists/DWObjectTableListService");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var DWQueryPMService_1 = require("../../../../Infrastructure/Services/StandardPMs/DWQueryPMService");
var DWQueryBuilderHelper_1 = require("../../../../Infrastructure/Helpers/DWQueryBuilderHelper");
var DWQueryBuilderComponent = /** @class */ (function (_super) {
    __extends(DWQueryBuilderComponent, _super);
    function DWQueryBuilderComponent(CD) {
        var _this = _super.call(this) || this;
        _this.CD = CD;
        _this.RTL = false;
        _this.BooleanValues = ["True", "False", "No Filter"];
        _this.AndOrOps = ["And", "Or"];
        _this.SelectedFieldsDataSource = [];
        _this.SelectedFiltersDataSource = [];
        _this.ObsList = [];
        _this.ObsListAll = [];
        _this.DataContext = _this;
        _this.AllFieldsObsList = [];
        _this.AllTables = [];
        _this.HasChanges = false;
        _this.IsBIReportWorkspace = false;
        _this.IsBIReportEditScreen = false;
        /////////
        //public TooltipId: string = null;
        //public TooltipContentId: string = null;
        _this.IconPath = "./Images/Help.png";
        _this.IconBackground = null;
        _this.Width = 200;
        _this.Height = 110;
        _this.IconSize = 17;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isbtnAddEnabled = false;
        _this.isbtnRemoveEnabled = false;
        _this.isbtnAddFilterEnabled = false;
        _this.isbtnRemoveFilterEnabled = false;
        _this.isbtnUpEnabled = true;
        _this.isbtnDownEnabled = true;
        _this.RootGroups = [];
        _this.WhereStmt = " where ";
        _this.TempFilters = [];
        _this.InnerTables = [];
        _this.SampleData = [];
        _this.IsPreview = true;
        _this.IsDataReturened = true;
        _this.BusyIndicatorText = null;
        _this.ShowBusyIndicator = false;
        _this.FiltersBusyIndicatorText = null;
        _this.FiltersShowBusyIndicator = false;
        _this.messageWindow = new MessageWindow_1.MessageWindow();
        _this.NotExist = true;
        _this._DWObjectTablePMService = new DWObjectTablePMService_1.DWObjectTablePMService();
        _this._DWQueryPMService = new DWQueryPMService_1.DWQueryPMService();
        _this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService_1.DWObjectFieldExtendedPMService();
        _this._DWQueryBuilderService = new DWQueryBuilderService_1.DWQueryBuilderService();
        _this._DWSubQueryPMService = new DWSubQueryPMService_1.DWSubQueryPMService();
        _this._DWObjectTableListService = new DWObjectTableListService_1.DWObjectTableListService();
        _this._DWQueryBuilderHelper = new DWQueryBuilderHelper_1.DWQueryBuilderHelper();
        if (_this.CurrentSession == null) {
            _this.SearchFieldsId = "SearchFields_-1_-1";
        }
        else {
            _this.SearchFieldsId = "DWQueryBuilderSearchFields_" + _this.CurrentSession.GetNewId("DWQueryBuilderSearchFields");
        }
        //this.ClearData();
        //this._DWQueryBuilderHelper.FillAllFactFields("Fact_Shipments");
        _this._DWObjectTableListService.getAll().subscribe(function (myResult) {
            _this.AllTables = myResult.Result;
            _this._DWObjectTablePMService.get("Fact_Shipments").subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this._DWObjectFieldPMService.GetDWObjectFieldsByDWTableIdGroupedByCategory(myResult.Result.Code).subscribe(function (Result) {
                        if (!Result.HasError) {
                            var MyGroups = [];
                            var MyAllGroups = [];
                            Result.Result.forEach(function (Group) {
                                if (Group.FieldsList.filter(function (a) { return a.DisplayInQueryBuilder == true; }).length > 0) {
                                    var view = new DWFieldsGroup(Group.Key, Group.FieldsList);
                                    if (MyGroups.length == 0) {
                                        view.IsDetailesOpened = true;
                                        view.DetailsIcon = "./Images/CellIcons/Arrowup.png";
                                    }
                                    else {
                                        view.IsDetailesOpened = false;
                                        view.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                                    }
                                    var MyInnerList = [];
                                    view.FieldsList.forEach(function (field) {
                                        if (field.DisplayInQueryBuilder == true) {
                                            var MyItem = new DWObjectFieldsDetails(field, _this);
                                            MyItem.ParentDataTypeCode = field.DataTypeCode;
                                            MyItem.Category1 = field.Category1;
                                            MyItem.Category2 = field.Category2;
                                            MyInnerList.push(MyItem);
                                            _this.ObsList.push(MyItem);
                                            _this.ObsListAll.push(MyItem);
                                        }
                                    });
                                    var view1 = new DWFieldsGroup(Group.Key, Group.FieldsList);
                                    if (MyAllGroups.length == 0) {
                                        view1.IsDetailesOpened = true;
                                        view1.DetailsIcon = "./Images/CellIcons/Arrowup.png";
                                    }
                                    else {
                                        view1.IsDetailesOpened = false;
                                        view1.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                                    }
                                    var MyInnerList1 = [];
                                    view1.FieldsList.forEach(function (field) {
                                        if (field.DisplayInQueryBuilder == true) {
                                            var MyItem = new DWObjectFieldsDetails(field, _this);
                                            MyItem.ParentDataTypeCode = field.DataTypeCode;
                                            MyItem.Category1 = field.Category1;
                                            MyItem.Category2 = field.Category2;
                                            MyInnerList1.push(MyItem);
                                            _this.ObsList.push(MyItem);
                                            _this.ObsListAll.push(MyItem);
                                        }
                                    });
                                    view.FieldsList = MyInnerList;
                                    view1.FieldsList = MyInnerList1;
                                    MyGroups.push(view);
                                    MyAllGroups.push(view1);
                                }
                            });
                            //Result.Result.forEach((field) => {
                            //    if (field.DisplayInQueryBuilder == true) {
                            //        var view = new DWObjectFieldsDetails(field, this);
                            //        view.ParentDataTypeCode = field.DataTypeCode;
                            //        view.Category1 = field.Category1;
                            //        view.Category2 = field.Category2;
                            //        this.ObsList.push(view);
                            //        this.ObsListAll.push(view);
                            //    }
                            //});
                            _this.DataSource = MyGroups; //this.ObsList;
                            _this.AllGroupsDataSource = MyAllGroups;
                            _this.AllFieldsDataSource = _this.ObsList;
                        }
                    });
                    //this.StartFiltersBusyIndicator("Restoring filters ..");
                    //this._DWObjectFieldPMService.getDWObjectFieldsWithChildrenByDWTableId(myResult.Result.Code).subscribe(Result => {
                    _this.ObsList = [];
                    if (window.DWObjectFields) {
                        window.DWObjectFields.forEach(function (field) {
                            if (field.DisplayInQueryBuilder == true || field.IsPrimaryKey == true) {
                                var view = new DWObjectFieldsDetails(field, _this);
                                view.ParentDataTypeCode = field.DataTypeCode;
                                _this.AllFieldsObsList.push(field);
                                _this.ObsList.push(view);
                                _this.ObsListAll.push(view);
                            }
                        });
                        //this.DataSource = this.ObsList;
                        _this.AllFieldsWithChildrenDataSource = _this.ObsList;
                        //this.StopFiltersBusyIndicator();
                    }
                    //});
                }
            });
        });
        return _this;
    }
    DWQueryBuilderComponent.prototype.mouseover = function (MyItem) {
        if (MyItem.HelpText) {
            var item = document.getElementById(MyItem.TooltipId);
            var itemRect = item.getBoundingClientRect();
            var isToRight = true;
            var ApplicationSession = document.getElementById("ApplicationSession");
            if (ApplicationSession) {
                var appWidth = ApplicationSession.clientWidth;
                var appHeight = ApplicationSession.clientHeight;
                if ((itemRect.left + this.Width) > appWidth) {
                    isToRight = false;
                }
            }
            document.getElementById(MyItem.TooltipContentId).style.position = "fixed";
            document.getElementById(MyItem.TooltipContentId).style.top = (itemRect.top - this.Height + 7) + 'px';
            if (isToRight) {
                document.getElementById(MyItem.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/TootipCenter.png')";
                document.getElementById(MyItem.TooltipContentId).style.left = (itemRect.left + 14) + 'px';
            }
            else {
                document.getElementById(MyItem.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/TootipFlipped.png')";
                document.getElementById(MyItem.TooltipContentId).style.left = (itemRect.left - this.Width) + 'px';
            }
        }
    };
    DWQueryBuilderComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.QID = args.DWQueryId;
        this.IsBIReportWorkspace = args.IsBIReportWorkspace;
        this.IsBIReportEditScreen = args.IsBIReportEditScreen;
        this.FolderId = args.FolderId;
        //this.AllFieldsWithChildrenDataSource = args.DWObjectFieldsWithChildren;
        if (this.QID) {
            this._DWSubQueryPMService.getByQueryId(this.QID).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    _this.ID = myResult.Result.SubQueryData.Id;
                    _this.EditButtonClicked();
                }
            });
        }
    };
    DWQueryBuilderComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        temp.select();
    };
    DWQueryBuilderComponent.prototype.FillPlaceHolder = function () {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchFieldsId);
            temp.placeholder = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Search");
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
    };
    DWQueryBuilderComponent.prototype.OnDeleteValue = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    };
    DWQueryBuilderComponent.prototype.Run = function () {
    };
    DWQueryBuilderComponent.prototype.SetSelectedItem = function (item) {
        this.SelectedItem = item;
        this.FieldSelectedItem = null;
        this.FilterSelectedItem = null;
        this.IsbtnAddEnabled = true;
        this.IsbtnRemoveEnabled = false;
        this.IsbtnAddFilterEnabled = true;
        this.IsbtnRemoveFilterEnabled = false;
        this.IsbtnUpEnabled = false;
        this.IsbtnDownEnabled = false;
    };
    DWQueryBuilderComponent.prototype.SetFieldSelectedItem = function (item) {
        this.FieldSelectedItem = item;
        this.SelectedItem = null;
        this.FilterSelectedItem = null;
        this.IsbtnAddEnabled = false;
        this.IsbtnRemoveEnabled = true;
        this.IsbtnUpEnabled = true;
        this.IsbtnDownEnabled = true;
        this.IsbtnAddFilterEnabled = false;
        this.IsbtnRemoveFilterEnabled = false;
    };
    DWQueryBuilderComponent.prototype.SetFilterSelectedItem = function (item) {
        this.FilterSelectedItem = item;
        this.SelectedItem = null;
        this.FieldSelectedItem = null;
        this.IsbtnAddFilterEnabled = false;
        this.IsbtnRemoveFilterEnabled = true;
        this.IsbtnAddEnabled = false;
        this.IsbtnRemoveEnabled = false;
    };
    Object.defineProperty(DWQueryBuilderComponent.prototype, "Notes", {
        get: function () { return this.notes; },
        set: function (newValue) {
            this.notes = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            var _this = this;
            this.searchText = newValue;
            if (newValue != null && newValue != "") {
                //var myAll = this.AllGroupsDataSource;
                this.AllGroupsDataSource.forEach(function (Group) {
                    var temp = Group.FieldsList.filter(function (a) { return a.Name.toLowerCase().indexOf(newValue.toLowerCase()) > -1 || a.DisplayName.toLowerCase().indexOf(newValue.toLowerCase()) > -1; });
                    _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].FieldsList = temp;
                    if (temp.length == 0) {
                        _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].IsDetailesOpened = false;
                        _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                    }
                    else {
                        _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].IsDetailesOpened = true;
                        _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].DetailsIcon = "./Images/CellIcons/Arrowup.png";
                    }
                });
                //this.DataSource = this.AllFieldsDataSource.filter(a => a.Name.toLowerCase().indexOf(newValue.toLowerCase()) > -1);
            }
            else {
                //this.DataSource = this.AllFieldsDataSource;
                var index = 0;
                this.AllGroupsDataSource.forEach(function (Group) {
                    if (index == 0) {
                        _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].IsDetailesOpened = true;
                        _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].DetailsIcon = "./Images/CellIcons/Arrowup.png";
                    }
                    else {
                        _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].IsDetailesOpened = false;
                        _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                    }
                    _this.DataSource.filter(function (a) { return a.Key == Group.Key; })[0].FieldsList = Group.FieldsList;
                    index++;
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            this.selectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "FieldSelectedItem", {
        get: function () { return this.fieldSelectedItem; },
        set: function (newValue) {
            this.fieldSelectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "FilterSelectedItem", {
        get: function () { return this.filterSelectedItem; },
        set: function (newValue) {
            this.filterSelectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "IsbtnAddEnabled", {
        get: function () { return this.isbtnAddEnabled; },
        set: function (newValue) {
            this.isbtnAddEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "IsbtnRemoveEnabled", {
        get: function () { return this.isbtnRemoveEnabled; },
        set: function (newValue) {
            this.isbtnRemoveEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "IsbtnAddFilterEnabled", {
        get: function () { return this.isbtnAddFilterEnabled; },
        set: function (newValue) {
            this.isbtnAddFilterEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "IsbtnRemoveFilterEnabled", {
        get: function () { return this.isbtnRemoveFilterEnabled; },
        set: function (newValue) {
            this.isbtnRemoveFilterEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "IsbtnUpEnabled", {
        get: function () { return this.isbtnUpEnabled; },
        set: function (newValue) {
            this.isbtnUpEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "IsbtnDownEnabled", {
        get: function () { return this.isbtnDownEnabled; },
        set: function (newValue) {
            this.isbtnDownEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    DWQueryBuilderComponent.prototype.ReorderColumnsList = function () {
        var queryColumnList = this.SelectedFieldsDataSource.sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
        var i = 0;
        for (; i < queryColumnList.length; i++) {
            queryColumnList[i].IndexOrder = i;
        }
    };
    DWQueryBuilderComponent.prototype.btnUp_Click = function (selectedItem) {
        var item = selectedItem; //this.FieldSelectedItem;
        if (item != null) {
            //this.HasChanges = true;
            var i = this.SelectedFieldsDataSource.indexOf(item);
            this.ReorderColumnsList();
            var upColumn = this.SelectedFieldsDataSource.filter(function (d) { return d.DisplayName == item.DisplayName; })[0];
            if (i > 0) {
                this.SelectedFieldsDataSource = this.SelectedFieldsDataSource.filter(function (d) { return d.DisplayName != upColumn.DisplayName; });
                this.SelectedFieldsDataSource.filter(function (o) { return o.IndexOrder == i - 1; })[0].IndexOrder = i;
                upColumn.IndexOrder = i - 1;
                this.SelectedFieldsDataSource.splice(i - 1, 0, upColumn);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
        }
    };
    //ShowArrows: boolean = false;
    DWQueryBuilderComponent.prototype.ShowOrderArrows = function (item) {
        this.FieldSelectedItem = item;
    };
    //ShowOrderArrows(item) {
    //}
    DWQueryBuilderComponent.prototype.btnDown_Click = function (selectedItem) {
        var item = selectedItem; //this.FieldSelectedItem;
        if (item != null) {
            //this.HasChanges = true;
            var i = this.SelectedFieldsDataSource.indexOf(item);
            this.ReorderColumnsList();
            var downColumn = this.SelectedFieldsDataSource.filter(function (d) { return d.DisplayName == item.DisplayName; })[0];
            if (i < this.SelectedFieldsDataSource.length - 1) {
                this.SelectedFieldsDataSource = this.SelectedFieldsDataSource.filter(function (d) { return d.DisplayName != item.DisplayName; });
                this.SelectedFieldsDataSource.filter(function (o) { return o.IndexOrder == i + 1; })[0].IndexOrder = i;
                downColumn.IndexOrder = i + 1;
                this.SelectedFieldsDataSource.splice(i + 1, 0, downColumn);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
        }
    };
    DWQueryBuilderComponent.prototype.ResetIndexes = function (TempArray) {
        var index = 0;
        TempArray.forEach(function (field) {
            field.IndexOrder = index;
            index++;
        });
        return TempArray;
    };
    DWQueryBuilderComponent.prototype.btnAdd_Click = function (item) {
        var _this = this;
        this.SelectedItem = item;
        var myCurrentItem = this.SelectedFieldsDataSource.filter(function (a) { return a.DisplayName == _this.SelectedItem.DisplayName; });
        if (this.SelectedItem && myCurrentItem && myCurrentItem.length == 0) {
            if (this.SelectedItem.Code == '[Full Date]') {
                this.SelectedItem.ParentDataTypeCode = "LookUp";
                this.SelectedItem.DataTypeCode = "Date";
                this.SelectedItem.HasTree = true;
            }
            if (this.SelectedItem.Name == 'Full Date') {
                this.SelectedItem.HasTree = false;
            }
            var tempData = this.SelectedFieldsDataSource;
            tempData.push(this.SelectedItem);
            this.SelectedFieldsDataSource = this.ResetIndexes(tempData);
            //this.SampleData = [];
            //this.Notes = "";
            //this.SaveChanges();
            this.ClearData();
        }
    };
    DWQueryBuilderComponent.prototype.ClearData = function () {
        this.SampleData = [];
        this.Notes = "";
        this.IsPreview = true;
        this.IsDataReturened = true;
    };
    DWQueryBuilderComponent.prototype.btnRemove_Click = function (item) {
        this.FieldSelectedItem = item;
        this.FieldSelectedItem = item;
        if (this.FieldSelectedItem) {
            var index = this.SelectedFieldsDataSource.indexOf(this.FieldSelectedItem);
            if (index !== -1) {
                this.SelectedFieldsDataSource.splice(index, 1);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
            //this.SampleData = [];
            //this.SaveChanges();
            this.ClearData();
        }
    };
    DWQueryBuilderComponent.prototype.btnAddFilter_Click = function (item) {
        if (item.CannotFilter == true) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Not available for filtering";
            this.messageWindow.Message = "This field is not available for filtering, you can use the code";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        var view = new DWObjectFieldsDetails(item.BaseDWObjectField, this);
        if (view.DWObjectTableCode.indexOf("DIM_") != -1) {
            //view.ParentDataTypeCode = "LookUp";
            if (view.Code == '[Full Date]') {
                view.ParentDataTypeCode = "Date";
                view.DataTypeCode = "Date";
                view.HasTree = true;
            }
            else {
                view.ParentDataTypeCode = "LookUp";
            }
            if (view.Name == 'Full Date') {
                view.HasTree = false;
            }
            if (item.BaseDWObjectField.DataTypeCode == "LookUp" || item.BaseDWObjectField.DataTypeCode == "Dimension") {
                view.ParentDimTabelName = item.BaseDWObjectField.DimensionTableCode;
            }
            else {
                view.ParentDimTabelName = item.BaseDWObjectField.DWObjectTableCode;
            }
        }
        else {
            view.ParentDataTypeCode = item.BaseDWObjectField.DataTypeCode;
        }
        view.DisplayName = item.DisplayName;
        view.DimensionTableDisplayName = item.DimensionTableDisplayName;
        this.SelectedItem = view;
        if (this.SelectedItem && this.SelectedFiltersDataSource.indexOf(this.SelectedItem) == -1) { //&& this.SelectedItem.DataTypeCode != "LookUp" && this.SelectedItem.DataTypeCode != "Dimension"
            if (this.SelectedFiltersDataSource.length == 0) {
                var DWObjectField = new DWObjectFieldsDetails(null, this);
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
                DWObjectField.FilterItems.push(this.SelectedItem);
                this.SelectedFiltersDataSource.push(DWObjectField);
            }
            else {
                var tempData = this.SelectedFiltersDataSource[0].FilterItems;
                tempData.push(this.SelectedItem);
                this.SelectedFiltersDataSource[0].FilterItems = tempData;
                var tempDataNew = this.SelectedFiltersDataSource;
                this.SelectedFiltersDataSource = [];
                this.SelectedFiltersDataSource = tempDataNew;
            }
            this.RunDetectChanges();
            if (this.SelectedItem.DataTypeCode == "Boolean") {
                //this.SaveChanges();
                this.ClearData();
            }
            this.ClearData();
        }
    };
    DWQueryBuilderComponent.prototype.btnRemoveFilter_Click = function () {
        if (this.FilterSelectedItem) {
            var index = this.SelectedFiltersDataSource.indexOf(this.FilterSelectedItem);
            if (index !== -1) {
                this.SelectedFiltersDataSource.splice(index, 1);
            }
            //this.SaveChanges();
            this.ClearData();
        }
    };
    DWQueryBuilderComponent.prototype.GetWhereStmtForFiltersList = function (FiltersList, AndOr) {
        var _this = this;
        FiltersList.forEach(function (Myfilter) {
            var isHaveMultiSelect = false;
            if (Myfilter.FilterItems.length > 0) {
                if (_this.GetIfFiltersHaveValues(Myfilter.FilterItems) == true) {
                    _this.WhereStmt = _this.WhereStmt + " ( ";
                }
                _this.GetWhereStmtForFiltersList(Myfilter.FilterItems, Myfilter.AndOr);
                if (_this.WhereStmt == " where ") {
                    _this.WhereStmt = "";
                }
                else if (_this.WhereStmt.substring(_this.WhereStmt.length - 4).indexOf("And") != -1 || _this.WhereStmt.substring(_this.WhereStmt.length - 4).indexOf("Or") != -1) {
                    _this.WhereStmt = _this.WhereStmt.substring(0, _this.WhereStmt.length - 4);
                }
                if (_this.WhereStmt != "" && _this.GetIfFiltersHaveValues(Myfilter.FilterItems) == true) {
                    _this.WhereStmt = _this.WhereStmt + " ) ";
                }
            }
            else {
                //Myfilter.FilterItems.filter(a => a.TextValue != null).forEach((filter) => {
                if (!Tools_1.AppTool.IsNullOrEmpty(Myfilter.TextValue)) {
                    var filter = Myfilter;
                    var OperationSimpol = "";
                    if (filter.Operation.Code == filter.equalsOp.Code) {
                        if (filter.DataTypeCode == 'Integer' || filter.DataTypeCode == 'Double' || filter.DataTypeCode == 'Decimal') {
                            OperationSimpol = " = @@ ";
                        }
                        else {
                            OperationSimpol = " IN ( '";
                            OperationSimpol = _this.BuildMultiValueSql(filter.TextValue, OperationSimpol);
                            isHaveMultiSelect = true;
                        }
                    }
                    else if (filter.Operation.Code == filter.notEqualsOp.Code) {
                        if (filter.DataTypeCode == 'Integer' || filter.DataTypeCode == 'Double' || filter.DataTypeCode == 'Decimal') {
                            OperationSimpol = " <> @@ ";
                        }
                        else {
                            OperationSimpol = " not IN ( '";
                            OperationSimpol = _this.BuildMultiValueSql(filter.TextValue, OperationSimpol);
                            isHaveMultiSelect = true;
                            //abed
                        }
                    }
                    else if (filter.Operation.Code == filter.startsWithOp.Code) {
                        OperationSimpol = " like '@@%' ";
                    }
                    //else if (filter.Operation.Code == filter.IsNullOp.Code) {
                    //    OperationSimpol = " like '%@@' ";
                    //}
                    else if (filter.Operation.Code == filter.IsNullOp.Code) {
                        OperationSimpol = " is null ";
                    }
                    else if (filter.Operation.Code == filter.IsNotNullOp.Code) {
                        OperationSimpol = " is not null ";
                    }
                    else if (filter.Operation.Code == filter.greaterThanOrEqualOp.Code) {
                        OperationSimpol = " >= @@ ";
                    }
                    else if (filter.Operation.Code == filter.largerThanOp.Code) {
                        OperationSimpol = " > @@ ";
                    }
                    else if (filter.Operation.Code == filter.lessThanOp.Code) {
                        OperationSimpol = " < @@ ";
                    }
                    else if (filter.Operation.Code == filter.lessThanOrEqualOp.Code) {
                        OperationSimpol = " <= @@ ";
                    }
                    if (filter.Operation.Code == filter.IsNullOp.Code) {
                        _this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is null or " + (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " = '' " + " " + AndOr + " ";
                    }
                    else if (filter.Operation.Code == filter.IsNotNullOp.Code) {
                        _this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is not null and " + (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " <> '' " + " " + AndOr + " ";
                    }
                    else {
                        var operation = !isHaveMultiSelect ? OperationSimpol.replace("@@", filter.TextValue) : OperationSimpol;
                        _this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + operation + " " + AndOr + " "; //" = " + "'" + filter.TextValue + "' and ";
                    }
                }
                else {
                    //if (this.WhereStmt == " where  ( ") {
                    //    this.WhereStmt = "";
                    //}
                }
                //});
            }
            //if (Myfilter.IsGroup == true) {
            //    this.WhereStmt = this.WhereStmt + " ) ";
            //}
        });
        //return WhereStmt;
    };
    DWQueryBuilderComponent.prototype.BuildMultiValueSql = function (textValue, operationSimpol) {
        var result = operationSimpol;
        if (textValue) {
            var values = textValue.toString().split(';');
            if (values.length > 0) {
                values.forEach(function (item) {
                    if (item) {
                        result += (item + "','");
                    }
                });
                result += ")";
                result = result.replace(",')", ")");
            }
            else
                result += " ')";
        }
        else
            result += " ')";
        return result;
    };
    DWQueryBuilderComponent.prototype.GetIfFiltersHaveValues = function (FiltersList) {
        return FiltersList.filter(function (a) { return !Tools_1.AppTool.IsNullOrEmpty(a.TextValue); }).length > 0;
    };
    DWQueryBuilderComponent.prototype.GetWhereJoined = function (FiltersList) {
        var _this = this;
        FiltersList.forEach(function (Myfilter) {
            if (Myfilter.FilterItems.length > 0) {
                _this.GetWhereJoined(Myfilter.FilterItems);
            }
            else {
                if (Myfilter.ParentDimTabelName != null && _this.InnerTables.filter(function (a) { return a.ParentDimTabelName == Myfilter.ParentDimTabelName; }).length == 0) {
                    _this.InnerTables.push(Myfilter);
                }
            }
        });
    };
    DWQueryBuilderComponent.prototype.DeleteField = function (Item, ListItems) {
        var _this = this;
        ListItems.forEach(function (Myfilter) {
            if (Myfilter.FilterItems.length > 0) { // Myfilter.FilterItems.indexOf(Item) > 
                _this.DeleteField(Item, Myfilter.FilterItems);
            }
            else {
                if (Myfilter == Item) {
                    ListItems = ListItems.filter(function (a) { return a != Item; });
                }
            }
        });
    };
    DWQueryBuilderComponent.prototype.SaveChanges = function (StopPreview) {
        var _this = this;
        if (StopPreview === void 0) { StopPreview = false; }
        this.DWQueryData = new DWQueryData_1.DWQueryData();
        this.DWQueryData.Filters = this.SelectedFiltersDataSource[0];
        this.DWQueryData.Columns = this.SelectedFieldsDataSource;
        this.DWQueryData.PageIndex = 0;
        this.DWQueryData.PageSize = 100;
        //if (this.DWQueryData.Filters) {
        if (this.SelectedFiltersDataSource.length > 0 && this.ValidFiltersValues(this.SelectedFiltersDataSource[0]) != true) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Invalid Filters";
            this.messageWindow.Message = "There is an invalid input in one of the filters";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        if (this.SelectedFieldsDataSource.length > 0) {
            this.StartBusyIndicator("Loading ..");
            this.IsPreview = !StopPreview;
            this._DWQueryBuilderService.GetNewDWQueryData(this.DWQueryData).subscribe(function (myResult) {
                if (!myResult.HasError) {
                    //this.SampleData = myResult.Result.SQLDataResult;
                    _this.StopBusyIndicator();
                    if (StopPreview == true) {
                        _this.SampleData = [];
                        var MySql = myResult.Result.SQLString.split("ORDER BY")[0];
                        _this.Notes = MySql; //myResult.Result.SQLString;
                    }
                    else {
                        _this.PreviewData(StopPreview, myResult.Result.SQLDataResult);
                    }
                }
            });
        }
        //else {
        //    this.Notes = SelectStmt + (HasMeasurement && GroupByStmt != " group by" ? GroupByStmt : "");
        //    if (GroupByStmt != " group by" || this.Notes.indexOf(" group by") == -1) {
        //        this.PreviewData(StopPreview);
        //    }
        //}
        //} 
        //this.InnerTables = [];
        //this.SampleData = [];
        //if (this.SelectedFieldsDataSource.length == 0) {
        //    return;
        //}
        //this.IsPreview = false;
        //var SelectStmt = "Select ";
        //var GroupByStmt = " group by ";
        ////var Wheremt = " Where ";
        //this.WhereStmt = " where ";
        //var HasMeasurement: boolean = false;
        //var FromTables = [];
        //this.SelectedFieldsDataSource.forEach((field) => {
        //    if (field.IsMeasurement) {
        //        HasMeasurement = true;
        //        SelectStmt += field.AggregationTypeCode + "(" + field.DWObjectTableCode + "." + field.Code + ")" + (field.DisplayName ? " as " + field.DisplayName + "," : ",");
        //    }
        //    else {
        //        SelectStmt += field.DWObjectTableCode + "." + field.Code + (field.DisplayName ? " as " + field.DisplayName + "," : ",");
        //        GroupByStmt += field.DWObjectTableCode + "." + field.Code + ",";
        //    }
        //    if (FromTables.filter(a => a == field.DWObjectTableCode).length == 0) {
        //        FromTables.push(field.DWObjectTableCode);
        //    }
        //});
        //SelectStmt = SelectStmt.substring(0, SelectStmt.length - 1);
        //GroupByStmt = GroupByStmt.substring(0, GroupByStmt.length - 1);
        //var Fact = FromTables.filter(a => a.indexOf("Fact") != -1)[0];
        //if (AppTool.IsNullOrEmpty(Fact)) {
        //    Fact = "Fact_Shipments";
        //}
        //SelectStmt += " from " + Fact;
        //this.SelectedFieldsDataSource.forEach((field) => {
        //    if (field.ParentDimTabelName != null && this.InnerTables.filter(a => a.ParentDimTabelName == field.ParentDimTabelName).length == 0) {
        //        this.InnerTables.push(field);
        //    }
        //});
        //if (this.SelectedFiltersDataSource.length > 0) {
        //    this.GetWhereJoined(this.SelectedFiltersDataSource);
        //    this.GetWhereStmtForFiltersList(this.SelectedFiltersDataSource, this.SelectedFiltersDataSource[0].AndOr);
        //}
        //FromTables = FromTables.filter(a => a != Fact);
        //this.InnerTables.forEach((mytbl) => {
        //    var Key = this.AllFieldsObsList.filter(a => a.DWObjectTableCode == mytbl.ParentDimTabelName && a.IsPrimaryKey == true)[0];
        //    var FactKey = this.AllFieldsDataSource.filter(a => a.DimensionTableCode == mytbl.ParentDimTabelName)[0];
        //    SelectStmt += " inner join " + mytbl.ParentDimTabelName + " on " + Fact + "." + ((FactKey.DataTypeCode.toLowerCase() == "lookup" || FactKey.DataTypeCode.toLowerCase() == "dimension") ? FactKey.DisplayName : FactKey.Code) + " = " + mytbl.ParentDimTabelName + "." + Key.Code
        //});
        //if (this.SelectedFiltersDataSource.length > 0) {
        //    this.Notes = SelectStmt + this.WhereStmt + (HasMeasurement && GroupByStmt != " group by" ? GroupByStmt : "");
        //    if (GroupByStmt != " group by" || this.Notes.indexOf(" group by") == -1) {
        //        this.PreviewData(StopPreview);
        //    }
        //}
        //else {
        //    this.Notes = SelectStmt + (HasMeasurement && GroupByStmt != " group by" ? GroupByStmt : "");
        //    if (GroupByStmt != " group by" || this.Notes.indexOf(" group by") == -1) {
        //        this.PreviewData(StopPreview);
        //    }
        //}
    };
    DWQueryBuilderComponent.prototype.ValidFiltersValues = function (MyFilter) {
        var _this = this;
        if (MyFilter.FilterItems.length == 0) {
            return true;
        }
        var Valid = true;
        MyFilter.FilterItems.forEach(function (field) {
            if (field.FilterItems.length == 0) {
                if (field.DataTypeCode && field.TextValue) {
                    switch (field.DataTypeCode.toLowerCase()) {
                        case 'integer':
                        case 'double':
                        case 'decimal':
                            {
                                var val;
                                if ((field.TextValue + "").indexOf(',') == -1) {
                                    val = Number(field.TextValue);
                                }
                                if (isNaN(Number(val))) {
                                    Valid = false;
                                }
                            }
                        default: {
                            break;
                        }
                    }
                }
            }
            else {
                _this.ValidFiltersValues(field);
            }
        });
        return Valid;
    };
    DWQueryBuilderComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    DWQueryBuilderComponent.prototype.PreviewData = function (StopPreview, Data) {
        if (StopPreview === void 0) { StopPreview = false; }
        //if (this.Notes) {
        if (StopPreview == true) {
            this.SampleData = [];
            this.IsPreview = false;
            return;
        }
        if (Data.length > 0) {
            this.IsDataReturened = true;
        }
        else {
            this.IsDataReturened = false;
        }
        this.IsPreview = true;
        this.SampleData = Data;
        //var tempSQL = this.Notes.replace("Select ", "Select top 100 ").trim();
        //this._DWQueryBuilderService.GetDWQueryData(tempSQL, "Fact_Shipments").subscribe(myResult => {
        //    if (!myResult.HasError) {
        //        this.SampleData = myResult.Result;
        //    }
        //});
        //}
    };
    DWQueryBuilderComponent.prototype.ShowSQL = function () {
        this.SaveChanges(true);
    };
    DWQueryBuilderComponent.prototype.AddFilterToGroup = function () {
        var DWObjectField = new DWObjectFieldsDetails(null, this);
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var tempData = this.SelectedFiltersDataSource[0].FilterItems;
        tempData.push(this.SelectedItem);
        this.SelectedFiltersDataSource[0].FilterItems = tempData;
    };
    DWQueryBuilderComponent.prototype.AddGroup = function () {
        var DWObjectField = new DWObjectFieldsDetails(null, this);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var DWInnerObjectField = new DWObjectFieldsDetails(null, this);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        var tempData = this.SelectedFiltersDataSource[0].FilterItems;
        tempData.push(this.SelectedItem);
        this.SelectedFiltersDataSource[0].FilterItems = tempData;
    };
    DWQueryBuilderComponent.prototype.StartBusyIndicator = function (myText) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
        this.RunDetectChanges();
    };
    DWQueryBuilderComponent.prototype.StopBusyIndicator = function () {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    };
    DWQueryBuilderComponent.prototype.StartFiltersBusyIndicator = function (myText) {
        this.FiltersBusyIndicatorText = myText;
        this.FiltersShowBusyIndicator = true;
        this.RunDetectChanges();
    };
    DWQueryBuilderComponent.prototype.StopFiltersBusyIndicator = function () {
        this.FiltersBusyIndicatorText = null;
        this.FiltersShowBusyIndicator = false;
    };
    Object.defineProperty(DWQueryBuilderComponent.prototype, "QID", {
        get: function () { return this.qID; },
        set: function (newValue) {
            this.qID = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWQueryBuilderComponent.prototype, "ID", {
        get: function () { return this.iD; },
        set: function (newValue) {
            this.iD = newValue;
        },
        enumerable: true,
        configurable: true
    });
    DWQueryBuilderComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        if (this.SelectedFieldsDataSource.length == 0) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Invalid Query";
            this.messageWindow.Message = "The query should contain at least one column.";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ..");
        this._DWObjectTablePMService.get("Fact_Shipments").subscribe(function (myResult) {
            if (!myResult.HasError) {
                var MySubQuery = new DWSubQueryPM_1.DWSubQueryPM();
                MySubQuery.Tenant = SessionLocator_1.SessionLocator.Tenant;
                MySubQuery.DWFactTableCode = myResult.Result.Code;
                //MySubQuery.DWQueryId = '1-1';
                MySubQuery.SQLString = _this.Notes;
                _this.QueryData = new DWQueryData_1.DWQueryData();
                _this.QueryData.SubQueryData = MySubQuery;
                _this.QueryData.Columns = _this.SelectedFieldsDataSource;
                _this.QueryData.Filters = _this.SelectedFiltersDataSource[0];
                if (Tools_1.AppTool.IsNullOrEmpty(_this.ID)) {
                    _this._DWSubQueryPMService.insertDWQueryData(_this.QueryData).subscribe(function (myResult) {
                        //if (!myResult.HasError) {
                        //}
                        _this.ID = myResult.Result.Id;
                        _this.QID = myResult.Result.DWQueryId;
                        _this.EditButtonClicked();
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (_this.IsBIReportWorkspace) {
                            _this.CurrentSession.CloseCurrentWindowEmit("ok");
                        }
                    });
                }
                else {
                    MySubQuery.Id = _this.ID;
                    if (_this.NotExist == true) {
                        _this.messageWindow.Width = 300;
                        _this.messageWindow.Height = 150;
                        _this.messageWindow.Title = "Query Doesn't Exist";
                        _this.messageWindow.Message = "Query With the Id " + _this.ID + " does not exist";
                        _this.messageWindow.Show(_this.messageWindow.Message);
                        _this.NotExist = true;
                    }
                    _this._DWSubQueryPMService.UpdateDWQueryData(_this.QueryData).subscribe(function (myResult) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (_this.IsBIReportWorkspace || _this.IsBIReportEditScreen) {
                            _this.CurrentSession.CloseCurrentWindowEmit("ok");
                        }
                    });
                }
            }
        });
    };
    DWQueryBuilderComponent.prototype.EditButtonClicked = function () {
        var _this = this;
        this.SelectedFieldsDataSource = [];
        this.SelectedFiltersDataSource = [];
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ..");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ID)) {
            var QueryData = new DWQueryData_1.DWQueryData();
            this._DWSubQueryPMService.get(this.ID).subscribe(function (myResult) {
                if (myResult.Result == null) {
                    _this.messageWindow.Width = 300;
                    _this.messageWindow.Height = 150;
                    _this.messageWindow.Title = "Query Doesn't Exist";
                    _this.messageWindow.Message = "Query With the Id " + _this.ID + " does not exist";
                    _this.messageWindow.Show(_this.messageWindow.Message);
                    _this.NotExist = true;
                    return;
                }
                else {
                    _this.NotExist = false;
                }
                if (!myResult.HasError) {
                    QueryData = myResult.Result;
                    ////////////////////////////////////////////////
                    var tempColumns = _this.SelectedFieldsDataSource;
                    QueryData.Columns.forEach(function (field) {
                        var view = new DWObjectFieldsDetails(field, _this);
                        view.ParentDataTypeCode = field.ParentDataTypeCode;
                        view.DisplayName = field.DisplayName;
                        view.DimensionTableDisplayName = field.DimensionTableDisplayName;
                        view.ParentCode = field.ParentCode;
                        view.ParentDimTabelName = field.ParentDimTabelName;
                        tempColumns.push(view);
                    });
                    _this.SelectedFieldsDataSource = _this.ResetIndexes(tempColumns);
                    //////////////////////////////////////////
                    ////////////////////////////////////////////////
                    if (QueryData.Filters) {
                        var DWObjectField = new DWObjectFieldsDetails(null, _this);
                        DWObjectField.IsGroup = true;
                        DWObjectField.setAndOrOperation(QueryData.Filters.AndOr, false);
                        DWObjectField.IndexOrder = _this.SelectedFiltersDataSource.length;
                        var MyFilter = _this.RestoreFilters(QueryData.Filters, DWObjectField);
                        var temp = [];
                        temp.push(MyFilter);
                        _this.SelectedFiltersDataSource = temp;
                    }
                    else {
                        _this.SelectedFiltersDataSource = [];
                    }
                    if (_this.CurrentSession.CurrentWindow) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                    //this.PreviewData(true, []);
                    //////////////////////////////////////////
                }
            });
        }
    };
    DWQueryBuilderComponent.prototype.RestoreFilters = function (BaseFilter, MyFilter) {
        var _this = this;
        BaseFilter.FilterItems.forEach(function (field) {
            var view = new DWObjectFieldsDetails(field, _this);
            if (field.FilterItems.length == 0) {
                if (field.DWObjectTableCode && field.DWObjectTableCode.indexOf("DIM_") != -1) {
                    if (view.Code == '[Full Date]') {
                        view.ParentDataTypeCode = "Date";
                        view.DataTypeCode = "Date";
                    }
                    else {
                        view.ParentDataTypeCode = "LookUp";
                    }
                    view.ParentDimTabelName = field.DWObjectTableCode;
                }
                else {
                    view.ParentDataTypeCode = field.DataTypeCode;
                }
            }
            if (field.FilterItems.length == 0) {
                //view.TextValue = field.TextValue;
                view.setTextValue(field.TextValue, false);
                view.MultiSelectedValueLists = _this.MapMultiSelectedValueLists(field.MultiSelectedValueLists);
                view.Operation = new ObjectFieldOperator(field.OperationCode, field.OperationName);
                MyFilter.FilterItems.push(view);
            }
            else {
                var DWObjectField = new DWObjectFieldsDetails(null, _this);
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = MyFilter.FilterItems.length;
                //DWObjectField.AndOr = field.AndOr;
                DWObjectField.setAndOrOperation(field.AndOr, false);
                _this.RestoreFilters(field, DWObjectField);
                MyFilter.FilterItems.push(DWObjectField);
            }
        });
        return MyFilter;
    };
    DWQueryBuilderComponent.prototype.MapMultiSelectedValueLists = function (lists) {
        var result = [];
        if (lists) {
            lists.forEach(function (field) {
                var item = new MultiSelectedValue();
                var i = "";
                var j = 0;
                while (field["Value" + i]) {
                    var valueDetails = new ValueDetails();
                    valueDetails.Header = field["Value" + i].Header;
                    valueDetails.Row = field["Value" + i].Row;
                    item["Value" + i] = valueDetails;
                    j += 1;
                    i = j.toString();
                }
                result.push(item);
            });
            return result;
        }
    };
    DWQueryBuilderComponent.prototype.RunDetectChanges = function () {
        if (this.CD) {
            var isDestroyed = this.CD['destroyed'];
            if (!isDestroyed) {
                this.CD.detectChanges();
            }
        }
    };
    DWQueryBuilderComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DWQueryBuilder',
            templateUrl: './DWQueryBuilderComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], DWQueryBuilderComponent);
    return DWQueryBuilderComponent;
}(BaseComponent_1.BaseComponent));
exports.DWQueryBuilderComponent = DWQueryBuilderComponent;
var DWObjectFieldsDetails = /** @class */ (function (_super) {
    __extends(DWObjectFieldsDetails, _super);
    function DWObjectFieldsDetails(DWObjectField, ParentClass) {
        if (DWObjectField === void 0) { DWObjectField = null; }
        if (ParentClass === void 0) { ParentClass = null; }
        var _this = _super.call(this) || this;
        _this.TooltipId = null;
        _this.TooltipContentId = null;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.cannotFilter = false;
        _this.Items = [];
        _this.FilterItems = [];
        _this.isCustom = false;
        _this.isGroup = false;
        _this.showBtns = false;
        _this.boolValue = "No";
        _this.textValue = (_this.dataTypeCode == "Boolean") ? false : null;
        _this.filterType = "Ask User";
        _this.isSetDefaults = true;
        _this.isMandatoryFilter = false;
        _this.DontSaveChanges = false;
        _this.ShowSampleDateCommand = new core_1.EventEmitter();
        _this.startsWithOp = new ObjectFieldOperator("StartsWith", "Starts With");
        _this.equalsOp = new ObjectFieldOperator("Equals", "Equals to");
        _this.notEqualsOp = new ObjectFieldOperator("NotEqual", "Not Equal to");
        _this.largerThanOp = new ObjectFieldOperator("LargerThan", "Greater Than");
        _this.lessThanOp = new ObjectFieldOperator("LessThan", "Less Than");
        _this.greaterThanOrEqualOp = new ObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
        _this.lessThanOrEqualOp = new ObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
        _this.IsNullOp = new ObjectFieldOperator("IsNull", "Is Empty");
        _this.IsNotNullOp = new ObjectFieldOperator("IsNotNull", "Has Value");
        _this.beforeOp = new ObjectFieldOperator("Before", "Before");
        _this.afterOp = new ObjectFieldOperator("After", "After");
        _this.previousOp = new ObjectFieldOperator("Previous", "Previous");
        _this.currentOp = new ObjectFieldOperator("Current", "Current");
        _this.nextOp = new ObjectFieldOperator("Next", "Next");
        _this.BetweenOp = new ObjectFieldOperator("Between", "Between");
        var idIndex = _this.CurrentSession.GetNewId("Tooltip");
        _this.TooltipId = "Tooltip_" + idIndex;
        _this.TooltipContentId = "TooltipContent_" + idIndex;
        _this.BaseDWObjectField = DWObjectField;
        _this.FilterTypes = [];
        _this.FilterTypes.push(new ObjectFieldOperator("Fixed Filter", "Fixed Filter"));
        _this.FilterTypes.push(new ObjectFieldOperator("Ask User", "Dynamic Filter"));
        if (ParentClass != null) {
            _this.MyParentClass = ParentClass;
            _this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        }
        if (DWObjectField != null) {
            _this.HideTree = DWObjectField.HideTree;
            _this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
            if (!_this.LOVAdditionalColumns && ParentClass && ParentClass.AllFieldsDataSource) {
                var field = ParentClass.AllFieldsDataSource.filter(function (a) { return a.DWObjectTableCode == DWObjectField.DWObjectTableCode && a.Code == DWObjectField.Code && a.Name == DWObjectField.Name; })[0];
                if (field) {
                    _this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns = field.LOVAdditionalColumns;
                }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
                _this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
            }
            else {
                _this.DimensionTableDisplayName = DWObjectField.ParentCode;
            }
            _this.Name = DWObjectField.Name;
            _this.Code = DWObjectField.Code;
            _this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
            _this.DimensionTableCode = DWObjectField.DimensionTableCode;
            _this.DataTypeCode = DWObjectField.DataTypeCode;
            //if (DWObjectField.FilterItems && DWObjectField.FilterItems.length == 0) {
            _this.DisplayName = _this.ComputeDisplayName(DWObjectField);
            //}
            _this.CannotFilter = DWObjectField.CannotFilter;
            _this.HelpText = DWObjectField.HelpText;
            _this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
            _this.IsMeasurement = DWObjectField.IsMeasurement;
            _this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
            _this.IsCustom = DWObjectField.IsCustom;
            if (DWObjectField.FilterType) {
                _this.FilterType = DWObjectField.FilterType;
            }
            if (DWObjectField.IsSetDefaults) {
                _this.IsSetDefaults = DWObjectField.IsSetDefaults;
            }
            if (DWObjectField.IsMandatoryFilter) {
                _this.IsMandatoryFilter = DWObjectField.IsMandatoryFilter;
            }
            //this.Name = DWObjectField.Name;
        }
        _this.FilterTypeSelected = _this.FilterTypes.filter(function (d) { return d.Code == _this.FilterType; })[0];
        return _this;
    }
    DWObjectFieldsDetails.prototype.ComputeDisplayName = function (DWObjectField) {
        //(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
        var Displayname = DWObjectField.DisplayName;
        if (Tools_1.AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) {
            if (DWObjectField.DWObjectTableCode && DWObjectField.DWObjectTableCode.indexOf("DIM_") != -1) {
                Displayname = DWObjectField.ParentCode + ' ' + DWObjectField.Name;
            }
            else {
                Displayname = DWObjectField.Name;
            }
        }
        return Displayname;
    };
    Object.defineProperty(DWObjectFieldsDetails.prototype, "HideTree", {
        get: function () { return this.hideTree; },
        set: function (newValue) { this.hideTree = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "CannotFilter", {
        get: function () { return this.cannotFilter; },
        set: function (newValue) { this.cannotFilter = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "HelpText", {
        get: function () { return this.helpText; },
        set: function (newValue) { this.helpText = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "LOVAdditionalColumns", {
        get: function () { return this.lOVAdditionalColumns; },
        set: function (newValue) { this.lOVAdditionalColumns = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Category1", {
        get: function () { return this.category1; },
        set: function (newValue) { this.category1 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsCustom", {
        get: function () { return this.isCustom; },
        set: function (newValue) { this.isCustom = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Category2", {
        get: function () { return this.category2; },
        set: function (newValue) { this.category2 = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IndexOrder", {
        get: function () { return this.indexOrder; },
        set: function (newValue) { this.indexOrder = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsGroup", {
        get: function () { return this.isGroup; },
        set: function (newValue) { this.isGroup = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsPrimaryKey", {
        get: function () { return this.isPrimaryKey; },
        set: function (newValue) { this.isPrimaryKey = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "ShowBtns", {
        get: function () { return this.showBtns; },
        set: function (newValue) { this.showBtns = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsViewTree", {
        get: function () { return this.isViewTree; },
        set: function (newValue) { this.isViewTree = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "HasTree", {
        get: function () { return this.hasTree; },
        set: function (newValue) { this.hasTree = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DisplayName", {
        get: function () { return this.displayname; },
        set: function (newValue) { this.displayname = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DimensionTableDisplayName", {
        get: function () { return this.dimensionTableDisplayName; },
        set: function (newValue) { this.dimensionTableDisplayName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "ParentCode", {
        get: function () { return this.parentcode; },
        set: function (newValue) { this.parentcode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "ParentDimTabelName", {
        get: function () { return this.parentDimTabelName; },
        set: function (newValue) { this.parentDimTabelName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DWObjectTableCode", {
        get: function () { return this.dWObjectTableCode; },
        set: function (newValue) { if (this.dWObjectTableCode != newValue) {
            this.dWObjectTableCode = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsMeasurement", {
        get: function () { return this.isMeasurement; },
        set: function (newValue) { if (this.isMeasurement != newValue) {
            this.isMeasurement = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "AggregationTypeCode", {
        get: function () { return this.aggregationTypeCode; },
        set: function (newValue) { if (this.aggregationTypeCode != newValue) {
            this.aggregationTypeCode = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DataTypeCode", {
        get: function () { return this.dataTypeCode; },
        set: function (newValue) {
            var _this = this;
            //if (this.dataTypeCode != newValue) {
            this.dataTypeCode = newValue;
            if (this.dataTypeCode == "Boolean") {
                this.TextValue = false;
            }
            if (this.dataTypeCode == "LookUp" || this.dataTypeCode == "Dimension") {
                this.HasTree = !this.HideTree ? true : false;
                var MyTable = this.MyParentClass.AllTables.filter(function (a) { return a.Code == _this.DimensionTableCode; });
                if (MyTable && MyTable.length > 0) {
                    this.Code = MyTable[0].DefaultFilterBy;
                    if (this.code && this.MyParentClass && this.MyParentClass.AllFieldsDataSource) {
                        var field = this.MyParentClass.AllFieldsDataSource.filter(function (d) { return d.Code == _this.code && d.DWObjectTableCode == _this.DimensionTableCode; })[0];
                        if (field) {
                            this.LOVAdditionalColumns = field.LOVAdditionalColumns;
                        }
                    }
                    this.DWObjectTableCode = MyTable[0].Code;
                    this.DisplayName = this.ComputeDisplayName(this); //(AppTool.IsNullOrEmpty(this.DisplayName)) ? (this.DWObjectTableCode + ' ' + this.Code) : (this.DisplayName);
                    this.ParentDimTabelName = this.DimensionTableCode;
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.DimensionTableDisplayName)) {
                        this.DimensionTableDisplayName = this.DimensionTableDisplayName;
                    }
                    else {
                        this.DimensionTableDisplayName = this.ParentCode;
                    }
                }
            }
            else {
                this.HasTree = false;
            }
            //}
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "ParentDataTypeCode", {
        get: function () { return this.parentDataTypeCode; },
        set: function (newValue) {
            if (this.parentDataTypeCode != newValue) {
                this.parentDataTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "DimensionTableCode", {
        get: function () { return this.dimensionTableCode; },
        set: function (newValue) { if (this.dimensionTableCode != newValue) {
            this.dimensionTableCode = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Operators", {
        get: function () { return this.GetFieldOperators(this); },
        set: function (newValue) {
            this.operators = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "BoolValue", {
        get: function () {
            if (this.TextValue == true) {
                this.boolValue = "Yes";
            }
            else if (this.TextValue == false) {
                this.boolValue = "No";
            }
            else {
                this.boolValue = "No Value";
            }
            return this.boolValue;
        },
        set: function (newValue) {
            if (this.boolValue != newValue) {
                this.boolValue = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "TextValue", {
        get: function () {
            return this.textValue;
        },
        set: function (newValue) {
            var _this = this;
            if (this.textValue != newValue) {
                this.textValue = newValue;
                if (this.ParentDataTypeCode == "DateTime" || this.ParentDataTypeCode == "Date") {
                    var timerToken = setTimeout(function () {
                        _this.ShowSampleDateCommand.emit(_this);
                    }, 0);
                }
                this.MyParentClass.ClearData();
                //if ((newValue == true || newValue == false) && this.textValue) {
                //    this.textValue = newValue;
                //    //if (!this.DontSaveChanges) {
                //    //    //this.MyParentClass.SaveChanges();
                //    //    this.MyParentClass.ClearData();
                //    //}
                //}
                //else if (newValue != true || newValue != false) {
                //    this.textValue = newValue;
                //    if (!this.DontSaveChanges) {
                //        //this.MyParentClass.SaveChanges();
                //        this.MyParentClass.ClearData();
                //    }
                //}
                this.DontSaveChanges = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    DWObjectFieldsDetails.prototype.setTextValue = function (Newvalue, ClearData) {
        if (ClearData === void 0) { ClearData = true; }
        this.textValue = Newvalue;
        if (ClearData == true) {
            this.MyParentClass.ClearData();
        }
    };
    DWObjectFieldsDetails.prototype.setAndOrOperation = function (Newvalue, ClearData) {
        if (ClearData === void 0) { ClearData = true; }
        this.AndOr = Newvalue;
        if (ClearData == true) {
            this.MyParentClass.ClearData();
        }
    };
    Object.defineProperty(DWObjectFieldsDetails.prototype, "MultiSelectedValueLists", {
        get: function () {
            return this.multiSelectedValueLists;
        },
        set: function (newValue) {
            this.multiSelectedValueLists = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "OperationName", {
        get: function () { return this.operationName; },
        set: function (newValue) {
            if (this.operationName != newValue) {
                this.operationName = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "OperationCode", {
        get: function () { return this.operationCode; },
        set: function (newValue) {
            if (this.operationCode != newValue) {
                this.operationCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "Operation", {
        get: function () {
            if (!this.operation) {
                if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText") && Tools_1.AppTool.IsNullOrEmpty(this.operation)) {
                    this.operation = new ObjectFieldOperator("StartsWith", "Starts With");
                    this.OperationCode = "StartsWith";
                    this.OperationName = "Starts With";
                    return this.operation;
                }
                else if ((this.ParentDataTypeCode == "Date" || this.ParentDataTypeCode == "DateTime") && Tools_1.AppTool.IsNullOrEmpty(this.operation)) {
                    this.operation = new ObjectFieldOperator("Before", "Before");
                    this.OperationCode = "Before";
                    this.OperationName = "Before";
                    return this.operation;
                }
                else {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.operation)) {
                        this.operation = new ObjectFieldOperator("Equals", "Equals to");
                    }
                    this.OperationCode = "Equals";
                    this.OperationName = "Equals to";
                    return this.operation;
                }
            }
            else {
                return this.operation;
            }
        },
        set: function (newValue) {
            this.OperationCode = newValue.Code;
            this.OperationName = newValue.Name;
            this.operation = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "AndOr", {
        get: function () {
            if (Tools_1.AppTool.IsNullOrEmpty(this.andOr)) {
                return "And";
            }
            return this.andOr;
        },
        set: function (newValue) {
            this.andOr = newValue;
            //this.MyParentClass.SaveChanges();
            //this.MyParentClass.ClearData();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "FilterType", {
        get: function () { return this.filterType; },
        set: function (newValue) { this.filterType = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "FilterTypeSelected", {
        get: function () { return this.filterTypeSelected; },
        set: function (newValue) {
            this.filterTypeSelected = newValue;
            if (this.filterTypeSelected) {
                this.FilterType = this.filterTypeSelected.Code;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsSetDefaults", {
        get: function () { return this.isSetDefaults; },
        set: function (newValue) {
            if (this.isSetDefaults != newValue) {
                this.isSetDefaults = newValue;
                if (newValue == false) {
                    this.TextValue = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWObjectFieldsDetails.prototype, "IsMandatoryFilter", {
        get: function () { return this.isMandatoryFilter; },
        set: function (newValue) { if (this.isMandatoryFilter != newValue) {
            this.isMandatoryFilter = newValue;
        } },
        enumerable: true,
        configurable: true
    });
    DWObjectFieldsDetails.prototype.FilterTypeChanged = function (Value) {
        this.FilterTypeSelected = Value;
    };
    DWObjectFieldsDetails.prototype.OpenFilterSettings = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.IsMandatoryFilter = this.IsMandatoryFilter;
        windowArgs.IsSetDefaults = this.IsSetDefaults;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 500;
        logWindow.Height = 260;
        logWindow.Title = "Dynamic Filter Settings";
        logWindow.Show('./CommonModules/CommonOthers/Components/DWQueryBuilder/DWFilterSettings');
        logWindow.WindowClosed.subscribe(function ($event) {
            if ($event) {
                var MySettings = $event.split(',');
                if (MySettings[0] == "true") {
                    _this.IsMandatoryFilter = true;
                }
                else {
                    _this.IsMandatoryFilter = false;
                }
                if (MySettings[1] == "true") {
                    _this.IsSetDefaults = true;
                }
                else {
                    _this.IsSetDefaults = false;
                }
            }
        });
    };
    DWObjectFieldsDetails.prototype.OperationValueChanged = function (operation) {
        if (operation.Code == this.currentOp.Code || operation.Code == this.beforeOp.Code || operation.Code == this.afterOp.Code || operation.Code == this.previousOp.Code || operation.Code == this.nextOp.Code || operation.Code == this.currentOp.Code || operation.Code == this.BetweenOp.Code) {
            this.DontSaveChanges = true;
            //this.TextValue = "";
            this.Operation = operation;
        }
        else {
            if (this.Operation.Code == this.IsNullOp.Code || this.Operation.Code == this.IsNotNullOp.Code) {
                this.TextValue = "";
                this.MultiSelectedValueLists = [];
            }
            this.Operation = operation;
            if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code) {
                this.TextValue = operation.Code;
            }
            if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code || !Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
                //this.MyParentClass.SaveChanges();
                this.MyParentClass.ClearData();
            }
        }
    };
    DWObjectFieldsDetails.prototype.LoadItems = function (DWObjectField) {
        if (this.IsViewTree) {
            this.IsViewTree = false;
        }
        else {
            if (this.Items.length == 0) {
                this.Load(DWObjectField);
            }
            else
                this.IsViewTree = true;
        }
    };
    DWObjectFieldsDetails.prototype.Load = function (DWObjectField) {
        var _this = this;
        var _DWObjectTablePMService = new DWObjectTablePMService_1.DWObjectTablePMService();
        var _DWObjectFieldPMService = new DWObjectFieldExtendedPMService_1.DWObjectFieldExtendedPMService();
        var ObsList = [];
        _DWObjectTablePMService.get(DWObjectField.DimensionTableCode).subscribe(function (myResult) {
            if (!myResult.HasError) {
                _DWObjectFieldPMService.getDWObjectFieldsByDWTableId(myResult.Result.Code).subscribe(function (Result) {
                    if (!Result.HasError) {
                        Result.Result.forEach(function (field) {
                            if (field.DisplayInQueryBuilder == true) {
                                var view = new DWObjectFieldsDetails(field, _this.MyParentClass);
                                if (field.Code == '[Full Date]') {
                                    view.ParentDataTypeCode = field.DataTypeCode;
                                    view.DataTypeCode = "Date";
                                }
                                else {
                                    view.ParentDataTypeCode = DWObjectField.DataTypeCode;
                                }
                                var dwObjectFieldName = DWObjectField.IsCustom ? DWObjectField.DisplayName : DWObjectField.Name;
                                if (!Tools_1.AppTool.IsNullOrEmpty(DWObjectField.Code)) {
                                    view.DisplayName = '[' + (dwObjectFieldName.replace('[', '').replace(']', '') + ' ' + view.Name.replace('[', '').replace(']', '')) + ']'; //.replace('[', '').replace('[', '').replace(']', '').replace(']', '');
                                    view.DimensionTableDisplayName = DWObjectField.Name.replace('[', '').replace(']', '');
                                }
                                else if (!Tools_1.AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) {
                                    view.DisplayName = dwObjectFieldName;
                                    view.DimensionTableDisplayName = DWObjectField.Name;
                                }
                                else {
                                    view.DisplayName = '[' + (dwObjectFieldName + ' ' + view.Name.replace('[', '').replace(']', '')) + ']'; //.replace('[', '').replace('[', '').replace(']', '').replace(']', '');
                                    view.DimensionTableDisplayName = DWObjectField.Name;
                                }
                                view.ParentCode = DWObjectField.Code;
                                view.ParentDimTabelName = DWObjectField.DimensionTableCode;
                                ObsList.push(view);
                            }
                        });
                        _this.Items = ObsList;
                        _this.IsViewTree = true;
                    }
                });
            }
        });
    };
    DWObjectFieldsDetails.prototype.onTextChange = function (value) {
        if (this.DataTypeCode == "Boolean") {
            if (value == "Yes") {
                this.TextValue = true;
            }
            else if (value == "No") {
                this.TextValue = false;
            }
            else {
                this.TextValue = null;
            }
        }
        else {
            this.TextValue = value;
        }
        this.ShowSampleDateCommand.emit(this);
    };
    DWObjectFieldsDetails.prototype.AndOrOpsChanged = function (value) {
        this.AndOr = value;
    };
    DWObjectFieldsDetails.prototype.OnMouseOver = function (event) {
        var e = event.toElement;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == false) {
            this.ShowBtns = true;
        }
    };
    DWObjectFieldsDetails.prototype.OnMouseOut = function (event) {
        var e = event.toElement;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == true) {
            this.ShowBtns = false;
        }
    };
    DWObjectFieldsDetails.prototype.preventInnerHover = function (event) {
        event.stopPropagation();
    };
    DWObjectFieldsDetails.prototype.AddFilterToGroup = function () {
        var DWObjectField = new DWObjectFieldsDetails();
        DWObjectField.IndexOrder = this.FilterItems.length;
        this.FilterItems.push(DWObjectField);
    };
    DWObjectFieldsDetails.prototype.AddGroup = function (Father) {
        var DWObjectField = new DWObjectFieldsDetails(null, Father.MyParentClass);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = Father.MyParentClass.SelectedFiltersDataSource.length;
        var DWInnerObjectField = new DWObjectFieldsDetails(null, Father.MyParentClass);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        Father.MyParentClass.SelectedFiltersDataSource.push(DWObjectField);
    };
    DWObjectFieldsDetails.prototype.FieldValueChanged = function (DWObjectField) {
        this.TextValue = "";
        this.MultiSelectedValueLists = [];
        this.Name = DWObjectField.Name;
        this.Code = DWObjectField.Code;
        this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        this.DataTypeCode = DWObjectField.DataTypeCode;
        this.DimensionTableCode = DWObjectField.DimensionTableCode;
        this.DisplayName = this.ComputeDisplayName(DWObjectField); //(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
        if (!Tools_1.AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
            this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
        }
        else {
            this.DimensionTableDisplayName = DWObjectField.ParentCode;
        }
        this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        this.IsMeasurement = DWObjectField.IsMeasurement;
        this.IsCustom = DWObjectField.IsCustom;
        this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
        if (this.DWObjectTableCode.indexOf("DIM_") != -1) {
            this.ParentDataTypeCode = "LookUp";
            this.ParentDimTabelName = DWObjectField.DWObjectTableCode;
        }
        else {
            this.ParentDataTypeCode = DWObjectField.DataTypeCode;
            this.ParentDimTabelName = DWObjectField.ParentDimTabelName;
        }
        this.Operators = this.GetFieldOperators(this);
        if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText")) {
            this.Operation = new ObjectFieldOperator("StartsWith", "Starts With");
        }
        else if ((this.ParentDataTypeCode == "Date" || this.ParentDataTypeCode == "DateTime")) {
            this.Operation = new ObjectFieldOperator("Before", "Before");
        }
        else {
            this.Operation = new ObjectFieldOperator("Equals", "Equals to");
        }
    };
    DWObjectFieldsDetails.prototype.onDeleteFilterClick = function () {
        this.MyParentClass.DeleteField(this, this.MyParentClass.SelectedFiltersDataSource);
    };
    DWObjectFieldsDetails.prototype.GetFieldOperators = function (field) {
        this.list = [];
        if (field.ParentDataTypeCode == "Text" || field.ParentDataTypeCode == "nText") {
            this.list.push(this.equalsOp);
            this.list.push(this.startsWithOp);
            this.list.push(this.IsNullOp);
            this.list.push(this.IsNotNullOp);
        }
        if (field.ParentDataTypeCode == "Integer" || field.ParentDataTypeCode == "UnsInteger"
            || field.ParentDataTypeCode == "Double" || field.ParentDataTypeCode == "SigDouble"
            || field.ParentDataTypeCode == "Decimal" || field.ParentDataTypeCode == "UnsDecimal") {
            this.list.push(this.largerThanOp);
            this.list.push(this.lessThanOp);
            this.list.push(this.equalsOp);
            this.list.push(this.greaterThanOrEqualOp);
            this.list.push(this.lessThanOrEqualOp);
        }
        if (field.ParentDataTypeCode == "LookUp" || field.ParentDataTypeCode == "Dimension" || field.ParentDataTypeCode == "PickList") {
            this.list.push(this.equalsOp);
            this.list.push(this.notEqualsOp);
            this.list.push(this.IsNullOp);
            this.list.push(this.IsNotNullOp);
        }
        if (field.ParentDataTypeCode == "Boolean") {
            this.list.push(this.equalsOp);
            this.list.push(this.notEqualsOp);
        }
        if (field.ParentDataTypeCode == "DateTime" || field.ParentDataTypeCode == "Date") {
            this.list.push(this.afterOp);
            this.list.push(this.beforeOp);
            this.list.push(this.previousOp);
            this.list.push(this.currentOp);
            this.list.push(this.nextOp);
            this.list.push(this.BetweenOp);
        }
        return this.list;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DWObjectFieldsDetails.prototype, "ShowSampleDateCommand", void 0);
    return DWObjectFieldsDetails;
}(BaseComponent_1.BaseComponent));
exports.DWObjectFieldsDetails = DWObjectFieldsDetails;
var ObjectFieldOperator = /** @class */ (function () {
    function ObjectFieldOperator(code, name) {
        this.Code = code;
        this.Name = name;
    }
    Object.defineProperty(ObjectFieldOperator.prototype, "Code", {
        get: function () { return this.code; },
        set: function (newValue) { this.code = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ObjectFieldOperator.prototype, "Name", {
        get: function () { return this.name; },
        set: function (newValue) { this.name = newValue; },
        enumerable: true,
        configurable: true
    });
    return ObjectFieldOperator;
}());
exports.ObjectFieldOperator = ObjectFieldOperator;
var DWFieldsGroup = /** @class */ (function () {
    function DWFieldsGroup(Key, FieldsList) {
        this.detailsIcon = "./Images/CellIcons/Arrowup.png";
        this.isDetailesOpened = false;
        this.Key = Key;
        this.FieldsList = FieldsList;
    }
    Object.defineProperty(DWFieldsGroup.prototype, "Key", {
        get: function () { return this.key; },
        set: function (newValue) { this.key = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWFieldsGroup.prototype, "FieldsList", {
        get: function () { return this.fieldsList; },
        set: function (newValue) { this.fieldsList = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWFieldsGroup.prototype, "DetailsIcon", {
        get: function () { return this.detailsIcon; },
        set: function (newValue) { this.detailsIcon = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWFieldsGroup.prototype, "IsDetailesOpened", {
        get: function () { return this.isDetailesOpened; },
        set: function (newValue) { this.isDetailesOpened = newValue; },
        enumerable: true,
        configurable: true
    });
    DWFieldsGroup.prototype.GroupClicked = function () {
        this.IsDetailesOpened = !this.IsDetailesOpened;
        if (!this.IsDetailesOpened) {
            this.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
        }
        else {
            this.DetailsIcon = "./Images/CellIcons/Arrowup.png";
        }
    };
    return DWFieldsGroup;
}());
exports.DWFieldsGroup = DWFieldsGroup;
var MultiSelectedValue = /** @class */ (function () {
    function MultiSelectedValue() {
    }
    return MultiSelectedValue;
}());
exports.MultiSelectedValue = MultiSelectedValue;
var ValueDetails = /** @class */ (function () {
    function ValueDetails() {
    }
    return ValueDetails;
}());
exports.ValueDetails = ValueDetails;
//# sourceMappingURL=DWQueryBuilderComponent.js.map