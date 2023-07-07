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

    

    const notificationIcon = document.getElementById('notification-icon');
    const notificationViewer = document.getElementById('notification-viewer');
    

    notificationIcon.addEventListener('click', () => {
        notificationViewer.classList.toggle('show');
    });

    // Close the notification viewer when clicking outside of it
    window.addEventListener('click', (event) => {
        if (!event.target.matches('#notification-icon') && !event.target.closest('.notification-viewer')) {
            notificationViewer.classList.remove('show');
            notificationIcon.style.color = 'gray';
        }
    });


    notificationConnection.on("ReceiveNotification", (notificationPayloadString) => {
        var notificationPayload = JSON.parse(notificationPayloadString);

        const notificationElement = document.createElement('div');
        notificationElement.classList.add('notification-item');
        notificationElement.innerText = notificationPayload.Content;
        notificationViewer.appendChild(notificationElement);
        notificationIcon.style.color = 'red';

        // display a toast
        const toastLiveExample = document.getElementById('liveToast');
        const toastMessage = document.getElementById('toast-message');
        toastMessage.innerText = "New Notification for you";
        const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLiveExample);
        toastBootstrap.show();
    });



    // dispossable sectiton
    window.addEventListener('beforeunload', function (event) {
        notificationConnection.stop();
        console.log('stopping the notification connection');
    });
})