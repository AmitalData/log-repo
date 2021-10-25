import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
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
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('quoteType')){
      this.addFormControls()
      this.subscribeCtrls();
      this.addDefaultValue()
    }
  }

  addFormControls() {
    const dateNow = new Date();
    const nextMonth = new Date(new Date().setMonth(new Date().getMonth()+1));
    const closeDate = new Date(new Date().setDate(new Date().getDate()+45));
    dateNow.setHours(0,0,0,0);
    nextMonth.setHours(0,0,0,0);
    closeDate.setHours(0,0,0,0);

    this.formGroup.addControl('quoteType', new FormControl('', Validators.required));
    this.formGroup.addControl('moveType', new FormControl('', Validators.required));
    this.formGroup.addControl('startDate', new FormControl(dateNow, Validators.required));
    this.formGroup.addControl('expirationDays', new FormControl(30, Validators.required));
    this.formGroup.addControl('expirationDate', new FormControl(nextMonth, Validators.required));
    this.formGroup.addControl('isAutomaticallyClosed', new FormControl(true));
    this.formGroup.addControl('automaticallyCloseDays', new FormControl(45));
    this.formGroup.addControl('automaticallyCloseDate', new FormControl(closeDate));
  }
  
  subscribeCtrls() {
    this.formGroup.controls.moveType.valueChanges.subscribe((newVal:MoveTypeList) => this.EntityPM.MoveTypeId = newVal?.Id)
    this.formGroup.controls.startDate.valueChanges.subscribe(newVal => this.EntityPM.StartDate = newVal)
    this.formGroup.controls.expirationDays.valueChanges.subscribe(newVal => this.EntityPM.ExpirationDays = newVal)
    this.formGroup.controls.expirationDate.valueChanges.subscribe(newVal => this.EntityPM.ExpirationDate = newVal)
    this.formGroup.controls.isAutomaticallyClosed.valueChanges.subscribe(newVal=> this.EntityPM.IsAutomaticallyClosed = newVal)
    this.formGroup.controls.automaticallyCloseDays.valueChanges.subscribe(newVal => this.EntityPM.AutomaticallyCloseDays = newVal)
    this.formGroup.controls.automaticallyCloseDate.valueChanges.subscribe(newVal => this.EntityPM.AutomaticallyCloseDate = newVal)
  }

  addDefaultValue() {
    this.checkType(this.quoteTypes[0].code);
  }
  
  checkType(code:string) {
    this.formGroup.controls.quoteType.setValue(code, {emitEvent: false});
    this.EntityPM.QuoteTypeCode = code;
  }

  expirationDaysChange(e: any) {
    let date: Date =new Date(this.formGroup.controls.startDate.value.getTime());
    if (-1 < e.value) {
      date.setDate(date.getDate() + e.value)
      this.formGroup.controls.expirationDate.setValue(date);
    }
  }
    
  closeDaysChange(e: any) {
    let date: Date =new Date(this.formGroup.controls.startDate.value.getTime());
    if (-1 < e.value) {
      date.setDate(date.getDate() + e.value)
      this.formGroup.controls.automaticallyCloseDate.setValue(date);
    }
  }

  onSelectedMoveType(val:MoveTypeList){
    this.EntityPM.MoveTypeId = val.Id;
  }
}
