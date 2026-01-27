import { Injectable } from "@angular/core";
import { AmendmentMessageResponse, DeclarationWebService } from "Customs/Services/WebServices/DeclarationWebService";

@Injectable()
export class AmendmentMessageCacheService {

  private ttlMs: number = 60 * 1000;
  private cache: any = {};
  private declarationWebService: DeclarationWebService;

  constructor(declarationWebService?: DeclarationWebService) {
    this.declarationWebService = declarationWebService || new DeclarationWebService();
  }

  static createInstance() {
    return new AmendmentMessageCacheService(new DeclarationWebService());
  }

  public SetTtlMs(ttlMs: number) {
    this.ttlMs = ttlMs;
  }

  public Clear(declarationId: string) {
    if (!declarationId) return;
    var key = this.buildKey(declarationId);
    if (key) delete this.cache[key];
  }

  public ClearAll() {
    this.cache = {};
  }

  public async Get(declarationId: string): Promise<AmendmentMessageResponse> {

    if (!declarationId) {
      return this.empty();
    }

    var key = this.buildKey(declarationId);
    var now = new Date().getTime();

    var entry = this.cache[key];

    if (entry && entry.value) {

      if (entry.exp > now) {
        return entry.value;
      }

      if (!entry.inFlight) {
        entry.inFlight = true;

        try {
          const res = await this.declarationWebService.FetchAmendmentMessage(declarationId);
          this.cache[key] = {
            value: res || this.empty(),
            exp: new Date().getTime() + this.ttlMs,
            inFlight: false
          };
          return this.cache[key].value;
        } catch {
          entry.inFlight = false;
          entry.exp = new Date().getTime() + 10 * 1000;
          return entry.value;
        }
      }

      return entry.value;
    }

    var res = await this.declarationWebService.FetchAmendmentMessage(declarationId);

    this.cache[key] = {
      value: res || this.empty(),
      exp: now + this.ttlMs,
      inFlight: false
    };

    return this.cache[key].value;
  }

  private refresh(declarationId: string, key: string): void {

    this.declarationWebService.FetchAmendmentMessage(declarationId)
      .then(res => {
        var now = new Date().getTime();
        this.cache[key] = {
          value: res || this.empty(),
          exp: now + this.ttlMs,
          inFlight: false
        };
      })
      .catch(() => {
        if (this.cache[key]) {
          this.cache[key].inFlight = false;
          this.cache[key].exp = new Date().getTime() + 10 * 1000;
        }
      });
  }

  private buildKey(declarationId: string): string {
    if (!declarationId) return null;
    return declarationId;
  }

  private empty(): AmendmentMessageResponse {
    var e = new AmendmentMessageResponse();
    e.AmendmentMessage = null;
    e.IsAmendmentDisplayOnly = false;
    return e;
  }
}
