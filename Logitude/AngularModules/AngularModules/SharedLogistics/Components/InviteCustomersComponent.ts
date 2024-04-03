import { Component, OnInit,OnDestroy } from '@angular/core';
import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { Guid } from '../../Infrastructure/Utilities/Guid';
import { SharedLogisticContactService } from '../Services/ExtendedPMs/SharedLogisticContactService';
import { CustomerList } from '../../Common/EntityLists/CustomerList';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { SessionInfo } from '../../Infrastructure/Utilities/SessionInfo';
import { SharedLogisticContactPM } from '../../Common/EntityPMs/SharedLogisticContactPM'
import { CustomerLineViewModel } from './ViewModel/CustomerLineViewModel';
import { ContactInputTemplateArgs } from '../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import { LogitudeWindow } from '../../Controls/Windows/LogitudeWindow';
import { ContactItemClass } from '../../CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent';
import { MessageWindow } from '../../Controls/Windows/MessageWindow';
import { ContactPM } from '../../Common/EntityPMs/ContactPM';
import { EntityResourceService } from '../../Infrastructure/Services/EntityResourceService';
import { DocumentTypeTemplatePMExtendedService } from 'Common/Services/ExtendedPMs/DocumentTypeTemplatePMExtendedService';
import { DocumentTypeTemplatePM } from 'Common/EntityPMs/DocumentTypeTemplatePM';
import { GeneralEmailSender } from '../../Infrastructure/Helpers/GeneralEmailSender';
import { AppTool } from '../../Infrastructure/Tools';
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';

@Component({

    selector: 'InviteCustomersComponent',
    templateUrl: './InviteCustomersComponent.html',
    //inputs: ['PartnerTypeId', , 'DateParameter', 'DataContext', 'OnCloseWindowEvent'],
    providers: [SharedLogisticContactService],
})
export class InviteCustomersComponent implements OnInit, OnDestroy {

    CurrentEntity: any;
    IsCargoTrackingMenuClicked: boolean;
    NoContactsVisibility: boolean;
    public SharedLogisticCustomerLineList: CustomerLineViewModel[];
    IsDigitalPortal: boolean = false;
    CustomerName: string;
    CustomerCode: string;
    InvitationStatus: string;
    IsLoginToOnlineVisibility: boolean = false;
    IsFromCustomerEdit: boolean = false;

