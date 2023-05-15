$(function () {
    $('#btnAddNewNote').click(function () {
        $.ajax({
            url: '/Home/GetPartialView',
            type: 'GET',
            success: function (result) {
                $('#myModal').find('.modal-content').html(result);
                $('#myModal').modal('show');
            },
            error: function () {
                alert('Error loading partial view');
            }
        });
    });
});