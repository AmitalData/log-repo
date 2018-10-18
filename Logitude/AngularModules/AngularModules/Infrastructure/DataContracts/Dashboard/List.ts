export class List<T> {
    public items: Array<T>;
    constructor() {
        this.items = [];
    }

    size(): number {
        return this.items.length;
    }

    add(value: T): void {
        this.items.push(value);
    }

    get(index: number): T {
        return this.items[index];
    }

    set(index: number, value: T): void {
        this.items[index] = value;

    }

    public getAll(): Array<T> {
        return this.items;
    }

    Assign(arr: Array<T>) {
        this.items = arr;

    }

    Clear(): void {
        this.items = [];

    }
}