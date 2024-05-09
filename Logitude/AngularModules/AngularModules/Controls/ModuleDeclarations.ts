
// Directives
import { ChildDirective } from './Directives/ChildDirective';

// Pipes
import {CountryFlagPipe} from './Pipes/CountryFlagPipe';
import {DateTimeToDatePipe} from  './Pipes/DateTimeToDatePipe';
import {DateTimePipe} from './Pipes/DateTimePipe';
import {TextCodeTranslationPipe} from './Pipes/TextCodeTranslationPipe';
import {IdGeneratorPipe} from './Pipes/IdGeneratorPipe';
import {IdGeneratorAsyncPipe} from './Pipes/IdGeneratorAsyncPipe';

import { DateToMonthPipe } from './Pipes/DateToMonthPipe';
import { MinutesToTimePipe } from './Pipes/MinutesToTimePipe';
import {ObjectFieldTextCodeTranslationPipe} from './Pipes/ObjectFieldTextCodeTranslationPipe';
 
// All
import {AccessLevelButton} from './All/AccessLevelButton';
import {CheckBox} from './All/CheckBox';
import {RadioButton} from './All/RadioButton';
import {BackButton} from './All/BackButton';
import {BusyIndicator} from './All/BusyIndicator';
import {Hyperlink} from './All/Hyperlink';
import {HyperlinkQuery} from './All/HyperlinkQuery';
import {ScrollViewer} from './All/ScrollViewer';
import {SectionBox, SectionHead, SectionBody} from './All/SectionBox';
import {TabSummary} from './All/TabSummary'
import {MettingSummary} from './All/MettingSummary';
import {WarningSummary} from './All/WarningSummary';
import {ValidationSummary} from './All/ValidationSummary';
import {SendButton} from './All/SendButton';
import {SplitButtonComponent} from './All/SplitButtonComponent';

//import {ClassificationsTree} from './All/ClassificationsTree';
import {Image} from './All/Image';
//import {TimeInput} from './All/TimeInput';

// Popups
import {HelpIcon} from './Popups/HelpIcon';
import {CellTooltip} from './Popups/CellTooltip';
import {SalesNotes} from './Popups/SalesNotes';
import {HelperNotes} from './Popups/HelperNotes';
import {QuickSearchTextBox} from './Popups/QuickSearchTextBox';
import {EmailSearchTextBox} from './Popups/EmailSearchTextBox';
import {ToggleButton, ToggleButtonItem} from './Popups/ToggleButton';

import {AddressTemplate} from './Templates/AddressTemplate';
import {GoogleMapsButton} from './Templates/GoogleMapsButton';

import {ComboBox} from './ComboBox';
import {IconButton} from './IconButton';
import {SearchTextBox} from './SearchTextBox';
import {ContactDatePicker} from './ContactDatePicker';
import {DirectionsFilter} from './DirectionsFilter';
import {TransportsFilter} from './TransportsFilter';
import {ActivitiesFilter} from './ActivitiesFilter';
import {LocationsFilter} from  './LocationsFilter';
import {DatesFilter} from './DatesFilter';

import {ConfirmWindowTemplateComponent} from './Windows/ConfirmWindow';
import {MessageWindowTemplateComponent} from './Windows/MessageWindow';
import {LogitudeWindowTemplateComponent} from './Windows/LogitudeWindow';
import {ShipmentArchiveFilter} from './ShipmentArchiveFilter';
import {CurrencyFilter} from './CurrencyFilter';
import {KeyControl} from './KeyControl';
import {ComboBoxWithInCheckBox} from './ComboBoxWithInCheckBox';
import {ShipmentTypeFilter} from './ShipmentTypeFilter';
import {ApplicationLockIndicator} from './ApplicationLockIndicator';
import {BooleanFilter} from './BooleanFilter';
import { NotificationBellComponent } from './NotificationBell/NotificationBellComponent';
import {UserFilter} from './UserFilter';
import {ConnectToFilter} from './ConnectToFilter';
import { ParticipatedFilter } from './ParticipatedFilter';
import { InvitedFilter } from './InvitedFilter';
import { LogitudeHotKeysComponent } from './LogitudeHotkeysComponent/LogitudeHotKeysComponent';
import {  NumberInputComponent } from './All/NumberInput';
import {  LogChipsComponent } from './All/LogChips';

export const Directives =
    [
        ChildDirective,
    ];

export const Pipes =
    [
        CountryFlagPipe,
        DateTimeToDatePipe,
        DateTimePipe,
        TextCodeTranslationPipe,
        IdGeneratorPipe,
        IdGeneratorAsyncPipe,
        DateToMonthPipe,
        MinutesToTimePipe,
        ObjectFieldTextCodeTranslationPipe,
    ];

export const Components =
    [
        // All
        AccessLevelButton,
        CheckBox,
        RadioButton,
        BackButton,
        MettingSummary,
        BusyIndicator,
        CellTooltip,
        Hyperlink,
        HyperlinkQuery,
        ScrollViewer,
        SectionBox,
        SectionHead,
        SectionBody,
        TabSummary,
        SendButton,
        SplitButtonComponent,
        //ClassificationsTree,
        CellTooltip,
        SalesNotes,
        HelperNotes,
        QuickSearchTextBox,
        EmailSearchTextBox,
        AddressTemplate,
        GoogleMapsButton,
       // TimeInput,
        NumberInputComponent,
        LogChipsComponent,

        ComboBox,
        HelpIcon,
        //SearchBox,
        IconButton,
        SearchTextBox,

        ContactDatePicker,
        DirectionsFilter,
        TransportsFilter,
        ActivitiesFilter,
        WarningSummary,
        ValidationSummary,
        ConfirmWindowTemplateComponent,
        MessageWindowTemplateComponent,
        LogitudeWindowTemplateComponent,
        ShipmentArchiveFilter,
        CurrencyFilter,
        KeyControl,
        ComboBoxWithInCheckBox,
        ShipmentTypeFilter,
        ApplicationLockIndicator,
        BooleanFilter,
        NotificationBellComponent,
        Image,
        LocationsFilter,
        DatesFilter,
        ToggleButton,
        ToggleButtonItem,
        UserFilter,
        ConnectToFilter,
        ParticipatedFilter,
        InvitedFilter,
        LogitudeHotKeysComponent ,
    ];

export class ModuleDeclarations {
    public static Get(name: string) {

        var myResult: any = null;

        switch (name) {
            case "ConfirmWindowTemplateComponent": { myResult = ConfirmWindowTemplateComponent; break; }
            case "MessageWindowTemplateComponent": { myResult = MessageWindowTemplateComponent; break; }
            case "LogitudeWindowTemplateComponent": { myResult = LogitudeWindowTemplateComponent; break; }
        }

        return myResult;
    }
}
