﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ProjectSignalR.Data;
using System.Security.Claims;

namespace ProjectSignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _db;
        public ChatHub(ApplicationDbContext db)
        {
            //dependency injection : Constructor Injection
            _db = db;
        }

        public override Task OnConnectedAsync()
        {
            var UserId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!String.IsNullOrEmpty(UserId))
            {
                //we want to retrieve the email of the logged in user
                var userName = _db.Users.FirstOrDefault(u => u.Id == UserId).UserName;
                Clients.Users(HubConnections.OnlineUsers()).SendAsync("RecieveConnectedUser",UserId, userName);
                HubConnections.AddUserConnection(UserId,Context.ConnectionId);
            }
            return base.OnConnectedAsync();
        }
        //public async Task SendMessageToAll(string user, string message) 
        //{ 
        //    await Clients.All.SendAsync("MessageRecieved", user, message);
        //}
        //[Authorize]
        //public async Task SendMessageToReciever(string sender, string reciever, string message) 
        //{
        //    var userId = _db.Users.FirstOrDefault(u => u.Email.ToLower() == reciever.ToLower()).Id;

        //    if(!string.IsNullOrEmpty(userId))
        //    {
        //        await Clients.User(userId).SendAsync("MessageRecieved", sender, message);
        //    }
        //}
    }
}
