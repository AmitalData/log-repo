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
var BaseComponent_1 = require("../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SupplierInvoiceItemPM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemPM");
var ObservableCollection_1 = require("../../../../../../Infrastructure/Utilities/ObservableCollection");
var TextCodeTranslator_1 = require("../../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../../../Infrastructure/Tools");
var ConfirmWindow_1 = require("../../../../../../Controls/Windows/ConfirmWindow");
var SupplierInvoiceItemVehiclePM_1 = require("../../../../../../Customs/EntityPMs/SupplierInvoiceItemVehiclePM");
var SessionLocator_1 = require("../../../../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../../../../Infrastructure/Services/EntityResourceService");
var Validator_1 = require("../../../../../../Infrastructure/Validators/Validator");
var VehicleExtendedPMService_1 = require("../../../../../../Customs/Services/ExtendedPMs/VehicleExtendedPMService");
var LogitudeWindow_1 = require("../../../../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../../../../../Infrastructure/Utilities/FeatureLocator");
var VehiclePMService_1 = require("../../../../../../Customs/Services/StandardPMs/VehiclePMService");
var CustomsSettingListService_1 = require("../../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var AmitalGatewayUtil_1 = require("../../../../../../Infrastructure/Utilities/AmitalGatewayUtil");
var CardListService_1 = require("../../../../../../Common/Services/StandardLists/CardListService");
var CustomsSettingExtendedListService_1 = require("../../../../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService");
var SupplierInvoiceItemVehicleComponent = /** @class */ (function (_super) {
    __extends(SupplierInvoiceItemVehicleComponent, _super);
    function SupplierInvoiceItemVehicleComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemVehicle";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsReleaseFileDisplay = false;
        _this.vehiclePMService = new VehiclePMService_1.VehiclePMService();
        _this.vehicleExtendedPMService = new VehicleExtendedPMService_1.VehicleExtendedPMService();
        _this.cardListService = new CardListService_1.CardListService();
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService;
        _this._CustomsSettingExtendedListService = new CustomsSettingExtendedListService_1.CustomsSettingExtendedListService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.VehiclesFilesButtonVisibility = false;
        _this.vehExisitInScreen = false;
        // OK
        // OK 
        _this.vehicles = [];
        _this.enableOkButton = true;
        _this.selectedVehicles = [];
        _this._MyResponseObject = null;
        _this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        _this.FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        console.log("....|| SupplierInvoiceItemVehicleComponent ||....");
        return _this;
    }
    SupplierInvoiceItemVehicleComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.entityResourceService.getEntityResourceByTableName("Customs.SupplierInvoiceItemVehicle").subscribe(function (response) {
                _this.entityResourceService.getEntityResourceByTableName("Customs.Vehicle").subscribe(function (response) {
                    _this.IsVisibile = true;
                    _this.invoiceItemPM = args.SupplierInvoiceItemPM;
                    _this.BuildVehicleList();
                    _this.IsDisplayOnly = args.IsDisplayOnly;
                    _this.OriginalItemPM = args.SupplierInvoiceItemPM;
                    _this.parent = args.parent;
                    _this.ClonedItemPM = _this.CloneEntity(args.SupplierInvoiceItemPM);
                    var table = window.ObjectTables.filter(function (d) { return d.Name === 'Customs.Vehicle'; })[0];
                    var feature = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return (f.FeatureTypeCode == "MODL") && f.ObjectTableId == table.Id; })[0];
                    if (feature) {
                        _this.SearchButtonVisibility = true;
                    }
                    _this.declarationPM = args.declarationPM;
                    if (_this.declarationPM.IsReleaseFile) {
                        _this._CustomsSettingExtendedListService.GetDefault("ISRAEL", "CGG_VEHICLE_BLK", "NON", "NON", SessionLocator_1.SessionLocator.Tenant)
                            .subscribe(function (response) {
                            var obj = response.Result;
                            if (obj) {
                                var DefaultValue = obj['DefaultValue'];
                                if (DefaultValue == "Y") {
                                    _this.IsReleaseFileDisplay = true;
                                }
                            }
                        });
                    }
                    _this.supplierInvoicePM = args.SupplierInvoicePM;
                    var featureVehicle = FeatureLocator_1.FeatureLocator.Features.filter(function (f) { return f.Code == "LOADVEHICLESFROMUNI"; })[0];
                    if (featureVehicle) {
                        _this.customsSettingListService.getAll().subscribe(function (response) {
                            var list = response.Result;
                            if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                                var customsSetting = list.filter(function (d) { return d.Tenant == SessionLocator_1.SessionLocator.Tenant; })[0];
                                if (!Tools_1.AppTool.IsNullOrEmpty(customsSetting)) {
                                    if (customsSetting.IsConnectedToUniFreight) {
                                        _this.VehiclesFilesButtonVisibility = true;
                                        ////let myDec = this.CurrentSession.CurrentEditComponent.EntityPM;
                                        //let myDec = args.declarationPM;
                                        //if (AmitalGatewayUtil.Instance.IsDeclarationInUse(myDec.CustomFileNo, myDec.IsConvertedDeclaration, myDec.IsConnectedToUnifreight)) {
                                        //    this.VehiclesFilesButtonVisibility = true;
                                        //}
                                    }
                                }
                            }
                        });
                    }
                });
            });
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.Add = function (supplierInvoiceItemVehiclePM) {
        if (!this.IsDisplayOnly && !this.IsReleaseFileDisplay) {
            //if (!this.ValidateList())
            //    return;
            ////last item valid
            //var lastitem = this.ItemsSource.Collection[this.ItemsSource.Length - 1];
            //if (lastitem) if (lastitem.valid) return;
            var counter = 0;
            //Get LineNumber
            if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {
                var items = this.invoiceItemPM.SupplierInvoiceItemVehicles.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
                if (items.length == 0)
                    counter = 0;
                else {
                    counter = items[this.invoiceItemPM.SupplierInvoiceItemVehicles.length - 1].LineNumber;
                }
            }
            counter += 1;
            //Get SequenceNumeric
            var sequence = 0;
            if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {
                var items = this.invoiceItemPM.SupplierInvoiceItemVehicles.sort(function (a, b) { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1; });
                if (items.length == 0)
                    sequence = 0;
                else {
                    sequence = items[this.invoiceItemPM.SupplierInvoiceItemVehicles.length - 1].SequenceNumeric;
                }
            }
            sequence += 1;
            var item = new SupplierInvoiceItemVehiclePM_1.SupplierInvoiceItemVehiclePM(this.invoiceItemPM);
            item.DeclarationId = this.invoiceItemPM.DeclarationId;
            item.Tenant = this.invoiceItemPM.Tenant;
            item.InvoiceCounterKey = this.invoiceItemPM.CounterKey;
            item.InvoiceItemLineNumber = this.invoiceItemPM.LineNumber;
            item.LineNumber = counter;
            item.SequenceNumeric = sequence;
            item.ChangeSetOp = "insert";
            if (supplierInvoiceItemVehiclePM != null) {
                item.RichbitFileNumber = supplierInvoiceItemVehiclePM.RichbitFileNumber;
                item.VehicleChassisNumber = supplierInvoiceItemVehiclePM.VehicleChassisNumber;
                item.RichbitFileStatus = supplierInvoiceItemVehiclePM.RichbitFileStatus;
            }
            if (!this.invoiceItemPM.SupplierInvoiceItemVehicles.includes(item)) {
                this.invoiceItemPM.AddSupplierInvoiceItemVehicle(item);
                this.ItemsSource.Insert(new InvoiceItemVehicleLine(item, this, this.invoiceItemPM));
            }
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.BuildVehicleList = function () {
        this.ItemsSource.Clear();
        for (var _i = 0, _a = this.invoiceItemPM.SupplierInvoiceItemVehicles; _i < _a.length; _i++) {
            var item = _a[_i];
            this.ItemsSource.Insert(new InvoiceItemVehicleLine(item, this, this.invoiceItemPM));
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.CancelButtonClicked = function () {
        var _this = this;
        if (this.invoiceItemPM.IsDirty && !this.IsDisplayOnly) {
            var confirm = new ConfirmWindow_1.ConfirmWindow();
            confirm.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Yes");
            confirm.ShowNoButton = true;
            confirm.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Cancel"));
            confirm.WindowClosed.subscribe(function (event) {
                if (confirm.Yes) {
                    confirm.Close();
                    _this.OkButtonClicked();
                }
                else {
                    _this.RejectChanges();
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
        else {
            this.CurrentSession.CloseCurrentWindow();
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (this.ItemsSource.Length > 0) {
            // Build richbit numbers list - string
            var richbitNumbersList = "";
            this.ItemsSource.Collection.forEach(function (veh) {
                var entity = veh.entityPM;
                var oldEntity = veh.entityPM.OldEntityPM;
                if (entity.IsDirty) {
                    if (entity.ChangeSetOp == "insert") {
                        if (veh.RichbitFileNumber)
                            richbitNumbersList += veh.RichbitFileNumber + ",";
                    }
                    else if (oldEntity)
                        if (entity.RichbitFileNumber != oldEntity.RichbitFileNumber && entity.RichbitFileNumber != oldEntity.richbitFileNumber)
                            if (veh.RichbitFileNumber)
                                richbitNumbersList += veh.RichbitFileNumber + ",";
                }
            });
            // validate it in server
            if (richbitNumbersList) {
                this.vehicleExtendedPMService.GetCheckRichbitNumbersError(richbitNumbersList).subscribe(function (response) {
                    if (response) {
                        var vehicleValidationError = [];
                        vehicleValidationError = response.Result;
                        if (vehicleValidationError && vehicleValidationError.length > 0) {
                            console.log("[Server Validation] ==> ", vehicleValidationError);
                            _this.ValidationErrorsList = [];
                            var VehiclesErrorsList = [];
                            vehicleValidationError.forEach(function (error) {
                                // validate (Delete-Add) problem
                                // if same declaration and same invoice, validate same invoice problem
                                if (error.DeclarationId == _this.declarationPM.Id && error.InvoiceCounterKey == _this.supplierInvoicePM.InvoiceCounterKey) {
                                    //check if invalid itemveh is deleted from the same invoice or not
                                    var deleted = true;
                                    for (var j = 0; j < _this.parent.EntityPM.SupplierInvoiceItems.length; j++) {
                                        var item = _this.parent.EntityPM.SupplierInvoiceItems[j];
                                        var sameItem = item.SupplierInvoiceItemVehicles.find(function (veh) { return veh.RichbitFileNumber == error.RichbitFileNumber; });
                                        if (sameItem) {
                                            if (sameItem.ChangeSetOp != "insert") {
                                                //check updated?
                                                if (sameItem.RichbitFileNumber == (sameItem.OldEntityPM.RichbitFileNumber ? sameItem.OldEntityPM.RichbitFileNumber : sameItem.RichbitFileNumber)) //richbit not changed
                                                    deleted = false;
                                            }
                                        }
                                    }
                                    //show result
                                    if (deleted) {
                                        //show message you must save item after adding it again
                                        _this.ValidationErrorsList.push("שלדה זו קיימת בחשבון ספק. חובה לשמור את חשבון הספק קודם"); //"This vehicle was used in this invoice, you must save currenct invoice before adding it again.";
                                    }
                                    else {
                                        //ext: 
                                        //used in same invoice 
                                        if (error.InvoiceItemLineNumber != _this.invoiceItemPM.LineNumber) {
                                            //var msg = (error.IsVehicle ? "The vehicle (" : "The richbit (") + error.RichbitFileNumber + ") is used in another item in this invoice";
                                            var msg = (error.IsVehicle ? "שילדה (" : "ריכיבת (") + error.RichbitFileNumber + ") קיימת בפרט אחר בחשבון ספק"; // + " - חשבון " + error.InvoiceCounterKey;
                                        }
                                        else {
                                            //var msg = (error.IsVehicle ? "Vehicle (" : "Richbit no.(") + error.RichbitFileNumber + ") is used in this invoice item!";
                                            var msg = "רכב זה (" + error.RichbitFileNumber + ") כבר הוזן בשורות קודמות";
                                        }
                                        //txe
                                        _this.ValidationErrorsList.push(msg);
                                    }
                                }
                                else if (error.DeclarationId == _this.declarationPM.Id && error.InvoiceCounterKey != _this.supplierInvoicePM.InvoiceCounterKey) {
                                    //the validation error in same declaration, but different invoice
                                    //ext:
                                    var msg = (error.IsVehicle ? "שילדה (" : "ריכיבת (") + error.RichbitFileNumber + ") קיימת בחשבון ספק אחר בהצהרה"; // + " - חשבון " + error.InvoiceCounterKey;;
                                    _this.ValidationErrorsList.push(msg);
                                    //txe 
                                }
                                else {
                                    //richbit/vehicle found in another declaration
                                    if (error.IsVehicle) // richbit can be duplicated in another declaration
                                        VehiclesErrorsList.push(error.ValidationText);
                                }
                            });
                            if (_this.ValidationErrorsList.length == 0) {
                                if (VehiclesErrorsList.length > 0) {
                                    //show message box
                                    //var confirmWindow = new ConfirmWindow();
                                    //confirmWindow.Width = 400;
                                    //var confirmMsg = "";
                                    //VehiclesErrorsList.forEach(error => {confirmMsg += error + "\n";});
                                    //confirmWindow.Show(confirmMsg);
                                    var windowArgs = {};
                                    windowArgs.Errors = VehiclesErrorsList;
                                    windowArgs.CancelButtonVisibility = true;
                                    windowArgs.SaveButtonText = "עדכן";
                                    windowArgs.CancelButtonText = "בטל";
                                    windowArgs.ComponentHeight = '328px';
                                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                                    logWindow.Width = 600;
                                    logWindow.Height = 400;
                                    //logWindow.Title = windowTitle;
                                    logWindow.ShowCloseButton = false;
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.WindowClosed.subscribe(function ($event) {
                                        switch ($event) {
                                            case "ok": {
                                                //ok
                                                if (_this.ValidateList())
                                                    _this.SubmitChanges();
                                                break;
                                            }
                                            case "cancel": {
                                                break;
                                            }
                                        }
                                    });
                                    logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
                                    _this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
                                }
                                else {
                                    //ok
                                    if (_this.ValidateList())
                                        _this.SubmitChanges();
                                }
                            }
                        }
                        else {
                            //ok
                            if (_this.ValidateList())
                                _this.SubmitChanges();
                        }
                    }
                });
            }
            else {
                // if (this.ValidateList())
                this.SubmitChanges();
            }
        }
        else {
            // no input
            //if (this.ValidateList())
            this.SubmitChanges();
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.SubmitChanges = function () {
        //if (!this.ValidateList())
        //    return;
        //if (this.EnableOkButton) {
        this.ValidationErrorsList = [];
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        for (var _i = 0, _a = this.invoiceItemPM.SupplierInvoiceItemVehicles; _i < _a.length; _i++) {
            var item = _a[_i];
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            if (Tools_1.AppTool.IsNullOrEmpty(item.RichbitFileNumber) && Tools_1.AppTool.IsNullOrEmpty(item.VehicleChassisNumber)) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.EmptyVehicle"));
            }
        }
        if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {
            this.invoiceItemPM.VehicleStatus = true;
            // trigger.VehicleStatusVisibility = Visibility.Visible;
        }
        else {
            this.invoiceItemPM.VehicleStatus = true;
            //  trigger.VehicleStatusVisibility = Visibility.Collapsed;
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            for (var _b = 0, _c = this.invoiceItemPM.SupplierInvoiceItemVehicles; _b < _c.length; _b++) {
                var item = _c[_b];
                if (!Tools_1.AppTool.IsNullOrEmpty(item.VehicleId) || !Tools_1.AppTool.IsNullOrEmpty(item.RichbitFileNumber)) {
                    item.VehicleTypeCode = "ZZZ";
                }
                else {
                    item.VehicleTypeCode = "CN";
                }
            }
            if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {
                this.invoiceItemPM.VehicleStatus = true;
            }
            else {
                this.invoiceItemPM.VehicleStatus = false;
            }
            this.ResequenceLines();
            ////add new vehs to temprory list in general tab
            //if (this.invoiceItemPM.SupplierInvoiceItemVehicles) {
            //    this.invoiceItemPM.SupplierInvoiceItemVehicles.forEach((veh) => {
            //        var exist = this.parent.addedVehicles.find(d => d == veh);
            //        if (!exist)
            //            this.parent.addedVehicles.push(veh);
            //    });
            //}
            this.CurrentSession.CloseCurrentWindow();
        }
        //} else {
        //    this.ValidationErrorsList = [];
        //    this.ValidationErrorsList.push("Please check items");
        //}
    };
    SupplierInvoiceItemVehicleComponent.prototype.ResequenceLines = function () {
        var index = 0;
        var list = this.invoiceItemPM.SupplierInvoiceItemVehicles;
        if (list.length > 0) {
            //list.sort(a => a.SequenceNumeric).forEach((item) =>
            list.sort(function (a, b) { return (a.SequenceNumeric > b.SequenceNumeric) ? 1 : ((b.SequenceNumeric > a.SequenceNumeric) ? -1 : 0); })
                .forEach(function (item) {
                index++;
                if (item.SequenceNumeric != index)
                    item.SequenceNumeric = index;
            });
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.RejectChanges = function () {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    };
    SupplierInvoiceItemVehicleComponent.prototype.MapEntitytoEntity = function (srcEntity, targetEntity, takeKeysFromTarget) {
        if (takeKeysFromTarget === void 0) { takeKeysFromTarget = false; }
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.CloneEntity = function (entityToClone) {
        var _this = this;
        var clonedEntity;
        clonedEntity = new SupplierInvoiceItemPM_1.SupplierInvoiceItemPM(entityToClone.EntityParentPM);
        this.MapEntitytoEntity(entityToClone, clonedEntity);
        clonedEntity.SupplierInvoiceItemVehicles = [];
        entityToClone.SupplierInvoiceItemVehicles.forEach(function (itemMod) {
            var clonedItemMod = new SupplierInvoiceItemVehiclePM_1.SupplierInvoiceItemVehiclePM(itemMod.EntityParentPM);
            _this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.SupplierInvoiceItemVehicles.push(clonedItemMod);
        });
        return clonedEntity;
    };
    SupplierInvoiceItemVehicleComponent.prototype.OnRowEnded = function ($event) {
        if (($event) == this.ItemsSource.Length) {
            this.Add(null);
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.OnFocus = function () {
        if (this.ItemsSource.Length == 0) {
            this.Add(null);
        }
    };
    Object.defineProperty(SupplierInvoiceItemVehicleComponent.prototype, "EnableOkButton", {
        get: function () { return this.enableOkButton; },
        set: function (value) { this.enableOkButton = value; },
        enumerable: true,
        configurable: true
    });
    SupplierInvoiceItemVehicleComponent.prototype.SearchButtonClicked = function () {
        var windowArgs = {};
        windowArgs.Parent = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 900;
        logWindow.Height = 600;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        //  logWindow.WindowClosed.subscribe(($event: any) => this.SelectVehicleCompleted($event));
        logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/VehiclesSearchComponent');
    };
    SupplierInvoiceItemVehicleComponent.prototype.SelectVehicleCompleted = function (selectedVehicles) {
        if (selectedVehicles != null) {
            for (var _i = 0, selectedVehicles_1 = selectedVehicles; _i < selectedVehicles_1.length; _i++) {
                var vehicle = selectedVehicles_1[_i];
                var lineNumber = 0;
                var sequence = 0;
                if (this.invoiceItemPM.SupplierInvoiceItemVehicles.length > 0) {
                    var items = this.invoiceItemPM.SupplierInvoiceItemVehicles.sort(function (a, b) { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1; });
                    if (items.length == 0)
                        lineNumber = 0;
                    else {
                        lineNumber = items[this.invoiceItemPM.SupplierInvoiceItemVehicles.length - 1].LineNumber;
                    }
                    var items = this.invoiceItemPM.SupplierInvoiceItemVehicles.sort(function (a, b) { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1; });
                    if (items.length == 0)
                        sequence = 0;
                    else {
                        sequence = items[this.invoiceItemPM.SupplierInvoiceItemVehicles.length - 1].SequenceNumeric;
                    }
                }
                lineNumber += 1;
                sequence += 1;
                var item = new SupplierInvoiceItemVehiclePM_1.SupplierInvoiceItemVehiclePM(this.invoiceItemPM);
                item.ChangeSetOp = "insert";
                item.DeclarationId = this.invoiceItemPM.DeclarationId;
                item.Tenant = this.invoiceItemPM.Tenant;
                item.InvoiceCounterKey = this.invoiceItemPM.CounterKey;
                item.InvoiceItemLineNumber = this.invoiceItemPM.LineNumber;
                item.LineNumber = lineNumber;
                //item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                item.VehicleChassisNumber = vehicle.VehicleChassisNumber;
                item.RichbitFileNumber = vehicle.RichbitFileNumber;
                item.RichbitFileStatus = vehicle.StatusName;
                item.SequenceNumeric = sequence;
                item.VehicleId = vehicle.Id;
                var itemvehicle = this.invoiceItemPM.SupplierInvoiceItemVehicles.find(function (d) { return d.VehicleChassisNumber == item.VehicleChassisNumber && d.RichbitFileNumber == item.RichbitFileNumber; });
                if (!itemvehicle) {
                    this.invoiceItemPM.AddSupplierInvoiceItemVehicle(item);
                    this.ItemsSource.Insert(new InvoiceItemVehicleLine(item, this, this.invoiceItemPM));
                }
                // this code moved to server.
                //this.vehiclePMService.get(vehicle.Id).subscribe(response => {
                //    if (response) {
                //        if (!response.HasError) {
                //            var vehicle: VehiclePM = response.Result;
                //            if (vehicle) {
                //                vehicle.DeclarationId = this.invoiceItemPM.DeclarationId;
                //            }
                //           this.vehiclePMService.update(vehicle).subscribe(response => {
                //           });
                //        }
                //    }
                //});
            }
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.VehiclesFilesButtonClicked = function () {
        var _this = this;
        var sub = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.UnifaceRequestArrived
            .subscribe(function (myUnifreightMessageM) {
            if (myUnifreightMessageM.LogitudeViewModel == "SupplierInvoiceItemVehicleComponent" &&
                (myUnifreightMessageM.LogitudeEntity == "Customs.Declaration" || myUnifreightMessageM.LogitudeEntity == "Declaration") &&
                myUnifreightMessageM.LogitudeEntityNumber == _this.invoiceItemPM.DeclarationId) {
                sub.unsubscribe();
                _this.CurrentSession.StopBusyIndicator();
                _this.UnifreightGetRihbitFromTransmissionsCallbackAction(myUnifreightMessageM);
            }
        });
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this.cardListService.getSingle(this.declarationPM.CustomerId)
            .subscribe(function (res) {
            var cardList = res.Result;
            var unifaceCustId = "";
            if (!Tools_1.AppTool.IsNullOrEmpty(cardList)) {
                unifaceCustId = cardList.Code;
            }
            AmitalGatewayUtil_1.AmitalGatewayUtil.Instance
                .GetRihbitFromTransmissions(_this.declarationPM.CustomFileNo, _this.invoiceItemPM.DeclarationId, "SupplierInvoiceItemVehicleComponent", unifaceCustId);
        });
    };
    SupplierInvoiceItemVehicleComponent.prototype.UnifreightGetRihbitFromTransmissionsCallbackAction = function (unifreightMessageM) {
        var rihbitDatalist = AmitalGatewayUtil_1.UnifreightMessageM.GetStringValue(unifreightMessageM, "Response.RihbitData");
        if (Tools_1.AppTool.IsNullOrEmpty(rihbitDatalist)) {
            console.log("Rihbit Data is null - u did not choose any Vehicle");
            return;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(rihbitDatalist)) {
            this._MyResponseObject = JSON.parse(rihbitDatalist);
            if (this._MyResponseObject != null && this._MyResponseObject.Vehicle != null) {
                for (var _i = 0, _a = this._MyResponseObject.Vehicle; _i < _a.length; _i++) {
                    var item = _a[_i];
                    var supplierInvoiceItemVehiclePM = new SupplierInvoiceItemVehiclePM_1.SupplierInvoiceItemVehiclePM(this.invoiceItemPM);
                    supplierInvoiceItemVehiclePM.RichbitFileNumber = item.RichbitFileNumber;
                    supplierInvoiceItemVehiclePM.VehicleChassisNumber = item.VehicleChassisNumber;
                    supplierInvoiceItemVehiclePM.RichbitFileStatus = item.RichbitFileStatus;
                    this.Add(supplierInvoiceItemVehiclePM);
                }
            }
        }
    };
    SupplierInvoiceItemVehicleComponent.prototype.ValidateList = function () {
        var _this = this;
        var errMsg = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Vehicle.O.VehicleWasUsed") + " ";
        var valid = true;
        var errors = [];
        if (this.ItemsSource.Collection) {
            //validate duplicated vehicle in same screen
            this.ItemsSource.Collection.forEach(function (item1) {
                //var vehicle: VehiclePM = null;
                //if (this.vehicles && this.vehicles.length > 0) {
                //    vehicle = this.vehicles.filter(d => d.RichbitFileNumber == item1.RichbitFileNumber)[0];
                //}
                //if (vehicle) {
                _this.ItemsSource.Collection.forEach(function (item2) {
                    if (item1.SequenceNumeric != item2.SequenceNumeric) {
                        if (item1.RichbitFileNumber && item2.RichbitFileNumber) {
                            if (item1.RichbitFileNumber == item2.RichbitFileNumber) {
                                var msg = "רכב זה (" + item2.RichbitFileNumber + ") כבר הוזן בשורות קודמות";
                                errors.push(msg); //duplicated vehicle
                                valid = false;
                            }
                        }
                    }
                });
                // }
            });
            //validate duplicated vehicle in another item in this declaration
            for (var i = 0; i < this.ItemsSource.Collection.length; i++) {
                var vehicle = this.ItemsSource.Collection[i];
                //var vehicleNotItem: VehiclePM = null;
                //if (this.vehicles && this.vehicles.length > 0) {
                //    vehicleNotItem = this.vehicles.filter(d => d.RichbitFileNumber == vehicle.RichbitFileNumber)[0];
                //}
                //if (vehicleNotItem) {
                for (var j = 0; j < this.parent.EntityPM.SupplierInvoiceItems.length; j++) {
                    var item = this.parent.EntityPM.SupplierInvoiceItems[j];
                    var exist;
                    if (vehicle.RichbitFileNumber) {
                        exist = item.SupplierInvoiceItemVehicles
                            .some(function (veh) {
                            return veh.InvoiceItemLineNumber != _this.invoiceItemPM.LineNumber &&
                                veh.RichbitFileNumber == vehicle.RichbitFileNumber;
                        });
                    }
                    if (exist) {
                        var txt = "ריכיבית (" + vehicle.RichbitFileNumber + ") קיימת בהצהרה מספר (" + this.declarationPM.CustomFileNo + ")";
                        errors.push(txt);
                        valid = false;
                    }
                }
                //  }
            }
            //fil validation list
            if (valid) {
                this.ValidationErrorsList = [];
                return true;
            }
            else {
                this.ValidationErrorsList = errors;
                return false;
            }
        }
        else {
            this.ValidationErrorsList = [];
            return true;
        }
    };
    SupplierInvoiceItemVehicleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SupplierInvoiceItemVehicleComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SupplierInvoiceItemVehicleComponent);
    return SupplierInvoiceItemVehicleComponent;
}(BaseComponent_1.BaseComponent));
exports.SupplierInvoiceItemVehicleComponent = SupplierInvoiceItemVehicleComponent;
var InvoiceItemVehicleLine = /** @class */ (function (_super) {
    __extends(InvoiceItemVehicleLine, _super);
    function InvoiceItemVehicleLine(EntityPM, Parent, Item) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.SupplierInvoiceItemVehicle";
        _this.vehicleExtendedPMService = new VehicleExtendedPMService_1.VehicleExtendedPMService();
        _this.vehiclePMService = new VehiclePMService_1.VehiclePMService();
        _this.valid = true;
        _this.vehicleSelected = false;
        _this.entityPM = EntityPM;
        _this.parent = Parent;
        _this.invoiceItem = Item;
        if (_this.RichbitFileNumber != null && _this.VehicleChassisNumber == null) {
            _this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", false);
        }
        if (_this.RichbitFileNumber == null && _this.VehicleChassisNumber != null) {
            _this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", false);
        }
        return _this;
    }
    Object.defineProperty(InvoiceItemVehicleLine.prototype, "SequenceNumeric", {
        //#region properties
        get: function () { return this.entityPM.SequenceNumeric; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemVehicleLine.prototype, "RichbitFileNumber", {
        get: function () {
            return this.entityPM.RichbitFileNumber;
        },
        set: function (value) {
            if (this.entityPM.RichbitFileNumber != value) {
                this.entityPM.RichbitFileNumber = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.entityPM.RichbitFileNumber = value.trim();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemVehicleLine.prototype, "VehicleChassisNumber", {
        get: function () {
            return this.entityPM.VehicleChassisNumber;
        },
        set: function (value) {
            if (this.entityPM.VehicleChassisNumber != value) {
                this.entityPM.VehicleChassisNumber = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.entityPM.VehicleChassisNumber = value.trim();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemVehicleLine.prototype, "RichbitFileStatus", {
        get: function () { return this.entityPM.RichbitFileStatus; },
        set: function (value) {
            if (this.entityPM.RichbitFileStatus != value) {
                this.entityPM.RichbitFileStatus = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoiceItemVehicleLine.prototype, "ExcludeFromInterface", {
        get: function () { return this.entityPM.ExcludeFromInterface; },
        set: function (value) {
            if (this.entityPM.ExcludeFromInterface != value) {
                this.entityPM.ExcludeFromInterface = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    InvoiceItemVehicleLine.prototype.SetFileNumber = function (logCellTemplate, VehicleChassisNumberTextBox) {
        var _this = this;
        //popup validation removed due to some problems whith popup show 
        this.vehicleExtendedPMService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(this.VehicleChassisNumber, null).subscribe(function (response) {
            if (response) {
                _this.vehicle = response.Result;
                if (_this.vehicle != null) {
                    if (_this.vehicle.DeclarationId != null) {
                        ////validation: if the vehicle deleted and added before saving invoice
                        //if (this.vehicle.DeclarationId == this.entityPM.DeclarationId) {
                        //    for (var j = 0; j < this.parent.parent.EntityPM.SupplierInvoiceItems.length; j++) {
                        //        var item = this.parent.parent.EntityPM.SupplierInvoiceItems[j];
                        //        var exist = item.SupplierInvoiceItemVehicles
                        //            .some((veh: SupplierInvoiceItemVehiclePM) =>
                        //                //veh.InvoiceItemLineNumber != this.invoiceItem.LineNumber
                        //                veh.RichbitFileNumber == this.RichbitFileNumber
                        //            //&& veh.VehicleChassisNumber == this.VehicleChassisNumber
                        //            );
                        //        if (exist) {
                        //            //
                        //        } else {
                        //            if (this.entityPM.ChangeSetOp == "insert") {
                        //                var messageWindow = new MessageWindow();
                        //                var msg = "שלדה זו קיימת בחשבון ספק, חובה לשמור את חשבון הספק קודם"; //"This vehicle was used in this invoice, you must save currenct invoice before adding it again.";
                        //                messageWindow.RTL = true;
                        //                messageWindow.Show(msg);
                        //                this.VehicleChassisNumber = "";
                        //            }
                        //            return;
                        //        }
                        //    }
                        //}
                        _this.valid = false;
                        _this.vehicleSelected = false;
                        //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                        //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", false, TextCodeTranslator.Translate("Customs.Vehicle.O.VehicleWasUsed") + " " + this.vehicle.CustomFileNumber);
                        _this.parent.EnableOkButton = false;
                        ////lock focus
                        //SessionLocator.SustainFocusOnCell = true;
                        //this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: VehicleChassisNumberTextBox.InputId });
                        _this.entityPM.RichbitFileNumber = _this.vehicle.RichbitFileNumber;
                        _this.entityPM.RichbitFileStatus = _this.vehicle.StatusName;
                        _this.entityPM.VehicleId = _this.vehicle.Id;
                        _this.RichbitFileStatus = _this.vehicle.StatusName;
                        //this.vehicle.DeclarationId = this.invoiceItem.DeclarationId;
                        _this.oldVehicle = _this.vehicle;
                    }
                    else {
                        _this.valid = true;
                        _this.vehicleSelected = true;
                        //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                        _this.entityPM.RichbitFileNumber = _this.vehicle.RichbitFileNumber;
                        _this.entityPM.RichbitFileStatus = _this.vehicle.StatusName;
                        _this.entityPM.VehicleId = _this.vehicle.Id;
                        _this.RichbitFileStatus = _this.vehicle.StatusName;
                        _this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", false);
                        _this.vehicle.DeclarationId = _this.invoiceItem.DeclarationId;
                        _this.oldVehicle = _this.vehicle;
                        _this.parent.EnableOkButton = true;
                    }
                }
                else {
                    _this.vehicleSelected = true;
                    _this.valid = true;
                    _this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", false);
                    //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                    _this.entityPM.RichbitFileNumber = null;
                    _this.entityPM.VehicleId = null;
                    _this.RichbitFileStatus = null;
                    _this.parent.EnableOkButton = true;
                    if (_this.oldVehicle != null) {
                        _this.oldVehicle.DeclarationId = null;
                    }
                }
            }
        });
    };
    InvoiceItemVehicleLine.prototype.SetChassisNumber = function (logCellTemplate, RichbitFileNumbernTextBox) {
        var _this = this;
        //popup validation removed due to some problems whith popup show 
        this.vehicleExtendedPMService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(null, this.RichbitFileNumber).subscribe(function (response) {
            if (response) {
                _this.vehicle = response.Result;
                if (_this.vehicle != null) {
                    if (_this.vehicle.DeclarationId != null) {
                        ////validation: if the vehicle deleted and added before saving invoice
                        //if (this.vehicle.DeclarationId == this.entityPM.DeclarationId) {
                        //    for (var j = 0; j < this.parent.parent.EntityPM.SupplierInvoiceItems.length; j++) {
                        //        var item = this.parent.parent.EntityPM.SupplierInvoiceItems[j];
                        //        var exist = item.SupplierInvoiceItemVehicles
                        //            .some((veh: SupplierInvoiceItemVehiclePM) =>
                        //                //veh.InvoiceItemLineNumber != this.invoiceItem.LineNumber
                        //                veh.RichbitFileNumber == this.RichbitFileNumber
                        //            //&& veh.VehicleChassisNumber == this.VehicleChassisNumber
                        //            );
                        //        if (exist) {
                        //            //
                        //        } else {
                        //            if (this.entityPM.ChangeSetOp == "insert") {
                        //                var messageWindow = new MessageWindow();
                        //                var msg = "שלדה זו קיימת בחשבון ספק, חובה לשמור את חשבון הספק קודם"; //"This vehicle was used in this invoice, you must save currenct invoice before adding it again.";
                        //                messageWindow.RTL = true;
                        //                messageWindow.Show(msg);
                        //                this.RichbitFileNumber = "";
                        //            }
                        //            return;
                        //        }
                        //    }
                        //}
                        //connected to dec --- NOT VALID
                        _this.valid = false;
                        _this.vehicleSelected = false;
                        _this.parent.EnableOkButton = false;
                        _this.entityPM.VehicleChassisNumber = _this.vehicle.VehicleChassisNumber;
                        _this.entityPM.RichbitFileStatus = _this.vehicle.StatusName;
                        _this.entityPM.VehicleId = _this.vehicle.Id;
                        _this.RichbitFileStatus = _this.vehicle.StatusName;
                        //this.vehicle.DeclarationId = this.invoiceItem.DeclarationId;
                        _this.oldVehicle = _this.vehicle;
                        //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", false, TextCodeTranslator.Translate("Customs.Vehicle.O.VehicleWasUsed") + " " + this.vehicle.CustomFileNumber);
                        ////lock focus
                        //SessionLocator.SustainFocusOnCell = true;
                        //this.CurrentSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: RichbitFileNumbernTextBox.InputId });
                    }
                    else {
                        //VALID 
                        _this.valid = true;
                        _this.vehicleSelected = true;
                        _this.parent.EnableOkButton = true;
                        //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                        _this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", false);
                        _this.entityPM.VehicleChassisNumber = _this.vehicle.VehicleChassisNumber;
                        _this.entityPM.RichbitFileStatus = _this.vehicle.StatusName;
                        _this.entityPM.VehicleId = _this.vehicle.Id;
                        _this.RichbitFileStatus = _this.vehicle.StatusName;
                        _this.vehicle.DeclarationId = _this.invoiceItem.DeclarationId;
                        _this.oldVehicle = _this.vehicle;
                    }
                }
                else {
                    // NO Vehicle
                    _this.valid = true;
                    _this.vehicleSelected = true;
                    _this.parent.EnableOkButton = true;
                    //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
                    _this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", false);
                    _this.entityPM.VehicleChassisNumber = null;
                    _this.entityPM.VehicleId = null;
                    _this.RichbitFileStatus = null;
                    if (_this.oldVehicle != null) {
                        _this.oldVehicle.DeclarationId = null;
                    }
                }
            }
        });
    };
    InvoiceItemVehicleLine.prototype.DeleteButtonClicked = function () {
        var _this = this;
        this.parent.ItemsSource.Remove(this);
        if (this.parent.invoiceItemPM.SupplierInvoiceItemVehicles.includes(this.entityPM)) {
            this.parent.invoiceItemPM.RemoveSupplierInvoiceItemVehicle(this.entityPM);
            //delete from general
            //var invoice = this.parent.parent.declarationPM.SupplierInvoices.find(d => d.InvoiceCounterKey == this.parent.supplierInvoicePM.InvoiceCounterKey);
            var invoice = this.parent.supplierInvoicePM;
            var exist = false;
            invoice.SupplierInvoiceItems.forEach(function (item) {
                var veh = item.SupplierInvoiceItemVehicles.find(function (d) { return d.RichbitFileNumber == _this.RichbitFileNumber && d.VehicleChassisNumber == _this.VehicleChassisNumber; });
                if (veh)
                    exist = true;
            });
            if (!exist) {
                var i = this.parent.parent.addedVehicles.findIndex(function (d) { return d.RichbitFileNumber == _this.RichbitFileNumber && d.VehicleChassisNumber == _this.VehicleChassisNumber; });
                if (!Tools_1.AppTool.IsNullOrEmpty(i))
                    this.parent.parent.addedVehicles.splice(i, 1);
            }
            ////Commentd=> moved to the server by mohammad
            //this.vehicleExtendedPMService.GetVehicleByVehicleChassisNumberOrRichbitFileNumber(this.VehicleChassisNumber, this.RichbitFileNumber).subscribe(response => {
            //    if (response) {
            //        if (!response.HasError) {
            //            var vehicle: VehiclePM = response.Result;
            //            if (vehicle) {
            //                vehicle.DeclarationId = null;
            //                this.vehiclePMService.update(vehicle).subscribe(response => {
            //                });
            //            } 
            //        }
            //    }
            //});
        }
    };
    InvoiceItemVehicleLine.prototype.OnRichbitFileNumbernLostFocus = function (logCellTemplate, RichbitFileNumbernTextBox) {
        if (this.RichbitFileNumber) {
            if (!this.vehicleSelected)
                this.SetChassisNumber(logCellTemplate, RichbitFileNumbernTextBox);
        }
        else { // enable all
            //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
            //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
            this.vehicleSelected = false;
            this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true);
            this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true);
        }
    };
    InvoiceItemVehicleLine.prototype.OnVehicleChassisNumberLostFocus = function (logCellTemplate, VehicleChassisNumberTextBox) {
        if (this.VehicleChassisNumber) {
            if (!this.vehicleSelected)
                this.SetFileNumber(logCellTemplate, VehicleChassisNumberTextBox);
        }
        else { // enable all
            //this.UIProperties.SetValidity("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
            //this.UIProperties.SetValidity("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true, null);
            this.vehicleSelected = false;
            this.UIProperties.SetEnabled("RichbitFileNumber", "Customs.SupplierInvoiceItemVehicle", true);
            this.UIProperties.SetEnabled("VehicleChassisNumber", "Customs.SupplierInvoiceItemVehicle", true);
        }
    };
    return InvoiceItemVehicleLine;
}(BaseComponent_1.BaseComponent));
exports.InvoiceItemVehicleLine = InvoiceItemVehicleLine;
var VehicleValidationError = /** @class */ (function () {
    function VehicleValidationError() {
        this.IsVehicle = false;
    }
    return VehicleValidationError;
}());
exports.VehicleValidationError = VehicleValidationError;
//# sourceMappingURL=SupplierInvoiceItemVehicleComponent.js.map