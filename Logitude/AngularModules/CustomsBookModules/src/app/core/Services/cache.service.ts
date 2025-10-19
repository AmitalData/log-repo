import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CacheService {
  private cache = new Map<string, any>();

  constructor() {}

  get(key: string): any {
    return this.cache.get(key);
  }

  set(key: string, data: any): void {
    this.cache.set(key, data);
  }

  getOrSet<T>(key: string, apiCall: any, ttl?: number): any {
    const cachedData = this.get(key);
    
    if (cachedData !== null && cachedData !== undefined) {
      return cachedData;
    }

    return apiCall;
  }

  async getByPromise(key: string, apiCall: () => Promise<any>, ttl?: number): Promise<any> {
    const cachedData = this.get(key);
    
    if (cachedData !== null && cachedData !== undefined) {
      return cachedData;
    }

    try {
      const result = await apiCall();
      this.set(key, result);
      return result;
    } catch (error) {
      console.error(`Cache error for key ${key}:`, error);
      throw error;
    }
  }

  delete(key: string): boolean {
    return this.cache.delete(key);
  }

  clear(): void {
    this.cache.clear();
  }

  clearPattern(pattern: string): void {
    const regex = new RegExp(pattern);
    const keysToDelete: string[] = [];

    for (const key of this.cache.keys()) {
      if (regex.test(key)) {
        keysToDelete.push(key);
      }
    }

    keysToDelete.forEach(key => this.cache.delete(key));
  }

  generateKey(prefix: string, params: Record<string, any>): string {
    const sortedParams = Object.keys(params)
      .sort()
      .map(key => `${key}:${params[key]}`)
      .join('|');
    
    return `${prefix}:${sortedParams}`;
  }

  getStats(): any {
    return {
      size: this.cache.size,
      keys: Array.from(this.cache.keys())
    };
  }
}
