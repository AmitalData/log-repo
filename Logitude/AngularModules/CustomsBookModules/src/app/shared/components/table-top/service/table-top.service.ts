import { Injectable } from '@angular/core';
import { TableTopState } from '../table-top.component';

@Injectable({
	providedIn: 'root',
})
export class TableTopService {
	categiries = ['הסכמים', 'ש.מכס','יח’ סטטיסטית', 'מס קניה', 'תמ”א'];

	getCategories() {
		return this.categiries;
	}

	getTableTop(state: string) {
		return state == TableTopState.Search ? 'תוצאות חיפוש' : ' קטרוגיות ספר מכס';
	}
}
