import {Component, OnInit} from 'angular2/core';
import {Router, RouteConfig, ROUTER_DIRECTIVES} from 'angular2/router';
import {Http, HTTP_PROVIDERS, Headers} from 'angular2/http';
import 'rxjs/add/operator/map';

@Component({
  //  template: `
  //  <h1 class="title">Login</h1>
  //  <a [routerLink]="['EditControl']">Login</a>
  //`,
    templateUrl: './app/infrastructure/login/login.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [HTTP_PROVIDERS]
})

export class LoginComponent implements OnInit {

    //public ObjectFields: ObjectField[];
    public Email: string;
    public Password: string;
    public LoginParams: any; //LogitudeFrontEnd.Classes.LoginParameters;
    public HideLoginForm: boolean;
    public HideTenantForm: boolean;
    public Tenant: number;
    public TenantList: any[]; //LogitudeFrontEnd.Classes.CompanyLogin[];
    public ShowLoginBusyIndicator: boolean;
    public LoginTokenParams: any; //LogitudeFrontEnd.Classes.LoginTokenParameter;
    public LoginFailed: boolean;
    public HidePendingLoading: boolean;
    public UserData: any;

    constructor(private _http: Http, private _router: Router) {
        //this.ObjectFields = [];
        this.ShowLoginBusyIndicator = false;
        this.HideLoginForm = false;
        this.HideTenantForm = true;
        this.LoginFailed = false;
        this.LoginParams = {};
        this.HidePendingLoading = true;
        
        window.Statuses = [];
        window.Ports = [];
        window.TransportModes = [];
        window.Directions = [];
        window.Cards = [];
        window.MenusTables = [];
        window.TextCodesTranslations = [];
        window.ObjectTables = [];
        window.Screens = [];
        window.ScreenFields = [];
        window.ObjectTableTabs = [];
        window.ObjectFields = [];

    }

    ngOnInit() {
        //this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&objectTableName=Shipment&inActive=false')
        //    .subscribe(objectFields => this.putInWindow(objectFields));
    }

    //putInWindow(objectfields) {
    //    this.ObjectFields = objectfields.json();
    //    window.ObjectFields = this.ObjectFields;
    //}

    Login() {
        this.HideLoginForm = true;
        this.HideTenantForm = true;
        this.HidePendingLoading = true;
        this.ShowLoginBusyIndicator = true;
        //this._router.navigate(['EditControl']);
        this.LoadClosedTablesToWindow();
    }
    Loginf(f, values) {
        //console.log(f);
        //console.log(this.Email, this.Password);
        var isValid = f.valid;
        //console.log(isValid);
        if (isValid) {
            this.LoginParams = {
                Email: this.Email,
                Password: this.Password,
                ByToken: null,
                CardId: null,
                CardType: null,
                IsMobileLogin: false,
                IsUser: true,
                GetToken: true
            };
            this.HidePendingLoading = false;
            //this.PostUserValidation(this.LoginParams);
        }
    }
    ChooseTenant(f, values) {
        console.log(f);
    }

    LoadClosedTablesToWindow() {
        var authHeader = new Headers();
        authHeader.append('Content-Type', 'application/json');
        authHeader.append('Accept', 'text/html,application/json,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8');
         
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&inActive=false&dumb2=dumb', authHeader)
            .map(res => { /*console.log(res);*/ res.json() })
            .subscribe(statuses => { /*console.log(statuses);*/ window.Statuses = statuses });
        //this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&inActive=false&inland=true&air=true&ocean=true')
        //    .subscribe(ports => window.Ports = ports.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&dummy=dummy')
            .subscribe(transportmodes => window.TransportModes = transportmodes.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&dummy2=dummy2')
            .subscribe(directions => window.Directions = directions.json());
        //this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&inActive=false&dumb=dumb')
        //    .subscribe(cards => window.Cards = cards.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&menustables=dummy')
            .subscribe(menustables => window.MenusTables = menustables.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&textcodetranslations=dummy')
            .subscribe(textcodestranslations => window.TextCodesTranslations = textcodestranslations.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&objecttables=dummy')
            .subscribe(objecttables => window.ObjectTables = objecttables.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&screens=dummy')
            .subscribe(screens => window.Screens = screens.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&screenfields=dummy')
            .subscribe(screenfields => window.ScreenFields = screenfields.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&objecttabletabs=dummy')
            .subscribe(objecttabletabs => window.ObjectTableTabs = objecttabletabs.json());
        this._http.get('http://localhost:9996/api/ngMetaData?tenant=1&objectTableName=Shipment&inActive=false')
            .subscribe(objectFields => this.MapLastTableAndNavigate(objectFields));
    }

    MapLastTableAndNavigate(objectfields) {
        window.ObjectFields = objectfields.json();
        //this._router.navigate(['EditControl']);
        this._router.navigate(['Root', 'MainMenu']);
    }

}
