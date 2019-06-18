"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CountryListService_1 = require("../Services/StandardLists/CountryListService");
var PortValidator = (function () {
    function PortValidator() {
    }
    PortValidator.prototype.Validate = function (entityPM) {
        var errors = [];
        var countryService = new CountryListService_1.CountryListService();
        countryService.getSingleFromCache(entityPM.CountryId).subscribe(function (response) {
            if (response.Result) {
                var country = response.Result;
                if (country != null) {
                    if (entityPM.StateId == null) {
                        if (country.IsStateRequired && country.HasStates) {
                            errors.push("State Field is Required");
                        }
                    }
                }
            }
        });
        return errors;
    };
    return PortValidator;
}());
exports.PortValidator = PortValidator;
//# sourceMappingURL=PortValidator.js.map