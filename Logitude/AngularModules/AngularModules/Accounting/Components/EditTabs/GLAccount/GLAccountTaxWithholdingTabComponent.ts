
import {Component }  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {GLAccountWithholdingTaxPM} from '../../../EntityPMs/GLAccountWithholdingTaxPM';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';

@Component({
    
    templateUrl: './GLAccountTaxWithholdingTabComponent.html',
 
})



export class GLAccountTaxWithholdingTabComponent extends BaseComponent {
  public DeductionType: any;
  public DeductionFileType: any;
  public AssessingOffice: any;

    public  ObjectTableName:string= "GLAccount";
    public DataContext: any = this;
    public Lines: ObservableCollection = new ObservableCollection([]);
    EntityPM: GLAccountPM = null;
    EntityResourceService: EntityResourceService = new EntityResourceService();
    visible: boolean;
    FilterSelectedValue: string = 'Active';
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityResourceService.getEntityResourceByTableName("GLAccountWithholdingTax").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("GLAccount").subscribe((response: any) => {

                this.visible = true;
                this.EntityPM = entityArgs.EntityPM;
                this.InactiveFilter = false;

              this.BuildGLAccountTaxWithholdingLinesList();
              this.Listen();
            });
        });
        
    }

private SaveCompletedEvent: any = null;
private LoadCompletedEvent: any = null;
public CurrentEditComponentId: string;
Listen() {


    if (this.CurrentSession.CurrentEditComponent != null) {
        this.CurrentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

        //
        if (this.SaveCompletedEvent == null) {
            this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.BuildGLAccountTaxWithholdingLinesList();
                }
            });
        }

        //
        if (this.LoadCompletedEvent == null) {
            this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    this.BuildGLAccountTaxWithholdingLinesList();
                }
            });
        }

    }
}

    InactiveFilter: boolean= null
    FilterItemClicked(filtervalue: string) {

        if (filtervalue == "Active") {
            this.InactiveFilter = false;
            this.BuildGLAccountTaxWithholdingLinesList();
            this.FilterSelectedValue = "Active";
        }
        else if (filtervalue == "Inactive") {
            this.InactiveFilter = true;
            this.BuildGLAccountTaxWithholdingLinesList();
            this.FilterSelectedValue = "Inactive";
        }
        else {
            this.InactiveFilter = null;
            this.BuildGLAccountTaxWithholdingLinesList();
            this.FilterSelectedValue = "All";
        }
    }


    get DeductionTypeId() { return this.EntityPM.DeductionTypeId; }
    set DeductionTypeId(value: string) {
        if (this.EntityPM.DeductionTypeId != value) {
            this.EntityPM.DeductionTypeId = value;
        }
    }


    get ConsolidationVat() { return this.EntityPM.ConsolidationVat; }
    set ConsolidationVat(value: string) {
        if (this.EntityPM.ConsolidationVat != value) {
            this.EntityPM.ConsolidationVat = value;
        }
    }

    get DeductionFileNumber() { return this.EntityPM.DeductionFileNumber; }
    set DeductionFileNumber(value: string) {
        if (this.EntityPM.DeductionFileNumber != value) {
            this.EntityPM.DeductionFileNumber = value;
        }
    }

    get DeductionFileTypeId() { return this.EntityPM.DeductionFileTypeId; }
    set DeductionFileTypeId(value: string) {
        if (this.EntityPM.DeductionFileTypeId != value) {
            this.EntityPM.DeductionFileTypeId = value;
        }
    }
    get AssessingOfficeCode() { return this.EntityPM.AssessingOfficeCode; }
    set AssessingOfficeCode(value: string) {
        if (this.EntityPM.AssessingOfficeCode != value) {
            this.EntityPM.AssessingOfficeCode = value;
        }
    }

    get Occupation() { return this.EntityPM.Occupation; }
    set Occupation(value: string) {
        if (this.EntityPM.Occupation != value) {
            this.EntityPM.Occupation = value;
        }
    }

    BuildGLAccountTaxWithholdingLinesList() {
       var  sequence:number= 0;
        this.Lines.Clear();
        if (this.InactiveFilter != null) {
            for (let item of this.EntityPM.GLAccountWithholdingTaxes.filter(d => d.Inactive == this.InactiveFilter)) {
                sequence += 1;
                this.Lines.Insert(new GLAccountTaxLine(item, this,sequence));
            }
        }
        else {
            for (let item of this.EntityPM.GLAccountWithholdingTaxes) {
                sequence += 1;
                this.Lines.Insert(new GLAccountTaxLine(item, this, sequence ));
            }
        }

     
    }
  LastLineToDate: Date;
  ValidationErrorsList: string[] = [];
  Add() {
     this.ValidationErrorsList=[];
      var FIELD_IS_REQUIERD: string = null;

      FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");

      if (AppTool.IsNullOrEmpty(this.DeductionFileTypeId) || AppTool.IsNullOrEmpty(this.AssessingOfficeCode) || AppTool.IsNullOrEmpty(this.DeductionTypeId) || AppTool.IsNullOrEmpty(this.DeductionFileNumber)) {

          if (AppTool.IsNullOrEmpty(this.DeductionFileTypeId)) {
              var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccount.F.DeductionFileTypeId"));
              this.ValidationErrorsList.push(s);
          }
          if (AppTool.IsNullOrEmpty(this.AssessingOfficeCode)) {
              var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccount.F.AssessingOfficeCode"));
              this.ValidationErrorsList.push(s);
          }
          if (AppTool.IsNullOrEmpty(this.DeductionTypeId)) {
              var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccount.F.DeductionTypeId"));
              this.ValidationErrorsList.push(s);
          }
          if (AppTool.IsNullOrEmpty(this.DeductionFileNumber)) {
              var s: string = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("GLAccount.F.DeductionFileNumber"));
              this.ValidationErrorsList.push(s);
          }
      }
    else {
      //this.ValidationErrorsList = [];
      var activeItems = this.EntityPM.GLAccountWithholdingTaxes.filter(d => !d.Inactive);
      var items = activeItems.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });

      var item = items[this.Lines.Collection.length - 1];
      if (item) {
        this.LastLineToDate = item.ToDate;
      }
      var windowArgs: any = {};
      windowArgs.GLAccountWithholdinTax = new GLAccountWithholdingTaxPM(this.EntityPM);
      windowArgs.GLAccount = this.EntityPM;
      windowArgs.LastLineToDate = this.LastLineToDate;
      windowArgs.IsNew = true;
      var windowTitle = TextCodeTranslator.Translate("Accounting.O.TaxLineTitle");

      var logWindow = new LogitudeWindow();
      logWindow.Width = 400;
      logWindow.Height = 400;
      logWindow.Title = windowTitle;
      logWindow.ShowCloseButton = false;
      logWindow.WindowArgs = windowArgs;
      logWindow.ComponentLoaded.subscribe(comp => {
        logWindow.WindowClosed.subscribe(s => {
          if (s) {
            this.AddTaxLine(comp);
          }
        });
      });
      logWindow.Show('./Accounting/Components/EditTabs/GLAccount/AddEditTaxWithholdingLineComponent');



    }

    }
    AddTaxLine(s:any) {
        if (s) {
            var line: number = 0;


            var items = this.EntityPM.GLAccountWithholdingTaxes.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });
            if (items.length == 0) line = 0;
            else {
                line = items[this.EntityPM.GLAccountWithholdingTaxes.length - 1].LineNumber;
            }
            line += 1;
            //var item = items[this.Lines.Collection.length - 1];
            //if (item) {
            //    this.LastLineToDate = item.ToDate;
            //}
            var item: GLAccountWithholdingTaxPM =s.entity;
            item.GLAccountId = this.EntityPM.Id;
            item.Tenant = this.EntityPM.Tenant;
            item.LineNumber = line;
            item.CreateDate = new Date();
            item.CreatedByUserId = SessionLocator.LoggedUserId;
            item.ChangeSetOp = "Insert";


            this.LastLineToDate = s.ToDate;

            item.Inactive = false;
            this.EntityPM.AddGLAccountWithholdingTax(item);
            if (this.FilterSelectedValue == "Active") this.InactiveFilter = false;
            else if (this.FilterSelectedValue == "Inactive") this.InactiveFilter = true;
            else this.InactiveFilter = null;
            this.BuildGLAccountTaxWithholdingLinesList();
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }



    }
}

