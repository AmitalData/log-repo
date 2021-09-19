import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';

@Injectable({providedIn: 'root'})
export class NewQuoteAutocomplateService {

  constructor() { }

  keyUp(ctrl: AbstractControl, list: string[]) {
    if (!list.some(x => x.includes(ctrl.value)))
      ctrl.setValue(ctrl.value.slice(0, -1))
  }

}
