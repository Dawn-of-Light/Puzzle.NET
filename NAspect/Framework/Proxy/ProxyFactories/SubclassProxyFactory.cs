// *
// * Copyright (C) 2005 Roger Johansson : http://www.puzzleframework.com
// *
// * This library is free software; you can redistribute it and/or modify it
// * under the terms of the GNU Lesser General Public License 2.1 or later, as
// * published by the Free Software Foundation. See the included license.txt
// * or http://www.gnu.org/copyleft/lesser.html for details.
// *
// *

using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Puzzle.NAspect.Framework.Aop;
#if NET2
#endif

namespace Puzzle.NAspect.Framework
{
    /// <summary>
    /// Factory that produces subclass proxy types.
    /// </summary>
    public class SubclassProxyFactory
    {
        /// <summary>
        /// Creates a proxy type of a given type.
        /// </summary>
        /// <param name="baseType">Type to proxyfy</param>
        /// <param name="aspects">Untyped list of <c>IAspects</c> to apply to the proxy.</param>
        /// <param name="mixins">Untyped list of <c>System.Type</c>s that will be mixed in.</param>
        /// <param name="engine">The AopEngine requesting the proxy type</param>
        /// <returns></returns>
        public static Type CreateProxyType(Type baseType, IList aspects, IList mixins, Engine engine)
        {
#if NET2 


            if (Engine.SerializerIsAvailable())
            {
                if (aspects.Count == 0 && mixins.Count == 2)
                    return baseType;
            }
            else
            {
                if (aspects.Count == 0 && mixins.Count == 1)
                    return baseType;
            }
#else
            if (aspects.Count == 0 && mixins.Count == 1)
				return baseType;
#endif


            SubclassProxyFactory factory = new SubclassProxyFactory(engine);


            return factory.CreateType(baseType, aspects, mixins);
        }

        private static long guid = 0;

        private static string GetMethodId(string methodName)
        {
            string methodId = "__" + methodName + "Wrapper" + guid.ToString();
            guid++;

            if (guid == long.MaxValue)
                guid = long.MinValue;

            if (guid == 0)
                throw new Exception("tough luch , youve proxied long.max methods");

            return methodId;
        }

        private Engine engine;
        private ArrayList wrapperMethods = new ArrayList();

        private SubclassProxyFactory(Engine engine)
        {
            this.engine = engine;
        }


        private AssemblyBuilder GetAssemblyBuilder()
        {
            AppDomain domain = Thread.GetDomain();
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = Guid.NewGuid().ToString();
            assemblyName.Version = new Version(1, 0, 0, 0);
            AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            return assemblyBuilder;
        }

        private Type CreateType(Type baseType, IList aspects, IList mixins)
        {
            string typeName = baseType.Name + "AopProxy";
            string moduleName = "MatsSoft.NPersist.Runtime.Proxy";

            AssemblyBuilder assemblyBuilder = GetAssemblyBuilder();

            Type[] interfaces = GetInterfaces(baseType, mixins);


            TypeBuilder typeBuilder = GetTypeBuilder(assemblyBuilder, moduleName, typeName, baseType, interfaces);
#if NET2 && DEBUG
            typeBuilder.SetCustomAttribute(DebuggerVisualizerBuilder());
#endif


            BuildMixinFields(typeBuilder, mixins);

            BuildConstructors(baseType, typeBuilder, mixins);

//            BuildDebugProperty(typeBuilder,baseType);

            foreach (Type mixinType in mixins)
            {
                if (mixinType.IsInterface)
                {
                    //ignore pure interfaces
                }
                else
                {
                    Type mixinInterfaceType = mixinType.GetInterfaces()[0];
                    MixinType(typeBuilder, mixinInterfaceType, GetMixinField(mixinInterfaceType), aspects);
                }
            }

            BuildMethods(baseType, typeBuilder, aspects);

            Type proxyType = typeBuilder.CreateType();

            BuildLookupTables(proxyType, aspects, mixins);

            return proxyType;
        }

