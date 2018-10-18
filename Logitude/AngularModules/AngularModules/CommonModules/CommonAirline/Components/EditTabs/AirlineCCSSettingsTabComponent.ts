import {Component, OnInit}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';

@Component({
    moduleId: module.id,
    templateUrl: './AirlineCCSSettingsTabComponent.html',
})

export class AirlineCCSSettingsTabComponent extends BaseComponent implements OnInit {
    public EntityPM: AirlinePM;
    public ObjectTableName: string = "Airline";
    public DataContext: AirlineCCSSettingsTabComponent = this;    
    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
    }

    ngOnInit() {
        if (this.EntityPM != null) {
            this.SetUIProperties();
        }
    }

    private SetUIProperties() {
        var isRegistrationNotesVisible: boolean = false;

        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "RegistrationNotes")) {
            isRegistrationNotesVisible = true;
        }

        this.UIProperties.SetVisibility("RegistrationNotes", this.ObjectTableName, isRegistrationNotesVisible);
    }

    get TTY() { return this.EntityPM.TTY; }
    set TTY(newValue: string) {
        if (this.EntityPM.TTY != newValue) {
            this.EntityPM.TTY = newValue;
        }
    }

    get GLSHKPIMA() { return this.EntityPM.GLSHKPIMA; }
    set GLSHKPIMA(newValue: string) {
        if (this.EntityPM.GLSHKPIMA != newValue) {
            this.EntityPM.GLSHKPIMA = newValue;
        }
    }

    get RegistrationNotes() { return this.EntityPM.RegistrationNotes; }
    set RegistrationNotes(newValue: string) {
        if (this.EntityPM.RegistrationNotes != newValue) {
            this.EntityPM.RegistrationNotes = newValue;
        }
    }

    get ChampFSRFSA() { return this.EntityPM.ChampFSRFSA; }
    set ChampFSRFSA(newValue: boolean) {
        if (this.EntityPM.ChampFSRFSA != newValue) {
            this.EntityPM.ChampFSRFSA = newValue;
        }
    }

    get GLSHKFSRFSA() { return this.EntityPM.GLSHKFSRFSA; }
    set GLSHKFSRFSA(newValue: boolean) {
        if (this.EntityPM.GLSHKFSRFSA != newValue) {
            this.EntityPM.GLSHKFSRFSA = newValue;
        }
    }

    get ChampFSU() { return this.EntityPM.ChampFSU; }
    set ChampFSU(newValue: boolean) {
        if (this.EntityPM.ChampFSU != newValue) {
            this.EntityPM.ChampFSU = newValue;
        }
    }

    get GLSHKFSU() { return this.EntityPM.GLSHKFSU; }
    set GLSHKFSU(newValue: boolean) {
        if (this.EntityPM.GLSHKFSU != newValue) {
            this.EntityPM.GLSHKFSU = newValue;
        }
    }

    get ChampFWB() { return this.EntityPM.ChampFWB; }
    set ChampFWB(newValue: boolean) {
        if (this.EntityPM.ChampFWB != newValue) {
            this.EntityPM.ChampFWB = newValue;
        }
    }

    get GLSHKFWB() { return this.EntityPM.GLSHKFWB; }
    set GLSHKFWB(newValue: boolean) {
        if (this.EntityPM.GLSHKFWB != newValue) {
            this.EntityPM.GLSHKFWB = newValue;
        }
    }

    get ChampFHL() { return this.EntityPM.ChampFHL; }
    set ChampFHL(newValue: boolean) {
        if (this.EntityPM.ChampFHL != newValue) {
            this.EntityPM.ChampFHL = newValue;
        }
    }

    get GLSHKFHL() { return this.EntityPM.GLSHKFHL; }
    set GLSHKFHL(newValue: boolean) {
        if (this.EntityPM.GLSHKFHL != newValue) {
            this.EntityPM.GLSHKFHL = newValue;
        }
    }

    get ChampFVRFVA() { return this.EntityPM.ChampFVRFVA; }
    set ChampFVRFVA(newValue: boolean) {
        if (this.EntityPM.ChampFVRFVA != newValue) {
            this.EntityPM.ChampFVRFVA = newValue;
        }
    }

    get GLSHKFVRFVA() { return this.EntityPM.GLSHKFVRFVA; }
    set GLSHKFVRFVA(newValue: boolean) {
        if (this.EntityPM.GLSHKFVRFVA != newValue) {
            this.EntityPM.GLSHKFVRFVA = newValue;
        }
    }

    get ChampFFRFFA() { return this.EntityPM.ChampFFRFFA; }
    set ChampFFRFFA(newValue: boolean) {
        if (this.EntityPM.ChampFFRFFA != newValue) {
            this.EntityPM.ChampFFRFFA = newValue;
        }
    }

    get GLSHKFFRFFA() { return this.EntityPM.GLSHKFFRFFA; }
    set GLSHKFFRFFA(newValue: boolean) {
        if (this.EntityPM.GLSHKFFRFFA != newValue) {
            this.EntityPM.GLSHKFFRFFA = newValue;
        }
    }

    get ChampNeedsRegistration() { return this.EntityPM.ChampNeedsRegistration; }
    set ChampNeedsRegistration(newValue: boolean) {
        if (this.EntityPM.ChampNeedsRegistration != newValue) {
            this.EntityPM.ChampNeedsRegistration = newValue;
        }
    }

    get GLSHKNeedsRegistration() { return this.EntityPM.GLSHKNeedsRegistration; }
    set GLSHKNeedsRegistration(newValue: boolean) {
        if (this.EntityPM.GLSHKNeedsRegistration != newValue) {
            this.EntityPM.GLSHKNeedsRegistration = newValue;
        }
    }
}