using System;
using System.Threading.Tasks;
using TestPlanning.Common.Helpers;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Wrappers;
using TestPlanning.Method.CommandHandlers;
using TestPlanning.Method.Context;
using TestPlanning.Method.Events;

namespace TestPlanning.Method.CommandSubscribers
{
    public class DeleteMethodCommandSubscriber : ICommandSubscriber<DeleteMethodCommand>
    {
        private readonly ConumerWrapper _conumerWrapper;
        private readonly MethodContext _context;
        private readonly Messages _messages;
        public DeleteMethodCommandSubscriber(ConumerWrapper conumerWrapper, MethodContext context, Messages messages)
        {
            _conumerWrapper = conumerWrapper;
            _context = context;
            _messages = messages;
        }

        public void Handle()
        {
            _conumerWrapper.ConumeMessage<DeleteMethodCommand>("DeleteMethodCommand", (messageValue) =>
            {
                try
                {
                    var existMethod = _context.Methods.Find(messageValue.Id);
                    if (existMethod != null)
                    {
                        _context.Methods.Remove(existMethod);
                        var saveResult = _context.SaveChanges();
                        if (saveResult > 0)
                        {
                            Task.Factory.StartNew(async () =>
                            {
                                await _messages.Dispatch(new MethodDeletedEvent(existMethod.Id, existMethod.Name));
                            });
                        }
                    }
                }
                catch (Exception exception)
                {
                  
                }
            });
        }
    }
}
