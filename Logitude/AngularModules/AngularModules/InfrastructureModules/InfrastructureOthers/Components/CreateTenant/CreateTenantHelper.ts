
declare var System: any;
declare var window: any;
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';


import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {ContactListService} from '../../../../Common/Services/StandardLists/ContactListService';
import {ContactList} from '../../../../Common/EntityLists/ContactList';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

export class CreateTenantHelper {

  
    AccountingCard: string;
    PrimaryContactId: string;
    CustomerName: string;
    CustomerId: string;
    VatNumber: string;
    ObjecttableName: string;
    CountryName: string;
    CountryCode: string;
    constructor(objecttablename: string, customerId: string, customerName: string, accountingCard: string, primaryContactId: string, vatNumber: string, countryName: string, countryCode: string)
    {
        this.ObjecttableName = objecttablename;
        this.AccountingCard = accountingCard;
        this.PrimaryContactId = primaryContactId;
        this.CustomerId = customerId;
        this.VatNumber = vatNumber;
        this.CustomerName = customerName;
        this.CountryName = countryName;
        this.CountryCode = countryCode; 

    }



    public CreateTenantMethod() {

        if (AppTool.IsNullOrEmpty(this.AccountingCard)) {
            if (!AppTool.IsNullOrEmpty(this.PrimaryContactId)) {
                var myService: ContactListService = new ContactListService();
                myService.getSingle(this.PrimaryContactId).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {
                        var contactList: ContactList = myResponse.Result;
                        if (contactList != null) {

                            var Msg: string = "";

                            if (this.ObjecttableName == "Customer") {
                                if ((ObjectsLocator.GlobalSetting.DeploymentStage == "logboxwe1" || ObjectsLocator.GlobalSetting.DeploymentStage == "Test2")) {
                                    if (AppTool.IsNullOrEmpty(contactList.BusinessPhone) && AppTool.IsNullOrEmpty(contactList.Mobile)) {
                                        Msg = "Business Phone Field Or Mobile Phone Field is Required";
                                    }
                                    else if (AppTool.IsNullOrEmpty(this.CustomerName)) Msg = "Customer Name is Required";
                                    else if (AppTool.IsNullOrEmpty(this.VatNumber)) Msg = "VatNumber is Required";

                                    if (!AppTool.IsNullOrEmpty(Msg)) {
                                        var logWindow = new LogitudeWindow();
                                        var windowArgs: any = {};
                                        windowArgs.MessageError = Msg;
                                        logWindow.WindowArgs = windowArgs;
                                        logWindow.Width = 510;
                                        logWindow.Height = 340;
                                        logWindow.Title = "Validation";
                                        logWindow.Show("./InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantValidationScreenComponent");
                                    }

                                }
                            }

                            if (AppTool.IsNullOrEmpty(Msg)) {
                                if (!AppTool.IsNullOrEmpty(contactList.Email) && !AppTool.IsNullOrEmpty(contactList.EnglishName) && !AppTool.IsNullOrEmpty(this.CustomerName)) {
                                    var logWindow = new LogitudeWindow();
                                    var windowArgs: any = {};
                                    windowArgs.CustomerId = this.CustomerId;
                                    windowArgs.Email = contactList.Email;
                                    windowArgs.ContactName = contactList.EnglishName;
                                    windowArgs.CustomerName = this.CustomerName;
                                    windowArgs.Phone = contactList.Mobile;
                                    windowArgs.CountryName = this.CountryName;
                                    windowArgs.CountryCode = this.CountryCode;
                                    windowArgs.ObjecttableName = this.ObjecttableName;
                                    logWindow.WindowArgs = windowArgs;
                                    logWindow.Width = 400;
                                    logWindow.Height = 120;
                                    logWindow.Title = "Package Type";
                                    logWindow.Show("./InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantPackageSelectionComponent");

                                }

                                else this.FillErrorsList(contactList.Email, contactList.EnglishName);

                            }
                          


                        }
                        else this.FillErrorsList();
                    }
                });
            }
            else this.FillErrorsList();
        }

        else {

            var messageWindow: MessageWindow = new MessageWindow();
            var validationErrorMessage = "note that this customer has a tenant already " + this.AccountingCard + ", please erase the tenant# in order to create a new one" + " (" + this.AccountingCard + " = External ID)"
            messageWindow.Show(validationErrorMessage);

        }
    }

    public FillErrorsList(email: string = "", contactname: string = "") {
        var logWindow = new LogitudeWindow();
        var windowArgs: any = {};
        windowArgs.ContactEmail = email;
        windowArgs.ContactName = contactname;
        windowArgs.CustomerName = this.CustomerName;


        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 510;
        logWindow.Height = 340;
        logWindow.Title = "Validation";
        logWindow.Show("./InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantValidationScreenComponent");
    }

   
 




}