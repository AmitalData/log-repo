"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// Pipes
var CountryFlagPipe_1 = require("./Pipes/CountryFlagPipe");
var DateTimeToDatePipe_1 = require("./Pipes/DateTimeToDatePipe");
var DateTimePipe_1 = require("./Pipes/DateTimePipe");
var TextCodeTranslationPipe_1 = require("./Pipes/TextCodeTranslationPipe");
var IdGeneratorPipe_1 = require("./Pipes/IdGeneratorPipe");
var DateToMonthPipe_1 = require("./Pipes/DateToMonthPipe");
var MinutesToTimePipe_1 = require("./Pipes/MinutesToTimePipe");
// All
var AccessLevelButton_1 = require("./All/AccessLevelButton");
var CheckBox_1 = require("./All/CheckBox");
var RadioButton_1 = require("./All/RadioButton");
var BackButton_1 = require("./All/BackButton");
var BusyIndicator_1 = require("./All/BusyIndicator");
var Hyperlink_1 = require("./All/Hyperlink");
var HyperlinkQuery_1 = require("./All/HyperlinkQuery");
var ScrollViewer_1 = require("./All/ScrollViewer");
var SectionBox_1 = require("./All/SectionBox");
var TabSummary_1 = require("./All/TabSummary");
var MettingSummary_1 = require("./All/MettingSummary");
var WarningSummary_1 = require("./All/WarningSummary");
var ValidationSummary_1 = require("./All/ValidationSummary");
var SendButton_1 = require("./All/SendButton");
var SplitButtonComponent_1 = require("./All/SplitButtonComponent");
//import {ClassificationsTree} from './All/ClassificationsTree';
var Image_1 = require("./All/Image");
//import {TimeInput} from './All/TimeInput';
// Popups
var HelpIcon_1 = require("./Popups/HelpIcon");
var CellTooltip_1 = require("./Popups/CellTooltip");
var SalesNotes_1 = require("./Popups/SalesNotes");
var HelperNotes_1 = require("./Popups/HelperNotes");
var QuickSearchTextBox_1 = require("./Popups/QuickSearchTextBox");
var EmailSearchTextBox_1 = require("./Popups/EmailSearchTextBox");
var ToggleButton_1 = require("./Popups/ToggleButton");
var AddressTemplate_1 = require("./Templates/AddressTemplate");
var GoogleMapsButton_1 = require("./Templates/GoogleMapsButton");
var ComboBox_1 = require("./ComboBox");
var IconButton_1 = require("./IconButton");
var SearchTextBox_1 = require("./SearchTextBox");
var ContactDatePicker_1 = require("./ContactDatePicker");
var DirectionsFilter_1 = require("./DirectionsFilter");
var TransportsFilter_1 = require("./TransportsFilter");
var ActivitiesFilter_1 = require("./ActivitiesFilter");
var LocationsFilter_1 = require("./LocationsFilter");
var DatesFilter_1 = require("./DatesFilter");
var ConfirmWindow_1 = require("./Windows/ConfirmWindow");
var MessageWindow_1 = require("./Windows/MessageWindow");
var LogitudeWindow_1 = require("./Windows/LogitudeWindow");
var ShipmentArchiveFilter_1 = require("./ShipmentArchiveFilter");
var CurrencyFilter_1 = require("./CurrencyFilter");
var KeyControl_1 = require("./KeyControl");
var ComboBoxWithInCheckBox_1 = require("./ComboBoxWithInCheckBox");
var ShipmentTypeFilter_1 = require("./ShipmentTypeFilter");
var ApplicationLockIndicator_1 = require("./ApplicationLockIndicator");
var BooleanFilter_1 = require("./BooleanFilter");
var NotificationBellComponent_1 = require("./NotificationBell/NotificationBellComponent");
var UserFilter_1 = require("./UserFilter");
var ConnectToFilter_1 = require("./ConnectToFilter");
exports.Pipes = [
    CountryFlagPipe_1.CountryFlagPipe,
    DateTimeToDatePipe_1.DateTimeToDatePipe,
    DateTimePipe_1.DateTimePipe,
    TextCodeTranslationPipe_1.TextCodeTranslationPipe,
    IdGeneratorPipe_1.IdGeneratorPipe,
    DateToMonthPipe_1.DateToMonthPipe,
    MinutesToTimePipe_1.MinutesToTimePipe,
];
exports.Components = [
    // All
    AccessLevelButton_1.AccessLevelButton,
    CheckBox_1.CheckBox,
    RadioButton_1.RadioButton,
    BackButton_1.BackButton,
    MettingSummary_1.MettingSummary,
    BusyIndicator_1.BusyIndicator,
    CellTooltip_1.CellTooltip,
    Hyperlink_1.Hyperlink,
    HyperlinkQuery_1.HyperlinkQuery,
    ScrollViewer_1.ScrollViewer,
    SectionBox_1.SectionBox,
    SectionBox_1.SectionHead,
    SectionBox_1.SectionBody,
    TabSummary_1.TabSummary,
    SendButton_1.SendButton,
    SplitButtonComponent_1.SplitButtonComponent,
    //ClassificationsTree,
    CellTooltip_1.CellTooltip,
    SalesNotes_1.SalesNotes,
    HelperNotes_1.HelperNotes,
    QuickSearchTextBox_1.QuickSearchTextBox,
    EmailSearchTextBox_1.EmailSearchTextBox,
    AddressTemplate_1.AddressTemplate,
    GoogleMapsButton_1.GoogleMapsButton,
    // TimeInput,
    ComboBox_1.ComboBox,
    HelpIcon_1.HelpIcon,
    //SearchBox,
    IconButton_1.IconButton,
    SearchTextBox_1.SearchTextBox,
    ContactDatePicker_1.ContactDatePicker,
    DirectionsFilter_1.DirectionsFilter,
    TransportsFilter_1.TransportsFilter,
    ActivitiesFilter_1.ActivitiesFilter,
    WarningSummary_1.WarningSummary,
    ValidationSummary_1.ValidationSummary,
    ConfirmWindow_1.ConfirmWindowTemplateComponent,
    MessageWindow_1.MessageWindowTemplateComponent,
    LogitudeWindow_1.LogitudeWindowTemplateComponent,
    ShipmentArchiveFilter_1.ShipmentArchiveFilter,
    CurrencyFilter_1.CurrencyFilter,
    KeyControl_1.KeyControl,
    ComboBoxWithInCheckBox_1.ComboBoxWithInCheckBox,
    ShipmentTypeFilter_1.ShipmentTypeFilter,
    ApplicationLockIndicator_1.ApplicationLockIndicator,
    BooleanFilter_1.BooleanFilter,
    NotificationBellComponent_1.NotificationBellComponent,
    Image_1.Image,
    LocationsFilter_1.LocationsFilter,
    DatesFilter_1.DatesFilter,
    ToggleButton_1.ToggleButton,
    ToggleButton_1.ToggleButtonItem,
    UserFilter_1.UserFilter,
    ConnectToFilter_1.ConnectToFilter,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "ConfirmWindowTemplateComponent": {
                myResult = ConfirmWindow_1.ConfirmWindowTemplateComponent;
                break;
            }
            case "MessageWindowTemplateComponent": {
                myResult = MessageWindow_1.MessageWindowTemplateComponent;
                break;
            }
            case "LogitudeWindowTemplateComponent": {
                myResult = LogitudeWindow_1.LogitudeWindowTemplateComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map