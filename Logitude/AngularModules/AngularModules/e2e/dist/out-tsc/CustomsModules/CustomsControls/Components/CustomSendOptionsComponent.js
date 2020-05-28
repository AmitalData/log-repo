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
var Tools_1 = require("../../../Infrastructure/Tools");
var RequestParamsBase_1 = require("../../../Customs/DataContract/RequestParams/RequestParamsBase");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
//////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////
var CustomSendOptionsComponent = /** @class */ (function () {
    function CustomSendOptionsComponent(_CD, myElement) {
        this._CD = _CD;
        this.AvoidDoubleClick = false;
        this.SendButtonClicked = new core_1.EventEmitter();
        this.IsDisabled = false;
        this.CustomSendOptionsButtonCanForcePersonalSign = false; //Show ForcePersonalSign
        this.ButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Send"); //"שלח";
        this.IsCheckBoxVisibile = true;
        this._DropdownDisplay = 'none';
        this._IsLoaded = false;
        this.IsDisabledTimeout = false;
        this.Width = -30;
        this.Height = -20;
        this._ElementRef = myElement;
        ///this.DataContext = this; 
        this._CustomSendOptionsArgs = new RequestParamsBase_1.CustomSendOptionsArgs();
        this._CustomSendOptionsArgs.ForcePersonalSign = false;
        var curId = CustomSendOptionsComponent_1.MyId++;
        this._CustomSendOptionsComponentId = "CustomSendOptionsComponent_" + curId;
        this._CustomSendOptionsComponentMenuId = "CustomSendOptionsComponentMenuId_" + curId;
        this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
    }
    CustomSendOptionsComponent_1 = CustomSendOptionsComponent;
    Object.defineProperty(CustomSendOptionsComponent.prototype, "ButtonCodeText", {
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
    Object.defineProperty(CustomSendOptionsComponent.prototype, "ForcePersonalSign", {
        get: function () { return this._CustomSendOptionsArgs.ForcePersonalSign; },
        set: function (val) {
            this._CustomSendOptionsArgs.ForcePersonalSign = val;
        },
        enumerable: true,
        configurable: true
    });
    CustomSendOptionsComponent.prototype.SendDefault = function () {
        this._CustomSendOptionsArgs.Option = "";
        this._CustomSendOptionsArgs.RequestVIA = RequestParamsBase_1.SendRequestVIA.Default;
        this.JustEmit();
    };
    CustomSendOptionsComponent.prototype.SendWI = function () {
        this._CustomSendOptionsArgs.Option = "WI";
        this._CustomSendOptionsArgs.RequestVIA = RequestParamsBase_1.SendRequestVIA.WebServiceInteractive;
        this.JustEmit();
    };
    CustomSendOptionsComponent.prototype.JustEmit = function () {
        var _this = this;
        this.DropdownDisplayClose();
        var toSign = this._CustomSendOptionsArgs.ForcePersonalSign;
        this.SendButtonClicked.emit({
            Option: this._CustomSendOptionsArgs.Option,
            ForcePersonalSign: toSign,
            RequestVIA: this._CustomSendOptionsArgs.RequestVIA,
        });
        this._CustomSendOptionsArgs.ForcePersonalSign = false;
        if (this.AvoidDoubleClick || this.CustomSendOptionsButtonCanForcePersonalSign) { //Due double request == double click 
            var featueDisable2Sec = true;
            if (featueDisable2Sec) {
                this.IsDisabledTimeout = true;
                setTimeout(function () {
                    _this.IsDisabledTimeout = false;
                }, 2000);
            }
        }
    };
    CustomSendOptionsComponent.prototype.SendWB = function () {
        this._CustomSendOptionsArgs.Option = "WB";
        this._CustomSendOptionsArgs.RequestVIA = RequestParamsBase_1.SendRequestVIA.WebServiceBatch;
        this.JustEmit();
    };
    CustomSendOptionsComponent.prototype.SendD = function () {
        this._CustomSendOptionsArgs.Option = "D";
        this._CustomSendOptionsArgs.RequestVIA = RequestParamsBase_1.SendRequestVIA.DCABatch;
        this.JustEmit();
    };
    CustomSendOptionsComponent.prototype.handleClick = function (event) {
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
            if (this._DropdownDisplay == 'block') {
                this.dropdowndisplayToggle();
            }
            //alert("outside");
        }
    };
    CustomSendOptionsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
            _this._IsLoaded = true;
            /// alert("this._IsLoaded");
            if (Tools_1.AppTool.IsNullOrEmpty(_this.ButtonText)) {
                _this.ButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Send"); //"שלח";
            }
        });
    };
    CustomSendOptionsComponent.prototype.DropdownDisplayClose = function () {
        this._DropdownDisplay = 'none';
    };
    CustomSendOptionsComponent.prototype.ForcePersonalSignToggle = function () {
        this._CustomSendOptionsArgs.ForcePersonalSign = !this._CustomSendOptionsArgs.ForcePersonalSign;
    };
    CustomSendOptionsComponent.prototype.dropdowndisplayToggle = function () {
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._CustomSendOptionsComponentId);
            var itemRect = item.getBoundingClientRect();
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.top = (itemRect.top + 24 ) + 'px';
            //document.getElementById(this._CustomSendOptionsComponentMenuId).style.left = (itemRect.left + 24 - this.Width) + 'px';
            document.getElementById(this._CustomSendOptionsComponentMenuId).style.top =
                itemRect.top + 'px';
            var DDLHeight = 67; //    height: 22px; * 3 +30 
            var Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
            if (itemRect.bottom + DDLHeight > this.getScreenHeight()) { //this.PaintTop = true                
                document.getElementById(this._CustomSendOptionsComponentMenuId).style.top =
                    (itemRect.top - DDLHeight - Extra) + 'px';
            }
            document.getElementById(this._CustomSendOptionsComponentMenuId).style.left =
                (itemRect.left + 80) + 'px'; //min-width: 80px
            this._DropdownDisplay = 'block';
        }
        else {
            this._DropdownDisplay = 'none';
        }
    };
    CustomSendOptionsComponent.prototype.getScreenHeight = function () {
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
    var CustomSendOptionsComponent_1;
    CustomSendOptionsComponent.MyId = 0;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CustomSendOptionsComponent.prototype, "AvoidDoubleClick", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CustomSendOptionsComponent.prototype, "SendButtonClicked", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CustomSendOptionsComponent.prototype, "IsDisabled", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CustomSendOptionsComponent.prototype, "CustomSendOptionsButtonCanForcePersonalSign", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], CustomSendOptionsComponent.prototype, "ButtonText", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], CustomSendOptionsComponent.prototype, "IsCheckBoxVisibile", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String),
        __metadata("design:paramtypes", [String])
    ], CustomSendOptionsComponent.prototype, "ButtonCodeText", null);
    CustomSendOptionsComponent = CustomSendOptionsComponent_1 = __decorate([
        core_1.Component({
            selector: 'custom-send-options',
            moduleId: module.id,
            //templateUrl: 'CustomsRequestsComponent.html',
            host: {
                '(document:click)': 'handleClick($event)',
            },
            templateUrl: './CustomSendOptionsComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, core_1.ElementRef])
    ], CustomSendOptionsComponent);
    return CustomSendOptionsComponent;
}());
exports.CustomSendOptionsComponent = CustomSendOptionsComponent;
//# sourceMappingURL=CustomSendOptionsComponent.js.map