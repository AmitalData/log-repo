import {Component, ChangeDetectorRef}  from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {CashBookPM} from '../../../EntityPMs/CashBookPM';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './CashBookGeneralTabComponent.html'
})

export class CashBookGeneralTabComponent extends BaseComponent {
    public oldCurrency: string = null;
    public EntityPM: CashBookPM = null;
    public ObjectTableName = "CashBook";
    public DataContext = this;
    public isRTL: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        // Set Entity
        this.EntityPM = entityArgs.EntityPM;
        this.SetUIProperties();

        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        console.log("Entity Reloaded");
                    }
                });
            }
        }
    }

    //#region Properties
    get CashBookTypeCode() { return this.EntityPM.CashBookTypeCode; }
    set CashBookTypeCode(value: string) {
        if (this.EntityPM.CashBookTypeCode != value) {
            this.EntityPM.CashBookTypeCode = value;
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

    glAccount: GLAccountPM;
    get GLAccount() { return this.glAccount; }
    set GLAccount(value: GLAccountPM) {
        if (this.glAccount != value) {
            this.glAccount = value;
        }
    }

    get CurrencyCode() { return this.EntityPM.CurrencyCode; }
    set CurrencyCode(value: string) {
        if (this.EntityPM.CurrencyCode != value) {
            this.EntityPM.CurrencyCode = value;
        }
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
        }
    }

    get AccountNumber() { return this.EntityPM.AccountNumber; }
    set AccountNumber(value: string) {
        if (this.EntityPM.AccountNumber != value) {
            this.EntityPM.AccountNumber = value;
        }
    }

    //#endregion

    SetUIProperties() {
        //if (!this.EntityPM.TypeCode) {
        //    this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
        //}

    }

}
