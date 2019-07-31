

declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit}  from '@angular/core';
import {PasswordChangeService} from '../../../Common/Services/Others/PasswordChangeService';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
@Component({
    moduleId: module.id,

    selector: 'ResetUserPassword',
    templateUrl: './ResetUserPasswordComponent.html',
    providers: [PasswordChangeService]
})

export class ResetUserPasswordComponent extends BaseComponent implements OnInit {
    public ValidationErrorsList: string[];
    public DataContext: ResetUserPasswordComponent = this;
    public myForm: FormGroup;
    UserId: string;
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(fb: FormBuilder, public _passwordChangeService: PasswordChangeService) {
        super();
        this.myForm = fb.group({});

    }


    ngOnInit() {

        this._entityResourceService.getEntityResourceByTableName("User").subscribe((response: any) => {
        });
    }

    SetDataContext(data: any) {


    }

    ResetClicked() {

        this.ValidationErrorsList = [];

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (!this.UserId) {
            this.ValidationErrorsList.push(msg.replace("%FieldName", "User"));

        }

        else {
            this.ResetPassword();
        }
    }



   
    ResetPassword() {
        var confirmMsg: string = TextCodeTranslator.Translate("User.M.YouWantToResetPassword");
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator.Translate("User.O.ResetUserPassword");
        confirmWindow.Width = 400;
        confirmWindow.Show(confirmMsg);

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.SendResetRequist();
            }
        });
    }
  

    SendResetRequist() {
        this._passwordChangeService.ResetUserPassword(this.UserId,  SessionInfo.LoggedUserTenant).subscribe(res => {



            var pmResponse: ServiceResponse = res;

            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    var messageWindow: MessageWindow = new MessageWindow();
                    messageWindow.Show(TextCodeTranslator.Translate("User.M.UserPasswordResetCompletedSuccessfully") + ": " + result + ".");
                }
            
            }


            this.CurrentSession.CurrentWindow.StopBusyIndicator();



    
        });
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }


}
