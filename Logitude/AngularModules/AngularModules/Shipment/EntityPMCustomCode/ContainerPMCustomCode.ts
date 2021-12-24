import { AddressList } from '../../Common/EntityLists/AddressList';
import { CardList } from '../../Common/EntityLists/CardList';
import { ContactList } from '../../Common/EntityLists/ContactList';
import { AddressListService } from '../../Common/Services/StandardLists/AddressListService';
import { CardListService } from '../../Common/Services/StandardLists/CardListService';
import { ContactListService } from '../../Common/Services/StandardLists/ContactListService';
import { ServiceResponse } from '../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../Infrastructure/Tools';
import { ContainerPM } from '../EntityPMs/ContainerPM';
import { VesselPMService } from '../../Common/Services/StandardPMs/VesselPMService';

export class ContainerPMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: ContainerPM) {
        if (propertyName == "TerminalId" && entityPM.TerminalId) {
            var cardService =  new CardListService();
            cardService.getSingle(entityPM.TerminalId).subscribe((myResponse: ServiceResponse) => {
                if (myResponse != null) {
                    if (!myResponse.HasError) {
                        var partnerCardList: CardList = myResponse.Result;
                        if (partnerCardList != null) {
                            entityPM.TerminalAddressId = partnerCardList.MainAddressId;
                            this.GetTerminalAddress(partnerCardList.MainAddressId, entityPM);
                            this.GetTerminalPhone(entityPM, partnerCardList);
                        }
                    }
                }
            });
        }

        if (propertyName == "TerminalAddressId" && entityPM.TerminalAddressId) {
            this.GetTerminalAddress(entityPM.TerminalAddressId, entityPM);
        }

        this.ManageContainerVesselLegs(propertyName, entityPM);
    }
   
    private static GetTerminalAddress(terminalAddressId: string , entityPM: ContainerPM) {
        var addressService = new AddressListService();
        if (terminalAddressId != null)
            addressService.getSingle(terminalAddressId).subscribe((response: any) => {
                if (response.Result) {
                    var addressList: AddressList = response.Result;
                    entityPM.TerminalAddress = [addressList.Address1,
                    addressList.Address2,
                        addressList.City,
                        addressList.CountryName].filter(s => !AppTool.IsNullOrEmpty(s)).join(" ,")
                     }
                }
            );
    }

    private static GetTerminalPhone(entityPM: ContainerPM, partnerCardList: CardList) {
        var contactService = new ContactListService();
        if (partnerCardList.BusinessPhone) {
            entityPM.TerminalPhone = partnerCardList.BusinessPhone
        } else {
            if (partnerCardList.ContactId != null)
                contactService.getSingle(partnerCardList.ContactId).subscribe((response: any) => {
                    if (response.Result) {
                        var contactList: ContactList = response.Result;
                        entityPM.TerminalPhone = contactList.BusinessPhone;
                    }
                });
        }
 
    }
    static ManageContainerVesselLegs(propertyName: string, entityPM: ContainerPM) {
        for (let vesselLegIndex = 1; vesselLegIndex <= 5; vesselLegIndex++) {
            this.HandleVesselLegName(propertyName, vesselLegIndex, entityPM);
        }
    }
    static HandleVesselLegName(propertyName: string, index: number, entityPM: ContainerPM) {
        var legId = "Leg" + index + "VesselId";
        var legName = "Leg" + index + "Vessel";
        if (propertyName == legId && entityPM[legId]) {
            var vesselPMService = new VesselPMService();
            vesselPMService.get(entityPM[legId]).subscribe((response: any) => {
                if (response.Result) {
                    var vessel = response.Result;
                    entityPM[legName] = vessel.EnglishName;
                }
            });
        }
    }
}
