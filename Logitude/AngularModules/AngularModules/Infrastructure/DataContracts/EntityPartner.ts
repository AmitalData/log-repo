
export class EntityPartner {
  PartnerType: string;
  PartnerId: string;
  IsUser: boolean;
  PartnerContactName: string;
  PartnerContactMail: string;
  PartnerTypeCode: string;
  constructor(partnerType: string, partnerId: string, isUser: boolean) {
    this.PartnerType = partnerType;
    this.PartnerId = partnerId;

    this.IsUser = isUser;
    if (this.PartnerType) {
      this.PartnerTypeCode = this.PartnerType.replace(" ", "").toUpperCase();
    }
  }
}




