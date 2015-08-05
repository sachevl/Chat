$(function () {
    $(document).ajaxError(function (event, request, settings) {
        alert('error');
    });

    function joinChat(name) {
        $.post('api/chat/join', { name: name }).done(function (userId) {
            var chatHub = $.connection.chat;

            chatHub.client.addNewMessageToPage = chat.discussionPresenter.addMessage;
            chatHub.client.leaves = chat.usersPresenter.removeUser;
            chatHub.client.joins = chat.usersPresenter.addUser;

            $.connection.hub.start().done(function () {
                chatHub.server.join(userId).done(chat.discussionPresenter.init);
            });
        });
    }

    chat.joinPresenter.showDialog(joinChat);
});

function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

var chat = {};

chat.joinPresenter = (function() {
    function showJoinDialog(f) {
        $('#joinModal').modal('show');

        $('#joinForm').submit(function (event) {
            event.preventDefault();
            f($('#inputName').val());
        });
    }

    function hideJoinDialog() {
        $('#joinModal').modal('hide');
    }

    return {
        showDialog: showJoinDialog,
        hideDialog: hideJoinDialog
    };
})();

chat.usersPresenter = (function() {
    function addUser(user) {
        $('#users').append('<li id="user-' + user.Id + '">' + htmlEncode(user.Name) + '</li>');
    }

    function removeUser(userId) {
        $('#user-' + userId).remove();
    }

    function addUsers(users) {
        users.forEach(addUser);
    }

    return {
        addUsers: addUsers,
        addUser: addUser,
        removeUser: removeUser
    };
})();

chat.discussionPresenter = (function() {
    function init() {
        $('#sendMessageForm').submit(function (event) {
            event.preventDefault();
            $.connection.chat.server.send($('#message').val());
            $('#message').val('').focus();
        });

        chat.joinPresenter.hideDialog();
        $('#message').focus();
        $('#userName').text(name);

        $.get('api/user').done(chat.usersPresenter.addUsers);
    }

    function addMessage (name, message) {
        $('#discussion').append('<li><strong>' + htmlEncode(name)
            + '</strong>: ' + htmlEncode(message) + '</li>');
    }

    return {
        init: init,
        addMessage: addMessage
    };
})();