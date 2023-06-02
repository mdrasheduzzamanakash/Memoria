/*
    this file requires _shared.js file as dependency
    this file also take jquery.min.js as dependency
*/



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

        var statusObj = {
            isSaveClicked: false,
            isTodoAdded: false,
            isLabelAdded: false,
            isTitleAdded: false,
            isDescriptionAdded: false,
            isFilesAdded: false,
            isRemainderAdded: false,
            isCollaboratorAdded: false
        };

       
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
                const todoToggle = document.querySelector('#myCheckbox');
                const todoSection = document.getElementById('todo-section');
                const noteTitle = document.getElementById('note-title');
                const noteDescription = document.getElementById('note-description');
                const saveButton = document.getElementById('save-button');
                const remainderButton = document.getElementById('remainder-button');
                const authorizationButton = document.getElementById('authorization-button');
                const todoInput = document.getElementById('todo-input');

                // date picking 
                const datePicker = document.getElementById("reminder-datepicker");

                // data containers 
                const labelsInputs = []; // Array to store the label input elements

                // Function to retrieve all the todo inputs
                function getTodoInputs() {
                    const todoItems = document.querySelectorAll('#added-todos .todo-item');
                    const todoValues = [];
                    todoItems.forEach(function (todoItem) {
                        const todoInput = todoItem.querySelector('span');
                        const todoValue = todoInput.textContent.trim();
                        if (todoValue !== '') {
                            var obj = {
                                value: todoValue,
                                state: false
                            };
                            todoValues.push(obj);
                        }
                    });
                    if (todoValues.length > 0) {
                        statusObj.isTodoAdded = true;
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
                    if (labelsValues.length > 0) {
                        statusObj.isLabelAdded = true;
                    }
                    return labelsValues;
                    
                }

                // handling the new label creation 
                const createNewLabelOption = document.getElementById('createNewLabel');
                const nestedInputContainer = document.getElementById('nestedInputContainer');
                const newLabelInput = document.getElementById('newLabelInput');


                // label selecting code
                var labelsValues = []; // to maintain unique labels 
                function insertANewLabel () {
                    var selectedLabel = labelSelect.value;
                    if (!labelsValues.includes(selectedLabel) && selectedLabel !== 'create-new') {
                        var labelElement = document.createElement('p');
                        labelElement.textContent = '#' + selectedLabel;
                        labelElement.id = selectedLabel;

                        var crossButton = document.createElement('button');
                        crossButton.classList.add('cross-button');
                        crossButton.innerHTML = '<i class="fas fa-times"></i>';
                        crossButton.id = selectedLabel;

                        // Add event listener to cross button
                        crossButton.addEventListener('click', function (event) {
                            labelElement.remove(); // Remove the label element
                            labelsValues = labelsValues.filter(value => value !== selectedLabel); // Remove the value from the array
                            labelsInputs = labelsInputs.filter(element => element.id !== event.target.id);
                        });

                        labelElement.appendChild(crossButton);
                        labelsContainer.appendChild(labelElement);
                        labelsInputs.push(labelElement);
                        
                    } else if(selectedLabel === 'create-new'){ 
                        nestedInputContainer.style.display = 'block';
                    }
                }


                newLabelInput.addEventListener('keypress', function (event) {
                    if(event.key === 'Enter') {
                        const newLabel = newLabelInput.value;
                        if (newLabel) {
                            addNewLabel(newLabel, userData.id)
                                .then(function (status) {
                                    newLabelInput.value = '';
                                    nestedInputContainer.style.display = 'none';
                                    var newLabelElement = document.createElement('option');
                                    newLabelElement.value = newLabel;
                                    newLabelElement.innerText = newLabel;
                                    var labelsSelect = document.getElementById('labelsSelect');
                                    var lastOptionElement = labelSelect.lastElementChild;
                                    labelsSelect.insertBefore(newLabelElement, lastOptionElement);
                                });
                        }
                    }
                });

                function modifySaveButton (event) {
                    if (event.target.value.trim() == '') {
                        statusObj.isTitleAdded = false;
                        saveButton.disabled = true;
                    } else {
                        statusObj.isTitleAdded = true;
                        saveButton.disabled = false;
                    }
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
                todoInput.addEventListener('keypress', function (event) {
                    if (event.key === 'Enter') {
                        event.preventDefault();

                        var todoText = this.value.trim();
                        if (todoText !== '') {
                            var todoItem = document.createElement('div');
                            todoItem.classList.add('todo-item');
                            var todoTextElement = document.createElement('span');
                            todoTextElement.textContent = todoText;
                            var removeButton = document.createElement('button');
                            removeButton.classList.add('btn', 'remove-todo');
                            removeButton.innerHTML = '<i class="fas fa-times"></i>';

                            todoItem.appendChild(removeButton);
                            todoItem.appendChild(todoTextElement);
                            var addedTodos = document.getElementById('added-todos');
                            addedTodos.appendChild(todoItem);
                            this.value = '';
                            this.focus();
                            this.focus();
                        }
                    }
                })
                document.addEventListener('click', function (event) {
                    if (event.target.classList.contains('remove-todo')) {
                        var todoItem = event.target.closest('.todo-item');
                        todoItem.remove();
                    }
                });
                remainderButton.addEventListener('click', function () { })
                noteTitle.addEventListener('input', modifySaveButton);

                // attachment related code
                $("#attachment-button").click(function () {
                    $("#file-input").click();
                });
                pdfjsLib.GlobalWorkerOptions.workerSrc = 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/2.8.335/pdf.worker.min.js';

                $("#file-input").change(function () {
                    statusObj.isFilesAdded = true;
                    totalSelectedAttachment++;
                    var file = this.files[0];
                    lastSelectedFile = file;
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
                                    if (totalSelectedAttachment === 0) {
                                        statusObj.isFilesAdded = false;
                                    }
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
                                                if (totalSelectedAttachment === 0) {
                                                    statusObj.isFilesAdded = false;
                                                }
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
                        statusObj.isRemainderAdded = true;
                        datePicker.style.display = "block";
                        // Set default date and time one hour later
                        var currentDate = new Date();
                        currentDate.setHours(currentDate.getHours() + 1);
                        var defaultDateTime = currentDate.toISOString().slice(0, 16);
                        datePicker.value = defaultDateTime;
                        note.IsRemainderAdded = true;

                    } else {
                        statusObj.isRemainderAdded = false;
                        datePicker.style.display = "none";
                        note.IsRemainderAdded = false;
                    }

                });

                // Handle date and time selection
                datePicker.addEventListener("changeDate", function (event) {
                    var selectedDateTime = event.target.value;
                    console.log(selectedDateTime);
                });

                // Saving the draft note 
                $("#myModal").on("hide.bs.modal", function () {
                    if (totalSelectedAttachment === totalUploadedAttachment) {
                        note.Id = noteData.id;
                        note.AddedBy = noteData.authorId;
                        note.Todos = JSON.stringify(getTodoInputs());
                        note.Title = noteTitle.value;
                        note.Description = noteDescription.value;
                        note.Labels = JSON.stringify(getLabelsInputs());
                        note.Type = null;


                        if (note.Description !== '') {
                            statusObj.isDescriptionAdded = true;
                        } else {
                            statusObj.isDescriptionAdded = false;
                        }


                        // remainder
                        if (note.IsRemainderAdded) {
                            note.RemainderDateTime = datePicker.value;
                            console.log(note.RemainderDateTime);
                        }

                        if (!statusObj.isSaveClicked) {
                            if (statusObj.isTitleAdded ||
                                statusObj.isDescriptionAdded ||
                                statusObj.isFilesAdded ||
                                statusObj.isLabelAdded ||
                                statusObj.isTodoAdded ||
                                statusObj.isRemainderAdded
                            ) {
                                saveNote(note)
                                    .then(function (addedNote) {
                                        $('#myModal').modal('hide');
                                    });
                            } else {
                                deleteNote(note.Id, note.AuthorId)
                                    .then(function (addedNote) {
                                        $('#myModal').modal('hide');
                                    });
                            }
                        } else {
                            
                        }

                    } else {
                        alert('Please wait.. Files uploading');
                    }
                });
               
                // Saving the note
                $('#save-button').off('click').on('click', function () {
                    if (totalSelectedAttachment === totalUploadedAttachment) {
                        statusObj.isSaveClicked = true;
                        note.Id = noteData.id;
                        note.AddedBy = noteData.authorId;
                        note.Todos = JSON.stringify(getTodoInputs());
                        note.Title = noteTitle.value;
                        note.Description = noteDescription.value;
                        note.Labels = JSON.stringify(getLabelsInputs());
                        note.Type = null;
                        note.IsDraft = false;

                        // remainder
                        if (note.IsRemainderAdded) {
                            note.RemainderDateTime = datePicker.value;
                            console.log(note.RemainderDateTime);
                        }
                        saveNote(note)
                            .then(function (addedNote) {
                                $('#myModal').modal('hide');
                                showSingleRawCardTop(addedNote);
                                showLinksPerNoteSingle(addedNote);
                                fetchAttachmentAllForANote(addedNote.id)
                                    .then(function (attachments) {
                                        if (attachments.length > 0) {
                                            showAttachmentPreviewToEachCardSingle(attachments[0]);
                                        }
                                        if (addedNote.isRemainderAdded) {
                                            showRemainderCountDown(addedNote);
                                        }
                                    })
                                var noteTitle = document.getElementById(`title-${addedNote.id}`);
                                noteTitle.addEventListener('click', handleNoteTitleClick);
                            });
                    } else {
                        alert('Please wait.. Files uploading');
                    }

                });

                authorizationButton.addEventListener('click', function () {
                    // clear the view and add the necessary view
                    const modalHeaderElements = document.getElementById('modal-header-element');
                    const updateModalBody = document.getElementById('update-modal-body');
                    const collaboratorModalBody = document.getElementById('collaborator-modal-body');
                    const updateModalFooter = document.getElementById('update-modal-footer');
                    const collaboratorModalFooter = document.getElementById('collaborator-modal-footer');

                    //modalHeaderElements.style.display = "none";
                    updateModalBody.style.display = "none";
                    updateModalFooter.style.display = "none";
                    collaboratorModalBody.style.display = "block";
                    collaboratorModalFooter.style.display = "block";
                    collaboratorModalFooter.style.display = "block";

                    const collaboratorDoneButton = document.getElementById('collaborator-done-button');
                    collaboratorDoneButton.addEventListener('click', function () {
                        updateModalBody.style.display = "block";
                        updateModalFooter.style.display = "block";
                        collaboratorModalBody.style.display = "none";
                        collaboratorModalFooter.style.display = "none";
                        collaboratorModalFooter.style.display = "none";
                    })

                    // perform search and append view with click listener
                    const collaboratorSearchBar = document.getElementById('collaborator-search');
                    const collaboratorContainer = document.getElementById('collaborator-container');
                    const selectedCollaboratorsContainer = document.getElementById('selected-collaborators-container');

                    let timeout;
                    collaboratorSearchBar.addEventListener('input', function (event) {
                        clearTimeout(timeout);
                        timeout = setTimeout(function () {
                            if (collaboratorSearchBar.value !== '') {
                                fetchCollaborators(collaboratorSearchBar.value)
                                    .then(function (fetchedCollaborators) {
                                        renderCollaboratorSearchResults(fetchedCollaborators, collaboratorContainer);
                                        addEventListenerToAllSearchedResult(collaboratorContainer, noteData);
                                    });
                            } else {
                                collaboratorContainer.innerHTML = '<p style="text-align:center;">Nothing to show<p>';
                            }

                        }, 500); // Delay in milliseconds
                    });
                });
            })
            .fail(function (error) {
                console.error("Error in nested AJAX calls");
                console.error(error);
            });
    });
});