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
var Image = /** @class */ (function () {
    function Image() {
        this.ControlId = null;
        this.Width = 16;
        this.Height = 16;
        this.Src = null;
        this.Title = null;
        this.IsDispaly = true;
    }
    Image.prototype.onError = function () {
        this.IsDispaly = false;
    };
    Image = __decorate([
        core_1.Component({
            selector: "Image",
            inputs: ['Src', 'Width', 'Height', 'Title'],
            template: "\n        <div *ngIf=\"IsDispaly\" [style.width.px]=\"Width\" [style.height.px]=\"Height\">\n             <img [attr.src]=\"Src\" [style.width.px]=\"Width\" [style.height.px]=\"Height\" [attr.title]=\"Title\" (error)=\"onError()\"/>\n         </div> \n    ",
        }),
        __metadata("design:paramtypes", [])
    ], Image);
    return Image;
}());
exports.Image = Image;
//# sourceMappingURL=Image.js.map