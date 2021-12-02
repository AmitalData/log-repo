import { ChangeDetectorRef, ElementRef, Injectable } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { NewQuoteDataShareService } from '../new-quote-data-share/new-quote-data-share.service';

@Injectable()
export class NewQuoteFocusErrorService {
  newQuoteRef: ElementRef = null as any;

  constructor(
    private dataShareService: NewQuoteDataShareService,
  ) { }

  focusError(form: FormGroup, elmRef: ElementRef) {
    this.newQuoteRef = elmRef
    const controlList: FormGroup["controls"] = form.controls;

    const partnerInvalid: string = ['shipper', 'consignee'].find((ctrlName: string) => controlList[ctrlName].invalid);
    if (partnerInvalid) {
      if((<FormGroup>controlList[partnerInvalid]).controls.partner.valid)
        this.dataShareService.partnersHidden[partnerInvalid].next(false);

      const invalidControl: HTMLElement = this.newQuoteRef.nativeElement.querySelector('app-new-quote-partner')
      return this.scrollToHtmlElm(invalidControl);
    }

    const i = (controlList.properties as FormArray).controls.findIndex((control) => control.invalid);
    if (i > - 1) {
      this.dataShareService.indexPropertyTab.next(i)
      const propertyCtrls: FormGroup["controls"] = ((controlList.properties as FormArray).at(0) as FormGroup).controls;

      const ctrlName = Object.keys(propertyCtrls).find((ctrlName: string) => propertyCtrls[ctrlName].invalid);
      setTimeout(() => 
        this.scrollToHtmlElm(this.findControl(ctrlName)), 10);
      return 
    }

    const ctrlName = Object.keys(controlList).find(ctrl => controlList[ctrl] instanceof FormControl && controlList[ctrl].invalid)
    if (!ctrlName)
      return this.scrollToHtmlElm(this.findControl(ctrlName));
  }

  private findControl(ctrlName: string): HTMLElement {
    return this.newQuoteRef.nativeElement.querySelector('[formcontrolname="' + ctrlName + '"], [ng-reflect-name="' + ctrlName + '"]');
  }

  private scrollToHtmlElm(htmlElm: HTMLElement) {
    htmlElm.focus();
    htmlElm.scrollIntoView({ behavior: "smooth", block: "center", inline: "center" });
  }
}
