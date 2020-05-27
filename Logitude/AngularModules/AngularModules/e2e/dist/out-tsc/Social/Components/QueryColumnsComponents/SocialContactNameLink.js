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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var SocialContactNameLink = /** @class */ (function () {
    function SocialContactNameLink(cd) {
        this.cd = cd;
        this.UserName = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    SocialContactNameLink.prototype.ngOnInit = function () {
    };
    SocialContactNameLink.prototype.setVariables = function (rowData, fieldName) {
        if (rowData != null) {
            this.rowData = rowData;
            this.UserName = rowData.EnglishName;
            this.Destroyed();
        }
    };
    SocialContactNameLink.prototype.ViewPostUserFeedsButtonClick = function (item) {
        //this.IsChecked = !this.IsChecked;
        var data = [];
        data.push("SocialContactLinkEvent");
        data.push(item);
        this.Destroyed();
        this.CurrentSession.SessionEvent.emit(data);
    };
    SocialContactNameLink.prototype.Destroyed = function () {
        var isDestroyed = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    };
    SocialContactNameLink = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialContactNameLink',
            templateUrl: './SocialContactNameLink.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], SocialContactNameLink);
    return SocialContactNameLink;
}());
exports.SocialContactNameLink = SocialContactNameLink;
//# sourceMappingURL=SocialContactNameLink.js.map