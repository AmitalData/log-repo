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
import {AddEditCustomerTenantAccessCardViewModel, CardListDataViewModel, RelatedCustomerComponent} from './RelatedCustomerComponent';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {CustomerTenantAccessPMService} from '../../Common/Services/StandardPMs/CustomerTenantAccessPMService';
import {CustomerTenantAccessCardsBatchPMService} from '../../Common/Services/StandardPMs/CustomerTenantAccessCardsBatchPMService';

@Component({
    moduleId: module.id,
    selector: 'AddCustomerBatchComponent',
    templateUrl: './AddCustomerBatchComponent.html',
})

export class AddCustomerBatchComponent extends BaseComponent {
    public AccessCardPM: CustomerTenantAccessCardPM;
    public Parent: RelatedCustomerComponent;
    public ValidationErrorsList: Array<string> = [];
    public DataContext: AddCustomerBatchComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.AccessCardPM = args.AccessCardPM;
            this.Parent = args.Parent;

        }
    }

    private fromDatetime: Date;
    public get FromDatetime() {  
        if (this.fromDatetime == null || this.fromDatetime.getFullYear() == 1 || this.fromDatetime == DateTool.GetDateFormats(new Date()).DateParts.DateObject) {
            var date = DateTool.GetCurrentDateTimeAsUtc();
            date.setDate(date.getDate() - 90);
            this.fromDatetime = date
        }
        return this.fromDatetime;
    }

    public set FromDatetime(value: Date) {
        if (this.fromDatetime != value)
            this.fromDatetime = value;
    }

    private toDatetime: Date;
    public get ToDatetime() {
        if (this.toDatetime == null || this.toDatetime.getFullYear() == 1 || this.toDatetime == DateTool.GetDateFormats(new Date()).DateParts.DateObject) {
            this.toDatetime = DateTool.GetCurrentDateTimeAsUtc();
        }
        return this.toDatetime;
    }

    public set ToDatetime(value: Date) {
        if (this.toDatetime != value)
            this.toDatetime = value;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        if (this.FromDatetime > this.ToDatetime)
            this.ValidationErrorsList.push("From date must be smaller\equal to To date");
        if (this.ValidationErrorsList.length == 0) {
            var service: CustomerTenantAccessCardsBatchPMService = new CustomerTenantAccessCardsBatchPMService();
            var customerTenantAccessCardsBatchPM: CustomerTenantAccessCardsBatchPM = new CustomerTenantAccessCardsBatchPM();
            customerTenantAccessCardsBatchPM.CustomerId = this.AccessCardPM.CustomerId;
            customerTenantAccessCardsBatchPM.CustomerTenantAccessId = this.AccessCardPM.CustomerTenantAccessId;
            customerTenantAccessCardsBatchPM.ToDatetime = this.ToDatetime;
            customerTenantAccessCardsBatchPM.FromDatetime = this.FromDatetime;
            customerTenantAccessCardsBatchPM.Status = "Created";
            customerTenantAccessCardsBatchPM.Tenant = this.AccessCardPM.Tenant;
            customerTenantAccessCardsBatchPM.TotalFailed = 0;
            customerTenantAccessCardsBatchPM.TotalShipment = 0;
            customerTenantAccessCardsBatchPM.Totalsucceeded = 0;
            service.insert(customerTenantAccessCardsBatchPM).subscribe(p => {
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            });;
        }
    }



}
