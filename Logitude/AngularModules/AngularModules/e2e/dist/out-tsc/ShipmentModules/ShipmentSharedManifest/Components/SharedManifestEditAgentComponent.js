"use strict";
/// <reference path="sharedmanifeststarted.ts" />
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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var SharedAgentManifestService_1 = require("../../../Shipment/Services/Others/SharedAgentManifestService");
var SharedManifestEditAgentComponent = /** @class */ (function () {
    function SharedManifestEditAgentComponent(_sharedAgentManifestService) {
        this._sharedAgentManifestService = _sharedAgentManifestService;
        this.ObjectTableName = "Shipment";
        this.isMyCustomer = false;
        this.IsSaveShipment = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.Listen();
    }
    SharedManifestEditAgentComponent.prototype.ngOnInit = function () {
        this.DataContext.SetUIProperties();
    };
    SharedManifestEditAgentComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    if (_this.IsSaveShipment) {
                        _this.Close();
                    }
                    _this.IsSaveShipment = false;
                }
            });
        }
    };
    SharedManifestEditAgentComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.isMyCustomer = dataContext.IsCustomer;
        this.Clone();
        this.IsAgentSharedManifests(dataContext);
    };
    SharedManifestEditAgentComponent.prototype.IsAgentSharedManifests = function (partnerItem) {
        var _this = this;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading...");
        this._sharedAgentManifestService.GetIsAgentSharedManifests(partnerItem.AgentId, partnerItem.EntityPM.Id).subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            if (!myResponse.HasError) {
                partnerItem.IsDisableNextButton = myResponse.Result ? true : false;
                ;
            }
        });
    };
    SharedManifestEditAgentComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    SharedManifestEditAgentComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.DataContext.PartnerId == null) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.Name")));
        }
        if (this.DataContext.Reference1 != null) {
            if (this.DataContext.Reference1.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.Reference1") + " max length is 50");
            }
        }
        if (this.DataContext.Reference2 != null) {
            if (this.DataContext.Reference2.length > 50) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.S.Partners.Reference2") + " max length is 50");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("Saving...");
            this.DataContext.fatherComponent._agentSharedLogisticsKeyPMService.GetSingleByAgentId(this.EntityPM.AgentId).subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (!myResponse.HasError) {
                    var agentSharedKey = myResponse.Result;
                    if (agentSharedKey) {
                        switch (agentSharedKey.StatusCode) {
                            case "A":
                                if (_this.EntityPM.ShipmentLevelCode == "C") {
                                    _this._sharedAgentManifestService.GetCheckIfMasterShipmentHaveHouseWithOtherAgent(_this.EntityPM.Id, _this.EntityPM.AgentId, _this.EntityPM.Tenant).subscribe(function (myResponse) {
                                        _this.CurrentSession.StopBusyIndicator();
                                        if (!myResponse.HasError) {
                                            if (myResponse.Result == true) {
                                                _this.ValidationErrorsList.push("One of the houses has agent different from the master shipment");
                                            }
                                            else {
                                                _this.CompleteSave();
                                            }
                                        }
                                    });
                                }
                                else {
                                    _this.CompleteSave();
                                }
                                break;
                            case "W":
                                _this.ValidationErrorsList.push("Waiting for the Agent’s approval to enable sharing");
                                break;
                            case "I":
                                _this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                                break;
                        }
                    }
                    else {
                        _this.ValidationErrorsList.push("Please connect with the agent from the agent’s shared logistics tab");
                    }
                }
                else {
                    _this.ValidationErrorsList.push(myResponse.ErrorsArray[0]);
                }
            });
        }
    };
    SharedManifestEditAgentComponent.prototype.CompleteSave = function () {
        if (this.ValidationErrorsList.length == 0) {
            if (this.isMyCustomer) {
                if (this.EntityPM.ShipmentLevelCode == "C") {
                    if (this.EntityPM.CustomerId != null) {
                        this.EntityPM.CustomerId = null;
                    }
                    if (this.EntityPM.CustomerName != null) {
                        this.EntityPM.CustomerName = null;
                    }
                    if (this.EntityPM.CustomerReference1 != null) {
                        this.EntityPM.CustomerReference1 = null;
                    }
                    if (this.EntityPM.CustomerReference2 != null) {
                        this.EntityPM.CustomerReference2 = null;
                    }
                    if (this.EntityPM.CustomerAddressId != null) {
                        this.EntityPM.CustomerAddressId = null;
                    }
                    if (this.EntityPM.CustomerContactId != null) {
                        this.EntityPM.CustomerContactId = null;
                    }
                    if (this.EntityPM.ShipmentCustomerTypeCode != null) {
                        this.EntityPM.ShipmentCustomerTypeCode = null;
                    }
                }
                else {
                    if (this.EntityPM.CustomerId != this.DataContext.PartnerId) {
                        this.EntityPM.CustomerId = this.DataContext.PartnerId;
                    }
                    if (this.EntityPM.CustomerName != this.DataContext.Name) {
                        this.EntityPM.CustomerName = this.DataContext.Name;
                    }
                    if (this.EntityPM.CustomerAddressId != this.DataContext.AddressId) {
                        this.EntityPM.CustomerAddressId = this.DataContext.AddressId;
                    }
                    if (this.EntityPM.CustomerContactId != this.DataContext.ContactId) {
                        this.EntityPM.CustomerContactId = this.DataContext.ContactId;
                    }
                    if (this.EntityPM.CustomerReference1 != this.DataContext.Reference1) {
                        this.EntityPM.CustomerReference1 = this.DataContext.Reference1;
                    }
                    if (this.EntityPM.CustomerReference2 != this.DataContext.Reference2) {
                        this.EntityPM.CustomerReference2 = this.DataContext.Reference2;
                    }
                }
            }
            if (this.EntityPM.IsDirty) {
                this.IsSaveShipment = true;
                this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else
                this.Close();
        }
    };
    SharedManifestEditAgentComponent.prototype.Close = function () {
        this.CurrentSession.CloseCurrentWindowEmit("OK");
        this.CurrentSession.FireEvent("ShipmentPartnersChanged");
    };
    SharedManifestEditAgentComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('PartnerId');
        this.myCloner.AddField('AddressId');
        this.myCloner.AddField('ContactId');
        this.myCloner.AddField('Reference1');
        this.myCloner.AddField('Reference2');
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Note');
        this.myCloner.AddEntity(this.EntityPM);
    };
    SharedManifestEditAgentComponent.prototype.RejectChanges = function () {
        this.DataContext.IsReseting = true;
        this.myCloner.RejectChanges();
        this.DataContext.IsReseting = false;
    };
    SharedManifestEditAgentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedManifestEditAgentComponent',
            templateUrl: './SharedManifestEditAgentComponent.html',
            providers: [SharedAgentManifestService_1.SharedAgentManifestService],
        }),
        __metadata("design:paramtypes", [SharedAgentManifestService_1.SharedAgentManifestService])
    ], SharedManifestEditAgentComponent);
    return SharedManifestEditAgentComponent;
}());
exports.SharedManifestEditAgentComponent = SharedManifestEditAgentComponent;
//# sourceMappingURL=SharedManifestEditAgentComponent.js.map