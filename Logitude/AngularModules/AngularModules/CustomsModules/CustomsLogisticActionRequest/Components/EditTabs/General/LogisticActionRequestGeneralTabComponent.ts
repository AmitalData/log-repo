import { Component, Output, EventEmitter, isDevMode, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { LogisticActionRequestPM } from 'Customs/EntityPMs/LogisticActionRequestPM';
import { ClientList } from 'Customs/EntityLists/ClientList';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { CargoIdentifireTypeList } from 'Customs/EntityLists/CargoIdentifireTypeList';
import { ApiQueryFilters } from 'Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationList } from 'Customs/EntityLists/DeclarationList';
import { MessageWindow } from 'Controls/Windows/MessageWindow';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { ConsignmentPM } from 'Customs/EntityPMs/ConsignmentPM';
import { DeclarationPMService } from 'Customs/Services/StandardPMs/DeclarationPMService';
import { DeclarationListService } from 'Customs/Services/StandardLists/DeclarationListService';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ConsignmentDeclartion, DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogisticActionRequestPMService } from 'Customs/Services/StandardPMs/LogisticActionRequestPMService';

@Component({
    templateUrl: './LogisticActionRequestGeneralTabComponent.html',
    styleUrls: ['./LogisticActionRequestGeneralTabComponent.scss'],
})

export class LogisticActionRequestGeneralTabComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    public DataContext: LogisticActionRequestGeneralTabComponent = this;
    entityPM: LogisticActionRequestPM;
    public ObjectTableName: string = "Customs.LogisticActionRequestGeneralTabComponent";
    public IsNewEntity: boolean = false;
    public IsDisplayOnly: boolean = false;
    SendButtonEnabled: boolean = true;
    OKButtonEnabled: boolean = true;
    exporterName: string = ''
    syncDeclaration$ = new Subject();
    subscriber: Subscription;

    // public TabsItemsSource: TabItem[] = [];
    // public Tabs: LogTab[] = [];
    // public ValidationErrorsList: any[];
    // private currentEditComponentId: string;
    // public DisplayOnlyMessage: string = "";
    // public ImporterCode: string = "";
    // public IsCustomsFileRetrieved: boolean = false;
    // public CargoIdentifiersList: ObservableCollection;
    // public ExportCargoTypeFilterItems: ApiQueryFilters;
    // FIELD_IS_REQUIERD: string;
    // RequestVIA: SendRequestVIA;
    // //public ItemsList: ObservableCollection;
    // public decCargoSplitCargoIdentifierModel: DecCargoSplitCargoIdentifierModel;

    get ExportFileNo() { return this.entityPM?.ExportFileNo }
    set ExportFileNo(value: string) {
        this.entityPM.ExportFileNo = value;

        if (value)
            this.syncDeclaration$.next()
    }

    get ExporterNumber() { return this.entityPM?.ExporterNumber }
    set ExporterNumber(value: string) {
        this.entityPM.ExporterNumber = value;

        // if(value) {
        //     const clientTable: ClientList[] = await this.logtuideTableDataService.getTable("Customs.Client")
        //     const client: ClientList = clientTable.find(x=> x.Code == this.entityPM.ExporterNumber);
        //     this.entityPM.ExporterIdentifierType = client.PassportTypeCode
        // }        
    }

    get RequestDate() { return this.entityPM?.RequestDate }
    set RequestDate(value: Date) { this.entityPM.RequestDate = value; }

    get RequestType() { return this.entityPM?.RequestType }
    set RequestType(value: string) { this.entityPM.RequestType = value; }

    get CargoIdentifierType() { return this.entityPM?.CargoIdentifierType }
    set CargoIdentifierType(value: string) {
        this.entityPM.CargoIdentifierType = value;
        if (value)
            this.setRequiredCargoKey();
    }

    get CargoIdentifierKey1() { return this.entityPM?.CargoIdentifierKey1 }
    set CargoIdentifierKey1(value: string) { this.entityPM.CargoIdentifierKey1 = value; }

    get CargoIdentifierKey2() { return this.entityPM?.CargoIdentifierKey2 }
    set CargoIdentifierKey2(value: string) { this.entityPM.CargoIdentifierKey2 = value; }

    get CargoIdentifierKey3() { return this.entityPM?.CargoIdentifierKey3 }
    set CargoIdentifierKey3(value: string) { this.entityPM.CargoIdentifierKey3 = value; }

    get PackagingTypeCode() { return this.entityPM?.PackagingTypeCode }
    set PackagingTypeCode(value: string) { this.entityPM.PackagingTypeCode = value; }

    get Quantity() { return this.entityPM?.Quantity }
    set Quantity(value: number) { this.entityPM.Quantity = value; }

    get DeliverySiteID() { return this.entityPM?.DeliverySiteID }
    set DeliverySiteID(value: string) { this.entityPM.DeliverySiteID = value; }

    get RequestReason() { return this.entityPM?.RequestReason }
    set RequestReason(value: string) { this.entityPM.RequestReason = value; }

    get RequestCancelStatus() { return this.entityPM?.RequestCancelStatus }
    set RequestCancelStatus(value: string) { this.entityPM.RequestCancelStatus = value; }

    get RequestNumber() { return this.entityPM?.RequestNumber }
    set RequestNumber(value: string) { this.entityPM.RequestNumber = value; }

    get DecisionRmarks() { return this.entityPM?.DecisionRmarks }
    set DecisionRmarks(value: string) { this.entityPM.DecisionRmarks = value; }


    constructor(
        private logisticActionRequestPMService: LogisticActionRequestPMService,
        private EntityResourceService: EntityResourceService,
        private logtuideTableDataService: LogtuideTableDataService,
        private cdr: ChangeDetectorRef,
    ) {
        super();
        this.entityPM = new LogisticActionRequestPM();
        this.EntityResourceService.getEntityResourceByTableName("Customs.LogisticActionRequestType").subscribe(response => { });

        this.Listen();
    }


    SetWindowArgs(winArg: any) {
        this.entityPM = winArg.CurrentEntity;

        this.setTransportModeId();
        this.UIProperties.SetEnabled("RequestCancelStatus", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("RequestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DecisionRmarks", this.ObjectTableName, false);

        this.setRequiredFields();
    }


    private setRequiredFields() {
        [
            'RequestDate',
            'ExportFileNo',
            'RequestType',
            'RequestReason',
            'DeliverySiteID',
            'CargoIdentifierType',
            'CargoIdentifierKey1',
            'PackagingTypeCode',
            'Quantity',
            'DecisionRmarks',
        ].forEach(fieldName => this.UIProperties.SetRequired(fieldName, this.ObjectTableName, true));
    }


    async setRequiredCargoKey() {
        const cargoIdentifireTypeTable: CargoIdentifireTypeList[] = await this.logtuideTableDataService.getTable("Customs.CargoIdentifireType")
        const cargoIdentifireType: CargoIdentifireTypeList = cargoIdentifireTypeTable.find(x => x.Code == this.entityPM.CargoIdentifierType);
        this.UIProperties.SetRequired('CargoIdentifierKey2', this.ObjectTableName, cargoIdentifireType.IsKey2Mandatory)
        this.UIProperties.SetRequired('CargoIdentifierKey3', this.ObjectTableName, cargoIdentifireType.IsKey3Mandatory)
    }


    private setTransportModeId() {
        if (!this.entityPM.TransportmodeId) return;

        let checked;
        switch (this.entityPM.TransportmodeId) {
            case 'A':
                checked = 'air';
                break;

            case 'O':
                checked = 'ocean';
                break;

            case 'L':
                checked = 'land';
                break;
        }

        (<HTMLInputElement>document.getElementById(checked)).checked = true;
    }


    subscribesyncDeclaration() {
        this.subscriber = this.syncDeclaration$
            .pipe(debounceTime(500))
            .subscribe(x => this.syncDeclaration())
    }


    private async syncDeclaration() {
        const consignmentDeclartion: ConsignmentDeclartion = await this.getDeclarationsandConsignment();

        if (!consignmentDeclartion.Consignment) return;

        if (!(await this.confirmSyncDeclaration())) return

        // const declaration: DeclarationList = declarations[0];
        console.log(consignmentDeclartion)
        // const consignment: ConsignmentPM = await this.getConsignmentByDeclarationId(declaration.Id);        


        this.ExporterNumber = consignmentDeclartion.Consignment.Declaration.ImporterCode;
        this.CargoIdentifierType = consignmentDeclartion.Consignment.CargoTypeCode
        this.CargoIdentifierKey1 = consignmentDeclartion.Consignment.ManifestNumber
        this.CargoIdentifierKey2 = consignmentDeclartion.Consignment.SecondCargoID
        this.CargoIdentifierKey3 = consignmentDeclartion.Consignment.ThirdCargoID
        this.DeliverySiteID = consignmentDeclartion.Consignment.StorageSiteCode
        this.entityPM.DeclarationId = consignmentDeclartion.Consignment.Declaration.Id;
        this.entityPM.DeclarationNumber = consignmentDeclartion.Consignment.Declaration.DeclarationNumber;

        this.cdr.detectChanges();
    }


    private async confirmSyncDeclaration() {
        const exporterName = this.exporterName ? 'ליצואן ' + this.exporterName + ' ' : '';

        const myConfirmWindow = new ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.Show(`אותרה הצהרה ${exporterName}לפי מס' התיק , האם לבצע קישור ?`);

        return new Promise(resolve =>
            myConfirmWindow.WindowClosed.subscribe(event =>
                resolve(myConfirmWindow.Yes)));
    }


    // private async getConsignmentByDeclarationId(declarationId: string) {
    //     const filters: ApiQueryFilters = new ApiQueryFilters()
    //     filters.PageIndex = 0
    //     filters.PageSize = 1
    //     filters.addAdditionalFilter('DeclarationId', declarationId, null, null, 'Equals', false, false, false, 'String');
    //     let consignments: ConsignmentPM[] = await this.logtuideTableDataService.getTable('Customs.Consignment');
    //     consignments = consignments.filter(x=> x.DeclarationId == declarationId)

    //     return consignments.length > 0 ? consignments[0] : null;
    // }


    private async getDeclarationsandConsignment(): Promise<ConsignmentDeclartion> {
        // const filters: ApiQueryFilters = new ApiQueryFilters()
        // filters.PageIndex = 0
        // filters.PageSize = 10
        // // filters.addAdditionalFilter('CustomFileNo', this.entityPM.ExportFileNo, null, null, 'Equals', false, false, false, 'String');
        // const declarations: DeclarationList[] = await this.logtuideTableDataService.getDataFromService(new DeclarationListService().getByFilters(filters));


        // filters.addAdditionalFilter("exportfile", this.entityPM.ExportFileNo, null, null, "NotEqual", false, false, false, "string");

        // var a =   await new EntityListService().getByFilters("Customs.Declaration",filters) as any;
        // a.subscribe(x=> {
        //     console.log(x)
        //     debugger;
        // })
        const res: ConsignmentDeclartion = await new DeclarationWebService().getDeclarationConsignment(this.entityPM.ExportFileNo)
        return res;
    }


    private Listen() {
        // SessionLocator.SelectedSession.StopBusyIndicator();
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {
            // SessionLocator.SelectedSession.CurrentEditComponent.SubscriptionAdd(
            //     SessionLocator.SelectedSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
            //         if (isSaveSuccess) {
            //             if (SessionLocator.SelectedSession.CurrentEditComponent.entityPM instanceof DeclarationCargoSplitPM)this.entityPM = SessionLocator.SelectedSession.CurrentEditComponent.entityPM;
            //             //this.RefreshEntity();

            //         }
            //     })
            // );
        }
    }


    ngOnInit() {
        if (!this.entityPM.RequestDate)
            this.initDefaultValue();

        this.subscribesyncDeclaration()
    }


    ngOnDestroy() {
        this.subscriber.unsubscribe();
    }


    initDefaultValue() {
        this.entityPM.RequestDate = new Date();
        this.entityPM.RequestType = '2';
    }


    TransportModeClicked(value: string) {
        if (this.entityPM.TransportmodeId != value)
            this.entityPM.TransportmodeId = value;
    }


    ImporterClicked(type, client: ClientList) {
        this.ExporterNumber = client?.Code;
        this.exporterName = client?.FullName;
    }


    ImporterTextChanged(type, item) {
    }


    SendButtonClicked() {
        //this.SaveEntityChanges(null);
        //if (this.ValidationErrorsList.length == 0) return;
        var errors = [];
        this.FillValidationErrorList.emit(errors); // clear validation msgs
        SessionLocator.SelectedSession.StartBusyIndicator("");
    }


    // OnSendCompleted() {
    //     SessionLocator.SelectedSession.CloseCurrentWindow();
    // }
    SaveEntityChanges() {
        const isInsert: boolean = !this.entityPM.Id;

        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
       
        (isInsert ? this.logisticActionRequestPMService.insert(this.entityPM) : this.logisticActionRequestPMService.update(this.entityPM))        
        .subscribe((myResponse: ServiceResponse) => {
            SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
            SessionLocator.SelectedSession.StopBusyIndicator();
        });
    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {        
        this.SaveEntityChanges();        
    }


    OkButtonClicked() {
        this.SaveEntityChanges();
    }


    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
    }


    RefreshEntity() {
        if (SessionLocator.SelectedSession.CurrentEditComponent)
            SessionLocator.SelectedSession.CurrentEditComponent.ReloadEntityPM();
    }


    EditImporter() {
        // SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        // SessionLocator.SelectedSession.CurrentEditComponent.SaveChanges();
        // SessionLocator.SelectedSession.StopBusyIndicator();
        var windowArgs: any = {};
        windowArgs.EntityPM = this.entityPM;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 350;
        logWindow.Height = 230;
        logWindow.Title = 'נתונים נוספים ליצואן';
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsLogisticActionRequest/Components/EditTabs/MoreDetailesforImporterComponent/MoreDetailesforImporterComponent');
    }


    ViewDocumentsComponent() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.entityPM;
        //windowArgs.ObjectTableName = "Customs.DeclarationCancellation";
        windowArgs.ObjectTableName = "Customs.LogisticActionRequest";// this.ObjectTableName;
        windowArgs.EntityParentPM = "LogisticActionRequest";
        //    windowArgs.SkipCtor = this.SkipCtor;
        windowArgs.IsFromStandAloneScreen = true;
        var windowTitle = "General.MH.Documents";

        var logWindow = new LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        // logWindow.WindowClosed.subscribe(($event: any) => this.SkipCtor = true);
        logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
    }
}
