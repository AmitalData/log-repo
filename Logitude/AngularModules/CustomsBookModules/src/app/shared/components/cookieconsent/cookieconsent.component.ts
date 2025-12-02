import { Component } from '@angular/core';
import * as CookieConsent from 'vanilla-cookieconsent';
import cookieTexts from './cookie-texts.json';
@Component({
  selector: 'cookieconsent',
  standalone: true,
  imports: [],
  template: '',
  styleUrls: []
})

export class CookieconsentComponent {

  ngOnInit(): void {
    document.documentElement.classList.add('cc--light-funky'); 
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
        translations: { en: cookieTexts },
      },

    });
    setTimeout(() => {
      CookieConsent.show();
    }, 3000);
  }

}

