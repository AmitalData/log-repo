import { ElementRef, Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class NewQuoteDataShareService {
  EntityPM: QuoteOPPM = null as any;
  newQuoteRef: ElementRef = null as any;
  formGroup: FormGroup = null as any;

  constructor() { }

  indexPropertyTab: BehaviorSubject<number> = new BehaviorSubject<number>(0);

  partnersHidden: PartnerHidden = {
    shipper: new BehaviorSubject<boolean>(true),
    consignee: new BehaviorSubject<boolean>(true),
  }
}

export type PartnerHidden = {
  shipper: BehaviorSubject<boolean>;
  consignee: BehaviorSubject<boolean>;
}