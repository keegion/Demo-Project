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
       
   
        
        public async Task SendMessage(string user, string time, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", user, time, message, ConnectedUsers);
        }

        public async Task OnlineUsers()
        {

            await Clients.All.SendAsync("Online",ConnectedUsers);
        }


        public void AddUser(string user)
        {
            if (!ConnectedUsers.Contains(user)){
                ConnectedUsers.Add(user);
            }
           
            
        }
        public void RemoveUser(string user)
        {
          
                ConnectedUsers.Remove(user);
            
        }
    }
}