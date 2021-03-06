﻿using MessagePack.Formatters;
using System;
using System.Reflection;

namespace MessagePack.Resolvers
{
    /// <summary>
    /// Get formatter from [MessaegPackFromatter] attribute.
    /// </summary>
    public sealed class AttributeFormatterResolver : IFormatterResolver
    {
        public static IFormatterResolver Instance = new AttributeFormatterResolver();

        AttributeFormatterResolver()
        {

        }

        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var attr = typeof(T).GetTypeInfo().GetCustomAttribute<MessagePackFormatterAttribute>();
                if (attr == null)
                {
                    return;
                }

                if (attr.Arguments == null)
                {
                    formatter = (IMessagePackFormatter<T>)Activator.CreateInstance(attr.FormatterType);
                }
                else
                {
                    formatter = (IMessagePackFormatter<T>)Activator.CreateInstance(attr.FormatterType, attr.Arguments);
                }
            }
        }
    }
}