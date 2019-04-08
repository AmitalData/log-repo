
declare var System: any;
declare var window: any;
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import 'rxjs/add/operator/map';
import {Component, OnInit }  from '@angular/core';



import {AppTool} from '../../../../Infrastructure/Tools';
@Component({
    moduleId: module.id,

    selector: 'CreateTenantValidationScreenComponent',
    templateUrl: './CreateTenantValidationScreenComponent.html',


})
export class CreateTenantValidationScreenComponent implements OnInit {

    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: string[];
    constructor() {


    }

    ngOnInit(


    ) {

    }

    SetWindowArgs(args: any) {
        var messageError: string = args.MessageError;
        var contactEmail: string = args.ContactEmail;
        var contactName: string = args.ContactName;
        var customerName: string = args.CustomerName;
        this.ValidationErrorsList = [];

        if (!AppTool.IsNullOrEmpty(messageError)) {
            this.ValidationErrorsList.push(messageError);
        }

        else {

            if (AppTool.IsNullOrEmpty(contactEmail)) this.ValidationErrorsList.push("Contact Email Field is Required");
            if (AppTool.IsNullOrEmpty(contactName)) this.ValidationErrorsList.push("Contact Name Field is Required");
            if (AppTool.IsNullOrEmpty(customerName)) this.ValidationErrorsList.push("Customer Name Field is Required");

        }
    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }






}
