/// <reference path="../../controls/windows/messagewindow.ts" />
import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {SharedLogisticContactService} from '../Services/ExtendedPMs/SharedLogisticContactService';
import {CustomerList} from '../../Common/EntityLists/CustomerList';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {SharedLogisticContactPM} from '../../Common/EntityPMs/SharedLogisticContactPM'
import {CustomerLineViewModel} from './ViewModel/CustomerLineViewModel';
import {ContactInputTemplateArgs} from '../../CommonModules/CommonPartners/Components/Templates/ContactInputTemplate';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {ContactItemClass} from '../../CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent';
import {MessageWindow} from '../../Controls/Windows/MessageWindow';
import {ContactPM} from '../../Common/EntityPMs/ContactPM';

import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
@Component({
    moduleId: module.id,
    selector: 'InviteCustomersComponent',
    templateUrl: './InviteCustomersComponent.html',
    //inputs: ['PartnerTypeId', , 'DateParameter', 'DataContext', 'OnCloseWindowEvent'],
    providers: [SharedLogisticContactService],
})
export class InviteCustomersComponent implements OnInit {

    CurrentEntity: CustomerList;
    NoContactsVisibility: boolean;
    public SharedLogisticCustomerLineList: CustomerLineViewModel[];

    CustomerName: string;
    CustomerCode: string;
    InvitationStatus: string;

    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _sharedLogisticContactService: SharedLogisticContactService) {
        this.CurrentSession.StartBusyIndicatorLoading();
    }

    ngOnInit(

    ) {

    }


    LoadData() {

        this.SharedLogisticCustomerLineList = [];
        this._sharedLogisticContactService.getSharedLogisticContactsbyCardId(this.CurrentEntity.Id,SessionInfo.LoggedUserTenant).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.StopBusyIndicator();

            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                 result.forEach((item) => {
                    this.SharedLogisticCustomerLineList.push(new CustomerLineViewModel(item,this));
       
                });


                 if (this.SharedLogisticCustomerLineList.length == 0) this.NoContactsVisibility = true;
                 else this.NoContactsVisibility = false;
            }
        });
    }


    sharedLogisticContact: SharedLogisticContactPM;
    SaveChanges(item: SharedLogisticContactPM) {
        this.sharedLogisticContact = item;
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
        this._sharedLogisticContactService.ContactInternetAccessInvitation(this.sharedLogisticContact).subscribe(res => {
            var pmResponse: ServiceResponse = res;
            this.CurrentSession.CurrentWindow.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {

                    //// this.currentAssemlyLocator.ListControl.GetSingleList(entityPM.Id);
                    // this.ReloadCustomer();
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


    ShowMessageWindow(message: string, title: string, textColor: string,windowheight: number, isShowOkButton: boolean = false) {

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


    EditUserButtoClick(item: CustomerLineViewModel) {
    
        var itemComponent = new ContactItemClass(item.contactPM, null, false);
        this.ShowAddEditContactWindow(itemComponent, "Edit Contact");

    }




    ShowAddEditContactWindow(itemComponent: ContactItemClass, title: string) {
        this._entityResourceService.getEntityResourceByTableName("Contact").subscribe(response => {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = title;
            logWindow.DataContext = itemComponent;
            logWindow.Show('./CommonModules/CommonPartners/Components/AddEdit/AddEditContactComponent');
            logWindow.WindowClosed.subscribe(($event: any) => this.LoadData());

        });
    }

    SetWindowArgs(args: any) {

        this.CurrentEntity = args.CurrentEntity;
        this.CustomerName = this.CurrentEntity.EnglishName;
        this.CustomerCode = this.CurrentEntity.Code;
        this.InvitationStatus = this.CurrentEntity.SharedLogisticsInvitationStatusName;
        this.LoadData();
    }

 

}
