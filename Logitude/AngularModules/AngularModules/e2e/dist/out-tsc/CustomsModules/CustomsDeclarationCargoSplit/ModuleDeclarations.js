"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DeclarationCargoSplitEditComponent_1 = require("./Components/EditTabs/DeclarationCargoSplitEditComponent");
var NewDeclarationCargoSplitComponent_1 = require("./Components/NewEntity/NewDeclarationCargoSplitComponent");
var DecCargoSplitConComponent_1 = require("./Components/EditTabs/DecCargoSplitConComponent");
var DecCargoSplitConsPackDetComponent_1 = require("./Components/EditTabs/DecCargoSplitConsPackDetComponent");
var CargoSplitGeneralTabComponent_1 = require("./Components/EditTabs/General/CargoSplitGeneralTabComponent");
exports.Components = [
    DeclarationCargoSplitEditComponent_1.DeclarationCargoSplitEditComponent,
    NewDeclarationCargoSplitComponent_1.NewDeclarationCargoSplitComponent,
    DecCargoSplitConComponent_1.DecCargoSplitConComponent,
    DecCargoSplitConsPackDetComponent_1.DecCargoSplitConsPackDetComponent,
    CargoSplitGeneralTabComponent_1.CargoSplitGeneralTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewDeclarationCargoSplitComponent": {
                myResult = NewDeclarationCargoSplitComponent_1.NewDeclarationCargoSplitComponent;
                break;
            }
            case "CargoSplitGeneralTabComponent": {
                myResult = CargoSplitGeneralTabComponent_1.CargoSplitGeneralTabComponent;
                break;
            }
            case "DeclarationCargoSplitEditComponent": {
                myResult = DeclarationCargoSplitEditComponent_1.DeclarationCargoSplitEditComponent;
                break;
            }
            case "DecCargoSplitConComponent": {
                myResult = DecCargoSplitConComponent_1.DecCargoSplitConComponent;
                break;
            }
            case "DecCargoSplitConsPackDetComponent": {
                myResult = DecCargoSplitConsPackDetComponent_1.DecCargoSplitConsPackDetComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map