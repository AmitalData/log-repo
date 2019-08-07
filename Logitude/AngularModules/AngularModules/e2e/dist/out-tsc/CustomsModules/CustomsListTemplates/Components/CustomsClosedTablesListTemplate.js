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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var Args_1 = require("../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ClosedTableStatusListService_1 = require("../../../Customs/Services/StandardLists/ClosedTableStatusListService");
var IIGGeneralMessagesService_1 = require("../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var SystemTableRequestParams_1 = require("../../../Customs/DataContract/RequestParams/SystemTableRequestParams");
var RequestParamsBase_1 = require("../../../Customs/DataContract/RequestParams/RequestParamsBase");
var CustomsClosedTablesListTemplate = /** @class */ (function () {
    function CustomsClosedTablesListTemplate(CD) {
        //        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
        this.CD = CD;
        this.TableUpdateButtonIsEnabled = false;
        this.UpdateButtonVisibility = false;
        this.TableUpdateButtonOpacity = "1";
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (Tools_1.AppTool.IsNullOrEmpty(CustomsClosedTablesListTemplate_1.translate_CommunicationLogBView)) {
            this._entityResourceService.getEntityResourceByTableName("CommunicationLog")
                .subscribe(function (response) {
                CustomsClosedTablesListTemplate_1.translate_CommunicationLogBView = TextCodeTranslator_1.TextCodeTranslator.Translate("CommunicationLog.B.View"); // itzik : Translate +_entityResourceService - its bad :due that i done this- 
            });
        }
    }
    CustomsClosedTablesListTemplate_1 = CustomsClosedTablesListTemplate;
    Object.defineProperty(CustomsClosedTablesListTemplate.prototype, "CommunicationLogBView", {
        // itzik : Translate +_entityResourceService - its bad :due that i done this- 
        get: function () {
            return CustomsClosedTablesListTemplate_1.translate_CommunicationLogBView;
        },
        enumerable: true,
        configurable: true
    });
    CustomsClosedTablesListTemplate.prototype.setVariables = function (CustomsClosedTable, fieldName) {
        ///console.log(rowData);
        this._CustomsClosedTable = CustomsClosedTable;
        this.fieldName = fieldName;
        this.RefreshFields();
    };
    CustomsClosedTablesListTemplate.prototype.RefreshFields = function () {
        if (this._CustomsClosedTable.Existed) {
            this.UpdateButtonVisibility = true;
        }
        if (this._CustomsClosedTable.StatusCode != "2") {
            this.TableUpdateButtonIsEnabled = true; // 
            this.TableUpdateButtonOpacity = "1";
        }
        else {
            this.TableUpdateButtonIsEnabled = false;
            this.TableUpdateButtonOpacity = "0.7";
        }
        this.CD.detectChanges();
    };
    CustomsClosedTablesListTemplate.prototype.TableUpdateButtonCommandAction = function () {
        var _this = this;
        if (this.TableUpdateButtonIsEnabled) {
            this.UpdateClosedTable();
            return;
        }
        var confirm = new ConfirmWindow_1.ConfirmWindow();
        var text = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.M.AlreadySendReSend");
        if (Tools_1.AppTool.IsNullOrEmpty(text)) {
            text = "יש בקשה זהה בתהליך, האם להמשיך ?";
        }
        confirm.Show(text);
        confirm.WindowClosed.subscribe(function (event) {
            if (confirm.Yes) {
                _this.UpdateClosedTable();
            }
        });
    };
    CustomsClosedTablesListTemplate.prototype.ShowDetailsNotExistTable = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 850;
        logitudeWindow.Height = 500;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = this._CustomsClosedTable.CustomsLocalName;
        logitudeWindow.WindowArgs = this._CustomsClosedTable;
        //logitudeWindow.Show('./Customs/Components/Maintenance/ClosedTableNotExistedComponent');
        logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/ClosedTableNotExistedComponent');
        return;
    };
    CustomsClosedTablesListTemplate.prototype.ShowDetails = function () {
        var _this = this;
        if (!this._CustomsClosedTable.Existed) {
            this.ShowDetailsNotExistTable();
            return;
        }
        var objectTablePM = //window.ObjectTables.filter(d => d.Id == ObjectTableId)[0];
         window.ObjectTables.filter(function (t) { return t.Name == _this._CustomsClosedTable.ObjectTableName; })[0];
        //var objectTablePM =   window.ObjectTables.filter(d => d.Id == ObjectTableId)[0];
        if (objectTablePM) {
            var allQueries = window.Queries.filter(function (x) { return x.ObjectTableId === objectTablePM.Id; }).sort(function (a, b) {
                return a.IndexOrder - b.IndexOrder;
            });
            if (allQueries.length == 0) {
                //var myConfirmWindow = new ConfirmWindow();
                //myConfirmWindow.Show("No Queries found for " + item.TranslatedName);
            }
            else {
                var listArgs = new Args_1.ListComponentArgs();
                var SelectedQuery = null;
                SelectedQuery = allQueries[0];
                listArgs.QueryCode = SelectedQuery.Code;
                listArgs.ObjectTableName = objectTablePM.Name;
                switch (listArgs.ObjectTableName) {
                    case 'Customs.GovernmentProcedureType':
                    case "Customs.NotificationDefinition":
                    case "Customs.CustomsHouseType":
                    case "Customs.CustomDocumentType":
                    case "Customs.UIMessage":
                        //case "Customs.InternationalSite":
                        listArgs.SuppressOnRowSelected = false;
                        break;
                    default:
                        listArgs.SuppressOnRowSelected = true;
                        break;
                }
                listArgs.BackButtonTitle = "Maintenance";
                this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                    listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(SelectedQuery.NameTextCodeCode);
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        //this.CurrentSession.AddMenuReference(cmpRef);
                    });
                });
            }
        }
    };
    CustomsClosedTablesListTemplate.prototype.UpdateClosedTable = function () {
        //this.TableUpdateButtonIsEnabled = false;
        var _this = this;
        var myClosedTableStatusListService = new ClosedTableStatusListService_1.ClosedTableStatusListService();
        //this.CurrentSession.StartBusyIndicator("");
        myClosedTableStatusListService.getSingleFromCache("2").subscribe(function (result) {
            var status = result.Result;
            _this._CustomsClosedTable.StatusName = status.LocalName;
            _this._CustomsClosedTable.LastUpdateDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), 0);
            _this._CustomsClosedTable.StatusCode = "2";
            //this._CustomsClosedTable.StatusName = "מעדכן";///<span _ngcontent-hdd-22="">מעדכן</span>
            _this.RefreshFields();
        });
        var systemTableRequestParams = new SystemTableRequestParams_1.SystemTableRequestParams();
        systemTableRequestParams.TableId = this._CustomsClosedTable.Id;
        systemTableRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        systemTableRequestParams.RequestVIA == RequestParamsBase_1.SendRequestVIA.WebServiceBatch; //all the time 
        var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
        myIIGGeneralMessagesService.PostUpdateClosedTables(systemTableRequestParams).subscribe(function (res) { });
    };
    var CustomsClosedTablesListTemplate_1;
    CustomsClosedTablesListTemplate.translate_CommunicationLogBView = "";
    CustomsClosedTablesListTemplate = CustomsClosedTablesListTemplate_1 = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: 'CustomsClosedTablesListTemplate.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], CustomsClosedTablesListTemplate);
    return CustomsClosedTablesListTemplate;
}());
exports.CustomsClosedTablesListTemplate = CustomsClosedTablesListTemplate;
//# sourceMappingURL=CustomsClosedTablesListTemplate.js.map