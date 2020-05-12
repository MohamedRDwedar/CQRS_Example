using System;
using System.Threading.Tasks;
using TestPlanning.Common.Helpers;
using TestPlanning.Common.Interfaces;
using TestPlanning.Common.Models;
using TestPlanning.Common.Wrappers;
using TestPlanning.Method.CommandHandlers;
using TestPlanning.Method.Context;
//using TestPlanning.Method.Events;
//using TestPlanning.Method.Events;

namespace TestPlanning.Method.CommandSubscribers
{
    public class AddMethodCommandSubscriber : ICommandSubscriber<AddMethodCommand>
    {
        private readonly ConumerWrapper _conumerWrapper;
        private MethodContext _context;
        private readonly Messages _messages;

        public AddMethodCommandSubscriber(ConumerWrapper conumerWrapper, MethodContext context, Messages messages)
        {
            _conumerWrapper = conumerWrapper;
            _context = context;
            _messages = messages;
        }

        public void Handle()
        {
            _conumerWrapper.ConumeMessage<AddMethodCommand>("AddMethodCommand", (messageValue) =>
            {
                try
                {
                    var method = new MethodModel
                    {
                        Id = 0,
                        Name = messageValue.Name,
                        TimeStamp = messageValue.TimeStamp
                    };
                    _context = new MethodContext();
                    _context.Methods.Add(method);
                    var saveResult = _context.SaveChanges();
                    if (saveResult > 0)
                    {
                        Task.Factory.StartNew(async () =>
                        {
                            //await _messages.Dispatch(new MethodAddedEvent(method.Id, method.Name));
                        });
                    }
                }
                catch (Exception exception)
                {

                }
            });
        }
    }
}
