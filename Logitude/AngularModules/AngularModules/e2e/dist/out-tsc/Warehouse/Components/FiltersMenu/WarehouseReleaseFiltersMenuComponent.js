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
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var WarehouseReleaseFiltersMenuComponent = /** @class */ (function () {
    function WarehouseReleaseFiltersMenuComponent() {
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        this.DirectionWidth = 140;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.selectedValue = "All";
        this.selectedDirection = "All";
        if (this.CurrentSession == null) {
            this.TransportFilter_A = "TransportFilter_A_-1_-1";
            this.TransportFilter_O = "TransportFilter_O_-1_-1";
            this.TransportFilter_I = "TransportFilter_I_-1_-1";
            this.DirectionFilter_E = "DirectionFilter_E_-1_-1";
            this.DirectionFilter_I = "DirectionFilter_I_-1_-1";
            this.DirectionFilter_R = "DirectionFilter_R_-1_-1";
            this.DirectionFilter_D = "DirectionFilter_D_-1_-1";
            this.DirectionFilter_C = "DirectionFilter_D_-1_-1";
        }
        else {
            var index_T = this.CurrentSession.GetNewId("WarehouseReleaseTransportFilterMenu");
            var index_D = this.CurrentSession.GetNewId("WarehouseReleaseDirectionFilterMenu");
            this.TransportFilter_A = "TransportFilter_A" + index_T;
            this.TransportFilter_O = "TransportFilter_O" + index_T;
            this.TransportFilter_I = "TransportFilter_I" + index_T;
            this.DirectionFilter_E = "DirectionFilter_E" + index_D;
            this.DirectionFilter_R = "DirectionFilter_R" + index_D;
            this.DirectionFilter_D = "DirectionFilter_D" + index_D;
            this.DirectionFilter_I = "DirectionFilter_I" + index_D;
            this.DirectionFilter_C = "DirectionFilter_C" + index_D;
        }
        this.DirectionWidth = 140;
    }
    WarehouseReleaseFiltersMenuComponent.prototype.ngAfterViewInit = function () {
        this.ApplyTransportSelectedStyle();
        this.ApplyDirectionSelectedStyle();
    };
    // Transport
    WarehouseReleaseFiltersMenuComponent.prototype.SetTransport = function (itemValue) {
        this.selectedValue = itemValue;
        this.ApplyTransportSelectedStyle();
    };
    Object.defineProperty(WarehouseReleaseFiltersMenuComponent.prototype, "SelectedValue", {
        get: function () { return this.selectedValue; },
        set: function (value) {
            if (this.selectedValue != value) {
                this.selectedValue = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseReleaseFiltersMenuComponent.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            var RemoveFilter = false;
            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(function (a) { return a.FieldName != "TransportModeId"; });
            }
            this.apiQueryFilters.addAdditionalFilter("TransportModeId", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
            if (itemValue == "All") {
                RemoveFilter = true;
            }
            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
            this.ApplyTransportSelectedStyle();
        }
    };
    WarehouseReleaseFiltersMenuComponent.prototype.itemMouseOver = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I.png");
                    //img_I.style.top = "1px";
                    break;
                }
            }
        }
    };
    WarehouseReleaseFiltersMenuComponent.prototype.itemMouseLeave = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var img_A = document.getElementById(this.TransportFilter_A);
            var img_O = document.getElementById(this.TransportFilter_O);
            var img_I = document.getElementById(this.TransportFilter_I);
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_g.png");
                    break;
                }
            }
        }
    };
    WarehouseReleaseFiltersMenuComponent.prototype.ApplyTransportSelectedStyle = function () {
        var itemValue = this.SelectedValue;
        var img_A = document.getElementById(this.TransportFilter_A);
        var img_O = document.getElementById(this.TransportFilter_O);
        var img_I = document.getElementById(this.TransportFilter_I);
        if (img_A) {
            this.CurrentSession.ChangeSessionHeader({ TransportId: itemValue });
            img_A.setAttribute("src", "./Images/TransportModes/A_g.png");
            img_O.setAttribute("src", "./Images/TransportModes/O_g.png");
            img_I.setAttribute("src", "./Images/TransportModes/I_G.png");
            switch (itemValue) {
                case "A": {
                    img_A.setAttribute("src", "./Images/TransportModes/A_w.png");
                    break;
                }
                case "O": {
                    img_O.setAttribute("src", "./Images/TransportModes/O_w.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./Images/TransportModes/I_w.png");
                    break;
                }
            }
        }
    };
    // Direction
    WarehouseReleaseFiltersMenuComponent.prototype.SetDirection = function (itemValue) {
        this.selectedDirection = itemValue;
        this.ApplyDirectionSelectedStyle();
    };
    Object.defineProperty(WarehouseReleaseFiltersMenuComponent.prototype, "SelectedDirection", {
        get: function () { return this.selectedDirection; },
        set: function (value) {
            if (this.selectedDirection != value) {
                this.selectedDirection = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    WarehouseReleaseFiltersMenuComponent.prototype.DirectionitemClicked = function (itemValue) {
        if (this.SelectedDirection != itemValue) {
            this.SelectedDirection = itemValue;
            var RemoveFilter = false;
            if (this.apiQueryFilters.AdditionalFilters.length > 0) {
                this.apiQueryFilters.AdditionalFilters = this.apiQueryFilters.AdditionalFilters.filter(function (a) { return a.FieldName != "DirectionId"; });
            }
            this.apiQueryFilters.addAdditionalFilter("DirectionId", itemValue, null, null, "Equals", false, true, false, "string", (itemValue == "All" ? true : false));
            this.SelectedValueChanged.emit({ Filters: this.apiQueryFilters, RemoveFilter: RemoveFilter });
            this.ApplyDirectionSelectedStyle();
        }
    };
    WarehouseReleaseFiltersMenuComponent.prototype.DirectionitemMouseOver = function (itemValue) {
        if (this.SelectedDirection != itemValue) {
            var img_E = document.getElementById(this.DirectionFilter_E);
            var img_I = document.getElementById(this.DirectionFilter_I);
            var img_R = document.getElementById(this.DirectionFilter_R);
            var img_D = document.getElementById(this.DirectionFilter_D);
            var img_C;
            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I.png");
                    break;
                }
                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R.png");
                    break;
                }
                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D.png");
                    break;
                }
                case "C": {
                    break;
                }
            }
        }
    };
    WarehouseReleaseFiltersMenuComponent.prototype.DirectionitemMouseLeave = function (itemValue) {
        if (this.SelectedDirection != itemValue) {
            var img_E = document.getElementById(this.DirectionFilter_E);
            var img_I = document.getElementById(this.DirectionFilter_I);
            var img_R = document.getElementById(this.DirectionFilter_R);
            var img_D = document.getElementById(this.DirectionFilter_D);
            var img_C;
            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E_g.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I_g.png");
                    break;
                }
                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R_g.png");
                    break;
                }
                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D_g.png");
                    break;
                }
                case "C": {
                    break;
                }
            }
        }
    };
    WarehouseReleaseFiltersMenuComponent.prototype.ApplyDirectionSelectedStyle = function () {
        var itemValue = this.SelectedDirection;
        var img_E = document.getElementById(this.DirectionFilter_E);
        var img_I = document.getElementById(this.DirectionFilter_I);
        var img_R = document.getElementById(this.DirectionFilter_R);
        var img_D = document.getElementById(this.DirectionFilter_D);
        var img_C;
        if (img_E) {
            this.CurrentSession.ChangeSessionHeader({ DirectionId: itemValue });
            //var img_C = document.getElementById("DirectionFilter_C");
            img_E.setAttribute("src", "./Images/Directions/E_g.png");
            img_I.setAttribute("src", "./Images/Directions/I_g.png");
            img_R.setAttribute("src", "./Images/Directions/R_G.png");
            img_D.setAttribute("src", "./Images/Directions/D_G.png");
            //img_C.setAttribute("src", "./Images/Directions/C_G.png");
            switch (itemValue) {
                case "E": {
                    img_E.setAttribute("src", "./Images/Directions/E_w.png");
                    break;
                }
                case "I": {
                    img_I.setAttribute("src", "./Images/Directions/I_w.png");
                    break;
                }
                case "R": {
                    img_R.setAttribute("src", "./Images/Directions/R_w.png");
                    break;
                }
                case "D": {
                    img_D.setAttribute("src", "./Images/Directions/D_w.png");
                    break;
                }
                case "C": {
                    break;
                }
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], WarehouseReleaseFiltersMenuComponent.prototype, "SelectedValueChanged", void 0);
    WarehouseReleaseFiltersMenuComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './WarehouseReleaseFiltersMenuComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], WarehouseReleaseFiltersMenuComponent);
    return WarehouseReleaseFiltersMenuComponent;
}());
exports.WarehouseReleaseFiltersMenuComponent = WarehouseReleaseFiltersMenuComponent;
//# sourceMappingURL=WarehouseReleaseFiltersMenuComponent.js.map