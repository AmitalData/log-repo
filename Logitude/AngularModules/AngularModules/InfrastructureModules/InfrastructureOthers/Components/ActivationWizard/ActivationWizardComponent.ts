declare var System: any, window: any;
import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {Http, Response} from '@angular/http';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {HybridPartnerExtendedListService} from '../../../../Common/Services/ExtendedLists/HybridPartnerExtendedListService';
import {HybridPartnerList} from '../../../../Common/EntityLists/HybridPartnerList';
import {WebFreightDomainService} from '../../../../Infrastructure/Services/WebFreightDomainService';
import {AppTool} from '../../../../Infrastructure/Tools';
import {CustomerPMService} from '../../../../Common/Services/StandardPMs/CustomerPMService';
import {CustomerTenantAccessRequestPM} from '../../../../Common/EntityPMs/CustomerTenantAccessRequestPM';
import {CustomerTenantAccessRequestExtendedPMService} from '../../../../Common/Services/ExtendedPMs/CustomerTenantAccessRequestExtendedPMService'
@Component({
    selector: 'ActivationWizard',
    moduleId: module.id,
    templateUrl: './ActivationWizardComponent.html',
})

export class ActivationWizardComponent implements OnInit, AfterViewInit {
    AddPartnerToWizard: boolean = false;
    hybridPartnerList: any[];
    allhybridPartnerList: any[];
    ValidationErrorsList: any[];
    public _HybridPartnerListService: HybridPartnerExtendedListService;
    public _CustomerPMService: CustomerPMService;
    public _CustomerTenantAccessRequestExtendedPMService: CustomerTenantAccessRequestExtendedPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public CD: ChangeDetectorRef) {
        this._HybridPartnerListService = new HybridPartnerExtendedListService();
        this._CustomerPMService = new CustomerPMService();
        this._CustomerTenantAccessRequestExtendedPMService = new CustomerTenantAccessRequestExtendedPMService();
    }
    ngOnInit() {
        this.hybridPartnerList = [];
        this.allhybridPartnerList = [];
        this.ValidationErrorsList = [];
        if (FeatureLocator.HasFeaturePermession("General", "ADDPARTNERTOACTIVATIONWIZARD")) {
            this.AddPartnerToWizard = true;
        }
        else {
            this.AddPartnerToWizard = false;
        }

        this.FillHybridPartnerList();
    }
    ngAfterViewInit() {

    }

    FillHybridPartnerList() {
        this._HybridPartnerListService.GetHybridPartnerLists(SessionLocator.Tenant).subscribe(res => {
            this.hybridPartnerList = [];
            res.forEach((item, key) => {
                this.hybridPartnerList.push(new HybridPartnerData(item, this));
            });
            this.CurrentSession.StopBusyIndicator();
        });
        this._HybridPartnerListService.GetHybridPartnerListWithNoRequest(SessionLocator.Tenant).subscribe(res => {
            this.allhybridPartnerList = [];
            res.forEach((item, key) => {
                this.allhybridPartnerList.push(new HybridPartnerData(item, this));
            });
            this.CurrentSession.StopBusyIndicator();
        });
    }

    RefreshBtnClick() {
        this.CurrentSession.StartBusyIndicator("Loading ...");
        this.ValidationErrorsList = [];
        this.FillHybridPartnerList();
    }

    SendRequest(item) {
        this.CurrentSession.StartBusyIndicator("loading ...");
        this._CustomerTenantAccessRequestExtendedPMService.get(item.ReqId).subscribe(res => {
            if (!res.HasError) { 
                var temp = res.Result;
                temp.RequestStatus = "W";
                this._CustomerTenantAccessRequestExtendedPMService.update(temp).subscribe(res1 => {
                    item.StatusName = "Waiting For Approval";
                    item.IsHasRequest = true;
                    this.CurrentSession.StopBusyIndicator();
                });
            }
            else {
                this.ValidationErrorsList = res.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    AddRequest(item) {
        //List < ValidationResult > errors = new List<ValidationResult>();
        //busyIndicatorStartEvent.Publish(new BusyIndicatorStartEventArgs() { Message = "Loading ...", Start = true });
        //bool validateEntry = ValidateEntry();
        //bool hasValidationErrors = CheckValidationErrors();
        this.CurrentSession.StartBusyIndicator("Adding ...");

        if (AppTool.IsNullOrEmpty(SessionLocator.TenantPM.CustomerId)) {
            this.ValidationErrorsList.push("Missing Customer in Tenant Definitions !");
        }
        if (AppTool.IsNullOrEmpty(SessionLocator.TenantPM.VatNumber)) {
            this.ValidationErrorsList.push("VatNumber is not defined");
        }
        //FillErrors(errors);

        if (this.ValidationErrorsList.length == 0) {
            this._CustomerPMService.get(SessionLocator.TenantPM.CustomerId).subscribe(res => {
               
                if (!res.HasError) {
                    var CustomerPm = res.Result;
                    if (CustomerPm != null) {
                        if (AppTool.IsNullOrEmpty(CustomerPm.PrimaryContactId)) {
                            this.ValidationErrorsList.push("Missing Primary Contact Details in Customer " + (!AppTool.IsNullOrEmpty(CustomerPm.EnglishName) != null ? CustomerPm.EnglishName : CustomerPm.LocalName));
                        }
                        if (this.ValidationErrorsList.length == 0) {
                            var pm = new CustomerTenantAccessRequestPM();
                            pm.ForwarderId = item.Id,
                                pm.RequestStatus = "N",
                                pm.Tenant = SessionLocator.Tenant;

                            this._CustomerTenantAccessRequestExtendedPMService.insert(pm).subscribe(res => {
                                // We Need To check If There Are Errors.
                                this.FillHybridPartnerList();
                                this.CurrentSession.StopBusyIndicator();
                            });


                            //if (!context.CustomerTenantAccessRequestPMs.Contains(pm)) {
                            //    context.CustomerTenantAccessRequestPMs.Add(pm);
                            //}

                            //if (submitOperation != null) {
                            //    if (!submitOperation.IsComplete) {
                            //        submitOperation.Cancel();
                            //    }
                            //}

                            //submitOperation = context.SubmitChanges();
                            //submitOperation.Completed += submitOperation_Completed;
                        }
                        else {
                            this.CurrentSession.StopBusyIndicator();
                        }
                    }
                    else {
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
                else {
                    this.ValidationErrorsList = res.ErrorsArray;
                }
            });
        }
        else {
            this.CurrentSession.StopBusyIndicator();
        }
    }
}

export class HybridPartnerData {
    ParentComponent: ActivationWizardComponent;
    public hybridPartnerList: HybridPartnerList;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(passedhybridPartnerList: HybridPartnerList, Parent: ActivationWizardComponent) {
        this.ParentComponent = Parent;
        this.hybridPartnerList = passedhybridPartnerList;
        var myService: WebFreightDomainService = new WebFreightDomainService();
        myService.getHypridPartnerLogo(this.hybridPartnerList.LogoId).subscribe(myResult => {
            if (myResult) {
                this.Source = "data:image/JPEG;base64," + myResult;
                //Parent.CD.detectChanges();
            }
        });
    }
    get Id() {
        return this.hybridPartnerList.Id;
    }
    set Id(newValue: string) {
        this.hybridPartnerList.Id = newValue;
    }

    get IsHasRequest() {
        return this.hybridPartnerList.IsHasRequest;
    }
    set IsHasRequest(newValue: boolean) {
        this.hybridPartnerList.IsHasRequest = newValue;
    }


    get LogoId() {
        return this.hybridPartnerList.LogoId;
    }
    set LogoId(newValue: string) {
        this.hybridPartnerList.LogoId = newValue;
    }

    get StatusName() {
        return this.hybridPartnerList.StatusName;
    }
    set StatusName(newValue: string) {
        this.hybridPartnerList.StatusName = newValue;
    }

    get Name() {
        return this.hybridPartnerList.Name;
    }
    set Name(newValue: string) {
        this.hybridPartnerList.Name = newValue;
    }
    public Source: any;
    get ReqId() {
        return this.hybridPartnerList.ReqId;
    }
    set ReqId(newValue: string) {
        this.hybridPartnerList.ReqId = newValue;
    }

    buttonVisibility: boolean = true;
    get ButtonVisibility() {
        if (this.hybridPartnerList.IsHasRequest == true) {
            this.buttonVisibility = false;
        }
        else {
            this.buttonVisibility = true;
        }
        return this.buttonVisibility;
    }
    set ButtonVisibility(newValue: boolean) {
        this.buttonVisibility = newValue;
    }

    statusVisibility: boolean = true;
    get StatusVisibility() {
        if (this.hybridPartnerList.IsHasRequest == true) {
            this.statusVisibility = true;
        }
        else {
            this.statusVisibility = false;
        }
        return this.statusVisibility;
    }
    set StatusVisibility(newValue: boolean) {
        this.statusVisibility = newValue;
    }


}
