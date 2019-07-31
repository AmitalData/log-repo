import { Component, Output, EventEmitter} from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { AppTool, FormatTool } from '../../../Infrastructure/Tools';
import { AccountingOpService } from '../../Services/Others/AccountingOpService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';

@Component({
    moduleId: module.id,
    templateUrl: './Generate1000Component.html',
})

export class Generate1000Component extends BaseComponent {
    public DataContext: Generate1000Component = this;
    public ObjectTableName: string = "GLAccount";
    Email: string
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    _AccountingOpService: AccountingOpService;
    constructor() {
        super();
        this.Email = SessionLocator.LoggedUserPM.Email;
        this.UIProperties.SetRequired("Email", this.ObjectTableName, true);
        this.CurrentSession.StopBusyIndicator();
        this._AccountingOpService = new AccountingOpService();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    OnKeyUp(key) {
        if (!AppTool.IsNullOrEmpty(key)) {
            if (key.keyCode == '13') {
                this.OkButtonClicked();
            }
        }
    }
    FillErrors() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.Email)) {
            //this.Year = new Date().getFullYear();
            this.ValidationErrorsList.push("Email is Required");
        }
        else if (!FormatTool.IsEmail(this.Email)) {
            this.ValidationErrorsList.push("Email is not valid");
        } else {
            this.ValidationErrorsList = [];

        }
    }
    OkButtonClicked() {
        this.FillErrors();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        this.CurrentSession.StartBusyIndicatorCreating();
        this._AccountingOpService
            .Generate1000(this.Email)
            .subscribe(
                (res: ServiceResponse) => {
                    if (res.HasError) {
                        this.ValidationErrorsList = res.ErrorsArray;

                    } else {
                        if (res.Result) {
                            var mw = new MessageWindow();
                            mw.Show(res.Result.Message);
                        }
                    }
                },
                (err) => {
                    alert(err);
                },
                () => {
                    this.CurrentSession.StopBusyIndicator();
                }
            );
    }
}
