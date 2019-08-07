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
var Tools_1 = require("../../../../Infrastructure/Tools");
var RequestParamsBase_1 = require("../../../../Customs/DataContract/RequestParams/RequestParamsBase");
var DropdownMenuFilterComponent = /** @class */ (function () {
    function DropdownMenuFilterComponent(_CD, myElement) {
        this._CD = _CD;
        this.Dropdownbutton_Text = "Show Dropdown Content";
        this.DropdownMenuButtonClicked = new core_1.EventEmitter();
        this._DropdownDisplay = 'none';
        this.Width = -60;
        this.Height = -40;
        this._ElementRef = myElement;
        ///this.DataContext = this; 
        this._CustomSendOptionsArgs = new RequestParamsBase_1.CustomSendOptionsArgs();
        this._CustomSendOptionsArgs.ForcePersonalSign = false;
        var curId = DropdownMenuFilterComponent_1.MyId++;
        this.MyDropdownMenuFilterId = curId;
        this._DropdownMenuFilterComponentId = "DropdownMenuFilterComponent_" + curId;
        this._DropdownMenuFilterComponentMenuId = "DropdownButtonComponentMenuId_" + curId;
    }
    DropdownMenuFilterComponent_1 = DropdownMenuFilterComponent;
    DropdownMenuFilterComponent.prototype.handleClick = function (event) {
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
            if (clickedComponent.class === "class-dropdownfilter-content") {
                inside = true;
                break;
            }
            if (conter > 50) {
                break;
            }
            conter++;
            clickedComponent = clickedComponent.parentNode;
        } while (clickedComponent);
        if (inside) {
        }
        else {
            //if (this._DropdownDisplay == 'block') {
            //    this.DropdowndisplayToggle(null);
            //}
            this.DropdownDisplayClose();
        }
    };
    DropdownMenuFilterComponent.prototype.ngOnInit = function () {
    };
    DropdownMenuFilterComponent.prototype.DropdownDisplayClose = function () {
        this._DropdownDisplay = 'none';
        this._CD.detectChanges();
    };
    DropdownMenuFilterComponent.EnsureLastDropdownMenuIsClosed = function () {
        //var lastDropdownMenuFilter = document.getElementById("DropdownButtonComponentMenuId_" + DropdownMenuFilterComponent.LastDropdownMenuFilterId);
        //if (!AppTool.IsNullOrEmpty(lastDropdownMenuFilter)) {
        //    lastDropdownMenuFilter.style.display = 'none';
        //}
    };
    DropdownMenuFilterComponent.prototype.DropdownMenuButtonClick = function (event) {
        this.DropdowndisplayToggle(event);
        this.DropdownMenuButtonClicked.emit(event);
    };
    DropdownMenuFilterComponent.prototype.DropdowndisplayToggle = function (event) {
        //MouseEvent
        DropdownMenuFilterComponent_1.LastDropdownMenuFilterId = this.MyDropdownMenuFilterId;
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._DropdownMenuFilterComponentId);
            var itemRect = item.getBoundingClientRect();
            var myTop = itemRect.top;
            var myleft = itemRect.left;
            if (!Tools_1.AppTool.IsNullOrEmpty(event)) {
                myleft = event.clientX; //: 19
                myTop = event.clientY; //: 19
                // event.stopPropagation();
            }
            document.getElementById(this._DropdownMenuFilterComponentMenuId).style.top =
                (myTop /*itemRect.top*/ /*+ 27*/ /*-5*/) + 'px';
            document.getElementById(this._DropdownMenuFilterComponentMenuId).style.left =
                (myleft /*itemRect.left*/ /*- 50*/ - 100 /*+5*/) + 'px'; //min-width: 80px
            this._DropdownDisplay = 'block';
        }
        else {
            this._DropdownDisplay = 'none';
        }
        this._CD.detectChanges();
    };
    var DropdownMenuFilterComponent_1;
    DropdownMenuFilterComponent.MyId = 0;
    DropdownMenuFilterComponent.LastDropdownMenuFilterId = 0;
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DropdownMenuFilterComponent.prototype, "IsDisabled", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], DropdownMenuFilterComponent.prototype, "Dropdownbutton_Text", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], DropdownMenuFilterComponent.prototype, "DropdownMenuButtonClicked", void 0);
    DropdownMenuFilterComponent = DropdownMenuFilterComponent_1 = __decorate([
        core_1.Component({
            selector: 'courier-filter-button',
            moduleId: module.id,
            host: { '(document:click)': 'handleClick($event)', },
            templateUrl: 'DropdownMenuFilterComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef, core_1.ElementRef])
    ], DropdownMenuFilterComponent);
    return DropdownMenuFilterComponent;
}());
exports.DropdownMenuFilterComponent = DropdownMenuFilterComponent;
//# sourceMappingURL=DropdownMenuFilterComponent.js.map