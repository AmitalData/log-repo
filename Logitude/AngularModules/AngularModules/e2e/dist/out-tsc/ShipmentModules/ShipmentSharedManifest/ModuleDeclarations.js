"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SharedManifestComponent_1 = require("./Components/SharedManifestComponent");
var SharedManifestStarted_1 = require("./Components/SharedManifestStarted");
var SharedManifestAdditionalComponent_1 = require("./Components/SharedManifestAdditionalComponent");
var SharedManifestsWorkSpaces_1 = require("./Components/SharedManifestsWorkSpaces");
var SharedManifestHeaderComponent_1 = require("./Components/SharedManifestHeaderComponent");
var SharedManifestEditAgentComponent_1 = require("./Components/SharedManifestEditAgentComponent");
exports.Components = [
    SharedManifestComponent_1.SharedManifestComponent,
    SharedManifestStarted_1.SharedManifestStarted,
    SharedManifestAdditionalComponent_1.SharedManifestAdditionalComponent,
    SharedManifestHeaderComponent_1.SharedManifestHeaderComponent,
    SharedManifestsWorkSpaces_1.SharedManifestsWorkSpaces,
    SharedManifestEditAgentComponent_1.SharedManifestEditAgentComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "SharedManifestComponent": {
                myResult = SharedManifestComponent_1.SharedManifestComponent;
                break;
            }
            case "SharedManifestStarted": {
                myResult = SharedManifestStarted_1.SharedManifestStarted;
                break;
            }
            case "SharedManifestAdditionalComponent": {
                myResult = SharedManifestAdditionalComponent_1.SharedManifestAdditionalComponent;
                break;
            }
            case "SharedManifestHeaderComponent": {
                myResult = SharedManifestHeaderComponent_1.SharedManifestHeaderComponent;
                break;
            }
            case "SharedManifestsWorkSpaces": {
                myResult = SharedManifestsWorkSpaces_1.SharedManifestsWorkSpaces;
                break;
            }
            case "SharedManifestEditAgentComponent": {
                myResult = SharedManifestEditAgentComponent_1.SharedManifestEditAgentComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map