"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../Tools");
var AttatchmentIconPipe = /** @class */ (function () {
    function AttatchmentIconPipe() {
    }
    AttatchmentIconPipe.prototype.transform = function (value, Parameter) {
        if (Parameter === void 0) { Parameter = null; }
        var path = "something";
        var newValue = value;
        if (!Tools_1.AppTool.IsNullOrEmpty(newValue)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(Parameter)) {
                path = this.ApplyParameterPipe(newValue, Parameter);
            }
            else {
                path = this.GetIcon(newValue);
            }
        }
        return path;
    };
    AttatchmentIconPipe.prototype.ApplyParameterPipe = function (value, Parameter) {
        var path = "something";
        if (Parameter == "FilingInboxCode") {
            path = this.GetIcon(value.split('.')[1]);
        }
        return path;
    };
    AttatchmentIconPipe.prototype.GetIcon = function (value) {
        var path = "something";
        switch (value.toUpperCase()) {
            case "PDF":
                {
                    path = "./Images/FileIcons/File-pdf-48.png";
                    break;
                }
            case "TXT":
                {
                    path = "./Images/FileIcons/txt_48.png";
                    break;
                }
            case "XLS":
                {
                    path = "./Images/FileIcons/Microsoft-Office-Excel-48.png";
                    break;
                }
            case "XLSX":
                {
                    path = "./Images/FileIcons/Microsoft-Office-Excel-48.png";
                    break;
                }
            case "DOC":
                {
                    path = "./Images/FileIcons/Microsoft-Office-Word-48.png";
                    break;
                }
            case "DOCX":
                {
                    path = "./Images/FileIcons/Microsoft-Office-Word-48.png";
                    break;
                }
            case "PPT":
                {
                    path = "./Images/FileIcons/Microsoft-Office-PowerPoint-48.png";
                    break;
                }
            case "PPTX":
                {
                    path = "./Images/FileIcons/Microsoft-Office-PowerPoint-48.png";
                    break;
                }
            case "ZIP":
                {
                    path = "./Images/FileIcons/Zip-icon.png";
                    break;
                }
            case "RAR":
                {
                    path = "./Images/FileIcons/Zip-icon.png";
                    break;
                }
            case "XML":
                {
                    path = "./Images/FileIcons/Document-xml-48.png";
                    break;
                }
            default:
                {
                    path = "./Images/FileIcons/attachment-icon.png";
                    break;
                }
        }
        return path;
    };
    AttatchmentIconPipe = __decorate([
        core_1.Pipe({ name: 'AttatchmentIconPipe' })
    ], AttatchmentIconPipe);
    return AttatchmentIconPipe;
}());
exports.AttatchmentIconPipe = AttatchmentIconPipe;
//# sourceMappingURL=AttatchmentIconPipe.js.map