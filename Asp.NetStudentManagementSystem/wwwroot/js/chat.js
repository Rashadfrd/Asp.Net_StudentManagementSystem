"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, sendername, message) {
    /*    var senders = JSON.parse(localStorage.getItem("senders")) || [];*/
    var whoSent = document.getElementById('whosent')
    var sentMessage = document.getElementById("messageInput");
    sentMessage.value = " ";
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;
    var li = document.createElement("li");
    var span = document.createElement("span");
    var now = new Date();
    const hoursAndMinutes =
        padTo2Digits(now.getHours()) + ':' + padTo2Digits(now.getMinutes());
    span.textContent = hoursAndMinutes;
    span.style.padding = "5px"
    li.style.borderRadius = "18px"
    li.style.margin = "5px"
    li.style.display = "inline"
    li.style.fontSize = "17px"
    span.style.fontSize = "11px"

    if (user == document.getElementById("receiverInput").value) {

        if (user == document.getElementById("senderInput").value) {
            li.style.textAlign = "end"
            li.style.borderTopRightRadius = "0px"
            li.style.backgroundColor = "#144365"
            li.style.color = "white"
            li.style.padding = "7px 10px 7px 30px"
            li.style.alignSelf = "flex-end"
            li.textContent = encodedMsg;
            li.appendChild(span);
            document.getElementById("messagesList").appendChild(li);
            var elem = document.getElementById('data-box');
            elem.scrollTop = elem.scrollHeight * 2;
        }
        else {
            li.style.borderTopLeftRadius = "0px"
            li.style.backgroundColor = "white"
            li.style.padding = "7px 30px 7px 10px"
            li.style.alignSelf = "flex-start"
            li.textContent = encodedMsg;
            li.prepend(span);
            document.getElementById("messagesList").appendChild(li);
            var elem = document.getElementById('data-box');
            elem.scrollTop = elem.scrollHeight * 2;

            if (document.getElementById(user)) {
                var singleMsg = document.getElementById(user)
                singleMsg.textContent = encodedMsg

            }
            else {
                var newInboxAnchor = document.createElement("a")
                newInboxAnchor.setAttribute('asp-route-email', user)
                var newInbox = document.createElement("li")
                newInbox.style.padding = "5px 15px"
                newInbox.style.height = "65px"
                newInbox.style.border = "1px solid #ccc"
                newInbox.style.marginBottom = "5px"
                newInbox.style.backgroundColor = "#f5f5fd"
                newInbox.style.borderRadius = "8px"
                var msgwrapper = document.createElement("div")
                msgwrapper.textContent = encodedMsg
                msgwrapper.setAttribute('id', user)
                newInbox.appendChild(msgwrapper)
                var namewrapper = document.createElement("div")
                namewrapper.textContent = sendername
                newInbox.prepend(namewrapper)
                newInboxAnchor.appendChild(newInbox)
                var singleMsg = document.getElementById("single-messages")
                singleMsg.appendChild(newInboxAnchor)
            }
        }
    }
    else {

        if (user == document.getElementById("senderInput").value) {
            li.style.textAlign = "end"
            li.style.borderTopRightRadius = "0px"
            li.style.backgroundColor = "#144365"
            li.style.color = "white"
            li.style.padding = "7px 10px 7px 30px"
            li.style.alignSelf = "flex-end"
            li.textContent = encodedMsg;
            li.appendChild(span);
            document.getElementById("messagesList").appendChild(li);
            var elem = document.getElementById('data-box');
            elem.scrollTop = elem.scrollHeight * 2;
        }
        else {

            if (document.getElementById(user)) {
                var singleMsg = document.getElementById(user)
                singleMsg.textContent = encodedMsg

            }
            else {
                var newInboxAnchor = document.createElement("a")
                newInboxAnchor.setAttribute('asp-route-email', user)
                var newInbox = document.createElement("li")
                newInbox.style.padding = "5px 15px"
                newInbox.style.height = "65px"
                newInbox.style.border = "1px solid #ccc"
                newInbox.style.backgroundColor = "#f5f5fd"
                newInbox.style.marginBottom = "5px"
                newInbox.style.borderRadius = "8px"
                var msgwrapper = document.createElement("div")
                msgwrapper.textContent = encodedMsg
                msgwrapper.setAttribute('id', user)
                newInbox.appendChild(msgwrapper)
                var namewrapper = document.createElement("div")
                namewrapper.textContent = sendername
                newInbox.prepend(namewrapper)
                newInboxAnchor.appendChild(newInbox)
                var singleMsg = document.getElementById("single-messages")
                singleMsg.appendChild(newInboxAnchor)
            }
        }
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var sendername = document.getElementById("senderNameInput").value;
    var sender = document.getElementById("senderInput").value;
    var receiver = document.getElementById("receiverInput").value;
    var message = document.getElementById("messageInput").value;

    if (receiver != "") {

        connection.invoke("SendMessageToGroup", sendername, sender, receiver, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    else {
        connection.invoke("SendMessage", sender, message).catch(function (err) {
            return console.error(err.toString());
        });
    }
    message = " ";

    event.preventDefault();
});
var elem = document.getElementById('data-box');
elem.scrollTop = elem.scrollHeight * 2;

function padTo2Digits(num) {
    return String(num).padStart(2, '0');
}