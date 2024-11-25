import { Injectable } from '@angular/core';

enum ChacheStatus { WAITING, READY }

@Injectable({
  providedIn: 'root'
})
export class CacheService {
  private cache: { [key: string]: { value: any, status?: ChacheStatus, waitingPromise?: Promise<any> } } = {};

  constructor() { }

  public async getByPromise(key: string, func: () => Promise<any>): Promise<any> {
    if (this.cache[key]?.status === ChacheStatus.READY)
      return new Promise<any>(resolve => resolve(this.cache[key].value));
    else if (this.cache[key]?.status === ChacheStatus.WAITING) {
      const value = await this.cache[key].waitingPromise;
      return new Promise<any>(resolve => resolve(value));
    }
    const promise = func();
    this.cache[key] = { value: null, status: ChacheStatus.WAITING, waitingPromise: promise };
    this.cache[key].value = await promise;

    return this.cache[key].value;
  }
}
