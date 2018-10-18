UPDATE       ObjectTableRules
SET                Condition = 'If(And([DirectionId]  =  "E",[TransportModeId]  =  "A",[Master] <> "",Or([ShipmentLevelCode]= "C",[ShipmentLevelCode]= "D")),True,False)'
WHERE        (rulecode = 'MAWB')

