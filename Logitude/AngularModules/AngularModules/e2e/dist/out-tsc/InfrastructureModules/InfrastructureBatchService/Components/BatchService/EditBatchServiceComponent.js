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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var BatchServicesDefinitionPMService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/BatchServicesDefinitionPMService");
var EditBatchServiceComponent = /** @class */ (function (_super) {
    __extends(EditBatchServiceComponent, _super);
    function EditBatchServiceComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "BatchServicesDefinition";
        _this.ValidationErrorsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    EditBatchServiceComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.Clone();
    };
    EditBatchServiceComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    EditBatchServiceComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.DataContext.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            var service = new BatchServicesDefinitionPMService_1.BatchServicesDefinitionPMService();
            service.update(this.EntityPM).subscribe(function (myResult) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindow();
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    };
    EditBatchServiceComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
        this.myCloner.AddField('NumberOfThreads');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.EntityPM);
    };
    EditBatchServiceComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    EditBatchServiceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditBatchServiceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditBatchServiceComponent);
    return EditBatchServiceComponent;
}(BaseComponent_1.BaseComponent));
exports.EditBatchServiceComponent = EditBatchServiceComponent;
//# sourceMappingURL=EditBatchServiceComponent.js.map