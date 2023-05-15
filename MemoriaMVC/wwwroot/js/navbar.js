const searchBar = document.getElementById('searchBar');
let timeout;

searchBar.addEventListener('input', function () {
    clearTimeout(timeout);
    timeout = setTimeout(function () {
        // Perform search here
        console.log(searchBar.value);
    }, 500); // Delay in milliseconds
});