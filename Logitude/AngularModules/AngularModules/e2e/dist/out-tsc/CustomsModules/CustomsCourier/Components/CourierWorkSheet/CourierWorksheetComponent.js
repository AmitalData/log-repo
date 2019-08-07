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
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var core_1 = require("@angular/core");
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var CourierMasterService_1 = require("../../../../Customs/Services/Others/CourierMasterService");
var CourierMasterValidator_1 = require("../../../../Customs/Validators/CourierMasterValidator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var DeclarationCourierStatusListService_1 = require("../../../../Customs/Services/StandardLists/DeclarationCourierStatusListService");
var EntityListService_1 = require("../../../../Infrastructure/Services/EntityListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var DropdownMenuFilterComponent_1 = require("./DropdownMenuFilterComponent");
var SendPayReadyLowRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/SendPayReadyLowRequestParams");
var SendALLCorrectRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/SendALLCorrectRequestParams");
var CourierWorksheetSharedDataService_1 = require("../../../../Customs/Services/DataChange/CourierWorksheetSharedDataService");
var CustomsSettingExtendedListService_1 = require("../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService");
var CourierWorksheetComponent = /** @class */ (function (_super) {
    __extends(CourierWorksheetComponent, _super);
    function CourierWorksheetComponent(_CourierWorksheetSharedDataService, entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this._CourierWorksheetSharedDataService = _CourierWorksheetSharedDataService;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.CourierMaster";
        _this.DataContext = _this;
        _this.ComponentBackground = "white";
        _this.CourierMasterValidator = new CourierMasterValidator_1.CourierMasterValidator();
        _this._CourierMasterService = new CourierMasterService_1.CourierMasterService();
        _this._DeclarationCourierStatusListService = new DeclarationCourierStatusListService_1.DeclarationCourierStatusListService();
        _this._EntityListService = new EntityListService_1.EntityListService();
        _this.MyDropdownMenuFilterComponent = new DropdownMenuFilterComponent_1.DropdownMenuFilterComponent(null, null);
        _this._ValidationErrors = [];
        _this._TabFilterList = [];
        _this._SelectedBOLValue = 'A'; //ALL//High//Low
        _this._SelectedStatusValue = 'A'; //ALL//Open//Close
        _this._SelectedAvailableValue = 'A'; //ALL//Available//NotAvailable//Additional
        _this._SelectedTotalInvoiceValue = 'A';
        _this._SelectedMNFValue = 'A'; // ALL/Complete/Wrong
        _this._SelectedDECValue = 'A'; // ALL/Complete/Wrong_SelectedItems
        _this._SelectedDOCValue = 'A'; // All/Correction/CorrectionUploaded
        _this._SelectedACCValue = 'A'; // Wrong/WrongSpecial
        _this.columns = null;
        _this.IsActionButtonsEnabled = false;
        _this.IsLoaded = false;
        _this.IsFiltered = false;
        _this.IsMamanEnabled = false;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.CustomBackFromEditevent = new core_1.EventEmitter();
        //constructor(public entityArgs: EntityArgs) {
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this._ReadyDECToBatchSend = 0;
        _this._ReadyMNFToBatchSend = 0;
        _this._ReadyLOWPAYToBatchSend = 0;
        _this._SVGTotal = 0;
        _this._DOCTotal = 0;
        _this._DOC_U_Total = 0;
        _this._DOC_C_Total = 0;
        _this._DEC_W_Total = 0;
        _this._DEC_C_Total = 0;
        _this._MNF_W_Total = 0;
        _this._MNF_C_Total = 0;
        _this._ACC_W_Total = 0;
        _this._ACC_WS_Total = 0;
        _this._CorrectMNFToBatchSend = 0;
        _this._CorrectDECToBatchSend = 0;
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            //sortingCol: "CourierHawb",
            //sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.MyScrollTop = 0;
        _this.preventSelect = false;
        //this.entityPM = entityArgs.EntityPM;
        _this._TabFilterList.push(new TabFilter("ALL", "כל הש.מ.ב ", null, null));
        _this._TabFilterList.push(new TabFilter("DOC", "בעיות במסמכים ", null, null));
        _this._TabFilterList.push(new TabFilter("SVG", "בעיות בסיווג", null, null));
        _this._TabFilterList.push(new TabFilter("MNF", "בעיות במצהר ", null, null));
        _this._TabFilterList.push(new TabFilter("DEC", "בעיות בהצהרה", null, null));
        _this._TabFilterList.push(new TabFilter("PAY", "תשלום", null, null));
        _this._TabFilterList.push(new TabFilter("HOLD", "Pending", null, null));
        _this._TabFilterList.push(new TabFilter("ACC", "מסוף", null, null));
        _this._SelectedTabFilter = _this._TabFilterList[0];
        //this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
        //this.PseventRowSelectEventSubscribe =
        //    this.CurrentSession.PseventRowSelectEvent.subscribe(
        //        (res) => {
        //            if (res == "CourierWorksheetListTemplate.SendSplitButton") {
        //                this.preventSelect = true;
        //            }
        //        });
        //);
        _this.GetMamanPUR();
        return _this;
    }
    Object.defineProperty(CourierWorksheetComponent.prototype, "RowsItems", {
        get: function () {
            return this._RowsItems;
        },
        set: function (value) {
            this._RowsItems = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierWorksheetComponent.prototype, "SelectedRow", {
        get: function () {
            return this._SelectedRow;
        },
        set: function (value) {
            this._SelectedRow = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierWorksheetComponent.prototype, "SelectedTabFilter", {
        set: function (val) { this._SelectedTabFilter = val; },
        enumerable: true,
        configurable: true
    });
    //PseventRowSelectEventSubscribe: any;
    CourierWorksheetComponent.prototype.ngOnDestroy = function () {
        //  this.PseventRowSelectEventSubscribe.unSubscribe();
    };
    CourierWorksheetComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.BuildColumns();
        this._CourierWorksheetSharedDataService.CurrentMessage
            .subscribe(function (message) {
            if (message == "DoRefresh") {
                _this.RefreshButtonClicked();
            }
        });
    };
    CourierWorksheetComponent.prototype.TabFilterClick = function (item) {
        this._CourierWorksheetSharedDataService._SelectedItems.Collection = [];
        this._SelectedTabFilter = item;
        this._SelectedMNFValue = 'A';
        this._SelectedDECValue = 'A';
        this._SelectedDOCValue = 'A';
        this._SelectedACCValue = 'A';
        switch (item.Code) {
            case "DECR":
                this._ReadyDECToBatchSend = item.Value;
                break;
            case "MNF":
                this._SelectedMNFValue = 'C';
                break;
            case "DEC":
                this._SelectedDECValue = 'C';
                break;
            case "DOC":
                this._SelectedDOCValue = 'A';
                break;
            case "ACC":
                this._SelectedACCValue = 'W';
                break;
        }
        this.RefreshButtonClicked();
    };
    CourierWorksheetComponent.prototype.SetWindowArgs = function (windowArgs) {
        this.entityPM = windowArgs.CurrentEntity;
        this.CheckRequiredFields();
        this.RefreshButtonClicked();
    };
    CourierWorksheetComponent.prototype.CheckRequiredFields = function () {
        var _this = this;
        this._CourierMasterService.GetRequiredFieldsForCourierMaster(this.entityPM.Id).subscribe(function (response) {
            if (response.Result) {
                _this._ValidationErrors = _this.GetRequiredErrorsList(response.Result.RequiredFields);
            }
        });
    };
    CourierWorksheetComponent.prototype.GetRequiredErrorsList = function (errorsList) {
        var errorsMessages = [];
        errorsList.forEach(function (error) {
            if (!Tools_1.AppTool.IsNullOrEmpty(error.CustomMessageError)) {
                if (error.CustomMessageError.indexOf("specialerror") > -1) {
                    var ErrorMessage = "";
                    var errorArr = error.CustomMessageError.split(',');
                    ErrorMessage = errorArr[1] + TextCodeTranslator_1.TextCodeTranslator.Translate(errorArr[2]);
                    errorsMessages.push(ErrorMessage);
                }
                else {
                    errorsMessages.push(TextCodeTranslator_1.TextCodeTranslator.Translate(error.CustomMessageError));
                }
            }
            else {
                var table = window.ObjectTables.filter(function (d) { return d.Name === error.TableName; })[0];
                var field = window.ObjectFields.filter(function (d) { return d.FieldName == error.FieldName && d.ObjectTableId == table.Id; })[0];
                var message = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.FieldForTableIsRequired");
                var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate(field.FullNameTextCodeCode);
                var tableName = TextCodeTranslator_1.TextCodeTranslator.Translate(error.TableName);
                message = message.replace('%FieldName', fieldName);
                message = message.replace('%TableName', tableName);
                message = message.replace('%EntityReference', error.EntityReference);
                errorsMessages.push(message);
            }
        });
        return errorsMessages;
    };
    CourierWorksheetComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    CourierWorksheetComponent.prototype.onSearchTextChangeEvent = function (text) {
        this.SearchFilter = text;
        this.RefreshList();
    };
    CourierWorksheetComponent.prototype.SendALLCorrectManifest = function (courierDeclarationStatusCode) {
        var _this = this;
        if (this._ReadyMNFToBatchSend == 0 && courierDeclarationStatusCode == "R") {
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        if (this._CorrectMNFToBatchSend == 0 && courierDeclarationStatusCode == "RV") {
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        var currRequestParams = new SendALLCorrectRequestParams_1.SendALLCorrectRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.entityPM.Id;
        currRequestParams.HAWB = this.entityPM.HAWB;
        currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
        if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
        }
        this._CourierMasterService.PostSendALLCorrectManifest(currRequestParams)
            .subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Show(res.Result);
            myMessageWindow.WindowClosed.subscribe(function (s) {
                _this.RefreshButtonClicked();
            });
        });
    };
    CourierWorksheetComponent.prototype.SendALLCorrectManifest_OLD = function (courierDeclarationStatusCode) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        if (courierDeclarationStatusCode == "M") {
            if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
                var currRequestParams = new SendALLCorrectRequestParams_1.SendALLCorrectRequestParams();
                currRequestParams.LoggingEnabled = true;
                currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
                currRequestParams.CourierMasterId = this.entityPM.Id;
                currRequestParams.HAWB = this.entityPM.HAWB;
                currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
                this._CourierMasterService.PostSendALLCorrectManifest(currRequestParams)
                    .subscribe(function (res) {
                    _this.CurrentSession.StopBusyIndicator();
                    var myMessageWindow = new MessageWindow_1.MessageWindow();
                    myMessageWindow.Show(res.Result);
                    myMessageWindow.WindowClosed.subscribe(function (s) {
                        _this.RefreshButtonClicked();
                    });
                });
            }
        }
        else {
            this._CourierMasterService.GetSendALLCorrectManifest(this.entityPM.Id, this.entityPM.HAWB, courierDeclarationStatusCode)
                .subscribe(function (res) {
                _this.CurrentSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow_1.MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(function (s) {
                    _this.RefreshButtonClicked();
                });
            });
        }
    };
    CourierWorksheetComponent.prototype.SendReadyLOWPAYToBatch = function () {
        var _this = this;
        if (this._ReadyLOWPAYToBatchSend == 0) {
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        this.CurrentSession.StartBusyIndicatorLoading();
        this.CurrentSession.entityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentMethod", 0).subscribe(function (response) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Width = 500;
            logitudeWindow.Height = 300;
            logitudeWindow.Title = "בנק לתשלום";
            _this.CurrentSession.StopBusyIndicator();
            logitudeWindow.ComponentLoaded.subscribe(function (cmpRef) {
                //    cmpRef.IsClosedLost = true;
            });
            logitudeWindow.WindowClosed.subscribe(function (resultWindowClosed) {
                var InternalBankId = resultWindowClosed;
                if (!Tools_1.AppTool.IsNullOrEmpty(InternalBankId)) {
                    _this.CurrentSession.StartBusyIndicatorCreating();
                    if (_this._CourierWorksheetSharedDataService._SelectedItems != null && _this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
                        var currRequestParams = new SendPayReadyLowRequestParams_1.SendPayReadyLowRequestParams();
                        currRequestParams.LoggingEnabled = true;
                        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        currRequestParams.CourierMasterId = _this.entityPM.Id;
                        currRequestParams.HAWB = _this.entityPM.HAWB;
                        currRequestParams.InternalBankId = InternalBankId;
                        currRequestParams.Declarations = _this._CourierWorksheetSharedDataService._SelectedItems.Collection;
                        _this._CourierMasterService.PostSendPayReadyLow2755(currRequestParams)
                            .subscribe(function (res) {
                            _this.CurrentSession.StopBusyIndicator();
                            var myMessageWindow = new MessageWindow_1.MessageWindow();
                            myMessageWindow.Show(res.Result);
                            myMessageWindow.WindowClosed.subscribe(function (s) {
                                _this.RefreshButtonClicked();
                            });
                        });
                    }
                    else {
                        _this._CourierMasterService.GetSendPayReadyLow2755(_this.entityPM.Id, _this.entityPM.HAWB, InternalBankId)
                            .subscribe(function (res) {
                            _this.CurrentSession.StopBusyIndicator();
                            var myMessageWindow = new MessageWindow_1.MessageWindow();
                            myMessageWindow.Show(res.Result);
                            myMessageWindow.WindowClosed.subscribe(function (s) {
                                _this.RefreshButtonClicked();
                            });
                        });
                    }
                }
            });
            logitudeWindow.Show('./CustomsModules/CustomsCourier/Components/CourierWorkSheet/GetInternalBankComponent');
            //''
            ;
        });
    };
    CourierWorksheetComponent.prototype.SendALLCorrectDec = function (courierDeclarationStatusCode) {
        var _this = this;
        if (this._ReadyDECToBatchSend == 0 && courierDeclarationStatusCode == "R") {
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        if (this._CorrectDECToBatchSend == 0 && courierDeclarationStatusCode == "RV") {
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        var currRequestParams = new SendALLCorrectRequestParams_1.SendALLCorrectRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.CourierMasterId = this.entityPM.Id;
        currRequestParams.HAWB = this.entityPM.HAWB;
        if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
            currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
        }
        currRequestParams.CourierDeclarationStatusCode = courierDeclarationStatusCode;
        currRequestParams.SelectedAvailableValue = this._SelectedAvailableValue;
        currRequestParams.SelectedBOLValue = this._SelectedBOLValue;
        currRequestParams.SelectedStatusValue = this._SelectedStatusValue;
        currRequestParams.SelectedTotalInvoiceValue = this._SelectedTotalInvoiceValue;
        this._CourierMasterService.PostSendALLCorrectDec(currRequestParams)
            .subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Show(res.Result);
            myMessageWindow.WindowClosed.subscribe(function (s) {
                _this.RefreshButtonClicked();
            });
        });
        //this.SendALLCorrectDec_OLD(courierDeclarationStatusCode);
    };
    CourierWorksheetComponent.prototype.SendALLCorrectDec_OLD = function (courierDeclarationStatusCode) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        if (courierDeclarationStatusCode == "M") {
            if (this._CourierWorksheetSharedDataService._SelectedItems != null && this._CourierWorksheetSharedDataService._SelectedItems.Collection.length > 0) {
                var currRequestParams = new SendALLCorrectRequestParams_1.SendALLCorrectRequestParams();
                currRequestParams.LoggingEnabled = true;
                currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
                currRequestParams.CourierMasterId = this.entityPM.Id;
                currRequestParams.HAWB = this.entityPM.HAWB;
                currRequestParams.Declarations = this._CourierWorksheetSharedDataService._SelectedItems.Collection;
                currRequestParams.SelectedAvailableValue = this._SelectedAvailableValue;
                currRequestParams.SelectedBOLValue = this._SelectedBOLValue;
                currRequestParams.SelectedStatusValue = this._SelectedStatusValue;
                currRequestParams.SelectedTotalInvoiceValue = this._SelectedTotalInvoiceValue;
                this._CourierMasterService.PostSendALLCorrectDec(currRequestParams)
                    .subscribe(function (res) {
                    _this.CurrentSession.StopBusyIndicator();
                    var myMessageWindow = new MessageWindow_1.MessageWindow();
                    myMessageWindow.Show(res.Result);
                    myMessageWindow.WindowClosed.subscribe(function (s) {
                        _this.RefreshButtonClicked();
                    });
                });
            }
        }
        else {
            this._CourierMasterService.GetSendALLCorrectDec(this.entityPM.Id, this.entityPM.HAWB, courierDeclarationStatusCode)
                .subscribe(function (res) {
                _this.CurrentSession.StopBusyIndicator();
                var myMessageWindow = new MessageWindow_1.MessageWindow();
                myMessageWindow.Show(res.Result);
                myMessageWindow.WindowClosed.subscribe(function (s) {
                    _this.RefreshButtonClicked();
                });
            });
        }
    };
    CourierWorksheetComponent.prototype.SendALLSVG = function () {
        if (this._SVGTotal == 0) {
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Width = 250;
            myMessageWindow.Height = 150;
            myMessageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CourierMaster.O.NoResults"));
            return;
        }
        else { //task 42046
            this.Navigate();
        }
    };
    CourierWorksheetComponent.prototype.Navigate = function () {
        var _this = this;
        //        this.CurrentQueryFilters = new ApiQueryFilters();
        var MyFilters = new ApiQueryFilters_1.ApiQueryFilters();
        MyFilters.SortBy = this.currentSortingCol;
        MyFilters.SortDirection = this.currentSortingDir;
        this.BuildFiltersForQuery(MyFilters);
        MyFilters.GetCount = false;
        MyFilters.PageIndex = 0;
        MyFilters.PageSize = 100;
        //this.CurrentQueryFilters = MyFilters;
        var ids = [];
        this._EntityListService.getByFilters("Customs.DeclarationCourierStatus", MyFilters, null).then(function (observable) {
            observable.subscribe(function (response) {
                console.log(response);
                response.Result.forEach(function (item) {
                    ids.push(item.DeclarationId);
                });
                console.log(ids);
                var selectedEntityId = ids[0];
                _this.CurrentSession.CurrentWindow.SuppressBusyIndicator = true;
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    var label = "מסך עבודה"; //TextCodeTranslator.Translate(this.SelectedQuery.NameTextCodeCode);
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({
                        EntityId: selectedEntityId,
                        BackButtonLabel: label,
                        NavigationIds: ids,
                        SelectedTabCode: "DCCF",
                        ObjectTableName: "Customs.Declaration",
                    });
                    cmpRef.instance.BackCompleted.subscribe(function (bk) {
                        if (_this.CurrentSession != null && _this.CurrentSession.CurrentWindow != null) {
                            _this.CurrentSession.CurrentWindow.SuppressBusyIndicator = false;
                        }
                        _this.OnBackFromEdit(selectedEntityId, event);
                    }); //  if (SessionLocator.LoggedUserPM.Email == "mohammad@fnarsoft.com") {
                    //this.DestroyMe = true;
                    //}
                });
            });
        });
    };
    CourierWorksheetComponent.prototype.OnSortInvoked = function ($event) {
        this.currentSortingCol = $event.colDef;
        this.currentSortingDir = $event.id;
    };
    //////////////////////////////
    CourierWorksheetComponent.prototype.OnColumnResisedevent = function () { };
    CourierWorksheetComponent.prototype.RefreshButtonClicked = function () {
        //this.onQueryChangeEvent.emit({ Filters: this.filterAgrs, Reload: true });
        this.RefreshStatistic();
        this.RefreshMasterRequiredFields();
        this.RefreshList();
    };
    CourierWorksheetComponent.prototype.RefreshList = function () {
        var _this = this;
        setTimeout(function () {
            _this.MenuHeaderchangeevent.emit({ Filters: _this.filterAgrs, IgnoreFilter: false });
        }, 10);
    };
    CourierWorksheetComponent.prototype.RefreshStatistic = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetStatistic(this.entityPM.Id)
            .subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var list;
            list = res.Result;
            list.forEach(function (item) {
                switch (item.Key) {
                    case "DECR": {
                        //statements; 
                        _this._ReadyDECToBatchSend = item.Value;
                        break;
                    }
                    case "DECR_RV": {
                        //statements; 
                        _this._CorrectDECToBatchSend = item.Value;
                        break;
                    }
                    case "MNFR": {
                        //statements; 
                        _this._ReadyMNFToBatchSend = item.Value;
                        break;
                    }
                    case "MNFR_RV": {
                        //statements; 
                        _this._CorrectMNFToBatchSend = item.Value;
                        break;
                    }
                    case "SVG": {
                        //statements; 
                        _this._SVGTotal = item.Value;
                        var TabFilter = _this._TabFilterList.filter(function (d) { return d.Code == item.Key; })[0];
                        TabFilter.Total = item.Value;
                        break;
                    }
                    case "MNF_W": {
                        //statements; 
                        _this._MNF_W_Total = item.Value;
                        break;
                    }
                    case "MNF_C": {
                        _this._MNF_C_Total = item.Value;
                        break;
                    }
                    case "DOC": {
                        //statements; 
                        _this._DOCTotal = item.Value;
                        var TabFilter = _this._TabFilterList.filter(function (d) { return d.Code == item.Key; })[0];
                        TabFilter.Total = item.Value;
                        break;
                    }
                    case "DOC_U": {
                        //statements; 
                        _this._DOC_U_Total = item.Value;
                        break;
                    }
                    case "DOC_C": {
                        _this._DOC_C_Total = item.Value;
                        break;
                    }
                    case "DEC_C": {
                        _this._DEC_C_Total = item.Value;
                        break;
                    }
                    case "DEC_W": {
                        _this._DEC_W_Total = item.Value;
                        break;
                    }
                    case "PAY_RL": {
                        //statements; 
                        _this._ReadyLOWPAYToBatchSend = item.Value;
                        break;
                    }
                    case "ACC_W": {
                        _this._ACC_W_Total = item.Value;
                        break;
                    }
                    case "ACC_WS": {
                        _this._ACC_WS_Total = item.Value;
                        break;
                    }
                    default: {
                        //statements; 
                        var TabFilter = _this._TabFilterList.filter(function (d) { return d.Code == item.Key; })[0];
                        TabFilter.Total = item.Value;
                        break;
                    }
                }
            });
        });
    };
    CourierWorksheetComponent.prototype.RefreshMasterRequiredFields = function () {
    };
    CourierWorksheetComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: "MyDeclarationCheckBox",
            DataTypeCode: 'String',
            Display: '',
            IsCustomTemplate: true,
            Styles: { width: '27px' },
            //IsCheckBox: true
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });
        this.columns.push({
            FieldName: 'CourierHawb',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierHawb"),
            Styles: { width: '120px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });
        //SortByName: 'CourierHawb'
        this.columns.push({
            FieldName: 'ProcedureCurrentName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.ProcedureCurrentName"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'HighLowValue',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.HighLowValue"),
            Styles: { width: '70px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'ImporterName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CustomerName"),
            Styles: { width: '200px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'ImporterCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.ImporterCode"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'DocumentStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.DocumentStatusCode"),
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'IsCourierMissingClassification',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IsCourierMissingClassification"),
            Styles: { width: '55px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'CourierManifestStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierManifestStatusCode"),
            Styles: { width: '55px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'CourierDeclarationStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierDeclarationStatusCode"),
            Styles: { width: '55px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'CourierPaymentStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPaymentStatusCode"),
            Styles: { width: '55px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'CourierCustomStatusName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierCustomStatusName"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });
        this.columns.push({
            FieldName: 'MamanStatusCode',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.MamanStatusCode"),
            Styles: { width: '55px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });
        this.columns.push({
            FieldName: 'SpecialActionStatus',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.SpecialActionStatus"),
            Styles: { width: '55px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });
        this.columns.push({
            FieldName: 'DeclarationStatusTypeName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.DeclarationStatusTypeName"),
            Styles: { width: '200px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'CourierPendingReasonName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.CourierPendingReasonName"),
            Styles: { width: '150px' },
            IsCustomTemplate: true,
            ServerSideSortable: false,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
        });
        this.columns.push({
            FieldName: 'IsClosedForFollowUp',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IsClosedForFollowUp"),
            Styles: { width: '70px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'CourierPendingReasonCode',
            DataTypeCode: 'String',
            //Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IsClosedForFollowUp"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
        this.columns.push({
            FieldName: 'SendSplitButton',
            DataTypeCode: 'String',
            //Display: TextCodeTranslator.Translate("Customs.DeclarationCourierStatus.F.IsClosedForFollowUp"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CourierWorksheetListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CourierWorksheetListTemplate',
            ServerSideSortable: false,
        });
    };
    CourierWorksheetComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        /*        if (AppTool.IsNullOrEmpty(filters.SortBy)) {
                    filters.SortBy = "CourierHawb";
                }
                if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
                    filters.SortDirection = "Descending";
                }*/
        this.BuildFiltersForQuery(filters);
        var myout = this._EntityListService.getExtendedByFilters("Customs.DeclarationCourierStatus", filters);
        return myout;
    };
    /*getRowsOld(skip, take, sortingCol, sortingDir, getCount: boolean, searchfields?: string, filters: ApiQueryFilters = null) {
 
         if (filters == null) {
             filters = new ApiQueryFilters();
         }
 
         filters.PageSize = take;
         filters.PageIndex = skip;
         filters.GetAll = false;
         filters.GetCount = true;
 
         filters.SortBy = sortingCol;
         filters.SortDirection = sortingDir;
         if (AppTool.IsNullOrEmpty(filters.SortBy)) {
             filters.SortBy = "CourierHawb";
         }
         if (AppTool.IsNullOrEmpty(filters.SortDirection)) {
             filters.SortDirection = "Descending";
         }
         filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
         filters.addAdditionalFilter("Tenant", SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
 
         switch (this._SelectedTabFilter.Code) {
             case "ALL": {
                 break;
             }
             default: {
                 filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                 break;
             }
         }
 
         switch (this._SelectedBOLValue) {
             case "L": {
                 filters.addAdditionalFilter("HighLowValue", "L", null, null, "Equals", false, false, false, "string");
                 break;
             }
             case "H": {
                 filters.addAdditionalFilter("HighLowValue", "H", null, null, "Equals", false, false, false, "string");
                 break;
             }
         }
 
         switch (this._SelectedStatusValue) {
             case "O": {
                 filters.addAdditionalFilter("IsClosedForFollowUp", false, null, null, "Equals", false, false, false, "Boolean");
                 break;
             }
             case "C": {
                 filters.addAdditionalFilter("IsClosedForFollowUp", true, null, null, "Equals", false, false, false, "Boolean");
                 break;
             }
         }
 
         switch (this._SelectedMNFValue) {
             case "C": {
                 filters.addAdditionalFilter("CourierManifestStatusCode", "M", null, null, "Equals", false, false, false, "string");
                 break;
             }
             case "W": {
                 filters.addAdditionalFilter("CourierManifestStatusCode", "X", null, null, "Equals", false, false, false, "string");
                 break;
             }
         }
 
         switch (this._SelectedDECValue) {
             case "C": {
                 filters.addAdditionalFilter("CourierDeclarationStatusCode", "M", null, null, "Equals", false, false, false, "string");
                 break;
             }
             case "W": {
                 filters.addAdditionalFilter("CourierDeclarationStatusCode", "X", null, null, "Equals", false, false, false, "string");
                 break;
             }
         }
 
         switch (this._SelectedDOCValue) {
             case "C": {
                 filters.addAdditionalFilter("DocumentStatusCode", "M", null, null, "Equals", false, false, false, "string");
                 break;
             }
             case "U": {
                 filters.addAdditionalFilter("DocumentStatusCode", "X", null, null, "Equals", false, false, false, "string");
                 break;
             }
         }
 
         switch (this._SelectedTotalInvoiceValue) {
             case "75": {
                 filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 75, null, null, "LessThanOrEqual", false, false, false, "number");
                 break;
             }
             case "500": {
                 filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 76, 500, null, "Between", false, false, false, "number", false);
                 break;
             }
             case "1000": {
                 filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 501, 1000, null, "Between", false, false, false, "number", false);
                 break;
             }
         }
 
         switch (this._SelectedAvailableValue) {
             case "AD": {
                 filters.addAdditionalFilter("AcceptanceStatusCode", "2", null, null, "Equals", false, false, false, "string");
                 break;
             }
             case "AV": {
                 filters.addAdditionalFilter("AcceptanceStatusCode", "1", null, null, "Equals", false, false, false, "string");
                 break;
             }
             case "NAV": {
                 filters.addAdditionalFilter("AcceptanceStatusCode", "0", null, null, "Equals", false, false, false, "string");
                 break;
             }
         }
 
         switch (this._SelectedACCValue) {
             case "W": {
                 filters.addAdditionalFilter("MamanStatusCode", "2", null, null, "Equals", false, false, false, "string");
                 break;
             }
         }
 
         if (!AppTool.IsNullOrEmpty(this.SearchFilter)) {
             filters.addAdditionalFilter("CourierSearchFields", this.SearchFilter, null, null, "Contains", false, false, false, "string", false, true);
         }
 
 
         var myout = this._EntityListService.getExtendedByFilters("Customs.DeclarationCourierStatus", filters);
 
         return myout;
     }*/
    CourierWorksheetComponent.prototype.BuildFiltersForQuery = function (filters) {
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("Tenant", SessionLocator_1.SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        switch (this._SelectedTabFilter.Code) {
            case "ACC":
            case "ALL": {
                break;
            }
            default: {
                filters.addAdditionalFilter("Is" + this._SelectedTabFilter.Code + "Tab", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
        }
        switch (this._SelectedBOLValue) {
            case "L": {
                filters.addAdditionalFilter("HighLowValue", "L", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "H": {
                filters.addAdditionalFilter("HighLowValue", "H", null, null, "Equals", false, false, false, "string");
                break;
            }
        }
        switch (this._SelectedStatusValue) {
            case "O": {
                filters.addAdditionalFilter("IsClosedForFollowUp", false, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
            case "C": {
                filters.addAdditionalFilter("IsClosedForFollowUp", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
        }
        switch (this._SelectedMNFValue) {
            case "C": {
                filters.addAdditionalFilter("CourierManifestStatusCode", "M", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "W": {
                filters.addAdditionalFilter("CourierManifestStatusCode", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }
        switch (this._SelectedDECValue) {
            case "C": {
                filters.addAdditionalFilter("CourierDeclarationStatusCode", "M", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "W": {
                filters.addAdditionalFilter("CourierDeclarationStatusCode", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }
        switch (this._SelectedDOCValue) {
            case "C": {
                filters.addAdditionalFilter("DocumentStatusCode", "M", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "U": {
                filters.addAdditionalFilter("DocumentStatusCode", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }
        switch (this._SelectedTotalInvoiceValue) {
            case "75": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 75, null, null, "LessThanOrEqual", false, false, false, "number");
                break;
            }
            case "500": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 76, 500, null, "Between", false, false, false, "number", false);
                break;
            }
            case "1000": {
                filters.addAdditionalFilter("TotalInvoiceAmountInUSD", 501, 1000, null, "Between", false, false, false, "number", false);
                break;
            }
        }
        switch (this._SelectedAvailableValue) {
            case "AD": {
                filters.addAdditionalFilter("AcceptanceStatusCode", "2", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "AV": {
                filters.addAdditionalFilter("AcceptanceStatusCode", "1", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "NAV": {
                filters.addAdditionalFilter("AcceptanceStatusCode", "0", null, null, "Equals", false, false, false, "string");
                break;
            }
        }
        switch (this._SelectedACCValue) {
            case "W": {
                filters.addAdditionalFilter("MamanStatusCode", "2", null, null, "Equals", false, false, false, "string");
                break;
            }
            case "WS": {
                filters.addAdditionalFilter("SpecialActionStatus", "X", null, null, "Equals", false, false, false, "string");
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchFilter)) {
            filters.addAdditionalFilter("CourierSearchFields", this.SearchFilter, null, null, "Contains", false, false, false, "string", false, true);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(filters.SortBy)) {
            filters.SortBy = "CourierHawb";
        }
        if (Tools_1.AppTool.IsNullOrEmpty(filters.SortDirection)) {
            filters.SortDirection = "Descending";
        }
    };
    CourierWorksheetComponent.prototype.ViewInitCompleted = function ($event) {
        ///this.LoadNotifications();
    };
    CourierWorksheetComponent.prototype.MNFFilterClicked = function (value) {
        if (this._SelectedMNFValue != value) {
            this._SelectedMNFValue = value;
            this.RefreshList();
        }
    };
    CourierWorksheetComponent.prototype.DECFilterClicked = function (value) {
        if (this._SelectedDECValue != value) {
            this._SelectedDECValue = value;
            this.RefreshList();
        }
    };
    CourierWorksheetComponent.prototype.DOCFilterClicked = function (value) {
        if (this._SelectedDOCValue != value) {
            this._SelectedDOCValue = value;
            this.RefreshList();
        }
    };
    CourierWorksheetComponent.prototype.ACCFilterClicked = function (value) {
        if (this._SelectedACCValue != value) {
            this._SelectedACCValue = value;
            this.RefreshList();
        }
    };
    CourierWorksheetComponent.prototype.SelectedBOLValueClick = function (value) {
        this._SelectedBOLValue = value;
        if (this._SelectedBOLValue == "A" && this._SelectedTotalInvoiceValue == "A" && this._SelectedStatusValue == "A" && this._SelectedAvailableValue == "A") {
            this.IsFiltered = false;
        }
        else {
            this.IsFiltered = true;
        }
        this.RefreshList();
    };
    CourierWorksheetComponent.prototype.SelectedTotalInvoiceValue = function (value) {
        this._SelectedTotalInvoiceValue = value;
        if (this._SelectedBOLValue == "A" && this._SelectedTotalInvoiceValue == "A" && this._SelectedStatusValue == "A" && this._SelectedAvailableValue == "A") {
            this.IsFiltered = false;
        }
        else {
            this.IsFiltered = true;
        }
        this.RefreshList();
    };
    CourierWorksheetComponent.prototype.SelectedStatusValueClick = function (value) {
        this._SelectedStatusValue = value;
        if (this._SelectedBOLValue == "A" && this._SelectedTotalInvoiceValue == "A" && this._SelectedStatusValue == "A" && this._SelectedAvailableValue == "A") {
            this.IsFiltered = false;
        }
        else {
            this.IsFiltered = true;
        }
        this.RefreshList();
    };
    CourierWorksheetComponent.prototype.SelectedAvailableValueClick = function (value) {
        this._SelectedAvailableValue = value;
        if (this._SelectedBOLValue == "A" && this._SelectedTotalInvoiceValue == "A" && this._SelectedStatusValue == "A" && this._SelectedAvailableValue == "A") {
            this.IsFiltered = false;
        }
        else {
            this.IsFiltered = true;
        }
        this.RefreshList();
    };
    CourierWorksheetComponent.prototype.FilterCancelButtonClicked = function () {
        this.MyDropdownMenuFilterComponent.DropdowndisplayToggle(null);
        this.RefreshList();
    };
    CourierWorksheetComponent.prototype.FilterCleanButtonClicked = function () {
        this._SelectedBOLValue = 'A';
        this._SelectedStatusValue = 'A';
        this._SelectedAvailableValue = 'A';
        this._SelectedTotalInvoiceValue = 'A';
        this.IsFiltered = false;
        this.RefreshList();
    };
    CourierWorksheetComponent.prototype.OnRowSelected = function (event) {
        this.MyScrollTop = event.scrollTop;
        if (this._CourierWorksheetSharedDataService.SupperssOnRowSelectedAction) {
            this._CourierWorksheetSharedDataService.SupperssOnRowSelectedAction = false;
            return;
        }
        this.OnRowSelectedBL(event);
        //let timerToken = setTimeout(() => {
        //    this.preventSelect = false;
        //    clearTimeout(timerToken);
        //    this.OnRowSelectedBL(event);
        //}, 1000);
    };
    CourierWorksheetComponent.prototype.OnRowSelectedBL = function (event) {
        var _this = this;
        if (this.preventSelect) {
            return;
        }
        if (true) //(!this.preventSelect) {
            var selected = event.rowData;
        if (selected) {
            var customEditIdentityKey = Guid_1.Guid.newGuid();
            var control = null;
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            var currentScreenCode = "";
            var objectTableName = "";
            var filters = [];
            switch (this._SelectedTabFilter.Code) {
                case "MNF":
                    {
                        if (this._SelectedMNFValue == 'C') {
                            currentScreenCode = "DEGC";
                            objectTableName = "Customs.Declaration";
                        }
                        else if (this._SelectedMNFValue == 'W') {
                            currentScreenCode = "DCCA";
                            objectTableName = "Customs.Declaration";
                            var myfilters = {
                                "IsManifest": true,
                                "IsConstraintsVisible": false,
                            };
                            filters.push(myfilters);
                        }
                        break;
                    }
                case "DEC":
                    {
                        if (this._SelectedDECValue == 'C') {
                            currentScreenCode = "DEGC";
                            objectTableName = "Customs.Declaration";
                        }
                        else if (this._SelectedDECValue == 'W') {
                            currentScreenCode = "DCCA";
                            objectTableName = "Customs.Declaration";
                        }
                        break;
                    }
                case "DOC":
                    {
                        currentScreenCode = "DCCD";
                        objectTableName = "Customs.Declaration";
                        break;
                    }
                case "SVG":
                    {
                        currentScreenCode = "DCCF";
                        objectTableName = "Customs.Declaration";
                    }
                    break;
                default:
                    {
                        currentScreenCode = "DEGC";
                        objectTableName = "Customs.Declaration";
                        break;
                    }
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(currentScreenCode)) {
                if (objectTableName == "Customs.Declaration") {
                    this.CurrentSession.CurrentWindow.SuppressBusyIndicator = true;
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({
                            SelectedTabCode: currentScreenCode,
                            EntityId: selected.DeclarationId,
                            ObjectTableName: objectTableName,
                        });
                        cmpRef.instance.BackCompleted.subscribe(function (bk) {
                            if (_this.CurrentSession != null && _this.CurrentSession.CurrentWindow != null) {
                                _this.CurrentSession.CurrentWindow.SuppressBusyIndicator = false;
                            }
                            _this.OnBackFromEdit(selected.DeclarationId, event);
                        });
                        if (_this._SelectedTabFilter.Code == "MNF" && _this._SelectedMNFValue == 'W') {
                            cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
                                .subscribe(function (myResult) {
                                var myDeclarationEditComponentController = cmpRef.instance.EditComponentController;
                                myDeclarationEditComponentController.CustomsAnswersShowManifest = true;
                                console.log("myDeclarationEditComponentController.CustomsAnswersShowManifest = true;");
                            });
                        }
                        if (currentScreenCode = "DCCF") {
                            cmpRef.instance.OnFirstTimeAfterSingleDataLoaded
                                .subscribe(function (myResult) {
                                var myDeclarationEditComponentController = cmpRef.instance.EditComponentController;
                                myDeclarationEditComponentController.ShowDeclarationClassificationComponentTAB = true;
                                console.log("myDeclarationEditComponentController.DeclarationClassificationComponent = true;");
                            });
                        }
                    });
                    //this.preventSelect = false;
                    return;
                }
            }
        }
    };
    CourierWorksheetComponent.prototype.OnBackFromEdit = function (selectedEntityId, $event) {
        this.MyScrollTop = $event.scrollTop;
        this.RefreshButtonClicked();
    };
    CourierWorksheetComponent.prototype.OpenCourierMaster = function (selectedTab) {
        var _this = this;
        var currentScreenCode = "";
        switch (selectedTab) {
            case "General":
                {
                    currentScreenCode = "COGN";
                    break;
                }
            case "ConnectedDeclarations":
                {
                    currentScreenCode = "COCD";
                    break;
                }
        }
        this.CurrentSession.CurrentWindow.SuppressBusyIndicator = true;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                SelectedTabCode: currentScreenCode,
                EntityId: _this.entityPM.Id,
                ObjectTableName: _this.ObjectTableName,
            });
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                _this.CurrentSession.CurrentWindow.SuppressBusyIndicator = false;
                _this._EntityListService.getSingle(_this.entityPM.Id, _this.ObjectTableName).then(function (res) {
                    res.subscribe(function (aa) {
                        _this.entityPM = aa.Result;
                        _this.CheckRequiredFields();
                        _this.RefreshButtonClicked();
                    });
                });
            });
        });
    };
    CourierWorksheetComponent.prototype.DeclarationsStatusRequestMethod = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetSendALLDeclarationsStatusRequest(this.entityPM.Id)
            .subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Show(res.Result);
            //this.RefreshButtonClicked();
        });
    };
    CourierWorksheetComponent.prototype.SendFTPMamanRequestMethod = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorCreating();
        this._CourierMasterService.GetSendFTPMamanRequest(this.entityPM.Id)
            .subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            var myMessageWindow = new MessageWindow_1.MessageWindow();
            myMessageWindow.Show(res.Result);
        });
    };
    CourierWorksheetComponent.prototype.GetMamanPUR = function () {
        var _this = this;
        var myCustomsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGO_CUST_MAMAN", "NON", "NON", SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (response) {
            _this.IsMamanEnabled = false;
            if (!response.HasError && response.Result != null && response.Result.DefaultValue == "Y") {
                _this.IsMamanEnabled = true;
            }
            myCustomsSettingExtendedListService.GetDefault("ISRAEL", "CGO_HWBBMMN", "NON", "NON", SessionLocator_1.SessionLocator.Tenant)
                .subscribe(function (res) {
                if (!res.HasError && res.Result != null && res.Result.DefaultValue == "Y") {
                    _this._CourierWorksheetSharedDataService.IsWebAPICourierGWMessageECTHRDataMamanEnable = true;
                }
            });
        });
    };
    __decorate([
        core_1.ViewChild(DropdownMenuFilterComponent_1.DropdownMenuFilterComponent),
        __metadata("design:type", DropdownMenuFilterComponent_1.DropdownMenuFilterComponent)
    ], CourierWorksheetComponent.prototype, "MyDropdownMenuFilterComponent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CourierWorksheetComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CourierWorksheetComponent.prototype, "onQueryChangeEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CourierWorksheetComponent.prototype, "CustomBackFromEditevent", void 0);
    CourierWorksheetComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CourierWorksheetComponent.html',
            providers: [CourierWorksheetSharedDataService_1.CourierWorksheetSharedDataService],
        }),
        __metadata("design:paramtypes", [CourierWorksheetSharedDataService_1.CourierWorksheetSharedDataService, EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], CourierWorksheetComponent);
    return CourierWorksheetComponent;
}(BaseComponent_1.BaseComponent));
exports.CourierWorksheetComponent = CourierWorksheetComponent;
var KeyValuePair = /** @class */ (function () {
    function KeyValuePair(Key, Value) {
        this.Key = Key;
        this.Value = Value;
    }
    return KeyValuePair;
}());
exports.KeyValuePair = KeyValuePair;
var TabFilter = /** @class */ (function () {
    function TabFilter(Code, Header, Total, Filter) {
        this.Code = Code;
        this.Header = Header;
        this.Total = Total;
        this.Filter = Filter;
    }
    return TabFilter;
}());
exports.TabFilter = TabFilter;
//# sourceMappingURL=CourierWorksheetComponent.js.map