$(function () {
    var chat = $.connection.chat;
    $('#loginModal').modal('show');

    $('#joinForm').submit(function (event) {
        event.preventDefault();
        joinChat($('#inputName').val());
    });

    function joinChat(name) {
        chat.client.addNewMessageToPage = function (name, message) {
            $('#main').append('<li><strong>' + htmlEncode(name)
                + '</strong>: ' + htmlEncode(message) + '</li>');
        };

        $.connection.hub.start().done(function () {
            chat.server.join(name).done(function () {
                $('#sendMessageForm').submit(function (event) {
                    event.preventDefault();
                    $.connection.chat.server.send($('#message').val());
                    $('#message').val('').focus();
                });

                $('#loginModal').modal('hide');
                $('#message').focus();
                $('#userName').text(name);
            });
        });
    }
});

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}