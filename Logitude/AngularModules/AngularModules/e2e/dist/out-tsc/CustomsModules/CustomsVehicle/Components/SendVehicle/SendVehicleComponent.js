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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var UpdateDeleteVehicleRequestParams_1 = require("../../../../Customs/DataContract/RequestParams/UpdateDeleteVehicleRequestParams");
var CustomMessageProgressComponent_1 = require("../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent");
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var IIGGeneralMessagesService_1 = require("../../../../Customs/Services/WebServices/IIGGeneralMessagesService");
var SendVehicleComponent = /** @class */ (function () {
    function SendVehicleComponent(entityPMService) {
        this.entityPMService = entityPMService;
        //------------------------------------------------------//
        this.ObjectTableName = "Customs.Vehicle";
    }
    SendVehicleComponent_1 = SendVehicleComponent;
    SendVehicleComponent.prototype.Run = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
    };
    SendVehicleComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        SendVehicleComponent_1.SaveEntityChanges(customSendOptionsArgs, this.EntityPM, false);
    };
    SendVehicleComponent.SaveEntityChanges = function (customSendOptionsArgs, EntityPM, isDelete) {
        var _this = this;
        EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        //this.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(EntityPM.Id)) {
            //this.CancelButtonClicked();
            return;
        }
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        //if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //this._totangoService.SendTotangoUserActivity(this.ObjectTableName, "New " + this.ObjectTableName);
        //this.CurrentSession.CurrentEditComponent.SaveChanges()
        var entityPMService = new EntityPMService_1.EntityPMService();
        entityPMService.update("Customs.Vehicle", EntityPM).then(function (res) {
            res.subscribe(function (myResponse) {
                _this.CurrentSession.StopBusyIndicator();
                if (myResponse.HasError) {
                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myResponse.ErrorsArray;
                    //this.SaveCompleted.emit(false);
                }
                else {
                    EntityPM = myResponse.Result;
                    if (Tools_1.AppTool.IsNullOrEmpty(EntityPM.Id)) {
                        var myErrors = [];
                        myErrors.push("this.EntityPM.Id is null");
                        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myErrors;
                    }
                    else {
                        if (customSendOptionsArgs == null) {
                            //this.CancelButtonClicked();
                        }
                        else {
                            var currRequestParams = new UpdateDeleteVehicleRequestParams_1.UpdateDeleteVehicleRequestParams(); ///Force new GUID On Each Send !!
                            currRequestParams.LoggingEnabled = true;
                            currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                            currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
                            currRequestParams.VehicleId = EntityPM.Id;
                            currRequestParams.IsDelete = isDelete;
                            CustomMessageProgressComponent_1.CustomMessageProgressComponent
                                .ShowProgressBar(currRequestParams.PBId, "שליחת מסר עדכון פרטי רכב", false)
                                .then(function (res) {
                                console.log(res);
                                //this.CancelButtonClicked();
                            }).catch(function (err) {
                                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(err);
                                //this.CancelButtonClicked();
                            });
                            var myIIGGeneralMessagesService = new IIGGeneralMessagesService_1.IIGGeneralMessagesService();
                            myIIGGeneralMessagesService.PostVehicleRequest(currRequestParams)
                                .subscribe(function (myServiceResponse) {
                                //this.CurrentSession.StopBusyIndicator();
                                //this.ResponseData = myServiceResponse.Result;
                                //this.OnMassageDisplayMethod();
                            });
                        }
                    }
                }
            }, function (error) {
                _this.CurrentSession.StopBusyIndicator();
                var myErrors = [];
                myErrors.push(error.message);
                _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = myErrors;
            });
        });
        //}
    };
    var SendVehicleComponent_1;
    SendVehicleComponent.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    SendVehicleComponent = SendVehicleComponent_1 = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SendVehicleComponent',
            templateUrl: "SendVehicleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityPMService_1.EntityPMService])
    ], SendVehicleComponent);
    return SendVehicleComponent;
}());
exports.SendVehicleComponent = SendVehicleComponent;
//# sourceMappingURL=SendVehicleComponent.js.map