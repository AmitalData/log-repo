import { LoginService } from "../../../Infrastructure/Services/LoginService";
import { CachedDataManager } from "../../../Infrastructure/Utilities/CachedDataManager";
import { SessionLocator } from "../../../Infrastructure/Utilities/SessionLocator";

declare var window: any;
export class MetadataService {
    private isRefreshCustomFieldsCompleted: boolean = false;
    private isRefreshTextCodesCompleted: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    private loginService: LoginService;
    constructor() {
        this.isRefreshCustomFieldsCompleted = false;
        this.isRefreshTextCodesCompleted = false;
        this.loginService = new LoginService();
        this.loginService.CurrentTenant = SessionLocator.Tenant;
    }

    public Refresh() {
        this.CurrentSession.StartBusyIndicator("Applying Changes ...");
        this.LoadCustomFields();
        this.LoadTextCodes();
    }

    LoadCustomFields() {
        window.ObjectFields = window.ObjectFields.filter(field => field.Tenant != SessionLocator.Tenant);
        this.loginService.GeLoggedTenantObjectFields().subscribe((response: any) => {
            if (response) {
                window.ObjectFields = window.ObjectFields.concat(response);
                this.isRefreshCustomFieldsCompleted = true;
                this.Complete();
            }
        });
    }

    LoadTextCodes() {
        CachedDataManager.RefreshTenantTextCodes().subscribe((response: any) => {
            this.isRefreshTextCodesCompleted = true;
            this.Complete();
        });
    }

    Complete() {
        if (!this.isRefreshCustomFieldsCompleted || !this.isRefreshTextCodesCompleted) return;
        this.CurrentSession.StopBusyIndicator();
        if (this.CurrentSession.CurrentWindow == null) return;
        this.CurrentSession.CloseCurrentWindow();
    }

}
