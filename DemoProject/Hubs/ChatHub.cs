using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using DemoProject.Models;
using Microsoft.AspNetCore.Http;
using System.Web;


namespace DemoProject.Hubs
{
    public class ChatHub : Hub
    {
        static List<string> ConnectedUsers = new List<string>();
       
   
        //Sends chat message to all clients
        public async Task SendMessage(string user, string time, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", user, time, message, ConnectedUsers);
        }
        //Updates the online users
        public async Task OnlineUsers()
        {

            await Clients.All.SendAsync("Online",ConnectedUsers);
        }

        //Add user to online user list
        public void AddUser(string user)
        {
            if (!ConnectedUsers.Contains(user)){
                ConnectedUsers.Add(user);
            }
           
        //Remove User from online user list
        }
        public void RemoveUser(string user)
        {
          
                ConnectedUsers.Remove(user);
            
        }
    }
}