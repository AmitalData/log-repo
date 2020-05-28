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
var ModulesService_1 = require("../../../Infrastructure/Services/ModulesService");
var EntityFollowComponent = /** @class */ (function () {
    function EntityFollowComponent() {
        this.IsFollowed = false;
        this.EntityId = "";
        this.ObjectTableName = "";
        this.FollowEntityLists = [];
        this.ObjectTableId = "";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ToolTipMessage = "";
        this.myModulesService = new ModulesService_1.ModulesService();
    }
    EntityFollowComponent.prototype.ngOnInit = function () {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
        if (table) {
            this.ObjectTableId = table.Id;
        }
        this.LoadEntityFollowers();
    };
    EntityFollowComponent.prototype.LoadEntityFollowers = function () {
        var _this = this;
        this.FollowEntityLists = [];
        this.ToolTipMessage = "";
        // this.CurrentSession.StartBusyIndicatorSaving();
        this.myModulesService.GetUserFollowEntityLists(this.EntityId, this.ObjectTableId).subscribe(function (res) {
            var pmResponse = res;
            //this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError && pmResponse.Result) {
                pmResponse.Result.forEach(function (item) {
                    _this.FollowEntityLists.push(item);
                    _this.ToolTipMessage += item.FollowerName + " \n";
                });
                _this.FollowersCount = _this.FollowEntityLists.length;
                var followEntityList = _this.FollowEntityLists.filter(function (f) { return f.FollowerUserId == SessionLocator_1.SessionLocator.LoggedUserId; })[0];
                if (followEntityList)
                    _this.IsFollowed = true;
                else
                    _this.IsFollowed = false;
            }
        });
    };
    EntityFollowComponent.prototype.AddFollowEntity = function (user) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myModulesService.AddFollowEntity(this.EntityId, this.ObjectTableId, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                _this.IsFollowed = true;
                _this.FollowersCount += 1;
                _this.LoadEntityFollowers();
            }
        });
    };
    EntityFollowComponent.prototype.DeleteFollowEntity = function (user) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myModulesService.DeleteFollowEntity(SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                _this.IsFollowed = false;
                _this.FollowersCount -= 1;
                _this.LoadEntityFollowers();
            }
        });
    };
    EntityFollowComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EntityFollowComponent',
            templateUrl: './EntityFollowComponent.html',
            inputs: ['EntityId', 'ObjectTableName'],
        }),
        __metadata("design:paramtypes", [])
    ], EntityFollowComponent);
    return EntityFollowComponent;
}());
exports.EntityFollowComponent = EntityFollowComponent;
//# sourceMappingURL=EntityFollowComponent.js.map