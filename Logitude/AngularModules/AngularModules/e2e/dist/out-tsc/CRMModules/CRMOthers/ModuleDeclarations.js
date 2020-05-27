"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewSLAComponent_1 = require("./Components/SLA/NewSLAComponent");
var SLAMainWindowComponent_1 = require("./Components/SLA/SLAMainWindowComponent");
var AddEditEscalationComponent_1 = require("./Components/SLA/AddEditEscalationComponent");
var BlockedCustomerComponent_1 = require("./Components/BlockedCustomer/BlockedCustomerComponent");
var NewBusinessHourAndHolidaysComponent_1 = require("./Components/BusinessHour/NewBusinessHourAndHolidaysComponent");
var AddEditBusinessHourHolidayComponent_1 = require("./Components/BusinessHour/AddEditBusinessHourHolidayComponent");
var TicketSettingsComponent_1 = require("./Components/TicketSettings/TicketSettingsComponent");
//Questionnaire
var AddEditQuestionnaireComponent_1 = require("./Components/Questionnaire/AddEditQuestionnaireComponent");
var AddEditQuestionnaireQuestionComponent_1 = require("./Components/Questionnaire/AddEditQuestionnaireQuestionComponent");
var QuestionnaireAnswersComponent_1 = require("./Components/Questionnaire/QuestionnaireAnswersComponent");
var AddEditPickListComponent_1 = require("./Components/Questionnaire/AddEditPickListComponent");
exports.Components = [
    NewSLAComponent_1.NewSLAComponent,
    SLAMainWindowComponent_1.SLAMainWindowComponent,
    AddEditEscalationComponent_1.AddEditEscalationComponent,
    BlockedCustomerComponent_1.BlockedCustomerComponent,
    NewBusinessHourAndHolidaysComponent_1.NewBusinessHourAndHolidaysComponent,
    AddEditBusinessHourHolidayComponent_1.AddEditBusinessHourHolidayComponent,
    TicketSettingsComponent_1.TicketSettingsComponent,
    AddEditQuestionnaireComponent_1.AddEditQuestionnaireComponent,
    AddEditQuestionnaireQuestionComponent_1.AddEditQuestionnaireQuestionComponent,
    QuestionnaireAnswersComponent_1.QuestionnaireAnswersComponent,
    AddEditPickListComponent_1.AddEditPickListComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewSLAComponent": {
                myResult = NewSLAComponent_1.NewSLAComponent;
                break;
            }
            case "SLAMainWindowComponent": {
                myResult = SLAMainWindowComponent_1.SLAMainWindowComponent;
                break;
            }
            case "NewBusinessHourAndHolidaysComponent": {
                myResult = NewBusinessHourAndHolidaysComponent_1.NewBusinessHourAndHolidaysComponent;
                break;
            }
            case "AddEditBusinessHourHolidayComponent": {
                myResult = AddEditBusinessHourHolidayComponent_1.AddEditBusinessHourHolidayComponent;
                break;
            }
            case "AddEditEscalationComponent": {
                myResult = AddEditEscalationComponent_1.AddEditEscalationComponent;
                break;
            }
            case "BlockedCustomerComponent": {
                myResult = BlockedCustomerComponent_1.BlockedCustomerComponent;
                break;
            }
            case "TicketSettingsComponent": {
                myResult = TicketSettingsComponent_1.TicketSettingsComponent;
                break;
            }
            case "AddEditQuestionnaireComponent": {
                myResult = AddEditQuestionnaireComponent_1.AddEditQuestionnaireComponent;
                break;
            }
            case "AddEditQuestionnaireQuestionComponent": {
                myResult = AddEditQuestionnaireQuestionComponent_1.AddEditQuestionnaireQuestionComponent;
                break;
            }
            case "QuestionnaireAnswersComponent": {
                myResult = QuestionnaireAnswersComponent_1.QuestionnaireAnswersComponent;
                break;
            }
            case "AddEditPickListComponent": {
                myResult = AddEditPickListComponent_1.AddEditPickListComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map