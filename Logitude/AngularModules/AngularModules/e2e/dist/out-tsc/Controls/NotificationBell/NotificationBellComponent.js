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
var NotificationExtendedListService_1 = require("../../Customs/Services/ExtendedLists/NotificationExtendedListService");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var NotificationPM_1 = require("../../Customs/EntityPMs/NotificationPM");
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../Windows/LogitudeWindow");
var Guid_1 = require("../../Infrastructure/Utilities/Guid");
var NotificationWebService_1 = require("../../Customs/Services/WebServices/NotificationWebService");
var MessageWindow_1 = require("../Windows/MessageWindow");
var TextCodeTranslator_1 = require("../../Infrastructure/Utilities/TextCodeTranslator");
var CustomsCollateralPMService_1 = require("../../Customs/Services/StandardPMs/CustomsCollateralPMService");
var DeclarationExtendedListService_1 = require("../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var NotificationBellComponent = /** @class */ (function () {
    function NotificationBellComponent(entityResourceService) {
        var _this = this;
        this.entityResourceService = entityResourceService;
        this.LayoutDirection = 'ltr';
        this.notificationExtendedListService = new NotificationExtendedListService_1.NotificationExtendedListService();
        this.notificationWebService = new NotificationWebService_1.NotificationWebService();
        this.customsCollateralPMService = new CustomsCollateralPMService_1.CustomsCollateralPMService();
        this.declarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        this.DataContext = this;
        this.PreventSelect = false;
        entityResourceService.getEntityResourceByTableName("Customs.Notification").subscribe(function (response) {
            entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
                entityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(function (response) {
                    entityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe(function (response) {
                        entityResourceService.getEntityResourceByTableName("Customs.PaymentOrderProtestReason").subscribe(function (response) {
                            entityResourceService.getEntityResourceByTableName("Customs.CustomsSetting").subscribe(function (response) {
                                _this.IsVisibile = true;
                                _this.GetOpenNotificationsCount();
                                _this.GetNotifications();
                            });
                        });
                    });
                });
            });
        });
    }
    NotificationBellComponent.prototype.GetNotifications = function () {
        var _this = this;
        this.ItemsSource = [];
        this.Notifications = [];
        this.notificationExtendedListService.GetGetTopTenNotifications(SessionInfo_1.SessionInfo.LoggedUserId).subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.Notifications = response.Result;
                    _this.BuildList();
                    _this.notificationExtendedListService.PutNotificationBadjCount(new NotificationPM_1.NotificationPM()).subscribe(function (response) {
                    });
                }
                else {
                    var msg = new MessageWindow_1.MessageWindow();
                    var s = response.ErrorsArray;
                    msg.Show(s[0]);
                }
            }
        });
    };
    NotificationBellComponent.prototype.BuildList = function () {
        this.ItemsSource = [];
        for (var _i = 0, _a = this.Notifications.filter(function (d) { return !d.IsClosedByAssignee; }); _i < _a.length; _i++) {
            var item = _a[_i];
            this.ItemsSource.push(new NotificationBellLine(item, this));
        }
    };
    NotificationBellComponent.prototype.GetOpenNotificationsCount = function () {
        var _this = this;
        this.notificationExtendedListService.GetOpenNotificationsCount(SessionInfo_1.SessionInfo.LoggedUserId).subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.count = response.Result;
                }
                else {
                    var msg = new MessageWindow_1.MessageWindow();
                    var s = response.ErrorsArray;
                    msg.Show(s[0]);
                }
            }
        });
    };
    NotificationBellComponent.prototype.SelectedLine = function (item) {
        var _this = this;
        // this.CurrentSession.CloseNotificationBellEvent.emit({  });
        if (!this.PreventSelect) {
            var selected = item.entity;
            if (selected) {
                var customEditIdentityKey = Guid_1.Guid.newGuid();
                var control = null;
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                //logitudeWindow.ZIndex = 5;
                var currentScreenCode = "";
                switch (selected.ObjectTableName) {
                    case "Customs.Declaration":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "3050N":
                                case "3050C":
                                case "3050U":
                                case "3052P":
                                    {
                                        currentScreenCode = "DCPO";
                                        break;
                                    }
                                case "190N":
                                case "190U":
                                case "196E":
                                case "190C":
                                    {
                                        currentScreenCode = "DCPC";
                                        break;
                                    }
                                case "2470N":
                                case "2470C":
                                case "2470P":
                                case "5018N":
                                case "8400C":
                                case "5117N":
                                case "8400A":
                                case "5101C":
                                case "5101D":
                                case "5101G":
                                //case "5101I":
                                case "5101S":
                                case "5101T":
                                case "5101U":
                                case "5101B":
                                case "5107N":
                                case "2754N":
                                case "70N":
                                case "70C":
                                case "60A":
                                    {
                                        var tab = window.ObjectTableTabs.find(function (d) { return d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0; });
                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow_1.MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;
                                    }
                                case "5101N":
                                    {
                                        currentScreenCode = "DCNT";
                                        break;
                                    }
                                case "8215A":
                                case "8215D":
                                case "8215C":
                                    {
                                        currentScreenCode = "DCCA";
                                        break;
                                    }
                                case "8227N":
                                case "8227D":
                                case "8227A":
                                case "8228D":
                                case "8228A":
                                    {
                                        currentScreenCode = "DCCD";
                                        break;
                                    }
                                case "1812N":
                                case "1812U":
                                case "2020N":
                                case "2000N":
                                    {
                                        currentScreenCode = "DCTP";
                                        break;
                                    }
                                default:
                                    {
                                        var msg = new MessageWindow_1.MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Customs.PaymentOrder":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "3050N":
                                case "3050C":
                                case "3050U":
                                case "3052P":
                                    {
                                        var tab = window.ObjectTableTabs.find(function (d) { return d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0; });
                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow_1.MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        var msg = new MessageWindow_1.MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Customs.PhysicalCheck":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "190N":
                                case "190U":
                                case "196E":
                                case "190C":
                                    {
                                        var tab = window.ObjectTableTabs.find(function (d) { return d.ObjectTableId == selected.ObjectTableId && d.IndexOrder == 0; });
                                        if (tab) {
                                            currentScreenCode = tab.Code;
                                        }
                                        else {
                                            var msg = new MessageWindow_1.MessageWindow();
                                            //msg.ZIndex = 5;
                                            msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        var msg = new MessageWindow_1.MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Customs.CustomsCollateral":
                        {
                            switch (selected.NotificationDefinitionCode) {
                                case "8213N":
                                case "8211N":
                                case "8211U":
                                    {
                                        this.entityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                                            _this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral").subscribe(function (response) {
                                                _this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsAnswer").subscribe(function (response) {
                                                    _this.entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateralsCondition").subscribe(function (response) {
                                                        _this.customsCollateralPMService.get(selected.EntityId).subscribe(function (response) {
                                                            var result = response.Result;
                                                            console.log("[response] customsCollateralPMService.get", result);
                                                            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                                                                control = './CustomsModules/CustomsCollateral/Components/CustomsCollateralComponent';
                                                                logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.EditCustomsCollateral");
                                                                logitudeWindow.WindowArgs = { CurrentEntity: result };
                                                                logitudeWindow.Height = 730;
                                                                logitudeWindow.Width = 660;
                                                                //logitudeWindow.ZIndex = 5;
                                                                logitudeWindow.Show(control);
                                                                var Ids = [];
                                                                Ids.push(selected.Id);
                                                                Ids.push(selected.Id);
                                                                _this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe(function (res) {
                                                                    _this.SetStatusCompleted(selected.Id, event);
                                                                });
                                                            }
                                                            else {
                                                                console.log("No collateral found!!!!!!");
                                                                return;
                                                            }
                                                        });
                                                    });
                                                });
                                            });
                                        });
                                        break;
                                    }
                                default:
                                    {
                                        var msg = new MessageWindow_1.MessageWindow();
                                        //msg.ZIndex = 5;
                                        msg.Show("לא נמצאה ישות להצגה");
                                        break;
                                    }
                            }
                            break;
                        }
                    default:
                        {
                            if (!Tools_1.AppTool.IsNullOrEmpty(selected.Reference1Number)) {
                                this.declarationExtendedListService.GetSingleDeclarationByCustomFileNo(selected.Reference1Number).subscribe(function (res) {
                                    var declaration = res.Result;
                                    console.log("[reponse] GetSingleDeclarationByCustomFileNo: ", declaration);
                                    if (!Tools_1.AppTool.IsNullOrEmpty(declaration)) {
                                        selected.ObjectTableName = "Customs.Declaration";
                                        currentScreenCode = "DEGC";
                                        _this.EditEntity(selected.ObjectTableName, selected.EntityId, null, currentScreenCode);
                                        var Ids = [];
                                        Ids.push(selected.Id);
                                        Ids.push(selected.Id);
                                        _this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe(function (res) {
                                            _this.SetStatusCompleted(selected.Id, event);
                                        });
                                    }
                                });
                            }
                            else {
                                var msg = new MessageWindow_1.MessageWindow();
                                msg.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.WrongEntityName"));
                            }
                            break;
                        }
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(control)) {
                    if (selected.ObjectTableName == "Customs.ProceduralFault") {
                        logitudeWindow.Height = 400;
                        logitudeWindow.Width = 820;
                        logitudeWindow.ShowCloseButton = true;
                    }
                    else {
                        logitudeWindow.Height = 730;
                        logitudeWindow.Width = 660;
                    }
                    logitudeWindow.Show(control);
                    var Ids = [];
                    Ids.push(selected.Id);
                    Ids.push(selected.Id);
                    this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe(function (res) {
                        _this.SetStatusCompleted(selected.Id, event);
                    });
                }
                else {
                    if (!Tools_1.AppTool.IsNullOrEmpty(currentScreenCode)) {
                        this.EditEntity(selected.ObjectTableName, selected.EntityId, null, currentScreenCode);
                        var Ids = [];
                        Ids.push(selected.Id);
                        Ids.push(selected.Id);
                        this.notificationWebService.SetNotificationsStatus(Ids, "Read").subscribe(function (res) {
                            _this.SetStatusCompleted(selected.Id, event);
                        });
                    }
                }
            }
            this.ParentComponent.IsControlVisibile = false;
        }
        this.PreventSelect = false;
    };
    NotificationBellComponent.prototype.EditEntity = function (objectTableName, entityId, windowTitle, defaultSelectedTabCode) {
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = windowTitle;
        editWindow.Height = 770;
        editWindow.Width = 1500;
        editWindow.ShowEditComponent(entityId, objectTableName, defaultSelectedTabCode);
        editWindow.WindowClosed.subscribe(function (res) {
        });
    };
    NotificationBellComponent.prototype.SetStatusCompleted = function (id, $event) {
        //this.entityListService.getSingle(id, this.ObjectTableName).then((res: any) => {
        //    //var re = res;
        //    res.subscribe((aa: any) => {
        //        $event.BackFromEdit.emit({ Data: aa.Result, rowIndex: $event.rowIndex });
        //    })
        //});
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], NotificationBellComponent.prototype, "ParentComponent", void 0);
    NotificationBellComponent = __decorate([
        core_1.Component({
            selector: 'NotificationBellComponent',
            moduleId: module.id,
            templateUrl: './NotificationBellComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], NotificationBellComponent);
    return NotificationBellComponent;
}());
exports.NotificationBellComponent = NotificationBellComponent;
var NotificationBellLine = /** @class */ (function () {
    function NotificationBellLine(entityPM, parent) {
        this.notificationExtendedListService = new NotificationExtendedListService_1.NotificationExtendedListService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.entity = entityPM;
        this.Parent = parent;
        if (this.entity.IsSeenByAssignee) {
            this.Background = "#F1F1F1";
        }
        else {
            this.Background = "white";
            this.IsSeenFontWeight = "bold";
        }
        if (this.entity.AssigneToNotificationTypeCode == "A") {
            this.IconeVisibility = true;
            this.BlueIconeVisibility = false;
        }
        else {
            this.IconeVisibility = false;
        }
        if (this.entity.NotificationDefinitionCode == "5101N") {
            this.BlueIconeVisibility = true;
            this.IconeVisibility = false;
        }
        var valueDate = new Date(this.entity.DueDate.valueOf()).valueOf();
        var today = Tools_1.DateTool.GetCurrentDateAsUtc().valueOf();
        if (valueDate != null && valueDate < today) {
            this.datecolor = "#ff6a00";
            this.fontcolor = "#ffffff";
        }
        else {
            this.datecolor = "#E2E2E2";
            this.fontcolor = "#6E7172";
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.entity.CustomerName) && !Tools_1.AppTool.IsNullOrEmpty(this.entity.Reference1Number)) {
            this.Reference1NumberWithCustomer = entityPM.Reference1Number + " * " + entityPM.CustomerName;
        }
        else if (Tools_1.AppTool.IsNullOrEmpty(this.entity.CustomerName) && !Tools_1.AppTool.IsNullOrEmpty(this.entity.Reference1Number)) {
            this.Reference1NumberWithCustomer = entityPM.Reference1Number;
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.CustomerName) && Tools_1.AppTool.IsNullOrEmpty(entityPM.Reference1Number)) {
            this.Reference1NumberWithCustomer = entityPM.CustomerName;
        }
    }
    Object.defineProperty(NotificationBellLine.prototype, "NotificationDefinitionName", {
        get: function () { return this.entity.NotificationDefinitionName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NotificationBellLine.prototype, "DueDate", {
        get: function () { return this.entity.DueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NotificationBellLine.prototype, "Description", {
        get: function () { return this.entity.Description; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NotificationBellLine.prototype, "CreateDate", {
        get: function () { return this.entity.CreateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NotificationBellLine.prototype, "NotificationDefinitionCode", {
        get: function () { return this.entity.NotificationDefinitionCode; },
        enumerable: true,
        configurable: true
    });
    NotificationBellLine.prototype.ClosedByAssigneeClicked = function () {
        var _this = this;
        this.Parent.count -= 1;
        this.Parent.PreventSelect = true;
        this.Parent.ParentComponent.IsControlVisibile = true;
        this.entity.IsClosedByAssignee = true;
        this.Parent.BuildList();
        this.notificationExtendedListService.PutNotificationsStatus(this.entity).subscribe(function (response) {
            if (response) {
                if (!response.HasError) {
                    _this.Parent.PreventSelect = false;
                }
            }
        });
    };
    return NotificationBellLine;
}());
exports.NotificationBellLine = NotificationBellLine;
//# sourceMappingURL=NotificationBellComponent.js.map