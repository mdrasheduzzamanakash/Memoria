$(function () {
    var noteData = null;
    var userData = null;

    const titleElement = document.getElementById('rightSectionTitle');
    const descriptionElement = document.getElementById('right-section-description');

    const noteChangeConnection = new signalR.HubConnectionBuilder()
        .withUrl("/noteChangeHub")
        .build();

    noteChangeConnection.start()
        .then(() => {
            noteChangeConnection.invoke("JoinNoteGroup", pageData.noteId)
                .then(() => {
                    // fetch note data
                    fetchNoteById(pageData.noteId)
                        .then(function (fetchedNote) {
                            noteData = fetchedNote;
                            if (noteData.title !== null && noteData.title !== '') {
                                titleElement.innerText = noteData.title;
                            }
                            if (noteData.description !== null && noteData.description !== '') {
                                descriptionElement.innerHTML = noteData.description;
                            }
                            return fetchUserById(pageData.writerId);
                        })
                        .then(function (fetchedUser) {
                            userData = fetchedUser;
                            titleElement.contentEditable = true;
                            descriptionElement.contentEditable = true;
                        })
                })
        })
        .catch(error => {
            console.error("SignalR connection error: ", error);
        });

    // Receiving function
    noteChangeConnection.on("ReceiveNoteChanges", (noteChangeReceivedString) => {
        var noteChangePayload = JSON.parse(noteChangeReceivedString);

        if (noteChangePayload.IsTitleChanged) {
            titleElement.contentEditable = false;
            titleElement.innerText = noteChangePayload.TitleChanges;
            titleElement.contentEditable = true;
        } else {
            console.log(noteChangePayload.DescriptionChanges)
            descriptionElement.contentEditable = false;
            descriptionElement.innerHTML = noteChangePayload.DescriptionChanges;
            descriptionElement.contentEditable = true;
        }

    });


    // add change tracking to the input field
    var noteTitleChangeTimer;
    titleElement.addEventListener('input', function (event) {
        clearTimeout(noteTitleChangeTimer);
        noteTitleChangeTimer = setTimeout((function (event) {
            if (titleElement.innerText !== '') {
                var currentTitle = titleElement.innerText;
                var noteChangesPaylaodString = JSON.stringify({
                    noteId: pageData.noteId,
                    writerId: pageData.writerId,
                    titleChanges: currentTitle,
                    descriptionChanges: '',
                    isTitleChanged: true,
                    isDescriptionChanged: false
                });
                noteChangeConnection.invoke("BroadCastNoteChange", noteChangesPaylaodString);
            }
        }), 500);
    });

    var noteDescriptionTimer;
    descriptionElement.addEventListener('input', function (event) {
        clearTimeout(noteDescriptionTimer);
        noteDescriptionTimer = setTimeout((function (event) {
            var currentDescription = descriptionElement.innerHTML;
            var noteChangesPaylaodString = JSON.stringify({
                noteId: pageData.noteId,
                writerId: pageData.writerId,
                titleChanges: '',
                descriptionChanges: currentDescription,
                isTitleChanged: false,
                isDescriptionChanged: true
            });
            noteChangeConnection.invoke("BroadCastNoteChange", noteChangesPaylaodString);
        }), 500);
    });

    // dispossable sectiton
    window.addEventListener('beforeunload', function (event) {
        noteChangeConnection.stop();
        console.log('stopping the connection');
    });

})