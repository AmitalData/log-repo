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
var Tools_1 = require("../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////
var SplitButtonComponent = /** @class */ (function () {
    function SplitButtonComponent(_CD, myElement) {
        this._CD = _CD;
        this.AvoidDoubleClick = false;
        this.DefaultSplitButtonClicked = new core_1.EventEmitter();
        this.IsDisabled = false;
        this.ButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Send"); //"שלח";
        this.MENUDivExtraTop = 24;
        this.MENUDivExtraLeft = 0;
        this.OnClickedShowMenuContent = false;
        this._DropdownDisplay = 'none';
        this.MyCurrentSplitButtonComponentId = 0;
        this._IsLoaded = false;
        this.IsDisabledTimeout = false;
        this.Width = -30;
        this.Height = -20;
        this._ElementRef = myElement;
        ///this.DataContext = this; 
        this.MyCurrentSplitButtonComponentId = SplitButtonComponent_1.MyCounterId++;
        this._SplitButtonComponentId = "SplitButtonComponent_" + this.MyCurrentSplitButtonComponentId;
        this._SplitButtonComponentMenuId = "SplitButtonComponentMenuId_" + this.MyCurrentSplitButtonComponentId;
        this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
    }
    SplitButtonComponent_1 = SplitButtonComponent;
    Object.defineProperty(SplitButtonComponent.prototype, "ButtonCodeText", {
        get: function () { return this._ButtonCodeText; },
        set: function (val) {
            if (this._ButtonCodeText == val)
                return;
            this._ButtonCodeText = val;
            this.ButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate(val);
            this._CD.detectChanges();
        },
        enumerable: true,
        configurable: true
    });
    SplitButtonComponent.prototype.DefaultButtonClick = function (sourceButton, event) {
        this.CloseOtherLastmenu();
        if (this.OnClickedShowMenuContent) {
            this.dropdowndisplayToggle(null /*event*/);
            this.DefaultSplitButtonClicked.emit(sourceButton);
        }
        else {
            //event.stopPropagation();
            //this.DropdownDisplayCloseANdJustEmit();
            if (sourceButton == "splitterButton") {
                this.dropdowndisplayToggle(event);
            }
            else {
                this.DefaultSplitButtonClicked.emit(sourceButton);
            }
        }
    };
    //DropdownDisplayCloseANdJustEmit() {
    //  this.DropdownDisplayClose();
    //  this.DefaultSplitButtonClicked.emit("DefaultButtonClicked");
    //}
    SplitButtonComponent.prototype.handleClick = function (event) {
        if (this._DropdownDisplay == 'none') {
            return;
        }
        var clickedComponent = event.target;
        var inside = false;
        var conter = 0;
        do {
            if (clickedComponent === this._ElementRef.nativeElement) {
                inside = true;
                break;
            }
            if (conter > 10) {
                break;
            }
            conter++;
            clickedComponent = clickedComponent.parentNode;
        } while (clickedComponent);
        if (inside) {
        }
        else {
            this.DropdownDisplayClose();
            if (this._DropdownDisplay == 'block') {
                this.dropdowndisplayToggle(null);
            }
            //alert("outside");
        }
    };
    SplitButtonComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this._IsLoaded = true;
            /// alert("this._IsLoaded");
            if (Tools_1.AppTool.IsNullOrEmpty(_this.ButtonText)) {
                _this.ButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Send"); //"שלח";
            }
        });
    };
    SplitButtonComponent.prototype.DropdownDisplayClose = function () {
        this._DropdownDisplay = 'none';
        this._CD.detectChanges();
    };
    SplitButtonComponent.prototype.CloseOtherLastmenu = function () {
        var suppress = true;
        if (suppress) {
            return;
        }
        if (SplitButtonComponent_1.LastSplitButtonClickedId != 0 && SplitButtonComponent_1.LastSplitButtonClickedId != this.MyCurrentSplitButtonComponentId) {
            var lastSplitButtonComponentMenu = document.getElementById("SplitButtonComponentMenuId_" + SplitButtonComponent_1.LastSplitButtonClickedId);
            if (!Tools_1.AppTool.IsNullOrEmpty(lastSplitButtonComponentMenu)) {
                lastSplitButtonComponentMenu.style.display = 'none';
            }
        }
        SplitButtonComponent_1.LastSplitButtonClickedId = this.MyCurrentSplitButtonComponentId;
    };
    SplitButtonComponent.EnsureLastSplitButtonIsClosed = function () {
        var suppress = true;
        if (suppress) {
            return;
        }
        var lastSplitButtonComponentMenu = document.getElementById("SplitButtonComponentMenuId_" + SplitButtonComponent_1.LastSplitButtonClickedId);
        if (!Tools_1.AppTool.IsNullOrEmpty(lastSplitButtonComponentMenu)) {
            lastSplitButtonComponentMenu.style.display = 'none';
        }
    };
    SplitButtonComponent.prototype.dropdowndisplayToggle = function (event) {
        if (!Tools_1.AppTool.IsNullOrEmpty(event)) {
            ///event.stopPropagation();
            this.CloseOtherLastmenu();
        }
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._SplitButtonComponentId);
            var itemRect = item.getBoundingClientRect();
            //document.getElementById(this._SplitButtonComponentMenuId).style.top = (itemRect.top + 24 ) + 'px';
            //document.getElementById(this._SplitButtonComponentMenuId).style.left = (itemRect.left + 24 - this.Width) + 'px';
            document.getElementById(this._SplitButtonComponentMenuId).style.top =
                itemRect.top + 'px';
            var DDLHeight = 67; //    height: 22px; * 3 +30 
            var MENUDivExtraTop = Number(this.MENUDivExtraTop); //22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
            if (itemRect.bottom + DDLHeight > this.getScreenHeight()) { //this.PaintTop = true                
                document.getElementById(this._SplitButtonComponentMenuId).style.top =
                    (itemRect.top - DDLHeight - MENUDivExtraTop) + 'px';
            }
            var MENUDivExtraLeft = Number(this.MENUDivExtraLeft); //22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
            document.getElementById(this._SplitButtonComponentMenuId).style.left =
                (itemRect.left + MENUDivExtraLeft) + 'px'; //min-width: 80px
            this._DropdownDisplay = 'block';
        }
        else {
            this._DropdownDisplay = 'none';
        }
        this._CD.detectChanges();
    };
    SplitButtonComponent.prototype.getScreenHeight = function () {
        if (self.innerHeight) {
            return self.innerHeight;
        }
        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }
        if (document.body) {
            return document.body.clientHeight;
        }
    };
    var SplitButtonComponent_1;
    SplitButtonComponent.MyCounterId = 0;
    SplitButtonComponent.LastSplitButtonClickedId = 0;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], SplitButtonComponent.prototype, "AvoidDoubleClick", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], SplitButtonComponent.prototype, "DefaultSplitButtonClicked", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], SplitButtonComponent.prototype, "IsDisabled", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], SplitButtonComponent.prototype, "ButtonText", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], SplitButtonComponent.prototype, "MENUDivExtraTop", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], SplitButtonComponent.prototype, "MENUDivExtraLeft", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], SplitButtonComponent.prototype, "OnClickedShowMenuContent", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String),
        __metadata("design:paramtypes", [String])
    ], SplitButtonComponent.prototype, "ButtonCodeText", null);
    SplitButtonComponent = SplitButtonComponent_1 = __decorate([
        core_1.Component({
            selector: 'split-button',
            moduleId: module.id,
            //templateUrl: 'CustomsRequestsComponent.html',
            host: {
                '(document:click)': 'handleClick($event)',
            },
            templateUrl: './SplitButtonComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, core_1.ElementRef])
    ], SplitButtonComponent);
    return SplitButtonComponent;
}());
exports.SplitButtonComponent = SplitButtonComponent;
//# sourceMappingURL=SplitButtonComponent.js.map