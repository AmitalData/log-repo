import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, Event, RoutesRecognized } from '@angular/router';
import { fromEvent } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, tap, map } from 'rxjs/operators';
import { FormBuilder } from '@angular/forms';


@Component({
    selector: 'dashboard-shipments',
    templateUrl: './dashboard-shipments.component.html',
    styleUrls: ['./dashboard-shipments.component.css']
})
export class DashboardShipmentsComponent 
{

   

    constructor(private router: Router)
    {
        // this.GetVariablesFromURI(); favorites s
        // this.listenToRouterEvents();
       
    }

 
}
