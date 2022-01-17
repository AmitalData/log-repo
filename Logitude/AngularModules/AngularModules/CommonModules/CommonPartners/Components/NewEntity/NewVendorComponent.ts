import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {NewPartnerTamplate} from '../Templates/NewPartnerTamplate';
import {PartnersDomainService, PartnerServicePM} from '../../../../Common/Services/PartnersDomainService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {VendorPM} from '../../../../Common/EntityPMs/VendorPM';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool } from '../../../../Infrastructure/Tools';

@Component({
    
    templateUrl: './NewVendorComponent.html',
})

export class NewVendorComponent {
    public EntityPM: VendorPM;
    public PartnerTypeId: string = "VD";
    public ObjectTableName: string = "Vendor";
    public ValidationErrorsList: string[] = [];
    public DomainService: PartnersDomainService;
    private PartnerTamplate: NewPartnerTamplate;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    @ViewChild('Child', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.EntityPM = new VendorPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.PartnerTypeId = this.PartnerTypeId;
        this.DomainService = new PartnersDomainService();
        this.RunComponent();
    }

    RunComponent() {
        if (this.viewContainerRef) {
            this.LoadChildComponent();
        }

        else {
            this.RunComponentTimer();
        }
    }

    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer() {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {
            this.timerToken = setTimeout(() => this.RunComponent(), 1);
        }
    }

    LoadChildComponent() {
        this._entityResourceService.getEntityResourceByTableName("Address", 0).subscribe(response=> {
            this._entityResourceService.getEntityResourceByTableName("Customer").subscribe(response2 => {
                SessionLocator.DynamicLoader.Load("./CommonModules/CommonPartners/Components/Templates/NewPartnerTamplate", this.viewContainerRef)
                    .then(cmpRef => {
                        this.PartnerTamplate = cmpRef.instance;
                        this.PartnerTamplate.EntityPM = this.EntityPM;
                        this.PartnerTamplate.CardTableName = this.ObjectTableName;
                        this.PartnerTamplate.PartnerTypeId = this.PartnerTypeId;
                        this.PartnerTamplate.DomainService = this.DomainService;
                        this.PartnerTamplate.InitTemplate();
                    });
            })
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = this.PartnerTamplate.Validate();

        if (errors.length == 0) {
            this.EntityPM.Code = this.PartnerTamplate.CardCode;
            this.EntityPM.EnglishName = this.PartnerTamplate.Name;
            this.EntityPM.LocalName = !AppTool.IsNullOrEmpty(this.PartnerTamplate.LocalName) ? this.PartnerTamplate.LocalName : this.PartnerTamplate.Name;
            this.EntityPM.VatNumber = this.PartnerTamplate.VatNumber;
            this.EntityPM.ExistedContactId = this.PartnerTamplate.ExistedContactId;

            Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            var args = new PartnerServicePM();
            args.Tenant = this.EntityPM.Tenant;
            args.PartnerTypeId = this.PartnerTypeId;
            args.Vendor = this.EntityPM;
            args.Address = this.PartnerTamplate.Address;
            if (this.PartnerTamplate.IsAddContactChecked) {
                this.PartnerTamplate.Contact.SetAsPrimaryForCard = true;
                args.Contact = this.PartnerTamplate.Contact;
            }

            this.DomainService.PostPartnerAddress(args).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (!myResponse.HasError) {
                    this.EntityPM = myResponse.Result.Vendor;
                    this.CurrentSession.CloseCurrentWindowEmit(this.EntityPM.Id);
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }
            });
        }
    }
}
