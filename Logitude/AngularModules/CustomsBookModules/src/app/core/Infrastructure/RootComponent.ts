import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { DynamicLoader } from '../App/DynamicLoader/DynamicLoader';
import {ServiceHelper} from './Utilities/ServiceHelper';
import {SessionLocator} from './Utilities/SessionLocator';
import {ExternalParams, ExternalParamsArg} from './Utilities/ExternalParams';
import {TermsofUseService} from './Services/WebServices/TermsofUseService';
import {SessionInfo} from './Utilities/SessionInfo'
import {ServiceResponse} from './DataContracts/ServiceResponse';
import {TermsofUseArgs} from './DataContracts/TermsofUseArgs';
import { environment } from '../environments/environment';
import { LoginService } from './Services/LoginService';
import { AppTool } from './Tools'
import { ChildDirective } from './Directives/ChildDirective';
declare var IsMobileDetected;

@Component({
  selector: 'RootComponent',
  template:
    `
        <div class="MediaFillRelative">
            <div ChildDirective></div>
        </div>
    `,
})

export class RootComponent implements AfterViewInit {

    @ViewChild(ChildDirective) Child: ChildDirective;

    constructor() {
    var data = window.sessionStorage.getItem('userdata');

    if (data != "SignOut") {
      this.BuildExternalParams();
    }
  }

  Boot(args: any) {
    ServiceHelper.HttpClient = args["Http"];
    SessionLocator.DynamicLoader = DynamicLoader;
    SessionLocator.RootComponent = this;

    if (environment.production) {
      SessionLocator.IsProduction = true;
    }
  }

  ngAfterViewInit() {
    this.RunComponent();
  }

    isDSV: boolean = false;
    isPrivateLable: boolean = false;
  RunComponent() {
      const url = window.location.href;
      //if (url.indexOf("staging") > -1)
      //    SessionLocator.WorkerRoleName = "staging";

      if (url.indexOf("localhost") > -1)
          SessionLocator.WorkerRoleName = "development";
      this.isPrivateLable = window.sessionStorage.getItem("IsPrivateLabel") == "true";
      this.isDSV = window.sessionStorage.getItem("IsDSV") == "true";
      this.LoginToSystem();
  }

    LoginToSystem() {
        const url = window.location.href;
        var data = window.sessionStorage.getItem('userdata');
        if (url.indexOf('localhost:4200/?D{%22$id') > -1 || url.indexOf('localhost:4200/?P{%22$id') > -1) {
            this.isPrivateLable = true;
            this.isDSV = url.indexOf('localhost:4200/?D{%22$id') > -1;
            data = url.split('?' + this.isDSV ? 'D' : 'P')[1];
        }
        if ((data && data == "SignOut") || (!data && !SessionLocator.IsExternalParams && url.indexOf('localhost') == -1)) {
            document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
        }

        else {
            this.LoadLoginPage();
            var IsPREQ = SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && (SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "preq" || SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "uid");
            if (url && IsPREQ == false && url.indexOf('localhost') == -1) {
                window.onbeforeunload = function (e) {
                    var message = "";
                    if (SessionLocator.ExternalParams && SessionLocator.ExternalParams.OneTimePasswordId) {
                        message = "when you leave this site can't not be used the key agin";
                    }
                    else if (!SessionLocator.IsSiguOut) {
                        message = "Are you sure you want to leave this page ?";
                    }

                    if (!AppTool.IsNullOrEmpty(message)) {
                        e.returnValue = message;
                        return message;
                    }
                };
            }

        }
    }

  private ClearLocation() {
    if (this.Child.Location) {
      this.Child.Location.clear();
    }
  }

