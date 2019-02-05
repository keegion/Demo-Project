using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Generic;
using DemoProject.Models;
using Microsoft.AspNetCore.Http;
using System.Web;
using System;
using System.Linq;

namespace DemoProject.Hubs
{
    public class ChatHub : Hub
    {
        static List<ConnectedAcc> ConnectedUsers = new List<ConnectedAcc>();
        static List<Messages> AllMessages = new List<Messages>();


        //Creates new user to ConnectedUsers list, loads userlist/messages
        public async Task Connect(string user)
        {

            var id = Context.ConnectionId;
        
            if(ConnectedUsers.SingleOrDefault(x => x.Username == user)==null)
            {
                string UserImg = GetUserImage(user);
                ConnectedUsers.Add(new ConnectedAcc { Username = user, ID = id, IMG = UserImg});
                await Clients.AllExcept(id).SendAsync("Join", user);
                await Clients.Caller.SendAsync("Messages", AllMessages);

            }
            await Clients.All.SendAsync("Online", ConnectedUsers);
            

        }
        //Sends chat message to all clients and adds it to message list
        public async Task SendMessage(string user, string time, string message)
        {
            string UserImg = GetUserImage(user);
            AllMessages.Add(new Messages { Username = user, Message = message, Time = time, IMG = UserImg});

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
        //Get user image path from database
        public string GetUserImage(string username)
        {
            string ImgSRC = "images/test.png";
            try
            {
                DemoProjectContext DB = new DemoProjectContext();
                Accounts DBACC = DB.Accounts.Find(username);
                if(DBACC.ImgSRC =="" || DBACC.ImgSRC == null)
                {
                    ImgSRC = "images/test.png";
                }else
                {
                    ImgSRC = DBACC.ImgSRC;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ImgSRC;
        }

        public void UploadImgToDB(string path, string username)
        {
            if (username != null)
            {
                DemoProjectContext DB = new DemoProjectContext();
                Accounts DBACC = DB.Accounts.Find(username);
                if (DBACC.Username != null)
                {


                    DBACC.ImgSRC = path;
                   

                    DB.SaveChanges();
                }
            }
        }
    }
}