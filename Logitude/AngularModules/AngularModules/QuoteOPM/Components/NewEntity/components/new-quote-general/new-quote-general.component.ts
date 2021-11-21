import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MoveTypeList } from 'Infrastructure/EntityLists/MoveTypeList';
import { MoveTypePMService } from 'Infrastructure/Services/StandardPMs/MoveTypePMService';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { QuoteOPTypeListService } from 'QuoteOPM/Services/StandardLists/QuoteOPTypeListService';
import { filter } from 'rxjs/operators';
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
  ) { }

  ngOnInit(): void {
    this.getMoveTypeData()
    this.resetForm();
  }

  resetForm() {
    this.newQuoteDataService.$resetForm.subscribe(() => {
      ['moveType', 'startDate', 'expirationDays', 'expirationDate', 'isAutomaticallyClosed', 'automaticallyCloseDays', 'automaticallyCloseDate'].forEach(ctrl => this.formGroup.controls[ctrl].reset())

      this.formGroup.controls.startDate.setValue(this.getDateNow())
    })

  }

  async getMoveTypeData() {
    this.moveTypes = await this.newQuoteDataService.getMoveTypeTable("A");
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('quoteType')) {
      this.addFormControls()
      this.subscribeCtrls();
    }
  }

  addFormControls() {
    const dateNow = this.getDateNow();
    // const nextMonth = new Date(new Date().setMonth(new Date().getMonth()+1));
    // const closeDate = new Date(new Date().setDate(new Date().getDate()+45));
    // nextMonth.setHours(0,0,0,0);
    // closeDate.setHours(0,0,0,0);
    this.formGroup.addControl('quoteType', new FormControl('', Validators.required));
    this.checkType(this.quoteTypes[0].code);

    if (this.EntityPM && !this.EntityPM.Id)
      this.initDefaultValue(dateNow);
    else
      this.initValueFromEntity();
  }

  private getDateNow() {
    const dateNow = new Date();
    dateNow.setHours(0, 0, 0, 0);
    return dateNow;
  }

  private initValueFromEntity() {
    const entityDate = new Date(this.EntityPM.StartDate);
    const entityDateNextMonth = new Date(entityDate.setMonth(new Date().getMonth() + 1));
    const entityDateCloseDate = new Date(entityDate.setDate(new Date().getDate() + 45));

    this.formGroup.addControl('moveType', new FormControl('', Validators.required));
    this.formGroup.addControl('startDate', new FormControl(entityDate, Validators.required));
    this.formGroup.addControl('expirationDays', new FormControl(this.EntityPM.ExpirationDays, Validators.required));
    this.formGroup.addControl('expirationDate', new FormControl(entityDateNextMonth, Validators.required));
    this.formGroup.addControl('isAutomaticallyClosed', new FormControl(this.EntityPM.IsAutomaticallyClosed));
    this.formGroup.addControl('automaticallyCloseDays', new FormControl(this.EntityPM.AutomaticallyCloseDays));
    this.formGroup.addControl('automaticallyCloseDate', new FormControl(entityDateCloseDate));
    this.formGroup.controls.quoteType.setValue(this.EntityPM.QuoteTypeCode);

    new MoveTypePMService().get(this.EntityPM.MoveTypeId).pipe(filter(x => x.Result)).subscribe((res: any) =>
      this.formGroup.controls.moveType.setValue(res.Result));
  }

  private initDefaultValue(dateNow: Date) {
    this.formGroup.addControl('moveType', new FormControl());
    this.formGroup.addControl('startDate', new FormControl(dateNow, Validators.required));
    this.formGroup.addControl('expirationDays', new FormControl(null, Validators.required));
    this.formGroup.addControl('expirationDate', new FormControl(null, Validators.required));
    this.formGroup.addControl('isAutomaticallyClosed', new FormControl());
    this.formGroup.addControl('automaticallyCloseDays', new FormControl());
    this.formGroup.addControl('automaticallyCloseDate', new FormControl());
  }

  subscribeCtrls() {
    this.formGroup.controls.moveType.valueChanges.subscribe((newVal: MoveTypeList) => this.EntityPM.MoveTypeId = newVal?.Id)
    this.formGroup.controls.startDate.valueChanges.subscribe(newVal => this.EntityPM.StartDate = newVal)
    this.formGroup.controls.expirationDays.valueChanges.subscribe(newVal => this.expirationDaysChange(newVal))
    this.formGroup.controls.expirationDate.valueChanges.subscribe(newVal => this.expirationDateChange(newVal))
    this.formGroup.controls.isAutomaticallyClosed.valueChanges.subscribe(newVal => this.EntityPM.IsAutomaticallyClosed = newVal)
    this.formGroup.controls.automaticallyCloseDays.valueChanges.subscribe(newVal => this.closeDaysChange(newVal))
    this.formGroup.controls.automaticallyCloseDate.valueChanges.subscribe(newVal => this.automaticallyCloseDateChange(newVal))
  }

  checkType(code: string) {
    this.formGroup.controls.quoteType.setValue(code, { emitEvent: false });
    this.EntityPM.QuoteTypeCode = code;
  }

  expirationDateChange(date: Date) {
    this.EntityPM.ExpirationDate = date;

    if (!date) return;
    const expirationDays: number = this.differenceBetweenDates(date, this.formGroup.value.startDate) - 1
    this.formGroup.controls.expirationDays.setValue(expirationDays, { emitEvent: false });
  }

  automaticallyCloseDateChange(date: Date) {
    this.EntityPM.AutomaticallyCloseDate = date;
    const automaticallyCloseDays: number = this.differenceBetweenDates(date, this.formGroup.value.startDate)
    this.formGroup.controls.automaticallyCloseDays.setValue(automaticallyCloseDays, { emitEvent: false });
  }

  expirationDaysChange(days: number) {
    if (0 > days) return;

    this.EntityPM.ExpirationDays = days;
    const expDate: Date = this.addDaysToDate(days, this.formGroup.controls.startDate.value)
    this.formGroup.controls.expirationDate.setValue(expDate, { emitEvent: false });
  }

  closeDaysChange(days: number) {
    if (0 > days) return;

    this.EntityPM.AutomaticallyCloseDays = days;
    const expDate: Date = this.addDaysToDate(days, this.formGroup.controls.startDate.value)
    this.formGroup.controls.automaticallyCloseDate.setValue(expDate, { emitEvent: false });
  }

  addDaysToDate(num: number, date: Date): Date {
    if (!date) return;
    const d = new Date(date.getTime());
    d.setDate(d.getDate() + num)
    return d;
  }

  differenceBetweenDates(date1: Date, date2: Date): number {
    return Math.ceil(Math.abs(<any>date1 - <any>date2) / (1000 * 60 * 60 * 24))
  }
}