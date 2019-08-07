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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../Infrastructure/Args");
var CustomsRequestMenuService_1 = require("../../../Customs/Services/Others/CustomsRequestMenuService");
var CustomsVendorPM_1 = require("../../../Customs/EntityPMs/CustomsVendorPM");
var CustomsRequestsComponent = /** @class */ (function () {
    function CustomsRequestsComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.DataContext = this;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    CustomsRequestsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Customs.Client").subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response2) {
                _this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe((function (resp) {
                    _this._CustomsRequestMenuService = new CustomsRequestMenuService_1.CustomsRequestMenuService();
                    //this.BuildCustomsList();
                    //this.ItemsSource = this.CustomsRequestMenuItems;
                    _this.ItemsSource = _this._CustomsRequestMenuService.CustomsRequestMenuItems;
                }));
            });
        });
    };
    CustomsRequestsComponent.prototype.Search = function (text) {
        var itemsSource = this._CustomsRequestMenuService.CustomsRequestMenuItems;
        if (Tools_1.AppTool.IsNullOrEmpty(text)) {
            this.ItemsSource = this._CustomsRequestMenuService.CustomsRequestMenuItems;
        }
        else {
            itemsSource = itemsSource.filter(function (f) { return f.TranslatedName != null; });
            itemsSource = itemsSource.filter(function (f) { return f.TranslatedName.toUpperCase().includes(text.toUpperCase()) || f.ScreenName.toUpperCase().includes(text.toUpperCase()); });
        }
        this.ItemsSource = itemsSource;
    };
    CustomsRequestsComponent.prototype.ItemClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            switch (item.ScreenName) {
                case 'Vendors':
                case 'Clients': { // Query 
                    this.OpenListQueryByObjectTable(item.objectTableName); // Abdullah: fill objectTableName when u build the item
                    break;
                }
                case 'SearchVendor': {
                    this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(function (response) {
                        _this._entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe(function (response) {
                            var vendor = new CustomsVendorPM_1.CustomsVendorPM();
                            vendor.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            vendor.VendorTypeCode = "1";
                            var args = {};
                            args.IsNewEntity = true;
                            args.EntityPM = vendor;
                            args.IsSearchMode = true; // yaron want to allowed to send response Even there is only VendorNum
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 960;
                            logWindow.Height = 570;
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.SearchVendors");
                            logWindow.WindowArgs = args;
                            logWindow.ShowCloseButton = true;
                            logWindow.Show('./CustomsModules/CustomsVendor/Components/NewEntity/NewVendorComponent');
                            logWindow.WindowClosed.subscribe(function ($event) {
                            });
                        });
                    });
                    break;
                }
                case 'NewVendor': {
                    this._entityResourceService.getEntityResourceByTableName("Customs.CustomsVendor").subscribe(function (response) {
                        _this._entityResourceService.getEntityResourceByTableName("Customs.VendorCommunication").subscribe(function (response) {
                            var vendor = new CustomsVendorPM_1.CustomsVendorPM();
                            vendor.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            vendor.VendorTypeCode = "1";
                            var args = {};
                            args.IsNewEntity = true;
                            args.EntityPM = vendor;
                            args.IsSearchMode = false; // yaron want to allowed to send response Even there is only VendorNum
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 960;
                            logWindow.Height = 570;
                            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vendor.O.New");
                            logWindow.WindowArgs = args;
                            logWindow.ShowCloseButton = true;
                            logWindow.Show('./CustomsModules/CustomsVendor/Components/Components/EditTabs/VendorEditComponent');
                            logWindow.WindowClosed.subscribe(function ($event) {
                            });
                        });
                    });
                    break;
                }
                default: {
                    this._CustomsRequestMenuService.ShowModal(item, "", null);
                    break;
                }
            }
        }
    };
    CustomsRequestsComponent.prototype.OpenListQueryByObjectTable = function (objectTableName) {
        var _this = this;
        var listArgs = new Args_1.ListComponentArgs();
        var SelectedQuery = null;
        // Get ObjectTable 
        var objectTablePM = window.ObjectTables.filter(function (d) { return d.Name == objectTableName; })[0];
        if (Tools_1.AppTool.IsNullOrEmpty(objectTablePM)) {
            console.log("[!] No ObjectTable found for " + objectTableName);
            return;
        }
        // Get Query
        var allQueries = window.Queries.filter(function (x) { return x.ObjectTableId === objectTablePM.Id; }).sort(function (a, b) { return a.IndexOrder - b.IndexOrder; });
        if (allQueries.length == 0) {
            console.log("[!] No Queries found for " + objectTablePM.Name);
            return;
        }
        SelectedQuery = allQueries.filter(function (f) { return ((f.UserId == SessionLocator_1.SessionLocator.LoggedUserId && f.Tenant == SessionLocator_1.SessionLocator.Tenant) || f.Tenant == 0); })[0];
        listArgs.QueryCode = SelectedQuery.Code;
        listArgs.ObjectTableName = objectTablePM.Name;
        listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.Customs"); // Customs Request--> General.MH.Customs | Customs-->Customs.General.O.Customs
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(SelectedQuery.NameTextCodeCode);
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
            });
        });
    };
    CustomsRequestsComponent.prototype.OpenListQueryByQueryCode = function (queryCode) {
        var _this = this;
        var listArgs = new Args_1.ListComponentArgs();
        var SelectedQuery = null;
        // Get Query
        var allQueries = window.Queries.filter(function (x) { return x.Code === queryCode; }).sort(function (a, b) { return a.IndexOrder - b.IndexOrder; });
        if (allQueries.length == 0) {
            console.log("[!] No Queries found for " + queryCode);
            return;
        }
        SelectedQuery = allQueries.filter(function (f) { return ((f.UserId == SessionLocator_1.SessionLocator.LoggedUserId && f.Tenant == SessionLocator_1.SessionLocator.Tenant) || f.Tenant == 0); })[0];
        // Get ObjectTable 
        var objectTablePM = window.ObjectTables.filter(function (d) { return d.Id == SelectedQuery.ObjectTableId; })[0];
        if (Tools_1.AppTool.IsNullOrEmpty(objectTablePM)) {
            console.log("[!] No ObjectTable found for query " + SelectedQuery);
            return;
        }
        listArgs.QueryCode = SelectedQuery.Code;
        listArgs.ObjectTableName = objectTablePM.Name;
        listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Customs"); // Customs Request
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
            listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(SelectedQuery.NameTextCodeCode);
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
            });
        });
    };
    CustomsRequestsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsRequestsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], CustomsRequestsComponent);
    return CustomsRequestsComponent;
}());
exports.CustomsRequestsComponent = CustomsRequestsComponent;
//# sourceMappingURL=CustomsRequestsComponent.js.map