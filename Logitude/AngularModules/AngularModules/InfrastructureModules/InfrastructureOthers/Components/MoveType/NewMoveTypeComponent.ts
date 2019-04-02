import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {Component} from '@angular/core';
import {MoveTypePM} from '../../../../Infrastructure/EntityPMs/MoveTypePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {MoveTypePMService} from '../../../../Infrastructure/Services/StandardPMs/MoveTypePMService';
import {ClassLevelValidator} from '../../../../Infrastructure/Validators/ClassLevelValidator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './NewMoveTypeComponent.html',    
})


export class NewMoveTypeComponent extends BaseComponent {

    public EntityPM: MoveTypePM;
    public DataContext: NewMoveTypeComponent = this;
    public ObjectTableName: string = "MoveType";
    public TenantPM: TenantPM;
    validator: ClassLevelValidator;
    public ValidationErrorsList: string[] = [];
    public MoveTypePM: MoveTypePM;
    public MoveTypePMService: MoveTypePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.EntityPM = new MoveTypePM();
        this.TenantPM = SessionLocator.TenantPM;
        this.MoveTypePM = new MoveTypePM();
        this.EntityPM = this.MoveTypePM;
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.MoveTypePMService = new MoveTypePMService();
        this.validator = new ClassLevelValidator();
        this.EntityPM.AddedManually = true;

    }
    private transportMode: string = null;
    public get TransportMode() { return this.transportMode; }

    public set TransportMode(value: string) { if (value != null) this.transportMode = value; }
    public get Code() { return this.MoveTypePM.Code; }
    public set Code(value: string) { this.MoveTypePM.Code = value; }

    public get MoveTypeEnglishName() { return this.MoveTypePM.MoveTypeEnglishName; }
    public set MoveTypeEnglishName(value: string) { this.MoveTypePM.MoveTypeEnglishName = value; }


    public get MoveTypeLocalName() { return this.MoveTypePM.MoveTypeLocalName; }
    public set MoveTypeLocalName(value: string) { this.MoveTypePM.MoveTypeLocalName = value; }


    OkButtonClicked() {
        this.ValidationErrorsList = [];

        this.BuildErrors();

        if (this.ValidationErrorsList.length == 0) {

            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");

            this.MoveTypePMService.insert(this.MoveTypePM).subscribe(res => {
                this.CurrentSession.CurrentWindow.StopBusyIndicator();

                var pmResponse: ServiceResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        this.CurrentSession.CloseCurrentWindow();
                    }
                }
                else {

                    pmResponse.ErrorsArray.forEach((item) => {
                        this.ValidationErrorsList.push(item);
                    });

                }



            })


        }


    }

    BuildErrors() {

        this.MoveTypePM.TransportModeId = this.TransportMode;


        var errorsArray = this.validator.Validate("MoveType", this.MoveTypePM);
        if (errorsArray.length > 0) {
            errorsArray.forEach((item) => {
                this.ValidationErrorsList.push(item);
            });
        }
    }

    CancelButtonClicked() {

        this.CurrentSession.CloseCurrentWindow();

    }
}
