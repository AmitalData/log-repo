import { Component } from '@angular/core';
import * as CookieConsent from 'vanilla-cookieconsent';
@Component({
  selector: 'cookieconsent',
  template: '',
  styleUrls: []
})

export class CookieconsentComponent {
    
  cookieTexts: any;

  ngOnInit(): void { 
    document.documentElement.classList.add('cc--light-funky');
    this.cookieTexts = require('./cookie-texts.json');
  }

  ngAfterViewInit(): void {
    CookieConsent.run({

      autoShow: false, 
      disablePageInteraction: false,
      mode: 'opt-in',
      revision: 1,

      categories: {
        necessary: {
          enabled: true, 
          readOnly: true,
        },
        preferences: {
          enabled: false,
        },
      },
      language: {
        default: 'en',
        translations: { en: this.cookieTexts },
      },

    });
    setTimeout(() => {
      CookieConsent.show();
    }, 3000);
  }

}