  BuildExternalParams() {
    //var url = 'login.aspx?Menu=Tickets&Action=Query&Parmters=[{"CompanyId":"1-720","ShipmentId":"1-26027", "ShipmentNumber" : "bdf22789-46e3-4"}]';
    var url = window.location.href;
    if (url) {
      var keys = url.split('?');
      if (keys[1]) {
        var vars = keys[1].split('&');
        if (vars) {
          SessionLocator.ExternalParams = new ExternalParams();
          SessionLocator.IsExternalParams = true;
          for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split('=');
            SessionLocator.ExternalParams[decodeURIComponent(pair[0])] = pair[1];
            if (decodeURIComponent(pair[0]) == "Parmters") {
              var str1 = pair[1];
              var str = decodeURIComponent(str1);
              var jsonPMKeys = JSON.parse(str);
              for (var key in jsonPMKeys) {
                var arg = new ExternalParamsArg();
                arg.FieldName = key;
                arg.FieldValue = jsonPMKeys[key];
                SessionLocator.ExternalParams.Args.push(arg);
              }
            }
          }
        }
      }
    }
  }
  _FinishLogin: boolean = false;
    LoadLoginPage() {
        this.ClearLocation();

        if (this.isPrivateLable == true) {
            this.LoadPrivateLablePages();
        }
        else {
            SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/LoginComponent", this.Child.Location)
                .then(cmpRef => {

                    cmpRef.instance.Blocking.subscribe(s => {
                        //SessionLocator.BlockType = s;
                        //this.LoadBlockingScreen();
                    });

                    cmpRef.instance.LoginCompleted.subscribe(s => {
                        this.OnLoginCompleted(s);
                    });
                });
        }
    }
    LoadPrivateLablePages() {
        if (SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
            this.LoadPrivateLableMobileLoginProcess();
        }
        else {
            this.LoadPrivateLableLoginProcess();
        }
    }

    private LoadPrivateLableLoginProcess() {
        SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/PrivateLabelComponents/PrivateLabelLoginProcessComponent", this.Child.Location)
            .then(cmpRef => {

                cmpRef.instance.Blocking.subscribe(s => {
                    SessionLocator.BlockType = s;
                    this.LoadBlockingScreen();
                });

                cmpRef.instance.LoginCompleted.subscribe(s => {
                    this.OnLoginCompleted();
                });
            });
    }

    LoadPrivateLableMobileLoginProcess() {
        SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/PrivateLabelComponents/DSVMobileLoginProcessComponent", this.Child.Location)
            .then(cmpRef => {

                cmpRef.instance.Blocking.subscribe(s => {
                    SessionLocator.BlockType = s;
                    this.LoadBlockingScreen();
                });

                cmpRef.instance.LoginCompleted.subscribe(s => {
                    this.OnLoginCompleted();
                });
            });
    }

  LoadBlockingScreen() {
    this.ClearLocation();

    SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/BlockScreenComponent", this.Child.Location)
      .then(cmpRefBlocked => {
        cmpRefBlocked.instance.BackToLoginCompleted.subscribe(r => {
          this.SignOutCompleted();
        });
      });
  }
  ViewHomeComponent() {

    this.ClearLocation();

    //SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelApprovebyMobileComponent", this.location)
    SessionLocator.DynamicLoader.Load("./Infrastructure/Components/HomeComponent/HomeComponent", this.Child.Location)
      .then(cmpRef => {
        cmpRef.instance.RunComponent();

        cmpRef.instance.SignoutCompleted.subscribe(s => {
          if (s == "Block") {
            this.LoadBlockingScreen();

          }

          else {
            this.SignOutCompleted();
          }
        });
      });
  }

  ViewDeclarationApprovalComponent() {

    this.ClearLocation();

    SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelApprovebyMobileComponent", this.Child.Location)
      .then(cmpRef => {
        cmpRef.instance.RunComponent();

        //cmpRef.instance.SignoutCompleted.subscribe(s => {
        //    if (s == "Block") {
        //        this.LoadBlockingScreen();

        //    }

        //    else {
        //        this.SignOutCompleted();
        //    }
        //});
      });
  }
  ViewEComercePaymentRequestComponent() {

    this.ClearLocation();

    SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/ECommercePaymentRequestMobileComponent", this.Child.Location)
      .then(cmpRef => {
        cmpRef.instance.RunComponent();

        //cmpRef.instance.SignoutCompleted.subscribe(s => {
        //    if (s == "Block") {
        //        this.LoadBlockingScreen();

        //    }

        //    else {
        //        this.SignOutCompleted();
        //    }
        //});
      });
  }

  VieUserIdNumberMobileComponent() {
    this.ClearLocation();
    SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/UserIdNumberMobileComponent", this.Child.Location)
      .then(cmpRef => {
        cmpRef.instance.RunComponent();
      });
  }

  OnLoginCompleted(Param: any = null) {

    if (Param == "IgnoreTerms") {
      if (SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "uid") {
        this.VieUserIdNumberMobileComponent();
      }
      else {
        this.ViewEComercePaymentRequestComponent();
      }
      return;
    }
    this._FinishLogin = true;
    var termsofUseService = new TermsofUseService();
    termsofUseService.GetCheckIfGoToTermUseComponent(SessionLocator.Tenant, SessionLocator.LoggedUserId).subscribe((res: ServiceResponse) => {
        var serviceResponse: ServiceResponse = res;
        this.checkTermsOfUseResult(serviceResponse); 
    });
    }

    checkTermsOfUseResult(serviceResponse: ServiceResponse) {

        var termsofUseArgs: TermsofUseArgs = serviceResponse.Result;
        if (!serviceResponse.HasError) {

            if (termsofUseArgs) {
                if (termsofUseArgs.IsTermOfUse) {
                    this.LoadTermsOfUse(serviceResponse);
                }

                else {
                    this.checkSessionLocator();
                }
            }
        }
        else {
            this.LoadTermsOfUse(serviceResponse);
        }
    }
    private LoadTermsOfUse(serviceResponse: ServiceResponse) {
        this.ClearLocation();
        if (this.isPrivateLable == true) {
            this.LoadPrivateLableTermsOfUseComponent(serviceResponse);
        }
        else
            this.LoadTermsOfUsesStartup(serviceResponse);
    }

    private LoadTermsOfUsesStartup(serviceResponse: ServiceResponse) {
        {
            if (!serviceResponse.HasError) {
                var termsofUseArgs: TermsofUseArgs = serviceResponse.Result;
            } else { 
                this.ClearLocation();
            } 
            SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/TermsOfUseStartupComponent", this.Child.Location)
                .then(cmpRef => {
                    cmpRef.instance.ComponentRef = cmpRef;
                    if (!serviceResponse.HasError) {
                        cmpRef.instance.Load(termsofUseArgs.PrivateLabelId, termsofUseArgs.Id);
                    } else {
                        if (serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) { 
                            cmpRef.instance.LoadErrorMessage(serviceResponse.ErrorsArray[0]);
                        } 
                    }
                    cmpRef.instance.TermsOfUseCompleted.subscribe(($event: any) => {

                        if ($event == "Accept") {
                            this.ViewHomeComponent();
                        }

                        else if ($event == "Decline") {
                            this.SignOutCompleted();
                        }
                    });
                });
        }
    }

    private checkSessionLocator() {
        if (SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
            this.ViewDeclarationApprovalComponent();
        }
        else if (SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "preq" && IsMobileDetected() == true) {
            this.ViewEComercePaymentRequestComponent();
        }
        else if (SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "uid" && IsMobileDetected() == true) {
            this.VieUserIdNumberMobileComponent();
        }
        else {
            this.ViewHomeComponent();
        }
    }
     
    LoadPrivateLableTermsOfUseComponent(serviceResponse: ServiceResponse) {
        if (!serviceResponse.HasError) { 
            var termsofUseArgs: TermsofUseArgs = serviceResponse.Result;
        } else {
            this.ClearLocation();
        }
         
        SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/CustomTermsOfUse/DSVTermsOfUseStartupComponent", this.Child.Location)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                if (!serviceResponse.HasError) {
                    cmpRef.instance.Load(termsofUseArgs.PrivateLabelId, termsofUseArgs.Id);
                } else {
                    if (serviceResponse.ErrorsArray && serviceResponse.ErrorsArray.length > 0) {
                        cmpRef.instance.LoadErrorMessage(serviceResponse.ErrorsArray[0]);
                    }
                }
                cmpRef.instance.TermsOfUseCompleted.subscribe(($event: any) => {

                    if ($event == "Accept") {
                        this.ViewHomeComponent();
                    }
                    else if ($event == "Decline") {
                        this.SignOutCompleted();
                    }
                });
            });
    }

  SignOutCompleted() {

    var loginService = new LoginService();
    loginService.GetSignOut().subscribe((res: any) => {

    });

    window.sessionStorage.setItem("userdata", "SignOut");
    SessionLocator.ClearLocalStorage();
    SessionLocator.IsSiguOut = true;
    SessionInfo.LoggedUserEmail = "";
    SessionInfo.LoggedUserId = "";
    SessionInfo.Token = "";

    this.ClearLocation();
    this.BackToLoginPage();




    //SignOutAut



    //window.sessionStorage.setItem("Token", "");

    }

    private BackToLoginPage() {
        if (window.sessionStorage.getItem("IsSharedLogistics") == "true" && !AppTool.IsNullOrEmpty(SessionInfo.LoggedUserTenant)) {
            document.location.href = ServiceHelper.GetLogitudeURL() + "?tenant=" + SessionInfo.LoggedUserTenant;
        }
        else {
            document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
        }
    }
}
