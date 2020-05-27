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
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var AddressValidator_1 = require("../../../../Infrastructure/Validators/AddressValidator");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AddEditAddressComponent = /** @class */ (function () {
    function AddEditAddressComponent() {
        this.EntityPM = null;
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.LoadCompletedEvent = null;
    }
    AddEditAddressComponent.prototype.ngOnInit = function () {
        if (this.DataContext != null) {
            this.DataContext.SetUIProperties();
        }
    };
    AddEditAddressComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ObjectTableName = dataContext.ObjectTableName;
        this.DomainService = dataContext.fatherComponent.DomainService;
        this.Clone();
        if (dataContext.IsNewEntity) {
            dataContext.Name = dataContext.fatherComponent.EntityPM.EnglishName;
        }
        if (dataContext.IsCopyMainAddress) {
            dataContext.CopyMainAddress();
        }
    };
    AddEditAddressComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditAddressComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.StartBusyIndicatorSaving();
        var isValid = this.Validate();
        if (!isValid) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            if (!this.EntityPM.IsDirty) {
                this.CurrentSession.CloseCurrentWindow();
            }
            else {
                this.Save();
            }
        }
    };
    AddEditAddressComponent.prototype.Validate = function () {
        var isValid = true;
        var errors = [];
        if (this.EntityPM != null) {
            var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
            var isLanguageValid = AddressValidator_1.AddressValidator.IsMainAddressEnglishCharacters(this.EntityPM);
            if (!isLanguageValid) {
                errors.push("Main address does not allow non-english characters");
            }
            if (this.EntityPM.AddressTypeId == "O") {
                if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Description)) {
                    errors.push(msg.replace("%FieldName", "Description"));
                }
            }
            if (this.DataContext.Country != null) {
                if (this.DataContext.State == null) {
                    if (this.DataContext.Country.IsStateRequired) {
                        errors.push(msg.replace("%FieldName", "State"));
                    }
                }
            }
            if (this.DataContext.fatherComponent.Customer != null) {
                if (this.DataContext.fatherComponent.Customer.IsCustomer) {
                    if (this.DataContext.fatherComponent.Customer.PartnerTypeId == "CS") {
                        if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerTelRequired) {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PhoneNumber)) {
                                errors.push("Phone Number is required");
                            }
                        }
                        if (SessionLocator_1.SessionLocator.TenantPM.IsCustomerFaxRequired) {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.FaxNumber)) {
                                errors.push("Fax Number is required");
                            }
                        }
                    }
                    else if (this.DataContext.fatherComponent.Customer.PartnerTypeId == "PO") {
                        if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialTelRequired) {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.PhoneNumber)) {
                                errors.push("Phone Number is required");
                            }
                        }
                        if (SessionLocator_1.SessionLocator.TenantPM.IsPotentialFaxRequired) {
                            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.FaxNumber)) {
                                errors.push("Fax Number is required");
                            }
                        }
                    }
                }
            }
        }
        isValid = errors.length == 0 ? true : false;
        this.ValidationErrorsList = errors;
        return isValid;
    };
    AddEditAddressComponent.prototype.Save = function () {
        var _this = this;
        var args = new PartnersDomainService_1.PartnerServicePM();
        args.Tenant = this.EntityPM.Tenant;
        args.AddressId = this.EntityPM.Id;
        args.PartnerId = this.EntityPM.CardId;
        args.Address = this.EntityPM;
        args.IsAddressDirty = this.EntityPM.IsDirty;
        if (this.DataContext.fatherComponent) {
            args.IsPartnerDirty = this.DataContext.fatherComponent.EntityPM.IsDirty;
            args.PartnerTypeId = this.DataContext.fatherComponent.PartnerTypeId;
        }
        this.DomainService.SetPartner(args, this.DataContext.fatherComponent.EntityPM);
        this.DomainService.PostPartnerAddress(args).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.DataContext.EntityPM = myResponse.Result.Address;
                if (!_this.LoadCompletedEvent) {
                    _this.LoadCompletedEvent = _this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isSuccess) {
                        Tools_1.AppTool.KillEventEmitter(_this.LoadCompletedEvent);
                        _this.LoadCompletedEvent = null;
                        if (isSuccess == false) {
                            _this.CurrentSession.StopBusyIndicator();
                        }
                        else {
                            if (_this.DataContext.IsNewEntity) {
                                _this.DataContext.fatherComponent.DomainService.GetAllAddressesPMsbyCardId(_this.EntityPM.CardId).subscribe(function (myResult) {
                                    _this.DataContext.fatherComponent.AllAddresses = myResult;
                                    _this.DataContext.fatherComponent.BuildItemsSource();
                                    _this.CurrentSession.CloseCurrentWindow();
                                });
                            }
                            else {
                                _this.DataContext.fatherComponent.BuildItemsSource();
                                _this.CurrentSession.CloseCurrentWindow();
                            }
                        }
                    });
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            }
        });
    };
    AddEditAddressComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Address1');
        this.myCloner.AddField('Address2');
        this.myCloner.AddField('City');
        this.myCloner.AddField('StateId');
        this.myCloner.AddField('CountryId');
        this.myCloner.AddField('ZipCode');
        this.myCloner.AddField('PhoneNumber');
        this.myCloner.AddField('FaxNumber');
        this.myCloner.AddField('ATTN');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AddEditAddressComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditAddressComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditAddressComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditAddressComponent);
    return AddEditAddressComponent;
}());
exports.AddEditAddressComponent = AddEditAddressComponent;
//# sourceMappingURL=AddEditAddressComponent.js.map