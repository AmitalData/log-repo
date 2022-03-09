import { Component, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { LogisticActionRequestPM } from 'Customs/EntityPMs/LogisticActionRequestPM';
import { ClientList } from 'Customs/EntityLists/ClientList';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { LogtuideTableDataService } from 'QuoteOPM/Components/NewEntity/components/autocomplate-table/logtuide-table-data.service';
import { CargoIdentifireTypeList } from 'Customs/EntityLists/CargoIdentifireTypeList';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { Subject, Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { ConsignmentDeclartion, DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogisticActionRequestPMService } from 'Customs/Services/StandardPMs/LogisticActionRequestPMService';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { LogisticActionRequestRequestParams } from 'Customs/DataContract/RequestParams/LogisticActionRequestRequestParams';

@Component({
    templateUrl: './LogisticActionRequestGeneralTabComponent.html',
    styleUrls: ['./LogisticActionRequestGeneralTabComponent.scss'],
})

export class LogisticActionRequestGeneralTabComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    @Output() MenuHeaderchangeevent = new EventEmitter();

    public DataContext: LogisticActionRequestGeneralTabComponent = this;
    entityPM: LogisticActionRequestPM;
    public ObjectTableName: string = "Customs.LogisticActionRequest";
    public IsNewEntity: boolean = false;
    public IsDisplayOnly: boolean = false;
    SendButtonEnabled: boolean = true;
    OKButtonEnabled: boolean = true;
    exporterName: string = ''
    syncDeclaration$ = new Subject();
    subscriber: Subscription;
    ValidationErrorsList: any[] = [];
    FIELD_IS_REQUIERD: string = TextCodeTranslator.Translate("General.M.FieldIsRequired");
    requierdFieldsList = [
        'ExportFileNo',
        'ExporterNumber',
        'RequestDate',
        'RequestType',
        'CargoIdentifierType',
        'CargoIdentifierKey1',
        'PackagingTypeCode',
        'Quantity',
        'DeliverySiteID',
    ]

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
        this.UIProperties.SetRequired('ExportFileNo', this.ObjectTableName, !value)

        if (value)
            this.syncDeclaration$.next()
    }

    get ExporterNumber() { return this.entityPM?.ExporterNumber }
    set ExporterNumber(value: string) {
        this.entityPM.ExporterNumber = value;
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
    set CargoIdentifierKey1(value: string) {
        this.entityPM.CargoIdentifierKey1 = value;
        this.UIProperties.SetRequired('CargoIdentifierKey1', this.ObjectTableName, !value)
    }

    get CargoIdentifierKey2() { return this.entityPM?.CargoIdentifierKey2 }
    set CargoIdentifierKey2(value: string) {
        this.entityPM.CargoIdentifierKey2 = value;
        this.setRequiredCargoKey();
    }

    get CargoIdentifierKey3() { return this.entityPM?.CargoIdentifierKey3 }
    set CargoIdentifierKey3(value: string) {
        this.entityPM.CargoIdentifierKey3 = value;
        this.setRequiredCargoKey();
    }

    get PackagingTypeCode() { return this.entityPM?.PackagingTypeCode }
    set PackagingTypeCode(value: string) { this.entityPM.PackagingTypeCode = value; }

    get Quantity() { return this.entityPM?.Quantity }
    set Quantity(value: number) {
        this.entityPM.Quantity = value;
        this.UIProperties.SetRequired('Quantity', this.ObjectTableName, !value)
    }

    get DeliverySiteID() { return this.entityPM?.DeliverySiteID }
    set DeliverySiteID(value: string) { this.entityPM.DeliverySiteID = value; }

    get RequestReason() { return this.entityPM?.RequestReason }
    set RequestReason(value: string) {
        this.entityPM.RequestReason = value;
        this.UIProperties.SetRequired('RequestReason', this.ObjectTableName, !value)
    }

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
        public entityArgs: EntityArgs,
    ) {
        super();
        this.entityPM = new LogisticActionRequestPM();
        this.EntityResourceService.getEntityResourceByTableName("Customs.LogisticActionRequestType").subscribe(response => { });

        this.Listen();
    }


    SetWindowArgs(winArg: any) {
        this.entityPM = winArg.CurrentEntity;

        this.setTransportModeId();
        this.setRequiredCargoKey();
    }


    private setRequiredFields() {
        this.requierdFieldsList.forEach(fieldName => this.UIProperties.SetRequired(fieldName, this.ObjectTableName, !this[fieldName]));
    }


    async setRequiredCargoKey() {
        const cargoIdentifireTypeTable: CargoIdentifireTypeList[] = await this.logtuideTableDataService.getTable("Customs.CargoIdentifireType")
        const cargoIdentifireType: CargoIdentifireTypeList = cargoIdentifireTypeTable.find(x => x.Code == this.entityPM.CargoIdentifierType);
        this.UIProperties.SetRequired('CargoIdentifierKey2', this.ObjectTableName, !this.entityPM.CargoIdentifierKey2 && cargoIdentifireType.IsKey2Mandatory)
        this.UIProperties.SetRequired('CargoIdentifierKey3', this.ObjectTableName, !this.entityPM.CargoIdentifierKey3 && cargoIdentifireType.IsKey3Mandatory)

        this.removeFromArray(this.requierdFieldsList, 'CargoIdentifierKey2')
        if (!this.entityPM.CargoIdentifierKey2 && cargoIdentifireType.IsKey2Mandatory)
            this.requierdFieldsList.push('CargoIdentifierKey2')

        this.removeFromArray(this.requierdFieldsList, 'CargoIdentifierKey3')
        if (!this.entityPM.CargoIdentifierKey3 && cargoIdentifireType.IsKey2Mandatory)
            this.requierdFieldsList.push('CargoIdentifierKey3')
    }


    private removeFromArray(arr: string[], val: string) {
        const index = arr.indexOf(val);
        if (index !== -1)
            arr.splice(index, 1);
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


    private async getDeclarationsandConsignment(): Promise<ConsignmentDeclartion> {
        const res: ConsignmentDeclartion = await new DeclarationWebService().getDeclarationConsignment(this.entityPM.ExportFileNo)
        return res;
    }


    private Listen() {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {
        }
    }


    ngOnInit() {
        if (!this.entityPM.RequestDate)
            this.initDefaultValue();

        this.subscribesyncDeclaration()

        this.UIProperties.SetEnabled("RequestCancelStatus", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("RequestNumber", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DecisionRmarks", this.ObjectTableName, false);

        this.setRequiredFields();
    }


    ngOnDestroy() {
        this.subscriber.unsubscribe();
    }


    initDefaultValue() {
        this.entityPM.RequestDate = new Date();
        this.entityPM.RequestType = '2';
        this.entityPM.Tenant = SessionLocator.Tenant;
        this.entityPM.Direction = 'E'
        this.entityPM.ExporterIdentifierType = '1'
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
        if (this.invalidate()) return;
        SessionLocator.SelectedSession.StartBusyIndicator("");
    }


    SaveEntityChanges() {
        const isInsert: boolean = !this.entityPM.Id;

        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

        (isInsert ? this.logisticActionRequestPMService.insert(this.entityPM) : this.logisticActionRequestPMService.update(this.entityPM))
            .subscribe((myResponse: ServiceResponse) => {
                SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");
                SessionLocator.SelectedSession.StopBusyIndicator();
            });
    }


    invalidate(): boolean {
        this.ValidationErrorsList = []
        this.requierdFieldsList
            .filter(filed => !this[filed])
            .forEach(filed =>
                this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.LogisticActionRequest.F." + filed))));

        // Validator.TryValidateObject(this.entityPM, this.ObjectTableName,  this.ValidationErrorsList);
        return !!this.ValidationErrorsList.length;
    }


    OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        this.SaveEntityChanges();

        var param = new LogisticActionRequestRequestParams();
        param.ExporterIdentifierType =  this.entityPM.ExporterIdentifierType;
        param.ExporterNumber = this.entityPM.ExporterNumber;
        param.PassportCountry = this.entityPM.PassportCountry;
        param.PassportNumber = this.entityPM.PassportNumber;
        param.RequestType = this.entityPM.RequestType;
        param.RequestReason = this.entityPM.RequestReason;
        param.DeliverySiteID = this.entityPM.DeliverySiteID;
        param.CargoIdentifierType = this.entityPM.CargoIdentifierType;
        param.CargoIdentifierKey1 = this.entityPM.CargoIdentifierKey1;
        param.CargoIdentifierKey2 = this.entityPM.CargoIdentifierKey2;
        param.CargoIdentifierKey3 = this.entityPM.CargoIdentifierKey3;
        param.PackagingTypeCode = this.entityPM.PackagingTypeCode;
        param.Quantity = this.entityPM.Quantity;
        param.LogisticActionRequestId = this.entityPM.Id;
        param.CustomsFile = this.entityPM.ExportFileNo;
    }


    OkButtonClicked() {
        if (this.invalidate()) return;

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
        windowArgs.ObjectTableName = this.ObjectTableName;

        var windowTitle = "Customs.Declaration.TH.Documents";

        var logWindow = new LogitudeWindow();
        logWindow.IsHideHeader = true;
        logWindow.Width = 1000;
        logWindow.Height = 700;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
      logWindow.WindowClosed.subscribe(($event: any) => this.OnDocumentsWindowClosed($event));
      this.entityArgs.SkipCtor = true;
      logWindow.Show('./CustomsModules/CustomsDocuments/Components/CustomsDocumentsComponent');
    }

    OnDocumentsWindowClosed(event) {
        this.entityArgs.SkipCtor = false;
      }
  
}
