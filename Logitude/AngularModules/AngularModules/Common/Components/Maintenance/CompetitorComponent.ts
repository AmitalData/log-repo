import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPMService} from '../../Services/StandardPMs/TenantPMService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {CompetitorPM} from '../../EntityPMs/CompetitorPM';
import {AppTool} from '../../../Infrastructure/Tools';
import {CountryList} from '../../EntityLists/CountryList';
import {CountryListService} from '../../Services/StandardLists/CountryListService';
import {StateList} from '../../EntityLists/StateList';
import {StateListService} from '../../Services/StandardLists/StateListService';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {CompetitorPMService} from '../../Services/StandardPMs/CompetitorPMService';
import {CitySelectionArgs} from '../../Args';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
@Component({
    selector: 'CompetitorComponent',
    moduleId: module.id,
    templateUrl: './CompetitorComponent.html',
})

export class CompetitorComponent extends BaseComponent {
    public EntityPM: CompetitorPM;
    public DataContext = this;
    public ObjectTableName: string = "Competitor";
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService,private entityArgs:EntityArgs) {
        super();
        if (entityArgs.EntityPM != null)
            this.EntityPM = entityArgs.EntityPM;
        else {
            this.EntityPM = new CompetitorPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;
        }

        this.UIProperties.SetRequired("City", this.ObjectTableName, true);
        this.UIProperties.SetRequired("CountryId", this.ObjectTableName, true);
        this.SetUIProperties(null);
    }

    SetWindowArgs(args:any) {


    }


    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) {
        if (this.EntityPM.Name != value)
            this.EntityPM.Name = value;
    }

    public get Address1() { return this.EntityPM.Address1; }
    public set Address1(value: string) {
        if (this.EntityPM.Address1 != value)
            this.EntityPM.Address1 = value;
    }


    public get Address2() { return this.EntityPM.Address2; }
    public set Address2(value: string) {
        if (this.EntityPM.Address2 != value)
            this.EntityPM.Address2 = value;
    }



    public get ZipCode() { return this.EntityPM.ZipCode; }
    public set ZipCode(value: string) {
        if (this.EntityPM.ZipCode != value)
            this.EntityPM.ZipCode = value;
    }

    SelectCityCommand() {
        var args = new CitySelectionArgs(this.CountryId);
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Select City";
        logWindow.WindowArgs = args;
        logWindow.Show("./CommonModules/CommonOthers/Components/CitySelection/CitySelectionComponent");
        logWindow.WindowClosed.subscribe(($event: any) => {
            if (args.IsCitySelected) {
                var mySelectedCity: string = args.CityName;               
                this.City = mySelectedCity;
                this.CountryId = args.CountryId;
                this.StateId = args.StateId;
            }
        });
    }




    public get City() { return this.EntityPM.City; }
    public set City(value: string) {
        if (this.EntityPM.City != value) {
            this.EntityPM.City = value;
            if (AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("City", this.ObjectTableName,true);
            }
            else {
                this.UIProperties.SetRequired("City", this.ObjectTableName, false);
            }
            
        }
    }



    public get CountryId() { return this.EntityPM.CountryId; }
    public set CountryId(value: string) {
        if (this.EntityPM.CountryId != value) {
            this.EntityPM.CountryId = value;

            var StateId = null;
           var  CountryCode = null;
           var CountryName = null;
           var countryListService: CountryListService = new CountryListService();
           countryListService.getAllFromCache().subscribe(result => {
               var list: CountryList = result.Result.filter(d => d.Tenant == SessionLocator.Tenant && d.Id == value)[0];
               if (list != null) {
                   CountryCode = list.Code;
                   CountryName = list.EnglishName;
               }

               if (AppTool.IsNullOrEmpty(value)) {
                   this.UIProperties.SetRequired("CountryId", this.ObjectTableName, true);
               }
               else {
                   this.UIProperties.SetRequired("CountryId", this.ObjectTableName, false);
               }

               this.SetUIProperties(list);
           });
          

          

        }
    }


    SetUIProperties(list: CountryList) {
        if (list != null) {
            this.UIProperties.SetEnabled("StateId", this.ObjectTableName, list.HasStates);
            this.UIProperties.SetRequired("StateId", this.ObjectTableName, list.IsStateRequired);
            if (!list.HasStates) {
                this.StateId = null;
                this.StateName = null;
            }
        }
        else {

            this.UIProperties.SetEnabled("StateId", this.ObjectTableName, false);
            this.UIProperties.SetRequired("StateId", this.ObjectTableName, false);

        }
    }



    public get StateId() { return this.EntityPM.StateId; }
    public set StateId(value: string) {
        if (this.EntityPM.StateId != value) {
            this.EntityPM.StateId = value;
            var stateListService: StateListService = new StateListService();
            stateListService.getAllFromCache().subscribe(result => {
                var list: StateList = result.Result.filter(d => d.Tenant == SessionLocator.Tenant && d.Id == value)[0];
                if (list != null) {
                    this.StateName = list.EnglishName;
                }
            });
            if (!AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("StateId", this.ObjectTableName, false);
            }
            else if (AppTool.IsNullOrEmpty(value))
                this.UIProperties.SetRequired("StateId", this.ObjectTableName, true);

            if (AppTool.IsNullOrEmpty(this.CountryId))
                this.SetUIProperties(null);
        }        
    }


    public get PhoneNumber() { return this.EntityPM.PhoneNumber; }
    public set PhoneNumber(value: string) {
        if (this.EntityPM.PhoneNumber != value)
            this.EntityPM.PhoneNumber = value;
    }


    public get FaxNumber() { return this.EntityPM.FaxNumber; }
    public set FaxNumber(value: string) {
        if (this.EntityPM.FaxNumber != value)
            this.EntityPM.FaxNumber = value;
    }


    public get Website() { return this.EntityPM.Website; }
    public set Website(value: string) {
        if (this.EntityPM.Website != value)
            this.EntityPM.Website = value;
    }


    public get Strengths() { return this.EntityPM.Strengths; }
    public set Strengths(value: string) {
        if (this.EntityPM.Strengths != value)
            this.EntityPM.Strengths = value;
    }



    public get Weaknesses() { return this.EntityPM.Weaknesses; }
    public set Weaknesses(value: string) {
        if (this.EntityPM.Weaknesses != value)
            this.EntityPM.Weaknesses = value;
    }



    public get Opportunity() { return this.EntityPM.Opportunity; }
    public set Opportunity(value: string) {
        if (this.EntityPM.Opportunity != value)
            this.EntityPM.Opportunity = value;
    }


    public get Threat() { return this.EntityPM.Threat; }
    public set Threat(value: string) {
        if (this.EntityPM.Threat != value)
            this.EntityPM.Threat = value;
    }


    public get StateName() { return this.EntityPM.StateName; }
    public set StateName(value: string) {
        if (this.EntityPM.StateName != value)
            this.EntityPM.StateName = value;
    }


    public get CountryCode() { return this.EntityPM.CountryCode; }
    public set CountryCode(value: string) {
        if (this.EntityPM.CountryCode != value)
            this.EntityPM.CountryCode = value;
    }



    public get CountryName() { return this.EntityPM.CountryName; }
    public set CountryName(value: string) {
        if (this.EntityPM.CountryName != value)
            this.EntityPM.CountryName = value;
    }







    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    Validate() {
        if (AppTool.IsNullOrEmpty(this.Name))
            this.ValidationErrorsList.push("Name field is required");
        if (AppTool.IsNullOrEmpty(this.City))
            this.ValidationErrorsList.push("City field is required");
        if (AppTool.IsNullOrEmpty(this.CountryId))
            this.ValidationErrorsList.push("Country field is required");
        if (this.UIProperties.GetUIProperty("StateId", this.ObjectTableName, this.DataContext,true).IsRequired && AppTool.IsNullOrEmpty(this.StateId)) {
            this.ValidationErrorsList.push("State field is required");
            
        }

    }
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        this.Validate();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorCreating();
            var myService = new CompetitorPMService();
            myService.insert(this.EntityPM).subscribe(myResult => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                    this.CurrentSession.StopBusyIndicator();

                }

            });

        }



    }

}
