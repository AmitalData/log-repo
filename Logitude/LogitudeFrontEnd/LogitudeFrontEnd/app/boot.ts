import {bootstrap}        from 'angular2/platform/browser';
import {ELEMENT_PROBE_PROVIDERS} from 'angular2/platform/common_dom';
import {ROUTER_PROVIDERS} from 'angular2/router';
import {Http, HTTP_PROVIDERS} from 'angular2/http';
import {RootComponent} from './root.component';
//import {AppComponent}     from './app.component';
//import {GeneralService}   from './general/general.service';
import {ShipmentsService} from './shipment/services/shipment-service/shipments.service';

import {provide, Provider}           from 'angular2/core';
import {LocationStrategy,
HashLocationStrategy} from 'angular2/router';
import {NG_VALIDATORS, Validator} from 'angular2/common';
import * as core from 'angular2/core';
declare var ag: any;
ag.grid.initialiseAgGridWithAngular2({ core: core });

import {ModalConfig} from './infrastructure/modal-libs/angular2-modal/models/ModalConfig';

bootstrap(RootComponent, [
    [ROUTER_PROVIDERS, HTTP_PROVIDERS, /*GeneralService,*/ ShipmentsService, /*ModalConfig,*/ 
        provide(LocationStrategy, { useClass: HashLocationStrategy }/*, ModalConfig, { useValue: new ModalConfig('lg', true, 81) }*/),
        new Provider(NG_VALIDATORS, { useValue: Validator, multi: true }),
        provide(ModalConfig, { useValue: new ModalConfig('lg', true, 81) }),
        ELEMENT_PROBE_PROVIDERS
    ],
    //DialogService
]);
