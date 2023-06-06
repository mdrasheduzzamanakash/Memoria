$(function () {
    const sendButton = document.getElementById('send-button');
    const commentInput = document.getElementById('message-input');
    const commentContainer = document.getElementById('left-section-container');
    var noteData = null;
    var userData = null;
    var collaborators = null;
    var writerDetails = null;
    var authorDetails = null;
    var previousComments = null;

    const noteCommentConnection = new signalR.HubConnectionBuilder()
        .withUrl("/noteCommentHub")
        .build();

    noteCommentConnection.start()
        .then(() => {
            noteCommentConnection.invoke("JoinNoteGroup", pageData.noteId)
                .then(() => {
                    // fetch note data
                    fetchNoteById(pageData.noteId)
                        .then(function (fetchedNote) {
                            noteData = fetchedNote;
                            return fetchUserById(pageData.writerId);
                        })
                        .then(function (fetchedUser) {
                            userData = fetchedUser;
                            return fetchAllCollaborators(pageData.noteId);
                        })
                        .then(function (fetchedCollaborators) {
                            collaborators = fetchedCollaborators;
                            return fetchUserDetails(pageData.writerId);
                        })
                        .then(function (writerDetailsData) {
                            writerDetails = writerDetailsData;
                            return fetchUserDetails(noteData.authorId);
                        })
                        .then(function (authorDetailsData) {
                            authorDetails = authorDetailsData;
                            return fetchCommentsOfANote(pageData.noteId);
                        })
                        .then(function (comments) {
                            for (let i = 0; i < comments.length; i++) {
                                var commentData = {
                                    firstName: comments[i].firstName,
                                    lastName: comments[i].lastName,
                                    email: comments[i].email,
                                    image: getCollaboratorsImage(comments[i].commenterId),
                                    content: comments[i].content
                                };

                                var commentDiv = formateCommentDiv(commentData);
                                commentContainer.appendChild(commentDiv);
                                commentContainer.scrollTop = commentContainer.scrollHeight;
                            }
                            sendButton.disabled = false;
                        })
                })
        })
        .catch(error => {
            console.error("SignalR comment connection error: ", error);
        });

    // creating comment element 
    function formateCommentDiv(commentData) {
        // Create the comment container
        var commentDiv = document.createElement('div');
        commentDiv.classList.add('comment');

        // Create the profile picture
        var profilePicture = document.createElement('img');
        profilePicture.classList.add('profile-picture');
        profilePicture.src = commentData.image;

        // Create the name and email section
        var nameContainer = document.createElement('div');
        nameContainer.classList.add('name-container');

        // Create the name element
        var nameElement = document.createElement('span');
        nameElement.classList.add('name');
        nameElement.textContent = commentData.firstName + ' ' + commentData.lastName;

        // Create the email element
        var emailElement = document.createElement('span');
        emailElement.classList.add('email');
        emailElement.textContent = commentData.email;

        // Append name and email elements to the name container
        nameContainer.appendChild(nameElement);
        nameContainer.appendChild(emailElement);

        // Create the comment content
        var contentElement = document.createElement('div');
        contentElement.classList.add('comment-content');
        contentElement.textContent = commentData.content;

        nameContainer.appendChild(contentElement);

        // Create the options (triple dot)
        var optionsElement = document.createElement('div');
        optionsElement.classList.add('comment-options');
        optionsElement.innerHTML = '<i class="fas fa-ellipsis-v"></i>';

        // Append all elements to the comment container
        commentDiv.appendChild(profilePicture);
        commentDiv.appendChild(nameContainer);
        commentDiv.appendChild(optionsElement);

        return commentDiv;
    }

    function getCollaboratorsImage(id) {
        for (let i = 0; i < collaborators.length; i++) {
            if (collaborators[i].id === id) {
                const imageSrc = 'data:' + collaborators[i].fileFormat + ';base64,' + collaborators[i].fileBase64;
                return imageSrc;
            }
        }
        if (authorDetails.id === id) {
            const imageSrc = 'data:' + authorDetails.fileFormat + ';base64,' + authorDetails.fileBase64;
            return imageSrc;
        }

        return '';
    }

    // Receiving function
    noteCommentConnection.on("ReceiveNoteComment", (noteCommentReceivedString) => {
        var noteCommentPayload = JSON.parse(noteCommentReceivedString);

        console.log(noteCommentPayload)

        // make the comment payload 
        var commentData = {
            firstName: noteCommentPayload.FirstName,
            lastName: noteCommentPayload.LastName,
            email: noteCommentPayload.Email,
            content: noteCommentPayload.Content,
            image: getCollaboratorsImage(noteCommentPayload.CommenterId)
        };

        var commentDiv = formateCommentDiv(commentData);
        commentContainer.appendChild(commentDiv);
        commentContainer.scrollTop = commentContainer.scrollHeight;
    });

    sendButton.addEventListener('click', function () {
        var currentComment = commentInput.value;
        var noteCommentPaylaodString = JSON.stringify({
            noteId: pageData.noteId,
            commenterId: pageData.writerId,
            content: currentComment,
            email: writerDetails.email,
            firstName: userData.firstName,
            lastName: userData.lastName,
            image: '',
            fileFormat: writerDetails.fileFormat
        });
        noteCommentConnection.invoke("BroadCastComment", noteCommentPaylaodString);
    });


    // dispossable sectiton
    window.addEventListener('beforeunload', function (event) {
        noteCommentConnection.stop();
        console.log('stopping the connection');
    });

})