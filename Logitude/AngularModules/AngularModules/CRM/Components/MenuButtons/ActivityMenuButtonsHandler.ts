declare var window: any;
import {ActivityPM} from '../../EntityPMs/ActivityPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ActivityStatusListService} from '../../Services/StandardLists/ActivityStatusListService';
import {ActivityValidator}  from '../../Validators/ActivityValidator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {DateTool} from '../../../Infrastructure/Tools'; 
import {ActivityInviteePM} from '../../EntityPMs/ActivityInviteePM'; 
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ActivityInputArgs} from '../../Args';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';

export class ActivityMenuButtonsHandler {
    public EntityPM: ActivityPM;
    public entityArgs: EntityArgs
    private status: boolean = false;
    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.InitializeServices();
        this.Listen();
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {
                var table = window.ObjectTables.filter(d => d.Name === 'Ticket')[0];

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "Cancel":
                            {
                                if (this.EntityPM.ActivityStatusCode == "X") {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }

                        case "MarkAsComplete":
                            {
                                if (this.EntityPM.ActivityStatusCode == "X") {
                                    button.IsDisabled = true;
                                }

                                else if (this.EntityPM.ActivityTypeCode == "EO" || this.EntityPM.ActivityTypeCode == "EI") {
                                    button.IsDisabled = true;
                                }

                                else if (!this.EntityPM.IsOpen) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }

                        case "ReOpen":
                            {
                                if (this.EntityPM.ActivityStatusCode == "X") {
                                    button.IsDisabled = true;
                                }

                                else if (this.EntityPM.ActivityTypeCode == "EO" || this.EntityPM.ActivityTypeCode == "EI") {
                                    button.IsDisabled = true;
                                }

                                else if (this.EntityPM.IsOpen) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "Copy":
                            {
                                if (this.EntityPM.ActivityStatusCode == "X") {
                                    button.IsDisabled = true;
                                }
                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                    }
                }
            }
        }
    }
    public MenuButtonClick(menuButton: MenuButtonPM) {
        switch (menuButton.EventCode) {
            case "Cancel":
                {
                    this.CancelActivity();
                    break;
                }

            case "MarkAsComplete":
                {
                    this.isCompleteActivity = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
            case "ReOpen":
                {
                    this.ReOpenActivity();
                    break;
                }

            case "Copy":
                {
                    this.isCopyActivity = true;
                    this.entityArgs.EditComponent.SaveChanges();
                    break;
                }
        }
    }

    private ActivityStatusListService: ActivityStatusListService;
    InitializeServices() {
        this.ActivityStatusListService = new ActivityStatusListService();
    }

    isValid: boolean = false;    
    isCompleteActivity: boolean = false;
    isCopyActivity: boolean = false;
    StopFlags() {
        this.isCompleteActivity = false;
        this.isCopyActivity = false;
    }
    Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    if (this.isCompleteActivity) {
                        this.CompleteActivity();
                    }
                    if (this.isCopyActivity) {
                        this.CopyActivity();
                    }
                }
                this.StopFlags();
            });
        }

        this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
            if (isLoadSuccess) {
                this.EntityPM = this.entityArgs.EditComponent.EntityPM;
            }
            this.StopFlags();
        });
    }
    Validate() {
        var validator = new ActivityValidator();
        var errors: string[] = validator.Validate(this.EntityPM);

        this.isValid = errors.length == 0 ? true : false;
        this.entityArgs.EditComponent.ValidationErrorsList = errors;

        if (!this.isValid) {
            this.StopFlags();
        }
    }

    CancelActivity() {
        var confirmMsg = "Are you sure you want to cancel this activity?";

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Width = 400;
        confirmWindow.Show(confirmMsg);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.Validate();
                if (this.isValid) {
                    this.CompleteSave('X');
                }
            }
        });
    }
    CompleteActivity() {
        this.CompleteSave('C');
    }
    ReOpenActivity() {
        this.Validate();
        if (this.isValid) {
            this.CompleteSave('N');
        }
    }
    CopyActivity() {
        var newActivity = new ActivityPM();
        newActivity.Tenant = this.EntityPM.Tenant;
        newActivity.ActivityTypeCode = this.EntityPM.ActivityTypeCode;
        newActivity.ActivityTypeName = this.EntityPM.ActivityTypeName;
        newActivity.Subject = this.EntityPM.Subject;
        newActivity.OwnerId = this.EntityPM.OwnerId;
        newActivity.OwnerName = this.EntityPM.OwnerName;
        newActivity.CreateDate = DateTool.GetCurrentDateAsUtc();
        newActivity.CreatedByUserId = SessionLocator.LoggedUserId;
        newActivity.UpdateDate = DateTool.GetCurrentDateAsUtc();
        newActivity.UpdatedByUserId = SessionLocator.LoggedUserId;;
        newActivity.IsOpen = true;
        newActivity.ActivityStatusCode = "N";
        newActivity.PriorityCode = this.EntityPM.PriorityCode;
        newActivity.ActivityStatusName = this.EntityPM.ActivityStatusName;
        newActivity.BranchId = this.EntityPM.BranchId;
        newActivity.AllDayEvent = this.EntityPM.AllDayEvent;
        newActivity.IsLeftVoiceMail = this.EntityPM.IsLeftVoiceMail;
        newActivity.NeedSynchronization = this.EntityPM.NeedSynchronization;
        newActivity.BusinessUnitId = this.EntityPM.BusinessUnitId;
        newActivity.PriorityName = this.EntityPM.PriorityName;
        newActivity.Description = this.EntityPM.Description;
        newActivity.CustomerId = this.EntityPM.CustomerId;
        newActivity.CustomerName = this.EntityPM.CustomerName;
        newActivity.OpportunityId = this.EntityPM.OpportunityId;
        newActivity.QuoteId = this.EntityPM.QuoteId;
        newActivity.Notes = this.EntityPM.Notes;
        newActivity.IsCopy = true;
        newActivity.OriginalActivitySubject = this.EntityPM.Subject;
        //appointment fields
        newActivity.Location = this.EntityPM.ActivityTypeCode == "AP" ? this.EntityPM.Location : null;
        newActivity.ActivityTimeTypeCode = this.EntityPM.ActivityTypeCode == "AP" ? this.EntityPM.ActivityTimeTypeCode : null;
        newActivity.Duration = this.EntityPM.ActivityTypeCode == "AP" ? this.EntityPM.Duration : null;
        //phone fields
        newActivity.CallTypeCode = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallTypeCode : null;
        newActivity.CallPurpose = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallPurpose : null;
        newActivity.CallDetails = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallDetails : null;
        newActivity.CallResult = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallResult : null;
        newActivity.CallWithId = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.CallWithId : null;
        newActivity.PhoneNumber = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.PhoneNumber : null;
        newActivity.SenderEmail = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.SenderEmail : null;
        newActivity.SenderContactId = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.SenderContactId : null;
        newActivity.SenderContactName = this.EntityPM.ActivityTypeCode == "CL" ? this.EntityPM.SenderContactName : null;

        if (newActivity.ActivityTypeCode == "AP") {
            this.EntityPM.ActivityInvitees.forEach(item => {
                var activity = new ActivityInviteePM(null);
                activity.ContactId = item.ContactId;
                activity.ContactName = item.ContactName;
                activity.Tenant = newActivity.Tenant;
                activity.Email = item.Email;
                activity.IsRequired = item.IsRequired;
                newActivity.ActivityInvitees.push(activity);
            });
        }

        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow();
        switch (newActivity.ActivityTypeCode) {
            case "TS":
                {
                    windowTitle = "Copy Task";
                    windowTitleIcon = "./Images/Activities/TS.png";
                    //logWindow.ShowActivityTitle = true;
                    //logWindow.ActivityTypeCode = "TS";
                    //logWindow.IsCopyCase = true;
                    break;
                }

            case "CL": {
                windowTitle = "Copy Phone Call";
                windowTitleIcon = "./Images/Activities/CL.png";
                break;
            }

            case "AP": {
                windowTitle = "Copy Appointment";
                windowTitleIcon = "./Images/Activities/AP.png";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }

            default: { break; }
        }

        var windowArgs: ActivityInputArgs = new ActivityInputArgs();
        windowArgs.TypeCode = newActivity.ActivityTypeCode;
        windowArgs.IsAddCustomerAllowed = true;
        windowArgs.Activity = newActivity;

        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
    }
    CompleteSave(type:string) {
        this.EntityPM.IsOpen = true;
        this.EntityPM.ActivityStatusCode = type;
        this.ActivityStatusListService.getAllFromCache().subscribe((resp: any) => {
            if (!resp.HasError) {
                var list = resp.Result;
                var activity = list.filter(d => d.Code == this.EntityPM.ActivityStatusCode)[0];
                if (activity != null) {
                    this.EntityPM.ActivityStatusName = activity.Name;
                }

                this.entityArgs.EditComponent.SaveChanges(TextCodeTranslator.Translate("General.M.Saving"));
            }
        });
    }

}