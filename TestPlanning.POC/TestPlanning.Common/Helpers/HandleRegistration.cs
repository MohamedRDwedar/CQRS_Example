using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestPlanning.Common.Interfaces;

namespace TestPlanning.Common
{
    // use https://simpleinjector.readthedocs.io/en/latest/index.html instead
    public static class HandlerRegistration
    {
        public static void AddHandlers(this IServiceCollection services)
        {
            var handlerTypes = typeof(ICommand).Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(y => IsHandlerInterface(y)))
                .Where(x => x.Name.EndsWith("Handler"))
                .ToList();

            foreach (Type type in handlerTypes)
            {
                AddHandler(services, type);
            }
        }

        private static void AddHandler(IServiceCollection services, Type type)
        {
            var attributes = type.GetCustomAttributes(false);

            var pipeline = attributes
                .Select(x => ToDecorator(x))
                .Concat(new[] { type })
                .Reverse()
                .ToList();

            var interfaceType = type.GetInterfaces().Single(y => IsHandlerInterface(y));
            var factory = BuildPipeline(pipeline, interfaceType);

            services.AddTransient(interfaceType, factory);
        }

        private static Func<IServiceProvider, object> BuildPipeline(List<Type> pipeline, Type interfaceType)
        {
            var ctors = pipeline
                .Select(x =>
                {
                    Type type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
                    return type.GetConstructors().Single();
                })
                .ToList();

            Func<IServiceProvider, object> func = provider =>
            {
                object current = null;

                foreach (ConstructorInfo ctor in ctors)
                {
                    List<ParameterInfo> parameterInfos = ctor.GetParameters().ToList();

                    object[] parameters = GetParameters(parameterInfos, current, provider);

                    current = ctor.Invoke(parameters);
                }

                return current;
            };

            return func;
        }

        private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
        {
            var result = new object[parameterInfos.Count];

            for (int i = 0; i < parameterInfos.Count; i++)
            {
                result[i] = GetParameter(parameterInfos[i], current, provider);
            }

            return result;
        }

        private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider provider)
        {
            var parameterType = parameterInfo.ParameterType;

            if (IsHandlerInterface(parameterType))
                return current;

            var service = provider.GetService(parameterType);
            if (service != null)
                return service;

            throw new ArgumentException($"Type {parameterType} not found");
        }

        private static Type ToDecorator(object attribute)
        {
            Type type = attribute.GetType();

            //if (type == typeof(DatabaseRetryAttribute))
            //    return typeof(DatabaseRetryDecorator<>);

            //if (type == typeof(AuditLogAttribute))
            //    return typeof(AuditLoggingDecorator<>);

            // other attributes go here

            throw new ArgumentException(attribute.ToString());
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(ICommandHandler<>) || typeDefinition == typeof(IEventHandler<>) || typeDefinition == typeof(IQueryHandler<,>);
        }
    }
}
