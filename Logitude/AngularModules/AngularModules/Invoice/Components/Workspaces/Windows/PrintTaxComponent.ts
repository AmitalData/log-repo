import {Component} from '@angular/core';
import {AppTool, FormatTool} from '../../../../Infrastructure/Tools';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {InvoiceDomainService} from '../../../Services/InvoiceDomainService';

@Component({
    moduleId: module.id,
    templateUrl: './PrintTaxComponent.html',
})

export class PrintTaxComponent extends BaseComponent {
    public DataContext = this;
    public TransferTypeCode: string = null;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    
    SetWindowArgs(transferTypeCode: string) {
    }
    
    private date1: Date;
    get Date1() { return this.date1; }
    set Date1(value: Date) {
        if (this.date1 != value) {
            this.date1 = value;
        }
    }

    private date2: Date;
    get Date2() { return this.date2; }
    set Date2(value: Date) {
        if (this.date2 != value) {
            this.date2 = value;
        }
    }

    private email: string;
    get Email() { return this.email; }
    set Email(value: string) {
        if (this.email != value) {
            this.email = value;
        }
    }
    
    SendButtonClicked() {
        var errors: string[] = [];
        
        if (this.Date1 == null) {
            errors.push("Start Date must be determined");
        }

        if (this.Date2 == null) {
            errors.push("End Date must be determined");
        }

        if (AppTool.IsNullOrEmpty(this.Email)) {
            errors.push("E-mail must be determined");
        }

        else {
            if (!FormatTool.IsEmail(this.Email)) {
                errors.push("The E-mail you entered is not valid");
            }
        }

        if (this.Date1 != null && this.Date2 != null) {
            if (this.Date1 > this.Date2) {
                errors.push("Start Date must be less than end date");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            this.CurrentSession.StartBusyIndicator("Building Files...");
            var service: InvoiceDomainService = new InvoiceDomainService();
            service.PrintTaxData(this.Date1, this.Date2, this.Email).subscribe((myResponse: ServiceResponse) => {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindowEmit("Ok");                
            });
        }
    }
    
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
