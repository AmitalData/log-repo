"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var CourierMasterService_1 = require("../../../../Customs/Services/Others/CourierMasterService");
var CourierMasterValidator_1 = require("../../../../Customs/Validators/CourierMasterValidator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CourierMasterGeneralTabComponent = /** @class */ (function (_super) {
    __extends(CourierMasterGeneralTabComponent, _super);
    function CourierMasterGeneralTabComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.ObjectTableName = "Customs.CourierMaster";
        _this.DataContext = _this;
        //entityPM: CourierMasterPM;
        _this.CourierMasterValidator = new CourierMasterValidator_1.CourierMasterValidator();
        _this.CourierMasterService = new CourierMasterService_1.CourierMasterService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.EntityPM = entityArgs.EntityPM;
        _this.Listen();
        return _this;
    }
    CourierMasterGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess && _this.CurrentSession.CurrentEditComponent) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "COGN") {
                    }
                }
            }));
        }
    };
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "AirlineId", {
        get: function () { return this.EntityPM.AirlineId; },
        set: function (value) {
            if (this.EntityPM.AirlineId != value) {
                this.EntityPM.AirlineId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "MAWB", {
        get: function () { return this.EntityPM.MAWB; },
        set: function (value) {
            if (this.EntityPM.MAWB != value) {
                this.EntityPM.MAWB = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "HAWB", {
        get: function () { return this.EntityPM.HAWB; },
        set: function (value) {
            if (this.EntityPM.HAWB != value) {
                this.EntityPM.HAWB = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "OriginPortCode", {
        get: function () { return this.EntityPM.OriginPortCode; },
        set: function (value) {
            if (this.EntityPM.OriginPortCode != value) {
                this.EntityPM.OriginPortCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "ManifestNumber", {
        get: function () { return this.EntityPM.ManifestNumber; },
        set: function (value) {
            if (this.EntityPM.ManifestNumber != value) {
                this.EntityPM.ManifestNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "GatewayPortCode", {
        get: function () { return this.EntityPM.GatewayPortCode; },
        set: function (value) {
            if (this.EntityPM.GatewayPortCode != value) {
                this.EntityPM.GatewayPortCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "FlightNumber", {
        get: function () { return this.EntityPM.FlightNumber; },
        set: function (value) {
            if (this.EntityPM.FlightNumber != value) {
                this.EntityPM.FlightNumber = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "IsOpen", {
        get: function () { return this.EntityPM.IsOpen; },
        set: function (value) {
            if (this.EntityPM.IsOpen != value) {
                this.EntityPM.IsOpen = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "DepartureDate", {
        get: function () { return this.EntityPM.DepartureDate; },
        set: function (value) {
            if (this.EntityPM.DepartureDate != value) {
                this.EntityPM.DepartureDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "PackageQuantity", {
        get: function () { return this.EntityPM.PackageQuantity; },
        set: function (value) {
            if (this.EntityPM.PackageQuantity != value) {
                this.EntityPM.PackageQuantity = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "GrossMassMeasure", {
        get: function () { return this.EntityPM.GrossMassMeasure; },
        set: function (value) {
            if (this.EntityPM.GrossMassMeasure != value) {
                this.EntityPM.GrossMassMeasure = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "EstimatedArrivalDateOnly", {
        get: function () { return this.EntityPM.EstimatedArrivalDateOnly; },
        set: function (value) {
            if (this.EntityPM.EstimatedArrivalDateOnly != value) {
                this.EntityPM.EstimatedArrivalDateOnly = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "EstimatedArrivalTimeOnly", {
        get: function () { return this.EntityPM.EstimatedArrivalTimeOnly; },
        set: function (value) {
            if (this.EntityPM.EstimatedArrivalTimeOnly != value) {
                this.EntityPM.EstimatedArrivalTimeOnly = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "EstimatedArrivalDate", {
        get: function () { return this.EntityPM.EstimatedArrivalDate; },
        set: function (value) {
            if (this.EntityPM.EstimatedArrivalDate != value) {
                this.EntityPM.EstimatedArrivalDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CourierMasterGeneralTabComponent.prototype, "WeightValueCode", {
        get: function () { return this.EntityPM.WeightValueCode; },
        set: function (value) {
            if (this.EntityPM.WeightValueCode != value) {
                this.EntityPM.WeightValueCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CourierMasterGeneralTabComponent.prototype.AirLineIdLostFocus = function (value) {
        //this.CourierMasterService.GetIfCourierMasterExists(this.EntityPM.Id, this.EntityPM.AirlineId, this.EntityPM.HAWB, this.EntityPM.MAWB).subscribe(Result => {
        //    var mm: ServiceResponse = Result;
        //    if (!mm.HasError) {
        //        if (mm.Result) {
        //            var errorMsg: string = "Already exist";
        //            this.UIProperties.SetValidity("AirlineId", "Customs.CourierMaster", false,"Already exist");
        //            //this.CourierMasterValidator.ValidationErrorMessageCodes.push(errorMsg);
        //            //this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(errorMsg);
        //        }
        //    }
        //});
    };
    CourierMasterGeneralTabComponent.prototype.MAWBLostFocus = function (value) {
    };
    CourierMasterGeneralTabComponent.prototype.HAWBLostFocus = function (value) {
    };
    CourierMasterGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CourierMasterGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CourierMasterGeneralTabComponent);
    return CourierMasterGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.CourierMasterGeneralTabComponent = CourierMasterGeneralTabComponent;
//# sourceMappingURL=CourierMasterGeneralTabComponent.js.map