        private void BuildLookupTables(Type proxyType, IList aspects, IList mixins)
        {
            MethodCache.methodsLookup[proxyType] = wrapperMethods;
            MethodCache.aspectsLookup[proxyType] = aspects;
            MethodCache.mixinsLookup[proxyType] = mixins;

            foreach (string methodId in wrapperMethods)
            {
                MethodInfo wrapperMethod =
                    proxyType.GetMethod(methodId,
                                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                        BindingFlags.DeclaredOnly);
                MethodCache.wrapperMethodLookup[methodId] = wrapperMethod;

                MethodBase baseMethod = (MethodBase) MethodCache.methodLookup[methodId];
                //array to return
                IList methodinterceptors = new ArrayList();
                //fetch all aspects from the type-aspect lookup
                foreach (IAspect aspect in aspects)
                {
                    IGenericAspect tmpAspect;
                    if (aspect is IGenericAspect)
                        tmpAspect = (IGenericAspect) aspect;
                    else
                        tmpAspect = TypedToGenericConverter.Convert((ITypedAspect) aspect);

                    foreach (IPointcut pointcut in tmpAspect.Pointcuts)
                    {
                        if (pointcut.IsMatch(baseMethod))
                        {
                            foreach (object interceptor in pointcut.Interceptors)
                            {
                                methodinterceptors.Add(interceptor);
                            }
                        }
                    }
                }
                MethodCache.methodInterceptorsLookup[methodId] = methodinterceptors;                
                CallInfo callInfo = MethodCache.GetCallInfo(methodId);
                callInfo.Interceptors = methodinterceptors;
            }
        }

        private void BuildMethods(Type baseType, TypeBuilder typeBuilder, IList aspects)
        {
            MethodInfo[] methods =
                baseType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            foreach (MethodInfo method in methods)
            {
                if (method.IsVirtual && !method.IsFinal)
                {
                    if (engine.PointCutMatcher.MethodShouldBeProxied(method, aspects))
                    {
                        BuildMethod(typeBuilder, method);
                    }
                }
                else if (method.IsVirtual && method.IsFinal)
                {
                    if (method.Name.IndexOf(".") >= 0)
                    {
                        //explicit iface method
                        if (engine.PointCutMatcher.MethodShouldBeProxied(method, aspects))
                        {
                            BuildMethod(typeBuilder, method);
                        }
                    }
                }
            }
        }


