import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TariffVersionPM } from '../../../EntityPMs/TariffVersionPM';
import { TariffLinePM } from '../../../EntityPMs/TariffLinePM';
import { DateTool } from '../../../../Infrastructure/Tools';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { TariffDomainService } from '../../../Services/TariffDomainService';
@Component({
    selector: 'TariffDatesValidationComponent',
    moduleId: module.id,
    templateUrl: './TariffDatesValidationComponent.html',
})

export class TariffDatesValidationComponent extends BaseComponent {
    
    private CurrentSession = SessionLocator.SelectedSession;
    public DataContext = this;
    public ObjectTableName = "Tariff";
    public EntityVersionPM: TariffVersionPM;
    public EntityLinePM: TariffLinePM;
    public ValidationErrorsList: string[] = [];
    public TariffType: string;
    constructor() {
        super();
        this.EntityVersionPM = new TariffVersionPM(null);
        this.EntityLinePM= new  TariffLinePM(null);
    }

    SetWindowArgs(args: any) {
        this.EntityVersionPM = args['CurrentVersion'];
        this.EntityLinePM = args['CurrentLine'];
        this.TariffType = args['TariffType'];
        this.Clone();
    }

    get StartDate() {
        return this.EntityVersionPM.StartDate;
    }
    set StartDate(value: Date) {
        if (this.EntityVersionPM.StartDate != value) {
            this.EntityVersionPM.StartDate = value;
        }
    }

    get InitialEnddate() {
        return this.EntityVersionPM.InitialEnddate;
    }
    set InitialEnddate(value: Date) {
        if (this.EntityVersionPM.InitialEnddate != value) {
            this.EntityVersionPM.InitialEnddate = value;
        }
    }

    private lineExpirationDate: Date;
    get LineExpirationDate() {
        return this.lineExpirationDate;
    }
    set LineExpirationDate(value: Date) {
        if (this.lineExpirationDate != value) {
            this.lineExpirationDate = value;
        }
    }

    // Commands
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];

        if (this.TariffType == "AFC" || this.TariffType == "OLC" || this.TariffType == "OFC") {
            if (this.StartDate == null) {
                this.ValidationErrorsList.push("Start date must be less than start date");
            }

            if (this.InitialEnddate != null && DateTool.GetDateParts(this.InitialEnddate).DateTicks < DateTool.GetCurrentDateAsUtc().valueOf()) {
                this.ValidationErrorsList.push("Can't set Expiration date Field to past date");
            }
        }

        else if (this.TariffType == "ASC" || this.TariffType == "OSC") {
            if (this.LineExpirationDate == null) {
                this.ValidationErrorsList.push("Expiration Date is required");
            }

            else {
                this.EntityLinePM.ExpirationDate = this.LineExpirationDate;

                var service: TariffDomainService = new TariffDomainService();
                service.GetCheckDatesValidty(this.EntityLinePM.OriginPortId, this.EntityLinePM.DestinationPortId, this.LineExpirationDate, this.EntityLinePM.TariffId).subscribe(result => {
                    if (result.HasError) {
                        this.ValidationErrorsList = this.ValidationErrorsList.concat(result.ErrorsArray);
                    }

                    if (this.ValidationErrorsList.length == 0) {
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                });
            }
        }

        if (this.ValidationErrorsList.length == 0 && (this.TariffType != "ASC" && this.TariffType != "OSC")) {
            this.CurrentSession.CloseCurrentWindowEmit("ok");
        }      
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);

        if (this.TariffType == "AFC" || this.TariffType == "OLC" || this.TariffType == "OFC") {
            this.myCloner.AddField('StartDate');
            this.myCloner.AddField('ExpirationDate');
            this.myCloner.AddEntity(this.EntityVersionPM);
        }

        else if (this.TariffType == "ASC" || this.TariffType == "OSC") {
            //this.myCloner.AddField('LineExpirationDate');
            //this.myCloner.AddEntity(this.EntityLinePM);
        }
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
