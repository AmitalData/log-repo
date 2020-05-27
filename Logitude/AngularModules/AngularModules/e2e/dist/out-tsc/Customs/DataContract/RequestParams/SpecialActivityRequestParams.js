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
Object.defineProperty(exports, "__esModule", { value: true });
var GenericRequestParams_1 = require("./GenericRequestParams");
var SpecialActivityRequestParams = /** @class */ (function (_super) {
    __extends(SpecialActivityRequestParams, _super);
    function SpecialActivityRequestParams() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return SpecialActivityRequestParams;
}(GenericRequestParams_1.GenericRequestParams));
exports.SpecialActivityRequestParams = SpecialActivityRequestParams;
var GeneralDetails = /** @class */ (function () {
    function GeneralDetails() {
    }
    return GeneralDetails;
}());
exports.GeneralDetails = GeneralDetails;
var CargoIdentifier = /** @class */ (function () {
    function CargoIdentifier() {
    }
    return CargoIdentifier;
}());
exports.CargoIdentifier = CargoIdentifier;
var GoodsDetails = /** @class */ (function () {
    function GoodsDetails() {
    }
    ;
    return GoodsDetails;
}());
exports.GoodsDetails = GoodsDetails;
var RepresentativeDetails = /** @class */ (function () {
    function RepresentativeDetails() {
    }
    return RepresentativeDetails;
}());
exports.RepresentativeDetails = RepresentativeDetails;
var SampleRequestDetails = /** @class */ (function () {
    function SampleRequestDetails() {
    }
    return SampleRequestDetails;
}());
exports.SampleRequestDetails = SampleRequestDetails;
var RePackingApprovalDetails = /** @class */ (function () {
    function RePackingApprovalDetails() {
    }
    return RePackingApprovalDetails;
}());
exports.RePackingApprovalDetails = RePackingApprovalDetails;
var CurrentPackingDetails = /** @class */ (function () {
    function CurrentPackingDetails() {
    }
    return CurrentPackingDetails;
}());
exports.CurrentPackingDetails = CurrentPackingDetails;
var DesiredPackingDetails = /** @class */ (function () {
    function DesiredPackingDetails() {
    }
    return DesiredPackingDetails;
}());
exports.DesiredPackingDetails = DesiredPackingDetails;
var PackingDetails = /** @class */ (function () {
    function PackingDetails() {
    }
    return PackingDetails;
}());
exports.PackingDetails = PackingDetails;
//# sourceMappingURL=SpecialActivityRequestParams.js.map