        private void BuildMethod(TypeBuilder typeBuilder, MethodInfo method)
        {
            string wrapperName = GetMethodId(method.Name);
            wrapperMethods.Add(wrapperName);

            MethodCache.methodLookup[wrapperName] = method;

            ParameterInfo[] parameterInfos = method.GetParameters();
            Type[] parameterTypes = new Type[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
                parameterTypes[i] = parameterInfos[i].ParameterType;

            string methodName = method.Name;
            MethodAttributes modifier = MethodAttributes.Public;
            if (method.IsFamily)
                modifier = MethodAttributes.Family;

            if (method.IsPublic)
                modifier = MethodAttributes.Public;

            if (method.IsFamilyOrAssembly)
                modifier = MethodAttributes.FamORAssem;

            if (method.IsFamilyAndAssembly)
                modifier = MethodAttributes.FamANDAssem;

            if (method.IsPrivate && methodName.IndexOf(".") >= 0)
            {
                int index = methodName.LastIndexOf(".") + 1;
                methodName = methodName.Substring(index);
                modifier = MethodAttributes.Public;
            }


            MethodBuilder methodBuilder =
                typeBuilder.DefineMethod(methodName, modifier | MethodAttributes.Virtual, CallingConventions.Standard,
                                         method.ReturnType, parameterTypes);
            methodBuilder.SetCustomAttribute(DebuggerStepThroughBuilder());
            methodBuilder.SetCustomAttribute(DebuggerHiddenBuilder());

            for (int i = 0; i < parameterInfos.Length; i++)
            {
            }

            ILGenerator il = methodBuilder.GetILGenerator();


            LocalBuilder paramList = il.DeclareLocal(typeof (ArrayList));


            //create param arraylist
            ConstructorInfo arrayListCtor = typeof (ArrayList).GetConstructor(new Type[0]);
            il.Emit(OpCodes.Newobj, arrayListCtor);
            il.Emit(OpCodes.Stloc, paramList);


            int j = 0;
            ConstructorInfo interceptedParameterCtor = typeof (InterceptedParameter).GetConstructors()[0];
            MethodInfo arrayListAddMethod = typeof (ArrayList).GetMethod("Add");
            MethodInfo getTypeMethod = typeof (Type).GetMethod("GetType", new Type[1] {typeof (string)});

            foreach (ParameterInfo parameter in parameterInfos)
            {
                il.Emit(OpCodes.Ldloc, paramList);
                string paramName = parameter.Name;
                if (paramName == null)
                {
                    paramName = "param" + j.ToString();
                }
                il.Emit(OpCodes.Ldstr, paramName);
                il.Emit(OpCodes.Ldc_I4, j);
                il.Emit(OpCodes.Ldstr, parameter.ParameterType.FullName.Replace("&", ""));
                il.Emit(OpCodes.Call, getTypeMethod);


                il.Emit(OpCodes.Ldarg, j + 1);

                if (parameter.ParameterType.FullName.IndexOf("&") >= 0)
                {
                    il.Emit(OpCodes.Ldind_Ref);
                    Type t = Type.GetType(parameter.ParameterType.FullName.Replace("&", ""));
                    if (t.IsValueType)
                        il.Emit(OpCodes.Box, t);
                }
                if (parameter.ParameterType.IsValueType)
                {
                    il.Emit(OpCodes.Box, parameter.ParameterType);
                }
                il.Emit(OpCodes.Ldc_I4, (int) ParameterType.ByVal);
                il.Emit(OpCodes.Newobj, interceptedParameterCtor);
                il.Emit(OpCodes.Callvirt, arrayListAddMethod);
                il.Emit(OpCodes.Pop);


                j++;
            }            

#if NET2
            CallInfo callInfo = new CallInfo(wrapperName,method, new ArrayList(), FastCall.GetMethodInvoker(method));
#else
			CallInfo callInfo = new CallInfo(wrapperName,method, new ArrayList());
#endif

            MethodInfo handleCallMethod = typeof(IAopProxy).GetMethod("HandleFastCall");
            int methodNr = MethodCache.AddCallInfo(callInfo, wrapperName);
            //il.Emit(OpCodes.Ldc_I4 ,methodNr);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, methodNr);
            il.Emit(OpCodes.Ldloc, paramList);
            il.Emit(OpCodes.Ldstr, method.ReturnType.FullName);
            il.Emit(OpCodes.Call, getTypeMethod);
            il.Emit(OpCodes.Callvirt, handleCallMethod);
            if (method.ReturnType == typeof (void))
            {
                il.Emit(OpCodes.Pop);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Unbox, method.ReturnType);
                il.Emit(OpCodes.Ldobj, method.ReturnType);
            }


            j = 0;
            MethodInfo get_ItemMethod = typeof (ArrayList).GetMethod("get_Item", new Type[1] {typeof (int)});
            foreach (ParameterInfo parameter in parameterInfos)
            {
                if (parameter.ParameterType.FullName.IndexOf("&") >= 0)
                {
                    il.Emit(OpCodes.Ldarg, j + 1);
                    il.Emit(OpCodes.Ldloc, paramList);
                    il.Emit(OpCodes.Ldc_I4, j);
                    il.Emit(OpCodes.Callvirt, get_ItemMethod);
                    il.Emit(OpCodes.Castclass, typeof (InterceptedParameter));
                    FieldInfo valueField = typeof (InterceptedParameter).GetField("Value");
                    il.Emit(OpCodes.Ldfld, valueField);
                    Type t = Type.GetType(parameter.ParameterType.FullName.Replace("&", ""));
                    if (t.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox, t);
                        il.Emit(OpCodes.Ldobj, t);
                        il.Emit(OpCodes.Stobj, t);
                    }
                    else
                    {
                        il.Emit(OpCodes.Castclass, t);
                        il.Emit(OpCodes.Stind_Ref);
                    }
                }
                j++;
            }


            il.Emit(OpCodes.Ret);

            BuildWrapperMethod(wrapperName, typeBuilder, method);
        }

        private void BuildWrapperMethod(string wrapperName, TypeBuilder typeBuilder, MethodBase method)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            Type[] parameterTypes = new Type[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
                parameterTypes[i] = parameterInfos[i].ParameterType;

            Type returnType = null;
            if (method is MethodInfo)
                returnType = ((MethodInfo) method).ReturnType;

            MethodBuilder methodBuilder =
                typeBuilder.DefineMethod(wrapperName, MethodAttributes.Private, CallingConventions.Standard, returnType,
                                         parameterTypes);
            methodBuilder.SetCustomAttribute(DebuggerStepThroughBuilder());
            methodBuilder.SetCustomAttribute(DebuggerHiddenBuilder());

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                methodBuilder.DefineParameter(i + 1, parameterInfos[i].Attributes, parameterInfos[i].Name);
            }

            ILGenerator il = methodBuilder.GetILGenerator();
//			il.EmitWriteLine("enter "  + wrapperName) ;

