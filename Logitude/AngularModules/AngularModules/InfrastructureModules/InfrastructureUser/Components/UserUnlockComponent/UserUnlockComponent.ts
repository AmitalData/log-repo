import {Component} from '@angular/core';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {PasswordChangeService} from '../../../../Common/Services/Others/PasswordChangeService';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';

@Component({
    moduleId: module.id,
    templateUrl: './UserUnlockComponent.html',
    providers: [PasswordChangeService],

})

export class UserUnlockComponent{

    public ValidationErrorsList: string[];
    CurrentPassword: string;
    IsValidPassword: boolean = false;
    IsShowProgressLoading: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _passwordChangeService: PasswordChangeService) {

    }

    ngOnInit(


    ) {

    }


    public SetWindowArgs(args: any) {
        //this.Tenant = args.EnttiyPM.Tenant;
        //this.entityPM = args.EnttiyPM;

        //this.ValidationErrorsList = args.ErrorsList;
        //this.ValidationWarningsList = args.WarningsList;

        //this.InitializeComponent();
    }


    OnSignoutClicked() {
        SessionLocator.HomeComponent.SignoutClicked();
    }

    UnlockClicked() {
        
        this.IsShowProgressLoading = false;
        if (this.CurrentPassword) {
            this.IsShowProgressLoading = true;
             var computerId: string = SessionLocator.GetComputerIdFromStorage();
             this._passwordChangeService.GetSetUserLastLogin(this.CurrentPassword, SessionInfo.LoggedUserPM.Id, SessionInfo.LoggedUserTenant, computerId).subscribe(res => {
                 this.IsShowProgressLoading = false;
                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.CurrentSession.CloseCurrentWindow();
                      
                    } else {

                        this.IsValidPassword = true;
                    }

                }



            });


        }

        else {
            this.IsValidPassword = true;

        }
    }



    onPasswordChanged(event) {

        if (event) {

            this.IsValidPassword = false;
        }

    }
}
