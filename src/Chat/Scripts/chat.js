$(function () {
    $(document).ajaxError(function (event, request, settings) {
        alert('error');
    });

    var chat = $.connection.chat;
    $('#loginModal').modal('show');

    $('#joinForm').submit(function (event) {
        event.preventDefault();
        joinChat($('#inputName').val());
    });

    function joinChat(name) {
        $.post('api/chat/join', { name: name }).done(function (userId) {
            chat.client.addNewMessageToPage = function (name, message) {
                $('#discussion').append('<li><strong>' + htmlEncode(name)
                    + '</strong>: ' + htmlEncode(message) + '</li>');
            };

            chat.client.leaves = function (userId) {
                $('#user-' + userId).remove();
            }

            chat.client.joins = addUser;

            $.connection.hub.start().done(function () {
                chat.server.join(userId).done(function () {
                    $('#sendMessageForm').submit(function (event) {
                        event.preventDefault();
                        $.connection.chat.server.send($('#message').val());
                        $('#message').val('').focus();
                    });

                    $('#loginModal').modal('hide');
                    $('#message').focus();
                    $('#userName').text(name);

                    $.get('api/user').done(function(users) {
                        users.forEach(addUser);
                    });
                });
            });
        });
    }
});

function addUser(user) {
    $('#users').append('<li id="user-' + user.Id + '">' + htmlEncode(user.Name) + '</li>');
}

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}