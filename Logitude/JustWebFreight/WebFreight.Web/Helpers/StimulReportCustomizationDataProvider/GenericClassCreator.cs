using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Logitude.BL.InfrastructureModel.EntityLists;

namespace WebFreight.Web.Helpers.StimulReportCustomizationDataProvider
{
        public static class GenericClassCreator
        {

            public static Type Create(string name, List<Field> fields)
            {
                var myType = CompileResultType(name, fields);
                return myType;
            }
            public static Type CompileResultType(string name, List<Field> fields)
            {
                TypeBuilder typeBuilder = GetTypeBuilder(name);

                foreach (var field in fields)
                {
                    CreateProperty(typeBuilder, field);
                }

                Type objectType = typeBuilder.CreateType();
                return objectType;
            }

            private static TypeBuilder GetTypeBuilder(string name)
            {
                var an = new AssemblyName(name);
                AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
                ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
                TypeBuilder typeBuilder = moduleBuilder.DefineType(name + "_"+ Guid.NewGuid().ToString(),
                        TypeAttributes.Public |
                        TypeAttributes.Class |
                        TypeAttributes.AutoClass |
                        TypeAttributes.AnsiClass |
                        TypeAttributes.BeforeFieldInit |
                        TypeAttributes.AutoLayout,
                        null);
                return typeBuilder;
            }

            private static void CreateProperty(TypeBuilder typeBuilder, Field field)
            {
               string propertyName = field.Name;
               Type propertyType = field.Type;

               FieldBuilder fieldBuilder = typeBuilder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
                MethodBuilder getPropMethodBuilder = typeBuilder.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
                ILGenerator getILGenerator = getPropMethodBuilder.GetILGenerator();

               getILGenerator.Emit(OpCodes.Ldarg_0);
               getILGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
               getILGenerator.Emit(OpCodes.Ret);

                MethodBuilder setPropMethodBuilder =
                    typeBuilder.DefineMethod("set_" + propertyName,
                      MethodAttributes.Public |
                      MethodAttributes.SpecialName |
                      MethodAttributes.HideBySig,
                      null, new[] { propertyType });

                ILGenerator setILGenerator = setPropMethodBuilder.GetILGenerator();
                Label modifyProperty = setILGenerator.DefineLabel();
                Label exitSet = setILGenerator.DefineLabel();

            setILGenerator.MarkLabel(modifyProperty);
            setILGenerator.Emit(OpCodes.Ldarg_0);
            setILGenerator.Emit(OpCodes.Ldarg_1);
            setILGenerator.Emit(OpCodes.Stfld, fieldBuilder);

            setILGenerator.Emit(OpCodes.Nop);
            setILGenerator.MarkLabel(exitSet);
            setILGenerator.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getPropMethodBuilder);
                propertyBuilder.SetSetMethod(setPropMethodBuilder);
            }

        }


    public class Field
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool IsList { get; set; }
        public bool IsCustom { get; set; }
        public string DataTypeCode { get; set; }
        public string LookUpTableId { get; set; }
        public int Tenant { get; set; }

        public bool IsChild { get; set; }

        
        public AdditionalDetails AdditinalDetails { get; set; }

    }

    public class AdditionalDetails
    {
        public Type Type { get; set; }
        public Type Name { get; set; }
        public List<Field> Fields { get; set; }
    }


}