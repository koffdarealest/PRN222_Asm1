using Microsoft.AspNetCore.SignalR;

namespace PRN222.Assignment2.Hubs
{
    public class EventHub : Hub
    {
        public async Task NotifyAttendeeCountUpdated(int eventId, int count)
        {
            await Clients.All.SendAsync("ReceiveAttendeeCountUpdated", eventId, count);
        }

        public async Task SendEventUpdated(int eventId, string title, string description, string startTime,
            string endTime, string location, int categoryId)
        {
            await Clients.All.SendAsync("ReceiveEventUpdated", eventId, title, description, startTime, endTime, location, categoryId);
        }
        public async Task SendEventDeleted(int eventId)
        {
            await Clients.All.SendAsync("ReceiveEventDeleted", eventId);
        }
    }
}
