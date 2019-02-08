using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using DemoProject.Models;
using System;
using System.Linq;


namespace DemoProject.Hubs
{
    public class ChatHub : Hub
    {
        static List<ConnectedAcc> ConnectedUsers = new List<ConnectedAcc>();
        static List<Messages> AllMessages = new List<Messages>();


        //Creates new user to ConnectedUsers list, loads userlist/messages
        public async Task Connect(string user, string path)
        {

            var id = Context.ConnectionId;

            if (ConnectedUsers.SingleOrDefault(x => x.Username == user) == null)
            {

                if (path == "" || path == null)
                {
                    path = "images/test.png";
                }

                ConnectedUsers.Add(new ConnectedAcc { Username = user, ID = id, IMG = path });
                await Clients.AllExcept(id).SendAsync("Join", user);
                await Clients.Caller.SendAsync("Messages", AllMessages);

            }
            await Clients.All.SendAsync("Online", ConnectedUsers);


        }
        //Sends chat message to all clients and adds it to message list
        public async Task SendMessage(string user, string time, string message)
        {
            ConnectedAcc acc = ConnectedUsers.SingleOrDefault(x => x.Username == user);
            string UserImg = acc.IMG;
            AllMessages.Add(new Messages { Username = user, Message = message, Time = time, IMG = UserImg });

            await Clients.All.SendAsync("ReceiveMessage", user, time, message, UserImg);
        }





        //On Disconnect removes username from list and sends disconnect msg to other clients.
        public override Task OnDisconnectedAsync(Exception exception)
        {

            var item = ConnectedUsers.SingleOrDefault(x => x.ID == Context.ConnectionId);
            if (item != null)
            {

                ConnectedUsers.Remove(item);
                Clients.All.SendAsync("Disconnected", item.Username);
                Clients.All.SendAsync("Online", ConnectedUsers);
            }

            return base.OnDisconnectedAsync(exception);
        }



    }
}