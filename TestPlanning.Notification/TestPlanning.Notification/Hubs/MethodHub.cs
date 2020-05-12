using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TestPlanning.Method.Models;

namespace TestPlanning.Notification.Hubs
{
    public class MethodHub : Hub
    {
        public async Task MethodCreatedMessage(MethodModel method)
        {
            await Clients.All.SendAsync("MethodCreated", method);
        }

        public async Task MethodUpadatedMessage(MethodModel method)
        {
            await Clients.All.SendAsync("MethodUpadated", method);
        }

        public async Task MethodDeletedMessage(long methodId)
        {
            await Clients.All.SendAsync("MethodDeleted", methodId);
        }
    }
}
