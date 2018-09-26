using System;
using System.Reflection;
using System.Reflection.Emit;

namespace PYS.VirtualClinic.Infrastructure
{
    internal static class ClassBuilder
    {
        public static Type CompileResultType<T>(string[] selectedPropertyNames, string typeName)
        {
            return CompileResultType(typeof(T), selectedPropertyNames, typeName);
        }

        public static Type CompileResultType(Type baseType, string[] selectedPropertyNames, string typeName, string moduleName = "MainModule", string assemblyName = "Assembly")
        {
            TypeBuilder tb = GetTypeBuilder(typeName, moduleName, assemblyName);

            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            foreach (var pn in selectedPropertyNames)
            {
                var property = baseType.GetProperty(pn);

                CreateProperty(tb, pn, property.PropertyType);
            }

            Type objectType = tb.CreateType();

            return objectType;
        }

        private static TypeBuilder GetTypeBuilder(string typeName, string moduleName, string assemblyName)
        {
            var an = new AssemblyName(assemblyName);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            TypeBuilder tb = moduleBuilder.DefineType(typeName,
                                                      TypeAttributes.Public |
                                                      TypeAttributes.Class |
                                                      TypeAttributes.AutoClass |
                                                      TypeAttributes.AnsiClass |
                                                      TypeAttributes.BeforeFieldInit |
                                                      TypeAttributes.AutoLayout,
                                                      null);
            return tb;
        }

        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr = tb.DefineMethod("set_" + propertyName,
                                                            MethodAttributes.Public |
                                                            MethodAttributes.SpecialName |
                                                            MethodAttributes.HideBySig,
                                                            null,
                                                            new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}