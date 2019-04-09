
import {Component}  from '@angular/core';
import {AppTool, ArrayTool} from '../../../../../../Infrastructure/Tools';
import { SupplierInvoiceItemPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import {BaseComponent} from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import {SessionLocator} from '../../../../../../Infrastructure/Utilities/SessionLocator';


@Component({
    moduleId: module.id,
    templateUrl: './AddEditActualLinesComponent.html',
})

export class AddEditActualLinesComponent extends BaseComponent {
    invoiceItemPM: SupplierInvoiceItemPM;
    IsDisplayOnly: boolean;
    DataContext: any = this;
    public ItemsSource: ObservableCollection;
    public ValidationErrorsList: string[] = [];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);

    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.invoiceItemPM = args.SupplierInvoiceItemPM;
        
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.BuildActualInvoiceLines();

        }
    }


      BuildActualInvoiceLines()
      {
          this.ItemsSource.Clear();

      var dataString:string = "";

      if (!AppTool.IsNullOrEmpty(this.invoiceItemPM.ActualInvoiceLines)) {
       
          dataString = this.invoiceItemPM.ActualInvoiceLines.trim();

          var dataArray: string[] = dataString.split(',');

        for(let number of dataArray)
        {
            this.ItemsSource.Insert(new ActualLineItem(number.toString(), this));
           
        }
    }
}

    fromNumber: number;
    get FromNumber() { return this.fromNumber }
    set FromNumber(value: number) { this.fromNumber = value; }

    toNumber: number;
    get ToNumber() { return this.toNumber }
    set ToNumber(value: number) { this.toNumber = value; }



    AddActualLines() {
        if (this.ToNumber == null) {
            this.ItemsSource.Insert(new ActualLineItem("",this));
        }
        else if (this.FromNumber != null && this.ToNumber >= this.FromNumber && this.ToNumber > 0 && this.FromNumber >= 0) {

            for (var i = this.FromNumber; i <= this.ToNumber; i++) {



                var number: string = i.toString();
                var existed: ActualLineItem = this.ItemsSource.Collection.filter(d => d.Number == number)[0];
                if (existed == null && number !="0") {
                    var line: ActualLineItem = new ActualLineItem(number,this);
                    this.ItemsSource.Insert(line);
                }
            }
            this.FromNumber = null;
            this.ToNumber = null;

        }

    }


    CancelButtonClicked() {

        this.CurrentSession.CloseCurrentWindowEmit("cancel");
    }

    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors = [];
        var dataString:string = null;
        for (let item of this.ItemsSource.Collection)
        {
            if (AppTool.IsNullOrEmpty(item.Number)) {
                errors.push("חסר ערכים בשורה");
            }

            if (AppTool.IsNullOrEmpty(dataString)) {
                dataString = item.Number;
            }

            else {
                dataString += "," + item.Number;
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
           
            this.CurrentSession.CloseCurrentWindowEmit(dataString);
        }

     

    }

}



export class ActualLineItem extends BaseComponent{
    parent: AddEditActualLinesComponent;
    DataContext: any = this;
    constructor(number: string, Parent: AddEditActualLinesComponent) {
        super();
        this.Number = number;
        this.parent = Parent;
    }
  
    number: string;
    get Number() { return this.number }
    set Number(value: string) { this.number = value; }


    DeleteButtonClicked()
    {
        if (this.parent.ItemsSource.Collection.includes(this)) {
            this.parent.ItemsSource.Remove(this);
        }
    }
 }
