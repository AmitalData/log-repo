import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { TenantManagementPM } from '../../../../Infrastructure/EntityPMs/TenantManagementPM';
import { GlobalDomainService } from '../../../../Common/Services/GlobalDomainService';
import { AppTool } from '../../../../Infrastructure/Tools';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

@Component({

    templateUrl: './AddEditPIMAComponent.html',
})

export class AddEditPIMAComponent extends BaseComponent {
    public EntityPM: TenantManagementPM;
    public ObjectTableName: string = "TenantManagement";
    public DataContext = this;
    private iGlobalDomainService: GlobalDomainService;

    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityResourceService: EntityResourceService) {
        super();
        this.iGlobalDomainService = new GlobalDomainService();
    }

    SetWindowArgs(args: any) {
        this.EntityPM = args['EntityPM'];
        this.Clone();
    }

    get PIMA() { return this.EntityPM.PIMA; }
    set PIMA(newValue: string) {
        if (this.EntityPM.PIMA != newValue) {
            this.EntityPM.PIMA = newValue;
        }
    }

   
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        this.iGlobalDomainService.GetTenantManagmentPIMA(this.PIMA, this.EntityPM.Id).subscribe((result: any) => {
            var duplicationMsg = result.Result;
            if (AppTool.IsNullOrEmpty(duplicationMsg)) {
                this.CurrentSession.CloseCurrentWindow();
            }

            else {
                this.ViewPIMADuplicationConfirmationWindow(duplicationMsg);
            }
        });
       
    }

    ViewPIMADuplicationConfirmationWindow(duplicationMsg) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(duplicationMsg);
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.CloseCurrentWindow();
            }
        });
    }
    

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('PIMA');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
