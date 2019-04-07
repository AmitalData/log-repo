declare var window: any;
import {OpportunityPM} from '../../EntityPMs/OpportunityPM';
import {MenuButtonPM} from '../../../Infrastructure/EntityPMs/MenuButtonPM'
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../Infrastructure/Utilities/FeatureLocator';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {OpportunityClosingReasonListService} from '../../Services/StandardLists/OpportunityClosingReasonListService';
import {OpportunityClosingReasonList} from '../../EntityLists/OpportunityClosingReasonList';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {CardListService} from '../../../Common/Services/StandardLists/CardListService';
import {CardList} from '../../../Common/EntityLists/CardList';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {TenantManagementPMService} from '../../../Infrastructure/Services/StandardPMs/TenantManagementPMService';
import {TenantManagementPM} from '../../../Infrastructure/EntityPMs/TenantManagementPM'; 
import {CustomerList} from '../../../Common/EntityLists/CustomerList';
import {CustomerListService} from '../../../Common/Services/StandardLists/CustomerListService';
import {ContactList} from '../../../Common/EntityLists/ContactList';
import {ContactListService} from '../../../Common/Services/StandardLists/ContactListService';
import {StageList} from '../../EntityLists/StageList';
import {StageListService} from '../../Services/StandardLists/StageListService';
import {OpportunityListService} from '../../Services/StandardLists/OpportunityListService';
import {OpportunityList} from '../../EntityLists/OpportunityList';
import {OpportunityArgs} from '../../Args'; 
import {OpportunityPMInitService} from '../../EntityPMInitServices/OpportunityPMInitService';
import {CreateTenantHelper} from '../../../InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantHelper';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

export class OpportunityMenuButtonsHandler {
    public EntityPM: OpportunityPM;
    public entityArgs: EntityArgs
    private status: boolean = false; 
    public DataContext: OpportunityMenuButtonsHandler = this;
    MenuButtonCode: string = null;
    isButtonClicked: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;

    StopFlags() {
        this.isButtonClicked = false;
        this.MenuButtonCode = null;
    }

    Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;