            if (IsExplicitInterfaceMethod(method))
            {
                BuildWrapperExplicitInterfaceMethod(method, il, wrapperName);
            }
            else
            {
                BuildWrapperVirtualMethod(il, parameterInfos, method);
            }
        }

        private static void BuildWrapperVirtualMethod(ILGenerator il, ParameterInfo[] parameterInfos, MethodBase method)
        {
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < parameterInfos.Length; i++)
                il.Emit(OpCodes.Ldarg, i + 1);

            if (method is MethodInfo)
            {
                il.Emit(OpCodes.Call, (MethodInfo) method);
            }
            if (method is ConstructorInfo)
            {
                il.Emit(OpCodes.Call, (ConstructorInfo) method);
            }

            il.Emit(OpCodes.Ret);
        }

        private static void BuildWrapperExplicitInterfaceMethod(MethodBase method, ILGenerator il, string wrapperName)
        {
            MethodInfo explicitMethod = (MethodInfo) method;

            ParameterInfo[] parameters = method.GetParameters();
            MethodInfo invokeMethod =
                typeof (MethodInfo).GetMethod("Invoke", new Type[] {typeof (object), typeof (object[])});
            LocalBuilder methodLocal = il.DeclareLocal(typeof (MethodInfo));
            LocalBuilder parametersLocal = il.DeclareLocal(typeof (object[]));
            LocalBuilder resultLocal = il.DeclareLocal(typeof (object));

            il.Emit(OpCodes.Ldc_I4, parameters.Length);
            il.Emit(OpCodes.Newarr, typeof (object));
            il.Emit(OpCodes.Stloc, parametersLocal);
            il.Emit(OpCodes.Ldloc, parametersLocal);
            //
            //				//			//fill param array
            for (int i = 0; i < parameters.Length; i++)
            {
                il.Emit(OpCodes.Ldc_I4, i); //load index
                il.Emit(OpCodes.Ldarg, i + 1); //load param i+1
                il.Emit(OpCodes.Stelem_Ref); //store at index
            }

            if (parameters.Length == 0)
                il.Emit(OpCodes.Pop);

            il.Emit(OpCodes.Ldstr, wrapperName);
            il.Emit(OpCodes.Call, MethodCache.GetMethodMethodInfo);
            il.Emit(OpCodes.Stloc, methodLocal);


            il.Emit(OpCodes.Ldloc, methodLocal);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldloc, parametersLocal);
            il.Emit(OpCodes.Callvirt, invokeMethod);
            il.Emit(OpCodes.Stloc, resultLocal);

            if (explicitMethod.ReturnType != Type.GetType("System.Void"))
            {
                il.Emit(OpCodes.Ldloc, resultLocal);
                if (explicitMethod.ReturnType.IsValueType)
                {
                    il.Emit(OpCodes.Unbox, explicitMethod.ReturnType);
                    il.Emit(OpCodes.Ldobj, explicitMethod.ReturnType);
                }
            }

            il.Emit(OpCodes.Ret);
        }

        private static bool IsExplicitInterfaceMethod(MethodBase method)
        {
            return method.IsPrivate;
        }

        private FieldBuilder GetMixinField(Type mixinType)
        {
            if (!mixinType.IsInterface)
                mixinType = mixinType.GetInterfaces()[0];

            string mixinName = mixinType.Name + "Mixin";

            return mixinFieldLookup[mixinName] as FieldBuilder;
        }

        private Hashtable mixinFieldLookup = new Hashtable();

        private void BuildMixinFields(TypeBuilder typeBuilder, IList mixins)
        {
            foreach (Type mixinType in mixins)
            {
                if (mixinType.IsInterface)
                {
                }
                else
                {
                    Type mixinInterface = mixinType.GetInterfaces()[0];

                    string mixinName = mixinInterface.Name + "Mixin";
                    FieldBuilder mixinField =
                        typeBuilder.DefineField(mixinName, mixinInterface, FieldAttributes.Private);
                    mixinFieldLookup[mixinName] = mixinField;
                }
            }
        }

        private static Type[] GetInterfaces(Type baseType, IList mixins)
        {
            Type[] mixinInterfaces = GetMixinInterfaces(mixins);
            Type[] baseInterfaces = baseType.GetInterfaces();

            Type[] interfaces = new Type[mixinInterfaces.Length + baseInterfaces.Length];
            Array.Copy(mixinInterfaces, 0, interfaces, 0, mixinInterfaces.Length);
            Array.Copy(baseInterfaces, 0, interfaces, mixinInterfaces.Length, baseInterfaces.Length);

            return interfaces;
        }

        private static Type[] GetMixinInterfaces(IList mixins)
        {
            Type[] mixinInterfaces = new Type[mixins.Count];
            for (int i = 0; i < mixins.Count; i++)
            {
                Type mixin = mixins[i] as Type;
                if (mixin.IsInterface)
                {
                    mixinInterfaces[i] = mixin;
                }
                else
                {
                    Type mixinInterface = mixin.GetInterfaces()[0];
                    mixinInterfaces[i] = mixinInterface;
                }
            }

            return mixinInterfaces;
        }

        private static TypeBuilder GetTypeBuilder(AssemblyBuilder assemblyBuilder, string moduleName, string typeName,
                                                  Type baseType, Type[] interfaces)
        {
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(moduleName);
            TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Public;
            return moduleBuilder.DefineType(typeName, typeAttributes, baseType, interfaces);
        }

        private void MixinType(TypeBuilder typeBuilder, Type mixinInterfaceType, FieldBuilder mixinField, IList aspects)
        {
            bool pointcut = true;
            MethodInfo[] methods = mixinInterfaceType.GetMethods();

            if (mixinInterfaceType == typeof (IAopProxy))
                pointcut = false;

#if NET2
            if (mixinInterfaceType.Name.Contains("ISerializableProxy"))
                pointcut = false;
#endif

            BuildMixinMethods(methods, typeBuilder, mixinField, aspects, pointcut);

            Type[] inheritedInterfaces = mixinInterfaceType.GetInterfaces();
            foreach (Type inheritedInterface in  inheritedInterfaces)
            {
                MixinType(typeBuilder, inheritedInterface, mixinField, aspects);
            }
        }

        private void BuildMixinMethods(MethodInfo[] methods, TypeBuilder typeBuilder, FieldBuilder mixinField,
                                       IList aspects, bool pointcut)
        {
            foreach (MethodInfo method in methods)
            {
                if (pointcut &&
                    (method.IsVirtual && !method.IsFinal &&
                     engine.PointCutMatcher.MethodShouldBeProxied(method, aspects)))
                {
                    BuildMixinMethod(typeBuilder, method, mixinField);
                }
                else
                {
                    BuildMixinWrapperMethod(method.Name, typeBuilder, method, mixinField);
                }
            }
        }

        private void BuildMixinMethod(TypeBuilder typeBuilder, MethodInfo method, FieldBuilder field)
        {
            if (method.DeclaringType == typeof (IAopProxy))
            {
                BuildMixinWrapperMethod(method.Name, typeBuilder, method, field);
                return;
            }


            string wrapperName = GetMethodId(method.Name);
            wrapperMethods.Add(wrapperName);
            MethodCache.methodLookup[wrapperName] = method;
            //		MethodCache.wrapperMethodLookup[wrapperName] = method;

            ParameterInfo[] parameterInfos = method.GetParameters();
            Type[] parameterTypes = new Type[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
                parameterTypes[i] = parameterInfos[i].ParameterType;

            MethodBuilder methodBuilder =
                typeBuilder.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.Virtual,
                                         CallingConventions.Standard, method.ReturnType, parameterTypes);
            methodBuilder.SetCustomAttribute(DebuggerStepThroughBuilder());
            methodBuilder.SetCustomAttribute(DebuggerHiddenBuilder());

            for (int i = 0; i < parameterInfos.Length; i++)
            {
            }

            ILGenerator il = methodBuilder.GetILGenerator();

            LocalBuilder paramList = il.DeclareLocal(typeof (ArrayList));


            //create param arraylist
            ConstructorInfo arrayListCtor = typeof (ArrayList).GetConstructor(new Type[0]);
            il.Emit(OpCodes.Newobj, arrayListCtor);
            il.Emit(OpCodes.Stloc, paramList);


            int j = 0;
            ConstructorInfo interceptedParameterCtor = typeof (InterceptedParameter).GetConstructors()[0];
            MethodInfo arrayListAddMethod = typeof (ArrayList).GetMethod("Add");
            MethodInfo getTypeMethod = typeof (Type).GetMethod("GetType", new Type[1] {typeof (string)});

            foreach (ParameterInfo parameter in parameterInfos)
            {
                il.Emit(OpCodes.Ldloc, paramList);
                string paramName = parameter.Name;
                if (paramName == null)
                {
                    paramName = "param" + j.ToString();
                }
                il.Emit(OpCodes.Ldstr, paramName);
                il.Emit(OpCodes.Ldc_I4, j);
                il.Emit(OpCodes.Ldstr, parameter.ParameterType.FullName.Replace("&", ""));
                il.Emit(OpCodes.Call, getTypeMethod);


                il.Emit(OpCodes.Ldarg, j + 1);

                if (parameter.ParameterType.FullName.IndexOf("&") >= 0)
                {
                    il.Emit(OpCodes.Ldind_Ref);
                    Type t = Type.GetType(parameter.ParameterType.FullName.Replace("&", ""));
                    if (t.IsValueType)
                        il.Emit(OpCodes.Box, t);
                }
                if (parameter.ParameterType.IsValueType)
                {
                    il.Emit(OpCodes.Box, parameter.ParameterType);
                }
                il.Emit(OpCodes.Ldc_I4, (int) ParameterType.ByVal);
                il.Emit(OpCodes.Newobj, interceptedParameterCtor);
                il.Emit(OpCodes.Callvirt, arrayListAddMethod);
                il.Emit(OpCodes.Pop);


                j++;
            }

      
#if NET2
            CallInfo callInfo = new CallInfo(wrapperName,method, new ArrayList(), FastCall.GetMethodInvoker(method));
#else
			CallInfo callInfo = new CallInfo(wrapperName,method, new ArrayList());
#endif

            MethodInfo handleCallMethod = typeof (IAopProxy).GetMethod("HandleFastCall");
            int methodNr = MethodCache.AddCallInfo(callInfo,wrapperName);
            //il.Emit(OpCodes.Ldc_I4 ,methodNr);

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field); // set the execution target to the mixin instance
            il.Emit(OpCodes.Ldc_I4 ,methodNr);
            il.Emit(OpCodes.Ldloc, paramList);
            il.Emit(OpCodes.Ldstr, method.ReturnType.FullName);
            il.Emit(OpCodes.Call, getTypeMethod);
            il.Emit(OpCodes.Callvirt, handleCallMethod);
            if (method.ReturnType == typeof (void))
            {
                il.Emit(OpCodes.Pop);
            }
            else if (method.ReturnType.IsValueType)
            {
                il.Emit(OpCodes.Unbox, method.ReturnType);
                il.Emit(OpCodes.Ldobj, method.ReturnType);
            }


            j = 0;
            MethodInfo get_ItemMethod = typeof (ArrayList).GetMethod("get_Item", new Type[1] {typeof (int)});
            foreach (ParameterInfo parameter in parameterInfos)
            {
                if (parameter.ParameterType.FullName.IndexOf("&") >= 0)
                {
                    il.Emit(OpCodes.Ldarg, j + 1);
                    il.Emit(OpCodes.Ldloc, paramList);
                    il.Emit(OpCodes.Ldc_I4, j);
                    il.Emit(OpCodes.Callvirt, get_ItemMethod);
                    il.Emit(OpCodes.Castclass, typeof (InterceptedParameter));
                    FieldInfo valueField = typeof (InterceptedParameter).GetField("Value");
                    il.Emit(OpCodes.Ldfld, valueField);
                    Type t = Type.GetType(parameter.ParameterType.FullName.Replace("&", ""));
                    if (t.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox, t);
                        il.Emit(OpCodes.Ldobj, t);
                        il.Emit(OpCodes.Stobj, t);
                    }
                    else
                    {
                        il.Emit(OpCodes.Castclass, t);
                        il.Emit(OpCodes.Stind_Ref);
                    }
                }
                j++;
            }


            il.Emit(OpCodes.Ret);


            BuildMixinWrapperMethod(wrapperName, typeBuilder, method, field);
        }

        private void BuildMixinWrapperMethod(string wrapperName, TypeBuilder typeBuilder, MethodInfo method,
                                             FieldBuilder field)
        {
            ParameterInfo[] parameterInfos = method.GetParameters();
            Type[] parameterTypes = new Type[parameterInfos.Length];
            for (int i = 0; i < parameterInfos.Length; i++)
                parameterTypes[i] = parameterInfos[i].ParameterType;

            MethodBuilder methodBuilder =
                typeBuilder.DefineMethod(wrapperName, MethodAttributes.Public | MethodAttributes.Virtual,
                                         CallingConventions.Standard, method.ReturnType, parameterTypes);
            methodBuilder.SetCustomAttribute(DebuggerStepThroughBuilder());
            methodBuilder.SetCustomAttribute(DebuggerHiddenBuilder());

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                methodBuilder.DefineParameter(i + 1, parameterInfos[i].Attributes, parameterInfos[i].Name);
            }

            ILGenerator il = methodBuilder.GetILGenerator();


            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, field);
            for (int i = 0; i < parameterInfos.Length; i++)
                il.Emit(OpCodes.Ldarg, i + 1);

            il.Emit(OpCodes.Callvirt, method);
            //	il.EmitWriteLine(method.Name) ;
            il.Emit(OpCodes.Ret);
        }

        private void BuildConstructors(Type baseType, TypeBuilder typeBuilder, IList mixins)
        {
            ConstructorInfo[] constructors = baseType.GetConstructors();
            foreach (ConstructorInfo constructor in constructors)
            {
                BuildConstructor(constructor, typeBuilder, mixins);
            }
            if (constructors.Length == 0)
            {
                constructors = typeof (object).GetConstructors();
                foreach (ConstructorInfo constructor in constructors)
                {
                    BuildConstructor(constructor, typeBuilder, mixins);
                }
            }
        }

