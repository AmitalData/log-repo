import {Component, OnInit, ViewChild, ViewContainerRef} from '@angular/core';
import {DynamicLoader_Cust} from './Utilities/DynamicLoader_Cust';
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
declare var IsMobileDetected;
@Component({
    selector: 'RootComponent',
    template:
    `
        <div class="MediaFillRelative">
            <div #Child></div>
        </div>
    `,
})

export class RootComponent_Cust implements OnInit {
  private isComponentBooted: boolean = false;
  private isComponentInited: boolean = false;
  @ViewChild("Child", { read: ViewContainerRef }) location: ViewContainerRef;
  constructor() {

    var data = window.sessionStorage.getItem('userdata');

    if (data != "SignOut") {
      this.BuildExternalParams();
    }
  }

  Boot(args: any) {
    ServiceHelper.Http = args["Http"];
    SessionLocator.Http = args["Http"];
    DynamicLoader_Cust.Compiler = args["Compiler"];
    DynamicLoader_Cust.Resolver = args["Resolver"];
    DynamicLoader_Cust.Injector = args["Injector"];
    DynamicLoader_Cust.ModuleLoader = args["ModuleLoader"];
    SessionLocator.DynamicLoader = DynamicLoader_Cust;
    SessionLocator.RootComponent = this;

    if (environment.production) {
      SessionLocator.IsProduction = true;
    }

    this.isComponentBooted = true;
    this.RunComponent();
  }

  ngOnInit() {
    this.isComponentInited = true;
    this.RunComponent();
  }

  isDSV: boolean = false;
  RunComponent() {
    if (this.isComponentBooted && this.isComponentInited) {
      var url = window.location.href;

      this.isDSV = url.toLowerCase().indexOf(".dsv.") > -1 ? true : false;

      var data = window.sessionStorage.getItem('userdata');
      if ((data && data == "SignOut") || (!data && !SessionLocator.IsExternalParams && url.indexOf('localhost') == -1)) {
        document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";
      }

      else {
        this.LoadLoginPage();
        if (url && url.indexOf('localhost') == -1) {
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
  }

  private ClearLocation() {
    if (this.location) {
      this.location.clear();
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
    //this.isDSV = true;
    if (this.isDSV == true) {
      if (SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
        SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/CustomLoginComponents/DSVMobileLoginProcessComponent", this.location)
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
      else {
        SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/CustomLoginComponents/DSVLoginProcessComponent", this.location)
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
    }
    else {
      SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/LoginComponent", this.location)
        .then(cmpRef => {

          cmpRef.instance.Blocking.subscribe(s => {
            SessionLocator.BlockType = s;
            this.LoadBlockingScreen();
          });

          cmpRef.instance.LoginCompleted.subscribe(s => {
            this.OnLoginCompleted(s);
          });
        });
    }
  }
  LoadBlockingScreen() {
    this.ClearLocation();

    SessionLocator.DynamicLoader.Load("./Infrastructure/Components/LoginComponent/BlockScreenComponent", this.location)
      .then(cmpRefBlocked => {
        cmpRefBlocked.instance.BackToLoginCompleted.subscribe(r => {
          this.SignOutCompleted();
        });
      });
  }
  ViewHomeComponent() {

    this.ClearLocation();

    //SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelApprovebyMobileComponent", this.location)
    SessionLocator.DynamicLoader.Load("./Infrastructure/Components/HomeComponent/HomeComponent", this.location)
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

    SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/PrivateLabelApprovebyMobileComponent", this.location)
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

    SessionLocator.DynamicLoader.Load("./ShipmentModules/ShipmentLogBox/Components/Logbox/ECommercePaymentRequestMobileComponent", this.location)
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

  OnLoginCompleted(Param: any = null) {

    if (Param == "IgnoreTerms") {
      this.ViewEComercePaymentRequestComponent();
      return;
    }
    this._FinishLogin = true;
    var termsofUseService = new TermsofUseService();
    termsofUseService.GetCheckIfGoToTermUseComponent(SessionLocator.Tenant, SessionLocator.LoggedUserId).subscribe(res => {
      var pmResponse: ServiceResponse = res;
      if (!pmResponse.HasError) {
        var myResult: TermsofUseArgs = pmResponse.Result;
        if (myResult) {
          if (myResult.IsTermOfUse) {
            this.ClearLocation();
            if (this.isDSV == true) {
              SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/CustomTermsOfUse/DSVTermsOfUseStartupComponent", this.location)
                .then(cmpRef => {
                  cmpRef.instance.ComponentRef = cmpRef;
                  cmpRef.instance.Load(myResult.Version);
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
            else {
              SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/TermsOfUseStartupComponent", this.location)
                .then(cmpRef => {
                  cmpRef.instance.ComponentRef = cmpRef;
                  cmpRef.instance.Load(myResult.Version);
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

          else {
            if (SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "dapp" && IsMobileDetected() == true) {
              this.ViewDeclarationApprovalComponent();
            }
            else if (SessionLocator.IsExternalParams && SessionLocator.ExternalParams && SessionLocator.ExternalParams.Menu && SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "preq" && IsMobileDetected() == true) {
              this.ViewEComercePaymentRequestComponent();
            }
            else {
              this.ViewHomeComponent();
            }
          }
        }
      }
    });
  }
  SignOutCompleted() {

    var loginService = new LoginService();
    loginService.GetSignOut().subscribe(res => {

    });

    window.sessionStorage.setItem("userdata", "SignOut");
    SessionLocator.ClearLocalStorage();
    SessionLocator.IsSiguOut = true;
    SessionInfo.LoggedUserEmail = "";
    SessionInfo.LoggedUserId = "";
    SessionInfo.Token = "";

    this.ClearLocation();
    document.location.href = ServiceHelper.GetLogitudeURL() + "Login.aspx";




    //SignOutAut



    //window.sessionStorage.setItem("Token", "");

  }
}