export class GLAccountTaxLine extends BaseComponent {
    entity: GLAccountWithholdingTaxPM;
    parent: GLAccountTaxWithholdingTabComponent;
    ObjectTableName: string = "GLAccountWithholdingTax";
    Sequence: number;
    constructor(Entity: GLAccountWithholdingTaxPM, Parent: GLAccountTaxWithholdingTabComponent, sequence: number) {
        super();
        this.entity = Entity;
        this.parent = Parent;
        this.Sequence = sequence;
        this.UIProperties.SetEnabled("Inactive", this.ObjectTableName, false);
    }

    get LineNumber() { return this.entity.LineNumber; }
    set LineNumber(value: number) {
        if (this.entity.LineNumber != value) {
            this.entity.LineNumber = value;
        }
    }

    get FromDate() { return this.entity.FromDate; }
    set FromDate(value: Date) {
        if (this.entity.FromDate != value) {
            if (value) {
                this.UIProperties.SetValidity("FromDate", this.ObjectTableName, true, null);

                //var datetocompare = DateTool.GetDateParts(this.LastLineToDate).DateObject;
                if ((this.entity.ToDate && value > this.entity.ToDate)) {
                    //var msg = new MessageWindow();
                    this.UIProperties.SetValidity("FromDate", this.ObjectTableName, false, "From date must be less than to date");

                    //msg.Show(TextCodeTranslator.Translate("GLAccounts.O.NotValidDate"));
                    this.entity.FromDate = null;
                }
             


                else {
                    this.entity.FromDate = value;
                    //  this.parent.LastLineToDate = value;
                }
            }
            else {
                this.entity.FromDate = null;
            }
        }

    }

