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
var TipsVisibilityPM_1 = require("../../../../Infrastructure/EntityPMs/TipsVisibilityPM");
var TipsVisibilityService_1 = require("../../../../Infrastructure/Services/ExtendedPMs/TipsVisibilityService");
var Guid_1 = require("../../../../Infrastructure/Utilities/Guid");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var TipsComponent = /** @class */ (function () {
    function TipsComponent(CD, _tipsVisibilityService) {
        this.CD = CD;
        this._tipsVisibilityService = _tipsVisibilityService;
        this.TipVisibilityChangedEvent = new core_1.EventEmitter();
        this.ShowTipEvent = new core_1.EventEmitter();
        //ShowAreaClick() {
        //    //this.TipIsVisible = true;
        //    this.TipMessage = this.Tip.ShortTextCodeCode;
        //    this.TipVisibility = window.TipsVisibilities.filter(d=> d.TipCode == this.Tip.Code && d.UserId == SessionInfo.LoggedUserId)[0];
        //    if (this.TipVisibility) {
        //        this.DontShow = !this.TipVisibility.IsVisible;
        //    }
        //    else {
        //        this.SaveChanges(true);
        //    }
        //}
        this.IsStartSave = false;
    }
    TipsComponent.prototype.ngOnInit = function () {
        //if (this.ShowTipEvent) {
        //    this.ShowTipEvent.subscribe(($event: any) => {
        var _this = this;
        //        this.ShowAreaClick();
        //    });
        //}
        this.DontShow = false;
        this.DontShowA1gimToolTipAreaCheckBoxId = Guid_1.Guid.newGuid();
        var table = null;
        if (this.IsInternalTips) {
            this.HeightTipsArea = "80px";
            table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName && (d.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || d.Tenant == 0); })[0];
        }
        else {
            this.HeightTipsArea = "120px";
            table = window.ObjectTables.filter(function (d) { return d.Name == _this.ObjectTableName; })[0];
        }
        if (table) {
            if (this.IsInternalTips)
                this.Tip = window.Tips.filter(function (d) { return d.Code == _this.Code && d.ObjectTableId == table.Id; })[0];
            else
                this.Tip = window.Tips.filter(function (d) { return d.Code == table.MainTipCode; })[0];
            if (this.Tip) {
                this.HasTip = true;
                //this.TipMessage = this.Tip.ShortTextCodeCode;
                //if (this.IsInternalTips) {
                var ft = TextCodeTranslator_1.TextCodeTranslator.Translate(this.Tip.ShortTextCodeCode);
                if (ft) {
                    var tipMessage = ft;
                    if (tipMessage.indexOf("<%L>") > -1) {
                        this.TipMessageLineLists = tipMessage.split("<%L>");
                        //   this.TipMessage = tipMessage.replace("<%L>", "<br>");//tipMessage.Replace("(%L)", Environment.NewLine);
                    }
                    else {
                        //  this.TipMessage = tipMessage.replace("(%L)", "<br>");//tipMessage.Replace("(%L)", Environment.NewLine);
                        this.TipMessageLineLists = tipMessage.split("(%L)");
                    }
                }
                else
                    this.TipMessageLineLists = this.Tip.ShortTextCodeCode.split("(%L)"); //this.TipMessage = this.Tip.Code;
                // }
                var isVisible = this.Tip.VisibilityDefaultValue;
                this.TipVisibility = window.TipsVisibilities.filter(function (d) { return d.TipCode == _this.Tip.Code && d.UserId == SessionInfo_1.SessionInfo.LoggedUserId; })[0];
                if (this.TipVisibility)
                    this.DontShow = !this.TipVisibility.IsVisible;
                else {
                    if (!this.IsFirstTipLoad)
                        this.SaveChanges(true);
                }
            }
        }
    };
    TipsComponent.prototype.SaveChanges = function (checkIsTipVisible) {
        var _this = this;
        if (!this.IsStartSave) {
            this.IsStartSave = true;
            this.isTipVisible = checkIsTipVisible;
            var tipVisibility = window.TipsVisibilities.filter(function (d) { return d.TipCode == _this.Tip.Code && d.UserId == SessionInfo_1.SessionInfo.LoggedUserId; })[0];
            if (tipVisibility) {
                tipVisibility.IsVisible = this.isTipVisible;
                window.TipsVisibilities = window.TipsVisibilities.filter(function (d) { return d.TipCode != _this.Tip.Code && d.UserId != SessionInfo_1.SessionInfo.LoggedUserId; });
                window.TipsVisibilities.push(tipVisibility);
                this._tipsVisibilityService.update(tipVisibility).subscribe(function (res) {
                    _this.IsStartSave = false;
                });
                // Update
            }
            else {
                tipVisibility = new TipsVisibilityPM_1.TipsVisibilityPM();
                tipVisibility.IsVisible = this.isTipVisible;
                tipVisibility.TipCode = this.Tip.Code;
                tipVisibility.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                tipVisibility.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                this._tipsVisibilityService.insert(tipVisibility).subscribe(function (res) {
                    var pmResponse = res;
                    _this.IsStartSave = false;
                    if (!pmResponse.HasError) {
                        var myResult = pmResponse.Result;
                        if (myResult) {
                            window.TipsVisibilities.push(myResult);
                        }
                    }
                });
                //   Add
            }
        }
    };
    TipsComponent.prototype.DontShowAginTipAreaClick = function () {
        this.SaveChanges(this.DontShow);
    };
    TipsComponent.prototype.CloseToolTipArea = function () {
        if (this.HasTip) {
            this.TipVisibilityChangedEvent.emit("false");
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TipsComponent.prototype, "TipVisibilityChangedEvent", void 0);
    TipsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TipsComponent.html',
            inputs: ['ObjectTableName', 'Code', 'IsInternalTips', 'IsFirstTipLoad'],
            selector: 'TipsComponent',
            providers: [TipsVisibilityService_1.TipsVisibilityService],
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, TipsVisibilityService_1.TipsVisibilityService])
    ], TipsComponent);
    return TipsComponent;
}());
exports.TipsComponent = TipsComponent;
//# sourceMappingURL=TipsComponent.js.map