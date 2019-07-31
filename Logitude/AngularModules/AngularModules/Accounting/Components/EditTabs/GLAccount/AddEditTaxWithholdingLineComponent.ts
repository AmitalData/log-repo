import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {GLAccountWithholdingTaxPM} from '../../../EntityPMs/GLAccountWithholdingTaxPM';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DateTool} from '../../../../Infrastructure/Tools';
import { GLAccountPMService } from '../../../Services/StandardPMs/GLAccountPMService';

import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { GLAccountTaxWithholdingTabComponent } from './GLAccountTaxWithholdingTabComponent';




@Component({
    moduleId: module.id,
    templateUrl: './AddEditTaxWithholdingLineComponent.html',

})



export class AddEditTaxWithholdingLineComponent extends BaseComponent {

    DataContext: any = this;
    ObjectTableName: string = "GLAccountWithholdingTax";
    entity: GLAccountWithholdingTaxPM;
    parent: GLAccountPM;
    LastLineToDate: Date;
  newEntity: GLAccountWithholdingTaxPM;
  gLAccountPMService: GLAccountPMService = new GLAccountPMService();
    public IsNew: boolean;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        
    }
    SetWindowArgs(args) {
        this.parent = args.GLAccount;
        this.entity = args.GLAccountWithholdinTax;
        this.LastLineToDate = args.LastLineToDate;
        this.newEntity = new GLAccountWithholdingTaxPM(this.parent);
        this.newEntity.Percentage = this.entity.Percentage;
        this.newEntity.FromDate = this.entity.FromDate;
        this.newEntity.ToDate = this.entity.ToDate;
        this.newEntity.Inactive = this.entity.Inactive;
        var today: Date = new Date();
        this.IsNew = args.IsNew;
        if (this.LastLineToDate == null || this.LastLineToDate.valueOf() < today.valueOf()) {
            if (this.ToDate == null) {
                this.FromDate = new Date();
            }
            // this.LastLineToDate = new Date();
        }
    }
    validationMsg1: string = null;
    lessOrGreatMsg: string = null;

    fromDate: Date;
    get FromDate() { return this.newEntity.FromDate; }
    set FromDate(value: Date) {
        this.UIProperties.SetRequired("FromDate", this.ObjectTableName, false);
        if (this.newEntity.FromDate != value) {
            if (value) {
                this.newEntity.FromDate = value;
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, null);
              var datetocompare = DateTool.GetDateParts(this.LastLineToDate).DateTicks;
              var dateFromcompare = DateTool.GetDateParts(this.LastLineFromDate).DateTicks;


              var fromDateParts = DateTool.GetDateParts(value).DateTicks;
             
                
                if ((this.newEntity.ToDate && value > this.newEntity.ToDate)) {

                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.O.MustBeLess"));
                    this.lessOrGreatMsg = TextCodeTranslator.Translate("Accounting.O.MustBeLess");

                }

                else {
                  this.lessOrGreatMsg = null;
                  this.validationMsg = null;
                }

              if (this.LastLineToDate && (fromDateParts < datetocompare || fromDateParts == datetocompare)) {

                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, TextCodeTranslator.Translate("GLAccounts.O.NotValidDate"));
                    this.validationMsg1 = TextCodeTranslator.Translate("GLAccounts.O.NotValidDate");
                  

                }
                else {
                   this.validationMsg1 = null;
                   this.validationMsg = null;
                
                }

              
            }
            else {
                this.validationMsg = null;
                this.validationMsg1 = null;
                this.fromDate = null;
            
               // this.UIProperties.SetRequired("FromDate", this.ObjectTableName, true);
                this.newEntity.FromDate = null;
            }
        }

    }
    validationMsg: string = null;
    toDate: Date;
    get ToDate() { return this.newEntity.ToDate; }
    set ToDate(value: Date) {
        this.UIProperties.SetRequired("ToDate", this.ObjectTableName, false);
        if (this.newEntity.ToDate != value) {
            if (value) {
                this.newEntity.ToDate = value;
                if (this.newEntity.FromDate && (this.newEntity.FromDate > value || this.newEntity.FromDate.valueOf() == value.valueOf())) {
                    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.O.MustBeLarger"));
                    this.lessOrGreatMsg = TextCodeTranslator.Translate("Accounting.O.MustBeLarger");
                 
                }
                else {
                   // this.newEntity.ToDate = value;
                    //this.LastLineToDate = value;
                    this.lessOrGreatMsg = null;
                }
            }

            else {
               // this.toDate = null;
                this.UIProperties.SetRequired("ToDate", this.ObjectTableName, true);
                this.newEntity.ToDate = null;
            }
        }
    }
    percantage: number;
    get Percentage() { return this.newEntity.Percentage; }
    set Percentage(value: number) {
        this.UIProperties.SetRequired("Percentage", this.ObjectTableName, false);
        if (this.newEntity.Percentage != value) {
          
           
                this.newEntity.Percentage = value;
            
        }
    }

    get Inactive() { return this.newEntity.Inactive; }
    set Inactive(value: boolean) {
        if (this.newEntity.Inactive != value) {
            this.newEntity.Inactive = value;
        }
    }

    public ValidationErrorsList: string[] = [];

    OkButtonClicked() {
        var FIELD_IS_REQUIERD: string = null;
        FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        this.ValidationErrorsList = [];
        if (this.validationMsg != null) {

            errors.push(this.validationMsg);
        }
        if (this.validationMsg1 != null) {
            errors.push(this.validationMsg1);
        }
        if (this.lessOrGreatMsg != null) {
            errors.push(this.lessOrGreatMsg);
        }
            if (this.FromDate == undefined) {
                var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccountWithholdingTax.F.FromDate"));
                errors.push(s);
            }
            if (this.ToDate == undefined) {
                var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccountWithholdingTax.F.ToDate"));
                errors.push(s);
            }
            if (this.Percentage == undefined) {
                var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccountWithholdingTax.F.Percentage"));
                errors.push(s);
            }


        //if (this.parent.GLAccountWithholdingTaxes.length > 0) {
        //    //if (this.parent.GLAccountWithholdingTaxes.find(d => d.FromDate ))
        //}
            if (errors.length == 0) {
                this.entity.Percentage = this.newEntity.Percentage;
                this.entity.FromDate = this.newEntity.FromDate;
                this.entity.ToDate = this.newEntity.ToDate;
                this.entity.Inactive = this.newEntity.Inactive;
              this.LastLineToDate = this.ToDate;
            
              this.CurrentSession.CloseCurrentWindowEmit("ok");
            //  this.gLAccountPMService.update(this.parent).subscribe((myResponse: ServiceResponse) => { });
           //   this.CurrentSession.CurrentEditComponent.SaveChanges();
            }
            else {
                this.ValidationErrorsList = errors;
            }

     
        
  }
   LastLineFromDate: Date;
    InactiveChecked(checked: boolean) {
        this.entity.Changed = true;
        var lines = this.parent.GLAccountWithholdingTaxes.filter(d => !d.Inactive);
        var items = lines.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });

        var item = items[lines.length - 1];

        if (checked) {
            this.Inactive = true;
            if (item) {
                this.LastLineToDate = item.ToDate;

              
            }
            else {
                this.LastLineToDate = null;
            }

        }
        else {
            this.Inactive = false;
            if (item) {
              this.LastLineToDate = item.ToDate;
              this.LastLineFromDate = item.FromDate;
              var datetocompare = DateTool.GetDateParts(this.LastLineToDate).DateTicks;
              var dateFromcompare = DateTool.GetDateParts(this.LastLineFromDate).DateTicks;

           
              var fromDateParts = DateTool.GetDateParts(this.FromDate).DateTicks;
              var toDateParts = DateTool.GetDateParts(this.ToDate).DateTicks;

             
 
              //var fromDateMonth: number = (this.FromDate.getMonth() +1 -1);
              //var fromDateDay: number = this.FromDate.getDay();
              //var fromDateYear: number = this.FromDate.getFullYear();



              if (fromDateParts == datetocompare || fromDateParts < datetocompare) {

                this.validationMsg = TextCodeTranslator.Translate("GLAccounts.O.NotValidDate");
                }
                else this.validationMsg = null;
            }
            else {
                this.LastLineToDate = null;
            }
        }

        
    }
    CancelButtonClicked() {
        //this.entity.Percentage = this.oldEntity.Percentage;
        //this.entity.FromDate = this.oldEntity.FromDate;
        //this.entity.ToDate = this.oldEntity.ToDate;
        this.CurrentSession.CloseCurrentWindow();

    }
}
