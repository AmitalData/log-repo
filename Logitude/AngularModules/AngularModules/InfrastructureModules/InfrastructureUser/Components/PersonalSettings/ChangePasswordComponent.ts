
import {Component, OnInit } from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ResetPasswordParameters} from './Filters/ResetPasswordParameters';
import {PasswordChangeService} from '../../../../Common/Services/Others/PasswordChangeService';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {Guid} from '../../../../Infrastructure/Utilities/Guid';

import {ChangePasswordParameter} from '../../../../Infrastructure/DataContracts/ChangePasswordParameter';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';



declare var passtring, PassWordValueTriming, isctype, ClientSideBestPassword, gSimilarityMap, gDictionary, DispPwdStrength, ClientSideStrongPassword, DispPwdStrength, ClientSideMediumPassword, DispPwdStrength, ClientSideWeakPassword: any;

@Component({
    moduleId: module.id,
    selector: 'ChangePassword',
    templateUrl: './ChangePasswordComponent.html',
    providers: [PasswordChangeService]

})
export class ChangePasswordComponent implements OnInit {

    IsShowVerifypassError: boolean;
    CurrentPassword: string;
    NewPassword: string;
    RetypePassword: string;
    strongPassword: boolean = false;
    Pdwcheckmsg0DivId: string;
    Pdwcheckmsg1DivId: string;
    Pdwcheckmsg2DivId: string;
    Pdwcheckmsg3DivId: string;
    Pdwcheckmsg4DivId: string;

    idSM1HtmlId: string;
    idSM2HtmlId: string;
    idSM3HtmlId: string;
    idSM4HtmlId: string;

    PdwcheckmsgKey: string;
    SMKey: string;
    constructor(public _passwordChangeService: PasswordChangeService) {

        this.PdwcheckmsgKey = Guid.newGuid();
        this.SMKey = Guid.newGuid();
        this.Pdwcheckmsg0DivId = this.PdwcheckmsgKey + "0";
        this.Pdwcheckmsg1DivId = this.PdwcheckmsgKey + "1";
        this.Pdwcheckmsg2DivId = this.PdwcheckmsgKey + "2";
        this.Pdwcheckmsg3DivId = this.PdwcheckmsgKey + "3";
        this.Pdwcheckmsg4DivId = this.PdwcheckmsgKey + "4";


        this.idSM1HtmlId = this.SMKey + "1";
        this.idSM2HtmlId = this.SMKey + "2";
        this.idSM3HtmlId = this.SMKey + "3";
        this.idSM4HtmlId = this.SMKey + "4";


    }

    ngOnInit(


    ) {



        this.NewPassword = "";

        this.RetypePassword = "";
    }


    SetDataContext(dataContext: any) {

    

    }


    CancelButtonClicked() {


        SessionLocator.CurrentSession.CloseCurrentWindow();

    }
    public ValidationErrorsList: string[];





