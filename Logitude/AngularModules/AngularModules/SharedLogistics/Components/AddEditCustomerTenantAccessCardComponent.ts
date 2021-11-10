import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {Guid} from '../../Infrastructure/Utilities/Guid';
import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {CustomerProductExtendedService} from '../../Common/Services/ExtendedPMs/CustomerProductExtendedService';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {CustomerPM} from '../../Common/EntityPMs/CustomerPM';
import {CustomerProductPM} from '../../Common/EntityPMs/CustomerProductPM';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CustomerTenantAccessPM} from '../../Common/EntityPMs/CustomerTenantAccessPM';
import {CustomerTenantAccessCardPM} from '../../Common/EntityPMs/CustomerTenantAccessCardPM';
import {EntityArgs} from '../../Infrastructure/DataContracts/EntityArgs';
import {ObservableCollection} from '../../Infrastructure/Utilities/ObservableCollection';
import {CardList} from '../../Common/EntityLists/CardList';
import {CommonDomainService} from '../../Common/Services/CommonDomainService';
import {PartnersDomainService} from '../../Common/Services/PartnersDomainService';
import {CustomerTenantAccessCardsBatchPM} from  '../../Common/EntityPMs/CustomerTenantAccessCardsBatchPM';
import {DateTool} from '../../Infrastructure/Tools';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {AddEditCustomerTenantAccessCardViewModel, CardListDataViewModel} from './RelatedCustomerComponent';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {CustomerTenantAccessPMService} from '../../Common/Services/StandardPMs/CustomerTenantAccessPMService';
import {CustomerPMService} from '../../Common/Services/StandardPMs/CustomerPMService';

@Component({
    
    selector: 'RelatedCustomerComponent',
    templateUrl: './AddEditCustomerTenantAccessCardComponent.html',
})

export class AddEditCustomerTenantAccessCardComponent extends BaseComponent {
    public SelectedItem: CardListDataViewModel;
    public viewModel: AddEditCustomerTenantAccessCardViewModel;
    public ValidationErrorsList: Array<string> = [];
    public DataLoaded: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public IsCustomsActivated: boolean = false;
    public IsExportActivated: boolean = false;
    public CanSelectOpption: boolean = false;

