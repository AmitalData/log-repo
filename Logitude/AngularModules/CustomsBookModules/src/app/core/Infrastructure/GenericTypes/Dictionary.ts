
export interface IDictionary<TValue> {
    Add(key: string, value: TValue);
    ContainsKey(key: string): boolean;
    Count(): number;
    Item(key: string): TValue;
    Keys(): string[];
    Remove(key: string): TValue;
    Values(): TValue[];
}

export class Dictionary<TValue> implements IDictionary<TValue> {
    private items: { [key: string]: TValue } = {};
 
    private count: number = 0;
 
    public ContainsKey(key: string): boolean {
        return this.items.hasOwnProperty(key);
    }
 
    public Count(): number {
        return this.count;
    }
 
    public Add(key: string, value: TValue) {
        this.items[key] = value;
        this.count++;
    }
 
    public Remove(key: string): TValue {
        var val = this.items[key];
        delete this.items[key];
        this.count--;
        return val;
    }
 
    public Item(key: string): TValue {
        return this.items[key];
    }
 
    public Keys(): string[] {
        var keySet: string[] = [];

        for (var prop in this.items) {
            if (this.items.hasOwnProperty(prop)) {
                keySet.push(prop);
            }
        }

        return keySet;
    }
 
    public Values(): TValue[] {
        var values: TValue[] = [];

        for (var prop in this.items) {
            if (this.items.hasOwnProperty(prop)) {
                values.push(this.items[prop]);
            }
        }

        return values;
    }
}