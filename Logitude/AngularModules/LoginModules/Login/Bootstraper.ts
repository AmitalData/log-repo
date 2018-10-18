import {enableProdMode} from '@angular/core';
import {platformBrowserDynamic} from '@angular/platform-browser-dynamic';
import {LogitudeLoginModule} from './LogitudeLoginModule';
enableProdMode();
platformBrowserDynamic().bootstrapModule(LogitudeLoginModule);
