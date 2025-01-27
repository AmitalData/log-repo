import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { SearchCustomsItemAutocomplateComponent } from '../page-top/search-customs-item-autocomplate/search-customs-item-autocomplate.component';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-preference-menu',
  standalone: true,
  imports: [FormsModule, MatAutocompleteModule, SearchCustomsItemAutocomplateComponent, NgFor],
  templateUrl: './preference-menu.html',
  styleUrl: './preference-menu.css',
})
export class PreferenceMenuComponent {
  preferenceTypeBackground: PreferenceType = PreferenceType.Background;
  preferenceTypeText: PreferenceType = PreferenceType.Text;

  levels = [1, 2, 3, 4, 5, 6, 7];
  selectedType: PreferenceType = PreferenceType.Background;
  preferences = {
    [PreferenceType.Background]: Array(7).fill('#ffffff'),
    [PreferenceType.Text]: Array(7).fill('#000000')
  };

  constructor(private preferencesService: PreferencesService) {
    this.levels.forEach(level => {
      this.preferences[PreferenceType.Background][level - 1] = this.preferencesService.getPreference(level, PreferenceType.Background);
      this.preferences[PreferenceType.Text][level - 1] = this.preferencesService.getPreference(level, PreferenceType.Text);
    });
  }

  updatePreferences(level: number) {
    this.preferencesService.setPreference(level, this.preferences[this.selectedType][level - 1], this.selectedType);
  }
}

import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root'
})


export class PreferencesService {
	private preferences = {
		[PreferenceType.Background]: Array(7).fill('#F3F5F7'), // default colors
		[PreferenceType.Text]: Array(7).fill('#1C1C1C') // default colors
	};

	setPreference(level: number, color: string, type: PreferenceType) {
		this.preferences[type][level - 1] = color; // level is 1-based, array is 0-based
	}

	getPreference(level: number, type: PreferenceType): string {
		return this.preferences[type][level - 1] || (type === PreferenceType.Background ? '#F3F5F7' : '#1C1C1C');
	}
}


export enum PreferenceType {
  Background = 'background',
  Text = 'text'
}