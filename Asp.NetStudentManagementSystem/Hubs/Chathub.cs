using Asp.NetStudentManagementSystem.DAL;
using Asp.NetStudentManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Asp.NetStudentManagementSystem.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        Random random = new Random();
        UserManager<AppUser> _userManager { get; }
        AppDbContext _context { get; }
        public ChatHub(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendMessageToGroup(string sendername, string sender, string receiver, string message)
        {
            int r = random.Next(0, 1000000);
            var senderUser = await _userManager.FindByEmailAsync(sender);
            var receiverUser = await _userManager.FindByEmailAsync(receiver);

            _context.Messages.Add(new() { Id = r, Content = message });
            _context.UserMessages.Add(new() { SenderId = senderUser.Id, ReceiverId = receiverUser.Id, MessageId = r, SentDate = DateTime.Now });
            _context.SaveChanges();
            await Clients.Group(receiver).SendAsync("ReceiveMessage", sender, sendername, message);
            await Clients.Group(sender).SendAsync("ReceiveMessage", sender, sendername, message);
        }
    }
}
