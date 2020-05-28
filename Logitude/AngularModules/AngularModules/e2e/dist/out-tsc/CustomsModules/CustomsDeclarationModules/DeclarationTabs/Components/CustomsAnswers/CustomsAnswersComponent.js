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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeclarationDisplayOnlyChecks_1 = require("../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks");
var DeclarationErrorView_1 = require("../../../../../Customs/EntityPMs/Extended/DeclarationErrorView");
var DeclarationEventManager_1 = require("../../../../../Customs/Utilities/DeclarationEventManager");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var DeclarationPMService_1 = require("../../../../../Customs/Services/StandardPMs/DeclarationPMService");
var ConstraintApprovalRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/ConstraintApprovalRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var DeclarationMessagesService_1 = require("../../../../../Customs/Services/WebServices/DeclarationMessagesService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var CustomsAnswersComponent = /** @class */ (function (_super) {
    __extends(CustomsAnswersComponent, _super);
    function CustomsAnswersComponent(entityArgs, cd, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.cd = cd;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.Declaration";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.ShowStorageStatusMessage = false;
        _this.DisplayOnlyMessage = "";
        _this.IsDescriptionVisible = false;
        _this.Errorslist = new ObservableCollection_1.ObservableCollection([]);
        _this.Warninglist = new ObservableCollection_1.ObservableCollection([]);
        _this.ConstraintsList = [];
        _this.ErrorsCount = "";
        _this.WarningsCount = "";
        _this.ConstraintsCount = "";
        _this.AllCount = "";
        _this.IsCourierDeclaration = false;
        //Services
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.declarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService;
        _this.declarationPMService = new DeclarationPMService_1.DeclarationPMService;
        //#endregion
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#region Filter Methods
        _this.IsErrorsVisible = true;
        _this.IsConstraintsVisible = true;
        _this.IsWarningsVisible = true;
        _this.FilterSelectedValue = 'All';
        //#endregion
        _this.CourierFilterSelectedValue = 'Declaration';
        _this.IsManifest = false;
        _this.IsMoreThan50 = false;
        _this.AllConstraintCount = 0;
        _this.isResourcesLoaded = false;
        _this.errorsLength = 0;
        //#endregion
        //#region constraint paging - client
        _this.pageSize = 50;
        _this.currenctPageIndex = 1;
        _this.currenctStartIndex = 0;
        _this.currenctEndIndex = _this.pageSize;
        _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationConstraint").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                            _this.EntityPM = _this.entityArgs.EntityPM;
                            _this.IsCourierDeclaration = _this.EntityPM.IsCourierDeclaration;
                            _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                            _this.Listen();
                            _this.ReloadDeclarationErrors();
                            console.log("Declaration", _this.EntityPM);
                            _this.DisplayOnlyCheck();
                            //#region Textcodes loading
                            _this.textcode_CollateralRequest = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.CollateralRequest");
                            _this.textcode_RequestedAmount = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.RequestedAmount");
                            _this.textcode_CollateralRequestStatusCode = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.CollateralRequestStatusCode");
                            _this.textcode_PaymentNumber = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PaymentOrder.F.PaymentNumber");
                            _this.textcode_AgentObjection = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.AgentObjection");
                            _this.textcode_DenialReason = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.DenialReason");
                            _this.textcode_ApprovalReason = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsCollateral.O.ApprovalReason");
                            //#enderegion
                        });
                    });
                });
            });
        });
        //Disable fields
        if (_this.IsDisplayOnly) {
            _this.SetScreenFieldsEditability();
        }
        return _this;
    }
    CustomsAnswersComponent.prototype.ngAfterViewInit = function () {
        this.SetFilter();
    };
    CustomsAnswersComponent.prototype.SetFilter = function () {
        var myDeclarationEditComponentController = this.CurrentSession.CurrentEditComponent.EditComponentController;
        if (myDeclarationEditComponentController.CustomsAnswersShowManifest) {
            this.CourierFilterSelectedValue = 'Manifest';
            this.IsManifest = true;
            this.IsConstraintsVisible = false;
            console.log("CustomsAnswersShowManifest");
            myDeclarationEditComponentController.CustomsAnswersShowManifest = false;
        }
        else {
            this.CourierFilterSelectedValue = 'Declaration';
            this.IsManifest = false;
            this.IsConstraintsVisible = true;
        }
    };
    CustomsAnswersComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.SetFilter();
                    _this.ReloadDeclarationErrors();
                    _this.DisplayOnlyCheck();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCCA") {
                        _this.SetFilter();
                        _this.ReloadDeclarationErrors();
                        _this.DisplayOnlyCheck();
                    }
                }
            }));
        }
    };
    CustomsAnswersComponent.prototype.SetScreenFieldsEditability = function () {
        //this.UIProperties.SetEnabled("DeclarationOfficeCode", this.ObjectTableName, !this.IsDisplayOnly);
    };
    Object.defineProperty(CustomsAnswersComponent.prototype, "Description", {
        get: function () { return this.description; },
        set: function (newValue) {
            this.description = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsAnswersComponent.prototype, "ListVersionId", {
        get: function () { return this.listVersionId; },
        set: function (newValue) {
            this.listVersionId = newValue;
            this.RefreshEntity();
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    CustomsAnswersComponent.prototype.RefreshEntity = function () {
        this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    };
    CustomsAnswersComponent.prototype.DisplayOnlyCheck = function () {
        var _this = this;
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            this.SetScreenFieldsEditability();
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(this.IsDisplayOnly);
            return;
        }
        else if (this.EntityPM.StorageStatusCode) {
            this.ShowStorageStatusMessage = true;
            this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + this.EntityPM.StorageStatusName;
        }
        var declarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks_1.DeclarationDisplayOnlyChecks();
        declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe(function (response) {
            var displayOnlyCheckResult = response.Result;
            _this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
            if (_this.IsDisplayOnly) {
                _this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
            }
            else if (_this.EntityPM.StorageStatusCode) {
                _this.ShowStorageStatusMessage = true;
                _this.DisplayOnlyMessage = "בקשת אחסנה הועברה למחסן - סטטוס הבקשה" + " " + _this.EntityPM.StorageStatusName;
            }
            _this.SetScreenFieldsEditability();
            DeclarationEventManager_1.DeclarationEventManager.DisplayModeChanged.emit(_this.IsDisplayOnly);
        });
    };
    CustomsAnswersComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            switch (itemValue) {
                case 'All':
                    {
                        this.IsErrorsVisible = true;
                        if (this.CourierFilterSelectedValue == "Declaration") {
                            this.IsConstraintsVisible = true;
                        }
                        else {
                            this.IsConstraintsVisible = false;
                        }
                        this.IsWarningsVisible = true;
                        break;
                    }
                case 'Errors':
                    {
                        this.IsErrorsVisible = true;
                        this.IsConstraintsVisible = false;
                        this.IsWarningsVisible = false;
                        break;
                    }
                case 'Constraints':
                    {
                        this.IsErrorsVisible = false;
                        if (this.CourierFilterSelectedValue == "Declaration") {
                            this.IsConstraintsVisible = true;
                        }
                        else {
                            this.IsConstraintsVisible = false;
                        }
                        this.IsWarningsVisible = false;
                        break;
                    }
                case 'Warnings':
                    {
                        this.IsErrorsVisible = false;
                        this.IsConstraintsVisible = false;
                        this.IsWarningsVisible = true;
                        break;
                    }
                default:
                    {
                        this.IsErrorsVisible = true;
                        if (this.CourierFilterSelectedValue == "Declaration") {
                            this.IsConstraintsVisible = true;
                        }
                        else {
                            this.IsConstraintsVisible = false;
                        }
                        this.IsWarningsVisible = true;
                        break;
                    }
            }
        }
    };
    CustomsAnswersComponent.prototype.CourierFilterClicked = function (value) {
        if (this.CourierFilterSelectedValue != value) {
            this.CourierFilterSelectedValue = value;
            if (value == 'Manifest') {
                this.IsManifest = true;
                this.IsConstraintsVisible = false;
            }
            else {
                this.IsManifest = false;
                this.IsConstraintsVisible = true;
            }
            this.LoadDeclarationErrors();
        }
    };
    // get errors code
    CustomsAnswersComponent.prototype.ReloadDeclarationErrors = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        //[1] GetDeclarationConstraints();
        this.declarationWebService.GetDeclarationConstraintsByDeclrationId(this.EntityPM.Id)
            .subscribe(function (myServiceResponse) {
            console.log("[Response] GetDeclarationConstraints : ", myServiceResponse.Result);
            var res = myServiceResponse.Result;
            if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                if (res.length == 0)
                    _this.CurrentSession.StopBusyIndicator();
                else if (res.length > 50) {
                    _this.AllConstraintCount = res.length;
                    res = res.slice(0, 50);
                    _this.IsMoreThan50 = true;
                }
                else {
                    _this.IsMoreThan50 = false;
                }
                var decConsts = _this.EntityPM.DeclarationConstraints;
                for (var _i = 0, decConsts_1 = decConsts; _i < decConsts_1.length; _i++) {
                    var constraint = decConsts_1[_i];
                    //this.EntityPM.RemoveDeclarationConstraint(constraint)
                }
                var newConsts = res;
                for (var _a = 0, newConsts_1 = newConsts; _a < newConsts_1.length; _a++) {
                    var constraint = newConsts_1[_a];
                    //this.EntityPM.AddDeclarationConstraint(constraint);
                }
                _this.LoadDeclarationErrors();
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    CustomsAnswersComponent.prototype.LoadConstriantsList = function (decErrors) {
        this.ConstraintsList = [];
        var constraints = this.EntityPM.DeclarationConstraints;
        if (constraints.length > 50) {
            //var startIndex = (this.currenctPageIndex - 1) * 50; // 1.. 0*50=0   2.. 1*50=50
            //var endIndex = this.currenctPageIndex * 50;    // 1.. 1*50=50  2.. 2*50=100
            constraints = constraints.slice(this.currenctStartIndex, this.currenctEndIndex);
            //constraints = constraints.slice(0, 50);
            this.IsMoreThan50 = true;
        }
        else {
            this.IsMoreThan50 = false;
        }
        //var declarationConstraints = [];
        for (var _i = 0, constraints_1 = constraints; _i < constraints_1.length; _i++) {
            var constraint = constraints_1[_i];
            var error = decErrors.find(function (d) { return d.ConstraintId == constraint.ConstraintNumber; });
            var item = new ConstraintLineModel(error, constraint, this);
            this.ConstraintsList.push(item);
        }
        //
        // Calculate count
        if (this.ConstraintsList.length > 0) {
            this.ConstraintsCount = "(" + this.ConstraintsList.length + ")";
        }
        else {
            this.ConstraintsCount = "";
        }
        var declarationErrors = decErrors;
        //build errors and warning grids
        this.BuildDeclarationErrorsWithConstraintsList(declarationErrors);
        //calculate all counts
        var allListsCount = this.ConstraintsList.length + this.Errorslist.Length + this.Warninglist.Length;
        if (allListsCount > 0) {
            if (this.IsMoreThan50)
                this.AllCount = "(" + (this.AllConstraintCount + this.Errorslist.Length + this.Warninglist.Length) + ")";
            else
                this.AllCount = "(" + allListsCount + ")";
        }
        else {
            this.AllCount = "";
        }
    };
    CustomsAnswersComponent.prototype.LoadDeclarationErrors = function () {
        var _this = this;
        //[2] GetDeclarationErrors();
        this.CurrentSession.StartBusyIndicatorLoading(); //Avoiding ReSend !!
        this.declarationWebService.GetDeclarationErrors(this.EntityPM.Id, this.ListVersionId, this.CourierFilterSelectedValue)
            .subscribe(function (myServiceResponse) {
            console.log("[Response] GetDeclarationErrors : ", myServiceResponse.Result);
            var res = myServiceResponse.Result;
            _this.errorsForDeclaration = res;
            if (res && res.length == 0) {
                _this.CurrentSession.StopBusyIndicator();
                _this.LoadConstriantsList([]);
            }
            _this.GetResources(res);
            var newErrors = [];
            var oldErrors = [];
            if (!Tools_1.AppTool.IsNullOrEmpty(res)) {
                //oldErrors = this.EntityPM.DeclarationErrorViews;
                for (var _i = 0, oldErrors_1 = oldErrors; _i < oldErrors_1.length; _i++) {
                    var error = oldErrors_1[_i];
                    //this.EntityPM.RemoveDeclarationErrorView(error);
                }
                newErrors = res;
                for (var _a = 0, newErrors_1 = newErrors; _a < newErrors_1.length; _a++) {
                    var error = newErrors_1[_a];
                    //this.EntityPM.AddDeclarationErrorView(error);
                }
                // this.LoadConstriantsList(newErrors); //temp
                if (Tools_1.AppTool.IsNullOrEmpty(_this.Description)) {
                    _this.IsDescriptionVisible = false;
                }
                else {
                    _this.IsDescriptionVisible = true;
                }
                _this.DisplayOnlyCheck();
            }
            else {
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    CustomsAnswersComponent.prototype.BuildDeclarationErrorsWithConstraintsList = function (declarationErrors) {
        var errorsList = [];
        var warningList = [];
        // reset grids
        this.Errorslist.Clear();
        this.Warninglist.Clear();
        this.Description = null;
        var listversion = null;
        for (var _i = 0, declarationErrors_1 = declarationErrors; _i < declarationErrors_1.length; _i++) {
            var error = declarationErrors_1[_i];
            if (error.Sort == 0 || error.Sort == null) {
                ///???? error.OrderBy = 99999999;
            }
            if (error.ListVersionId == "1") {
                // var item = new ErrorModel(error);
                //this.Errorslist.Insert(error);
                errorsList.push(error);
            }
            else if (error.ListVersionId == "4" || error.ListVersionId == "5") {
                //this.Warninglist.Insert(error);
                warningList.push(error);
            }
            else if (error.ListVersionId == "A") {
                listversion = "A";
                if (!this.Description)
                    this.Description = "";
                this.Description += error.Description + ", ";
                //SystemMessagesVisibility = Visibility.Visible;
            }
            //VeiwModel.ConstraintIndication = error.ConstraintIndication;
        }
        if (this.Description) {
            this.Description = this.Description.replace(/,\s*$/, ""); //remove last comma
        }
        errorsList.sort(function (a, b) { return (a.Sort === b.Sort) ? 0 : (a.Sort < b.Sort) ? -1 : 1; });
        warningList.sort(function (a, b) { return (a.Sort === b.Sort) ? 0 : (a.Sort < b.Sort) ? -1 : 1; });
        this.Errorslist.InsertCollection(errorsList);
        this.Warninglist.InsertCollection(warningList);
        if (listversion == null) {
            this.Description = null;
            //SystemMessagesVisibility = Visibility.Collapsed;
        }
        if (this.Errorslist.Length > 0) {
            this.ErrorsCount = "(" + this.Errorslist.Length + ")";
        }
        else {
            this.ErrorsCount = "";
        }
        if (this.Warninglist.Length > 0) {
            this.WarningsCount = "(" + this.Warninglist.Length + ")";
        }
        else {
            this.WarningsCount = "";
        }
        this.CurrentSession.StopBusyIndicator();
    };
    CustomsAnswersComponent.prototype.GetResources = function (errors) {
        var _this = this;
        var tables = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(errors)) {
            this.errorsLength = errors.length;
            errors.forEach(function (el) {
                if (!Tools_1.AppTool.IsNullOrEmpty(el.FieldNameTextCode)) {
                    var splittedWords = el.FieldNameTextCode.split('.');
                    var objectTableName = splittedWords[0] + "." + splittedWords[1];
                    _this.GetResourceForTableName(objectTableName, errors);
                    //if (tables.indexOf(el.TableNameTextCode) < 0) {
                    //    tables.push(el.TableNameTextCode);
                    //}
                }
                else {
                    _this.errorsLength--;
                    console.log("No FieldNameTextCode", el);
                }
                if (_this.errorsLength == 0)
                    _this.LoadConstriantsList(errors);
            });
        }
    };
    CustomsAnswersComponent.prototype.GetResourceForTableName = function (tableName, errors) {
        var _this = this;
        // var tableName = tables.pop();
        if (tableName == 'Customs.SupplierInvioceItemsCertificate') {
            tableName = 'Customs.SupplierInvioceItemCertificat';
        }
        this.EntityResourceService.getEntityResourceByTableName(tableName).subscribe(function (response) {
            // this.GetResourceForTableName(tables);
            if (_this.errorsLength != 1) {
                _this.errorsLength--;
            }
            else {
                _this.LoadConstriantsList(errors);
            }
        });
    };
    CustomsAnswersComponent.prototype.SendButtonClicked = function (constraint, event) {
        var _this = this;
        this.RequestVIA = event.RequestVIA;
        if (this.EntityPM.IsDirty) {
            this.CurrentSession.CurrentEditComponent.SaveChanges();
            var event = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (event) {
                    event.unsubscribe();
                }
                if (isSaveSuccess) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(constraint.AgentExplanation)) {
                        _this.SendConstraintMethod(constraint);
                    }
                    else {
                        var msg = new MessageWindow_1.MessageWindow();
                        msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.FillAgentExplanation"));
                    }
                }
            });
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(constraint.AgentExplanation)) {
                this.SendConstraintMethod(constraint);
            }
            else {
                var msg = new MessageWindow_1.MessageWindow();
                msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.FillAgentExplanation"));
            }
        }
    };
    CustomsAnswersComponent.prototype.SendConstraintMethod = function (constraint) {
        var _this = this;
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
        var requestParams = new ConstraintApprovalRequestParams_1.ConstraintApprovalRequestParams();
        requestParams.LoggingEnabled = true;
        requestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        requestParams.ConstraintNumber = constraint.ConstraintNumber;
        requestParams.ConstraintTypeName = constraint.ConstraintTypeName;
        requestParams.ConstraintStatusName = constraint.ConstraintStatus;
        requestParams.ApprovalDecision = constraint.ApprovalDecisionName;
        requestParams.AgentExplanation = constraint.AgentExplanation;
        requestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        requestParams.RequestName = "Declaration Constriant";
        requestParams.ResponseName = "Declaration Constriant";
        //requestParams.TestCase = SelectedTest;
        requestParams.DeclarationId = this.EntityPM.Id;
        requestParams.LoggingEntityId = this.EntityPM.Id;
        requestParams.LoggingEntityReference = this.EntityPM.DeclarationNumber;
        requestParams.LoggingObjectTableId = ObjectTable.Id;
        requestParams.RequestVIA = this.RequestVIA;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent.ShowProgressBar(requestParams.PBId, "שליחת בקשה לאישור אילוץ", true).then(function (res) {
            _this.ResponseData = res;
            console.log("Response/ShowProgressBar : ", _this.ResponseData);
            _this.OnSendCompleted();
        }).catch(function (err) {
            _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = err;
        });
        this.declarationMessagesService.PostSendDeclarationConstraint(requestParams).subscribe(function (myServiceResponse) {
        });
    };
    CustomsAnswersComponent.prototype.OnSendCompleted = function () {
        var _this = this;
        var timerTiken = setTimeout(function () {
            _this.RefreshEntity();
        }, 500);
    };
    //#region XML Errors
    CustomsAnswersComponent.prototype.EditEntity = function (declarationError) {
        var _this = this;
        if (Tools_1.AppTool.IsNullOrEmpty(declarationError)) {
            console.warn("[!] There is no declaraion error for the constraint!");
        }
        else {
            this.CurrentSession.StartBusyIndicatorLoading();
            switch (declarationError.EntityName.toLowerCase()) {
                case "declaration":
                case "consignment":
                    {
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 1000;
                        logWindow.Height = 700;
                        logWindow.ShowCloseButton = true;
                        logWindow.Title = this.GetEditedScreenTitle(declarationError.EntityName, declarationError);
                        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/General/DeclarationGeneralComponent');
                        logWindow.WindowArgs = {
                            DeclarationError: declarationError,
                            entityArgs: this.entityArgs,
                            entityPM: this.EntityPM,
                            IsDisplayOnly: this.IsDisplayOnly,
                        };
                        logWindow.WindowClosed.subscribe(function ($event) {
                            if ($event != 'cancel') {
                                _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                            }
                        });
                        this.CurrentSession.StopBusyIndicator();
                        // Remove DetectChanges from answer tab
                        this.cd.detach();
                        logWindow.WindowClosed.subscribe(function () {
                            _this.cd.reattach();
                            var timertoken = setTimeout(function () {
                                _this.cd.detectChanges();
                            }, 100);
                        });
                        break;
                    }
                case "supplierinvoice": {
                    this.declarationWebService
                        .GetSupplierInvoiceBySequenceNumber(declarationError.DeclarationId, +declarationError.LineNumber, 0, 500)
                        .subscribe(function (response) {
                        console.log("[Response] GetSupplierInvoiceBySequenceNumber: ", response);
                        var supplierInvoicePM = response.Result;
                        if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM)) {
                            _this.CurrentSession.StartBusyIndicatorLoading();
                            var windowArgs = {};
                            windowArgs.EntityPM = supplierInvoicePM;
                            windowArgs.declarationPM = _this.EntityPM;
                            windowArgs.DeclarationError = declarationError;
                            windowArgs.IsFromCustomsAnswer = true;
                            windowArgs.IsInvoiceAnswer = true;
                            windowArgs.NumberOfLoadedItems = 500;
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Width = 1030;
                            logWindow.Height = 600;
                            if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + _this.EntityPM.DeclarationNumber;
                            }
                            else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + _this.EntityPM.DeclarationNumber;
                            }
                            else if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;
                            }
                            else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                            }
                            windowArgs.IsDisplayOnly = _this.IsDisplayOnly;
                            logWindow.ShowCloseButton = false;
                            logWindow.WindowArgs = windowArgs;
                            logWindow.Title = _this.GetEditedScreenTitle(declarationError.EntityName, declarationError);
                            logWindow.WindowClosed.subscribe(function ($event) {
                                if ($event != 'cancel') {
                                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                }
                            });
                            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                            _this.CurrentSession.StopBusyIndicator();
                            // Remove DetectChanges from answer tab
                            _this.cd.detach();
                            logWindow.WindowClosed.subscribe(function () {
                                _this.cd.reattach();
                                var timertoken = setTimeout(function () {
                                    _this.cd.detectChanges();
                                }, 100);
                            });
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                            var window = new MessageWindow_1.MessageWindow();
                            window.Show("There is no invoice with such key in this declaration!!");
                        }
                    });
                    break;
                }
                case "supplierinvoiceitem":
                    {
                        if (Tools_1.AppTool.IsNullOrEmpty(declarationError.LineNumber)) {
                            console.log("No line number", declarationError);
                            return;
                        }
                        var lines = declarationError.LineNumber.split(',');
                        var invSequence = +lines[0];
                        var itemSequence = +lines[1];
                        this.declarationWebService
                            .GetSupplierInvoiceWithItemBySequenceNumber(declarationError.DeclarationId, invSequence, itemSequence, 0, 500)
                            .subscribe(function (response) {
                            console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);
                            var supplierInvoicePM = response.Result;
                            if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM)) {
                                _this.CurrentSession.StartBusyIndicatorLoading();
                                var windowArgs = {};
                                windowArgs.EntityPM = supplierInvoicePM;
                                windowArgs.declarationPM = _this.EntityPM;
                                windowArgs.DeclarationError = declarationError;
                                windowArgs.IsFromCustomsAnswer = true;
                                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                logWindow.Width = 1030;
                                logWindow.Height = 600;
                                if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber + "-" + _this.EntityPM.DeclarationNumber;
                                }
                                else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + _this.EntityPM.DeclarationNumber;
                                }
                                else if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice") + " " + supplierInvoicePM.InvoiceNumber;
                                }
                                else if (Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeclarationNumber)) {
                                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                                }
                                _this.CurrentSession.StopBusyIndicator();
                                windowArgs.IsDisplayOnly = _this.IsDisplayOnly;
                                logWindow.ShowCloseButton = false;
                                logWindow.WindowArgs = windowArgs;
                                logWindow.Title = _this.GetEditedScreenTitle(declarationError.EntityName, declarationError);
                                logWindow.WindowClosed.subscribe(function ($event) {
                                    if ($event != 'cancel') {
                                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                    }
                                });
                                logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/AddEditSupplierInvoiceComponent');
                                _this.CurrentSession.StopBusyIndicator();
                                // Remove DetectChanges from answer tab
                                _this.cd.detach();
                                logWindow.WindowClosed.subscribe(function () {
                                    _this.cd.reattach();
                                    var timertoken = setTimeout(function () {
                                        _this.cd.detectChanges();
                                    }, 100);
                                });
                            }
                            else {
                                var window = new MessageWindow_1.MessageWindow();
                                window.Show("There is no invoice with such key in this declaration!!");
                            }
                        });
                        break;
                    }
                case "supplierinvioceitemscertificate":
                    {
                        if (Tools_1.AppTool.IsNullOrEmpty(declarationError.LineNumber)) {
                            console.log("No line number", declarationError);
                            return;
                        }
                        var lines = declarationError.LineNumber.split(',');
                        var invSequence = +lines[0];
                        var itemSequence = +lines[1];
                        this.declarationWebService
                            .GetSupplierInvoiceWithItemBySequenceNumber(declarationError.DeclarationId, invSequence, itemSequence, 0, 500, "certificate")
                            .subscribe(function (response) {
                            console.log("[Response] GetSupplierInvoiceWithItemBySequenceNumber: ", response);
                            var supplierInvoicePM = response.Result;
                            if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM)) {
                                _this.CurrentSession.StartBusyIndicatorLoading();
                                var invoiceItem = supplierInvoicePM.SupplierInvoiceItems.find(function (d) { return d.SequenceNumeric == declarationError.ParentLine; });
                                if (!Tools_1.AppTool.IsNullOrEmpty(invoiceItem)) {
                                    // open certificate
                                    var windowArgs = {};
                                    windowArgs.SupplierInvoiceItemPM = invoiceItem;
                                    windowArgs.DeclarationError = declarationError;
                                    windowArgs.IsDisplayOnly = _this.IsDisplayOnly;
                                    windowArgs.InvoiceNumber = supplierInvoicePM.InvoiceNumber;
                                    windowArgs.SupplierInvoicePM = supplierInvoicePM;
                                    windowArgs.LineNumber = declarationError.LineNumber;
                                    var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Width = 1000;
                                    logWindow.Height = 600;
                                    //if (supplierInvoicePM.ClassificationCode != null) {
                                    //    logWindow.Title = "אישורים לפרט מכס" + " " + supplierInvoicePM.ClassificationCode;
                                    //}
                                    //else {
                                    //    logWindow.Title = "אישורים לפרט מכס";
                                    //}
                                    logWindow.ShowCloseButton = false;
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.Title = _this.GetEditedScreenTitle(declarationError.EntityName, declarationError);
                                    _this.CurrentSession.StopBusyIndicator();
                                    logWindow.WindowClosed.subscribe(function ($event) {
                                        if ($event != 'cancel') {
                                            _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                        }
                                    });
                                    logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/SupplierInvoiceItemCertificatesComponent');
                                    //end open certificate
                                    // Remove DetectChanges from answer tab
                                    _this.cd.detach();
                                    logWindow.WindowClosed.subscribe(function () {
                                        _this.cd.reattach();
                                        var timertoken = setTimeout(function () {
                                            _this.cd.detectChanges();
                                        }, 100);
                                    });
                                }
                                _this.CurrentSession.StopBusyIndicator();
                            }
                            else {
                                var window = new MessageWindow_1.MessageWindow();
                                window.Show("There is no invoice with such key in this declaration!!");
                            }
                        });
                        break;
                    }
                default:
                    {
                        var window = new MessageWindow_1.MessageWindow();
                        window.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                        break;
                    }
            }
        }
    };
    CustomsAnswersComponent.prototype.ShowXMLErrors = function (error) {
        //if (!AppTool.IsNullOrEmpty(error.Field)) {
        //    this.UIProperties.SetValidity(error.Field, "Customs.SupplierInvoice", false, error.Description);
        //}
        var errors = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(error.Description)) {
            var xmlErrors = error.Description.split(/,|:/);
            for (var _i = 0, xmlErrors_1 = xmlErrors; _i < xmlErrors_1.length; _i++) {
                var xmlError = xmlErrors_1[_i];
                errors.push(xmlError);
            }
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }
        if (error.EntityName != null) {
            if (error.EntityName.toLowerCase() == "supplierinvoiceitem") {
                //if (OnShowXMLErrors != null) {
                //    OnShowXMLErrors(new OnShowXMLErrorEvenArgs() { SupplierInvoiceItem = InvoiceItemsObslist.Where(d => d.SequenceNumeric == error.Line).FirstOrDefault(), });
                //}
            }
        }
    };
    CustomsAnswersComponent.prototype.GetEditedScreenTitle = function (entityName, declarationError) {
        var title = "";
        switch (entityName.toLowerCase()) {
            case "declaration":
            case "consignment":
                {
                    title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration");
                    break;
                }
            case "supplierinvoice":
                {
                    var declaration = this.EntityPM;
                    var supplierInvoicePM = declaration.SupplierInvoices.find(function (d) { return d.DeclarationId == declaration.Id && d.SequenceNumeric == declarationError.Line; });
                    if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && !Tools_1.AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
                        title = supplierInvoicePM.InvoiceNumber + "-" + declaration.DeclarationNumber + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                    }
                    else if ((Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && !Tools_1.AppTool.IsNullOrEmpty(declaration.DeclarationNumber)) {
                        title = declaration.DeclarationNumber + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) && (Tools_1.AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
                        title = supplierInvoicePM.InvoiceNumber + " " + TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                    }
                    if ((Tools_1.AppTool.IsNullOrEmpty(supplierInvoicePM.InvoiceNumber) || supplierInvoicePM.InvoiceNumber == "") && (Tools_1.AppTool.IsNullOrEmpty(declaration.DeclarationNumber) || declaration.DeclarationNumber == "")) {
                        title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoice");
                    }
                    break;
                }
            case "supplierinvoiceitem":
                {
                    title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditInvoiceItem");
                    break;
                }
            case "supplierinvioceitemscertificate":
                {
                    title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Certificates");
                    break;
                }
        }
        return title;
    };
    CustomsAnswersComponent.prototype.ConstraintNavigationButtonClicked = function (dir) {
        if (dir == "next") {
            var lastPageNumber = Math.ceil(this.AllConstraintCount / this.pageSize);
            if (lastPageNumber == this.currenctPageIndex) {
                return;
            }
            else {
                this.currenctPageIndex++;
                this.currenctStartIndex = (this.currenctPageIndex - 1) * this.pageSize; // 1.. 0*50=0   2.. 1*50=50
                this.currenctEndIndex = this.currenctPageIndex * this.pageSize; // 1.. 1*50=50  2.. 2*50=100
                this.LoadConstriantsList(this.errorsForDeclaration);
            }
        }
        else if (dir == "prev") {
            if (this.currenctPageIndex <= 1)
                return;
            this.currenctPageIndex--;
            this.currenctStartIndex = (this.currenctPageIndex - 1) * this.pageSize; // 1.. 0*50=0   2.. 1*50=50
            this.currenctEndIndex = this.currenctPageIndex * this.pageSize; // 1.. 1*50=50  2.. 2*50=100
            this.LoadConstriantsList(this.errorsForDeclaration);
        }
    };
    CustomsAnswersComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomsAnswersComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], CustomsAnswersComponent);
    return CustomsAnswersComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsAnswersComponent = CustomsAnswersComponent;
