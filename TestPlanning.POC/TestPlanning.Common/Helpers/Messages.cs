using CSharpFunctionalExtensions;
using System;
using System.Threading.Tasks;
using TestPlanning.Common.Interfaces;

namespace TestPlanning.Common.Helpers
{
    public sealed class Messages
    {
        private IServiceProvider _serviceProvider;
        public Messages(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<Result> Dispatch(IEvent command)
        {
            try
            {
                var type = typeof(IEventHandler<>);
                Type[] typeArgs = { command.GetType() };
                var handlerType = type.MakeGenericType(typeArgs);

                dynamic handler = _serviceProvider.GetService(handlerType);
                Result result = await handler.HandleAsync((dynamic)command);

                return result;
            }
            catch (Exception exception)
            {
                return Result.Failure(exception.Message);
            }
        }

        public async Task<Result> Dispatch(ICommand command)
        {
            var type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            Result result = await handler.HandleAsync((dynamic)command);
            return result;
        }

        public void Subscribe(ICommand command)
        {
            var type = typeof(ICommandSubscriber<>);
            Type[] typeArgs = { command.GetType() };
            var handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            handler.Handle();
        }

        public Result<T> Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _serviceProvider.GetService(handlerType);
            Result<T> result = handler.Handle((dynamic)query);

            return result;
        }
    }
}
