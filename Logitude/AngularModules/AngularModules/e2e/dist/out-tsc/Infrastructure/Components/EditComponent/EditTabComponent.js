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
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var EditTabComponent = /** @class */ (function () {
    function EditTabComponent(ChangeDetectorRef) {
        this.ChangeDetectorRef = ChangeDetectorRef;
        this.isLoaderReady = false;
        this.CurrentlySelected = false;
        this.Retries = 0;
        this.selected = false;
    }
    EditTabComponent.prototype.Run = function (tabCode, componentPath) {
        this.TabCode = tabCode;
        this.ComponentPath = componentPath;
        this.RunComponent();
    };
    EditTabComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.ViewContainerRef) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load(this.ComponentPath, this.ViewContainerRef)
                .then(function (cmpRef) {
                _this.Selected = true;
                _this.ComponentInst = cmpRef.instance;
                _this.ComponentRef = cmpRef;
            });
        }
        else {
            this.RunComponentTimer();
        }
    };
    EditTabComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(EditTabComponent.prototype, "Selected", {
        get: function () { return this.selected; },
        set: function (value) {
            var _this = this;
            if (this.selected != value) {
                this.selected = value;
                setTimeout(function () { return _this.RunChangeDetector(); }, 1);
            }
        },
        enumerable: true,
        configurable: true
    });
    EditTabComponent.prototype.RunChangeDetector = function () {
        if (this.Selected == true) {
            var d = this.TabCode;
            this.ChangeDetectorRef.reattach();
        }
        else {
            var d = this.TabCode;
            this.ChangeDetectorRef.detach();
        }
    };
    EditTabComponent.prototype.DestroyCurrentTab = function () {
        //this.ComponentInst.ngOnDestroy();
        this.ComponentRef.destroy();
        this.ComponentRef = null;
    };
    __decorate([
        core_1.ViewChild('Child', { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], EditTabComponent.prototype, "ViewContainerRef", void 0);
    EditTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditTabComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], EditTabComponent);
    return EditTabComponent;
}());
exports.EditTabComponent = EditTabComponent;
//# sourceMappingURL=EditTabComponent.js.map