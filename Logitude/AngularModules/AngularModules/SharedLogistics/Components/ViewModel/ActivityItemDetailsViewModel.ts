
declare var System: any;
declare var window: any;
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {EventTypePM} from '../../../Infrastructure/EntityPMs/EventTypePM';
import {CardLogActivityDetails} from '../../DataContracts/CardLogActivityDetails';
import {CardLogDetails} from '../../DataContracts/CardLogDetails';
import {ActivityZoomItemViewModel} from './ActivityZoomItemViewModel';
export class ActivityItemDetailsViewModel {

    public CardName: string;
    public Activity: string;
    public ContactName: string;
    public Module: string;
    public GMTLogDateTime: Date;


    Details: CardLogActivityDetails;
    ActivityZoomItem: ActivityZoomItemViewModel;
    constructor(details: CardLogActivityDetails, activity: ActivityZoomItemViewModel) {
        this.Details = details;
        this.ActivityZoomItem = activity;


        this.CardName = activity.CardName
        this.ContactName = activity.ContactName;

        this.Module = details.Module;
        this.GMTLogDateTime = details.GMTLogDateTime;
        this.Activity = details.Activity;
    }


















}