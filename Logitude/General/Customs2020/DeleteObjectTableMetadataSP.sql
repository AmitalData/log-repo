create or replace PROCEDURE usp_DeleteObjectTableMetadata(
    v_pTableName IN VARCHAR2 )
AS
  v_ObjectTableId VARCHAR2(15);
BEGIN
  BEGIN
  
  BEGIN
    SELECT Id
    INTO v_ObjectTableId
    FROM ObjectTables
    WHERE NAME  = v_pTableName
    AND ROWNUM <= 1;
    EXCEPTION
    WHEN NO_DATA_FOUND THEN
    v_ObjectTableId := NULL;
    END;
    --*--Delete--*--
    --ObjectFields
    DELETE objectfields
    WHERE tenant      = 0
    AND ObjectTableId = v_ObjectTableId;
    DELETE querycolumns
    WHERE tenant   = 0
    AND userid    IS NULL
    AND QueryCode IN
      ( SELECT UniqueCode FROM Queries WHERE ObjectTableId = v_ObjectTableId
      ) ;
    DELETE AdvancedQueryFilters
    WHERE tenant   = 0
    AND userid    IS NULL
    AND QueryCode IN
      ( SELECT UniqueCode FROM Queries WHERE ObjectTableId = v_ObjectTableId
      ) ;
    DELETE RuleConditionFields
    WHERE tenant               = 0
    AND ObjectTableRuleId NOT IN
      ( SELECT id FROM ObjectTableRules WHERE SystemLevel = 0
      )
    AND ObjectTableRuleId IN
      ( SELECT id FROM ObjectTableRules WHERE ObjectTableId = v_ObjectTableId
      ) ;
    DELETE ObjectTableRuleFields
    WHERE tenant           = 0
    AND systemlevel        = 1
    AND ObjectTableRuleId IN
      ( SELECT id FROM ObjectTableRules WHERE ObjectTableId = v_ObjectTableId
      ) ;
    DELETE ObjectTableRules
    WHERE tenant    = 0
    AND systemlevel = 1
    AND Id         IN
      ( SELECT id FROM ObjectTableRules WHERE ObjectTableId = v_ObjectTableId
      ) ;
    --Screens
    DELETE ScreenFields
    WHERE tenant    = 0
    AND ScreenCode IN
      ( SELECT code FROM Screens WHERE ObjectTableId = v_ObjectTableId
      ) ;
    DELETE screens WHERE tenant = 0 AND ObjectTableId = v_ObjectTableId;
    --Queries
    DELETE Queries
    WHERE tenant      = 0
    AND userid       IS NULL
    AND systemlevel   = 1
    AND ObjectTableId = v_ObjectTableId;
    --TextCodes
    DELETE MenuButtons
    WHERE tenant           = 0
    AND MenuButtonGroupId IN
      ( SELECT id FROM MenuButtonGroups WHERE ObjectTableId = v_ObjectTableId
      ) ;
    DELETE ObjectTableTabs WHERE tenant = 0 AND ObjectTableId = v_ObjectTableId;
    DELETE textcodes
    WHERE tenant  = 0
    AND code NOT IN
      (SELECT NameTextCodeCode
      FROM queries
      WHERE tenant          = 0
      AND userid           IS NOT NULL
      AND SystemLevel       = 0
      AND NameTextCodeCode IS NOT NULL
      )
    AND code NOT IN
      ( SELECT ShortTextCodeCode FROM tips
      )
    AND ObjectTableId = v_ObjectTableId;
    --Features
    DELETE Features
    WHERE tenant      = 0
    AND ObjectTableId = v_ObjectTableId;-----------
  END;
END;