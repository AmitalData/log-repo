import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class AmitalApiRequestsSelectionService {
    private selectedIds: Set<string> = new Set();
    
    private selectionChanged = new BehaviorSubject<number>(0);
    public selectionChanged$ = this.selectionChanged.asObservable();

    constructor() { }

    toggleSelection(id: string): void {
        if (this.selectedIds.has(id)) {
            this.selectedIds.delete(id);
        } else {
            this.selectedIds.add(id);
        }
        this.notifyChange();
    }

    isSelected(id: string): boolean {
        return this.selectedIds.has(id);
    }

    getSelectedIds(): string[] {
        return Array.from(this.selectedIds);
    }

    hasSelection(): boolean {
        return this.selectedIds.size > 0;
    }

    clearAll(): void {
        this.selectedIds.clear();
        this.notifyChange();
    }

    private notifyChange(): void {
        this.selectionChanged.next(this.selectedIds.size);
    }
}