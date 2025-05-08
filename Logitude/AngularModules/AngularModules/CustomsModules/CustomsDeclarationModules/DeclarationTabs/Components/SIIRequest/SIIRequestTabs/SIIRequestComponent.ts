import { Component, QueryList, ViewChildren, OnInit } from '@angular/core';
import { EntityArgs } from '../../../../../../Infrastructure/DataContracts/EntityArgs';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { SIIRequestPMService } from 'Customs/Services/StandardPMs/SIIRequestPMService';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { SiiRequestMode } from '../SIIRequestTabComponent';
import { SIIRequestPM } from 'Customs/EntityPMs/SIIRequestPM';
import { SupplierInvoiceItemList } from 'Customs/EntityLists/Extended/SupplierInvoiceItemList';
import { SupplierInvoiceItemExtendedListService } from 'Customs/Services/ExtendedLists/SupplierInvoiceItemExtendedListService';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from 'Infrastructure/Tools';
import { DeclarationGeneralComponent } from '../../General/DeclarationGeneralComponent';

@Component({
    selector: 'SIIRequestComponent',
    templateUrl: './SIIRequestComponent.html',
    styleUrls: ['./SIIRequestComponent.scss'],
    providers: [EntityArgs],
})


export class SIIRequestComponent extends BaseComponent implements OnInit {
    public DataContext = this;
    public ObjectTableName: string = "Customs.Declaration";
    @ViewChildren(LocationDirective) public AllLocations: QueryList<LocationDirective>;
    siiRequestPMService: SIIRequestPMService = new SIIRequestPMService();
    supplierinvoiceitemsWebService: SupplierInvoiceItemExtendedListService = new SupplierInvoiceItemExtendedListService();
    public entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    public DecalarationData: DeclarationPM;
    public IsNewOrEdit: SiiRequestMode;
    isDispalyOnlyStatusList: number[] = [4, 8];
    public isAllowChange: boolean = false;
    public entityPM: SIIRequestPM = new SIIRequestPM();
    public supplierInvoiceItemsList: SupplierInvoiceItemList[] = [];
    public filterAgrs: ApiQueryFilters;

    constructor(public entityArgs: EntityArgs) {
        super();
    }

    ngOnInit(): void {
        this.initiallizeComponent();
    }

    initiallizeComponent() {
        this.getAllSupplierinvoiceItemsByDeclarationId(this.DecalarationData.Id, this.DecalarationData.Tenant);
    }

    getAllSupplierinvoiceItemsByDeclarationId(declarationId: string, tenant: number) {
        let filterAgrs: ApiQueryFilters = this.filterAgrs;
        filterAgrs.addAdditionalFilter("DeclarationId", declarationId, null, null, "Equals", false, false, false, "string", false);
        filterAgrs.addAdditionalFilter("Tenant", tenant, null, null, "Equals", true, false, false, "string");
        this.supplierinvoiceitemsWebService.getByFilters(this.filterAgrs).subscribe((response: any) => {
            let myResponse: ServiceResponse = response;
            if (myResponse?.HasError && myResponse?.Result) {
                this.supplierInvoiceItemsList = myResponse.Result;
            }
        });
    }

    SetWindowArgs(args: any) {
        this.entityPM = args.CertificateOfOrigin;
        this.DecalarationData = args.Decalaration;
        this.IsNewOrEdit = args.IsNewOrEdit;
        this.isAllowChange = args.isAllowChange;
        this.filterAgrs = args.filterAgrs;
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = "Customs.CertificateOfOrigin";
    }


    //#region  CertificateOfOrigin properties
    public get Id(): string {
        return this.entityPM.Id;
    }
    public set Id(newValue: string) {
        this.entityPM.Id = newValue;
    }

    public get Tenant(): number {
        return this.entityPM.Tenant;
    }
    public set Tenant(newValue: number) {
        this.entityPM.Tenant = newValue;
    }

    public get RequestNo(): string {
        return this.entityPM.RequestNo;
    }
    public set RequestNo(newValue: string) {
        this.entityPM.RequestNo = newValue;
    }

    public get DeclarationId(): string {
        return this.entityPM.DeclarationId;
    }
    public set DeclarationId(newValue: string) {
        this.entityPM.DeclarationId = newValue;
    }

    public get Status(): string {
        return this.entityPM.Status;
    }
    public set Status(newValue: string) {
        this.entityPM.Status = newValue;
    }

    public get WareHouseAddress(): string {
        return this.entityPM.WareHouseAddress;
    }
    public set WareHouseAddress(newValue: string) {
        this.entityPM.WareHouseAddress = newValue;
    }

    public get WareHouseCity(): string {
        return this.entityPM.WareHouseCity;
    }
    public set WareHouseCity(newValue: string) {
        this.entityPM.WareHouseCity = newValue;
    }

    public get IsClosed(): boolean {
        return this.entityPM.IsClosed;
    }
    public set IsClosed(newValue: boolean) {
        this.entityPM.IsClosed = newValue;
    }

    public get Remarks(): string {
        return this.entityPM.Remarks;
    }
    public set Remarks(newValue: string) {
        this.entityPM.Remarks = newValue;
    }
    //#endregion

    public get CustomFileNo(): string {
        return this.DecalarationData.CustomFileNo;
    }
    public set CustomFileNo(newValue: string) {
        this.DecalarationData.CustomFileNo = newValue;
    }

    // public get ExporterResponsibleName(): string {
    //     return this.ExporterResponsibleName;
    // }
    // public get MobilePhone(): string {
    //     return this.MobilePhone;
    // }
    // public get ImporterPhone(): string {
    //     return this.ImporterPhone;
    // }
    // public get SubmitDate(): string {
    //     return this.SubmitDate;
    // }
    // public get ImporterEmail(): string {
    //     return this.ImporterEmail;
    // }
    // public get ImporterId(): string {
    //     return AppTool.IsNullOrEmpty(this.DecalarationData.ImporterId) ? this.DecalarationData.ImporterId : this.DecalarationData.ImporterCode;
    // }

    
}
