import { enableProdMode } from '@angular/core';
import { platformBrowser } from '@angular/platform-browser';
import { LogitudeLoginModuleAOTNgFactory } from '../aot/Login/LogitudeLoginModuleAOT.ngfactory';
enableProdMode();
platformBrowser().bootstrapModuleFactory(LogitudeLoginModuleAOTNgFactory);
//# sourceMappingURL=BootstraperAOT.js.map