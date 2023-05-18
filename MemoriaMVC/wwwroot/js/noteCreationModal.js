// ajax function for partial note-creation-modal fetching 
$(function () {
    // creating new note functionality
    $('#btnAddNewNote').off('click').on('click', function () {
        var note = {
            AuthorId: null,
            Id: null,
            Status: 0,
            UpdatedDateAndTime: null,
            UpdatedBy: null,
            AddedBy: null,
            FileFormat: null,
            Type: null,
            Title: null,
            Description: null,
            Todos: null,
            Labels: null,
            TrashingDate: null,
            BgColor: null,
            IsHidden: false,
            IsTrashed: false,
            IsArchived: false,
            IsPinned: false,
            IsMarked: false,
            IsDraft: false,
            IsArchieved: false,
            IsRemainderAdded: false,
            RemainderDateTime: null,
        };
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


        fetchNoteCreationModal()
            .then(function (data) {
                return fetchUserData(data);
            })
            .then(function (userData) {
                return createDraftNote(userData);
            })
            .then(function (noteData) {
                let totalSelectedAttachment = 0;
                let totalUploadedAttachment = 0;
                // modal handling code
                var labelSelect = document.getElementById('labelsSelect');
                var labelsContainer = document.getElementById('labelsContainer');
                const todoContainer = document.getElementById('todo-container');
                const addTodoButton = document.getElementById('add-todo');
                const todoToggle = document.querySelector('#myCheckbox');
                const todoSection = document.getElementById('todo-section');
                const noteTitle = document.getElementById('note-title');
                const noteDescription = document.getElementById('note-description');
                const closeButton = document.querySelector('#myModal .modal-header .btn-close');

                // getting all the buttons
                const saveButton = document.getElementById('save-button');
                const attachmentButton = document.getElementById('attachment-button');
                const remainderButton = document.getElementById('remainder-button');
                const authorizationButton = document.getElementById('authorization-button');

                // date picking 
                const datePicker = document.getElementById("reminder-datepicker");

                // data containers 
                const todoInputs = []; // Array to store the todo input elements
                const labelsInputs = []; // Array to store the label input elements

                // Function to retrieve all the todo inputs
                function getTodoInputs() {
                    const todoValues = [];
                    for (let i = 0; i < todoInputs.length; i++) {
                        const todoInput = todoInputs[i];
                        const todoValue = todoInput.value.trim();
                        if (todoValue !== '') {
                            todoValues.push(todoValue);
                        }
                    }
                    return todoValues;
                }

                // Function to retrieve all the label inputs
                function getLabelsInputs() {
                    const labelsValues = [];
                    for (let i = 0; i < labelsInputs.length; i++) {
                        const labelInput = labelsInputs[i];
                        const labelValue = labelInput.textContent.trim();
                        if (labelValue != null) {
                            labelsValues.push(labelValue);
                        }
                    }
                    return labelsValues;
                }

                // label selecting code
                function insertANewLabel () {
                    var selectedLabel = labelSelect.value;
                    var labelElement = document.createElement('p');
                    labelElement.classList.add('labels-container-item');
                    labelElement.textContent = selectedLabel;
                    labelsContainer.appendChild(labelElement);
                    labelsInputs.push(labelElement);
                }

                // Function to create a new todo element
                function createTodoElement() {
                    const todoElement = document.createElement('div');
                    todoElement.classList.add('form-check');

                    const checkboxElement = document.createElement('input');
                    checkboxElement.classList.add('form-check-input');
                    checkboxElement.type = 'checkbox';

                    const inputElement = document.createElement('input');
                    inputElement.classList.add('form-control');
                    inputElement.placeholder = 'Enter todo';

                    // Append the checkbox and input field to the todo element
                    todoElement.appendChild(checkboxElement);
                    todoElement.appendChild(inputElement);

                    // Append the todo element to the todo container
                    todoContainer.appendChild(todoElement);

                    // Add the input element to the todoInputs array
                    todoInputs.push(inputElement);
                }

                function modifySaveButton (event) {
                    if (event.target.value.trim() == '') {
                        saveButton.disabled = true;
                    } else {
                        saveButton.disabled = false;
                    }
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
                        if (todo.length > 25) {
                            console.log(todo.length);
                            return todo.slice(0, 25) + '...';
                        } else {
                            return todo;
                        }
                    });


                    // Create the card HTML using template literals
                    var cardHTML = `
                        <div class="flex-note-container-item" style=" width: 18rem;">
                          <img src="..." class="" alt="...">
                          <div class="">
                            <h5 class="" style="cursor:pointer;">${truncatedTitle}</h5>
                            <p class="">${truncatedDescription}</p>
                          </div>
                          <ul class="">
                            ${todosArray
                                    .map(item => `<li class="">${item}</li>`)
                                    .join('')}
                          </ul>
                          <div class="">
                            <a href="#" class="">Card link</a>
                            <a href="#" class="">Another link</a>
                          </div>
                        </div>
                      `;

                    // Append the new note HTML at the top of the card container
                    var cardContainer = document.getElementById('card-container');
                    cardContainer.insertAdjacentHTML('afterbegin', cardHTML);
                }


                // Adding event listeners 
                labelSelect.addEventListener('change', insertANewLabel);
                todoToggle.addEventListener('change', function () {
                    if (this.checked) {
                        todoSection.style.display = 'block'; // Show the todo container
                    } else {
                        todoSection.style.display = 'none'; // Hide the todo container
                    }
                });
                addTodoButton.addEventListener('click', createTodoElement);
                remainderButton.addEventListener('click', function () { })
                authorizationButton.addEventListener('click', AddAuthorizer);
                noteTitle.addEventListener('input', modifySaveButton);

                // attachment related code
                $("#attachment-button").click(function () {
                    $("#file-input").click();
                });
                pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.8.335/pdf.worker.min.js';

                $("#file-input").change(function () {
                    totalSelectedAttachment++;
                    var file = this.files[0];
                    // Check the file type
                    if (file.type.startsWith("image/")) {
                        // Upload the image to the database
                        UploadAttachment(noteData, file)
                            .then(function (attachmentId) {
                                totalUploadedAttachment++;
                                // Preview Image file
                                previewImage(file, attachmentId);
                            });
                    } else if (file.type === "application/pdf") {
                        // Upload the file to the database
                        UploadAttachment(noteData, file)
                            .then(function (attachmentId) {
                                totalUploadedAttachment++;
                                // Preview Pdf file
                                previewPDF(file, attachmentId);
                            });
                    }

                    $(this).val("");
                });

                function previewImage(file, attachmentId) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        // Create an image element
                        var img = $("<img>").attr("src", e.target.result);

                        // Create the close button
                        const closeButton = $('<button>', {
                            class: 'close-button',
                            html: '&times;'
                        });
                        // remove attachment from database
                        closeButton.on('click', function () {
                            var attachmentId = $(this).closest('.file-details').data('attachment-id');
                            $.ajax({
                                url: '/Attachments/Delete/' + attachmentId,
                                method: 'DELETE',
                                data: { attachmentId: attachmentId },
                                success: function (response) {
                                    totalSelectedAttachment--;
                                    totalUploadedAttachment--;
                                    closeButton.parent().remove();
                                },
                                error: function (xhr, status, error) {
                                    console.log('Error:', error);
                                }
                            });
                        });

                        // Append the image to the file details container
                        var fileDetails = $("<div>")
                            .addClass(["file-details"])
                            .data('attachment-id', attachmentId)
                            .append(img);
                        fileDetails.append(closeButton);
                        $("#file-details-container").append(fileDetails);
                    };
                    reader.readAsDataURL(file);
                }

                function previewPDF(file, attachmentId) {
                    var fileReader = new FileReader();
                    fileReader.onload = function () {
                        var typedarray = new Uint8Array(this.result);

                        // Load the PDF using pdf.js
                        pdfjsLib.getDocument(typedarray).promise.then(function (pdf) {
                            // Render the first page of the PDF
                            pdf.getPage(1).then(function (page) {
                                var viewport = page.getViewport({ scale: 1 });
                                var canvas = $("<canvas>").get(0);
                                var context = canvas.getContext("2d");

                                canvas.width = viewport.width;
                                canvas.height = viewport.height;

                                // Render the PDF page to the canvas
                                page.render({
                                    canvasContext: context,
                                    viewport: viewport
                                }).promise.then(function () {
                                    // Create the close button
                                    const closeButton = $('<button>', {
                                        class: 'close-button',
                                        html: '&times;'
                                    });
                                    closeButton.on('click', function () {
                                        var attachmentId = $(this).closest('.file-details').data('attachment-id');
                                        $.ajax({
                                            url: 'Attachments/Delete/' + attachmentId,
                                            method: 'DELETE',
                                            data: { attachmentId: attachmentId },
                                            success: function (response) {
                                                totalSelectedAttachment--;
                                                totalUploadedAttachment--;
                                                closeButton.parent().remove();
                                            },
                                            error: function (xhr, status, error) {
                                                console.log('Error:', error);
                                            }
                                        });
                                    });
                                    // Append the canvas to the file details container
                                    var fileDetails = $("<div>")
                                        .addClass("file-details")
                                        .append(canvas);
                                    $("#file-details-container")
                                        .append(fileDetails);
                                    fileDetails.append(closeButton);
                                });
                            });
                        }).catch(function (error) {
                            console.error("Error occurred while loading PDF:", error);
                        });
                    };
                    fileReader.readAsArrayBuffer(file);
                }

                // Handle remainder button click
                remainderButton.addEventListener("click", function () {
                    if (datePicker.style.display === "none") {
                        datePicker.style.display = "block";
                        // Set default date and time one hour later
                        var currentDate = new Date();
                        currentDate.setHours(currentDate.getHours() + 1);
                        var defaultDateTime = currentDate.toISOString().slice(0, 16);
                        datePicker.value = defaultDateTime;
                        note.IsRemainderAdded = true;

                    } else {
                        datePicker.style.display = "none";
                        note.IsRemainderAdded = false;
                    }

                });

                // Handle date and time selection
                datePicker.addEventListener("changeDate", function (event) {
                    var selectedDateTime = event.target.value;
                    console.log(selectedDateTime);
                });


                // Saving the note
                $('#save-button').off('click').on('click', function () {
                    if (totalSelectedAttachment === totalUploadedAttachment) {
                        note.Id = noteData.id;
                        note.AddedBy = noteData.authorId;
                        note.Todos = JSON.stringify(getTodoInputs());
                        note.Title = noteTitle.value;
                        note.Description = noteDescription.value;
                        note.Labels = JSON.stringify(getLabelsInputs());
                        note.Type = null;
                        note.IsDraft = false;
                        note.IsDraft = false;
                        // remainder
                        if (note.IsRemainderAdded) {
                            note.RemainderDateTime = datePicker.value;
                            console.log(note.RemainderDateTime);
                        }
                        saveNote(note)
                            .then(function (addedNote) {
                                $('#myModal').modal('hide');
                                console.log('---',addedNote)
                                showSingleRawCardTop(addedNote);
                            });
                    } else {
                        alert('Please wait.. Files uploading');
                    }

                });
            })
            .fail(function (error) {
                console.error("Error in nested AJAX calls");
                console.error(error);
            });
    });
});