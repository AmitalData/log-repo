

update TextCodes set DefaultText = 'Inside Shipment Package', DefaultTextPlural = 'Inside Shipment Packages' where Code = 'InsideShipmentPackage'
update TextCodes set DefaultText = 'Shipment Receivable', DefaultTextPlural = 'Shipment Receivables' where Code = 'ShipmentReceivable'
update TextCodes set DefaultText = 'Charges Type', DefaultTextPlural = 'Charges Types' where Code = 'ChargesType'
update TextCodes set DefaultText = 'Custom Agent', DefaultTextPlural = 'Custom Agents' where Code = 'CustomAgent'
update TextCodes set DefaultText = 'Shipment Package', DefaultTextPlural = 'Shipment Packages' where Code = 'ShipmentPackage'
update TextCodes set DefaultText = 'Global Zone', DefaultTextPlural = 'Global Zones' where Code = 'GlobalZone'
update TextCodes set DefaultText = 'Payment Term', DefaultTextPlural = 'Payment Terms' where Code = 'PaymentTerm'
update TextCodes set DefaultText = 'Shipping Agent', DefaultTextPlural = 'Shipping Agents' where Code = 'ShippingAgent'
update TextCodes set DefaultText = 'Shipping Line', DefaultTextPlural = 'Shipping Lines' where Code = 'ShippingLine'
update TextCodes set DefaultText = 'Package Type', DefaultTextPlural = 'Package Types' where Code = 'PackageType'
update TextCodes set DefaultText = 'Document Type', DefaultTextPlural = 'Document Types' where Code = 'DocumentType'
update TextCodes set DefaultText = 'Event Type', DefaultTextPlural = 'Event Types' where Code = 'EventType'


select Code, DefaultText, DefaultTextPlural, IsSpellChecked from TextCodes where TextCodeTypeCode = 'T' and IsSpellChecked = 1
go