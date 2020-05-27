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
var Tools_1 = require("../../Infrastructure/Tools");
var AccessLevelButton = /** @class */ (function () {
    function AccessLevelButton() {
        this.isDown = false;
        this.AccessLevelChanged = new core_1.EventEmitter();
        this.accessLevelCode = null;
        this.accessLevelName = null;
        this.mySource = null;
        this.isEnabled = true;
    }
    AccessLevelButton.prototype.ngOnInit = function () {
        if (this.Feature) {
            this.AccessLevelCode = this.Feature.AccessLevelCode;
            this.isDown = this.AccessLevelCode == "OR" ? true : false;
            this.SetSource();
        }
    };
    AccessLevelButton.prototype.SetSource = function () {
        if (this.Feature) {
            var levelCode = this.AccessLevelCode;
            if (Tools_1.AppTool.IsNullOrEmpty(levelCode)) {
                levelCode = "NO";
            }
            this.Source = "./_Resources/Images/Icons/AccessLevels/" + levelCode + ".png";
        }
    };
    Object.defineProperty(AccessLevelButton.prototype, "AccessLevelCode", {
        get: function () { return this.accessLevelCode; },
        set: function (value) {
            if (this.Feature != null) {
                if (this.accessLevelCode != value) {
                    this.accessLevelCode = value;
                    this.SetSource();
                    switch (value) {
                        case "OR": {
                            this.AccessLevelName = "Organization";
                            break;
                        }
                        case "PR": {
                            this.AccessLevelName = "Parent";
                            break;
                        }
                        case "BU": {
                            this.AccessLevelName = "Business Unit";
                            break;
                        }
                        case "US": {
                            this.AccessLevelName = "User";
                            break;
                        }
                        default: {
                            this.AccessLevelName = "None";
                            break;
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccessLevelButton.prototype, "AccessLevelName", {
        get: function () { return this.accessLevelName; },
        set: function (value) {
            if (this.Feature != null) {
                if (this.accessLevelName != value) {
                    this.accessLevelName = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccessLevelButton.prototype, "Source", {
        get: function () { return this.mySource; },
        set: function (value) {
            if (this.mySource != value) {
                this.mySource = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AccessLevelButton.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (value) {
            if (this.isEnabled != value) {
                this.isEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    AccessLevelButton.prototype.ButtonClicked = function () {
        if (this.Feature) {
            var levelCode = this.AccessLevelCode;
            if (Tools_1.AppTool.IsNullOrEmpty(levelCode)) {
                levelCode = "NO";
            }
            if (this.Feature.IsBusinessUnitEnabled == false) {
                if (Tools_1.AppTool.IsNullOrEmpty(levelCode) || levelCode == "NO") {
                    levelCode = "OR";
                }
                else {
                    levelCode = "NO";
                }
            }
            else {
                if (this.isDown) {
                    switch (levelCode) {
                        case "OR": {
                            levelCode = "PR";
                            break;
                        }
                        case "PR": {
                            levelCode = "BU";
                            break;
                        }
                        case "BU": {
                            levelCode = "US";
                            break;
                        }
                        case "US": {
                            levelCode = "NO";
                            this.isDown = false;
                            break;
                        }
                    }
                }
                else {
                    switch (levelCode) {
                        case "NO": {
                            levelCode = "US";
                            break;
                        }
                        case "US": {
                            levelCode = "BU";
                            break;
                        }
                        case "BU": {
                            levelCode = "PR";
                            break;
                        }
                        case "PR": {
                            levelCode = "OR";
                            this.isDown = true;
                            break;
                        }
                    }
                }
            }
            if (this.AccessLevelCode != levelCode) {
                this.AccessLevelCode = levelCode;
                this.AccessLevelChanged.emit(levelCode);
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], AccessLevelButton.prototype, "AccessLevelChanged", void 0);
    AccessLevelButton = __decorate([
        core_1.Component({
            selector: 'AccessLevelButton',
            inputs: ['IsEnabled', 'Feature', 'AccessLevelCode'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <div class=\"MediaFill\">\n        <button class=\"AccessLevelButton CenterCenter\" [disabled]=\"!IsEnabled\" tabindex=\"-1\" *ngIf=\"Feature\" (click)=\"ButtonClicked()\" [attr.title]=\"AccessLevelName\">\n            <img [attr.src]=\"Source\" style=\"visibility: inherit; vertical-align: middle; display: block;\" />\n        </button> \n    </div>\n    ",
            styles: ["\n    .AccessLevelButton:disabled {\n        opacity: 0.5;\n        cursor: default;        \n    }\n\n    .AccessLevelButton {\n        height: 16px !important;\n        width: 16px !important;        \n        background: none !important;\n        border: none !important;\n        padding: 0 !important;        \n        cursor: pointer;        \n    }\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], AccessLevelButton);
    return AccessLevelButton;
}());
exports.AccessLevelButton = AccessLevelButton;
//# sourceMappingURL=AccessLevelButton.js.map