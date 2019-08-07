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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var core_1 = require("@angular/core");
var PostsArgs_1 = require("../../../Infrastructure/DataContracts/PostsArgs");
var SocialComponent = /** @class */ (function () {
    function SocialComponent() {
        this.QueryName = "";
        this.SubQueryName = "";
        this.EntityId = "";
        this.ObjectTableName = "";
        this.ScreenCode = "";
        this.EntityDescription = "";
        this.IsEntityMode = false;
        this.RegardingEntity = "";
        this.ObjectTableId = "";
        this.InsideEntity = false;
        this.Retries = 0;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SOCIALGENERAL")) {
            this.RunComponent();
        }
    }
    SocialComponent.prototype.ngOnInit = function () {
    };
    SocialComponent.prototype.RunComponent = function () {
        if (this.viewContainerRef) {
            this.LoadSocialComponent();
        }
        else {
            this.RunComponentTimer();
        }
    };
    SocialComponent.prototype.LoadSocialComponent = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Social/Components/SocialMainComponent', this.viewContainerRef)
            .then(function (cmpRef) {
            _this.SocialPeopleComponent = cmpRef.instance;
            var table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
            if (table) {
                _this.ObjectTableId = table.Id;
            }
            var postsArgs = new PostsArgs_1.PostsArgs();
            postsArgs.QueryName = _this.QueryName;
            postsArgs.SubQueryName = _this.SubQueryName;
            postsArgs.UserId = SessionLocator_1.SessionLocator.LoggedUserId;
            postsArgs.ScreenCode = _this.ScreenCode;
            postsArgs.InsideEntity = _this.InsideEntity;
            postsArgs.EntityId = _this.EntityId;
            postsArgs.ObjectTableId = _this.ObjectTableId;
            postsArgs.EntityDescription = _this.EntityDescription;
            postsArgs.RegardingEntity = _this.RegardingEntity;
            postsArgs.IsEntityMode = _this.IsEntityMode;
            postsArgs.HideLeftArea = true;
            _this.SocialPeopleComponent.InitializeSocialMainComponent(postsArgs);
        });
    };
    SocialComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 100) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], SocialComponent.prototype, "viewContainerRef", void 0);
    SocialComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SocialComponent',
            templateUrl: './SocialComponent.html',
            inputs: ['QueryName', 'SubQueryName', 'EntityId', 'ObjectTableName', 'ScreenCode', 'EntityDescription', 'IsEntityMode', 'InsideEntity', 'RegardingEntity'],
        }),
        __metadata("design:paramtypes", [])
    ], SocialComponent);
    return SocialComponent;
}());
exports.SocialComponent = SocialComponent;
//# sourceMappingURL=SocialComponent.js.map