                    switch (this.MenuButtonCode) {
                        case "CloseAsWon": {
                            this.CloseWon_SaveCompleted();
                            break;
                        }             
                        case "CloseAsLost": {
                            this.CloseLost_SaveCompleted();
                            break;
                        }        
                        case "Copy": {
                            this.StartCopy_SaveCompleted();
                            break;
                        }

                        case "Cancel": {
                            this.CancelCommand_SaveCompleted();
                            break;
                        }
                       
                    }
                }

                this.StopFlags();
                this.CurrentSession.StopBusyIndicator();

            });

            this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                }

                this.StopFlags();
            });
        }
    }


    public SetEntityPM(entityArgs: EntityArgs) {
        this.entityArgs = entityArgs;
        this.EntityPM = entityArgs.EntityPM;
        this.Listen();
    }
    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('ClosingReasonId');
        this.myCloner.AddField('ClosingReasonCode');
        this.myCloner.AddField('ClosingDescription');
        this.myCloner.AddField('PostToFollowersAsWon');
        this.myCloner.AddField('ClosedToCompetitorId');
        this.myCloner.AddField('ActualClosingDate');   
        this.myCloner.AddField('StageId');  
        this.myCloner.AddField('IsClosed'); 
        this.myCloner.AddField('IsClosedLost');         
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }

    public CheckButtonState(menuButtons: MenuButtonPM[]) {
        if (this.EntityPM != null) {
            if (this.entityArgs.EditComponent != null) {

                for (var i = 0; i < menuButtons.length; i++) {
                    var button = menuButtons[i];
                    switch (button.EventCode) {

                        case "Cancel":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }

                        case "ReOpen":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }

                                else if (this.EntityPM.IsClosed) {
                                    button.IsDisabled = false;
                                }                             

                                else {
                                    button.IsDisabled = true;
                                }
                                break;
                            }

                        case "Copy":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }                             

                                else {
                                    button.IsDisabled = false;
                                }
                                break;
                            }
                        case "CloseAsWon":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }

                                else if (this.EntityPM.IsClosed) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }

                                break;
                            }

                        case "CloseAsLost":
                            {
                                if (this.EntityPM.IsCancelled) {
                                    button.IsDisabled = true;
                                }

                                else if (this.EntityPM.IsClosed) {
                                    button.IsDisabled = true;
                                }

                                else {
                                    button.IsDisabled = false;
                                }

                                break;
                            }


                        case "OpportunityTotango":
                            {                              
                                break;
                            }


                        case "OpportunityTenantManagement":
                            {
                                button.Width = 90;
                                break;
                            }

                        case "OpportunityCreateTenant":
                            {
                                button.Width = 120;

                                if (!FeatureLocator.HasFeaturePermession("Opportunity", "CREATETENANT")) {
                                    button.IsHidden = true;
                                }

                                else {
                                    button.IsHidden = false;
                                }

                                break;
                            }



                        case "Edit":
                            {
                                button.IsHidden = true;

                                if (!this.EntityPM.IsCancelled) {
                                    if (this.EntityPM.IsClosed) {
                                        button.IsHidden = false;
                                    }
                                }

                                break;
                            }
                    }
                }
            }
        }
    }
    public ValidationErrorsList: Array<string> = [];
    public confirmWindow: ConfirmWindow;
    ConfirmWindow_Unloaded() {
        this.ValidationErrorsList = [];
        if (this.confirmWindow.Yes) {           
            if (this.Validate()) {
                this.EntityPM.IsCancelled = true;
                this.CurrentSession.StartBusyIndicatorSaving();
                this.entityArgs.EditComponent.SaveChanges();

                }
        }
        this.StopFlags();
    }
    CancelCommand_SaveCompleted() {
        this.entityArgs.EditComponent.SaveChanges("Saving");                
    }

    CancelCommand() {
        var confirmMsg: string = "Are you sure you want to cancel this Opportunity ?";
        if (this.confirmWindow == null)
            this.confirmWindow = new ConfirmWindow();
        this.confirmWindow.Width = 400;
        this.confirmWindow.ShowCancelButton = false;
        this.confirmWindow.WindowClosed.subscribe(event => { this.ConfirmWindow_Unloaded(); });
        this.confirmWindow.Show(confirmMsg);

    }

    CloseWon_SaveCompleted() {
            var closingListService: OpportunityClosingReasonListService = new OpportunityClosingReasonListService();
            closingListService.getAllFromCache().subscribe(result => {
                var myClosingReason: OpportunityClosingReasonList = result.Result.filter(d => d.Code == "WN")[0];
                if (myClosingReason == null) {
                    var messageWindow = new MessageWindow();
                    messageWindow.Show("Error closing reason is not found");
                }
                else {
                    this.Clone();

                    var logWindow = new LogitudeWindow();
                    logWindow.Width = 500;
                    logWindow.Height = 300;
                    logWindow.Title = "Close As Won";
                    logWindow.WindowArgs = this.EntityPM;
                    logWindow.ComponentLoaded.subscribe(cmpRef => {
                        cmpRef.ClosingReasonId = myClosingReason.Id;
                    });
                    logWindow.WindowClosed.subscribe(event => {
                        if (event == "cancle") {
                            this.RejectChanges();
                        }
                        else {
                            OpportunityPMInitService.InitValues(this.EntityPM, false);
                        }
                    });
                    logWindow.Show('./CRM/Components/MenuButtons/CloseAsWonOrLostComponent');                   
                }

            });
          
          

        



    }
    Validate() {

        this.ValidationErrorsList = [];
        Validator.TryValidateObject(this.EntityPM, "Opportunity", this.ValidationErrorsList);
        if (this.ValidationErrorsList.length == 0) {
            this.entityArgs.EditComponent.ValidationErrorsList = this.ValidationErrorsList;
            return true;
        }
        else
            this.entityArgs.EditComponent.ValidationErrorsList = this.ValidationErrorsList;
        return false;
    }
    CloseAsWon() {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    }

    CloseAsLost() {
        this.entityArgs.EditComponent.SaveChanges("Saving");


    }

    CloseLost_SaveCompleted() {
        this.Clone();

        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 330;
        logWindow.Title = "Close As Lost";
        logWindow.WindowArgs = this.EntityPM;
        logWindow.ComponentLoaded.subscribe(cmpRef => {
            cmpRef.IsClosedLost = true;
        });
        logWindow.WindowClosed.subscribe(event => {
            if (event == "cancle") {
                this.RejectChanges();
            }
            else {
                OpportunityPMInitService.InitValues(this.EntityPM, false);
            }
        });
        logWindow.Show('./CRM/Components/MenuButtons/CloseAsWonOrLostComponent');
    }

    ReOpenOpoortunity() {
        this.Clone();
        this.EntityPM.StageId = null;
        this.EntityPM.IsClosed = false;
        this.EntityPM.ActualClosingDate = null;
        this.EntityPM.ClosingDescription = null;
        this.EntityPM.ClosingReasonId = null;
        this.EntityPM.EstimatedClosingDate = null;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 200;
        logWindow.Title = "Select Stage";
        logWindow.WindowArgs = this.EntityPM;
    
        logWindow.WindowClosed.subscribe(event => {
            if (event == "cancle") {
                this.RejectChanges();
            }
            else {

                OpportunityPMInitService.InitValues(this.EntityPM, false);
            }
            this.StopFlags();
        });
        logWindow.Show('./CRM/Components/MenuButtons/ReOpen_StageComponent');
    }

    private LoadCustomer(tag: string) {
    var cardService: CardListService = new CardListService();
    cardService.getSingle(this.EntityPM.CustomerId).subscribe(result => {
        var loadedCustomer: CardList = result.Result;
        this.EntityPM.CustomerExternalId = loadedCustomer.ReceivablesAccountingCard;
        if (loadedCustomer != null) {

            if (tag == "manage") {
                this.EditTenantManagement();
            }

            else if (tag == "totango") {
                this.GoTotango();
            }
        }
    });
 
    }
    GoTotango() {

        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerExternalId)) {      
            var link = "https://app.totango.com/#!/customerDetails?customer=" + this.EntityPM.CustomerExternalId;
            window.open(link, '_blank');
        }
        else {
            var message: MessageWindow = new MessageWindow();
            message.Width = 350;
            message.Height = 180;
            message.Show("Please Fill External Id Field");           
        }

        this.StopFlags();


    }
    EditTenantManagement() {
        if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerExternalId)) {
            var id: number = parseInt(this.EntityPM.CustomerExternalId);

            var managementService: TenantManagementPMService = new TenantManagementPMService();
            managementService.get(id).subscribe((myResponse: ServiceResponse) => {
                if (myResponse.HasError) {
                    var window: MessageWindow = new MessageWindow();
                    window.Width = 350;
                    window.Height = 180;
                    window.Show(myResponse.ErrorsArray[0]);
                }

                else {
                    var ten: TenantManagementPM = myResponse.Result;

                    if (ten != null) {
                        this.StartEditing(ten.Id);
                    }
                }
            });    
        }

        else {
            var window: MessageWindow = new MessageWindow();
            window.Width = 350;
            window.Height = 180;
            window.Show("Please Fill External Id Field");
            this.StopFlags();

        }

    }

    StartEditing(id) {
        var editWindow: LogitudeWindow = new LogitudeWindow();
        editWindow.IsFillScreen = true;
        editWindow.ShowHeaderButtons = true;
        editWindow.Title = "Edit Tenant Management";
        editWindow.IsEditComponent = true;
        editWindow.WindowClosed.subscribe(event => {
            this.StopFlags();
        });
        editWindow.ShowEditComponent(id, "TenantManagement", "", true);
    }



    CreateTenantMethod() {
        var confirmWindow: ConfirmWindow = new ConfirmWindow();
        confirmWindow.Title = "";
        confirmWindow.Width = 400;
        confirmWindow.Height = 200;
        confirmWindow.Show("Please confirm creating a new tenant for this customer ?");
        confirmWindow.WindowClosed.subscribe(event => {
            this.StopFlags();

            if (confirmWindow.Yes) {
                if (this.EntityPM.Field3 == null || (this.EntityPM.Field3 != null && AppTool.IsNullOrEmpty(this.EntityPM.Field3.Value))) {
                    if (!AppTool.IsNullOrEmpty(this.EntityPM.CustomerId)) {
                        var customerService: CustomerListService = new CustomerListService();
                        customerService.getSingle(this.EntityPM.CustomerId).subscribe(result => {
                            var customer: CardList = result.Result;
                            var createTenantHelper: CreateTenantHelper = new CreateTenantHelper("Opportunity", customer.Id, customer.EnglishName, customer.ReceivablesAccountingCard, customer.PrimaryContactId, customer.VatNumber, customer.CountryName, customer.CountryCode);
                            createTenantHelper.CreateTenantMethod();

                        });
                    }
                }

                else {
                    var messageWindow: MessageWindow = new MessageWindow();
                    var validationErrorMessage = "note that this customer has a tenant already " + this.EntityPM.Field3.Value + ", please erase the tenant# in order to create a new one" + " (" + this.EntityPM.Field3.Value + " = External ID)"
                    messageWindow.Show(validationErrorMessage);
                }
            }
        });
    }




    CopyOpportunity() {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    }
    StartCopy() {
        this.entityArgs.EditComponent.SaveChanges("Saving");
    }
    StartCopy_SaveCompleted() {
        var newOpportunity: OpportunityPM = new OpportunityPM()
        newOpportunity.Tenant = this.EntityPM.Tenant;
        newOpportunity.OwnerId = this.EntityPM.OwnerId;
        newOpportunity.Subject = this.EntityPM.Subject;
        newOpportunity.CustomerId = this.EntityPM.CustomerId;
        newOpportunity.LeadSourceId = this.EntityPM.LeadSourceId;
        newOpportunity.ContactId = this.EntityPM.ContactId;
        newOpportunity.CustomerName = this.EntityPM.CustomerName;
        newOpportunity.CustomerRankCode = this.EntityPM.CustomerRankCode;
        newOpportunity.CustomerRankName = this.EntityPM.CustomerRankName;
        newOpportunity.NumberOfShipments = this.EntityPM.NumberOfShipments;
        newOpportunity.OpportunityTypeId = this.EntityPM.OpportunityTypeId;
        newOpportunity.OwnerName = this.EntityPM.OwnerName;
        newOpportunity.RatingName = this.EntityPM.RatingName;
        newOpportunity.EstimatedClosingDate = this.EntityPM.EstimatedClosingDate;
        newOpportunity.ValueField = this.EntityPM.ValueField;
        newOpportunity.CreateDate = this.EntityPM.CreateDate;
        newOpportunity.CreatedByUserId= this.EntityPM.CreatedByUserId;
        newOpportunity.UpdateDate = this.EntityPM.UpdateDate;
        newOpportunity.UpdatedByUserId = this.EntityPM.UpdatedByUserId;
        newOpportunity.RatingCode = this.EntityPM.RatingCode;
        newOpportunity.IsCopy = true;
        newOpportunity.CopyFromEntityId = this.EntityPM.Id;
        newOpportunity.Notes = this.EntityPM.Notes;
        newOpportunity.BusinessUnitId = this.EntityPM.BusinessUnitId;
       
       var todayDateTime: Date = DateTool.GetCurrentDateTimeAsUtc();
       var stageService: StageListService = new StageListService();
       stageService.getAllFromCache().subscribe(result => {
           var myStage: StageList = result.Result.filter(d => d.Code == "QUA" && d.Tenant == SessionLocator.Tenant)[0];
           if (myStage != null) {
               newOpportunity.StageId = myStage.Id;
               newOpportunity.StageName = myStage.Name;
               newOpportunity.Probability = myStage.Probability;

               if (myStage.MaxDays != null) {
                   newOpportunity.StageDueDate = DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), parseFloat(myStage.MaxDays+""));
               }


               var logWindow = new LogitudeWindow();
               logWindow.Width = 850;
               logWindow.Height = 600;
               logWindow.Title = "Copy Opportunity";
               var args = new OpportunityArgs();
               args.Entity = newOpportunity;
               args.IsNew = false;
               logWindow.WindowArgs = args;
               logWindow.Show('./CRMModules/CRMOpportunity/Components/NewEntity/NewOpportunityComponent');

               logWindow.WindowClosed.subscribe(s => {
                   if (s) {
                       
                       SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                           .then(cmpRef => {
                               cmpRef.instance.ComponentRef = cmpRef;
                               cmpRef.instance.Run({ EntityId: s , ObjectTableName: 'Opportunity', BackButtonLabel: "Opportunity" });                              
                           });
                   }
               });


           }
       });
     

           

        
    }


    EditOpportunity() {

        this.Clone();
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Edit Opportunity";
        logWindow.WindowArgs = this.EntityPM;       
        logWindow.WindowClosed.subscribe(event => {
            if (event == "cancle") {
                this.RejectChanges();
            }
            this.StopFlags();
        });
        logWindow.Show('./CRM/Components/MenuButtons/EditClosedOpportunityComponent');                   
        

    }


    public MenuButtonClick(menuButton: MenuButtonPM) {
        if (!this.isButtonClicked) {

            this.StopFlags();
            this.isButtonClicked = true;
            this.MenuButtonCode = menuButton.EventCode;
            switch (menuButton.EventCode) {
                case "Cancel":
                    {
                        this.CancelCommand();
                        break;
                    }

                case "CloseAsWon":
                    {
                        this.CloseAsWon();
                        break;
                    }
                case "CloseAsLost":
                    {
                        this.CloseAsLost();
                        break;
                    }

                case "ReOpen":
                    {
                        this.ReOpenOpoortunity();
                        break;
                    }

                case "Copy":
                    {
                        this.CopyOpportunity();
                        break;
                    }


                case "OpportunityTenantManagement":
                    {
                        this.LoadCustomer("manage");
                        break;
                    }


                case "OpportunityTotango":
                    {
                        this.LoadCustomer("totango");
                        break;
                    }


                case "OpportunityCreateTenant":
                    {
                        this.CreateTenantMethod();
                        break;
                    }


                case "Edit":
                    {
                        this.EditOpportunity();
                        break;
                    }

            }
        }
    }

}