    SaveButtonClicked() {

        this.IsShowVerifypassError = false;
        this.ValidationErrorsList = [];

 
        this.PasswordValidation ();

        if (this.ValidationErrorsList.length > 0) {
            return;
        }
        
        if (!AppTool.IsNullOrEmpty(this.CurrentPassword)) {
            SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

            var changePasswordParameter: ChangePasswordParameter = new ChangePasswordParameter();
            changePasswordParameter.ContactId = SessionInfo.LoggedUserPM.Id;
            changePasswordParameter.CurrentPassword = this.CurrentPassword;
            changePasswordParameter.Email = SessionInfo.LoggedUserPM.Email;

            this._passwordChangeService.CheckUserPassword(changePasswordParameter).subscribe(res => {

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {

                        if (this.CurrentPassword == this.NewPassword) {
                            SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.NewPasswordCantBeSameAsCurrentOne"));
                        } else this.ChangePassword();

                       

                    } else {

                        this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.CurrentPasswordDoesntMatchYourInput"));

                        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }

                }



            });
        }
        else {

            this.ValidationErrorsList.push("Current Password can't be empty!");
        }

    }

    PasswordValidation() {
        var isvalidPass: boolean = true;
        if (this.NewPassword.length < 8) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordsMinimumLengthIs8Characters"));
            isvalidPass = false;
        }

        if (this.NewPassword.length > 16) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordsMaximumLlengthIs16Characters"));
            isvalidPass = false;
        }

        if (!this.strongPassword && isvalidPass) {

            this.ValidationErrorsList.push("Password is not strong enough. Please use at least three of the four characters types possible.");
        }


        var userName = SessionInfo.LoggedUserPM.EnglishName.split(' ');
        var email = SessionInfo.LoggedUserPM.Email;

        //Password Contains User Name

        var ContainsUserName = false;
        if (userName) {
            userName.forEach((item) => {
                if (!AppTool.IsNullOrEmpty(item)) {
                    if (this.NewPassword.toLowerCase().indexOf(item.toLowerCase()) > -1) {
                        ContainsUserName = true;

                    }
                }

            });
        }

        if (ContainsUserName) {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordMustntContainUserName"));
        }


        //Password Contains User Email
        var ContainsEmail = false;
        if (this.NewPassword.toLowerCase().indexOf(email.toLowerCase()) > -1) {
            ContainsEmail = true;
        }
        else {

            var emalData = [];
            var userEmail = SessionInfo.LoggedUserPM.Email.split('@');
            emalData.push(userEmail[0]);
            emalData.push(userEmail[1].split('.')[0]);
            emalData.push(userEmail[1].split('.')[1]);

            if (emalData) {
                emalData.forEach((item) => {
                    if (!AppTool.IsNullOrEmpty(item)) {
                        if (this.NewPassword.toLowerCase().indexOf(item.toLowerCase()) > -1) {
                            ContainsEmail = true;

                        }
                    }

                });
            }


        }
        if (ContainsEmail) {
            this.ValidationErrorsList.push("Password mustn't contain user email!");

        }

        // contain series(5 letters / numbers)
        var errorList: string[] = this.IsPasswordContainsSeries(this.NewPassword);
        if (errorList.length > 0) {
            errorList.forEach((item) =>{
                this.ValidationErrorsList.push(item);
            });
            
        }

    }

    ChangePassword() {

        if (this.NewPassword) {

            if (this.NewPassword == this.RetypePassword) {
              
                    var changePasswordParameter: ChangePasswordParameter = new ChangePasswordParameter();
                    changePasswordParameter.Email = SessionInfo.LoggedUserPM.Email;
                    changePasswordParameter.NewPassword = this.NewPassword;
                    changePasswordParameter.CurrentPassword = this.CurrentPassword;


                    this._passwordChangeService.ChangeUserPassword(changePasswordParameter).subscribe(res => {

                        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();

                        var pmResponse: ServiceResponse = res;
                        if (!pmResponse.HasError) {
                            var myResult = pmResponse.Result;
                            if (myResult) {
                                this.CancelButtonClicked();
                            }
                            else {
                                this.ValidationErrorsList.push("Changing password failed!");
                            }
                        } else {
                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {

                                var messageWindow: MessageWindow = new MessageWindow();
                                messageWindow.Title = "Logitude Message";
                                messageWindow.Show(pmResponse.ErrorsArray[0]);

                            }

                        }


                    });

                

            }

            else {
                this.IsShowVerifypassError = true;
                SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            }

        }
        else {
            this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordCantBeEmpty"));


            SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();

        }

    }

    onPasswordChanged() {

        var passtringstring = passtring();
        if (passtringstring) {

            var cursorPosition = this.NewPassword.length;
            PassWordValueTriming(passtringstring);

        }

    }



    EvalPwdStrength(sP: string, pdwcheckmsgKey: string, sMKey: string) {

        this.strongPassword = false;
        if (ClientSideBestPassword(sP, gSimilarityMap, gDictionary)) {
            DispPwdStrength(4, 'pwdCheckCase4', pdwcheckmsgKey, sMKey);
            this.strongPassword = true;
        }
        else if (ClientSideStrongPassword(sP, gSimilarityMap, gDictionary)) {
            DispPwdStrength(3, 'pwdCheckCase3', pdwcheckmsgKey, sMKey);
            this.strongPassword = true;
        }
        else if (ClientSideMediumPassword(sP, gSimilarityMap, gDictionary)) {
            DispPwdStrength(2, 'pwdCheckCase2', pdwcheckmsgKey, sMKey);
        }
        else if (ClientSideWeakPassword(sP, gSimilarityMap, gDictionary)) {
            DispPwdStrength(1, 'pwdCheckCase1', pdwcheckmsgKey, sMKey);
        }
        else {
            DispPwdStrength(0, 'pwdCheckCase0', pdwcheckmsgKey, sMKey);
        }
    }


    capLock(e: any) {

        var kc = e.keyCode ? e.keyCode : e.which;

        if (kc == 20) {

        }
        else {
            var sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk)) {

            }

            else {

            }

        }



    }

    

    IsPasswordContainsSeries(password: string) {

        var errorList: string[] = [];

        var result = false;
        if (password) password = password.toUpperCase();

        var passwordNumnberList: any = [];
        for (var i = 0; i < password.length; i++) {
            var char = password.charAt(i);
            var x = 0;
            if ('0123456789'.indexOf(char) !== -1) x = Number(char);
            else x = char.charCodeAt(0);
         
            passwordNumnberList.push(x);
        }

        result = this.IsSeries(passwordNumnberList, "+");
        if (!result) result = this.IsSeries(passwordNumnberList, "-");
        if (result) errorList.push("Password should not contain more than 3 following characters");

        result = this.IsSeries(passwordNumnberList, "Same");
        if (result) errorList.push("Password should not contain more than 3 consecutive repeating characters");

        return errorList;
    }




    IsSeries(passwordNumnberList: any, operatorCode: string) {

        var result = false;
        var seriesNumnberCount: number = 0;
        var seriesNumnberList: any = [];
        passwordNumnberList.forEach((item) => {
            var IsNotSeriesNumnber = false;
            if (item <= 9 || ((item >= 65 && item <= 90))) {
                if (seriesNumnberList.length == 0) seriesNumnberList.push(item);
             
                else {

                    if (operatorCode == "+") {
                        if (seriesNumnberList[seriesNumnberList.length - 1] + 1 == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        } else IsNotSeriesNumnber = true;
                    }

                    else if (operatorCode == "-") {
                        if (seriesNumnberList[seriesNumnberList.length - 1] - 1 == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        } else IsNotSeriesNumnber = true;
                    }

                    else if (operatorCode == "Same") {
                        if (seriesNumnberList[seriesNumnberList.length - 1]  == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        } else IsNotSeriesNumnber = true;
                    }
                }

            } else IsNotSeriesNumnber = true;


            if (seriesNumnberCount == 3) {
                result = true;
                return;
            }

            if (IsNotSeriesNumnber) {
                seriesNumnberCount = 0;
                seriesNumnberList = [];
                seriesNumnberList.push(item);
         
            }

        });

        return result;
    }
}















