function showRawCardSingle(cardData) {
    var maxTitleLength = 50;
    var maxDescriptionLength = 100; // maximum length for description

    // Truncate the description if it exceeds the maximum length
    var truncatedDescription = cardData.description.length > maxDescriptionLength
        ? cardData.description.slice(0, maxDescriptionLength) + '...'
        : cardData.description;
    var truncatedTitle = cardData.title.length > maxTitleLength
        ? cardData.title.slice(0, maxTitleLength) + '...'
        : cardData.title;

    var todosArray = JSON.parse(cardData.todos);
    todosArray = todosArray.slice(0, 3).map(todo => {
        if (todo.value.length > 25) {
            return todo.value.slice(0, 25) + '...';
        } else {
            return todo.value;
        }
    });


    // Create the card HTML using template literals
    var cardHTML = `
                        <div class="flex-note-container-item" style=" width: 18rem;" id="${cardData.id}">

                            <div style="padding:10px;">
                                <div class="">
                                    <h5 class="note-title" style="cursor:pointer; word-break: break-word;" id="title-${cardData.id}">${truncatedTitle}</h5>
                                    <p class="" style="word-break: break-word;">${truncatedDescription}</p>
                                </div>
                                <ul class="">
                                    ${todosArray
            .map(item => `<li class="">${item}</li>`).join('')}
                                </ul>
                                <div class="note-link-container">
                                    
                                </div>
                            </div>
                        </div>
                `;

    // Append the card HTML to the container
    var cardContainer = document.getElementById('card-container');
    cardContainer.innerHTML += cardHTML;
}

function showSingleRawCardTop(newNote) {
    var maxTitleLength = 50;
    var maxDescriptionLength = 100; // maximum length for description

    // Truncate the description if it exceeds the maximum length
    var truncatedDescription = newNote.description.length > maxDescriptionLength
        ? newNote.description.slice(0, maxDescriptionLength) + '...'
        : newNote.description;
    var truncatedTitle = newNote.title.length > maxTitleLength
        ? newNote.title.slice(0, maxTitleLength) + '...'
        : newNote.title;

    var todosArray = JSON.parse(newNote.todos);
    todosArray = todosArray.slice(0, 3).map(todo => {
        if (todo.value.length > 25) {
            return todo.slice(0, 25) + '...';
        } else {
            return todo.value;
        }
    });



    // Create the card HTML using template literals
    var cardHTML = `
                        <div class="flex-note-container-item" style=" width: 18rem;" id="${newNote.id}">

                            <div style="padding:10px;">
                                <div class="">
                                <h5 class="note-title" style="cursor:pointer; word-break: break-word;" id="title-${newNote.id}">${truncatedTitle}</h5>
                                <p class="" style="word-break: break-word;">${truncatedDescription}</p>
                                </div>
                                <ul class="">
                                ${todosArray
            .map(item => `<li class="">${item}</li>`)
            .join('')}
                                </ul>
                                <div class="note-link-container">
                                </div>
                             </div>
                          
                        </div>
                      `;

    // Append the new note HTML at the top of the card container
    var cardContainer = document.getElementById('card-container');
    cardContainer.insertAdjacentHTML('afterbegin', cardHTML);
}

function extractLinks(text) {
    try {
        var urlRegex = /(https?:\/\/[^\s]+)/g;
        var urls = text.match(urlRegex);
        return urls;
    } catch (ex) {
        console.log(ex);
    }
}

function showLinksPerNoteSingle(nonDraftNote) {
    try {
        var text = nonDraftNote.description;
        var links = extractLinks(text);
        if (links != null) {
            var noteContainer = document.getElementById(nonDraftNote.id);
            var linkContainer = noteContainer.querySelector('.note-link-container');
            for (let j = 0; j < links.length; j++) {
                var link = links[j];
                var linkText = link.slice(0, 35) + '...';
                var linkHTML = `<a href="${link}" style="text-decoration: none;
                                                                         color: #808080">${linkText}</a>`;
                linkContainer.innerHTML += linkHTML;
            }
        }
    } catch (ex) {
        console.log(ex);
    }

}

function showAttachmentPreviewToEachCardSingle(attachment) {
    try {
        var noteElement = document.getElementById(attachment.noteId);

        if (attachment.fileType.startsWith('image')) {
            // Create an img element for image preview
            var imgElement = document.createElement('img');
            imgElement.src = 'data:' + attachment.fileType + ';base64,' + attachment.fileBase64;
            imgElement.classList.add('image-preview-in-note');
            noteElement.insertBefore(imgElement, noteElement.firstChild);
        } 
        else if (attachment.fileType === 'application/pdf') {
            // Create an iframe element for PDF preview
            var iframeElement = document.createElement('iframe');
            iframeElement.src = 'data:' + attachment.fileType + ';base64,' + attachment.fileBase64;
            iframeElement.classList.add('pdf-preview-in-note');
            noteElement.insertBefore(iframeElement, noteElement.firstChild);
        }
    } catch (e) {
        console.log(e);
    }
}

