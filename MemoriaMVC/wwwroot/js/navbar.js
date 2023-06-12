$(function () {
    const logoutElement = document.getElementById('logout-cog');
    const myProfileButton = document.getElementById('my-profile-cog');

    logoutElement.addEventListener('click', function () {
        // TODO: remove refresh token
        // TODO: invalidate jwt token or revoke it

        // temoporary implementation 
        document.cookie = "jwt" + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        window.location.href = '/LandingPage/Index';
    });

    myProfileButton.addEventListener('click', function () {
        const loggedInUserId = localStorage.getItem('loggedInUserId');
        window.location.href = '/Users/Profile/' + loggedInUserId;
    });


})