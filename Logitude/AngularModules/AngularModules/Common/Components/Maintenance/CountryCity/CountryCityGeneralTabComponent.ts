import {Component, OnInit, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CountryCityPM} from '../../../EntityPMs/CountryCityPM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {EntityArgs} from  '../../../../Infrastructure/DataContracts/EntityArgs';
import {StateList} from '../../../EntityLists/StateList';
import {CountryList} from '../../../EntityLists/CountryList';

@Component({
    moduleId: module.id,
    templateUrl: './CountryCityGeneralTabComponent.html',
})

export class CountryCityGeneralTabComponent extends BaseComponent implements OnInit, OnDestroy {
    public EntityPM: CountryCityPM;
    public DataContext = this;
    public IsNewEntity: boolean = true;
    public ObjectTableName: string = "CountryCity";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public args: EntityArgs) {
        super();

        this.EntityPM = args.EntityPM;
        this.Listen();

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
            this.IsNewEntity = true;
        }

        else {
            this.IsNewEntity = false;
        }        
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public SetUIProperties() {
        this.SetUIProperties_State();        
    }
    private SetUIProperties_State() {
        this.SetUIProperties_StateEnabled();
        this.SetUIProperties_StateRequired();
    }
    private SetUIProperties_StateEnabled() {
        var isEnabled = false;

        if (this.Country != null) {
            if (this.Country.HasStates) {
                isEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("StateId", this.ObjectTableName, isEnabled);
    }
    private SetUIProperties_StateRequired() {
        var isRequired = false;

        if (this.Country != null) {
            if (this.State == null) {
                if (this.Country.IsStateRequired) {
                    isRequired = true;
                }
            }
        }

        this.UIProperties.SetRequired("StateId", this.ObjectTableName, isRequired);
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get Notes() { return this.EntityPM.Notes; }
    set Notes(value: string) {
        if (this.EntityPM.Notes != value) {
            this.EntityPM.Notes = value;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    private country: CountryList = null;
    get Country() { return this.country; }
    set Country(value: CountryList) {
        if (this.country != value) {
            this.country = value;
            this.OnCountryChanged(value);
        }
    }

    get CountryId() { return this.EntityPM.CountryId; }
    set CountryId(value: string) {
        if (this.EntityPM.CountryId != value) {
            this.EntityPM.CountryId = value;
            this.StateId = null;
        }
    }

    get CountryCode() { return this.EntityPM.CountryCode; }
    set CountryCode(value: string) {
        if (this.EntityPM.CountryCode != value) {
            this.EntityPM.CountryCode = value;
        }
    }

    get CountryEnglishName() { return this.EntityPM.CountryEnglishName; }
    set CountryEnglishName(value: string) {
        if (this.EntityPM.CountryEnglishName != value) {
            this.EntityPM.CountryEnglishName = value;
        }
    }

    private state: StateList = null;
    get State() { return this.state; }
    set State(value: StateList) {
        if (this.state != value) {
            this.state = value;
            this.OnStateChanged(value);
        }
    }

    get StateId() { return this.EntityPM.StateId; }
    set StateId(value: string) {
        if (this.EntityPM.StateId != value) {
            this.EntityPM.StateId = value;
        }
    }

    get StateCode() { return this.EntityPM.StateCode; }
    set StateCode(newValue: string) {
        if (this.EntityPM.StateCode != newValue) {
            this.EntityPM.StateCode = newValue;
        }
    }

    get StateEnglishName() { return this.EntityPM.StateEnglishName; }
    set StateEnglishName(newValue: string) {
        if (this.EntityPM.StateEnglishName != newValue) {
            this.EntityPM.StateEnglishName = newValue;
        }
    }

    private OnCountryChanged(list: CountryList) {
        if (list == null) {
            this.CountryCode = null;
            this.CountryEnglishName = null;
        }

        else {
            this.CountryCode = list.Code;
            this.CountryEnglishName = list.EnglishName;
        }

        this.SetUIProperties_State();
    }
    private OnStateChanged(list: StateList) {
        if (list == null) {
            this.StateCode = null;
            this.StateEnglishName = null;
        }

        else {
            this.StateCode = list.Code;
            this.StateEnglishName = list.EnglishName;
        }

        this.SetUIProperties_StateRequired();
    }
}
