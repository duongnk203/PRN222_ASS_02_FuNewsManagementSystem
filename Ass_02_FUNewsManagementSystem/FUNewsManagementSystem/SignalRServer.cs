using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem
{
    public class SignalRServer : Hub
    {
        public async Task SendCommentsUpdate(int newsArticleId)
        {
            await Clients.Group(newsArticleId.ToString()).SendAsync("ReceiveCommentsUpdate", newsArticleId);
        }

    }
}
