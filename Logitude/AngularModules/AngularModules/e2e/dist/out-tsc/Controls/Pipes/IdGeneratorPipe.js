"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../Infrastructure/Tools");
var ControlsIdCounter_1 = require("../../Infrastructure/Utilities/ControlsIdCounter");
var IdGeneratorPipe = /** @class */ (function () {
    function IdGeneratorPipe() {
    }
    IdGeneratorPipe.prototype.transform = function (value) {
        if (value) {
            value = Tools_1.AppTool.Replace(value, " ", "");
        }
        var UnuieqDomId = value;
        if (this.CheckIfExists(value)) {
            var counterId = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter(value);
            if (counterId != null) {
                UnuieqDomId = UnuieqDomId + "_" + counterId;
            }
        }
        return UnuieqDomId;
    };
    IdGeneratorPipe.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    IdGeneratorPipe = __decorate([
        core_1.Pipe({ name: 'IdGeneratorPipe' })
    ], IdGeneratorPipe);
    return IdGeneratorPipe;
}());
exports.IdGeneratorPipe = IdGeneratorPipe;
//# sourceMappingURL=IdGeneratorPipe.js.map