#if NET2
        private CustomAttributeBuilder DebuggerVisualizerBuilder()
        {
            Type t = typeof (DebuggerVisualizerAttribute);
            ConstructorInfo ci = t.GetConstructor(new Type[] {typeof (string), typeof (string)});
            CustomAttributeBuilder cb = new CustomAttributeBuilder(ci, new object[]
                                                                           {
                                                                               "Puzzle.NAspect.Debug.AopProxyVisualizer, Puzzle.Naspect.Debug, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a8e5914f83beaab3"
                                                                               ,
                                                                               "Puzzle.NAspect.Debug.AopProxyObjectSource, Puzzle.Naspect.Debug, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a8e5914f83beaab3"
                                                                           },
                                                                   new PropertyInfo[]
                                                                       {
                                                                           typeof (DebuggerVisualizerAttribute).
                                                                               GetProperty("Description")
                                                                       },
                                                                   new object[] {"Aop Visualizer"});

            return cb;
        }
#endif

        private CustomAttributeBuilder DebuggerStepThroughBuilder()
        {
            Type t = typeof (DebuggerStepThroughAttribute);
            ConstructorInfo ci = t.GetConstructor(new Type[] {});
            CustomAttributeBuilder cb = new CustomAttributeBuilder(ci, new object[] {});
            return cb;
        }

        private CustomAttributeBuilder DebuggerHiddenBuilder()
        {
            Type t = typeof (DebuggerHiddenAttribute);
            ConstructorInfo ci = t.GetConstructor(new Type[] {});
            CustomAttributeBuilder cb = new CustomAttributeBuilder(ci, new object[] {});
            return cb;
        }

        private void BuildConstructor(ConstructorInfo constructor, TypeBuilder typeBuilder, IList mixins)
        {
            string wrapperName = GetMethodId(constructor.Name);
            wrapperMethods.Add(wrapperName);
            MethodCache.methodLookup[wrapperName] = constructor;

            ParameterInfo[] parameters = constructor.GetParameters();

            //make proxy ctor param count same as superclass
            Type[] parameterTypes = new Type[parameters.Length + 1];

            //copy super ctor param types
            for (int i = 0; i <= parameters.Length - 1; i++)
            {
                parameterTypes[i + 1] = parameters[i].ParameterType;
            }
            parameterTypes[0] = typeof (object);


            ConstructorBuilder proxyConstructor =
                typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);
            ILGenerator il = proxyConstructor.GetILGenerator();

            proxyConstructor.SetCustomAttribute(DebuggerStepThroughBuilder());
            proxyConstructor.SetCustomAttribute(DebuggerHiddenBuilder());

            foreach (Type mixinType in mixins)
            {
                if (mixinType.IsInterface)
                {
                    //ignore interface type mixins , they do not have an impelemntation
                }
                else
                {
                    //				il.EmitWriteLine("setting mixin instance " + mixinType.FullName) ;
                    il.Emit(OpCodes.Ldarg_0);
                    ConstructorInfo mixinCtor = (mixinType).GetConstructor(new Type[] {});
                    il.Emit(OpCodes.Newobj, mixinCtor);
                    il.Emit(OpCodes.Stfld, GetMixinField(mixinType));
                }
            }

            //associate iproxyaware mixins with this instance
            MethodInfo setProxyMethod = typeof (IProxyAware).GetMethod("SetProxy");
            foreach (Type mixinType in mixins)
            {
                if (mixinType.IsInterface)
                {
                    //ignore interface type mixins , they do not have an impelemntation
                }
                else
                {
                    if (typeof (IProxyAware).IsAssignableFrom(mixinType))
                    {
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldfld, GetMixinField(mixinType));
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Callvirt, setProxyMethod);
                    }
                }
            }

            //--------------------------
            LocalBuilder paramList = il.DeclareLocal(typeof (ArrayList));


            //create param arraylist
            ConstructorInfo arrayListCtor = typeof (ArrayList).GetConstructor(new Type[0]);
            il.Emit(OpCodes.Newobj, arrayListCtor);
            il.Emit(OpCodes.Stloc, paramList);


            ConstructorInfo interceptedParameterCtor = typeof (InterceptedParameter).GetConstructors()[0];
            MethodInfo arrayListAddMethod = typeof (ArrayList).GetMethod("Add");
            MethodInfo getTypeMethod = typeof (Type).GetMethod("GetType", new Type[1] {typeof (string)});

            il.Emit(OpCodes.Ldloc, paramList);

            il.Emit(OpCodes.Ldstr, "_state");
            il.Emit(OpCodes.Ldc_I4, 0);
            il.Emit(OpCodes.Ldstr, typeof (object).FullName);
            il.Emit(OpCodes.Call, getTypeMethod);

            il.Emit(OpCodes.Ldarg, 0 + 1);

            il.Emit(OpCodes.Ldc_I4, (int) ParameterType.ByVal);
            il.Emit(OpCodes.Newobj, interceptedParameterCtor);
            il.Emit(OpCodes.Callvirt, arrayListAddMethod);
            il.Emit(OpCodes.Pop);


            int j = 1;
            foreach (ParameterInfo parameter in parameters)
            {
                il.Emit(OpCodes.Ldloc, paramList);
                string paramName = parameter.Name;
                if (paramName == null)
                {
                    paramName = "param" + j.ToString();
                }
                il.Emit(OpCodes.Ldstr, paramName);
                il.Emit(OpCodes.Ldc_I4, j);
                il.Emit(OpCodes.Ldstr, parameter.ParameterType.FullName.Replace("&", ""));
                il.Emit(OpCodes.Call, getTypeMethod);


                il.Emit(OpCodes.Ldarg, j + 1);

                if (parameter.ParameterType.FullName.IndexOf("&") >= 0)
                {
                    il.Emit(OpCodes.Ldind_Ref);
                    Type t = Type.GetType(parameter.ParameterType.FullName.Replace("&", ""));
                    if (t.IsValueType)
                        il.Emit(OpCodes.Box, t);
                }
                if (parameter.ParameterType.IsValueType)
                {
                    il.Emit(OpCodes.Box, parameter.ParameterType);
                }
                il.Emit(OpCodes.Ldc_I4, (int) ParameterType.ByVal);
                il.Emit(OpCodes.Newobj, interceptedParameterCtor);
                il.Emit(OpCodes.Callvirt, arrayListAddMethod);
                il.Emit(OpCodes.Pop);


                j++;
            }

