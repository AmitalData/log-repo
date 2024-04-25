import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppHeaderComponent } from '../shared/components/app-header/app-header.component';
import { AppFooterComponent } from '../shared/components/app-footer/app-footer.component';
import { MainPageComponent } from '../features/main-page/main-page.component';
import { NgFor, NgForOf } from '@angular/common';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [NgFor, NgForOf, RouterOutlet, AppHeaderComponent, AppFooterComponent, MainPageComponent, CommonModule],
	templateUrl: './app.component.html',
	styleUrl: './app.component.css',
})
export class AppComponent {}
