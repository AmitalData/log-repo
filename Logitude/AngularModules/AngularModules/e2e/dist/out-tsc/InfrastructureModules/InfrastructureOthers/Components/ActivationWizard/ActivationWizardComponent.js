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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var HybridPartnerExtendedListService_1 = require("../../../../Common/Services/ExtendedLists/HybridPartnerExtendedListService");
var WebFreightDomainService_1 = require("../../../../Infrastructure/Services/WebFreightDomainService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var CustomerPMService_1 = require("../../../../Common/Services/StandardPMs/CustomerPMService");
var CustomerTenantAccessRequestPM_1 = require("../../../../Common/EntityPMs/CustomerTenantAccessRequestPM");
var CustomerTenantAccessRequestExtendedPMService_1 = require("../../../../Common/Services/ExtendedPMs/CustomerTenantAccessRequestExtendedPMService");
var ActivationWizardComponent = /** @class */ (function () {
    function ActivationWizardComponent(CD) {
        this.CD = CD;
        this.AddPartnerToWizard = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this._HybridPartnerListService = new HybridPartnerExtendedListService_1.HybridPartnerExtendedListService();
        this._CustomerPMService = new CustomerPMService_1.CustomerPMService();
        this._CustomerTenantAccessRequestExtendedPMService = new CustomerTenantAccessRequestExtendedPMService_1.CustomerTenantAccessRequestExtendedPMService();
    }
    ActivationWizardComponent.prototype.ngOnInit = function () {
        this.hybridPartnerList = [];
        this.allhybridPartnerList = [];
        this.ValidationErrorsList = [];
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "ADDPARTNERTOACTIVATIONWIZARD")) {
            this.AddPartnerToWizard = true;
        }
        else {
            this.AddPartnerToWizard = false;
        }
        this.FillHybridPartnerList();
    };
    ActivationWizardComponent.prototype.ngAfterViewInit = function () {
    };
    ActivationWizardComponent.prototype.FillHybridPartnerList = function () {
        var _this = this;
        this._HybridPartnerListService.GetHybridPartnerLists(SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            _this.hybridPartnerList = [];
            res.forEach(function (item, key) {
                _this.hybridPartnerList.push(new HybridPartnerData(item, _this));
            });
            _this.CurrentSession.StopBusyIndicator();
        });
        this._HybridPartnerListService.GetHybridPartnerListWithNoRequest(SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            _this.allhybridPartnerList = [];
            res.forEach(function (item, key) {
                _this.allhybridPartnerList.push(new HybridPartnerData(item, _this));
            });
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ActivationWizardComponent.prototype.RefreshBtnClick = function () {
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this.ValidationErrorsList = [];
        this.FillHybridPartnerList();
    };
    ActivationWizardComponent.prototype.SendRequest = function (item) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator("loading ...");
        this._CustomerTenantAccessRequestExtendedPMService.get(item.ReqId).subscribe(function (res) {
            if (!res.HasError) {
                var temp = res.Result;
                temp.RequestStatus = "W";
                _this._CustomerTenantAccessRequestExtendedPMService.update(temp).subscribe(function (res1) {
                    item.StatusName = "Waiting For Approval";
                    item.IsHasRequest = true;
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                _this.ValidationErrorsList = res.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    ActivationWizardComponent.prototype.AddRequest = function (item) {
        var _this = this;
        //List < ValidationResult > errors = new List<ValidationResult>();
        //busyIndicatorStartEvent.Publish(new BusyIndicatorStartEventArgs() { Message = "Loading ...", Start = true });
        //bool validateEntry = ValidateEntry();
        //bool hasValidationErrors = CheckValidationErrors();
        this.CurrentSession.StartBusyIndicator("Adding ...");
        if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.CustomerId)) {
            this.ValidationErrorsList.push("Missing Customer in Tenant Definitions !");
        }
        if (Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.TenantPM.VatNumber)) {
            this.ValidationErrorsList.push("VatNumber is not defined");
        }
        //FillErrors(errors);
        if (this.ValidationErrorsList.length == 0) {
            this._CustomerPMService.get(SessionLocator_1.SessionLocator.TenantPM.CustomerId).subscribe(function (res) {
                if (!res.HasError) {
                    var CustomerPm = res.Result;
                    if (CustomerPm != null) {
                        if (Tools_1.AppTool.IsNullOrEmpty(CustomerPm.PrimaryContactId)) {
                            _this.ValidationErrorsList.push("Missing Primary Contact Details in Customer " + (!Tools_1.AppTool.IsNullOrEmpty(CustomerPm.EnglishName) != null ? CustomerPm.EnglishName : CustomerPm.LocalName));
                        }
                        if (_this.ValidationErrorsList.length == 0) {
                            var pm = new CustomerTenantAccessRequestPM_1.CustomerTenantAccessRequestPM();
                            pm.ForwarderId = item.Id,
                                pm.RequestStatus = "N",
                                pm.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            _this._CustomerTenantAccessRequestExtendedPMService.insert(pm).subscribe(function (res) {
                                // We Need To check If There Are Errors.
                                _this.FillHybridPartnerList();
                                _this.CurrentSession.StopBusyIndicator();
                            });
                            //if (!context.CustomerTenantAccessRequestPMs.Contains(pm)) {
                            //    context.CustomerTenantAccessRequestPMs.Add(pm);
                            //}
                            //if (submitOperation != null) {
                            //    if (!submitOperation.IsComplete) {
                            //        submitOperation.Cancel();
                            //    }
                            //}
                            //submitOperation = context.SubmitChanges();
                            //submitOperation.Completed += submitOperation_Completed;
                        }
                        else {
                            _this.CurrentSession.StopBusyIndicator();
                        }
                    }
                    else {
                        _this.CurrentSession.StopBusyIndicator();
                    }
                }
                else {
                    _this.ValidationErrorsList = res.ErrorsArray;
                }
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
        }
    };
    ActivationWizardComponent = __decorate([
        core_1.Component({
            selector: 'ActivationWizard',
            moduleId: module.id,
            templateUrl: './ActivationWizardComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ActivationWizardComponent);
    return ActivationWizardComponent;
}());
exports.ActivationWizardComponent = ActivationWizardComponent;
var HybridPartnerData = /** @class */ (function () {
    function HybridPartnerData(passedhybridPartnerList, Parent) {
        var _this = this;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.buttonVisibility = true;
        this.statusVisibility = true;
        this.ParentComponent = Parent;
        this.hybridPartnerList = passedhybridPartnerList;
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        myService.getHypridPartnerLogo(this.hybridPartnerList.LogoId).subscribe(function (myResult) {
            if (myResult) {
                _this.Source = "data:image/JPEG;base64," + myResult;
                //Parent.CD.detectChanges();
            }
        });
    }
    Object.defineProperty(HybridPartnerData.prototype, "Id", {
        get: function () {
            return this.hybridPartnerList.Id;
        },
        set: function (newValue) {
            this.hybridPartnerList.Id = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerData.prototype, "IsHasRequest", {
        get: function () {
            return this.hybridPartnerList.IsHasRequest;
        },
        set: function (newValue) {
            this.hybridPartnerList.IsHasRequest = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerData.prototype, "LogoId", {
        get: function () {
            return this.hybridPartnerList.LogoId;
        },
        set: function (newValue) {
            this.hybridPartnerList.LogoId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerData.prototype, "StatusName", {
        get: function () {
            return this.hybridPartnerList.StatusName;
        },
        set: function (newValue) {
            this.hybridPartnerList.StatusName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerData.prototype, "Name", {
        get: function () {
            return this.hybridPartnerList.Name;
        },
        set: function (newValue) {
            this.hybridPartnerList.Name = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerData.prototype, "ReqId", {
        get: function () {
            return this.hybridPartnerList.ReqId;
        },
        set: function (newValue) {
            this.hybridPartnerList.ReqId = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerData.prototype, "ButtonVisibility", {
        get: function () {
            if (this.hybridPartnerList.IsHasRequest == true) {
                this.buttonVisibility = false;
            }
            else {
                this.buttonVisibility = true;
            }
            return this.buttonVisibility;
        },
        set: function (newValue) {
            this.buttonVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HybridPartnerData.prototype, "StatusVisibility", {
        get: function () {
            if (this.hybridPartnerList.IsHasRequest == true) {
                this.statusVisibility = true;
            }
            else {
                this.statusVisibility = false;
            }
            return this.statusVisibility;
        },
        set: function (newValue) {
            this.statusVisibility = newValue;
        },
        enumerable: true,
        configurable: true
    });
    return HybridPartnerData;
}());
exports.HybridPartnerData = HybridPartnerData;
//# sourceMappingURL=ActivationWizardComponent.js.map