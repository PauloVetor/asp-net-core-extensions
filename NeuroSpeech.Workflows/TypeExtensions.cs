﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuroSpeech.Workflows
{
    internal static class TypeExtensions
    {

        public static (Type t1, Type t2) Get2GenericArguments(this Type type)
        {
            var bt = type;
            while (!bt.IsGenericType || bt.GetGenericArguments().Length != 2)
                bt = bt.BaseType;
            var ta = bt.GetGenericArguments();
            return (ta[0], ta[1]);
        }

        public static Type GetArgument(this Type type, Type genericType)
        {
            while (!type.IsConstructedGenericType)
            {
                type = type.BaseType;
                if (type == null)
                    break;
            }
            if(type == null || type.GetGenericTypeDefinition() != genericType)
                throw new InvalidOperationException($"{type} does not construct {genericType}");
            return type.GetGenericArguments()[0];
        }

        public static string GetFriendlyName(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetFriendlyName() + "[]";
            }

            if(type.IsConstructedGenericType)
            {
                return type.GetGenericTypeDefinition().GetFriendlyName() + "<" + 
                    string.Join(",", type.GetGenericArguments().Select(x => x.GetFriendlyName())) + ">";
            }

            return type.Namespace + "." + type.Name;
        }

    }
}
