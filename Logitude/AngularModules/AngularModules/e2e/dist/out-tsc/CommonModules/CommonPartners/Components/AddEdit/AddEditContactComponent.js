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
var ContactInputTemplate_1 = require("../../../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate");
var Tools_1 = require("../../../../Infrastructure/Tools");
var PartnersDomainService_1 = require("../../../../Common/Services/PartnersDomainService");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AddEditContactComponent = /** @class */ (function () {
    function AddEditContactComponent() {
        this.CardId = null;
        this.EntityPM = null;
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Retries = 0;
        this.ShowSecondPartOfWindow = null;
        this.LoadCompletedEvent = null;
    }
    AddEditContactComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ObjectTableName = dataContext.ObjectTableName;
        if (dataContext.fatherComponent) {
            this.CardId = dataContext.fatherComponent.EntityId;
            this.DomainService = dataContext.fatherComponent.DomainService;
        }
        else {
            this.CardId = dataContext.EntityPM.CardId;
            this.DomainService = new PartnersDomainService_1.PartnersDomainService();
        }
        this.RunComponent();
        this.Clone();
    };
    AddEditContactComponent.prototype.SetWindowArgs = function (args) {
        if (args) {
            this.ShowSecondPartOfWindow = args.ShowSecondPartOfWindow;
        }
    };
    AddEditContactComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    AddEditContactComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    AddEditContactComponent.prototype.LoadChildComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/ContactInputTemplate", this.viewContainerRef)
            .then(function (cmpRef) {
            _this.ContactTemplate = cmpRef.instance;
            var args = new ContactInputTemplate_1.ContactInputTemplateArgs();
            args.CardId = _this.CardId;
            args.ShowSearchContacts = true;
            args.IsNewEntity = _this.DataContext.IsNewEntity;
            args.EntityPM = _this.EntityPM;
            args.ShowSecondPartOfWindow = _this.ShowSecondPartOfWindow;
            var isBlockingEmail = false;
            if (!_this.DataContext.IsNewEntity) {
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.Email)) {
                    isBlockingEmail = true;
                }
            }
            args.BlockEditingEmail = isBlockingEmail;
            _this.ContactTemplate.InitTemplate(args);
            _this.Clone();
        });
    };
    AddEditContactComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    };
    AddEditContactComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.StartBusyIndicatorSaving();
        this.ValidationErrorsList = this.ContactTemplate.Validate();
        if (this.ValidationErrorsList.length > 0) {
            this.CurrentSession.StopBusyIndicator();
        }
        else {
            if (!this.EntityPM.IsDirty) {
                this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
            }
            else {
                if (this.DataContext.IsNewEntity) {
                    if (this.DataContext.fatherComponent && this.DataContext.fatherComponent.ItemsSource.length == 0) {
                        this.DataContext.fatherComponent.EntityPM.IsFirstContactToAdd = true;
                        this.DataContext.fatherComponent.EntityPM.PrimaryContactId = this.EntityPM.Id;
                    }
                }
                this.Save();
            }
        }
    };
    AddEditContactComponent.prototype.Save = function () {
        var _this = this;
        var args = new PartnersDomainService_1.PartnerServicePM();
        args.Tenant = this.EntityPM.Tenant;
        args.ContactId = this.EntityPM.Id;
        args.PartnerId = this.EntityPM.CardId;
        args.Contact = this.EntityPM;
        args.IsContactDirty = this.EntityPM.IsDirty;
        if (this.DataContext.fatherComponent) {
            args.IsPartnerDirty = this.DataContext.fatherComponent.EntityPM.IsDirty;
            args.PartnerTypeId = this.DataContext.fatherComponent.PartnerTypeId;
            this.DomainService.SetPartner(args, this.DataContext.fatherComponent.EntityPM);
        }
        else {
            this.DomainService.SetPartner(args, this.DataContext.EntityPM, "CO");
        }
        this.DomainService.PostPartnerAddress(args).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.CurrentSession.StopBusyIndicator();
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                _this.DataContext.EntityPM = myResponse.Result.Contact;
                if (!_this.CurrentSession.CurrentEditComponent) {
                    if (_this.DataContext.IsNewEntity) {
                        _this.DataContext.IsNewEntity = false;
                    }
                    _this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    if (!_this.LoadCompletedEvent) {
                        _this.LoadCompletedEvent = _this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isSuccess) {
                            Tools_1.AppTool.KillEventEmitter(_this.LoadCompletedEvent);
                            _this.LoadCompletedEvent = null;
                            if (isSuccess == false) {
                                _this.CurrentSession.StopBusyIndicator();
                            }
                            else {
                                if (_this.DataContext.IsNewEntity) {
                                    _this.DataContext.IsNewEntity = false;
                                    _this.DataContext.fatherComponent.IsNoDataVisible = false;
                                    _this.DataContext.fatherComponent.ItemsSource.push(_this.DataContext);
                                }
                                _this.CurrentSession.CloseCurrentWindowEmit(_this.EntityPM.Id);
                            }
                        });
                        _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }
                }
            }
        });
    };
    AddEditContactComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.ContactTemplate);
        this.myCloner.AddField('Email');
        this.myCloner.AddField('EnglishName');
        this.myCloner.AddField('LocalName');
        this.myCloner.AddField('BusinessPhone');
        this.myCloner.AddField('Mobile');
        this.myCloner.AddField('Fax');
        this.myCloner.AddField('Anniversary');
        this.myCloner.AddField('Birthday');
        this.myCloner.AddField('InActive');
        this.myCloner.AddField('Position');
        this.myCloner.AddField('IsAll');
        this.myCloner.AddField('IsAirExport');
        this.myCloner.AddField('IsAirImport');
        this.myCloner.AddField('IsOceanExport');
        this.myCloner.AddField('IsOceanImport');
        this.myCloner.AddField('IsCustomsImport');
        this.myCloner.AddField('IsInlandDomestic');
        this.myCloner.AddEntity(this.EntityPM);
    };
    AddEditContactComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], AddEditContactComponent.prototype, "viewContainerRef", void 0);
    AddEditContactComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditContactComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditContactComponent);
    return AddEditContactComponent;
}());
exports.AddEditContactComponent = AddEditContactComponent;
//# sourceMappingURL=AddEditContactComponent.js.map