function fetchNoteCreationModal() {
    var deferred = $.Deferred();

    $.ajax({
        url: '/Notes/GetPartialView/' + userData.id,
        type: 'GET',
        success: function (result) {
            $('#myModal').find('.modal-content').html(result);
            $('#myModal').modal('show');
            deferred.resolve(result);
        },
        error: function () {
            alert('Error loading partial view');
        }
    });

    return deferred.promise();
}

function fetchNoteUpdationModal() {
    var deferred = $.Deferred();

    $.ajax({
        url: '/Notes/GetPartialViewUpdation/' + userData.id,
        type: 'GET',
        success: function (result) {
            $('#myModal').find('.modal-content').html(result);
            $('#myModal').modal('show');
            deferred.resolve(result);
        },
        error: function () {
            alert('Error loading partial view');
        }
    });

    return deferred.promise();
}


function fetchUserData(dataFromPrevious) {
    var deferred = $.Deferred();
    $.ajax({
        url: '/Users/GetById/' + userData.id,
        type: 'GET',
        success: function (user) {
            deferred.resolve(user);
        },
        error: function () {
            alert('Error loading user data');
        }
    });
    return deferred.promise();
}

function createDraftNote(userData) {
    var deferred = $.Deferred();
    note.AuthorId = userData.id;
    note.IsDraft = true;
    var noteString = JSON.stringify(note);
    $.ajax({
        url: '/Notes/CreateDraftNote/',
        type: 'POST',
        contentType: 'application/json',
        data: noteString,
        success: function (noteData) {
            deferred.resolve(noteData);
        },
        error: function () {
            alert('Error loading user data');
        }
    });
    return deferred.promise();
}

function saveNote(finalNote) {
    var deferred = $.Deferred();
    var noteString = JSON.stringify(finalNote);
    $.ajax({
        url: '/Notes/SaveNote/',
        type: 'POST',
        contentType: 'application/json',
        data: noteString,
        success: function (noteData) {
            deferred.resolve(noteData);
        },
        error: function () {
            alert('Error loading user data');
        }
    });
    return deferred.promise();
}


function UploadAttachment(noteData, file) {
    var deferred = $.Deferred();
    var formData = new FormData();
    formData.append("file", file);

    formData.append('attachmentMetaData', JSON.stringify({
        NoteId: noteData.id,
        FileType: file.type,
        ContentSize: file.size,
        OwnerId: noteData.authorId
    }));

    $.ajax({
        url: "/Attachments/AddAttachment",
        type: 'POST',
        data: formData,
        processData: false,
        contentType: false,

        success: function (response) {
            deferred.resolve(response);
        },
        error: function () {
            alert('Error uploading attachment');
        }
    });

    return deferred.promise();
}

function DeleteAttachment(attachmentId) {
    var deferred = $.Deferred();

    $.ajax({
        url: "/Attachments/DeleteAttachment/" + attachmentId,
        type: "DELETE",
        success: function (response) {
            deferred.resolve(response);
        },
        error: function () {
            alert('Error deleting attachment');
        }
    });

    return deferred.promise();
}

function AddAuthorizer() { }

function DeleteAuthorizer() { }

function GetDefaultAttachment(noteId) { }

function fetchNonDraftNotes() {
    var deferred = $.Deferred();

    $.ajax({
        url: "/Notes/AllWithOutDraft/",
        data: {
            authorId: userData.id
        },
        success: function (response) {
            deferred.resolve(response);
        },
        error: function (xhr, status, error) {
            console.log("error in fetchNonDraftNotes");
        }
    });

    return deferred.promise();
}

function fetchAttachmentPreview(notes) {
    var deferred = $.Deferred();

    var noteIds = [];
    for (let i = 0; i < nonDraftNotes.length; i++) {
        noteIds.push(nonDraftNotes[i].id);
    }

    $.ajax({
        url: "/Attachments/AllAttachmentPreview/",
        data: {
            noteIds: JSON.stringify(noteIds) // Serialize the array as a JSON string
        },
        success: function (response) {
            attachments = response;
            deferred.resolve(response);
        },
        error: function (xhr, status, error) {
            console.log("error in fetchAttachmentPreview");
        }
    });

    return deferred.promise();
}

function fetchAttachmentAllForANote(noteId) {
    var deferred = $.Deferred();
    $.ajax({
        url: "/Attachments/AttachmentAllForANote/",
        data: {
            noteId: noteId 
        },
        success: function (response) {
            deferred.resolve(response);
        },
        error: function (xhr, status, error) {
            console.log("error in fetchAttachmentPreview");
        }
    });

    return deferred.promise();
}

function fetchNoteById(id) {
    var deferred = $.Deferred();

    $.ajax({
        url: "/Notes/GetById/",
        data: {
            noteId: id
        }, 
        success: function (response) {
            deferred.resolve(response);
        },
        error: function (xhr, status, error) {
            console.log("error in fetchNonDraftNotes");
        }
    })

    return deferred.promise();
}