#if NET2
            CallInfo callInfo = new CallInfo(wrapperName,constructor, new ArrayList(), FastCall.GetMethodInvoker(constructor));
#else
			CallInfo callInfo = new CallInfo(wrapperName,constructor, new ArrayList());
#endif

            MethodInfo handleCallMethod = typeof(IAopProxy).GetMethod("HandleFastCall");
            int methodNr = MethodCache.AddCallInfo(callInfo, wrapperName);
            //il.Emit(OpCodes.Ldc_I4 ,methodNr);


            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldc_I4, methodNr);
            il.Emit(OpCodes.Ldloc, paramList);
            il.Emit(OpCodes.Ldstr, typeof (void).FullName);
            il.Emit(OpCodes.Call, getTypeMethod);
            il.Emit(OpCodes.Callvirt, handleCallMethod);
            il.Emit(OpCodes.Pop);


            j = 1;
            MethodInfo get_ItemMethod = typeof (ArrayList).GetMethod("get_Item", new Type[1] {typeof (int)});
            foreach (ParameterInfo parameter in parameters)
            {
                if (parameter.ParameterType.FullName.IndexOf("&") >= 0)
                {
                    il.Emit(OpCodes.Ldarg, j + 1);
                    il.Emit(OpCodes.Ldloc, paramList);
                    il.Emit(OpCodes.Ldc_I4, j);
                    il.Emit(OpCodes.Callvirt, get_ItemMethod);
                    il.Emit(OpCodes.Castclass, typeof (InterceptedParameter));
                    FieldInfo valueField = typeof (InterceptedParameter).GetField("Value");
                    il.Emit(OpCodes.Ldfld, valueField);
                    Type t = Type.GetType(parameter.ParameterType.FullName.Replace("&", ""));
                    if (t.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox, t);
                        il.Emit(OpCodes.Ldobj, t);
                        il.Emit(OpCodes.Stobj, t);
                    }
                    else
                    {
                        il.Emit(OpCodes.Castclass, t);
                        il.Emit(OpCodes.Stind_Ref);
                    }
                }
                j++;
            }

            il.Emit(OpCodes.Ret);
            //--------------

            BuildWrapperMethod(wrapperName, typeBuilder, constructor);
        }
    }
}