var ConstraintLineModel = /** @class */ (function (_super) {
    __extends(ConstraintLineModel, _super);
    function ConstraintLineModel(declarationError, constraintPM, Parent) {
        var _this = _super.call(this) || this;
        _this.declarationError = declarationError;
        _this.constraintPM = constraintPM;
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.lineHeight = 62;
        _this.hasNoError = false;
        _this.displayOnly = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        // Visibility 
        _this.IsApprovalDecisionHyperlinkVisibile = false;
        _this.IsStatusHyperlinkVisibile = false;
        _this.IsCollateralInfoVisibile = false;
        _this.IsAgentObjectionVisibile = false;
        _this.IsCollateralAnswersVisibile = false;
        _this.IsRequestedDocumentVisibile = false;
        _this.IsPaymentNumberVisibile = false;
        _this.IsStatus1Visibile = false;
        _this.IsStatus2Visibile = false;
        _this.IsStatus3Visibile = false;
        _this.IsStatus4Visibile = false;
        _this.IsStatus5Visibile = false;
        _this.IsStatus4AgentObjectionVisibile = false;
        //*//
        _this.IsAgentObjectionButtonVisibile = true;
        _this.IsApprovalDenaialVisibile = false;
        _this.ApprovalDenaialTitle = "";
        _this.parent = Parent;
        if (Tools_1.AppTool.IsNullOrEmpty(_this.declarationError)) {
            _this.hasNoError = true;
            _this.declarationError = new DeclarationErrorView_1.DeclarationErrorView();
        }
        _this.textcode_TableNameTextCode = TextCodeTranslator_1.TextCodeTranslator.Translate(_this.declarationError.TableNameTextCode);
        _this.UIProperties.SetEnabled("AgentExplanation", "Customs.DeclarationConstraint", !_this.parent.IsDisplayOnly);
        _this.ManageScreensVisibility();
        _this.displayOnly = _this.parent.IsDisplayOnly;
        if (_this.parent.EntityPM.DeclarationStatusTypeCode == '11'
            && _this.constraintPM.ConstraintTypeCode == '1'
            && _this.constraintPM.ConstraintStatusCode == '1') {
            _this.UIProperties.SetEnabled("AgentExplanation", "Customs.DeclarationConstraint", true);
            _this.displayOnly = false;
        }
        return _this;
    }
    Object.defineProperty(ConstraintLineModel.prototype, "ConstraintId", {
        //#region Properties 
        get: function () { return this.declarationError.ConstraintId; },
        set: function (newValue) {
            this.declarationError.ConstraintId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "LineNumber", {
        get: function () { return this.declarationError.LineNumber; },
        set: function (newValue) {
            this.declarationError.LineNumber = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "ErrorMessage", {
        get: function () {
            return Tools_1.AppTool.IsNullOrEmpty(this.declarationError) ? "" : this.declarationError.Description;
        },
        set: function (newValue) {
            this.declarationError.Description = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "FieldNameTextCode", {
        get: function () {
            return this.declarationError.FieldNameTextCode;
        },
        set: function (newValue) {
            this.declarationError.FieldNameTextCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "FieldName", {
        get: function () {
            var field = "";
            if (this.FieldNameTextCode != null) {
                field = TextCodeTranslator_1.TextCodeTranslator.Translate(this.FieldNameTextCode);
            }
            return field;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "TableNameTextCode", {
        get: function () {
            return this.declarationError.TableNameTextCode;
        },
        set: function (newValue) {
            this.declarationError.TableNameTextCode = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "CustomsCollateralId", {
        get: function () { return this.constraintPM.CustomsCollateralId; },
        set: function (newValue) { this.constraintPM.CustomsCollateralId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "ConstraintNumber", {
        get: function () { return this.constraintPM.ConstraintNumber; },
        set: function (newValue) { this.constraintPM.ConstraintNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "ConstraintTypeName", {
        get: function () { return this.constraintPM.ConstraintTypeName; },
        set: function (newValue) { this.constraintPM.ConstraintTypeName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "ConstraintStatus", {
        get: function () { return this.constraintPM.ConstraintStatusName; },
        set: function (value) { this.constraintPM.ConstraintStatusName = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "ConstraintStatusCode", {
        get: function () { return this.constraintPM.ConstraintStatusCode; },
        set: function (newValue) { this.constraintPM.ConstraintStatusCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "AgentExplanation", {
        get: function () { return this.constraintPM.AgentExplanation; },
        set: function (newValue) {
            this.constraintPM.AgentExplanation = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "ApprovalDecisionName", {
        get: function () {
            if (this.constraintPM != null) {
                return this.constraintPM.ApprovalDecisionName;
            }
            else
                return null;
        },
        set: function (newValue) {
            this.constraintPM.ApprovalDecisionName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "ApprovalUserName", {
        get: function () {
            if (this.constraintPM != null) {
                return this.constraintPM.ApprovalUserName;
            }
            else
                return null;
        },
        set: function (newValue) {
            this.constraintPM.ApprovalUserName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "ApprovalNote", {
        get: function () {
            if (this.constraintPM != null) {
                return this.constraintPM.ApprovalNote;
            }
            else
                return null;
        },
        set: function (newValue) {
            this.constraintPM.ApprovalNote = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "AgentObjection", {
        get: function () {
            if (this.constraintPM != null) {
                return this.constraintPM.AgentObjection;
            }
            else
                return null;
        },
        set: function (newValue) {
            this.constraintPM.AgentObjection = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "CollateralRequestNumber", {
        get: function () { return this.collateralRequestNumber; },
        set: function (newValue) { this.collateralRequestNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "PaymentNumber", {
        get: function () { return this.paymentNumber; },
        set: function (newValue) { this.paymentNumber = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "PaymentOrderId", {
        get: function () { return this.paymentOrderId; },
        set: function (newValue) { this.paymentOrderId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "RequestedAmount", {
        get: function () { return this.requestedAmount; },
        set: function (newValue) { this.requestedAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ConstraintLineModel.prototype, "CollateralRequestStatus", {
        get: function () { return this.collateralRequestStatus; },
        set: function (newValue) { this.collateralRequestStatus = newValue; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    ConstraintLineModel.prototype.ManageScreensVisibility = function () {
        this.IsApprovalDecisionHyperlinkVisibile = false;
        this.IsStatusHyperlinkVisibile = true;
        switch (this.ConstraintStatusCode) {
            case "1":
                {
                    this.lineHeight = 80;
                    this.IsStatus1Visibile = true;
                    this.IsStatus2Visibile = false;
                    this.IsStatus3Visibile = false;
                    this.IsStatus4Visibile = false;
                    this.IsStatus5Visibile = false;
                    break;
                }
            case "2":
                {
                    this.IsStatus1Visibile = false;
                    this.IsStatus2Visibile = true;
                    this.IsStatus3Visibile = false;
                    this.IsStatus4Visibile = false;
                    this.IsStatus5Visibile = false;
                    break;
                }
            case "3":
                {
                    this.lineHeight = 115;
                    this.IsStatus1Visibile = false;
                    this.IsStatus2Visibile = false;
                    this.IsStatus3Visibile = true;
                    this.IsStatus4Visibile = false;
                    this.IsStatus5Visibile = false;
                    this.CheckDetailsInfoVisibilities();
                    break;
                }
            case "4":
                {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.constraintPM.AgentObjection)) {
                        this.IsStatus4AgentObjectionVisibile = false;
                    }
                    this.IsStatus1Visibile = false;
                    this.IsStatus2Visibile = false;
                    this.IsStatus3Visibile = false;
                    this.IsStatus4Visibile = true;
                    this.IsStatus5Visibile = false;
                    break;
                }
            case "5":
                {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.constraintPM.CustomsCollateralId)) {
                        this.IsStatus1Visibile = false;
                        this.IsStatus2Visibile = false;
                        this.IsStatus3Visibile = false;
                        this.IsStatus4Visibile = false;
                        this.IsStatus5Visibile = true;
                    }
                    else {
                        this.IsStatus1Visibile = false;
                        this.IsStatus2Visibile = false;
                        this.IsStatus3Visibile = true;
                        this.IsStatus4Visibile = false;
                        this.IsStatus5Visibile = false;
                        this.CheckDetailsInfoVisibilities();
                    }
                    if (!Tools_1.AppTool.IsNullOrEmpty(this.constraintPM.ApprovalDecision)) {
                        this.IsApprovalDecisionHyperlinkVisibile = true;
                        this.IsStatusHyperlinkVisibile = false;
                    }
                    else {
                        this.IsApprovalDecisionHyperlinkVisibile = false;
                        this.IsStatusHyperlinkVisibile = true;
                    }
                    break;
                }
            default:
                {
                    this.IsStatus1Visibile = false;
                    this.IsStatus2Visibile = false;
                    this.IsStatus3Visibile = false;
                    this.IsStatus4Visibile = false;
                    this.IsStatus5Visibile = false;
                    break;
                }
        }
    };
    ConstraintLineModel.prototype.CheckDetailsInfoVisibilities = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.constraintPM.CustomsCollateralId)) {
            this.IsCollateralInfoVisibile = true;
        }
        else {
            this.IsCollateralInfoVisibile = false;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.constraintPM.AgentObjection)) {
            this.IsAgentObjectionVisibile = true;
        }
        else {
            this.IsAgentObjectionVisibile = false;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.constraintPM.CustomsCollateralId)) {
            this.declarationWebService.GetSingleCustomsCollateral(this.constraintPM.CustomsCollateralId).subscribe(function (response) {
                var result = response.Result;
                console.log("[Response/GetSingleCustomsCollateral]: ", result);
                if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                    var collateral = result;
                    _this.Collateral = collateral;
                    // calculate request amount
                    var sum = 0;
                    collateral.CustomsCollateralsConditions.forEach(function (el) {
                        sum += el.RequestedAmount;
                    });
                    _this.RequestedAmount = sum + "";
                    console.log("RequestedAmount calculated:", _this.RequestedAmount);
                    _this.CollateralRequestNumber = collateral.CollateralRequestNumber;
                    _this.CollateralRequestStatus = collateral.CollateralRequestStatusName;
                    if (collateral.CustomsCollateralsAnswers.Count != 0) {
                        _this.IsCollateralAnswersVisibile = true;
                        if (collateral.PaymentNumber != null) {
                            _this.PaymentOrderId = collateral.PaymentOrderId;
                            _this.PaymentNumber = collateral.PaymentNumber;
                            _this.IsPaymentNumberVisibile = true;
                        }
                        else {
                            _this.IsPaymentNumberVisibile = false;
                        }
                    }
                    else {
                        _this.IsCollateralAnswersVisibile = false;
                    }
                }
                else {
                }
                if (_this.IsCollateralAnswersVisibile || _this.IsAgentObjectionVisibile) {
                    _this.lineHeight = 115;
                }
            });
        }
        this.declarationWebService.CheckIfDocumentPointerExistsForConstraint(this.constraintPM.ConstraintNumber).subscribe(function (response) {
            var result = response.Result;
            console.log("[Response/CheckIfDocumentPointerExistsForConstraint]: ", result);
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                var exist = result;
                if (exist) {
                    _this.IsAgentObjectionVisibile = true;
                }
                else {
                    _this.IsRequestedDocumentVisibile = false;
                }
            }
        });
    };
    ConstraintLineModel.prototype.ViewConstraintDetails = function () {
        var _this = this;
        var constraint = this.constraintPM;
        if (constraint.ApprovalDecision == "2") {
            this.IsAgentObjectionButtonVisibile = false;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(constraint.CustomsCollateralId)) {
            if (this.ConstraintStatusCode == "1" || this.ConstraintStatusCode == "2") {
                this.IsApprovalDenaialVisibile = false;
            }
            if (this.ConstraintStatusCode == "4") {
                this.ApprovalDenaialTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationConstraint.O.DenaialReason");
            }
            else if (this.ConstraintStatusCode == "5") {
                this.ApprovalDenaialTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationConstraint.O.ApprovalReason");
            }
            else {
                this.ApprovalDenaialTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.DeclarationConstraint.O.ApprovalReason");
            }
            var window = new LogitudeWindow_1.LogitudeWindow();
            window.Width = 600;
            window.Height = 500;
            window.Title = "פרטי אילוץ";
            window.WindowArgs = {
                DeclarationError: this.declarationError,
                ConstraintPM: this.constraintPM,
                ApprovalDenaialTitle: this.ApprovalDenaialTitle,
                IsAgentObjectionButtonVisibile: this.IsAgentObjectionButtonVisibile,
            };
            window.Show("./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CustomsAnswers/ConstraintsDetailsComponent");
            // Remove DetectChanges from answer tab
            this.parent.cd.detach();
            window.WindowClosed.subscribe(function () {
                _this.parent.cd.reattach();
                var timertoken = setTimeout(function () {
                    _this.parent.cd.detectChanges();
                }, 100);
            });
        }
        else {
            var window = new LogitudeWindow_1.LogitudeWindow();
            window.Width = 610;
            window.Height = 660;
            window.ShowCloseButton = true;
            window.WindowArgs = {
                DeclarationError: this.declarationError,
                ConstraintPM: this.constraintPM,
                ApprovalDenaialTitle: this.ApprovalDenaialTitle,
                IsAgentObjectionButtonVisibile: this.IsAgentObjectionButtonVisibile,
            };
            window.Title = "";
            window.Show("./CustomsModules/CustomsDeclarationModules/DeclarationTabs/Components/CustomsAnswers/ConstraintDetailWithCollateralComponent");
            // Remove DetectChanges from answer tab
            this.parent.cd.detach();
            window.WindowClosed.subscribe(function () {
                _this.parent.cd.reattach();
                var timertoken = setTimeout(function () {
                    _this.parent.cd.detectChanges();
                }, 100);
            });
        }
    };
    ConstraintLineModel.prototype.PaymentNumberLinkClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.PaymentOrderId)) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.PaymentOrderId, ObjectTableName: 'Customs.PaymentOrder' });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                });
            });
        }
    };
    ConstraintLineModel.prototype.OpenCollateralWindow = function () {
        var _this = this;
        if (this.Collateral) {
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
            logitudeWindow.WindowArgs = { CurrentEntity: this.Collateral };
            logitudeWindow.Height = 730;
            logitudeWindow.Width = 660;
            logitudeWindow.Show('./CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent');
            // Remove DetectChanges from answer tab
            this.parent.cd.detach();
            logitudeWindow.WindowClosed.subscribe(function () {
                _this.parent.cd.reattach();
                _this.parent.cd.detectChanges();
            });
        }
        else {
            console.log("[Error] No colleteral to open!!!");
        }
    };
    return ConstraintLineModel;
}(BaseComponent_1.BaseComponent));
exports.ConstraintLineModel = ConstraintLineModel;
//# sourceMappingURL=CustomsAnswersComponent.js.map