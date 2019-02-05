"use strict";
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


connection.start().catch(function (err) {
    return console.error(err.toString());
});

//call Load on page load which will call load user function
window.onload = Invoke();

//Load Online users one second after connected
function Invoke() {

    var user = document.getElementById("username").innerHTML;
    setTimeout(function () { connection.invoke("Connect", user); }, 500);

}
//recieve message
connection.on("ReceiveMessage", function (user, time, message) {

    var msg = '<b>' + user + '</b> ' + time + ' </br>' + message + '<hr>';
    $('#messagesList').append(msg);


    //scroll down automatically
    $("#messagesList").stop().animate({ scrollTop: $("#messagesList")[0].scrollHeight }, 1000);

});
//List online Users
connection.on("Online", function (connectedUsers) {

    $('#onlineUsers').html("<hr>");
    connectedUsers.forEach(function (user) {
        var userM = '<b>' + user.username + '<b><hr><br>'
        console.log(user);
        $('#onlineUsers').append(userM);

    });
});
//User Disconnected
connection.on("Disconnected", function (user) {

    var msg = '<b>' + user + '</b> has disconnected from the server<hr>';
    $('#messagesList').append(msg);


    //scroll down automatically
    $("#messagesList").stop().animate({ scrollTop: $("#messagesList")[0].scrollHeight }, 1000);

});
//User Joined
connection.on("Join", function (user) {

    var msg = '<b>' + user + '</b> has joined the server<hr>';
    $('#messagesList').append(msg);


    //scroll down automatically
    $("#messagesList").stop().animate({ scrollTop: $("#messagesList")[0].scrollHeight }, 1000);

});



//Send Button
document.getElementById("sendButton").addEventListener("click", function (event) {
    SendMessage();
});

//Enter button

document.getElementById('messageInput').onkeydown = function (e) {
    if (e.keyCode == 13) {
        SendMessage();
    }
};

//Send message
function SendMessage() {

    var message = document.getElementById("messageInput").value;
    if (message) {
        var user = document.getElementById("username").innerHTML;
        var time = FormatDate(new Date());
        connection.invoke("SendMessage", user, time, message).catch(function (err) {
            return console.error(err.toString());

        });
        
        clearInput();
    }
  
}

//date time format
function FormatDate(now) {
    var formatedDate = dateFormat(now, "dd/mm HH:MM")
    return formatedDate;
}
// clear message input
function clearInput() {
    $(".messageBar :input").each(function () {
        $(this).val(''); //hide form values
    });
}
