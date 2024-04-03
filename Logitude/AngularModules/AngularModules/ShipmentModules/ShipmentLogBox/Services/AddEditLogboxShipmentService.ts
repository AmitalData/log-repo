import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { SystemEnvironmentService } from '../../../Infrastructure/Utilities/SystemEnvironmentService';

export class AddEditLogboxShipmentService {
    public shipmentObjectTableName = "Shipment";
    public CustomerTenantAccessRequestPartner: any;
    public IsLogbox: boolean = SystemEnvironmentService.IsLogBox();
    public IsPrivateLabel: boolean = SessionLocator.PrivateLableSettings ? true : false;
    public IsDSV: boolean = false;
    public IsCustomsActivated: boolean = false;
    public IsExportActivated: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(customerTenantAccessRequestPartner) {
        this.Initialize(customerTenantAccessRequestPartner);
    }

    private Initialize(customerTenantAccessRequestPartner) {
        this.InitializePrivateLabel(customerTenantAccessRequestPartner);
    }

    private InitializePrivateLabel(customerTenantAccessRequestPartner) {
        if (!this.IsPrivateLabel) return;
        if (!customerTenantAccessRequestPartner) return;
        this.CustomerTenantAccessRequestPartner = customerTenantAccessRequestPartner;
        this.IsDSV = SessionLocator.PrivateLableSettings.PrivateLabelDomain.toLowerCase().indexOf("dsv") > -1;
        this.SetActivatedCustomerDirections();
    }

    private SetActivatedCustomerDirections() {
        let isPrivateLabelCustomsActivated = SessionLocator.PrivateLableSettings.IsCustomsActivated;
        let isPrivateLabelExportActivated = SessionLocator.PrivateLableSettings.IsExportActivated;
        this.IsCustomsActivated = this.CustomerTenantAccessRequestPartner.IsCustoms && isPrivateLabelCustomsActivated;
        this.IsExportActivated = this.CustomerTenantAccessRequestPartner.IsExport && isPrivateLabelExportActivated;
    }

    public GetNewShipmentWindow(newArgs) {
        let entityResourceService = new EntityResourceService();
        let windowArgs: any = {};
        windowArgs.IsNew = true;
        windowArgs.HideDocumentSection = newArgs.HideDocumentSection;
        let logitudeWindow = new LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Title = "Create New Shipment";
        windowArgs.IsCustomsActivated = this.IsCustomsActivated;
        windowArgs.IsExportActivate = this.IsExportActivated;
        this.CurrentSession.StartBusyIndicatorLoading();
        entityResourceService.getEntityResourceByTableName(this.shipmentObjectTableName).subscribe((response: any) => {
            this.CurrentSession.StopBusyIndicator();
            let newWindowComponentPath = this.GetWindowComponentPath(logitudeWindow);
            logitudeWindow.Show(newWindowComponentPath);
        });

        return logitudeWindow;
    }

    private GetWindowComponentPath(newWindow: LogitudeWindow) {
        let newWindowComponentPath = './ShipmentModules/ShipmentLogBox/Components/Logbox/';
        if (this.hasExportShipmentOption()) {
            return this.LoadNewAddShipmentComponent(newWindow, newWindowComponentPath);
        }

        return this.LoadAddEditComponent(newWindow, newWindowComponentPath);
    }

    private hasExportShipmentOption() {
        return !this.IsLogbox && (this.IsExportActivated);
    }

    private LoadNewAddShipmentComponent(newWindow: LogitudeWindow, newWindowComponentPath: string) {
        newWindow.Width = this.IsPrivateLabel ? 960 : 600;
        newWindow.Height = this.IsPrivateLabel ? 600 : 350;
        newWindowComponentPath += this.IsPrivateLabel ? 'AddEditPrivateLabelShipmentComponent' : 'AddEditImporterShipmentComponent';
        return newWindowComponentPath;
    }

    private LoadAddEditComponent(newWindow: LogitudeWindow, newWindowComponentPath: string) {
        newWindow.Width = 600;
        newWindow.Height = this.IsPrivateLabel ? (this.IsDSV ? 376 : 420) : 350;
        newWindowComponentPath += this.IsPrivateLabel ? 'AddEditPrivateLabelCustomsShipmentComponent' : 'AddEditImporterShipmentComponent';
        return newWindowComponentPath;
    }
}