    get ToDate() { return this.entity.ToDate; }
    set ToDate(value: Date) {
        if (this.entity.ToDate != value) {
            if (value) {

                if (this.entity.FromDate && (this.entity.FromDate > value || this.entity.FromDate.valueOf() == value.valueOf())) {
                    this.UIProperties.SetValidity("ToDate", this.ObjectTableName, false, "To date must be larger than from date");


                    this.entity.ToDate = null;
                }
                else {
                    this.entity.ToDate = value;
                  //  this.LastLineToDate = value;
                }
            }

            else {
                this.entity.ToDate = null;
            }
        }
    }


    get Percentage() { return this.entity.Percentage; }
    set Percentage(value: number) {
        if (this.entity.Percentage != value) {
            this.entity.Percentage = value;
        }
    }

   
    get Inactive() { return this.entity.Inactive; }
    set Inactive(value: boolean) {
        if (this.entity.Inactive != value) {
            this.entity.Inactive = value;
        }
    }
  
  

    EditItem(item:this) {

        var windowArgs: any = {};
        windowArgs.GLAccountWithholdinTax =this.entity;
        windowArgs.GLAccount = this.parent.EntityPM;
        windowArgs.IsNew = false;

        var windowTitle = TextCodeTranslator.Translate("Accounting.O.EditTaxPeriod");

        var logWindow = new LogitudeWindow();
        logWindow.Width = 400;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.ComponentLoaded.subscribe(comp => {
            logWindow.WindowClosed.subscribe(s => {
                if (s) {
                    this.EditTaxLine(comp);
                }
            });
        });
        logWindow.Show('./Accounting/Components/EditTabs/GLAccount/AddEditTaxWithholdingLineComponent');



    }

 

   EditTaxLine(s: any) {
        if (s) {
          
         
          this.entity = s.entity;
          this.entity.ChangeSetOp = "Update";
   
          this.parent.BuildGLAccountTaxWithholdingLinesList();
       //   this.CurrentSession.CurrentEditComponent.SaveChanges();
        
        }



    }

}