    constructor() {
        super();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    CheckIsImportActivated(item: CustomerTenantAccessCardPM) {

    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var checkIfCustomerSelected: CardListDataViewModel = this.viewModel.CardObsList.filter(a => a.IsSelectedSubmited == true)[0];
      

        if (checkIfCustomerSelected == null) {
            this.ValidationErrorsList.push("Please Select Customer");
        }

        if (this.ValidateCustomerTenantOptions(checkIfCustomerSelected)) {
            this.ValidationErrorsList.push("You can't add a new card. You have to choose either Export or Customs option.");
        }

        else if (checkIfCustomerSelected != null){
            this.InitializeCustomerTenantAccessCardd(checkIfCustomerSelected);

            Validator.TryValidateObject(this.viewModel.EntityPM, "CustomerTenantAccessCard", this.ValidationErrorsList);


            if (this.ValidationErrorsList.length == 0) {
                var window: ConfirmWindow = new ConfirmWindow();
                window.Title = "Confirm build shipments";
                window.Width = 450;
                window.Height = 190;
                window.YesButtonText = "Yes";
                window.NoButtonText = "No";
                window.Show("Do you want to build Shipments & Documents from " + this.viewModel.HybridStartDate.toDateString() + " ?");
                window.WindowClosed.subscribe(event => {
                    if (window.Yes) {
                        this.viewModel.EntityPM.BuildBatch = true;
                        this.viewModel.StatusTypeCode = "IP";
                        this.viewModel.StatusType = "In Progress";
                    }
                    else if (window.No) {
                        this.viewModel.StatusTypeCode = "A";
                        this.viewModel.StatusType = "Accepted";
                    }
                    this.CompleteConfirmation(checkIfCustomerSelected);

                });
            }
        }
    }

    private InitializeCustomerTenantAccessCardd(checkIfCustomerSelected: CardListDataViewModel) {
        this.viewModel.EntityPM.CustomerId = checkIfCustomerSelected.Id;
        this.viewModel.EntityPM.CustomerCode = checkIfCustomerSelected.Code;
        this.viewModel.EntityPM.CustomerName = checkIfCustomerSelected.EnglishName;
        this.viewModel.EntityPM.IsCustomsActivated = checkIfCustomerSelected.IsCustomsActivated;
        this.viewModel.EntityPM.IsExportActivated = checkIfCustomerSelected.IsExportActivated;
    }

    private ValidateCustomerTenantOptions(checkIfCustomerSelected: CardListDataViewModel) { 
        return checkIfCustomerSelected?.IsCustomsActivated == false && checkIfCustomerSelected.IsExportActivated == false;
    }

    CompleteConfirmation(checkIfCustomerSelected: CardListDataViewModel) {

        if (this.viewModel.isNew) {
            this.CurrentSession.StartBusyIndicatorSaving();
            this.viewModel.isNew = false;
            var service: CustomerPMService = new CustomerPMService();
            service.get(this.viewModel.CustomerId).subscribe((res:any) => {
                if (!res.HasError) {
                    var Customer = res.Result;
                    if (Customer != null) {
                        this.viewModel.CustomerCode = Customer.Code;
                        this.viewModel.CustomerName = Customer.EnglishName;
                        this.viewModel.StatusType = this.viewModel.EntityPM.StatusType;
                        this.viewModel.CreateByUserId = SessionLocator.LoggedUserPM.EnglishName;
                        if (!this.viewModel.Parent.ObsList.includes(this.viewModel)) {
                            this.viewModel.Parent.ObsList.push(this.viewModel);
                            this.viewModel.Parent.EntityPM.AddCustomerTenantAccessCardPM(this.viewModel.EntityPM);
                        }

                        if (!this.viewModel.Parent.RealCustomerTenantAccessPM.CustomerTenantAccessCards.includes(this.viewModel.EntityPM)) {
                            this.viewModel.Parent.RealCustomerTenantAccessPM.CustomerTenantAccessCards.push(this.viewModel.EntityPM);
                            //this.viewModel.Parent.RealCustomerTenantAccessPM.AddCustomerTenantAccessCardPM(this.viewModel.EntityPM);
                        }


                        if (this.viewModel.Parent.ObsList.filter(a => a.StatusType == "In Progress")[0]) {
                            this.viewModel.customertenantAccessPM.Status = "IP";
                            this.viewModel.Parent.RealCustomerTenantAccessPM.Status = "IP";
                            this.viewModel.Parent.EntityPM.Status = "IP";
                        }

                        else {
                            if (this.viewModel.Parent.ObsList.filter(a => a.StatusType == "Accepted")[0]) {
                                this.viewModel.customertenantAccessPM.Status = "A";
                                this.viewModel.Parent.RealCustomerTenantAccessPM.Status = "A";
                                this.viewModel.Parent.EntityPM.Status = "A";

                            }
                            else if (this.viewModel.Parent.ObsList.filter(a => a.StatusType != "Accepted")[0] && this.viewModel.Parent.ObsList.filter(a => a.StatusType != "In Active")[0]) {
                                this.viewModel.customertenantAccessPM.Status = "W";
                                this.viewModel.Parent.RealCustomerTenantAccessPM.Status = "W";
                                this.viewModel.Parent.EntityPM.Status = "W";
                            }
                            else {
                                this.viewModel.customertenantAccessPM.Status = "IA";
                                this.viewModel.Parent.RealCustomerTenantAccessPM.Status = "IA";
                                this.viewModel.Parent.EntityPM.Status = "IA";
                            }
                        }

                        var service: CustomerTenantAccessPMService = new CustomerTenantAccessPMService();
                        service.update(this.viewModel.Parent.EntityPM).subscribe(p => {
                            this.CurrentSession.StopBusyIndicator();
                            this.viewModel.Parent.IsShowTipArea = false;
                            if (this.viewModel.Parent.SelectedItem == null && this.viewModel.Parent.ObsList.length > 0) {
                                this.viewModel.Parent.SelectedItem = this.viewModel.Parent.ObsList[0];
                            }
                            this.CurrentSession.CloseCurrentWindow();
                        });
                    }



                }
            });


            }
          
                
    }

    SetWindowArgs(args: AddEditCustomerTenantAccessCardViewModel) {
        if (args != null) {
            this.viewModel = args;
            this.IsCustomsActivated = args.Parent.IsCustomsActivated;
            this.IsExportActivated = args.Parent.IsExportActivated;

            if (this.IsCustomsActivated && this.IsExportActivated) {
                this.CanSelectOpption = true;
                this.IsExportActivated = false;
                this.IsCustomsActivated = false;
            }


            //this.CanSelectOpption = args.Parent.CanSelectOpption;
            this.SetCustomerTenantAccessCardOptions();
            this.DataLoaded = true;   
        }
    }
    SetCustomerTenantAccessCardOptions() {
        if (this.viewModel.CardObsList != null) {
          
            this.viewModel.CardObsList.forEach( card => this.SetDirectionsFields(card));
        }
    }
     
    private SetDirectionsFields(card: CardListDataViewModel) {
        card.IsCustomsActivated = this.IsCustomsActivated;
        card.IsExportActivated = this.IsExportActivated;
    }
}
