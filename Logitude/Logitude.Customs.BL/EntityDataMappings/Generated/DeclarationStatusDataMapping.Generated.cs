
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;  
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class DeclarationStatusDataMapping: IMapping<DeclarationStatusPM, DeclarationStatus>,IMappingEncodeBase64NVARCHARFields<DeclarationStatusPM>
   {
          public enum POCOPropertyNames
          { 
		     None,  
	         Tenant, 
	         SearchFields, 
	         DeclarationId, 
	         FieldC1, 
	         FieldC2, 
	         FieldC3, 
	         FieldC4, 
	         FieldC5, 
	         FieldC6, 
	         FieldC7, 
	         FieldC8, 
	         FieldC9, 
	         FieldC10, 
	         FieldC11, 
	         FieldC12, 
	         FieldC13, 
	         FieldC14, 
	         FieldC15, 
	         FieldC16, 
	         FieldC17, 
	         FieldC18, 
	         FieldC19, 
	         FieldC20, 
	         FieldC21, 
	         FieldC22, 
	         FieldC23, 
	         FieldC24, 
	         FieldC25, 
	         FieldC26, 
	         FieldC27, 
	         FieldC28, 
	         FieldC29, 
	         FieldC30, 
	         FieldC31, 
	         FieldC32, 
	         FieldC33, 
	         FieldC34, 
	         FieldC35, 
	         FieldC36, 
	         FieldC37, 
	         FieldC38, 
	         FieldC39, 
	         FieldC40, 
	         FieldC41, 
	         FieldC42, 
	         FieldC43, 
	         FieldC44, 
	         FieldC45, 
	         FieldC46, 
	         FieldC47, 
	         FieldC48, 
	         FieldC49, 
	         FieldC50, 
	         FieldD1, 
	         FieldD2, 
	         FieldD3, 
	         FieldD4, 
	         FieldD5, 
	         FieldD6, 
	         FieldD7, 
	         FieldD8, 
	         FieldD9, 
	         FieldD10, 
	         FieldD11, 
	         FieldD12, 
	         FieldD13, 
	         FieldD14, 
	         FieldD15, 
	         FieldD16, 
	         FieldD17, 
	         FieldD18, 
	         FieldD19, 
	         FieldD20, 
	         FieldD21, 
	         FieldD22, 
	         FieldD23, 
	         FieldD24, 
	         FieldD25, 
	         FieldD26, 
	         FieldD27, 
	         FieldD28, 
	         FieldD29, 
	         FieldD30, 
	         FieldD31, 
	         FieldD32, 
	         FieldD33, 
	         FieldD34, 
	         FieldD35, 
	         FieldD36, 
	         FieldD37, 
	         FieldD38, 
	         FieldD39, 
	         FieldD40, 
	         FieldD41, 
	         FieldD42, 
	         FieldD43, 
	         FieldD44, 
	         FieldD45, 
	         FieldD46, 
	         FieldD47, 
	         FieldD48, 
	         FieldD49, 
	         FieldD50, 
	         FieldR1, 
	         FieldR2, 
	         FieldR3, 
	         FieldR4, 
	         FieldR5, 
	         FieldR6, 
	         FieldR7, 
	         FieldR8, 
	         FieldR9, 
	         FieldR10, 
	         FieldR11, 
	         FieldR12, 
	         FieldR13, 
	         FieldR14, 
	         FieldR15, 
	         FieldR16, 
	         FieldR17, 
	         FieldR18, 
	         FieldR19, 
	         FieldR20,
	      }


	      public enum PMPropertyNames
          { 
		     None,  
	         Tenant, 
	         SearchFields, 
	         DeclarationId, 
	         FieldC1, 
	         FieldC2, 
	         FieldC3, 
	         FieldC4, 
	         FieldC5, 
	         FieldC6, 
	         FieldC7, 
	         FieldC8, 
	         FieldC9, 
	         FieldC10, 
	         FieldC11, 
	         FieldC12, 
	         FieldC13, 
	         FieldC14, 
	         FieldC15, 
	         FieldC16, 
	         FieldC17, 
	         FieldC18, 
	         FieldC19, 
	         FieldC20, 
	         FieldC21, 
	         FieldC22, 
	         FieldC23, 
	         FieldC24, 
	         FieldC25, 
	         FieldC26, 
	         FieldC27, 
	         FieldC28, 
	         FieldC29, 
	         FieldC30, 
	         FieldC31, 
	         FieldC32, 
	         FieldC33, 
	         FieldC34, 
	         FieldC35, 
	         FieldC36, 
	         FieldC37, 
	         FieldC38, 
	         FieldC39, 
	         FieldC40, 
	         FieldC41, 
	         FieldC42, 
	         FieldC43, 
	         FieldC44, 
	         FieldC45, 
	         FieldC46, 
	         FieldC47, 
	         FieldC48, 
	         FieldC49, 
	         FieldC50, 
	         FieldD1, 
	         FieldD2, 
	         FieldD3, 
	         FieldD4, 
	         FieldD5, 
	         FieldD6, 
	         FieldD7, 
	         FieldD8, 
	         FieldD9, 
	         FieldD10, 
	         FieldD11, 
	         FieldD12, 
	         FieldD13, 
	         FieldD14, 
	         FieldD15, 
	         FieldD16, 
	         FieldD17, 
	         FieldD18, 
	         FieldD19, 
	         FieldD20, 
	         FieldD21, 
	         FieldD22, 
	         FieldD23, 
	         FieldD24, 
	         FieldD25, 
	         FieldD26, 
	         FieldD27, 
	         FieldD28, 
	         FieldD29, 
	         FieldD30, 
	         FieldD31, 
	         FieldD32, 
	         FieldD33, 
	         FieldD34, 
	         FieldD35, 
	         FieldD36, 
	         FieldD37, 
	         FieldD38, 
	         FieldD39, 
	         FieldD40, 
	         FieldD41, 
	         FieldD42, 
	         FieldD43, 
	         FieldD44, 
	         FieldD45, 
	         FieldD46, 
	         FieldD47, 
	         FieldD48, 
	         FieldD49, 
	         FieldD50, 
	         FieldR1, 
	         FieldR2, 
	         FieldR3, 
	         FieldR4, 
	         FieldR5, 
	         FieldR6, 
	         FieldR7, 
	         FieldR8, 
	         FieldR9, 
	         FieldR10, 
	         FieldR11, 
	         FieldR12, 
	         FieldR13, 
	         FieldR14, 
	         FieldR15, 
	         FieldR16, 
	         FieldR17, 
	         FieldR18, 
	         FieldR19, 
	         FieldR20,
	      }

		List<POCOPropertyNames> CustomMappedPOCOProperties=new List<POCOPropertyNames>();
        List<PMPropertyNames> CustomMappedPMProperties=new List<PMPropertyNames>();
    
	    public void PMToPOCO(DeclarationStatusPM entityPM, DeclarationStatus entityPOCO)
        {
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
				entityPOCO.Tenant = entityPM.Tenant;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
				entityPOCO.SearchFields = entityPM.SearchFields;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC1))
            {
				entityPOCO.FieldC1 = entityPM.FieldC1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC2))
            {
				entityPOCO.FieldC2 = entityPM.FieldC2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC3))
            {
				entityPOCO.FieldC3 = entityPM.FieldC3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC4))
            {
				entityPOCO.FieldC4 = entityPM.FieldC4;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC5))
            {
				entityPOCO.FieldC5 = entityPM.FieldC5;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC6))
            {
				entityPOCO.FieldC6 = entityPM.FieldC6;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC7))
            {
				entityPOCO.FieldC7 = entityPM.FieldC7;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC8))
            {
				entityPOCO.FieldC8 = entityPM.FieldC8;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC9))
            {
				entityPOCO.FieldC9 = entityPM.FieldC9;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC10))
            {
				entityPOCO.FieldC10 = entityPM.FieldC10;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC11))
            {
				entityPOCO.FieldC11 = entityPM.FieldC11;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC12))
            {
				entityPOCO.FieldC12 = entityPM.FieldC12;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC13))
            {
				entityPOCO.FieldC13 = entityPM.FieldC13;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC14))
            {
				entityPOCO.FieldC14 = entityPM.FieldC14;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC15))
            {
				entityPOCO.FieldC15 = entityPM.FieldC15;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC16))
            {
				entityPOCO.FieldC16 = entityPM.FieldC16;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC17))
            {
				entityPOCO.FieldC17 = entityPM.FieldC17;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC18))
            {
				entityPOCO.FieldC18 = entityPM.FieldC18;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC19))
            {
				entityPOCO.FieldC19 = entityPM.FieldC19;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC20))
            {
				entityPOCO.FieldC20 = entityPM.FieldC20;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC21))
            {
				entityPOCO.FieldC21 = entityPM.FieldC21;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC22))
            {
				entityPOCO.FieldC22 = entityPM.FieldC22;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC23))
            {
				entityPOCO.FieldC23 = entityPM.FieldC23;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC24))
            {
				entityPOCO.FieldC24 = entityPM.FieldC24;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC25))
            {
				entityPOCO.FieldC25 = entityPM.FieldC25;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC26))
            {
				entityPOCO.FieldC26 = entityPM.FieldC26;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC27))
            {
				entityPOCO.FieldC27 = entityPM.FieldC27;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC28))
            {
				entityPOCO.FieldC28 = entityPM.FieldC28;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC29))
            {
				entityPOCO.FieldC29 = entityPM.FieldC29;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC30))
            {
				entityPOCO.FieldC30 = entityPM.FieldC30;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC31))
            {
				entityPOCO.FieldC31 = entityPM.FieldC31;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC32))
            {
				entityPOCO.FieldC32 = entityPM.FieldC32;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC33))
            {
				entityPOCO.FieldC33 = entityPM.FieldC33;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC34))
            {
				entityPOCO.FieldC34 = entityPM.FieldC34;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC35))
            {
				entityPOCO.FieldC35 = entityPM.FieldC35;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC36))
            {
				entityPOCO.FieldC36 = entityPM.FieldC36;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC37))
            {
				entityPOCO.FieldC37 = entityPM.FieldC37;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC38))
            {
				entityPOCO.FieldC38 = entityPM.FieldC38;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC39))
            {
				entityPOCO.FieldC39 = entityPM.FieldC39;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC40))
            {
				entityPOCO.FieldC40 = entityPM.FieldC40;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC41))
            {
				entityPOCO.FieldC41 = entityPM.FieldC41;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC42))
            {
				entityPOCO.FieldC42 = entityPM.FieldC42;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC43))
            {
				entityPOCO.FieldC43 = entityPM.FieldC43;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC44))
            {
				entityPOCO.FieldC44 = entityPM.FieldC44;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC45))
            {
				entityPOCO.FieldC45 = entityPM.FieldC45;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC46))
            {
				entityPOCO.FieldC46 = entityPM.FieldC46;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC47))
            {
				entityPOCO.FieldC47 = entityPM.FieldC47;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC48))
            {
				entityPOCO.FieldC48 = entityPM.FieldC48;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC49))
            {
				entityPOCO.FieldC49 = entityPM.FieldC49;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC50))
            {
				entityPOCO.FieldC50 = entityPM.FieldC50;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD1))
            {
				entityPOCO.FieldD1 = entityPM.FieldD1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD2))
            {
				entityPOCO.FieldD2 = entityPM.FieldD2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD3))
            {
				entityPOCO.FieldD3 = entityPM.FieldD3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD4))
            {
				entityPOCO.FieldD4 = entityPM.FieldD4;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD5))
            {
				entityPOCO.FieldD5 = entityPM.FieldD5;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD6))
            {
				entityPOCO.FieldD6 = entityPM.FieldD6;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD7))
            {
				entityPOCO.FieldD7 = entityPM.FieldD7;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD8))
            {
				entityPOCO.FieldD8 = entityPM.FieldD8;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD9))
            {
				entityPOCO.FieldD9 = entityPM.FieldD9;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD10))
            {
				entityPOCO.FieldD10 = entityPM.FieldD10;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD11))
            {
				entityPOCO.FieldD11 = entityPM.FieldD11;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD12))
            {
				entityPOCO.FieldD12 = entityPM.FieldD12;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD13))
            {
				entityPOCO.FieldD13 = entityPM.FieldD13;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD14))
            {
				entityPOCO.FieldD14 = entityPM.FieldD14;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD15))
            {
				entityPOCO.FieldD15 = entityPM.FieldD15;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD16))
            {
				entityPOCO.FieldD16 = entityPM.FieldD16;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD17))
            {
				entityPOCO.FieldD17 = entityPM.FieldD17;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD18))
            {
				entityPOCO.FieldD18 = entityPM.FieldD18;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD19))
            {
				entityPOCO.FieldD19 = entityPM.FieldD19;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD20))
            {
				entityPOCO.FieldD20 = entityPM.FieldD20;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD21))
            {
				entityPOCO.FieldD21 = entityPM.FieldD21;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD22))
            {
				entityPOCO.FieldD22 = entityPM.FieldD22;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD23))
            {
				entityPOCO.FieldD23 = entityPM.FieldD23;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD24))
            {
				entityPOCO.FieldD24 = entityPM.FieldD24;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD25))
            {
				entityPOCO.FieldD25 = entityPM.FieldD25;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD26))
            {
				entityPOCO.FieldD26 = entityPM.FieldD26;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD27))
            {
				entityPOCO.FieldD27 = entityPM.FieldD27;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD28))
            {
				entityPOCO.FieldD28 = entityPM.FieldD28;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD29))
            {
				entityPOCO.FieldD29 = entityPM.FieldD29;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD30))
            {
				entityPOCO.FieldD30 = entityPM.FieldD30;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD31))
            {
				entityPOCO.FieldD31 = entityPM.FieldD31;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD32))
            {
				entityPOCO.FieldD32 = entityPM.FieldD32;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD33))
            {
				entityPOCO.FieldD33 = entityPM.FieldD33;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD34))
            {
				entityPOCO.FieldD34 = entityPM.FieldD34;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD35))
            {
				entityPOCO.FieldD35 = entityPM.FieldD35;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD36))
            {
				entityPOCO.FieldD36 = entityPM.FieldD36;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD37))
            {
				entityPOCO.FieldD37 = entityPM.FieldD37;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD38))
            {
				entityPOCO.FieldD38 = entityPM.FieldD38;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD39))
            {
				entityPOCO.FieldD39 = entityPM.FieldD39;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD40))
            {
				entityPOCO.FieldD40 = entityPM.FieldD40;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD41))
            {
				entityPOCO.FieldD41 = entityPM.FieldD41;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD42))
            {
				entityPOCO.FieldD42 = entityPM.FieldD42;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD43))
            {
				entityPOCO.FieldD43 = entityPM.FieldD43;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD44))
            {
				entityPOCO.FieldD44 = entityPM.FieldD44;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD45))
            {
				entityPOCO.FieldD45 = entityPM.FieldD45;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD46))
            {
				entityPOCO.FieldD46 = entityPM.FieldD46;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD47))
            {
				entityPOCO.FieldD47 = entityPM.FieldD47;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD48))
            {
				entityPOCO.FieldD48 = entityPM.FieldD48;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD49))
            {
				entityPOCO.FieldD49 = entityPM.FieldD49;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD50))
            {
				entityPOCO.FieldD50 = entityPM.FieldD50;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR1))
            {
				entityPOCO.FieldR1 = entityPM.FieldR1;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR2))
            {
				entityPOCO.FieldR2 = entityPM.FieldR2;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR3))
            {
				entityPOCO.FieldR3 = entityPM.FieldR3;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR4))
            {
				entityPOCO.FieldR4 = entityPM.FieldR4;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR5))
            {
				entityPOCO.FieldR5 = entityPM.FieldR5;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR6))
            {
				entityPOCO.FieldR6 = entityPM.FieldR6;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR7))
            {
				entityPOCO.FieldR7 = entityPM.FieldR7;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR8))
            {
				entityPOCO.FieldR8 = entityPM.FieldR8;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR9))
            {
				entityPOCO.FieldR9 = entityPM.FieldR9;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR10))
            {
				entityPOCO.FieldR10 = entityPM.FieldR10;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR11))
            {
				entityPOCO.FieldR11 = entityPM.FieldR11;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR12))
            {
				entityPOCO.FieldR12 = entityPM.FieldR12;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR13))
            {
				entityPOCO.FieldR13 = entityPM.FieldR13;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR14))
            {
				entityPOCO.FieldR14 = entityPM.FieldR14;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR15))
            {
				entityPOCO.FieldR15 = entityPM.FieldR15;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR16))
            {
				entityPOCO.FieldR16 = entityPM.FieldR16;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR17))
            {
				entityPOCO.FieldR17 = entityPM.FieldR17;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR18))
            {
				entityPOCO.FieldR18 = entityPM.FieldR18;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR19))
            {
				entityPOCO.FieldR19 = entityPM.FieldR19;
			}
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR20))
            {
				entityPOCO.FieldR20 = entityPM.FieldR20;
			}
			
				BuildSearchFieldsGenerated(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
		  }

		public void POCOToPM(DeclarationStatusPM entityPM, DeclarationStatus entityPOCO)
        {
			 
			if (!CustomMappedPMProperties.Contains(PMPropertyNames.Tenant))
            {
					entityPM.Tenant = entityPOCO.Tenant;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.SearchFields))
            {
					entityPM.SearchFields = entityPOCO.SearchFields;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.DeclarationId))
            {
					entityPM.DeclarationId = entityPOCO.DeclarationId;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC1))
            {
					entityPM.FieldC1 = entityPOCO.FieldC1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC2))
            {
					entityPM.FieldC2 = entityPOCO.FieldC2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC3))
            {
					entityPM.FieldC3 = entityPOCO.FieldC3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC4))
            {
					entityPM.FieldC4 = entityPOCO.FieldC4;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC5))
            {
					entityPM.FieldC5 = entityPOCO.FieldC5;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC6))
            {
					entityPM.FieldC6 = entityPOCO.FieldC6;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC7))
            {
					entityPM.FieldC7 = entityPOCO.FieldC7;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC8))
            {
					entityPM.FieldC8 = entityPOCO.FieldC8;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC9))
            {
					entityPM.FieldC9 = entityPOCO.FieldC9;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC10))
            {
					entityPM.FieldC10 = entityPOCO.FieldC10;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC11))
            {
					entityPM.FieldC11 = entityPOCO.FieldC11;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC12))
            {
					entityPM.FieldC12 = entityPOCO.FieldC12;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC13))
            {
					entityPM.FieldC13 = entityPOCO.FieldC13;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC14))
            {
					entityPM.FieldC14 = entityPOCO.FieldC14;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC15))
            {
					entityPM.FieldC15 = entityPOCO.FieldC15;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC16))
            {
					entityPM.FieldC16 = entityPOCO.FieldC16;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC17))
            {
					entityPM.FieldC17 = entityPOCO.FieldC17;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC18))
            {
					entityPM.FieldC18 = entityPOCO.FieldC18;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC19))
            {
					entityPM.FieldC19 = entityPOCO.FieldC19;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC20))
            {
					entityPM.FieldC20 = entityPOCO.FieldC20;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC21))
            {
					entityPM.FieldC21 = entityPOCO.FieldC21;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC22))
            {
					entityPM.FieldC22 = entityPOCO.FieldC22;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC23))
            {
					entityPM.FieldC23 = entityPOCO.FieldC23;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC24))
            {
					entityPM.FieldC24 = entityPOCO.FieldC24;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC25))
            {
					entityPM.FieldC25 = entityPOCO.FieldC25;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC26))
            {
					entityPM.FieldC26 = entityPOCO.FieldC26;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC27))
            {
					entityPM.FieldC27 = entityPOCO.FieldC27;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC28))
            {
					entityPM.FieldC28 = entityPOCO.FieldC28;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC29))
            {
					entityPM.FieldC29 = entityPOCO.FieldC29;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC30))
            {
					entityPM.FieldC30 = entityPOCO.FieldC30;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC31))
            {
					entityPM.FieldC31 = entityPOCO.FieldC31;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC32))
            {
					entityPM.FieldC32 = entityPOCO.FieldC32;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC33))
            {
					entityPM.FieldC33 = entityPOCO.FieldC33;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC34))
            {
					entityPM.FieldC34 = entityPOCO.FieldC34;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC35))
            {
					entityPM.FieldC35 = entityPOCO.FieldC35;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC36))
            {
					entityPM.FieldC36 = entityPOCO.FieldC36;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC37))
            {
					entityPM.FieldC37 = entityPOCO.FieldC37;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC38))
            {
					entityPM.FieldC38 = entityPOCO.FieldC38;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC39))
            {
					entityPM.FieldC39 = entityPOCO.FieldC39;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC40))
            {
					entityPM.FieldC40 = entityPOCO.FieldC40;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC41))
            {
					entityPM.FieldC41 = entityPOCO.FieldC41;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC42))
            {
					entityPM.FieldC42 = entityPOCO.FieldC42;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC43))
            {
					entityPM.FieldC43 = entityPOCO.FieldC43;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC44))
            {
					entityPM.FieldC44 = entityPOCO.FieldC44;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC45))
            {
					entityPM.FieldC45 = entityPOCO.FieldC45;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC46))
            {
					entityPM.FieldC46 = entityPOCO.FieldC46;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC47))
            {
					entityPM.FieldC47 = entityPOCO.FieldC47;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC48))
            {
					entityPM.FieldC48 = entityPOCO.FieldC48;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC49))
            {
					entityPM.FieldC49 = entityPOCO.FieldC49;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldC50))
            {
					entityPM.FieldC50 = entityPOCO.FieldC50;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD1))
            {
					entityPM.FieldD1 = entityPOCO.FieldD1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD2))
            {
					entityPM.FieldD2 = entityPOCO.FieldD2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD3))
            {
					entityPM.FieldD3 = entityPOCO.FieldD3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD4))
            {
					entityPM.FieldD4 = entityPOCO.FieldD4;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD5))
            {
					entityPM.FieldD5 = entityPOCO.FieldD5;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD6))
            {
					entityPM.FieldD6 = entityPOCO.FieldD6;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD7))
            {
					entityPM.FieldD7 = entityPOCO.FieldD7;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD8))
            {
					entityPM.FieldD8 = entityPOCO.FieldD8;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD9))
            {
					entityPM.FieldD9 = entityPOCO.FieldD9;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD10))
            {
					entityPM.FieldD10 = entityPOCO.FieldD10;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD11))
            {
					entityPM.FieldD11 = entityPOCO.FieldD11;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD12))
            {
					entityPM.FieldD12 = entityPOCO.FieldD12;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD13))
            {
					entityPM.FieldD13 = entityPOCO.FieldD13;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD14))
            {
					entityPM.FieldD14 = entityPOCO.FieldD14;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD15))
            {
					entityPM.FieldD15 = entityPOCO.FieldD15;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD16))
            {
					entityPM.FieldD16 = entityPOCO.FieldD16;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD17))
            {
					entityPM.FieldD17 = entityPOCO.FieldD17;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD18))
            {
					entityPM.FieldD18 = entityPOCO.FieldD18;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD19))
            {
					entityPM.FieldD19 = entityPOCO.FieldD19;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD20))
            {
					entityPM.FieldD20 = entityPOCO.FieldD20;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD21))
            {
					entityPM.FieldD21 = entityPOCO.FieldD21;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD22))
            {
					entityPM.FieldD22 = entityPOCO.FieldD22;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD23))
            {
					entityPM.FieldD23 = entityPOCO.FieldD23;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD24))
            {
					entityPM.FieldD24 = entityPOCO.FieldD24;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD25))
            {
					entityPM.FieldD25 = entityPOCO.FieldD25;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD26))
            {
					entityPM.FieldD26 = entityPOCO.FieldD26;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD27))
            {
					entityPM.FieldD27 = entityPOCO.FieldD27;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD28))
            {
					entityPM.FieldD28 = entityPOCO.FieldD28;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD29))
            {
					entityPM.FieldD29 = entityPOCO.FieldD29;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD30))
            {
					entityPM.FieldD30 = entityPOCO.FieldD30;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD31))
            {
					entityPM.FieldD31 = entityPOCO.FieldD31;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD32))
            {
					entityPM.FieldD32 = entityPOCO.FieldD32;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD33))
            {
					entityPM.FieldD33 = entityPOCO.FieldD33;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD34))
            {
					entityPM.FieldD34 = entityPOCO.FieldD34;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD35))
            {
					entityPM.FieldD35 = entityPOCO.FieldD35;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD36))
            {
					entityPM.FieldD36 = entityPOCO.FieldD36;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD37))
            {
					entityPM.FieldD37 = entityPOCO.FieldD37;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD38))
            {
					entityPM.FieldD38 = entityPOCO.FieldD38;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD39))
            {
					entityPM.FieldD39 = entityPOCO.FieldD39;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD40))
            {
					entityPM.FieldD40 = entityPOCO.FieldD40;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD41))
            {
					entityPM.FieldD41 = entityPOCO.FieldD41;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD42))
            {
					entityPM.FieldD42 = entityPOCO.FieldD42;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD43))
            {
					entityPM.FieldD43 = entityPOCO.FieldD43;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD44))
            {
					entityPM.FieldD44 = entityPOCO.FieldD44;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD45))
            {
					entityPM.FieldD45 = entityPOCO.FieldD45;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD46))
            {
					entityPM.FieldD46 = entityPOCO.FieldD46;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD47))
            {
					entityPM.FieldD47 = entityPOCO.FieldD47;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD48))
            {
					entityPM.FieldD48 = entityPOCO.FieldD48;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD49))
            {
					entityPM.FieldD49 = entityPOCO.FieldD49;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldD50))
            {
					entityPM.FieldD50 = entityPOCO.FieldD50;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR1))
            {
					entityPM.FieldR1 = entityPOCO.FieldR1;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR2))
            {
					entityPM.FieldR2 = entityPOCO.FieldR2;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR3))
            {
					entityPM.FieldR3 = entityPOCO.FieldR3;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR4))
            {
					entityPM.FieldR4 = entityPOCO.FieldR4;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR5))
            {
					entityPM.FieldR5 = entityPOCO.FieldR5;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR6))
            {
					entityPM.FieldR6 = entityPOCO.FieldR6;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR7))
            {
					entityPM.FieldR7 = entityPOCO.FieldR7;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR8))
            {
					entityPM.FieldR8 = entityPOCO.FieldR8;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR9))
            {
					entityPM.FieldR9 = entityPOCO.FieldR9;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR10))
            {
					entityPM.FieldR10 = entityPOCO.FieldR10;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR11))
            {
					entityPM.FieldR11 = entityPOCO.FieldR11;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR12))
            {
					entityPM.FieldR12 = entityPOCO.FieldR12;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR13))
            {
					entityPM.FieldR13 = entityPOCO.FieldR13;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR14))
            {
					entityPM.FieldR14 = entityPOCO.FieldR14;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR15))
            {
					entityPM.FieldR15 = entityPOCO.FieldR15;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR16))
            {
					entityPM.FieldR16 = entityPOCO.FieldR16;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR17))
            {
					entityPM.FieldR17 = entityPOCO.FieldR17;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR18))
            {
					entityPM.FieldR18 = entityPOCO.FieldR18;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR19))
            {
					entityPM.FieldR19 = entityPOCO.FieldR19;
            }

			if (!CustomMappedPMProperties.Contains(PMPropertyNames.FieldR20))
            {
					entityPM.FieldR20 = entityPOCO.FieldR20;
            }

		}

		public void PMToOldPM(DeclarationStatusPM entityPM, DeclarationStatusPM oldEntityPM)
        {
		     oldEntityPM.ChangedProperties.Clear();
			 
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.Tenant))
            {
                oldEntityPM.Tenant = entityPM.Tenant;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.SearchFields))
            {
                oldEntityPM.SearchFields = entityPM.SearchFields;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC1))
            {
                oldEntityPM.FieldC1 = entityPM.FieldC1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC2))
            {
                oldEntityPM.FieldC2 = entityPM.FieldC2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC3))
            {
                oldEntityPM.FieldC3 = entityPM.FieldC3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC4))
            {
                oldEntityPM.FieldC4 = entityPM.FieldC4;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC5))
            {
                oldEntityPM.FieldC5 = entityPM.FieldC5;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC6))
            {
                oldEntityPM.FieldC6 = entityPM.FieldC6;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC7))
            {
                oldEntityPM.FieldC7 = entityPM.FieldC7;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC8))
            {
                oldEntityPM.FieldC8 = entityPM.FieldC8;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC9))
            {
                oldEntityPM.FieldC9 = entityPM.FieldC9;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC10))
            {
                oldEntityPM.FieldC10 = entityPM.FieldC10;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC11))
            {
                oldEntityPM.FieldC11 = entityPM.FieldC11;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC12))
            {
                oldEntityPM.FieldC12 = entityPM.FieldC12;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC13))
            {
                oldEntityPM.FieldC13 = entityPM.FieldC13;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC14))
            {
                oldEntityPM.FieldC14 = entityPM.FieldC14;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC15))
            {
                oldEntityPM.FieldC15 = entityPM.FieldC15;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC16))
            {
                oldEntityPM.FieldC16 = entityPM.FieldC16;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC17))
            {
                oldEntityPM.FieldC17 = entityPM.FieldC17;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC18))
            {
                oldEntityPM.FieldC18 = entityPM.FieldC18;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC19))
            {
                oldEntityPM.FieldC19 = entityPM.FieldC19;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC20))
            {
                oldEntityPM.FieldC20 = entityPM.FieldC20;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC21))
            {
                oldEntityPM.FieldC21 = entityPM.FieldC21;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC22))
            {
                oldEntityPM.FieldC22 = entityPM.FieldC22;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC23))
            {
                oldEntityPM.FieldC23 = entityPM.FieldC23;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC24))
            {
                oldEntityPM.FieldC24 = entityPM.FieldC24;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC25))
            {
                oldEntityPM.FieldC25 = entityPM.FieldC25;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC26))
            {
                oldEntityPM.FieldC26 = entityPM.FieldC26;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC27))
            {
                oldEntityPM.FieldC27 = entityPM.FieldC27;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC28))
            {
                oldEntityPM.FieldC28 = entityPM.FieldC28;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC29))
            {
                oldEntityPM.FieldC29 = entityPM.FieldC29;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC30))
            {
                oldEntityPM.FieldC30 = entityPM.FieldC30;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC31))
            {
                oldEntityPM.FieldC31 = entityPM.FieldC31;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC32))
            {
                oldEntityPM.FieldC32 = entityPM.FieldC32;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC33))
            {
                oldEntityPM.FieldC33 = entityPM.FieldC33;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC34))
            {
                oldEntityPM.FieldC34 = entityPM.FieldC34;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC35))
            {
                oldEntityPM.FieldC35 = entityPM.FieldC35;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC36))
            {
                oldEntityPM.FieldC36 = entityPM.FieldC36;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC37))
            {
                oldEntityPM.FieldC37 = entityPM.FieldC37;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC38))
            {
                oldEntityPM.FieldC38 = entityPM.FieldC38;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC39))
            {
                oldEntityPM.FieldC39 = entityPM.FieldC39;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC40))
            {
                oldEntityPM.FieldC40 = entityPM.FieldC40;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC41))
            {
                oldEntityPM.FieldC41 = entityPM.FieldC41;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC42))
            {
                oldEntityPM.FieldC42 = entityPM.FieldC42;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC43))
            {
                oldEntityPM.FieldC43 = entityPM.FieldC43;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC44))
            {
                oldEntityPM.FieldC44 = entityPM.FieldC44;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC45))
            {
                oldEntityPM.FieldC45 = entityPM.FieldC45;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC46))
            {
                oldEntityPM.FieldC46 = entityPM.FieldC46;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC47))
            {
                oldEntityPM.FieldC47 = entityPM.FieldC47;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC48))
            {
                oldEntityPM.FieldC48 = entityPM.FieldC48;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC49))
            {
                oldEntityPM.FieldC49 = entityPM.FieldC49;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldC50))
            {
                oldEntityPM.FieldC50 = entityPM.FieldC50;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD1))
            {
                oldEntityPM.FieldD1 = entityPM.FieldD1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD2))
            {
                oldEntityPM.FieldD2 = entityPM.FieldD2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD3))
            {
                oldEntityPM.FieldD3 = entityPM.FieldD3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD4))
            {
                oldEntityPM.FieldD4 = entityPM.FieldD4;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD5))
            {
                oldEntityPM.FieldD5 = entityPM.FieldD5;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD6))
            {
                oldEntityPM.FieldD6 = entityPM.FieldD6;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD7))
            {
                oldEntityPM.FieldD7 = entityPM.FieldD7;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD8))
            {
                oldEntityPM.FieldD8 = entityPM.FieldD8;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD9))
            {
                oldEntityPM.FieldD9 = entityPM.FieldD9;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD10))
            {
                oldEntityPM.FieldD10 = entityPM.FieldD10;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD11))
            {
                oldEntityPM.FieldD11 = entityPM.FieldD11;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD12))
            {
                oldEntityPM.FieldD12 = entityPM.FieldD12;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD13))
            {
                oldEntityPM.FieldD13 = entityPM.FieldD13;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD14))
            {
                oldEntityPM.FieldD14 = entityPM.FieldD14;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD15))
            {
                oldEntityPM.FieldD15 = entityPM.FieldD15;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD16))
            {
                oldEntityPM.FieldD16 = entityPM.FieldD16;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD17))
            {
                oldEntityPM.FieldD17 = entityPM.FieldD17;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD18))
            {
                oldEntityPM.FieldD18 = entityPM.FieldD18;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD19))
            {
                oldEntityPM.FieldD19 = entityPM.FieldD19;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD20))
            {
                oldEntityPM.FieldD20 = entityPM.FieldD20;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD21))
            {
                oldEntityPM.FieldD21 = entityPM.FieldD21;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD22))
            {
                oldEntityPM.FieldD22 = entityPM.FieldD22;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD23))
            {
                oldEntityPM.FieldD23 = entityPM.FieldD23;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD24))
            {
                oldEntityPM.FieldD24 = entityPM.FieldD24;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD25))
            {
                oldEntityPM.FieldD25 = entityPM.FieldD25;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD26))
            {
                oldEntityPM.FieldD26 = entityPM.FieldD26;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD27))
            {
                oldEntityPM.FieldD27 = entityPM.FieldD27;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD28))
            {
                oldEntityPM.FieldD28 = entityPM.FieldD28;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD29))
            {
                oldEntityPM.FieldD29 = entityPM.FieldD29;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD30))
            {
                oldEntityPM.FieldD30 = entityPM.FieldD30;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD31))
            {
                oldEntityPM.FieldD31 = entityPM.FieldD31;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD32))
            {
                oldEntityPM.FieldD32 = entityPM.FieldD32;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD33))
            {
                oldEntityPM.FieldD33 = entityPM.FieldD33;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD34))
            {
                oldEntityPM.FieldD34 = entityPM.FieldD34;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD35))
            {
                oldEntityPM.FieldD35 = entityPM.FieldD35;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD36))
            {
                oldEntityPM.FieldD36 = entityPM.FieldD36;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD37))
            {
                oldEntityPM.FieldD37 = entityPM.FieldD37;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD38))
            {
                oldEntityPM.FieldD38 = entityPM.FieldD38;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD39))
            {
                oldEntityPM.FieldD39 = entityPM.FieldD39;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD40))
            {
                oldEntityPM.FieldD40 = entityPM.FieldD40;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD41))
            {
                oldEntityPM.FieldD41 = entityPM.FieldD41;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD42))
            {
                oldEntityPM.FieldD42 = entityPM.FieldD42;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD43))
            {
                oldEntityPM.FieldD43 = entityPM.FieldD43;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD44))
            {
                oldEntityPM.FieldD44 = entityPM.FieldD44;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD45))
            {
                oldEntityPM.FieldD45 = entityPM.FieldD45;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD46))
            {
                oldEntityPM.FieldD46 = entityPM.FieldD46;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD47))
            {
                oldEntityPM.FieldD47 = entityPM.FieldD47;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD48))
            {
                oldEntityPM.FieldD48 = entityPM.FieldD48;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD49))
            {
                oldEntityPM.FieldD49 = entityPM.FieldD49;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldD50))
            {
                oldEntityPM.FieldD50 = entityPM.FieldD50;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR1))
            {
                oldEntityPM.FieldR1 = entityPM.FieldR1;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR2))
            {
                oldEntityPM.FieldR2 = entityPM.FieldR2;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR3))
            {
                oldEntityPM.FieldR3 = entityPM.FieldR3;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR4))
            {
                oldEntityPM.FieldR4 = entityPM.FieldR4;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR5))
            {
                oldEntityPM.FieldR5 = entityPM.FieldR5;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR6))
            {
                oldEntityPM.FieldR6 = entityPM.FieldR6;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR7))
            {
                oldEntityPM.FieldR7 = entityPM.FieldR7;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR8))
            {
                oldEntityPM.FieldR8 = entityPM.FieldR8;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR9))
            {
                oldEntityPM.FieldR9 = entityPM.FieldR9;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR10))
            {
                oldEntityPM.FieldR10 = entityPM.FieldR10;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR11))
            {
                oldEntityPM.FieldR11 = entityPM.FieldR11;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR12))
            {
                oldEntityPM.FieldR12 = entityPM.FieldR12;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR13))
            {
                oldEntityPM.FieldR13 = entityPM.FieldR13;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR14))
            {
                oldEntityPM.FieldR14 = entityPM.FieldR14;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR15))
            {
                oldEntityPM.FieldR15 = entityPM.FieldR15;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR16))
            {
                oldEntityPM.FieldR16 = entityPM.FieldR16;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR17))
            {
                oldEntityPM.FieldR17 = entityPM.FieldR17;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR18))
            {
                oldEntityPM.FieldR18 = entityPM.FieldR18;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR19))
            {
                oldEntityPM.FieldR19 = entityPM.FieldR19;
            }
			
			if (!CustomMappedPOCOProperties.Contains(POCOPropertyNames.FieldR20))
            {
                oldEntityPM.FieldR20 = entityPM.FieldR20;
            }
			
		}

	    public void EncodeBase64NVARCHARFields(DeclarationStatusPM entityPM)
        {
            if (String.IsNullOrWhiteSpace(entityPM.EncodeBase64NVARCHARFieldsBy)) 
            {
                return;

            }
            if (!String.IsNullOrWhiteSpace(entityPM.SearchFields)) //T4 find type == nText 
            {
                entityPM.SearchFields = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.SearchFields));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR1)) //T4 find type == nText 
            {
                entityPM.FieldR1 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR1));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR2)) //T4 find type == nText 
            {
                entityPM.FieldR2 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR2));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR3)) //T4 find type == nText 
            {
                entityPM.FieldR3 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR3));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR4)) //T4 find type == nText 
            {
                entityPM.FieldR4 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR4));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR5)) //T4 find type == nText 
            {
                entityPM.FieldR5 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR5));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR6)) //T4 find type == nText 
            {
                entityPM.FieldR6 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR6));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR7)) //T4 find type == nText 
            {
                entityPM.FieldR7 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR7));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR8)) //T4 find type == nText 
            {
                entityPM.FieldR8 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR8));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR9)) //T4 find type == nText 
            {
                entityPM.FieldR9 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR9));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR10)) //T4 find type == nText 
            {
                entityPM.FieldR10 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR10));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR11)) //T4 find type == nText 
            {
                entityPM.FieldR11 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR11));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR12)) //T4 find type == nText 
            {
                entityPM.FieldR12 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR12));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR13)) //T4 find type == nText 
            {
                entityPM.FieldR13 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR13));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR14)) //T4 find type == nText 
            {
                entityPM.FieldR14 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR14));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR15)) //T4 find type == nText 
            {
                entityPM.FieldR15 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR15));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR16)) //T4 find type == nText 
            {
                entityPM.FieldR16 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR16));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR17)) //T4 find type == nText 
            {
                entityPM.FieldR17 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR17));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR18)) //T4 find type == nText 
            {
                entityPM.FieldR18 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR18));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR19)) //T4 find type == nText 
            {
                entityPM.FieldR19 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR19));
            }
            if (!String.IsNullOrWhiteSpace(entityPM.FieldR20)) //T4 find type == nText 
            {
                entityPM.FieldR20 = Encoding.GetEncoding(entityPM.EncodeBase64NVARCHARFieldsBy).GetString(Convert.FromBase64String(entityPM.FieldR20));
            }
            entityPM.EncodeBase64NVARCHARFieldsBy=null;
		}


	    public void AddPOCOPropertyName(POCOPropertyNames pocoPropertyName)
        {
            CustomMappedPOCOProperties.Add(pocoPropertyName);
        }

        public void AddPMPropertyName(PMPropertyNames pocoPropertyName)
        {
            CustomMappedPMProperties.Add(pocoPropertyName);
        }
		
		private void BuildSearchFieldsGenerated(DeclarationStatusPM entityPM, DeclarationStatus entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";
			
           
            entityPM.SearchFields += mySearchFields;
            entityPOCO.SearchFields += mySearchFields;
        }
			  
   }
}
	 