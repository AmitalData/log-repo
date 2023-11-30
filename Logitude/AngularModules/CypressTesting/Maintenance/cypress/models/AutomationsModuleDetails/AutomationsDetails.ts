import { ConditionsDetails } from "../../models/AutomationsModuleDetails/ConditionsDetails"

export interface AutomationsDetails {
 Name: string     
 Description: string           
 conditionsDetails1:ConditionsDetails
 conditionsDetails2:ConditionsDetails
}