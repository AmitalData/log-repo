
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


    CurrentPassword: string;
    NewPassword: string;
    RetypePassword: string;
    strongPassword: boolean = false;










    PasswordLenghtImg: string;
    PasswordContainsCharactersImg: string;
    PasswordContainsNumberImg: string;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public _passwordChangeService: PasswordChangeService) {
    
        this.PasswordLenghtImg = "./_Resources/Images/Icons/ChangePassword/verified.png"
        this.PasswordContainsCharactersImg = "./_Resources/Images/Icons/ChangePassword/verified.png"
        this.PasswordContainsNumberImg = "./_Resources/Images/Icons/ChangePassword/verified.png"
    }

    ngOnInit(


    ) {



        this.NewPassword = "";

        this.RetypePassword = "";
    }


    SetDataContext(dataContext: any) {

    

    }


    CancelButtonClicked() {


        this.CurrentSession.CloseCurrentWindow();

    }
    public ValidationErrorsList: string[];




    Validation() {
        this.ValidationErrorsList = [];

        if (this.NewPassword == this.RetypePassword) {

            if (!this.NewPassword) {
                this.ValidationErrorsList.push("Confirm your password");
                return;
            }

            this.PasswordValidation();

            if (this.ValidationErrorsList.length > 0) {
                return;
            }


            if (!this.IsContainsLowerUpperCase(this.NewPassword)) {
                this.ValidationErrorsList.push("Your password must include an uppercase and lowercase letter.");
                return;
            }

            if (!this.IsContainsNumber(this.NewPassword)) {
                this.ValidationErrorsList.push("Your password must include a number.");

                return;
            }
            if (this.NewPassword.length < 8) {
                this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordsMinimumLengthIs8Characters"));
                return;
            }
        }
        else {

            this.ValidationErrorsList.push("The passwords you entered do not match.");
            return;
        }

        if (this.ValidationErrorsList.length == 0) {

            if (!AppTool.IsNullOrEmpty(this.CurrentPassword)) {
                if (this.NewPassword == this.CurrentPassword) {
                    this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.NewPasswordCantBeSameAsCurrentOne"));
                    return ;
                }
            } else {

                this.ValidationErrorsList.push("Current Password can't be empty!");
                return;
            }

        }

    }
    SaveButtonClicked() {
        this.Validation();

        if (this.ValidationErrorsList.length == 0) {

                this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

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
                                this.CurrentSession.CurrentWindow.StopBusyIndicator();
                                this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.NewPasswordCantBeSameAsCurrentOne"));
                            } else this.ChangePassword();



                        } else {

                            this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.CurrentPasswordDoesntMatchYourInput"));

                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        }

                    }



                });
            
         
        }
    }

    PasswordValidation() {
        var isvalidPass: boolean = true;

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

        var changePasswordParameter: ChangePasswordParameter = new ChangePasswordParameter();
        changePasswordParameter.Email = SessionInfo.LoggedUserPM.Email;
        changePasswordParameter.NewPassword = this.NewPassword;
        changePasswordParameter.CurrentPassword = this.CurrentPassword;


        this._passwordChangeService.ChangeUserPassword(changePasswordParameter).subscribe(res => {

            this.CurrentSession.CurrentWindow.StopBusyIndicator();

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

    onPasswordChanged() {

        var passtringstring = passtring();
        if (passtringstring) {

            var cursorPosition = this.NewPassword.length;
            PassWordValueTriming(passtringstring);

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


    IsContainsLowerUpperCase(str) {
        return str.match(/[a-z]/) && str.match(/[A-Z]/);
    }

    IsContainsNumber(str) {
        var regex = /\d/g;
        return regex.test(str);
    }



    Passwordkeyup(passtring) {

    this.ValidationErrorsList = [];

        document.getElementById("PasswordLenghtDiv").style.color = "gray";
        document.getElementById("PasswordContainsCharactersDiv").style.color = "gray";
        document.getElementById("PasswordContainsNumberDiv").style.color = "gray";

        this.PasswordLenghtImg = "./_Resources/Images/Icons/ChangePassword/verified.png"
        this.PasswordContainsCharactersImg = "./_Resources/Images/Icons/ChangePassword/verified.png"
        this.PasswordContainsNumberImg = "./_Resources/Images/Icons/ChangePassword/verified.png"

        if (passtring) {

            if (passtring.length >= 8) {
                document.getElementById("PasswordLenghtDiv").style.color = "green";
                this.PasswordLenghtImg = "./_Resources/Images/Icons/ChangePassword/verifiedGreen.png"

            }

            if (this.IsContainsLowerUpperCase(passtring)) {
                document.getElementById("PasswordContainsCharactersDiv").style.color = "green";
                this.PasswordContainsCharactersImg = "./_Resources/Images/Icons/ChangePassword/verifiedGreen.png"
            }

            if (this.IsContainsNumber(passtring)) {
                document.getElementById("PasswordContainsNumberDiv").style.color = "green";
                this.PasswordContainsNumberImg = "./_Resources/Images/Icons/ChangePassword/verifiedGreen.png"
            }


           this.PasswordValidation();



        }
    }
}















