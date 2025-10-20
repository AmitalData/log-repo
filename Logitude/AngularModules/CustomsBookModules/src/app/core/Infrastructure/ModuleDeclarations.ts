// Directives
import { FocusMeDirective } from './Utilities/FocusMeDirective';
import { LocationDirective } from './Utilities/LocationDirective';
import { FixedPositionDirective } from './Utilities/FixedPositionDirective';
import { ChildDirective } from './Directives/ChildDirective';

// Pipes.
import { NumbersPipe } from './Pipes/NumbersPipe';
import { PaddingPipe } from './Pipes/PaddingPipe';
import { ReplacePipe } from './Pipes/ReplacePipe';
import { HighlightSearch } from './Pipes/HighlightSearch';
import { StringToColorPipe } from './Pipes/StringToColorPipe';
import { DateTimeToTimePipe } from './Pipes/DateTimeToTimePipe';
import { AttatchmentIconPipe } from './Pipes/AttatchmentIconPipe';
import { GroupByPipe } from './Pipes/GroupByPipe';
import { MenuButtonsItemsPipe } from './Pipes/MenuButtonsItemsPipe';
import { StageAgePipe } from './Pipes/StageAgePipe';
import { DateTimeToColorPipe } from './Pipes/DateTimePipes/DateTimeToColorPipe';
import { DateTimeToBackgroundPipe } from './Pipes/DateTimeToBackgroundPipe';
import { ExchangeRateDatePipe } from './Pipes/ExchangeRateDatePipe';
import { LogBoxStatusForegroundPipe } from './Pipes/LogBoxStatusForegroundPipe';
import { InvoiceDueDateForegroundPipe } from './Pipes/InvoiceDueDateForegroundPipe';
import { CustomNumbersPipe } from './Pipes/CustomNumbersPipe';
import { RatesNumbersPipe } from './Pipes/RatesNumbersPipe';
import { DateTimeToMSDYDatePipe } from './Pipes/DateTimeToMSDYDatePipe';
import { FollowUpDatePipe } from './Pipes/FollowUpDatePipe';
import { DateTimeToShortDatePipe } from './Pipes/DateTimeToShortDatePipe';
import { SafePipe } from './Pipes/SafePipe';
import { LogBoxStatusDatePipe } from './Pipes/LogBoxStatusDatePipe';
import { TimeToHoursMinutesPipe } from './Pipes/TimeToHoursMinutesPipe';
import { CustomFieldResolverPipe } from './Pipes/CustomFieldResolverPipe';
import { TextCodeTranslationPipe } from './Pipes/TextCodeTranslationPipe';


export const Directives = [
    FocusMeDirective,
    LocationDirective,
    FixedPositionDirective,
    ChildDirective,
];

export const Pipes = [
    NumbersPipe,
    PaddingPipe,
    ReplacePipe,
    HighlightSearch,
    StringToColorPipe,
    DateTimeToTimePipe,
    AttatchmentIconPipe,
    GroupByPipe,
    MenuButtonsItemsPipe,
    StageAgePipe,
    DateTimeToColorPipe,
    DateTimeToBackgroundPipe,
    ExchangeRateDatePipe,
    LogBoxStatusForegroundPipe,
    InvoiceDueDateForegroundPipe,
    RatesNumbersPipe,
    DateTimeToMSDYDatePipe,
    CustomNumbersPipe,
    FollowUpDatePipe,
    DateTimeToShortDatePipe,
    SafePipe,
    LogBoxStatusDatePipe,
    TimeToHoursMinutesPipe,
    CustomFieldResolverPipe,
    TextCodeTranslationPipe
];
