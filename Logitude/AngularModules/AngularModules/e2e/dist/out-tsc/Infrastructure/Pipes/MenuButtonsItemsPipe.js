"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var MenuButtonsItemsPipe = /** @class */ (function () {
    function MenuButtonsItemsPipe() {
    }
    MenuButtonsItemsPipe.prototype.transform = function (menuItems, menuButtonParentId) {
        if (menuItems && menuButtonParentId) {
            var myResult = menuItems.filter(function (d) { return (d.MenuButtonType == 'menuitem' || d.MenuButtonType == 'separator') && d.ParentMenuButtonId == menuButtonParentId; });
            myResult = myResult.sort(function (a, b) {
                if (a.Index > b.Index) {
                    return 1;
                }
                else if (b.Index > a.Index) {
                    return -1;
                }
                return 0;
            });
            return myResult;
        }
        return null;
    };
    MenuButtonsItemsPipe = __decorate([
        core_1.Pipe({ name: 'MenuButtonsItemsPipe' })
    ], MenuButtonsItemsPipe);
    return MenuButtonsItemsPipe;
}());
exports.MenuButtonsItemsPipe = MenuButtonsItemsPipe;
//# sourceMappingURL=MenuButtonsItemsPipe.js.map