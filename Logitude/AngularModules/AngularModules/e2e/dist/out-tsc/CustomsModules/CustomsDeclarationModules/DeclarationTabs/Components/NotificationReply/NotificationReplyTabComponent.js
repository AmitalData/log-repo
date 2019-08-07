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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var NotificationReplyPM_1 = require("../../../../../Customs/EntityPMs/NotificationReplyPM");
var EntityPMService_1 = require("../../../../../Infrastructure/Services/EntityPMService");
var MessageToAgentRequestParams_1 = require("../../../../../Customs/DataContract/RequestParams/MessageToAgentRequestParams");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var NotificationPMService_1 = require("../../../../../Customs/Services/StandardPMs/NotificationPMService");
var NotificationWebService_1 = require("../../../../../Customs/Services/WebServices/NotificationWebService");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var CustomMessageProgressComponent_1 = require("../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var NotificationReplyTabComponent = /** @class */ (function (_super) {
    __extends(NotificationReplyTabComponent, _super);
    function NotificationReplyTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.EntityPM = null;
        _this.ObjectTableName = "Customs.Declaration";
        _this.messageBorderText = "לא התקבלו הודעות מהמכס עבור הצהרה זו";
        _this.declarationPM = null;
        _this.messageBorderVisibility = true;
        //List<NotificationPM> notifications;
        _this.NotificationsList = [];
        _this.NotificationsGroupsList = [];
        _this.notificationWebService = new NotificationWebService_1.NotificationWebService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (!Tools_1.AppTool.IsNullOrEmpty(entityArgs)) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.Declaration").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.Notification").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.NotificationReply").subscribe(function (response) {
                        _this.declarationPM = entityArgs.EntityPM;
                        _this.Listen();
                        _this.LoadNotificationReplies();
                    });
                });
            });
        }
        return _this;
    }
    NotificationReplyTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.CurrentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "DCNT") {
                        _this.LoadNotificationReplies();
                    }
                }
            }));
        }
    };
    NotificationReplyTabComponent.prototype.LoadNotificationReplies = function () {
        var _this = this;
        var objecttable = window.ObjectTables.filter(function (d) { return d.Name == "Customs.Declaration"; })[0];
        this.notificationWebService.GetNotificationsByDefinitionCode(objecttable.Id, this.declarationPM.Id, this.declarationPM.Tenant)
            .subscribe(function (myResponse) {
            _this.GetNotificationsByDefinitionCodeOp_Completed(myResponse, false);
        });
    };
    NotificationReplyTabComponent.prototype.GetNotificationsByDefinitionCodeOp_Completed = function (myResponse, sourceIsCostomFile) {
        this.MessageBorderVisibility = true;
        if (myResponse.Result != null && myResponse.Result.length > 0) {
            this.MessageBorderVisibility = false;
            this.FillNotificationData(myResponse.Result);
        }
    };
    NotificationReplyTabComponent.prototype.FillNotificationData = function (notificationResponseList) {
        var _this = this;
        if (notificationResponseList != null) {
            //Group list by CreateDate
            this.NotificationsGroupsList = [];
            notificationResponseList.forEach(function (notificationItem) {
                var notificationsList = [];
                var listCreate = _this.NotificationsGroupsList.filter(function (item) { return item.CreateDate == notificationItem.CreateDate; });
                var myGroupCreate = null;
                if (!Tools_1.AppTool.IsNullOrEmpty(listCreate) && listCreate.length > 0) {
                    myGroupCreate = listCreate[0];
                }
                if (Tools_1.AppTool.IsNullOrEmpty(myGroupCreate)) {
                    myGroupCreate = new NotificationGroupHeaderViewModel(notificationItem, notificationsList);
                }
                myGroupCreate.NotificationsList.push(new DeclarationNotificationItemViewModel(notificationItem, _this));
                _this.NotificationsGroupsList.push(myGroupCreate);
            });
            //Order list by CreateDate
            this.NotificationsGroupsList.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.CreateDate) === Tools_1.DateTool.GetDateFromDate(b.CreateDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.CreateDate) < Tools_1.DateTool.GetDateFromDate(b.CreateDate)) ? -1 : 1; });
        }
    };
    Object.defineProperty(NotificationReplyTabComponent.prototype, "MessageBorderVisibility", {
        get: function () { return this.messageBorderVisibility; },
        set: function (newValue) { this.messageBorderVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NotificationReplyTabComponent.prototype, "MessageBorderText", {
        get: function () { return this.messageBorderText; },
        set: function (newValue) { this.messageBorderText = newValue; },
        enumerable: true,
        configurable: true
    });
    NotificationReplyTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NotificationReplyTabComponent.html',
            selector: 'NotificationReplyTabComponent',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], NotificationReplyTabComponent);
    return NotificationReplyTabComponent;
}(BaseComponent_1.BaseComponent));
exports.NotificationReplyTabComponent = NotificationReplyTabComponent;
var DeclarationNotificationItemViewModel = /** @class */ (function (_super) {
    __extends(DeclarationNotificationItemViewModel, _super);
    function DeclarationNotificationItemViewModel(notificationPM, trigger) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.DataContext = _this;
        _this.ResponseList = [];
        _this.withAnswerGridVisibility = false;
        _this.notificationPMService = new NotificationPMService_1.NotificationPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.entityPM = notificationPM;
        _this.parent = trigger;
        _this.NotificationData = _this.entityPM.Description; // + Environment.NewLine;
        if (_this.entityPM.NotificationRplies.length == 0) {
            _this.WithAnswerGridVisibility = false;
        }
        else {
            _this.WithAnswerGridVisibility = true;
            for (var _i = 0, _a = _this.entityPM.NotificationRplies; _i < _a.length; _i++) {
                var reply = _a[_i];
                _this.ResponseList.push(reply);
            }
            //Order list by CreateDate
            _this.ResponseList.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.ReplyDateTime) === Tools_1.DateTool.GetDateFromDate(b.ReplyDateTime)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.ReplyDateTime) < Tools_1.DateTool.GetDateFromDate(b.ReplyDateTime)) ? -1 : 1; });
        }
        return _this;
    }
    Object.defineProperty(DeclarationNotificationItemViewModel.prototype, "NotificationData", {
        get: function () { return this.notificationData; },
        set: function (newValue) { this.notificationData = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationNotificationItemViewModel.prototype, "NotificationReply", {
        get: function () { return this.notificationReply; },
        set: function (newValue) { this.notificationReply = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DeclarationNotificationItemViewModel.prototype, "WithAnswerGridVisibility", {
        get: function () { return this.withAnswerGridVisibility; },
        set: function (newValue) { this.withAnswerGridVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    DeclarationNotificationItemViewModel.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        if (Tools_1.AppTool.IsNullOrEmpty(this.NotificationReply)) {
            var validationErrors = [];
            validationErrors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.O.NoReplyEntered"));
            var windowArgs = {};
            windowArgs.Errors = validationErrors;
            windowArgs.ComponentHeight = '228px';
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 500;
            logWindow.Height = 300;
            logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.O.NotificationReplySendErrors");
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./CustomsModules/CustomControls/Components/CustomsErrorsComponent');
            return;
        }
        this.AddNewNotificationReplay(customSendOptionsArgs);
    };
    DeclarationNotificationItemViewModel.prototype.AddNewNotificationReplay = function (customSendOptionsArgs) {
        var _this = this;
        var newNotificationReplyPM = new NotificationReplyPM_1.NotificationReplyPM(this.entityPM);
        newNotificationReplyPM.NotificationId = this.entityPM.Id;
        newNotificationReplyPM.Tenant = this.entityPM.Tenant;
        newNotificationReplyPM.Line = (Tools_1.ArrayTool.Max(this.entityPM.NotificationRplies, "Line") + 1);
        newNotificationReplyPM.RepliedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        newNotificationReplyPM.ReplyDateTime = new Date();
        newNotificationReplyPM.ResponseToCustoms = this.NotificationReply;
        this.ResponseList.push(newNotificationReplyPM);
        this.entityPM.AddNotificationReply(newNotificationReplyPM);
        var entityPMService = new EntityPMService_1.EntityPMService();
        entityPMService.update("Customs.Notification", this.entityPM).then(function (res) {
            res.subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.CurrentSession.StopBusyIndicator();
                }
                else {
                    _this.SendNotificationReplay(customSendOptionsArgs);
                    _this.NotificationReply = null;
                }
            }, function (error) {
                _this.CurrentSession.StopBusyIndicator();
            });
        });
    };
    DeclarationNotificationItemViewModel.prototype.SendNotificationReplay = function (customSendOptionsArgs) {
        var currRequestParams = new MessageToAgentRequestParams_1.MessageToAgentRequestParams();
        currRequestParams.LoggingEnabled = true;
        currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
        currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        currRequestParams.NotificationId = this.entityPM.Id;
        currRequestParams.DeclarationId = this.parent.declarationPM.Id;
        CustomMessageProgressComponent_1.CustomMessageProgressComponent
            .ShowProgressBar(currRequestParams.PBId, "שליחת תשובה להודעה", true)
            .then(function (res) {
            //this.responseData = res;
            //this.OnMassageDisplayMethod();
        }).catch(function (err) {
            //this.ValidationErrorsList.push(err);
        });
        this.parent.notificationWebService.PostSendNotificationReplyRequest(currRequestParams)
            .subscribe(function (myServiceResponse) {
        });
        //this.parent.FillNotificationData();
        //NotificationData = null;
        //NotificationData = entityPM.Description + Environment.NewLine + entityPM.ResponseNotes + Environment.NewLine;
    };
    return DeclarationNotificationItemViewModel;
}(BaseComponent_1.BaseComponent));
exports.DeclarationNotificationItemViewModel = DeclarationNotificationItemViewModel;
var NotificationGroupHeaderViewModel = /** @class */ (function (_super) {
    __extends(NotificationGroupHeaderViewModel, _super);
    function NotificationGroupHeaderViewModel(notificationPM, notificationList) {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.DataContext = _this;
        _this.NotificationsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.entityPM = notificationPM;
        _this.NotificationsList = notificationList;
        return _this;
    }
    Object.defineProperty(NotificationGroupHeaderViewModel.prototype, "CreateDate", {
        get: function () { return this.entityPM.CreateDate; },
        set: function (newValue) { this.entityPM.CreateDate = newValue; },
        enumerable: true,
        configurable: true
    });
    return NotificationGroupHeaderViewModel;
}(BaseComponent_1.BaseComponent));
exports.NotificationGroupHeaderViewModel = NotificationGroupHeaderViewModel;
//# sourceMappingURL=NotificationReplyTabComponent.js.map