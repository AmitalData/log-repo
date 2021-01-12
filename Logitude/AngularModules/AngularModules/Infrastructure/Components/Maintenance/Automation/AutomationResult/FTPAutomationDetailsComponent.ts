import { Component} from '@angular/core';
import { BaseComponent } from '../../../LogitudeComponents/BaseComponent';
import { FTPAutomationDetails } from '../../../../DataContracts/AutomationSendInterface';
import { SessionLocator } from '../../../../Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Utilities/TextCodeTranslator';
import { AppTool } from '../../../../Tools';


@Component({
    selector: 'SendInterfaceResult',
    templateUrl: './FTPAutomationDetailsComponent.html',
    inputs: [''],

})
export class FTPAutomationDetailsComponent extends BaseComponent {

    public DataContext: any;
    private CurrentSession = SessionLocator.SelectedSession;

    public FTPAutomationDetails: FTPAutomationDetails;
    public BasicDisplayMode: boolean = false;
    public ObjectTableName: string = "FTPAutomationDetails";
    constructor() {
        super();

        this.DataContext = this;
    }

    SetWindowArgs(args: any) {
        this.FTPAutomationDetails = args.FTPAutomationDetails;
        if (this.FTPAutomationDetails) {
            this.Folder = this.FTPAutomationDetails.Folder;
            this.Host = this.FTPAutomationDetails.Host;
            this.Password = this.FTPAutomationDetails.Password;
            this.UserName = this.FTPAutomationDetails.UserName;


            //this.Prefix = this.FTPAutomationDetails.Prefix;
            //this.Subject = this.FTPAutomationDetails.Subject;
            //this.Suffix = this.FTPAutomationDetails.Suffix;
            //this.Extension = this.FTPAutomationDetails.Extension;
            //this.From = this.FTPAutomationDetails.From;

        }

    }
    ValidationErrorsList: string[];
    SaveButtonClicked() {


        this.ValidationErrorsList = [];
        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrEmpty(this.DataContext.UserName)) this.ValidationErrorsList.push(msg.replace("%FieldName", "UserName"));
        if (AppTool.IsNullOrEmpty(this.DataContext.Password)) this.ValidationErrorsList.push(msg.replace("%FieldName", "Password"));
        if (AppTool.IsNullOrEmpty(this.DataContext.Folder)) this.ValidationErrorsList.push(msg.replace("%FieldName", "Folder"));
        if (AppTool.IsNullOrEmpty(this.DataContext.Host)) this.ValidationErrorsList.push(msg.replace("%FieldName", "Host"));


        if (this.ValidationErrorsList.length == 0) {

            this.FTPAutomationDetails.Folder = this.Folder;
            this.FTPAutomationDetails.Host = this.Host;
            this.FTPAutomationDetails.Password = this.Password;
            this.FTPAutomationDetails.UserName = this.UserName;

            this.CurrentSession.CurrentWindow.Close("Changed");

        }



            //this.FTPAutomationDetails.Extension = this.Extension;
            //this.FTPAutomationDetails.Prefix = this.Prefix;
            //this.FTPAutomationDetails.Subject = this.Subject;
            //this.FTPAutomationDetails.Suffix = this.Suffix;
        //this.FTPAutomationDetails.From = this.From;












    }


    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }
    //ExtensionLostFocus(input: any) {
    //    if (this.FTPAutomationDetails.Extension && this.FTPAutomationDetails.Extension.startsWith("."))
    //        this.FTPAutomationDetails.Extension = this.FTPAutomationDetails.Extension.substring(1, this.FTPAutomationDetails.Extension.length);

    //}




    //private suffix: string;
    //get Suffix() { return this.suffix; }
    //set Suffix(value: string) {
    //    if (this.suffix != value) {
    //        this.suffix = value;
    //    }
    //}



    //private subject: string;
    //get Subject() { return this.subject; }
    //set Subject(value: string) {
    //    if (this.subject != value) {
    //        this.subject = value;
    //    }
    //}

    //private from: string;
    //get From() { return this.from; }
    //set From(value: string) {
    //    if (this.from != value) {
    //        this.from = value;
    //    }
    //}


    //private prefix: string;
    //get Prefix() { return this.prefix; }
    //set Prefix(value: string) {
    //    if (this.prefix != value) {
    //        this.prefix = value;
    //    }
    //}


    //private extension: string;
    //get Extension() { return this.extension; }
    //set Extension(value: string) {
    //    if (this.extension != value) {
    //        this.extension = value;
    //    }
    //}


    private userName: string;
    get UserName() { return this.userName; }
    set UserName(value: string) {
        if (this.userName != value) {
            this.userName= value;
        }
    }


    private password: string;
    get Password() { return this.password; }
    set Password(value: string) {
        if (this.password != value) {
            this.password = value;
        }
    }

    private host: string;
    get Host() { return this.host; }
    set Host(value: string) {
        if (this.host != value) {
            this.host = value;
        }
    }

    private folder: string;
    get Folder() { return this.folder; }
    set Folder(value: string) {
        if (this.folder != value) {
            this.folder = value;
        }
    }



}








