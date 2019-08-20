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
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var EditFeaturesPackageLinkComponent = /** @class */ (function () {
    function EditFeaturesPackageLinkComponent() {
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.AllCloners = [];
    }
    EditFeaturesPackageLinkComponent.prototype.SetWindowArgs = function (args) {
        this.ItemsSource = args['Items'];
        this.Clone();
    };
    EditFeaturesPackageLinkComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    EditFeaturesPackageLinkComponent.prototype.OkButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindowEmit("Ok");
    };
    EditFeaturesPackageLinkComponent.prototype.Clone = function () {
        var _this = this;
        this.ItemsSource.forEach(function (item) {
            var myCloner = new Cloner_1.Cloner(item);
            myCloner.AddField('IsActive');
            myCloner.AddEntity(item.Feature);
            _this.AllCloners.push(myCloner);
        });
    };
    EditFeaturesPackageLinkComponent.prototype.RejectChanges = function () {
        this.AllCloners.forEach(function (myCloner) {
            myCloner.RejectChanges();
        });
    };
    EditFeaturesPackageLinkComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditFeaturesPackageLinkComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditFeaturesPackageLinkComponent);
    return EditFeaturesPackageLinkComponent;
}());
exports.EditFeaturesPackageLinkComponent = EditFeaturesPackageLinkComponent;
//# sourceMappingURL=EditFeaturesPackageLinkComponent.js.map