using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem
{
    public class SignalRServer : Hub
    {
        public async Task SendCommentUpdate(int newsArticleId)
        {
            await Clients.All.SendAsync("ReceiveCommentUpdate", newsArticleId);
        }

        public async Task JoinGroup(int newsArticleId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, newsArticleId.ToString());
        }

        public async Task LeaveGroup(int newsArticleId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, newsArticleId.ToString());
        }
    }
}
