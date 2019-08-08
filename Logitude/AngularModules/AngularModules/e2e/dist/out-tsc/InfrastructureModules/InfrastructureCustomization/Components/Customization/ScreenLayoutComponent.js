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
var GeneralDomainService_1 = require("../../../../Infrastructure/Services/GeneralDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ScreenFieldPM_1 = require("../../../../Infrastructure/EntityPMs/ScreenFieldPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ScreenLayoutArgs_1 = require("../../../../Infrastructure/DataContracts/ScreenLayoutArgs");
var LoginService_1 = require("../../../../Infrastructure/Services/LoginService");
var http_1 = require("@angular/http");
var ScreenLayoutComponent = /** @class */ (function (_super) {
    __extends(ScreenLayoutComponent, _super);
    function ScreenLayoutComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.MyArgs = new ScreenLayoutArgs_1.ScreenLayoutArgs();
        _this.TableScreensCollection = [];
        //public TabsList: ObservableCollection;
        //public CountText: number = 0;
        _this.ValidationErrorsList = [];
        _this.banckStackFields = [];
        _this.AllbanckStackFields = [];
        _this.currentScreenFields = [];
        _this.currentObjectFields = [];
        _this.ScreenRows = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsMouseOver = false;
        _this.myService = new EntityResourceService_1.EntityResourceService();
        _this.myGeneralService = new GeneralDomainService_1.GeneralDomainService();
        _this.loginService = new LoginService_1.LoginService();
        return _this;
        //this.TabsList = new ObservableCollection([]);            
    }
    ScreenLayoutComponent.prototype.SelectionChanged = function (Item) {
        var _this = this;
        //this.OkClicked(false);
        this.SelectedItem = Item;
        this.FillbanckStackFields();
        this.myGeneralService.GetScreenModificationByScreenId(Item.ScreenPM.Id).subscribe(function (myResult) {
            var myResponse = myResult;
            if (myResponse.Result != null) {
                _this.GenerateScreen(myResponse.Result);
            }
            else {
                _this.GenerateScreen(Item.ScreenPM);
            }
        });
    };
    ScreenLayoutComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.ObjecttableId = windowArgs.ObjectTableID;
        this.FillTableScreensCollection();
    };
    ScreenLayoutComponent.prototype.GenerateScreen = function (Item) {
        this.ScreenRows = [];
        for (var i = 0; i < Item.NumberOfColumns; i++) {
            var RDetails = new ScreenRowDetails();
            RDetails.ColumnIndex = i;
            var myFields = this.currentScreenFields.filter(function (a) { return a.Column == i; }).sort(function (a, b) { return a.Row - b.Row; });
            myFields.forEach(function (field) {
                if (RDetails.ScreenFieldPMs == null) {
                    RDetails.ScreenFieldPMs = [];
                }
                if (RDetails.ObjectFieldPMs == null) {
                    RDetails.ObjectFieldPMs = [];
                }
                RDetails.ScreenFieldPMs.push(field);
                var myOField = window.ObjectFields.filter(function (a) { return a.Id == field.ObjectFieldId; })[0];
                RDetails.ObjectFieldPMs.push(myOField);
            });
            this.ScreenRows.push(RDetails);
        }
    };
    ScreenLayoutComponent.prototype.FillTableScreensCollection = function () {
        var _this = this;
        this.TableScreensCollection = [];
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjecttableId; })[0];
        var Screens = window.Screens.filter(function (d) { return d.ObjectTableId == _this.ObjecttableId; });
        if (Screens.length > 0) {
            Screens.forEach(function (screen) {
                var name = screen.Name;
                if (table.Name == "Shipment") {
                    if (screen.Code.indexOf("Master") > -1) {
                        name += " (Master)";
                    }
                }
                var item = new ScreenItem();
                item.ScreenPM = screen;
                item.Name = name;
                _this.TableScreensCollection.push(item);
            });
            this.SelectionChanged(this.TableScreensCollection[0]);
        }
    };
    ScreenLayoutComponent.prototype.FillbanckStackFields = function () {
        var _this = this;
        this.banckStackFields = [];
        this.AllbanckStackFields = [];
        var table = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjecttableId; })[0];
        this.currentObjectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId == _this.ObjecttableId && !d.IsCustomFilter && d.PMPropertyPath != null && d.DataTypeCode != null && !d.IsMulti; }); //.Where(o => !d.IsCustomFilter && d.PMPropertyPath != null).OrderBy(f => f.FullNameTextCodeDefaultText).ToList();
        this.currentObjectFields = this.currentObjectFields.sort(function (a, b) { return (a.FullNameTextCodeDefaultText.toLowerCase() === b.FullNameTextCodeDefaultText.toLowerCase()) ? 0 : (a.FullNameTextCodeDefaultText.toLowerCase() < b.FullNameTextCodeDefaultText.toLowerCase()) ? -1 : 1; }); //.OrderBy(f => f.FullNameTextCodeDefaultText).ToList();
        if (this.SelectedItem) {
            this.currentScreenFields = window.ScreenFields.filter(function (sf) { return sf.Tenant == SessionLocator_1.SessionLocator.Tenant && sf.ScreenId == _this.SelectedItem.ScreenPM.Id; });
            if (this.currentScreenFields.length == 0) {
                this.currentScreenFields = window.ScreenFields.filter(function (sf) { return sf.Tenant == 0 && sf.ScreenId == _this.SelectedItem.ScreenPM.Id; });
            }
        }
        else {
            this.currentScreenFields = [];
        }
        this.currentObjectFields.forEach(function (objectField) {
            if (_this.currentScreenFields.filter(function (sf) { return sf.ObjectFieldId == objectField.Id; }).length == 0) {
                //if (objectField.DataTypeCode != null && !objectField.IsMulti) {
                _this.banckStackFields.push(objectField);
                _this.AllbanckStackFields.push(objectField);
                //}
            }
        });
        //this.SelectionChanged(this.ListBoxItemSource[0]);      
    };
    ScreenLayoutComponent.prototype.ListBoxSelectionMethod = function (Item) {
        //this.TabsList.Clear();
        //this.CountText = 0;
        //var myService: GeneralDomainService = new GeneralDomainService();
        //myService.GetTranslationsByParam(Item.Code, this.ObjecttableId, InfraSettings.TenantPM.Language).subscribe((myResult: ServiceResponse) => {
        //    if (myResult) {
        //        this.TranslationList = myResult.Result;           
        //        this.TabsList.InsertCollection(myResult.Result);
        //        this.CountText = myResult.Result.length;
        //    }
        //});       
    };
    ScreenLayoutComponent.prototype.CancelClicked = function () { this.CurrentSession.CloseCurrentWindow(); };
    ScreenLayoutComponent.prototype.OkClicked = function (CloseWindow) {
        var _this = this;
        if (CloseWindow === void 0) { CloseWindow = true; }
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
        var ScreenId = this.SelectedItem.ScreenPM.Id;
        //this.MyArgs.RemovedScreenFields = [];
        this.MyArgs.ScreenFields = [];
        var Columns = 0;
        var Rows = 0;
        this.ScreenRows.forEach(function (sItem) {
            Columns++;
            if (sItem.ScreenFieldPMs) {
                sItem.ScreenFieldPMs.forEach(function (myfield) {
                    Rows++;
                    ScreenId = myfield.ScreenId;
                    _this.MyArgs.ScreenFields.push(myfield);
                });
            }
        });
        this.MyArgs.Columns = Columns;
        this.MyArgs.Rows = Math.ceil(Rows / Columns);
        this.MyArgs.ScreenId = ScreenId; //this.SelectedItem.ScreenPM.Id;
        this.myGeneralService.updateScreenFields(this.MyArgs).subscribe(function (myResult) {
            _this.authHeader = new http_1.Headers();
            _this.authHeader.append('Content-Type', 'application/json');
            _this.authHeader.append('Accept', 'application/json');
            _this.loginService.AuthHeader = _this.authHeader;
            _this.loginService.CurrentTenant = SessionLocator_1.SessionLocator.Tenant;
            _this.loginService.GetScreenFields().subscribe(function (myResult) {
                if (myResult != null) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    if (CloseWindow == true) {
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                    window.ScreenFields = myResult;
                    _this.currentScreenFields = window.ScreenFields.filter(function (sf) { return sf.Tenant == SessionLocator_1.SessionLocator.Tenant && sf.ScreenId == ScreenId; });
                    if (_this.currentScreenFields.length == 0) {
                        _this.currentScreenFields = window.ScreenFields.filter(function (sf) { return sf.Tenant == 0 && sf.ScreenId == ScreenId; });
                    }
                }
                _this.loginService.GetScreens().subscribe(function (myScreensResult) {
                    window.Screens = myScreensResult;
                });
            });
        });
    };
    ScreenLayoutComponent.prototype.OnMyMouseDown = function (event) {
        console.log("Here we Go ..");
    };
    ScreenLayoutComponent.prototype.OnMyMouseUp = function (event) {
        if (this.IsMouseOver == true) {
            console.log("Here we Go Up..");
        }
    };
    ScreenLayoutComponent.prototype.onMyDragEnter = function ($event) {
        event.preventDefault();
    };
    ScreenLayoutComponent.prototype.onMyDrop = function (event, item, row) {
        var id = event.dataTransfer.getData("Id");
        var myitem = this.banckStackFields.filter(function (d) { return d.Id == id; })[0];
        if (myitem) {
            var test = window.ObjectFields.filter(function (a) { return a.Id == myitem.Id; })[0];
            if (Tools_1.AppTool.IsNullOrEmpty(test.ObjectTableName)) {
                var table = window.ObjectTables.filter(function (d) { return d.Id == test.ObjectTableId; })[0];
                window.ObjectFields.filter(function (a) { return a.Id == myitem.Id; })[0].ObjectTableName = table.Name;
            }
            this.banckStackFields = this.banckStackFields.filter(function (d) { return d.Id != id; });
            this.AllbanckStackFields = this.AllbanckStackFields.filter(function (d) { return d.Id != id; });
            if (this.ScreenRows) {
                var Rows = this.ScreenRows.filter(function (a) { return a.ColumnIndex == item.ColumnIndex; })[0]; //.push(item); 
                if (Rows.ScreenFieldPMs == null) {
                    Rows.ScreenFieldPMs = [];
                }
                if (Rows.ObjectFieldPMs == null) {
                    Rows.ObjectFieldPMs = [];
                }
                var screenField = new ScreenFieldPM_1.ScreenFieldPM();
                screenField.Column = item.ColumnIndex;
                screenField.ObjectFieldId = myitem.Id;
                screenField.ScreenId = this.SelectedItem.ScreenPM.Id;
                screenField.Tenant = SessionLocator_1.SessionLocator.Tenant;
                screenField.Row = Rows.ScreenFieldPMs.length;
                Rows.ScreenFieldPMs.push(screenField);
                Rows.ObjectFieldPMs.push(myitem);
            }
        }
        else {
            var Rows = this.ScreenRows.filter(function (a) { return a.ColumnIndex == item.ColumnIndex; })[0];
            this.ScreenRows.forEach(function (sItem) {
                if (sItem.ObjectFieldPMs) {
                    var temp = sItem.ObjectFieldPMs.filter(function (a) { return a.Id == id; });
                    if (temp.length > 0) {
                        var SField = sItem.ScreenFieldPMs.filter(function (a) { return a.ObjectFieldId == id; })[0];
                        sItem.ObjectFieldPMs = sItem.ObjectFieldPMs.filter(function (a) { return a.Id != id; });
                        sItem.ScreenFieldPMs = sItem.ScreenFieldPMs.filter(function (a) { return a.ObjectFieldId != id; });
                        SField.Column = item.ColumnIndex;
                        SField.Row = Rows.ScreenFieldPMs ? Rows.ScreenFieldPMs.length : 0;
                        if (Rows.ScreenFieldPMs == null) {
                            Rows.ScreenFieldPMs = [];
                        }
                        if (Rows.ObjectFieldPMs == null) {
                            Rows.ObjectFieldPMs = [];
                        }
                        Rows.ObjectFieldPMs.push(temp[0]);
                        Rows.ScreenFieldPMs.push(SField);
                    }
                    if (sItem.ColumnIndex == item.ColumnIndex) {
                        sItem.ScreenFieldPMs.forEach(function (myfield) {
                            myfield.Row = sItem.ScreenFieldPMs.indexOf(myfield);
                        });
                    }
                }
            });
        }
    };
    ScreenLayoutComponent.prototype.OnObjectFieldDragStart = function (event, item) {
        if (item) {
            event.dataTransfer.setData("Id", item.Id);
        }
    };
    ScreenLayoutComponent.prototype.OnScreenFieldDragStart = function (event, item1) {
        if (item1) {
            event.dataTransfer.setData("Id", item1.Id);
        }
    };
    ScreenLayoutComponent.prototype.SearchTextChanged = function (value) {
        if (Tools_1.AppTool.IsNullOrEmpty(value)) {
            this.banckStackFields = this.AllbanckStackFields;
        }
        else {
            this.banckStackFields = this.AllbanckStackFields.filter(function (f) { return (f.FullNameTextCodeDefaultText.toLowerCase().indexOf(value.toLowerCase()) > -1); });
        }
    };
    ScreenLayoutComponent.prototype.OnDeleteField = function (item) {
        var _this = this;
        this.AllbanckStackFields.push(item);
        this.ScreenRows.forEach(function (sItem) {
            if (sItem.ObjectFieldPMs) {
                var temp = sItem.ObjectFieldPMs.filter(function (a) { return a.Id == item.Id; });
                if (temp.length > 0) {
                    sItem.ObjectFieldPMs = sItem.ObjectFieldPMs.filter(function (a) { return a.Id != item.Id; });
                    var myItem = sItem.ScreenFieldPMs.filter(function (a) { return a.ObjectFieldId == item.Id; })[0];
                    sItem.ScreenFieldPMs = sItem.ScreenFieldPMs.filter(function (a) { return a.ObjectFieldId != item.Id; });
                    if (_this.MyArgs.RemovedScreenFields == null) {
                        _this.MyArgs.RemovedScreenFields = [];
                    }
                    _this.MyArgs.RemovedScreenFields.push(myItem);
                }
                sItem.ScreenFieldPMs.forEach(function (myfield) {
                    myfield.Row = sItem.ScreenFieldPMs.indexOf(myfield);
                });
            }
        });
    };
    ScreenLayoutComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ScreenLayoutComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ScreenLayoutComponent);
    return ScreenLayoutComponent;
}(BaseComponent_1.BaseComponent));
exports.ScreenLayoutComponent = ScreenLayoutComponent;
var ScreenItem = /** @class */ (function (_super) {
    __extends(ScreenItem, _super);
    function ScreenItem() {
        return _super.call(this) || this;
    }
    return ScreenItem;
}(BaseComponent_1.BaseComponent));
exports.ScreenItem = ScreenItem;
var ScreenRowDetails = /** @class */ (function () {
    function ScreenRowDetails() {
    }
    return ScreenRowDetails;
}());
exports.ScreenRowDetails = ScreenRowDetails;
//# sourceMappingURL=ScreenLayoutComponent.js.map