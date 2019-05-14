import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {CourierMasterPM} from '../../../../Customs/EntityPMs/CourierMasterPM';
import { CourierMasterPMService } from '../../../../Customs/Services/StandardPMs/CourierMasterPMService';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../../Infrastructure/Tools';
import {CourierMasterService} from '../../../../Customs/Services/Others/CourierMasterService';

@Component({
    moduleId: module.id,
    templateUrl: './NewCourierComponent.html',
})
     
export class NewCourierComponent extends BaseComponent {

    public EntityPM: CourierMasterPM;
    ObjectTableName: string = "Customs.CourierMaster";
    DataContext: any = this;
    CourierMasterService: CourierMasterService = new CourierMasterService();
 
 
    CourierMasterPMService: CourierMasterPMService = new CourierMasterPMService();
    entityResourceService: EntityResourceService = new EntityResourceService();
    public ValidationErrorsList: string[] = [];
    FIELD_IS_REQUIERD: string;
    QueryNameText: string = "";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.EntityPM = new CourierMasterPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;

        this.EntityPM.MAWBTypeCode= "740";
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
       

    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.QueryNameText = AppTool.IsNullOrEmpty(args.QueryNameTextCode) ? "Back" : TextCodeTranslator.Translate(args.QueryNameTextCode);
        }
    }
    GetRequierdFieldErrorText(fieldName) {
        return this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate(fieldName));
    }


    get AirlineId() { return this.EntityPM.AirlineId; }
    set AirlineId(value: string) {
      
        if (this.EntityPM.AirlineId != value) {
            this.EntityPM.AirlineId = value;
        }
    }

    get MAWB() { return this.EntityPM.MAWB; }
    set MAWB(value: string) {
        if (this.EntityPM.MAWB != value) {
            this.EntityPM.MAWB = value;
        }
    }

    get HAWB() { return this.EntityPM.HAWB; }
    set HAWB(value: string) {
        if (this.EntityPM.HAWB != value) {
            this.EntityPM.HAWB = value;
        }
    }
    

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        // Custom Validation
      

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {

            this.CourierMasterService.GetIfCourierMasterExists(this.EntityPM.Id, this.AirlineId, this.HAWB, this.MAWB).subscribe(Result => {
                var mm: ServiceResponse = Result;
                if (!mm.HasError) {
                    if (!mm.Result) {
                        this.SubmitChanges();
                    }
                    else {
                        errors.push(TextCodeTranslator.Translate("Customs.General.O.CourierAlreadyExist"));//"There is already master courier with the same values");

                        this.ValidationErrorsList = errors; //.push(TextCodeTranslator.Translate("Customs.General.O.CourierAlreadyExist"));//"There is already master courier with the same values");

                    }
                }
                
            });
           
        }

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }



    SubmitChanges() {
        this.CurrentSession.StartBusyIndicator("");
        this.CourierMasterPMService.insert(this.EntityPM).subscribe(Result => {

            var mm: ServiceResponse = Result;
            if (!mm.HasError) {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CloseCurrentWindowEmit("ok");
                var entity = mm.Result;
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName, BackButtonLabel: this.QueryNameText });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.CancelButtonClicked();
                        });
                    });

            }

            else {
                  this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
