SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[udf_ShipmentSearch]

      (@keywords nvarchar(4000))

returns table

as

  return (select * from shipments where contains(SearchFields,@keywords) )