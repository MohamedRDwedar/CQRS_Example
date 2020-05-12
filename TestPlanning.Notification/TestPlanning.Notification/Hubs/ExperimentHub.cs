using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TestPlanning.Experiment.Models;

namespace TestPlanning.Notification.Hubs
{
    public class ExperimentHub : Hub
    {
        // https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio

        public async Task ExperimentCreatedMessage(ExperimentModel experiment)
        {
            await Clients.All.SendAsync("ExperimentCreated", experiment);
        }

        public async Task ExperimentUpadatedMessage(ExperimentModel experiment)
        {
            await Clients.All.SendAsync("ExperimentUpadated", experiment);
        }

        public async Task ExperimentDeletedMessage(long experimentId)
        {
            await Clients.All.SendAsync("ExperimentDeleted", experimentId);
        }
    }
}
