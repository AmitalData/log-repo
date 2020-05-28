"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var StateListService_1 = require("../../Common/Services/StandardLists/StateListService");
var CountryListService_1 = require("../../Common/Services/StandardLists/CountryListService");
var PortPMCustomCode = /** @class */ (function () {
    function PortPMCustomCode() {
    }
    PortPMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        var _this = this;
        this.entityPM = entityPM;
        if (propertyName == "CountryId" && entityPM.CountryId) {
            var countryService = new CountryListService_1.CountryListService();
            countryService.getSingleFromCache(entityPM.CountryId).subscribe(function (response) {
                if (response.Result) {
                    _this.Country = response.Result;
                    _this.OnCountryChanged(_this.Country);
                }
            });
        }
        if (propertyName == "StateId") {
            if (entityPM.StateId) {
                var stateService = new StateListService_1.StateListService();
                stateService.getSingleFromCache(entityPM.StateId).subscribe(function (response) {
                    if (response.Result) {
                        _this.State = response.Result;
                        _this.OnStateChanged(_this.State);
                    }
                });
            }
            else {
                if (this.Country != null) {
                    if (this.Country.IsStateRequired) {
                        this.entityPM.UIProperties.SetRequired("StateId", this.ObjectTableName, true);
                    }
                }
            }
        }
    };
    PortPMCustomCode.SetUIProperties_State = function () {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    };
    PortPMCustomCode.SetUIProperties_StateEnabled = function () {
        var isEnabled = false;
        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }
        this.entityPM.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    };
    PortPMCustomCode.SetUIProperties_StateRequired = function () {
        var isRequired = false;
        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }
        this.entityPM.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    };
    PortPMCustomCode.OnCountryChanged = function (list) {
        if (list == null) {
            this.entityPM.CountryCode = null;
            this.entityPM.CountryName = null;
        }
        else {
            this.entityPM.CountryCode = list.Code;
            this.entityPM.CountryName = list.EnglishName;
        }
        this.SetUIProperties_State();
    };
    PortPMCustomCode.OnStateChanged = function (list) {
        if (list == null) {
            this.entityPM.StateCode = null;
        }
        else {
            this.entityPM.StateCode = list.Code;
        }
        this.SetUIProperties_StateRequired();
    };
    PortPMCustomCode.ObjectTableName = "Port";
    return PortPMCustomCode;
}());
exports.PortPMCustomCode = PortPMCustomCode;
//# sourceMappingURL=PortPMCustomCode.js.map