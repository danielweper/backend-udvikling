"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.on("ReceiveButton", function (user, color) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} pressed ${color}`;
});

connection.on("ReceiveClass", (person) => {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    
    li.textContent = `sent Person with name: ${person.name} & age: ${person.age}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

var color = document.getElementById("coloredButton");
color.addEventListener("click", function (event) {
        var user = document.getElementById("userInput").value;
        var buttonColor = color.value;
        var message = `${buttonColor}`;
        console.log(buttonColor);

        connection.invoke("ClickButton", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        console.log("eee");
        event.preventDefault();
    });

document.getElementById("sendClass").addEventListener("click", function (event) {
    const person = {name:"Danny", age:25};
    
    connection.invoke("SendClass", person).catch(function (err) {
        console.log("didn't sent class");
        return console.error(err.toString());
    });

    console.log("sent class");
    event.preventDefault();
});