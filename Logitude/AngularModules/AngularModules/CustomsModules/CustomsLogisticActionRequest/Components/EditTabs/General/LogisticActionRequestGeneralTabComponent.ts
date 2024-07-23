import { Component, Output, EventEmitter, Input, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { ClientList } from 'Customs/EntityLists/ClientList';
import { LogitudeWindow } from 'Controls/Windows/LogitudeWindow';
import { CargoIdentifireTypeList } from 'Customs/EntityLists/CargoIdentifireTypeList';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';
import { Subscription } from 'rxjs';
import { ConsignmentDeclartions, DeclarationWebService } from 'Customs/Services/WebServices/DeclarationWebService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { LogisticActionRequestPMService } from 'Customs/Services/StandardPMs/LogisticActionRequestPMService';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { LogisticActionRequestRequestParams } from 'Customs/DataContract/RequestParams/LogisticActionRequestRequestParams';
import { LogisticActionRequestWebService } from 'Customs/Services/WebServices/LogisticActionRequestWebService';
import { ResponseDataBase } from 'Customs/DataContract/ResponseData/ResponseDataBase';
import { CustomMessageProgressComponent } from 'CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { ClientListService } from 'Customs/Services/StandardLists/ClientListService'
import { CargoIdentifireTypeListService } from 'Customs/Services/StandardLists/CargoIdentifireTypeListService';
import { CargoIdentifireTypePM } from 'Customs/EntityPMs/CargoIdentifireTypePM';
import { LogisticActionRequestService } from 'Customs/Services/Others/LogisticActionRequestService';
import { loggerService } from 'Infrastructure/Utilities/logger.service';
import { LogisticActionRequestPM } from 'Customs/EntityPMs/LogisticActionRequestPM';
import { DeclarationPM } from 'Customs/EntityPMs/DeclarationPM';
import { LogisticActionRequestsCloseSharedDataService } from 'Customs/Services/DataChange/LogisticActionRequestCloseSharedDataService';
import { ConsignmentPM } from 'Customs/EntityPMs/ConsignmentPM';
import { LogtuideTableDataService } from 'Infrastructure/Services/logtuide-table-data.service';

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
    submit: boolean = false;
    exporterName: string = ''
    subscriber: Subscription;
    ValidationErrorsList: any[] = [];
    declartionVal: DeclarationPM;
    private clientListService: ClientListService = null;
    @Input() QueryFilterItems: ApiQueryFilters;
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
        'RequestReason',
    ]
    public isTransportA: boolean = false;
    public isTransportO: boolean = false;
    public isTransportL: boolean = false;
    public SecondCargoIDPlaceholder: string = " ";
    public ThirdCargoIdPlaceholder: string = " ";
    public ManifestNumberPlaceholder: string = " ";
    public isEntityChange: boolean = false;
    _CargoIdentifireTypeListService: CargoIdentifireTypeListService = new CargoIdentifireTypeListService();
    LogisticActionRequestService: LogisticActionRequestService = new LogisticActionRequestService();
    exportFileNoCurrentValue: string = '';
    private isImporterClicked: boolean = false;
    private currentClient: ClientList;
    consignments: ConsignmentPM[] = [];
    consignmentSelected: ConsignmentPM;

    get ExportFileNo() { return this.entityPM?.ExportFileNo }
    set ExportFileNo(value: string) {
        this.entityPM.ExportFileNo = value;
        this.setRequiredField('ExportFileNo', !value)
    }

    get ExporterNumber() { return this.entityPM?.ExporterNumber }
    set ExporterNumber(value: string) {
        this.entityPM.ExporterNumber = value;
        this.setRequiredField('ExporterNumber', !value)
    }

    get RequestDate() { return this.entityPM?.RequestDate }
    set RequestDate(value: Date) { this.entityPM.RequestDate = value; }

    get RequestType() { return this.entityPM?.RequestType }
    set RequestType(value: string) {
        this.entityPM.RequestType = value;
        this.setRequiredField('RequestType', !value)
    }

    get CargoIdentifierType() { return this.entityPM?.CargoIdentifierType }
    set CargoIdentifierType(value: string) {
        this.entityPM.CargoIdentifierType = value;

        if (value) {
            this.setRequiredCargoKey();
            this.setPlaceholderForCargoKey();
        }
    }

    get CargoIdentifierKey1() { return this.entityPM?.CargoIdentifierKey1 }
    set CargoIdentifierKey1(value: string) {
        this.entityPM.CargoIdentifierKey1 = value;
        this.setRequiredField('CargoIdentifierKey1', !value)
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
    set PackagingTypeCode(value: string) {
        this.entityPM.PackagingTypeCode = value;
        this.setRequiredField('PackagingTypeCode', !value)
    }

    get Quantity() { return this.entityPM?.Quantity }
    set Quantity(value: number) {
        this.entityPM.Quantity = value;
        this.setRequiredField('Quantity', !value)
    }

    get DeliverySiteID() { return this.entityPM?.DeliverySiteID }
    set DeliverySiteID(value: string) { this.entityPM.DeliverySiteID = value; }

    get RequestReason() { return this.entityPM?.RequestReason }
    set RequestReason(value: string) {
        this.entityPM.RequestReason = value;
        this.setRequiredField('RequestReason', !value)
    }

    get RequestCancelStatus() { return this.entityPM?.RequestCancelStatus }
    set RequestCancelStatus(value: string) { this.entityPM.RequestCancelStatus = value; }

    get RequestNumber() { return this.entityPM?.RequestNumber }
    set RequestNumber(value: string) { this.entityPM.RequestNumber = value; }

    get DecisionRmarks() { return this.entityPM?.DecisionRmarks }
    set DecisionRmarks(value: string) { this.entityPM.DecisionRmarks = value; }

    get CalculatedExporterName() { return this.entityPM?.CalculatedExporterName }
    set CalculatedExporterName(newValue: string) { this.entityPM.CalculatedExporterName = newValue; }

    constructor(
        private logisticActionRequestPMService: LogisticActionRequestPMService,
        private EntityResourceService: EntityResourceService,
         private logtuideTableDataService: LogtuideTableDataService,
        private cdr: ChangeDetectorRef,
        public entityArgs: EntityArgs,
        public logisticActionRequestWebService: LogisticActionRequestWebService,
        private logger: loggerService,
        private _logisticActionRequestsCloseSharedDataService: LogisticActionRequestsCloseSharedDataService,
    ) {
        super();
        this.entityPM = new LogisticActionRequestPM();
        this.EntityResourceService.getEntityResourceByTableName("Customs.LogisticActionRequestType").subscribe(response => { });
        this.logger.loggerAppSettingsName = "20220301.LogUntilDateyyyyMMdd";

        this.Listen();
    }


    SetWindowArgs(winArg: any) {
        this.entityPM = winArg.CurrentEntity;
        this.exportFileNoCurrentValue = this.entityPM.ExportFileNo;

        this.setRequiredCargoKey();
        this.setPlaceholderForCargoKey()
        this.initConsignmentsRule();
    }


    async initConsignmentsRule() {
        if(!this.entityPM.ExportFileNo) return;

        const consignmentDeclartion: ConsignmentDeclartions = await this.getDeclarationsandConsignment();
        if (consignmentDeclartion.Declarations.length !== 1) return;

        const consignment: ConsignmentPM = consignmentDeclartion.Consignments.find(x=> x.ManifestNumber == this.CargoIdentifierKey1);
        if(!consignment) return;

        this.consignments = consignmentDeclartion.Consignments;
        this.consignmentSelected = consignment
        this.setDisabledConsignmentFields();
    }


    private setRequiredFields() {
        this.requierdFieldsList.forEach(fieldName => this.UIProperties.SetWarning(fieldName, this.ObjectTableName, !this[fieldName]));
    }


    async setRequiredCargoKey() {
        const cargoIdentifireTypeTable: CargoIdentifireTypeList[] =  await this.logtuideTableDataService.getTable("Customs.CargoIdentifireType")
        const cargoIdentifireType: CargoIdentifireTypeList = cargoIdentifireTypeTable.find(x => x.Code == this.entityPM.CargoIdentifierType);
        this.setRequiredField('CargoIdentifierKey2', !this.entityPM.CargoIdentifierKey2 && cargoIdentifireType.IsKey2Mandatory)
        this.setRequiredField('CargoIdentifierKey3', !this.entityPM.CargoIdentifierKey3 && cargoIdentifireType.IsKey3Mandatory)
    }


    setRequiredField(name: string, fieldIsRequired: boolean) {
        this.UIProperties.SetWarning(name, this.ObjectTableName, fieldIsRequired);

        if (fieldIsRequired) {
            if (this.requierdFieldsList.every(x => x != name))
                this.requierdFieldsList.push(name)
        } else
            this.removeFromArray(this.requierdFieldsList, name)

        // this.invalidate()
    }


    private removeFromArray(arr: string[], val: string) {
        const index = arr.indexOf(val);
        if (index !== -1)
            arr.splice(index, 1);
    }

    private async syncDeclaration() {
        const consignmentDeclartion: ConsignmentDeclartions = await this.getDeclarationsandConsignment();

        if (consignmentDeclartion.Declarations.length !== 1 || !(await this.confirmSyncDeclaration())) return;

        const declaration: DeclarationPM = consignmentDeclartion.Declarations[0];

        this.ExporterNumber = declaration.ImporterCode;
        this.entityPM.DeclarationId = declaration.Id;
        this.entityPM.DeclarationNumber = declaration.DeclarationNumber;

        this.declartionVal = declaration;
        if (consignmentDeclartion.ConsignmentPackages.length === 1) {
            this.entityPM.PackagingTypeCode = consignmentDeclartion.ConsignmentPackages[0].PackageTypeCode
            this.entityPM.Quantity = consignmentDeclartion.ConsignmentPackages[0].Quantity;
        }

        this.consignments = consignmentDeclartion.Consignments;
        if (this.consignments.length > 0) {
            const consignment: ConsignmentPM = this.consignments[0];
            this.CargoIdentifierType = consignment.CargoTypeCode
            this.DeliverySiteID = consignment.StorageSiteCode
            this.selectCargoIdentifierKey(consignment)
        }

        this.setDisabledConsignmentFields();
        this.cdr.detectChanges();
    }

    selectCargoIdentifierKey(consignment: ConsignmentPM) {
        this.CargoIdentifierKey1 = consignment.ManifestNumber;
        this.CargoIdentifierKey2 = consignment.SecondCargoID;
        this.CargoIdentifierKey3 = consignment.ThirdCargoID;

        if(this.consignmentSelected != consignment)
            this.consignmentSelected = consignment
    }

    private async openConfirmWindow(msg: string): Promise<boolean> {
        const myConfirmWindow = new ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.Show(msg);
        const res = await myConfirmWindow.WindowClosedPromise() as any;
        return res.Yes;
    }


    private async confirmSyncDeclaration() {
        const exporterName = this.exporterName ? TextCodeTranslator.Translate('Customs.LogisticActionRequest.O.ForImporter') + ' ' + this.exporterName + ' ' : '';

        const myConfirmWindow = new ConfirmWindow();
        myConfirmWindow.Width = 400;
        myConfirmWindow.Show(`${TextCodeTranslator.Translate('Customs.LogisticActionRequest.O.FindDeclaration')} ${exporterName} ${TextCodeTranslator.Translate('Customs.LogisticActionRequest.O.FindDeclaration2')}?`);

        return new Promise(resolve =>
            myConfirmWindow.WindowClosed.subscribe(event =>
                resolve(myConfirmWindow.Yes)));
    }


    private async getDeclarationsandConsignment(): Promise<ConsignmentDeclartions> {
        const res: ConsignmentDeclartions = await new DeclarationWebService().getDeclarationConsignment(this.entityPM.ExportFileNo)
        return res;
    }


    private Listen() {
        if (SessionLocator.SelectedSession.CurrentEditComponent != null) {
        }
    }


    ngOnInit() {
        if (!this.entityPM.RequestDate)
            this.initDefaultValue();

        // this.subscribesyncDeclaration()

        this.setDisabledFields();
        this.setDisabledConsignmentFields();
        this.setRequiredFields();
        this.clientListService = new ClientListService();
    }


    ngOnDestroy() {
        this.subscriber?.unsubscribe();
        this.restartCounterCloseRequest()
    }


    initDefaultValue() {
        this.entityPM.RequestDate = new Date();
        this.entityPM.RequestType = '2';
        this.entityPM.Tenant = SessionLocator.Tenant;
        this.entityPM.Direction = 'E'
        this.entityPM.ExporterIdentifierType = '1'
    }

    setDisabledConsignmentFields() {
        ["CargoIdentifierKey2", "CargoIdentifierKey3"].forEach((fieldName: string) =>
            this.UIProperties.SetEnabled(fieldName, this.ObjectTableName, !this.consignments?.length));
    }

    setDisabledFields() {
        ["RequestCancelStatus", "RequestNumber", "DecisionRmarks"].forEach((fieldName: string) =>
            this.UIProperties.SetEnabled(fieldName, this.ObjectTableName, false));
    }

    onBlurExportFileNo() {
        if(this.entityPM.ExportFileNo !== this.exportFileNoCurrentValue){
            this.consignments = [];
            this.setDisabledConsignmentFields();

            if (this.entityPM.ExportFileNo)
                this.syncDeclaration()
        }        

        this.exportFileNoCurrentValue = this.entityPM.ExportFileNo;
        // this.syncDeclaration$.next()
    }


    async TransportModeClicked(value: string) {
        if (this.entityPM.TransportmodeId == value) return;
        if (this.entityPM.TransportmodeId) {
            if (await this.openConfirmWindow(TextCodeTranslator.Translate('Customs.LogisticActionRequest.O.ChangeTransportType') + '?'))
                this.clearField();
            else {
                value = this.entityPM.TransportmodeId;
                this.entityPM.TransportmodeId = '';
                this.cdr.detectChanges();
            }
        }

        this.entityPM.TransportmodeId = value;
    }


    private clearField() {
        this.ExportFileNo = '';
        this.ExporterNumber = '';
        this.CargoIdentifierKey1 = '';
        this.CargoIdentifierKey2 = '';
        this.CargoIdentifierKey3 = '';
        this.PackagingTypeCode = '';
        this.Quantity = 0;
        this.DeliverySiteID = '';
        this.RequestReason = '';
    }


    ImporterClicked(type, client: ClientList) {
        this.ExporterNumber = client?.Code;
        this.exporterName = client?.FullName;
        if (client) this.isImporterClicked = true;
        if (client && !AppTool.IsNullOrEmpty(client.FullName)) {
            this.CalculatedExporterName = client.FullName;
            this.currentClient = client;
        }
        else {
            this.CalculatedExporterName = "";
            this.currentClient = null;
        }
    }


    ImporterTextChanged(type, item: any) {
        if (this.currentClient != null && !AppTool.IsNullOrEmpty(item) && this.ExporterNumber == this.currentClient.Code) {
            this.CalculatedExporterName = this.currentClient.FullName;
        }
        else {
            this.CalculatedExporterName = "";
        }
    }

    ImporterLostFocus(type: any, item: any) {
        this.isImporterClicked = false;
        if (this.currentClient == null && !AppTool.IsNullOrEmpty(item)) {
            var filters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 10;

            filters.addAdditionalFilter("Code", item, null, null, "Equal", false, false, false, "string");

            this.clientListService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {

                var itemsCount = 0;

                if (myResponse != null) {
                    itemsCount = myResponse.Result.length;
                    if (itemsCount == 1) {
                        this.currentClient = myResponse.Result;
                        if (this.currentClient != null) {
                            if (!AppTool.IsNullOrEmpty(this.currentClient.Code)) {
                                this.ExporterNumber = this.currentClient.Code;
                            }
                            else {
                                this.ExporterNumber = myResponse.Result[0].Code;
                            }
                            if (!AppTool.IsNullOrEmpty(this.currentClient.FullName)) {
                                this.CalculatedExporterName = this.currentClient.FullName;
                            }
                            else {
                                this.CalculatedExporterName = myResponse.Result[0].FullName;
                            }
                        }
                        else {
                            this.CalculatedExporterName = "";
                        }
                    }
                }
            });
        }

        if (this.currentClient != null && !AppTool.IsNullOrEmpty(item) && this.ExporterNumber == this.currentClient.Code) {
            this.CalculatedExporterName = this.currentClient.FullName;
        }
        else {
            this.CalculatedExporterName = "";
        }
    }

    SendButtonClicked() {
        // if (this.invalidate()) return;
        SessionLocator.SelectedSession.StartBusyIndicator("");
    }


    async SaveEntityChanges(DontClose: boolean = false) {
        this.logger.sendError('start SaveEntityChange', 'entityPM: ' + JSON.stringify(this.entityPM));
        const isInsert: boolean = !this.entityPM.Id;
        SessionLocator.SelectedSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        const errMess = await this.CheckIfLogisticActionRequestExist();
        if (!AppTool.IsNullOrEmpty(errMess)) {
            this.ValidationErrorsList.push(errMess);
            SessionLocator.SelectedSession.StopBusyIndicator();
            return;
        }
        const res = 
        await this.logtuideTableDataService.getDataFromService(
           (isInsert ? this.logisticActionRequestPMService.insert(this.entityPM) : this.logisticActionRequestPMService.update(this.entityPM)))

        this.isEntityChange = true;
            
        this.logger.sendError('after save', 'res: ', JSON.stringify(res));

        if (!DontClose)
            SessionLocator.SelectedSession.CloseCurrentWindowEmit("ok");

        SessionLocator.SelectedSession.StopBusyIndicator();
        return res;
    }

    async CheckIfLogisticActionRequestExist() {
        return new Promise(resolve => {
            this.LogisticActionRequestService.GetIfLogisticActionRequestExists(this.entityPM.Id, this.entityPM.CargoIdentifierKey1, this.entityPM.CargoIdentifierKey2, this.entityPM.CargoIdentifierKey3, this.entityPM.CargoIdentifierType).subscribe((Result: any) => {
                const mm: ServiceResponse = Result;
                if (!mm.HasError && mm.Result)
                    resolve(TextCodeTranslator.Translate("Customs.General.O.LogisticActionRequestAlreadyExist"));
                resolve("");
            });
        });
    }

    invalidate(): boolean {
        if (!this.submit) return;

        this.ValidationErrorsList = []
        this.requierdFieldsList
            .filter(filed => !this[filed])
            .forEach(filed =>
                this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("Customs.LogisticActionRequest.F." + filed))));

        // Validator.TryValidateObject(this.entityPM, this.ObjectTableName,  this.ValidationErrorsList);
        this.logger.sendError('is invalid', 'ValidationErrorsList: ' + JSON.stringify(this.ValidationErrorsList))
        return !!this.ValidationErrorsList.length;
    }


    async OnCustomSendOptionsButtonClick(customSendOptionsArgs) {
        this.logger.sendError('OnCustomSendOptionsButtonClick')
        this.submit = true;
        if (this.invalidate()) return;

        const entity: LogisticActionRequestPM = await this.SaveEntityChanges();
        this.entityPM.Id = entity.Id;

        SessionLocator.SelectedSession.StartBusyIndicatorSaving();

        const param: LogisticActionRequestRequestParams = await this.getCustomsParams();
        this.logger.sendError('param', 'param: ' + JSON.stringify(param));
        const res: ResponseDataBase = await this.logisticActionRequestWebService.SendCustomsMessage8410(param);
        this.logger.sendError('after SendCustomsMessage8410', 'res: ' + JSON.stringify(res));

        CustomMessageProgressComponent
            .ShowProgressBar(SessionLocator.SelectedSession, param.PBId, TextCodeTranslator.Translate('Customs.LogisticActionRequest.O.CancelRequestImporter'), false)
            .catch((err) => {
                this.logger.sendError('after err', 'err: ' + JSON.stringify(err));
                this.ValidationErrorsList.push(err)
            });
    }


    private async getCustomsParams() {
        const param: LogisticActionRequestRequestParams = new LogisticActionRequestRequestParams();
        param.LogisticActionRequestId = this.entityPM.Id;
        param.ExporterIdentifierType = this.entityPM.ExporterIdentifierType;
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
        param.CustomsFile = this.declartionVal?.CustomFileNo;
        param.Tenant = this.entityPM.Tenant;
        param.ExportFile = this.entityPM.ExportFileNo;

        if (!AppTool.IsNullOrEmpty(this.entityPM.DeclarationId) && AppTool.IsNullOrEmpty(param.CustomsFile)) {
            var consignmentDeclartion: ConsignmentDeclartions = await this.getDeclarationsandConsignment();

            const declaration: DeclarationPM = (consignmentDeclartion as any).Declarations[0];
            param.CustomsFile = declaration?.CustomFileNo;

        }

        return param;
    }

    OkButtonClicked() {
        this.submit = true;
        // if (this.invalidate()) return;
        this.logger.sendError('OkButtonClicked');
        this.SaveEntityChanges();
    }


    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindowEmit("Cancel");
    }


    private restartCounterCloseRequest() {
        this._logisticActionRequestsCloseSharedDataService._SelectedItems.Clear();
        this._logisticActionRequestsCloseSharedDataService.IsDisplayButtonClose = false;
    }


    RefreshEntity() {
        SessionLocator.SelectedSession.CurrentEditComponent?.ReloadEntityPM();
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
        logWindow.Title = TextCodeTranslator.Translate('Customs.General.O.MoreDetailsForImporter');
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CustomsModules/CustomsLogisticActionRequest/Components/EditTabs/MoreDetailesforImporterComponent/MoreDetailesforImporterComponent');
        logWindow.WindowClosed.subscribe(() => {
            this.setRequiredField('ExporterNumber', this.entityPM.ExporterIdentifierType == '1' && !this.entityPM.ExporterNumber)
            this.setRequiredField('PassportNumber', this.entityPM.ExporterIdentifierType == '2' && !this.entityPM.PassportNumber)
            this.setRequiredField('PassportCountry', this.entityPM.ExporterIdentifierType == '2' && !this.entityPM.PassportCountry)
        })
    }


    async ViewDocumentsComponent() {

        //save entity
        this.submit = true;
        //this.SaveEntityChanges(true);
        const entity: LogisticActionRequestPM = await this.SaveEntityChanges(true);
        this.entityPM.Id = entity.Id;

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


    private async setPlaceholderForCargoKey() {
        const res: CargoIdentifireTypePM =    await this.logtuideTableDataService.getDataFromService(this._CargoIdentifireTypeListService.getSingleFromCache(this.entityPM.CargoIdentifierType))

        this.ManifestNumberPlaceholder = res.CargoIdentifierKey1Name;
        this.SecondCargoIDPlaceholder = res.CargoIdentifierKey2Name ?? '';
        this.ThirdCargoIdPlaceholder = res.CargoIdentifierKey3Name ?? '';
    }
}
