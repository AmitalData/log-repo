"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var InfrastructureDomainService_1 = require("./InfrastructureDomainService");
var Services = /** @class */ (function () {
    function Services() {
    }
    Services.BuildServices = function () {
        this.InfrastructureDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
    };
    return Services;
}());
exports.Services = Services;
//# sourceMappingURL=Services.js.map