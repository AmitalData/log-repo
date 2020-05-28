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
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ShipmentDeliveryPM_1 = require("../../../../../Shipment/EntityPMs/ShipmentDeliveryPM");
var ShipmentPickUpDeliveryPackagePM_1 = require("../../../../../Shipment/EntityPMs/ShipmentPickUpDeliveryPackagePM");
var ShipmentPMService_1 = require("../../../../../Shipment/Services/StandardPMs/ShipmentPMService");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../../../Infrastructure/Validators/Validator");
var ShipmentValidator_1 = require("../../../../../Shipment/Validators/ShipmentValidator");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var Tools_2 = require("../../../../../Shipment/Tools");
var ContainerFollowupWizardComponent = /** @class */ (function (_super) {
    __extends(ContainerFollowupWizardComponent, _super);
    function ContainerFollowupWizardComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.ValidationErrorsList = [];
        _this.IsResourcesReady = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsDeliveryConnectedWithMultiContainers = false;
        _this.isLoaderReady = false;
        _this.Retries = 0;
        _this.entityPMService = new ShipmentPMService_1.ShipmentPMService();
        return _this;
    }
    ContainerFollowupWizardComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("Shipment").subscribe(function (res1) {
            _this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(function (res2) {
                _this.entityResourceService.getEntityResourceByTableName("ShipmentPackage").subscribe(function (res3) {
                    var entityId = args['EntityId'];
                    if (entityId) {
                        _this.EntityId = entityId.split(':')[0];
                        _this.Shipmentd = entityId.split(':')[1];
                        _this.CurrentSession.StartBusyIndicatorLoading();
                        _this.entityPMService.get(_this.Shipmentd).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                _this.ShipmentPM = myResponse.Result;
                                _this.EntityPM = _this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.Id == _this.EntityId; })[0];
                                if (_this.EntityPM) {
                                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.DeliveryId)) {
                                        var allPackages = _this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.DeliveryId == _this.EntityPM.DeliveryId; });
                                        if (allPackages.length > 1) {
                                            _this.IsDeliveryConnectedWithMultiContainers = true;
                                        }
                                    }
                                }
                                _this.RunComponent();
                                _this.IsResourcesReady = true;
                            }
                            else {
                                _this.ValidationErrorsList = myResponse.ErrorsArray;
                            }
                            _this.CurrentSession.StopBusyIndicator();
                        });
                    }
                });
            });
        });
    };
    ContainerFollowupWizardComponent.prototype.RunComponent = function () {
        if (this.ChildViewContainerRef) {
            this.isLoaderReady = true;
            this.LoadTemplate();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ContainerFollowupWizardComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ContainerFollowupWizardComponent.prototype.LoadTemplate = function () {
        var _this = this;
        if (this.ChildViewContainerRef) {
            this.ChildViewContainerRef.clear();
            var myComponentPath = './ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupWizardTemplate';
            SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.ChildViewContainerRef)
                .then(function (cmpRef) {
                _this.TemplateComponent = cmpRef.instance;
                cmpRef.instance.Run({ FatherComponent: _this });
            });
        }
    };
    ContainerFollowupWizardComponent.prototype.CloseClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit(this.EntityId);
    };
    ContainerFollowupWizardComponent.prototype.SaveClicked = function () {
        var isValid = this.Validate();
        if (isValid) {
            this.Save();
        }
    };
    ContainerFollowupWizardComponent.prototype.Validate = function () {
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, "ShipmentPackage", errors);
        Validator_1.Validator.TryValidateObject(this.ShipmentPM, "Shipment", errors);
        // Series Dates
        Tools_2.RoutingHelper.ValidateRoutingsSeriesDates_PackageFollowup(this.ShipmentPM, this.EntityPM, errors, "D");
        Tools_2.RoutingHelper.ValidateRoutingsSeriesDates_PackageFollowup(this.ShipmentPM, this.EntityPM, errors, "R");
        // Actual Dates
        if (!Tools_1.DateTool.IsActualDateValid(this.EntityPM.DeliveryATD)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATD")));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.EntityPM.DeliveryATA)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATA")));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.EntityPM.EmptyContainerReturnATD)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATD")));
        }
        if (!Tools_1.DateTool.IsActualDateValid(this.EntityPM.EmptyContainerReturnATA)) {
            errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATA")));
        }
        if (errors.length == 0) {
            var myShipmentValidator = new ShipmentValidator_1.ShipmentValidator();
            var myShipmentErrors = myShipmentValidator.Validate(this.ShipmentPM);
            if (myShipmentErrors.length > 0) {
                errors = myShipmentErrors;
                //errors.push(TextCodeTranslator.Translate("Shipment.M.Routings.CantProceedAddingDelivery"));
            }
        }
        this.ValidationErrorsList = errors;
        return errors.length == 0 ? true : false;
    };
    ContainerFollowupWizardComponent.prototype.Save = function (myCommandCode) {
        var _this = this;
        if (myCommandCode === void 0) { myCommandCode = null; }
        if (this.ShipmentPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.entityPMService.update(this.ShipmentPM).subscribe(function (myResponse) {
                if (myResponse.HasError) {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
                else {
                    _this.ShipmentPM = myResponse.Result;
                    _this.EntityPM = _this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.Id == _this.EntityId; })[0];
                    _this.LoadTemplate();
                    _this.OnSaveCompleted(myCommandCode);
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
        else {
            this.OnSaveCompleted(myCommandCode);
        }
    };
    ContainerFollowupWizardComponent.prototype.OnSaveCompleted = function (myCommandCode) {
        if (myCommandCode === void 0) { myCommandCode = null; }
        switch (myCommandCode) {
            case "ActionLink_D": {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                    this.AddRouting("D");
                }
                else {
                    this.EditRouting("D");
                }
                break;
            }
            case "ActionLink_R": {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EmptyContainerReturnId)) {
                    this.AddRouting("R");
                }
                else {
                    this.EditRouting("R");
                }
                break;
            }
        }
    };
    ContainerFollowupWizardComponent.prototype.AddRouting = function (typeCode) {
        var _this = this;
        var myDeliveryIndex = 1;
        var myWindowTitle = null;
        var myPickUpDeliveryTypeCode = null;
        switch (typeCode) {
            case "R": {
                myPickUpDeliveryTypeCode = "EMPT";
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddEmptyCR");
                if (this.ShipmentPM.ShipmentContainerReturnIndex) {
                    myDeliveryIndex = this.ShipmentPM.ShipmentContainerReturnIndex + 1;
                }
                break;
            }
            default: {
                myPickUpDeliveryTypeCode = "DELV";
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.AddDelivery");
                if (this.ShipmentPM.ShipmentDeliveryIndex) {
                    myDeliveryIndex = this.ShipmentPM.ShipmentDeliveryIndex + 1;
                }
                break;
            }
        }
        var newDeliveryPM = new ShipmentDeliveryPM_1.ShipmentDeliveryPM(null);
        newDeliveryPM.FullResponsibility = true;
        newDeliveryPM.Tenant = this.ShipmentPM.Tenant;
        newDeliveryPM.ShipmentId = this.ShipmentPM.Id;
        newDeliveryPM.ShipmentNumber = this.ShipmentPM.ShipmentNumber;
        newDeliveryPM.PickUpDeliveryNumber = this.ShipmentPM.ShipmentNumber + "/" + myDeliveryIndex;
        newDeliveryPM.PickUpDeliveryTypeCode = myPickUpDeliveryTypeCode;
        newDeliveryPM.ConnectedPackageId = this.EntityPM.Id;
        switch (typeCode) {
            case "D": {
                newDeliveryPM.ETD = this.EntityPM.DeliveryETD;
                newDeliveryPM.ATD = this.EntityPM.DeliveryATD;
                newDeliveryPM.ETA = this.EntityPM.DeliveryETA;
                newDeliveryPM.ATA = this.EntityPM.DeliveryATA;
                newDeliveryPM.TransportModeCode = this.EntityPM.DeliveryTransportModeCode;
                break;
            }
            case "R": {
                newDeliveryPM.ETD = this.EntityPM.EmptyContainerReturnETD;
                newDeliveryPM.ATD = this.EntityPM.EmptyContainerReturnATD;
                newDeliveryPM.ETA = this.EntityPM.EmptyContainerReturnETA;
                newDeliveryPM.ATA = this.EntityPM.EmptyContainerReturnATA;
                newDeliveryPM.TransportModeCode = this.EntityPM.ECRTransportModeCode;
                break;
            }
        }
        var newDeliveryPackagePM = new ShipmentPickUpDeliveryPackagePM_1.ShipmentPickUpDeliveryPackagePM(null);
        newDeliveryPackagePM.Tenant = this.EntityPM.Tenant;
        newDeliveryPackagePM.ContainerNumber = this.EntityPM.ContainerNumber;
        newDeliveryPackagePM.Description = this.EntityPM.Description;
        newDeliveryPackagePM.PackageTypeId = this.EntityPM.PackageTypeId;
        newDeliveryPackagePM.PackageTypeName = this.EntityPM.PackageTypeName;
        newDeliveryPackagePM.Quantity = this.EntityPM.Quantity;
        newDeliveryPackagePM.Volume = this.EntityPM.Volume;
        newDeliveryPackagePM.Weight = this.EntityPM.Weight;
        newDeliveryPackagePM.ShipperSeal = this.EntityPM.ShipperSeal;
        newDeliveryPackagePM.Width = this.EntityPM.Width;
        newDeliveryPackagePM.Height = this.EntityPM.Height;
        newDeliveryPackagePM.Length = this.EntityPM.Length;
        newDeliveryPackagePM.Harmonize = this.EntityPM.Harmonize;
        newDeliveryPackagePM.OriginalShipmentPackageId = this.EntityPM.Id;
        newDeliveryPM.AddPackage(newDeliveryPackagePM);
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = myWindowTitle;
        logitudeWindow.WindowArgs = { ShipmentPM: this.ShipmentPM, EntityPM: newDeliveryPM, IsNewEntity: true, ContainerReturnDeliveryId: this.EntityPM.DeliveryId, IsContainerFollowup: true };
        logitudeWindow.Width = 950;
        logitudeWindow.Height = 595;
        logitudeWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.ShipmentPM = s;
                _this.EntityPM = _this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.Id == _this.EntityId; })[0];
                _this.LoadTemplate();
            }
        });
        logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
    };
    ContainerFollowupWizardComponent.prototype.EditRouting = function (typeCode) {
        var _this = this;
        var myDeliveryId = null;
        var myWindowTitle = null;
        var myEditedDelivery = null;
        switch (typeCode) {
            case "R": {
                myDeliveryId = this.EntityPM.EmptyContainerReturnId;
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditEmptyCR");
                break;
            }
            default: {
                myDeliveryId = this.EntityPM.DeliveryId;
                myWindowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.O.Routings.EditDelivery");
                break;
            }
        }
        var myEditedDelivery = this.ShipmentPM.ShipmentDeliveries.filter(function (f) { return f.Id == myDeliveryId; })[0];
        if (myEditedDelivery) {
            var windowTitle = myWindowTitle + ": " + myEditedDelivery.PickUpDeliveryNumber;
            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
            logitudeWindow.Title = windowTitle;
            logitudeWindow.WindowArgs = { ShipmentPM: this.ShipmentPM, EntityPM: myEditedDelivery, IsNewEntity: false, IsContainerFollowup: true };
            logitudeWindow.Width = 950;
            logitudeWindow.Height = 595;
            logitudeWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.ShipmentPM = s;
                    _this.EntityPM = _this.ShipmentPM.ShipmentPackages.filter(function (f) { return f.Id == _this.EntityId; })[0];
                    _this.LoadTemplate();
                }
            });
            logitudeWindow.Show('./ShipmentModules/ShipmentRouting/Components/Routings/AddEditDeliveryComponent');
        }
    };
    ContainerFollowupWizardComponent.prototype.ViewShipmentClicked = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.ShipmentPM.Id, ObjectTableName: 'Shipment', BackButtonLabel: "Shipment: " + _this.ShipmentPM.ShipmentNumber });
            //let isEditComponentSaved = false;
            //cmpRef.instance.BackCompleted.subscribe(bk => {
            //    if (isEditComponentSaved) {
            //        this.entityArgs.EditComponent.ReloadEntityPM();
            //    }
            //});
            //cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            //    if (isSaveSuccess) {
            //        isEditComponentSaved = true;
            //    }
            //});
            //cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
            //    if (isSaveSuccess) {
            //        isEditComponentSaved = true;
            //    }
            //});
        });
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ContainerFollowupWizardComponent.prototype, "ChildViewContainerRef", void 0);
    ContainerFollowupWizardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContainerFollowupWizardComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ContainerFollowupWizardComponent);
    return ContainerFollowupWizardComponent;
}(BaseComponent_1.BaseComponent));
exports.ContainerFollowupWizardComponent = ContainerFollowupWizardComponent;
//# sourceMappingURL=ContainerFollowupWizardComponent.js.map