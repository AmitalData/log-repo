import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { MoveTypeList } from 'Infrastructure/EntityLists/MoveTypeList';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { Incoterm, NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-general',
  templateUrl: './new-quote-general.component.html',
  styleUrls: ['./new-quote-general.component.scss']
})
export class NewQuoteGeneralComponent implements OnInit {
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() formGroup: FormGroup = new FormGroup({});

  quoteTypes: { text: string, code: string }[] = [
    { text: TextCodeTranslator.Translate('QuoteOP.S.NewQuote.SpotRate'), code: 'A' },
    { text: TextCodeTranslator.Translate('QuoteOP.S.NewQuote.RoutingRates'), code: 'P' },
  ]

  moveTypeSelected: string[] = []  
  moveTypes: MoveTypeList[] = []

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) {}

  ngOnInit(): void {
    this.getMoveTypeData()    
  }


  async getMoveTypeData(){
    this.moveTypes = await this.newQuoteDataService.getMoveTypeTable("A");
    console.log(this.moveTypes)
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('quoteType'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('quoteType', new FormControl(''));
    this.formGroup.addControl('incotermId', new FormControl(''));
    this.formGroup.addControl('moveType', new FormControl(''));
    this.formGroup.addControl('startDate', new FormControl(''));
    this.formGroup.addControl('expirationDays', new FormControl(''));
    this.formGroup.addControl('expirationDate', new FormControl(''));
    this.formGroup.addControl('isAutomaticallyClosed', new FormControl(''));
    this.formGroup.addControl('automaticallyCloseDays', new FormControl(''));
    this.formGroup.addControl('automaticallyCloseDate', new FormControl(''));
  }
  
  checkType(code:string) {
    this.formGroup.controls.quoteType.setValue(code, {emitEvent: false});
    this.EntityPM.QuoteTypeCode = code;
  }

  subscribeCtrls() {
    this.formGroup.controls.startDate.valueChanges.subscribe(newVal => this.EntityPM.StartDate = newVal)
    this.formGroup.controls.expirationDays.valueChanges.subscribe(newVal => this.EntityPM.ExpirationDays = newVal)
    this.formGroup.controls.expirationDate.valueChanges.subscribe(newVal => this.EntityPM.ExpirationDate = newVal)
    this.formGroup.controls.isAutomaticallyClosed.valueChanges.subscribe(newVal=> this.EntityPM.IsAutomaticallyClosed = newVal)
    this.formGroup.controls.automaticallyCloseDays.valueChanges.subscribe(newVal => this.EntityPM.AutomaticallyCloseDays = newVal)
    this.formGroup.controls.automaticallyCloseDate.valueChanges.subscribe(newVal => this.EntityPM.AutomaticallyCloseDate = newVal)
  }

  expirationDaysChange(e: any) {
    let date: Date = this.formGroup.controls.startDate.value;
    if (-1 < e.value) {
      date.setDate(date.getDate() + e.value)
      this.formGroup.controls.expirationDate.setValue(date);
    }
  }

  closeDaysChange(e: any) {
    let date: Date = this.formGroup.controls.startDate.value;
    if (-1 < e.value) {
      date.setDate(date.getDate() + e.value)
      this.formGroup.controls.automaticallyCloseDate.setValue(date);
    }
  }

  onSelectedIncoterm(val:any){
    this.EntityPM.IncotermId = val.Id;    
  }

  onSelectedMoveType(val:MoveTypeList){
    this.EntityPM.MoveTypeId = val.Id;
  }
}
