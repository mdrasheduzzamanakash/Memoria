$(function () {
    var userId = localStorage.getItem("loggedInUserId");
    const notificationConnection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub?userId=" + userId)
        .build();

    notificationConnection.start()
        .then(() => {
            console.log('done-not-connection');
        })
        .catch(error => {
            console.error("SignalR connection error: ", error);
        });

    notificationConnection.on("ReceiveNotification", (notificationPayloadString) => {
        var notificationPayload = JSON.parse(notificationPayloadString);

        console.log(notificationPayload);
        
    });

    // dispossable sectiton
    window.addEventListener('beforeunload', function (event) {
        notificationConnection.stop();
        console.log('stopping the notification connection');
    });
})