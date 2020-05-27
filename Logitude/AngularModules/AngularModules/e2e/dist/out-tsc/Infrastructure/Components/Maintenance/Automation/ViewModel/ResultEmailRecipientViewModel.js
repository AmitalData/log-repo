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
var Guid_1 = require("../../../../../Infrastructure/Utilities/Guid");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ResultEmailRecipientViewModel = /** @class */ (function (_super) {
    __extends(ResultEmailRecipientViewModel, _super);
    function ResultEmailRecipientViewModel(entityPM, addEditAutomationsComponent) {
        var _this = _super.call(this) || this;
        _this.FieldName = entityPM.FieldName;
        _this.Id = entityPM.Id;
        _this.Tenant = entityPM.Tenant;
        _this.FullName = entityPM.FullNameTextCodeDefaultText;
        _this.Key = Guid_1.Guid.newGuid();
        _this.EntityContactVariable = addEditAutomationsComponent.EntityContactVariable;
        _this.AddEditAutomationsComponent = addEditAutomationsComponent;
        if (_this.AddEditAutomationsComponent.EntityContactVariable && _this.AddEditAutomationsComponent.EntityContactVariable.length > 0 && _this.AddEditAutomationsComponent.EntityContactVariable.indexOf(entityPM.Id) != -1) {
            _this.IsChecked = true;
        }
        else
            _this.IsChecked = false;
        return _this;
    }
    ResultEmailRecipientViewModel.prototype.ngOnInit = function () {
    };
    ResultEmailRecipientViewModel.prototype.CheckedResultEmailRecipient = function (item) {
        if (this.AddEditAutomationsComponent.EntityContactVariable.indexOf(item.Id) == -1) {
            this.AddEditAutomationsComponent.EntityContactVariable.push(item.Id);
        }
        else {
            this.AddEditAutomationsComponent.EntityContactVariable = this.AddEditAutomationsComponent.EntityContactVariable.filter(function (d) { return d != item.Id; });
        }
        this.AddEditAutomationsComponent.IsChangeAutomation = true;
    };
    return ResultEmailRecipientViewModel;
}(BaseComponent_1.BaseComponent));
exports.ResultEmailRecipientViewModel = ResultEmailRecipientViewModel;
var Operator = /** @class */ (function () {
    function Operator(name, code) {
        this.Code = code;
        this.Name = name;
    }
    return Operator;
}());
//# sourceMappingURL=ResultEmailRecipientViewModel.js.map