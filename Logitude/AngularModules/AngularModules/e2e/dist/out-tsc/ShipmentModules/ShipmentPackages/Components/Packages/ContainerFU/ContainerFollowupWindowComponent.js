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
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../../Infrastructure/Utilities/Cloner");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_2 = require("../../../../../Shipment/Tools");
var ServiceLocator_1 = require("../../../../../Infrastructure/Locators/ServiceLocator");
var ContainerFollowupWindowComponent = /** @class */ (function () {
    function ContainerFollowupWindowComponent() {
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsDeliveryConnectedWithMultiContainers = false;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.SaveCompletedEvent = null;
        this.SaveActionCompletedEvent = null;
    }
    ContainerFollowupWindowComponent.prototype.SetWindowArgs = function (args) {
        this.Code = args['Code'];
        this.DataContext = args['DataContext'];
        this.EntityPM = this.DataContext.EntityPM;
        this.TabComponent = this.DataContext.fatherComponent;
        this.IsNewFollowup = args['IsNewFollowup'];
        this.IsNewFollowup_Totango = args['IsNewFollowup'];
        this.CheckMultiConnected();
        this.Listen();
        this.RunComponent();
    };
    ContainerFollowupWindowComponent.prototype.CheckMultiConnected = function () {
        var _this = this;
        var isDeliveryConnectedWithMultiContainers = false;
        if (this.Code == "D") {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                var allPackages = this.TabComponent.EntityPM.ShipmentPackages.filter(function (f) { return f.DeliveryId == _this.EntityPM.DeliveryId; });
                if (allPackages.length > 1) {
                    isDeliveryConnectedWithMultiContainers = true;
                }
            }
        }
        this.IsDeliveryConnectedWithMultiContainers = isDeliveryConnectedWithMultiContainers;
    };
    ContainerFollowupWindowComponent.prototype.RunComponent = function () {
        if (this.ChildViewContainerRef) {
            this.isLoaderReady = true;
            this.LoadTemplate();
        }
        else {
            this.RunComponentTimer();
        }
    };
    ContainerFollowupWindowComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    ContainerFollowupWindowComponent.prototype.LoadTemplate = function () {
        var _this = this;
        if (this.ChildViewContainerRef) {
            this.ChildViewContainerRef.clear();
            var myComponentPath = './ShipmentModules/ShipmentPackages/Components/Packages/ContainerFU/ContainerFollowupWindowTemplate';
            SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.ChildViewContainerRef)
                .then(function (cmpRef) {
                _this.TemplateComponent = cmpRef.instance;
                cmpRef.instance.Run({ Code: _this.Code, DataContext: _this.DataContext, FatherComponent: _this });
                _this.Clone();
                if (_this.IsNewFollowup) {
                    _this.IsNewFollowup = false;
                    _this.TemplateComponent.IsFollowup = true;
                    _this.TemplateComponent.TransportModeCode = "BYTR";
                }
            });
        }
    };
    ContainerFollowupWindowComponent.prototype.Listen = function () {
        var _this = this;
        this.SaveCompletedEvent = this.TabComponent.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
            if (isSaveSuccess) {
                if (_this.myRequestedCommand == "ActionLink" || _this.myRequestedCommand == "DeleteLink") {
                    _this.TabComponent.BuildItemsSource();
                    _this.DataContext = _this.TabComponent.ItemsSource.Collection.filter(function (f) { return f.EntityPM.Id == _this.EntityPM.Id; })[0];
                    _this.EntityPM = _this.DataContext.EntityPM;
                    _this.CheckMultiConnected();
                    _this.LoadTemplate();
                }
            }
        });
    };
    ContainerFollowupWindowComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveActionCompletedEvent);
        this.SaveCompletedEvent = null;
        this.SaveActionCompletedEvent = null;
    };
    ContainerFollowupWindowComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    ContainerFollowupWindowComponent.prototype.OkButtonClicked = function () {
        var isValid = this.Validate();
        if (isValid) {
            this.Save("Ok");
        }
    };
    ContainerFollowupWindowComponent.prototype.Validate = function () {
        var errors = [];
        // Series Dates
        Tools_2.RoutingHelper.ValidateRoutingsSeriesDates_PackageFollowup(this.DataContext.ShipmentPM, this.EntityPM, errors, this.Code);
        // Actual Dates
        switch (this.Code) {
            case "D": {
                if (!Tools_1.DateTool.IsActualDateValid(this.EntityPM.DeliveryATD)) {
                    errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATD")));
                }
                if (!Tools_1.DateTool.IsActualDateValid(this.EntityPM.DeliveryATA)) {
                    errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.DeliveryATA")));
                }
                break;
            }
            case "R": {
                if (!Tools_1.DateTool.IsActualDateValid(this.EntityPM.EmptyContainerReturnATD)) {
                    errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATD")));
                }
                if (!Tools_1.DateTool.IsActualDateValid(this.EntityPM.EmptyContainerReturnATA)) {
                    errors.push(Tools_1.DateTool.ActualDateMessage.replace("Field", TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentPackage.F.EmptyContainerReturnATA")));
                }
                break;
            }
        }
        this.ValidationErrorsList = errors;
        return errors.length == 0 ? true : false;
    };
    ContainerFollowupWindowComponent.prototype.Save = function (myCommand) {
        var _this = this;
        this.myRequestedCommand = myCommand;
        if (!this.SaveActionCompletedEvent) {
            this.SaveActionCompletedEvent = this.TabComponent.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    if (_this.IsNewFollowup_Totango) {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Container F/U", "Added Container F/U");
                    }
                    _this.TabComponent.BuildItemsSource();
                    _this.DataContext = _this.TabComponent.ItemsSource.Collection.filter(function (f) { return f.EntityPM.Id == _this.EntityPM.Id; })[0];
                    _this.EntityPM = _this.DataContext.EntityPM;
                    _this.CheckMultiConnected();
                    _this.OnSaveCompleted();
                }
                else {
                    _this.ValidationErrorsList = _this.DataContext.fatherComponent.entityArgs.EditComponent.ValidationErrorsList;
                }
                Tools_1.AppTool.KillEventEmitter(_this.SaveActionCompletedEvent);
                _this.SaveActionCompletedEvent = null;
            });
            this.TabComponent.entityArgs.EditComponent.SaveChanges();
        }
    };
    ContainerFollowupWindowComponent.prototype.OnSaveCompleted = function () {
        switch (this.myRequestedCommand) {
            case "Ok": {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
                break;
            }
            case "ActionLink": {
                switch (this.Code) {
                    case "D": {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.DeliveryId)) {
                            this.DataContext.AddRouting(this.Code);
                        }
                        else {
                            this.DataContext.EditRouting(this.Code);
                        }
                        break;
                    }
                    case "R": {
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.EmptyContainerReturnId)) {
                            this.DataContext.AddRouting(this.Code);
                        }
                        else {
                            this.DataContext.EditRouting(this.Code);
                        }
                        break;
                    }
                }
                break;
            }
            case "DeleteLink": {
                //this.DataContext.SetUIProperties_ContainerFU();
                break;
            }
        }
    };
    ContainerFollowupWindowComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.TemplateComponent);
        this.myCloner.AddField('IsFollowup');
        this.myCloner.AddField('ETD');
        this.myCloner.AddField('ATD');
        this.myCloner.AddField('ETA');
        this.myCloner.AddField('ATA');
        this.myCloner.AddField('From');
        this.myCloner.AddField('To');
        this.myCloner.AddField('TransportModeCode');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    };
    ContainerFollowupWindowComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], ContainerFollowupWindowComponent.prototype, "ChildViewContainerRef", void 0);
    ContainerFollowupWindowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ContainerFollowupWindowComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ContainerFollowupWindowComponent);
    return ContainerFollowupWindowComponent;
}());
exports.ContainerFollowupWindowComponent = ContainerFollowupWindowComponent;
//# sourceMappingURL=ContainerFollowupWindowComponent.js.map