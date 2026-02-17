
declare var System: any;
declare var window: any;
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {SharedLogisticContactPM} from '../../../Common/EntityPMs/SharedLogisticContactPM';
import {ContactPM} from '../../../Common/EntityPMs/ContactPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ContactPMService} from '../../../Common/Services/StandardPMs/ContactPMService';
import {InviteCustomersComponent} from '../../Components/InviteCustomersComponent';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceLocator} from '../../../Infrastructure/Locators/ServiceLocator';

export class CustomerLineViewModel {


    public get Name() {

        if (this.entityPM) {
            return this.entityPM.Name;
        }
        else return "";
    }

    public HasAccessPath: boolean;
    public HasAccressFill: boolean;
    IsEnableEditContact: boolean = false;

    public get InviteButtonContent() {

        if (this.InternetAccess) {
            return  "Invite again";
        }
        else return "Invite";
    }

    entityPM: SharedLogisticContactPM;
    public contactPM: ContactPM;
    public get InternetAccess() {

        if (this.entityPM) {
            return this.entityPM.InternetAccess;
        }
        else return false;
    }
    public set InternetAccess(value: boolean) {
        if (this.entityPM != null) {
            this.entityPM.InternetAccess = value;
            this.Trigger.SaveChanges(this.entityPM);       
        }

    }


    public get EnglishName() {

        if (this.entityPM) {
            return this.entityPM.EnglishName;
        }
        else return "";
    }
    public set EnglishName(value: string) {
        if (this.entityPM != null) {
            this.entityPM.EnglishName = value;


        }

    }
    

    public get Email() {

        if (this.entityPM) {
            return this.entityPM.Email;
        }
        else return "";
    }
    public set Email(value: string) {
        if (this.entityPM != null) {
            this.entityPM.Email = value;


        }

    }



    public get Position() {

        if (this.entityPM) {
            return this.entityPM.Position;
        }
        else return "";
    }
    public set Position(value: string) {
        if (this.entityPM != null) {
            this.entityPM.Position = value;


        }

    }


    public get BusinessPhone() {

        if (this.entityPM) {
            return this.entityPM.BusinessPhone;
        }
        else return "";
    }
    public set BusinessPhone(value: string) {
        if (this.entityPM != null) {
            this.entityPM.BusinessPhone = value;


        }

    }

    public get Mobile() {

        if (this.entityPM) {
            return this.entityPM.Mobile;
        }
        else return "";
    }
    public set Mobile(value: string) {
        if (this.entityPM != null) {
            this.entityPM.Mobile = value;


        }

    }

    public get Fax() {

        if (this.entityPM) {
            return this.entityPM.Fax;
        }
        else return "";
    }
    public set Fax(value: string) {
        if (this.entityPM != null) {
            this.entityPM.Fax = value;


        }

    }

    public get LastLoginDate() {

        if (this.entityPM) {
            return this.entityPM.LastLoginDate;
        } else return "";
       
    }
    public set LastLoginDate(value: string) {
        if (this.entityPM != null) {
            this.entityPM.LastLoginDate = value;


        }

    }

    public contactPMService: ContactPMService;
    Trigger: InviteCustomersComponent;
    constructor(item: SharedLogisticContactPM, trigger: InviteCustomersComponent ) {
        this.entityPM = item;
        this.Trigger = trigger;
        if (this.contactPMService == null) {
            this.contactPMService = new ContactPMService();

        }


        this.contactPMService.get(this.entityPM.ContactId).subscribe(res=> {
            var pmResponse: ServiceResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    this.contactPM = myResult;
                    this.IsEnableEditContact = true;
                }
            }
        });
   

       
    }

    InviteButtonButtonclick() {

     
        if (!this.Email) {
            if (this.Trigger) {
                this.Trigger.ShowMessageWindow("The Contact you want to invite has no email! \nplease fill the email then press invite", "", "#1B90CB",130);
            }

        }
        else {
            this.InternetAccess = true;
        }

    }

    BlockAccessButtonclick() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Are you sure you want block this contact's access ?");
        confirmWindow.ShowCancelButton = true;
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                ServiceLocator.SendTotangoUserActivity("Contact", "Mobile blocked");
                this.InternetAccess = false;
            }
        });
    }


}