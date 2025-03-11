$(() => {
    var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer");

    connection.start();
    connection.on("LoadComment", function () {
        LoadComments();
    });

    LoadComments();

    function LoadComments() {
        var content = "";
        $.ajax({
            url: '/Pages/Comments/I'
        });
    }

});