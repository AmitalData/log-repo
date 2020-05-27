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
var DecCargoSplitConsItemPM_1 = require("../../../../Customs/EntityPMs/DecCargoSplitConsItemPM");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObservableCollection_1 = require("../../../../Infrastructure/Utilities/ObservableCollection");
var DecCargoSplitConComponent = /** @class */ (function (_super) {
    __extends(DecCargoSplitConComponent, _super);
    function DecCargoSplitConComponent(entityArgs, CD, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.CD = CD;
        _this.EntityResourceService = EntityResourceService;
        _this.ObjectTableName = "Customs.DecCargoSplitCon";
        _this.DataContext = _this;
        _this.IsDisplayOnly = false;
        _this.DisplayOnlyMessage = "";
        _this.IsImporerCodeEnabled = true;
        _this.IsClosed = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.isImporterClicked = false;
        _this.SelectedRow = null;
        _this.conItemPackDetStatus = false;
        _this.conItemPackDetStatusVisibility = false;
        _this.ItemsList = new ObservableCollection_1.ObservableCollection([]);
        _this.SetDisplayFields();
        return _this;
        //EntityResourceService.getEntityResourceByTableName("Customs.TreatmentWay", 0).subscribe((res: any) => {
        //var entityListService: TreatmentWayListService = new TreatmentWayListService();
        //entityListService.getAllFromCache().subscribe((res: any) => {
        //this.Listen();
        //});
        //});
    }
    DecCargoSplitConComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            //this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
            //    this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
            //        if (this.currentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
            //            if (tabCode == "DEGC") {
            //                this.DisplayOnlyCheck();
            //            }
            //        }
            //    })
            //);
        }
    };
    DecCargoSplitConComponent.prototype.SetTabArgs = function (args) {
        var _this = this;
        this.EntityPM = args.EntityPM;
        this.declarationCargoSplitPM = args.Parent;
        this.IsClosed = this.declarationCargoSplitPM.IsClosed;
        this.IsDisplayOnly = args.Disabled;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DecCargoSplitConsItems)) {
            for (var _i = 0, _a = this.EntityPM.DecCargoSplitConsItems; _i < _a.length; _i++) {
                var conItem = _a[_i];
                var item = new DecCargoSplitConsItemModel(conItem);
                this.ItemsList.Insert(item);
            }
        }
        this.CurrentSession.SubscriptionAdd(this.CurrentSession.CollateralAnswerRefreshEvent.subscribe(function (res) {
            _this.SetClosedDeclarationCargoSplitScreesn(res.IsClosed);
        }));
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM)) {
            this.PreceduralFilterItems = new ApiQueryFilters_1.ApiQueryFilters();
            this.PreceduralFilterItems.addAdditionalFilter("IsImport", true, null, null, "Equals", false, false, false, "boolean");
        }
        this.firstTime = true;
        this.SetClosedDeclarationCargoSplitScreesn(this.declarationCargoSplitPM.IsClosed);
        if (this.declarationCargoSplitPM.IsClosed) {
            this.IsClosed = true;
        }
        else {
            this.DisplayOnlyMessageVisibility = false;
        }
        this.SetDisplayFields();
        /*
        if (this.EntityPM.CustomsTapgFile != null) {
            this.RequestNumberLabelVisibility = true;
            this.TapagFileLabelVisibility = true;
            if (!AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && !AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagFile + "/" + this.EntityPM.RequestedTapagNumeral;
            }

            else if (!AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagFile;
            }

            else if (AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagFile) && !AppTool.IsNullOrEmpty(this.EntityPM.RequestedTapagNumeral)) {
                this.RequestNumber = this.EntityPM.RequestedTapagNumeral;
            }


            if (!AppTool.IsNullOrEmpty(this.CustomsTapgFile) && !AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsTapgFile + "/" +this.CustomsNumeral;
            }

            else if (!AppTool.IsNullOrEmpty(this.CustomsTapgFile) && AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsTapgFile;
            }

            else if (AppTool.IsNullOrEmpty(this.CustomsTapgFile) && !AppTool.IsNullOrEmpty(this.CustomsNumeral)) {
                this.TapagFile = this.CustomsNumeral;
            }
        }
        


        if (this.EntityPM.PaymentOrderId != null) {
            this.PaymentOrderVsibility = true;
            this.PaymentOrder = this.EntityPM.PaymentOrderNumber;
            this.PaymentOrderStatus = this.EntityPM.PaymentOrderStatus;

           
        }
        */
    };
    DecCargoSplitConComponent.prototype.SetDisplayFields = function () {
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessageVisibility = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ConditionCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, false);
            this.DisplayOnlyMessageVisibility = true;
        }
        else {
            this.DisplayOnlyMessageVisibility = false;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ConditionCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, true);
            this.DisplayOnlyMessageVisibility = false;
        }
        this.IsImporerCodeEnabled = !this.IsDisplayOnly;
    };
    DecCargoSplitConComponent.prototype.SetClosedDeclarationCargoSplitScreesn = function (IsClosed) {
        if (IsClosed) {
            this.IsClosed = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ConditionCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ProcedureCurrentCode", this.ObjectTableName, false);
            //if (this.EntityPM.NewFileRequest) {
            //    this.IsNewFile = true;
            //}
            // else {
            //if (this.EntityPM.AnswerEntityTypeCode != null && this.EntityPM.AllocatedAmount != null) {
            //    this.IsTapag = true;
            //}
            //}
        }
        else {
            this.IsClosed = false;
            /*
            if (!this.EntityPM.NewFileRequest) {

                if (this.EntityPM.AnswerEntityTypeCode != null && this.EntityPM.AllocatedAmount != null) {
                    this.IsTapag = true;
                }

                else if (this.EntityPM.AnswerEntityTypeCode == null && this.EntityPM.AllocatedAmount == null) {
                    return;

                }


                this.IsTapag = true;
                if (this.EntityPM.AnswerEntityTypeCode == null) {
                    this.AnswerEntityRedIconVisibility = true;
                }
                else {
                    this.AnswerEntityRedIconVisibility = false;
                }

                if (AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile)) {
                    this.CustomsTapgFileRedIconVisibility = true;
                }
                else {
                    this.CustomsTapgFileRedIconVisibility = false;
                }

                if (this.EntityPM.AllocatedAmount == null) {
                    this.AllocatedAmountRedIconVisibility = true;
                }
                else {
                    this.AllocatedAmountRedIconVisibility = false;
                }
            }

            if (this.EntityPM.NewFileRequest) {
                this.IsNewFile = true;

                if (this.EntityPM.RequestFileTypeCode != null) {
                    this.RequestFileCodeRedIconVisibility = false;
                }
                else {
                    this.RequestFileCodeRedIconVisibility = true;
                }

                if (this.EntityPM.RequestFileAmount != null) {
                    this.RequestFileRedIconVisibility = false;
                }
                else {
                    this.RequestFileRedIconVisibility = true;
                }

            }
        }
            */
        }
    };
    DecCargoSplitConComponent.prototype.DisplayOnlyCheck = function () {
        this.IsDisplayOnly = this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayMode;
        if (this.IsDisplayOnly) {
            this.DisplayOnlyMessage = "לתצוגה בלבד - " + this.CurrentSession.CurrentEditComponent.EditComponentController.InDisplayModeMessage;
            return;
        }
        this.IsImporerCodeEnabled = !this.IsDisplayOnly;
        /*
         var declarationDisplayOnlyChecks: DeclarationDisplayOnlyChecks = new DeclarationDisplayOnlyChecks();
         declarationDisplayOnlyChecks.DeclarationViewDisplayOnlyChecks(this.EntityPM).subscribe((response: any) => {
             var displayOnlyCheckResult: DisplayOnlyCheckResult = response.Result;
             this.IsDisplayOnly = displayOnlyCheckResult.IsDisplayOnly;
             if (this.IsDisplayOnly) {
                 this.DisplayOnlyMessage = "לתצוגה בלבד - " + displayOnlyCheckResult.DisplayOnlyMessage;
             }
             
         });
         */
    };
    DecCargoSplitConComponent.prototype.checkImportersVisibility = function () {
        if (this.IsDisplayOnly)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ImporterCode))
            if (this.ImporterCode.includes("F") || this.ImporterCode.includes("P")) {
                this.IsImporerCodeEnabled = false;
                this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            }
    };
    DecCargoSplitConComponent.prototype.ImporterClicked = function (client) {
        if (client) {
            this.isImporterClicked = true;
            this.UIProperties.SetEnabled("ImporterCode", this.ObjectTableName, true);
            this.ImporterCode = client.Code;
        }
    };
    DecCargoSplitConComponent.prototype.ImporterLostFocus = function (item, importerSearchBox) {
        this.isImporterClicked = false;
        this.ImporterCode = item;
    };
    DecCargoSplitConComponent.prototype.AddItem = function () {
        if (!this.IsDisplayOnly) {
            var counter = 0;
            if (this.EntityPM.DecCargoSplitConsItems.length > 0) {
                var items = this.EntityPM.DecCargoSplitConsItems.sort(function (a, b) { return (a.ItemLine === b.ItemLine) ? 0 : (a.ItemLine < b.ItemLine) ? -1 : 1; });
                if (items.length == 0)
                    counter = 0;
                else {
                    counter = items[this.EntityPM.DecCargoSplitConsItems.length - 1].ItemLine;
                }
            }
            counter += 1;
            var item = new DecCargoSplitConsItemPM_1.DecCargoSplitConsItemPM(this.EntityPM);
            item.DeclarationCargoSplitId = this.EntityPM.DeclarationCargoSplitId;
            item.Tenant = this.EntityPM.Tenant;
            item.DecCargoSplitConsLineNo = this.EntityPM.LineNumber;
            item.ItemLine = counter;
            if (!this.EntityPM.DecCargoSplitConsItems.includes(item)) {
                this.EntityPM.AddDecCargoSplitConsItem(item);
                var line = new DecCargoSplitConsItemModel(item);
                this.ItemsList.Insert(line);
            }
        }
    };
    DecCargoSplitConComponent.prototype.OnRowEnded = function ($event) {
        console.log("this.ItemsList.Length : " + this.ItemsList.Length);
        if (($event) == this.ItemsList.Length) {
            this.AddItem();
        }
    };
    DecCargoSplitConComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
        if (this.ItemsList.Length == 0) {
            this.AddItem();
        }
    };
    DecCargoSplitConComponent.prototype.OnFocus = function () {
        if (this.ItemsList.Length == 0) {
            this.AddItem();
        }
    };
    DecCargoSplitConComponent.prototype.DeleteButtonClicked = function (item) {
        var _this = this;
        //this.DeleteSelected(item);
        //return;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Width = 300;
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.DeletePackage"));
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Confirm");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.DeleteSelected(item);
            }
            else if (confirmWindow.No) {
            }
        });
        //this.customsDocumentPointerService.GetCheckForPointers(item.DeclarationId, item.InvoiceCounterKey).subscribe((myResponse: ServiceResponse) => {
        //    var exist = myResponse;
        //   if (myResponse.Result) {
        //       var confirmWindow1 = new ConfirmWindow();
        //       //     confirmWindow.DisplayWariningIconImage();
        //       confirmWindow1.Width = 400;
        //       confirmWindow1.Title = TextCodeTranslator.Translate("Customs.General.O.Warning");
        //       confirmWindow1.Height = 190;
        //       confirmWindow1.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        //       confirmWindow1.NoButtonText = TextCodeTranslator.Translate("General.B.Cancel");
        //       confirmWindow1.ShowWarningImage = true;
        //       confirmWindow1.ShowNoButton
        //       confirmWindow1.Show(TextCodeTranslator.Translate("Customs.General.O.InvoiceRelatedPoiner"));
        //       confirmWindow1.WindowClosed.subscribe((event: any) => {
        //           if (confirmWindow1.Yes) {
        //               this.DeleteSelected(item);
        //           } else if (confirmWindow1.No) {
        //           }
        ////       });
        ////   }
        //   else {
        //this.DeleteSelected(item);
        //}
        //}
        //      });
        //    } else if (confirmWindow.No) {
        //    }
        //});
    };
    DecCargoSplitConComponent.prototype.DeleteSelected = function (item) {
        //this.CurrentSession.StartBusyIndicator("");
        this.lastDeletedItem = item.EntityPM;
        //var SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
        //SaveCompletedEvent.unsubscribe();
        this.ItemsList.Remove(item);
        this.EntityPM.RemoveDecCargoSplitConsItem(item.EntityPM);
        /*
        this.supplierInvoiceExtendedPMService.delete(item.DeclarationId, item.InvoiceCounterKey).subscribe((myResponse: ServiceResponse) => {

            if (!myResponse.HasError) {
                this.ItemsSource.Remove(item);
                
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.CurrentSession.StopBusyIndicator();

            }


        });
        */
        //});
        //this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    DecCargoSplitConComponent.prototype.EditButtonClicked = function (item) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(item.EntityPM) && !Tools_1.AppTool.IsNullOrEmpty(item.EntityPM.ItemLine)) {
            var windowArgs = {};
            windowArgs.DecCargoSplitConsItemPM = item.EntityPM;
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            windowArgs.DeclarationCargoSplitPM = this.declarationCargoSplitPM;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ConsignmentPackages");
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 750;
            logWindow.Height = 350;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.ConsignmentPackages");
            if (item.EntityPM.ParentCargoConsinmentItem != null) {
                logWindow.Title = logWindow.Title + " סידורי " + item.ParentCargoConsinmentItem;
            }
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(function ($event) {
                if ($event == "ok") {
                    //this.CD.reattach();
                    _this.RefreshEntity();
                }
                _this.SetStatusVisibility();
            });
            logWindow.Show('./CustomsModules/CustomsDeclarationCargoSplit/Components/EditTabs/DecCargoSplitConsPackDetComponent');
        }
        /*
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, "Customs.Declaration", errors);

        for (let item of this.EntityPM.Consignments) {
            for (let line of item.ConsignmentPackages) {
                if (line.MarksNumbers == null && line.PackageMeasureQualifierCode == null && line.PackageQuantity == null && line.PackageTypeCode == null && line.GrossMassMeasure == null) {
                    var errorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.EmptyConsignmentPackage");
                    if (!AppTool.IsNullOrEmpty(errorMessage)) {
                        errors.push(errorMessage);

                    }
                }

            }
        }


        if (errors.length > 0) {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        }


        else {
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            if (this.EntityPM.IsDirty) {
                this.CurrentSession.StartBusyIndicator("");
                this.declarationPMService.update(this.EntityPM).subscribe((response: ServiceResponse) => {
                    var declaration = response.Result;
                    this.CurrentSession.StopBusyIndicator();
                    if (!AppTool.IsNullOrEmpty(declaration)) {
                        if (!AppTool.IsNullOrEmpty(item)) {
                            this.EditItem(item);

                        }
                    }

                });
            }
            else {
                this.EditItem(item);
            }

        }
        */
    };
    DecCargoSplitConComponent.prototype.RefreshEntity = function () {
        if (this.CurrentSession.CurrentEditComponent) {
            this.CurrentSession.CurrentEditComponent.EditComponentController.ResetMustRefresh();
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
        }
    };
    DecCargoSplitConComponent.prototype.SetStatusVisibility = function () {
        if (this.SelectedRow.EntityPM.ChangeSetOp = "Update") {
            this.conItemPackDetStatusVisibility = true;
        }
        else {
            this.conItemPackDetStatusVisibility = false;
        }
    };
    Object.defineProperty(DecCargoSplitConComponent.prototype, "ConItemPackDetStatus", {
        get: function () { return this.conItemPackDetStatus; },
        set: function (value) { this.conItemPackDetStatus = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConComponent.prototype, "ConItemPackDetStatusVisibility", {
        get: function () { return this.conItemPackDetStatusVisibility; },
        set: function (value) { this.conItemPackDetStatusVisibility = value; },
        enumerable: true,
        configurable: true
    });
    DecCargoSplitConComponent.prototype.EditItem = function (item) {
        this.CurrentSession.StartBusyIndicator("");
        /*
        var supplierInvoiceExtendedPMService: SupplierInvoiceExtendedPMService = new SupplierInvoiceExtendedPMService();

        this.supplierInvoiceExtendedPMService.GetSingleSupplierInvoicePMWithLimitedItems(this.EntityPM.Id, item.InvoiceCounterKey, 0, this.NumberOfLoadedItems, "parent").subscribe(response => {

            var windowArgs: any = {};
            windowArgs.EntityPM = response.Result;
            windowArgs.declarationPM = this.EntityPM;
            windowArgs.NumberOfLoadedItems = this.NumberOfLoadedItems;
            var windowTitle = "Supplier Invoice";

            var logWindow = new LogitudeWindow();
            logWindow.Width = 995; // don't change this width!
            logWindow.Height = 600;

            if (!AppTool.IsNullOrEmpty(item.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditItem") + " " + item.InvoiceNumber + "-" + this.EntityPM.DeclarationNumber;

            }
            else if (AppTool.IsNullOrEmpty(item.InvoiceNumber) && !AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditItem") + " " + this.EntityPM.DeclarationNumber;

            }
            else if (!AppTool.IsNullOrEmpty(item.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditItem") + " " + item.InvoiceNumber;

            }
            else if (AppTool.IsNullOrEmpty(item.InvoiceNumber) && AppTool.IsNullOrEmpty(this.EntityPM.DeclarationNumber)) {
                windowArgs.WindowTitle = TextCodeTranslator.Translate("Customs.Declaration.O.EditItem");

            }
            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            this.CD.detach();
            logWindow.WindowClosed.subscribe((event: any) => {
                if (event != 'cancel') {

                    this.RefreshEntity();
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                }
                else {
                    this.ReloadMyScreen();
                }
                this.CD.reattach();

            });
            logWindow.IsHideHeader = true;
            logWindow.Show('./Customs/Components/Declaration/EditTabs/SupplierInvoices/AddEditSupplierInvoiceComponent');
            this.CurrentSession.StopBusyIndicator();
        });
        */
    };
    Object.defineProperty(DecCargoSplitConComponent.prototype, "ImporterCode", {
        get: function () { return this.EntityPM ? this.EntityPM.ImporterCode : null; },
        set: function (newValue) {
            this.EntityPM.ImporterCode = newValue;
            //this.UIProperties.SetRequired("Code", "Customs.Client", AppTool.IsNullOrEmpty(newValue));
            this.UIProperties.SetRequired("ImporterCode", "Customs.DecCargoSplitCon", Tools_1.AppTool.IsNullOrEmpty(newValue));
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConComponent.prototype, "ProcedureCurrentCode", {
        get: function () { return this.EntityPM ? this.EntityPM.ProcedureCurrentCode : null; },
        set: function (newValue) {
            this.EntityPM.ProcedureCurrentCode = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                //this.RequestFileRedIconVisibility = true;
            }
            else {
                //this.RequestFileRedIconVisibility = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConComponent.prototype, "ConditionCode", {
        get: function () { return this.EntityPM ? this.EntityPM.ConditionCode : null; },
        set: function (newValue) {
            this.EntityPM.ConditionCode = newValue;
            if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
                //this.RequestFileRedIconVisibility = true;
            }
            else {
                //this.RequestFileRedIconVisibility = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConComponent.prototype, "DisplayOnlyMessageVisibility", {
        get: function () { return this.displayOnlyMessageVisibility; },
        set: function (newValue) { this.displayOnlyMessageVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    /*
    
    private isNewFile: boolean;
    public get IsNewFile() { return this.isNewFile; }
    public set IsNewFile(newValue: boolean)
    {
        this.isNewFile = newValue;
        if (newValue && !this.DeclarationCargoSplitPM.IsClosed) {

            this.IsTapag = false;
            this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, true);

            this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, false);
            this.EntityPM.NewFileRequest = true;

            if (this.EntityPM.RequestFileTypeCode != null) {
                this.RequestFileCodeRedIconVisibility = false;
            }
            else {
                this.RequestFileCodeRedIconVisibility = true;
            }

            if (this.EntityPM.RequestFileAmount != null) {
                this.RequestFileRedIconVisibility = false;
            }
            else {
                this.RequestFileRedIconVisibility = true;
            }

          
        //    this.firstTime = false;

            if ((this.EntityPM.CollateralsRequestFileConds == null || this.EntityPM.CollateralsRequestFileConds.length == 0) && (this.collateralPM != null && this.collateralPM.DeclarationCargoSplitsConditions != null))
            {
                for (var item of this.collateralPM.DeclarationCargoSplitsConditions)
                {
                    var collateralsRequestFileCond: CollateralsRequestFileCondPM = new CollateralsRequestFileCondPM(this.collateralPM);
                    collateralsRequestFileCond.DeclarationCargoSplitId = this.EntityPM.DeclarationCargoSplitId;
                    collateralsRequestFileCond.LineNumber = this.EntityPM.LineNumber;
                    collateralsRequestFileCond.Tenant = this.EntityPM.Tenant;
                    collateralsRequestFileCond.ConditionCode = item.ConditionCode;
                    collateralsRequestFileCond.ConditionName = item.ConditionName;
                    collateralsRequestFileCond.RequestedAmount = item.RequestedAmount;
                    this.EntityPM.AddCollateralsRequestFileCond(collateralsRequestFileCond);
                }
            }

        }
    }
   

   private isTapag: boolean;
     public get IsTapag() { return this.isTapag; }
     public set IsTapag(newValue: boolean) {
         this.isTapag = newValue;

         if (newValue && !this.collateralPM.IsClosed) {

             this.IsNewFile = false;
             this.UIProperties.SetEnabled("RequestFileTypeCode", this.ObjectTableName, false);
             this.UIProperties.SetEnabled("RequestFileAmount", this.ObjectTableName, false);

             this.UIProperties.SetEnabled("AnswerEntityTypeCode", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("AllocatedAmount", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("CustomsTapgFile", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("Remarks", this.ObjectTableName, true);
             this.UIProperties.SetEnabled("CustomsNumeral", this.ObjectTableName, true);
             this.EntityPM.NewFileRequest = false;

             if (this.EntityPM.AnswerEntityTypeCode == null) {
                 this.AnswerEntityRedIconVisibility = true;
             }
             else {
                 this.AnswerEntityRedIconVisibility = false;
             }

             if (AppTool.IsNullOrEmpty(this.EntityPM.CustomsTapgFile)) {
                 this.CustomsTapgFileRedIconVisibility = true;
             }
             else {
                 this.CustomsTapgFileRedIconVisibility = false;
             }

             if (this.EntityPM.AllocatedAmount == null) {
                 this.AllocatedAmountRedIconVisibility = true;
             }
             else {
                 this.AllocatedAmountRedIconVisibility = false;
             }
            
           //  this.firstTime = false;

         }
     }
 

     

    private tapagFile: string;
    public get TapagFile() { return this.tapagFile; }
    public set TapagFile(newValue: string) { this.tapagFile = newValue; }

    private requestNumber: string;
    public get RequestNumber() { return this.requestNumber; }
    public set RequestNumber(newValue: string) { this.requestNumber = newValue; }

    public get AnswerEntityTypeCode() { return this.EntityPM ? this.EntityPM.AnswerEntityTypeCode : null; }
    public set AnswerEntityTypeCode(newValue: string) {
        this.EntityPM.AnswerEntityTypeCode = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.AnswerEntityRedIconVisibility = true;
        }
        else {
            this.AnswerEntityRedIconVisibility = false;
        }
    }

    public get AnswerForCollateralStatusName() { return this.EntityPM ? this.EntityPM.AnswerForCollateralStatusName : null; }

    public get AllocatedAmount() { return this.EntityPM ? this.EntityPM.AllocatedAmount : null; }
    public set AllocatedAmount(newValue: number) {
        this.EntityPM.AllocatedAmount = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.AllocatedAmountRedIconVisibility = true;
        }
        else {
            this.AllocatedAmountRedIconVisibility = false;
        }
    }


    public get CustomsTapgFile() { return this.EntityPM ? this.EntityPM.CustomsTapgFile : null; }
    public set CustomsTapgFile(newValue: string) {
        this.EntityPM.CustomsTapgFile = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.CustomsTapgFileRedIconVisibility = true;
        }
        else {
            this.CustomsTapgFileRedIconVisibility = false;
        }
    }

    public get CustomsNumeral() { return this.EntityPM ? this.EntityPM.CustomsNumeral : null; }
    public set CustomsNumeral(newValue: string) { this.EntityPM.CustomsNumeral = newValue; }

    public get Remarks() { return this.EntityPM ? this.EntityPM.Remarks : null; }
    public set Remarks(newValue: string) { this.EntityPM.Remarks = newValue; }

    public get RequestFileTypeName() { return this.EntityPM ? this.EntityPM.RequestFileTypeName : null; }
    public set RequestFileTypeName(newValue: string) { this.EntityPM.RequestFileTypeName = newValue; }

    public get RequestFileTypeCode() { return this.EntityPM ? this.EntityPM.RequestFileTypeCode : null; }
    public set RequestFileTypeCode(newValue: string) {
        this.EntityPM.RequestFileTypeCode = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.RequestFileCodeRedIconVisibility = true;
        }
        else {
            this.RequestFileCodeRedIconVisibility = false;
        }
    }


    public get RequestFileAmount() { return this.EntityPM ? this.EntityPM.RequestFileAmount : null; }
    public set RequestFileAmount(newValue: number) {
        this.EntityPM.RequestFileAmount = newValue;
        if (AppTool.IsNullOrEmpty(newValue)) {
            this.RequestFileRedIconVisibility = true;
        }
        else {
            this.RequestFileRedIconVisibility = false;
        }
    }

    public get NewFileRequest() { return this.EntityPM ? this.EntityPM.NewFileRequest : null; }
    public set NewFileRequest(newValue: boolean) {
        this.EntityPM.NewFileRequest = newValue;

    }

    private customsTapgFileRedIconVisibility: boolean;
    public get CustomsTapgFileRedIconVisibility() { return this.customsTapgFileRedIconVisibility; }
    public set CustomsTapgFileRedIconVisibility(newValue: boolean) { this.customsTapgFileRedIconVisibility = newValue; }

    private allocatedAmountRedIconVisibility: boolean;
    public get AllocatedAmountRedIconVisibility() { return this.allocatedAmountRedIconVisibility; }
    public set AllocatedAmountRedIconVisibility(newValue: boolean) { this.allocatedAmountRedIconVisibility = newValue; }

    private answerEntityRedIconVisibility: boolean;
    public get AnswerEntityRedIconVisibility() { return this.answerEntityRedIconVisibility; }
    public set AnswerEntityRedIconVisibility(newValue: boolean) { this.answerEntityRedIconVisibility = newValue; }

    private requestFileRedIconVisibility: boolean;
    public get RequestFileRedIconVisibility() { return this.requestFileRedIconVisibility; }
    public set RequestFileRedIconVisibility(newValue: boolean) { this.requestFileRedIconVisibility = newValue; }

    private requestFileCodeRedIconVisibility: boolean;
    public get RequestFileCodeRedIconVisibility() { return this.requestFileCodeRedIconVisibility; }
    public set RequestFileCodeRedIconVisibility(newValue: boolean) { this.requestFileCodeRedIconVisibility = newValue; }

    private paymentOrderVsibility: boolean;
    public get PaymentOrderVsibility() { return this.paymentOrderVsibility; }
    public set PaymentOrderVsibility(newValue: boolean) { this.paymentOrderVsibility = newValue; }

    private tapagFileLabelVisibility: boolean;
    public get TapagFileLabelVisibility() { return this.tapagFileLabelVisibility; }
    public set TapagFileLabelVisibility(newValue: boolean) { this.tapagFileLabelVisibility = newValue; }

    private requestNumberLabelVisibility: boolean;
    public get RequestNumberLabelVisibility() { return this.requestNumberLabelVisibility; }
    public set RequestNumberLabelVisibility(newValue: boolean) { this.requestNumberLabelVisibility = newValue; }


    private answerSentTextVisibility: boolean;
    public get AnswerSentTextVisibility() { return this.answerSentTextVisibility; }
    public set AnswerSentTextVisibility(newValue: boolean) { this.answerSentTextVisibility = newValue; }

    currentScreenCode: string;
    OpenPaymentOrder() {
        this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(response => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(response => {
                this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe(response => {
                    this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderProtestReason").subscribe(response => {
                        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsSetting").subscribe(response => {

                            this.EditEntity("Customs.PaymentOrder", this.EntityPM.PaymentOrderId, null, "POGN");
                        });
                    });
                });
            });
        });


                        }


    */
    DecCargoSplitConComponent.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
        });
    };
    DecCargoSplitConComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DecCargoSplitConComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], DecCargoSplitConComponent);
    return DecCargoSplitConComponent;
}(BaseComponent_1.BaseComponent));
exports.DecCargoSplitConComponent = DecCargoSplitConComponent;
var DecCargoSplitConsItemModel = /** @class */ (function (_super) {
    __extends(DecCargoSplitConsItemModel, _super);
    function DecCargoSplitConsItemModel(line) {
        var _this = _super.call(this) || this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.valid = true;
        _this.EntityPM = line;
        return _this;
    }
    Object.defineProperty(DecCargoSplitConsItemModel.prototype, "CargoDescription", {
        //#region Properties
        get: function () { return this.EntityPM.CargoDescription; },
        set: function (newValue) { this.EntityPM.CargoDescription = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsItemModel.prototype, "RequestReasonName", {
        get: function () { return this.EntityPM.RequestReasonName; },
        set: function (newValue) { this.EntityPM.RequestReasonName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsItemModel.prototype, "RequestReasonCode", {
        get: function () { return this.EntityPM.RequestReasonCode; },
        set: function (newValue) { this.EntityPM.RequestReasonCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsItemModel.prototype, "ParentCargoConsinmentItem", {
        get: function () { return this.EntityPM.ParentCargoConsinmentItem; },
        set: function (newValue) { this.EntityPM.ParentCargoConsinmentItem = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DecCargoSplitConsItemModel.prototype, "GrossMassMeasure", {
        get: function () { return this.EntityPM.GrossMassMeasure; },
        set: function (newValue) { this.EntityPM.GrossMassMeasure = newValue; },
        enumerable: true,
        configurable: true
    });
    //#endregion
    DecCargoSplitConsItemModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    DecCargoSplitConsItemModel.prototype.ParentCargoConsinmentItemKeyUp = function (event, logCellTemplate, parentCargoConsinmentItemTextBox) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnParentCargoConsinmentItemLostFocus(logCellTemplate, parentCargoConsinmentItemTextBox);
        }
    };
    DecCargoSplitConsItemModel.prototype.OnParentCargoConsinmentItemLostFocus = function (logCellTemplate, parentCargoConsinmentItemTextBox) {
        var newValue = this.ParentCargoConsinmentItem;
        this.valid = true;
        this.UIProperties.SetValidity("ParentCargoConsinmentItem", "Customs.DecCargoSplitConsItem", true, "");
        if (Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            this.UIProperties.SetValidity("ParentCargoConsinmentItem", "Customs.DecCargoSplitConsItem", true, "");
        }
        else if (newValue.toString().length > 3) {
            this.valid = false;
            this.UIProperties.SetValidity("ParentCargoConsinmentItem", "Customs.DecCargoSplitConsItem", false, "סידורי במטען אב לא יכול להיות ארוך משלושה תווים");
        }
        else {
            this.valid = true;
            this.UIProperties.SetValidity("ParentCargoConsinmentItem", "Customs.DecCargoSplitConsItem", true, "");
        }
        if (this.valid != true) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: parentCargoConsinmentItemTextBox.InputId });
        }
        //this.ParentCargoConsinmentItem = newValue;
        //parentCargoConsinmentItemTextBox.TextValue = newValue;
    };
    DecCargoSplitConsItemModel.prototype.GrossMassMeasureKeyUp = function (event, logCellTemplate, grossMassMeasureTextBox) {
        var key = event.keyCode;
        if (key == 13) {
            this.OnGrossMassMeasureLostFocus(logCellTemplate, grossMassMeasureTextBox);
        }
    };
    DecCargoSplitConsItemModel.prototype.OnGrossMassMeasureLostFocus = function (logCellTemplate, grossMassMeasureTextBox) {
        var newValue = this.GrossMassMeasure;
        this.valid = true;
        this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", true, "");
        if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            var strValue = newValue.toString();
            if (strValue.indexOf(".") > -1)
                strValue = newValue.toString().substring(0, newValue.toString().indexOf("."));
            if (strValue.length > 11) {
                this.valid = false;
                this.UIProperties.SetValidity("GrossMassMeasure", "Customs.DecCargoSplitConsPackDet", false, "משקל לא יכול להיות ארוך מאחד עשר תווים");
            }
        }
        if (this.valid != true) {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = true;
            this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: grossMassMeasureTextBox.InputId });
        }
    };
    return DecCargoSplitConsItemModel;
}(BaseComponent_1.BaseComponent));
exports.DecCargoSplitConsItemModel = DecCargoSplitConsItemModel;
//# sourceMappingURL=DecCargoSplitConComponent.js.map