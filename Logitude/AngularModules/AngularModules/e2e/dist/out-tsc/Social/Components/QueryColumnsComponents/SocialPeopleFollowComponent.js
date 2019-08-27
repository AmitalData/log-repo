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
var FollowerExtendedPMService_1 = require("../../Services/ExtendedPMs/FollowerExtendedPMService");
var SocialPeopleFollowComponent = /** @class */ (function () {
    function SocialPeopleFollowComponent(cd) {
        this.cd = cd;
        this.IsFollowed = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.followerExtendedPMService = new FollowerExtendedPMService_1.FollowerExtendedPMService();
    }
    SocialPeopleFollowComponent.prototype.ngOnInit = function () {
    };
    SocialPeopleFollowComponent.prototype.setVariables = function (rowData, fieldName) {
        if (rowData != null) {
            this.rowData = rowData;
            //this.IsFollowed = rowData.IsFollowed;
            this.Destroyed();
        }
    };
    SocialPeopleFollowComponent.prototype.FollowingButtonClick = function (user) {
        this.AddDeleteFollower(user);
    };
    SocialPeopleFollowComponent.prototype.AddDeleteFollower = function (user) {
        var _this = this;
        var isdelete = user.IsFollowed;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.followerExtendedPMService.AddDeleteFollower(user.Id, SessionLocator_1.SessionLocator.LoggedUserId, isdelete, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                _this.rowData.IsFollowed = !isdelete;
                _this.Destroyed();
            }
        });
    };
    SocialPeopleFollowComponent.prototype.Destroyed = function () {
        var isDestroyed = this.cd['destroyed'];
        if (!isDestroyed) {
            this.cd.detectChanges();
        }
    };
    SocialPeopleFollowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialPeopleFollowComponent',
            templateUrl: './SocialPeopleFollowComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], SocialPeopleFollowComponent);
    return SocialPeopleFollowComponent;
}());
exports.SocialPeopleFollowComponent = SocialPeopleFollowComponent;
//# sourceMappingURL=SocialPeopleFollowComponent.js.map