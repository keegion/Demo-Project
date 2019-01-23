"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
//recieve message
connection.on("ReceiveMessage", function (user, time, message) {
   
    var msg = '<b>'+ user +'</b> '+time+' </br>'+ message+ '<hr>';
    $('#messagesList').append(msg);
    //scroll down automatically
    $("#messagesList").stop().animate({ scrollTop: $("#messagesList")[0].scrollHeight }, 1000);
   
});


connection.start().catch(function (err) {
    return console.error(err.toString());
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
        var user = "TestUser";
        var time = FormatDate(new Date());
        connection.invoke("SendMessage", user, time, message).catch(function (err) {
            return console.error(err.toString());

        });
        clearInput();
    }
}

//date time format
function FormatDate(now){
    var formatedDate = dateFormat(now, "dd/mm HH:MM")
    return formatedDate;
}
// clear message input
function clearInput() {
    $(".messageBar :input").each(function () {
        $(this).val(''); //hide form values
    });
}