    DocumentTypeTemplates: DocumentTypeTemplatePM[];
    CanChangeTemplate: boolean = false;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    public documentTypeTemplatePMExtendedService: DocumentTypeTemplatePMExtendedService;
    private HTMLTemplate: string;
    private ToEmail: string;
    private Subject: string;
    private Cc: string;
    private Bcc: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedLogisticContactService: SharedLogisticContactService) {
        this.documentTypeTemplatePMExtendedService = new DocumentTypeTemplatePMExtendedService();
        this.CurrentSession.StartBusyIndicatorLoading(); 
        this.Listen();
    }

    private SendToCustomerEvent: any = null;
    private SessionEvent: any = null;
    Listen() {
        if (!this.SendToCustomerEvent) {
            this.SendToCustomerEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s.Name == "DigitalPortalHTMLTemplate") {
                    this.HTMLTemplate = s.htmlString;
                    this.ToEmail = s.To;
                    this.Subject = s.Subject;
                    this.Cc = s.Cc;
                    this.Bcc = s.Bcc;
                    this.SendInvitaion(null);
                }
            });
        }
    }

    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SendToCustomerEvent);
        AppTool.KillEventEmitter(this.SessionEvent);
    }

    ngOnInit() {

    }

    LoadData() {

        this.SharedLogisticCustomerLineList = [];
        this._sharedLogisticContactService.getSharedLogisticContactsbyCardId(this.CurrentEntity.Id, SessionInfo.LoggedUserTenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();

            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                result.forEach((item) => {
                    this.SharedLogisticCustomerLineList.push(new CustomerLineViewModel(item, this));

                });


                if (this.SharedLogisticCustomerLineList.length == 0) this.NoContactsVisibility = true;
                else this.NoContactsVisibility = false;
            }
        });

        if (!this.IsCargoTrackingMenuClicked) {
            this.GetDocumentTemplates();
        }
    }

    GetDocumentTemplates() {
        this.DocumentTypeTemplates = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.documentTypeTemplatePMExtendedService.GetDocumentTypeTemplatesForDocumentTypeCode("SLCIN", "M", this.CurrentEntity.Tenant).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();
            if (pmResponse.HasError) {
                return;
            }
            if (!pmResponse.Result) {
                return;
            }
            this.SetDocumentTypeTemplates(pmResponse.Result);
        });

    }

    private SetDocumentTypeTemplates(result) {
        this.DocumentTypeTemplates = result;
        this.CanChangeTemplate = this.DocumentTypeTemplates.length > 1;
    }

    sharedLogisticContact: SharedLogisticContactPM;
    haveInternetAccess:boolean;
    SaveChanges(item: SharedLogisticContactPM, haveInternetAccess) {
        this.haveInternetAccess = haveInternetAccess;
        this.sharedLogisticContact = item;

        if (this.IsDigitalPortal == true &&  haveInternetAccess) {
            this.SendDigitaPortalDocument();
        }
        else {
            if (this.CanChangeTemplate && haveInternetAccess) {
                this.ShowTemplateTypePicker();
                return;
            }
            this.SendInvitaion(null);
        }
    }

    GeneralEmailSender: GeneralEmailSender;
    SendDigitaPortalDocument() {
        var eventRefreshName =  "DigitalPortalHTMLTemplate";
        if (!this.GeneralEmailSender || (this.GeneralEmailSender && !this.GeneralEmailSender.LoadingSendingComponent)) {
            this.GeneralEmailSender = new GeneralEmailSender("SharedLogistics", "SLCIN", null, null, this.CurrentEntity.Id, "", "", null, null, eventRefreshName, null, false, null, null, this.sharedLogisticContact.Email, this.IsDigitalPortal);
            this.GeneralEmailSender.ShowFullSendControll();
        }
    }
    
    ShowTemplateTypePicker() {
        var windowArgs: any = {};
        windowArgs.DataViewModel = this;
        windowArgs.DocumentTypeTemplates = this.DocumentTypeTemplates;

        var logWindow = new LogitudeWindow();
        logWindow.Width = 500;
        logWindow.Height = 200;
        logWindow.Title = "Invitation Message Template";

        logWindow.WindowArgs = windowArgs;
        logWindow.Show("./SharedLogistics/Components/TemplateTypeComponent");
    }


    private SendInvitaion(templateId) {
        this.sharedLogisticContact.IsDigitalPortal = this.IsDigitalPortal;
        this.sharedLogisticContact.InternetAccess = this.haveInternetAccess;
        this.sharedLogisticContact.IsCargoTrackingInvitation = this.IsCargoTrackingMenuClicked;
        this.sharedLogisticContact.TemplateId = templateId;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

        if (this.IsDigitalPortal) {
            this.sharedLogisticContact.HTMLTemplate = this.HTMLTemplate;
            this.sharedLogisticContact.ToEmail = this.ToEmail;
            this.sharedLogisticContact.Subject = this.Subject;
            this.sharedLogisticContact.Cc = this.Cc;
            this.sharedLogisticContact.Bcc = this.Bcc;
        }

        this._sharedLogisticContactService.ContactInternetAccessInvitation(this.sharedLogisticContact).subscribe((res: any) => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    if (this.sharedLogisticContact.InternetAccess) {
                        this.ShowMessageWindow("Invitation email sent to " + "\" " + this.sharedLogisticContact.EnglishName + " \"" + " with temporary password.", "Send Invitation", "gray", 150, true);
                    }
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    this.ShowMessage(pmResponse.ErrorsArray[0], "Logitude Message");
                }
            }
        });
    }

    public ShowMessage(message: string, title: string = "") {
        var messageWindow: MessageWindow = new MessageWindow();
        messageWindow.Show(message);

        if (title) {
            messageWindow.Title = title;
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


    ShowMessageWindow(message: string, title: string, textColor: string, windowheight: number, isShowOkButton: boolean = false) {

        var windowArgs: any = {};

        windowArgs.Message = message;
        windowArgs.TextColor = textColor;
        windowArgs.IsShowOkButton = isShowOkButton;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = windowheight;
        logWindow.IsShowCloseButton = !isShowOkButton;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = title;

        logWindow.Show("./SharedLogistics/Components/SharedMessageComponent");
    }




    NewContactButtonClick() {

        var item = new ContactPM();
        item.Tenant = this.CurrentEntity.Tenant;
        item.CardId = this.CurrentEntity.Id;

        var itemComponent = new ContactItemClass(item, null, true);

        this.ShowAddEditContactWindow(itemComponent, "Add Contact");




    }

    LoginToOnlineVisibility() {
        window.open(`https://${SessionLocator.TenantManagementJS.CustomerURL}/online-visibility?securitykey=${ServiceHelper.GetLoggedUserToken()}&cid=${this.CurrentEntity.Id}&ctype=${this.CurrentEntity.PartnerTypeId}`, "_blank");
    }

    EditUserButtoClick(item: CustomerLineViewModel) {

        var itemComponent = new ContactItemClass(item.contactPM, null, false);
        this.ShowAddEditContactWindow(itemComponent, "Edit Contact");

    }




    ShowAddEditContactWindow(itemComponent: ContactItemClass, title: string) {
        this._entityResourceService.getEntityResourceByTableName("Contact").subscribe((response: any) => {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = title;
            logWindow.DataContext = itemComponent;
            var windowArgs: any = {};
            windowArgs.IsFromCustomerEdit = true;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditContactComponent');
            logWindow.WindowClosed.subscribe(($event: any) => this.LoadData());

        });
    }

    SetWindowArgs(args: any) {
        this.CurrentEntity = args.CurrentEntity;
        this.IsCargoTrackingMenuClicked = args.IsCargoTrackingMenuClicked;
        this.CustomerName = this.CurrentEntity.EnglishName;
        this.CustomerCode = this.CurrentEntity.Code;
        this.InvitationStatus = this.IsCargoTrackingMenuClicked ? this.CurrentEntity.CargoTrackingInvitationStatusName : this.CurrentEntity.SharedLogisticsInvitationStatusName;
        this.IsDigitalPortal = args.IsDigitalPortal;
        this.IsLoginToOnlineVisibility = this.IsDigitalPortal && this.CurrentEntity.CustomerStatusCode !== "ACT";

        this.LoadData();
    }

}
