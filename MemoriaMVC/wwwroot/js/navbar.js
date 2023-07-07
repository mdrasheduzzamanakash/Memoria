$(function () {
    const logoutElement = document.getElementById('logout-cog');
    const myProfileButton = document.getElementById('my-profile-cog');
    const themeChangeButton = document.getElementById('theme-toggle-cog');
    const navBarProfileImage = document.getElementById('navbar-profile-image');

    // decorate navbar 
    var profileImage = localStorage.getItem('profileImage');
    var profileImageFileType = localStorage.getItem('profileImageFileType');
    navBarProfileImage.src = 'data:' + profileImageFileType + ';base64,' + profileImage;
    navBarProfileImage.classList.add('profile-picture');

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

    themeChangeButton.addEventListener('click', function () {
        // add theme change code 
    });
})