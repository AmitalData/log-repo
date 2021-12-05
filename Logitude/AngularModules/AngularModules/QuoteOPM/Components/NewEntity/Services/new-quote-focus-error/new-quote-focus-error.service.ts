import { ChangeDetectorRef, ElementRef, Injectable, isDevMode } from '@angular/core';
import { FormArray, FormControl, FormGroup } from '@angular/forms';
import { NewQuoteDataShareService } from '../new-quote-data-share/new-quote-data-share.service';

@Injectable()
export class NewQuoteFocusErrorService {
  newQuoteRef: ElementRef = null as any;
  controlList: FormGroup["controls"] = null as any;

  constructor(
    private dataShareService: NewQuoteDataShareService,
  ) { }

  focusError(form: FormGroup, elmRef: ElementRef) {
    this.newQuoteRef = elmRef
    this.controlList = form.controls;

    this.focusPartnerError()
    this.focusPropertiesError()
    this.focusAllError()
  }

  private focusPartnerError() {
    const partnerInvalid: string = ['shipper', 'consignee'].find((ctrlName: string) => this.controlList[ctrlName].invalid);
    if (partnerInvalid) {
      if ((<FormGroup>this.controlList[partnerInvalid]).controls.partner.valid)
        this.dataShareService.partnersHidden[partnerInvalid].next(false);

      const invalidControl: HTMLElement = this.newQuoteRef.nativeElement.querySelector('app-new-quote-partner')
      return this.scrollToHtmlElm(invalidControl);
    }
  }

  private focusPropertiesError() {
    const i = (this.controlList.properties as FormArray).controls.findIndex((control) => control.invalid);
    if (i > - 1) {
      this.dataShareService.indexPropertyTab.next(i)
      const propertyCtrls: FormGroup["controls"] = ((this.controlList.properties as FormArray).at(0) as FormGroup).controls;

      const ctrlName = Object.keys(propertyCtrls).find((ctrlName: string) => propertyCtrls[ctrlName].invalid);
      setTimeout(() =>
        this.scrollToHtmlElm(this.findControl(ctrlName)), 10);
      return
    }
  }

  private focusAllError() {
    const ctrlName = Object.keys(this.controlList).find(ctrl => this.controlList[ctrl] instanceof FormControl && this.controlList[ctrl].invalid)
    if (!ctrlName)
      return this.scrollToHtmlElm(this.findControl(ctrlName));
  }

  private findControl(ctrlName: string): HTMLElement {
    return this.newQuoteRef.nativeElement.querySelector(`#${ctrlName}, [ng-reflect-name="${ctrlName}"]`);
  }

  private scrollToHtmlElm(htmlElm: HTMLElement) {
    htmlElm.focus();
    htmlElm.scrollIntoView({ behavior: "smooth", block: "center", inline: "center" });
  }
}
