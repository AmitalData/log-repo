import { Injectable } from "@angular/core";
import { Xml2jsonService } from "Infrastructure/Services/xml2json/xml2json.service";
import { AmitalGatewayUtil, UnifreightMessageM } from "Infrastructure/Utilities/AmitalGatewayUtil";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { QuoteOPPM } from "QuoteOPM/EntityPMs/QuoteOPPM";
import { QuoteOPPropertiesPM } from "QuoteOPM/EntityPMs/QuoteOPPropertiesPM";
import { Subscription } from "rxjs";
import { PriceCheckFilters, PriceChekRootResponse, Offer, Result } from "../price-check.type";
import { xmlPriceString } from "./xmlPriceTEst";

@Injectable()
export class PriceCheckUnifreightService {

    constructor(
        private xml2Json: Xml2jsonService,
    ) { }

    async getPrices(quote: QuoteOPPM, priceCheckFilters: PriceCheckFilters): Promise<PriceChekRootResponse> {
        const logitudeEntity = 'QuoteOP';
        const logitudeViewModel = 'QuotesComponent';
        var unifreightMessageM = new UnifreightMessageM();
        unifreightMessageM.UnifreightEntity = 'GPRHEAD';
        unifreightMessageM.UnifreightEntityNumber = '-1';
        unifreightMessageM.LogitudeEntity = logitudeEntity;
        unifreightMessageM.LogitudeEntityNumber = quote.Id;
        unifreightMessageM.LogitudeViewModel = logitudeViewModel;
        unifreightMessageM.Requset = [];
        unifreightMessageM.Requset.push(["PriceCheckRequest", this.createRequestXml(quote, priceCheckFilters)]);

        const responsePromise = this.getUnifright(logitudeEntity, quote, logitudeViewModel, unifreightMessageM);

        return responsePromise;
    }

    private getUnifright(logitudeEntity: string, quote: QuoteOPPM, logitudeViewModel: string, unifreightMessageM: UnifreightMessageM) {
        const responsePromise = new Promise<PriceChekRootResponse>((resolve, reject) => {
            const sub: Subscription = AmitalGatewayUtil.Instance.UnifaceRequestArrived
                .subscribe(
                    (mess: UnifreightMessageM) => {
                        const IsMatchUnifreightCallbackCommand: boolean = (
                            mess.LogitudeEntity == logitudeEntity &&
                            mess.LogitudeEntityNumber == quote.Id &&
                            mess.LogitudeViewModel == logitudeViewModel);

                        if (IsMatchUnifreightCallbackCommand) {
                            sub.unsubscribe();
                            SessionLocator.SelectedSession.StopBusyIndicator();
                            let sBool = UnifreightMessageM.GetStringValue(mess, AmitalGatewayUtil.Instance.DeclarationMessaging.UnifreightResponseStatus);
                            if (sBool) {
                                const xmlString: string = mess.Requset.find(x => x[0] === 'Response.PriceCheckResponse')[1];
                                const prices: PriceChekRootResponse = this.xml2Json.decodeXmlStr2Json(xmlString);
                                this.fixData(prices);
                                resolve(prices as any);
                            }
                            else
                                reject(mess as any);

                        }
                    }, (err) => {
                        reject(err);
                        SessionLocator.SelectedSession.StopBusyIndicator();
                    }
                );
        });

        SessionLocator.SelectedSession.StartBusyIndicator("RunPriceCheckQuery...");

        AmitalGatewayUtil.Instance.SendRequestToUnifreightAsync(
            "QuotesComponent.SendRequestToUnifreightAsync",
            "GPRHMAIN.LogitudeTask",
            "RunPriceCheckQuery",
            unifreightMessageM,
            "Feature 148708: פניה ליוניפרייט לקבלת מחירים");
        return responsePromise;
    }

