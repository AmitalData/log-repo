import {Component, OnInit}  from '@angular/core';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {SharedLogisticsService} from '../Services/Others/SharedLogisticsService';
import {ServiceResponse} from '../../Infrastructure/DataContracts/ServiceResponse';
import {SessionInfo} from '../../Infrastructure/Utilities/SessionInfo';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';
import {CustomerProductExtendedService} from '../../Common/Services/ExtendedPMs/CustomerProductExtendedService';
import {EntityResourceService} from '../../Infrastructure/Services/EntityResourceService';
import {CustomerProductPM} from '../../Common/EntityPMs/CustomerProductPM';
import {BaseComponent} from '../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CustomerTenantAccessPM} from '../../Common/EntityPMs/CustomerTenantAccessPM';
import {CustomerTenantAccessCardPM} from '../../Common/EntityPMs/CustomerTenantAccessCardPM';
import {EntityArgs} from '../../Infrastructure/DataContracts/EntityArgs';
import {CustomerTenantAccessCardsBatchPM} from  '../../Common/EntityPMs/CustomerTenantAccessCardsBatchPM';
import {LogitudeWindow} from '../../Controls/Windows/LogitudeWindow';
import {AddEditCustomerTenantAccessCardViewModel, CardListDataViewModel, RelatedCustomerComponent} from './RelatedCustomerComponent';
import {ConfirmWindow} from '../../Controls/Windows/ConfirmWindow';
import {Validator} from '../../Infrastructure/Validators/Validator';
import {CustomerTenantAccessPMService} from '../../Common/Services/StandardPMs/CustomerTenantAccessPMService';
import {CustomerTenantAccessStatusTypeList} from '../../Common/EntityLists/CustomerTenantAccessStatusTypeList';
import {CustomerTenantAccessStatusTypeListService} from '../../Common/Services/StandardLists/CustomerTenantAccessStatusTypeListService';
import {CommonDomainService} from '../../Common/Services/CommonDomainService';
import {AppTool} from '../../Infrastructure/Tools';
@Component({
    moduleId: module.id,
    selector: 'EditRelatedCustomerComponent',
    templateUrl: './EditRelatedCustomerComponent.html',
})

export class EditRelatedCustomerComponent extends BaseComponent {
    public StatusSelectedItem: any;
    public DataContext: EditRelatedCustomerComponent = this;
    public EntityPM: CustomerTenantAccessCardPM;
    public ObjectTableName = "CustomerTenantAccessCard";
    public Parent: RelatedCustomerComponent;
    public StatusList: Array<CustomerTenantAccessStatusTypeList> = [];
    public ValidationErrorsList: Array<string> = [];
    public SelectedStatus: string = null;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    public get StatusTypeCode() {
        return this.EntityPM.StatusTypeCode;
    }
    public set StatusTypeCode(value: string) {
        if (this.EntityPM.StatusTypeCode != value)
            this.EntityPM.StatusTypeCode = value;
    }
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving(); 
        var service: CommonDomainService = new CommonDomainService();
     
                this.ValidationErrorsList = [];
                Validator.TryValidateObject(this.EntityPM, "CustomerTenantAccessCard", this.ValidationErrorsList);
                this.EntityPM.BuildBatch = true;
                if (this.ValidationErrorsList.length == 0) {
                    this.StatusTypeCode = this.StatusSelectedItem.Code;
                    this.EntityPM.StatusTypeCode = this.StatusSelectedItem.Code;
                    this.EntityPM.StatusType = this.StatusSelectedItem.EnglishName;

                    if (this.Parent.EntityPM.CustomerTenantAccessCards.filter(a => a.StatusTypeCode == "IP")[0]) {
                        this.Parent.EntityPM.Status = "IP";
                    }

                    else {
                        if (this.Parent.EntityPM.CustomerTenantAccessCards.filter(a => a.StatusTypeCode == "A")[0]) {
                            this.Parent.EntityPM.Status = "A";
                            this.Parent.RealCustomerTenantAccessPM.Status = "A";
                        }
                        else if (this.Parent.EntityPM.CustomerTenantAccessCards.filter(a => a.StatusTypeCode != "A")[0] && this.Parent.EntityPM.CustomerTenantAccessCards.filter(a => a.StatusTypeCode != "IA")[0]) {
                            this.Parent.EntityPM.Status = "W";
                            this.Parent.RealCustomerTenantAccessPM.Status = "W";
                        }
                        else {
                            this.Parent.EntityPM.Status = "IA";
                            this.Parent.RealCustomerTenantAccessPM.Status = "IA";
                        }
                    }


                    var updateService: CustomerTenantAccessPMService = new CustomerTenantAccessPMService();
                    updateService.update(this.Parent.EntityPM).subscribe(res => {
                        this.CurrentSession.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindowEmit("OK");

                    });
                }
            


    }


    SetWindowArgs(args: any) {
        if (args != null) {
            this.EntityPM = args.EntityPM;
            this.Parent = args.Parent;
            this.fillComboBox();
          
        }
    }

    fillComboBox() {
        var entityService: EntityResourceService = new EntityResourceService();
        entityService.getEntityResourceByTableName("CustomerTenantAccessStatusType", 0).subscribe(p => {
            var service: CustomerTenantAccessStatusTypeListService = new CustomerTenantAccessStatusTypeListService();
            service.getAllFromCache().subscribe(res => {
                if (!res.HasError) {
                    this.StatusList = res.Result.filter(d => d.Code == "A" || d.Code == "IA");
                    if (!AppTool.IsNullOrEmpty(this.EntityPM.StatusTypeCode)) {
                        this.StatusSelectedItem = this.StatusList.filter(s => s.Code == this.EntityPM.StatusTypeCode)[0];
                    }
                }
            });
        });        
    }




    




}
