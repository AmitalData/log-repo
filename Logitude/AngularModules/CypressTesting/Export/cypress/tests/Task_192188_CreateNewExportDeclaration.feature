@smoke
Feature: Create New Export Declaration
    The user creates new Declaration, Fill all the Data and Save it

    Scenario: Create new Declaration 
        Given the user logged in and navigates to Export workspace
        And open new declaration 
            | Dairection        | Export   |
            | ExportFileNumber  | 565652   |
            | Customer          | בדיקות אוטומטיות - לא לגעת |
        
        When approve the new declaration
        Then the declaration should save successfully

    Scenario: Add details to declaration
        Given Declaration with the following details

            | DeclarationOfficeHandlCode  | בית מכס נתב   |
            | ExportDeclarationOfficeCode | בית מכס נתב   |
            | ExporterNumber              | 510182462      |
            | DeclarationTypeCode         | הצהרת יצוא     |
            | ProcedureCurrentCode        |  יצוא מסחרי    | 
            | ClientSearch                | 510273394       |
            | DeclarationDocumentTypeCode | הצהרת יצוא     |
            | DeclarationDocumentId       | 23042891149326  |
           
            | DestinationCountryCode|  אוסטריה|
            | AutonomyRegionTypeCode | רש"   |
            | RecipientName          | AVI |    
            | RecipientAddress       | DAVI |
            | RecipientIssueCountryCode | אוסטריה |
            | TransportType | יצוא  |
            | CargoType |שטר מטען אווירי יצוא |
            | FirstCargoID           | 2023   |
            | SecondCargoID          | 00176096 |
            | ThirdCargoID           | 114 |
            | FinalDestinationPortCode | ADORD Ordino |
            | LoadingPortCode        |  נתב |
            | UnloadingPortCode      | ADORD Ordino |
            | CargoDescription       | בדיקות אוטומציה, לא לגעת, שריה/אורית שלום |
            | StorageSite            | נתב |
            | RecieverWareHouse      | נתב |
            | InternalTransition     |  מעבר פנימי - שער אפרים |
            
        When save Declaration
        Then the Declaration save successfully

   