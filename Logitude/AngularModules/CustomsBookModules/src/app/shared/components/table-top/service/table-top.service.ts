import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class TableTopService {
	categiries = ['הסכמים', 'ש.מכס', 'מס קניה', 'תמ”א', 'יח’ סטטיסטית'];

	getCategories() {
		return this.categiries;
	}

	getTableTop(state: string) {
		return state === 'Search' ? ' קטרוגיות ספר מכס' : 'תוצאות חיפוש';
	}
}