    async getPricesTest(quote: QuoteOPPM, priceCheckFilters: PriceCheckFilters): Promise<PriceChekRootResponse> {
        console.log(this.createRequestXml(quote, priceCheckFilters))
        return await new Promise<PriceChekRootResponse>((resolve) => {
            const prices: PriceChekRootResponse = this.xml2Json.decodeXmlStr2Json(xmlPriceString)
            this.fixData(prices)
            resolve(prices);
        })
    }

    private calculateProducteCode(direction: string, transport: string) {
        if (direction === 'E') {
            if (transport === 'A')
                return 'AE'
            else if (transport === 'O')
                return 'OE'
        } else if (direction === 'I') {
            if (transport === 'A')
                return 'AI'
            else if (transport === 'O')
                return 'OI'
        }
    }

    private createRequestXml(quote: QuoteOPPM, priceCheckFilters: PriceCheckFilters): string {
        let xml: string = '';

        quote.QuoteProperties.forEach((property: QuoteOPPropertiesPM, i: number) => {
            xml +=
                `
      <PriceChekRequest>
        <QuoteProperties>
          <Order>???</Order>
          <Client>${quote.CustomerId}</Client>
          <From>${property.FromPortId}</From>
          <To>${property.ToPortId}</To>
          <CarrierCode>${property.MainCarriageCarrierId}LH</CarrierCode>
          <ProductCode>${this.calculateProducteCode(quote.DirectionId, quote.TransportModeId)}</ProductCode>
          <CostTariffUseCodes>0000CR,CRE,AG</CostTariffUseCodes>
          <SaleTariffUseCodes>0000EX,EXD,AG</SaleTariffUseCodes>
          <StartDate>${this.fixDateFormat(quote.StartDate as any)}</StartDate>
          <GrossWeightAmount>${quote.GrossWeight}</GrossWeightAmount>
          <GrossWeightUOM>${quote.GrossWeightUnitCode}</GrossWeightUOM>
          <VolumeAmount>${quote.Volume}</VolumeAmount>
          <VolumeUOM>${quote.VolumeUnitCode}</VolumeUOM>
          <ChargeableWeightAmount>$${quote.ChargeableWeight}</ChargeableWeightAmount>
          <ChargeableWeightUOM>${quote.ChargeableWeightUnitCode}</ChargeableWeightUOM>
          <QuoteType>${quote.QuoteTypeCode}SpotRate</QuoteType>
          <Incoterms>${property.IncotermId}</Incoterms>
          <Currency>${quote.SaleCurrencyId}</Currency>
          <SpecialService>${property.SpecialServiceID}</SpecialService>
          <MaxOffers>3</MaxOffers>
          <Cheapest>${priceCheckFilters.Cheapest ? 'True' : 'Null'}</Cheapest>
          <Fastest>${priceCheckFilters.Quickest ? 'True' : 'Null'}</Fastest>
          <Direct>${priceCheckFilters.Direct ? 'True' : 'Null'}</Direct>
        </QuoteProperties>
      </PriceChekRequest>
    `
        })
        return xml;

    }

    private fixData(prices: PriceChekRootResponse) {
        prices.PriceChekResponse.Offers.Offer.reduce((previousValue: any[], offer: Offer) => {
            const haveMuchResult: boolean = !!(<any>offer.Result).length;
            const offers: Offer[] = [];
            if (haveMuchResult) {
                const results: Result[] = offer.Result as any;
                results.forEach(result => {
                    const copyOffer: Offer = JSON.parse(JSON.stringify(offer));
                    copyOffer.Result = result
                    offers.push(copyOffer)
                })
            }

            return previousValue.concat([offers]);
        }, []);

        prices.PriceChekResponse.Offers.Offer.forEach(x => {
            if ((<any>x.Result).length)
                x.Result = (<any>x.Result)[0];
        })
    }

    private fixDateFormat(date: string): string {
        return new Date("2021-11-18T00:00:00Z").toJSON().slice(0, 10).split('-').reverse().join('/');
    }
}
