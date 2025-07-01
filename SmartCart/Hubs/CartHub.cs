using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SmartCart.API.Hubs
{
    public class CartHub : Hub
    {
        public async Task JoinCartGroup (string cartId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, cartId);
        }
    }
}
