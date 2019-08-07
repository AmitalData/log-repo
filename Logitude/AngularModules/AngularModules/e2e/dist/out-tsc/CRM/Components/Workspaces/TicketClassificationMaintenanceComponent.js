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
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TicketClassificationPM_1 = require("../../EntityPMs/TicketClassificationPM");
var TicketClassificationPMService_1 = require("../../Services/StandardPMs/TicketClassificationPMService");
var TicketClassificationListService_1 = require("../../Services/StandardLists/TicketClassificationListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var TicketClassificationMaintenanceComponent = /** @class */ (function (_super) {
    __extends(TicketClassificationMaintenanceComponent, _super);
    function TicketClassificationMaintenanceComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "TicketClassification";
        _this.DataContext = _this;
        _this.Classifications = [];
        _this.IsVisible = false;
        _this.collapseAll = false;
        _this.Classifications = [];
        return _this;
    }
    TicketClassificationMaintenanceComponent.prototype.Run = function () {
        //this.BuildTreeView();
    };
    //Classifications Tree
    TicketClassificationMaintenanceComponent.prototype.BuildTreeView = function () {
        var _this = this;
        this.Classifications = [];
        var service = new TicketClassificationListService_1.TicketClassificationListService();
        service.getAllFromCache().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var allClassifications = myResponse.Result;
                allClassifications.filter(function (d) { return d.ParentId == null; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
                    _this.Classifications.push(new ClassificationData(item, allClassifications, item.Name, true, _this));
                });
                _this.IsVisible = true;
            }
        });
    };
    Object.defineProperty(TicketClassificationMaintenanceComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
                if (value == null) {
                    this.ClassificationTreePath = null;
                }
                else {
                    this.ClassificationTreePath = value.FullName;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketClassificationMaintenanceComponent.prototype, "ClassificationTreePath", {
        get: function () { return this.classificationTreePath; },
        set: function (value) {
            this.classificationTreePath = value;
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    TicketClassificationMaintenanceComponent.prototype.BackButtonClicked = function () {
        if (this.ComponentRef != null) {
            this.ComponentRef.destroy();
            this.ComponentRef = null;
        }
    };
    Object.defineProperty(TicketClassificationMaintenanceComponent.prototype, "CollapseAll", {
        get: function () {
            return this.collapseAll;
        },
        set: function (value) {
            if (this.collapseAll != value) {
                this.collapseAll = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TicketClassificationMaintenanceComponent.prototype.CollapseAllClicked = function () {
        this.CollapseAll = true;
    };
    TicketClassificationMaintenanceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TicketClassificationMaintenanceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TicketClassificationMaintenanceComponent);
    return TicketClassificationMaintenanceComponent;
}(BaseComponent_1.BaseComponent));
exports.TicketClassificationMaintenanceComponent = TicketClassificationMaintenanceComponent;
var ClassificationData = /** @class */ (function (_super) {
    __extends(ClassificationData, _super);
    function ClassificationData(entity, allClassifications, fullName, Isexpanded, trigger) {
        var _this = _super.call(this) || this;
        _this.ChildClassifications = [];
        _this.ObjectTableName = "TicketClassification";
        _this.DataContext = _this;
        _this.isExpanded = false;
        _this.expandAllOrNot = true;
        _this.isSelected = false;
        _this.entity = entity;
        _this.ChildClassifications = [];
        _this.FullName = fullName;
        _this.trigger = trigger;
        allClassifications.filter(function (d) { return d.ParentId == entity.Id; }).sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; }).forEach(function (item) {
            var name = fullName + "/" + item.Name;
            _this.ChildClassifications.push(new ClassificationData(item, allClassifications, name, false, trigger));
        });
        _this.IsExpanded = Isexpanded;
        return _this;
    }
    Object.defineProperty(ClassificationData.prototype, "Id", {
        //Properties
        get: function () { return this.entity.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "Name", {
        get: function () { return this.entity.Name; },
        set: function (value) { this.entity.Name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "ParentId", {
        get: function () { return this.entity.ParentId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "Inactive", {
        get: function () { return this.entity.Inactive; },
        set: function (value) { this.entity.Inactive = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "DefaultSeverityId", {
        get: function () { return this.entity.DefaultSeverityId; },
        set: function (value) { this.entity.DefaultSeverityId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "EmployeeGroupId", {
        get: function () { return this.entity.EmployeeGroupId; },
        set: function (value) { this.entity.EmployeeGroupId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "ManagerUserId", {
        get: function () { return this.entity.ManagerUserId; },
        set: function (value) { this.entity.ManagerUserId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "IsExpanded", {
        get: function () { return this.isExpanded; },
        set: function (value) {
            this.isExpanded = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "ExpandAllOrNot", {
        get: function () { return this.expandAllOrNot; },
        set: function (value) {
            this.expandAllOrNot = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationData.prototype, "IsSelected", {
        get: function () { return this.isSelected; },
        set: function (value) {
            this.isSelected = value;
        },
        enumerable: true,
        configurable: true
    });
    // Commands 
    ClassificationData.prototype.AddNewClassificationChild = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = "New Classification";
        var context = new ClassificationChildArgs(this.Id, null, true, this);
        logitudeWindow.DataContext = context;
        logitudeWindow.Show('./CRM/Components/Workspaces/AddEditClassificationComponent');
    };
    ClassificationData.prototype.EditClassificationChild = function () {
        var _this = this;
        var service = new TicketClassificationPMService_1.TicketClassificationPMService();
        service.get(this.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var editEntityPM = myResponse.Result;
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = "Edit Classification";
                var context = new ClassificationChildArgs(editEntityPM.ParentId, editEntityPM, false, _this);
                logitudeWindow.DataContext = context;
                logitudeWindow.Show('./CRM/Components/Workspaces/AddEditClassificationComponent');
            }
        });
    };
    return ClassificationData;
}(BaseComponent_1.BaseComponent));
exports.ClassificationData = ClassificationData;
var ClassificationChildArgs = /** @class */ (function (_super) {
    __extends(ClassificationChildArgs, _super);
    function ClassificationChildArgs(Id, entityPM, isNew, trigger) {
        var _this = _super.call(this) || this;
        _this.trigger = trigger;
        _this.ObjectTableName = "TicketClassification";
        _this.DataContext = _this;
        _this.isNew = isNew;
        if (isNew) {
            _this.entityPM = new TicketClassificationPM_1.TicketClassificationPM();
            _this.entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        }
        else {
            _this.entityPM = entityPM;
            //this.FillUsersList();
        }
        _this.ParentId = Id;
        return _this;
    }
    Object.defineProperty(ClassificationChildArgs.prototype, "Name", {
        //Properties
        get: function () { return this.entityPM.Name; },
        set: function (value) {
            this.entityPM.Name = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "Inactive", {
        get: function () { return this.entityPM.Inactive; },
        set: function (value) {
            this.entityPM.Inactive = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "EmployeeGroupId", {
        get: function () { return this.entityPM.EmployeeGroupId; },
        set: function (value) {
            if (this.entityPM.EmployeeGroupId != value) {
                this.entityPM.EmployeeGroupId = value;
                this.SetUIRequiredProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "DefaultSeverityId", {
        get: function () { return this.entityPM.DefaultSeverityId; },
        set: function (value) {
            if (this.entityPM.DefaultSeverityId != value) {
                this.entityPM.DefaultSeverityId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "ManagerUserId", {
        get: function () { return this.entityPM.ManagerUserId; },
        set: function (value) {
            if (this.entityPM.ManagerUserId != value) {
                this.entityPM.ManagerUserId = value;
                this.SetUIRequiredProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "EscalationNotify", {
        get: function () { return this.entityPM.EscalationNotify; },
        set: function (value) {
            if (this.entityPM.EscalationNotify != value) {
                this.entityPM.EscalationNotify = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ClassificationChildArgs.prototype, "ParentId", {
        get: function () { return this.entityPM.ParentId; },
        set: function (value) {
            if (this.entityPM.ParentId != value) {
                this.entityPM.ParentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ClassificationChildArgs.prototype.SetUIRequiredProperties = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.Name) && this.Name == "General") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EmployeeGroupId))
                this.UIProperties.SetRequired("EmployeeGroupId", this.ObjectTableName, true);
        }
    };
    return ClassificationChildArgs;
}(BaseComponent_1.BaseComponent));
exports.ClassificationChildArgs = ClassificationChildArgs;
//# sourceMappingURL=TicketClassificationMaintenanceComponent.js.map