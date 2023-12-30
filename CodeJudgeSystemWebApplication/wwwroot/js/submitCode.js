let files = [];

function updateFilePreview() {
    console.log('files in top of update: ', files);
    const input = document.getElementById('files');
    const previewContainer = document.getElementById('file-preview');
    const selectedFilesTextArea = document.getElementById('selected-files');

    // Clear previous previews
    previewContainer.innerHTML = '';

    for (let i = 0; i < input.files.length; i++) {
        const file = input.files[i];

        // Create a div for each file preview
        const filePreview = document.createElement('div');
        filePreview.className = 'file-preview';

        console.log('File size: ', file.size);

        // Create a div for file information
        const fileInfo = document.createElement('div');
        fileInfo.className = 'file-info';

        // Create a span for the file name
        const fileName = document.createElement('span');
        fileName.textContent = file.name;

        // Create a "Remove" button
        const removeButton = document.createElement('span');
        removeButton.className = 'remove-btn';
        removeButton.textContent = 'Remove';
        removeButton.onclick = () => removeFile(file);

        // Append elements to the preview container
        fileInfo.appendChild(fileName);
        filePreview.appendChild(fileInfo);
        filePreview.appendChild(removeButton);
        previewContainer.appendChild(filePreview);

        console.log('files before insertion: ', files);
        // Add the file to the files array if it's not already present
        if (!files.some(existingFile => existingFile.name === file.name)) {
            files.push(file);
        }
        console.log('files after insertion: ', files);
    }

    // Update the files textarea
    selectedFilesTextArea.value = files.map(file => file.name).join('\n');
    console.log('selectedFilesTextArea.value: ', selectedFilesTextArea.value);
}

function removeFile(fileToRemove) {
    // Remove the file from the files array based on the file name
    files = files.filter(existingFile => existingFile.name !== fileToRemove.name);
    console.log('filtered files: ', files);

    // Update the file preview
    updateFilePreview();
}

/*-------------------- User side(top) / Server side(bottom) -------------------*/

const url = 'api/Files/upload';

function uploadFiles() {

    if (files.length <= 0) {
        //TODO: Show message to the user in the form!
        console.log('Please, select file first!');
    }

    const formData = new FormData();

    for (var i = 0; i < files.length; i++) {
        formData.append('File', files[i]);
    }

    fetch(url, {
        method: 'POST',
        body: formData,
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('File upload failed. Status: ' + response.status);
            }
            return response.text();
        })
        .then(data => {
            console.log('File uploaded successfully!', data);
            // Tell the user the data is saved successfuly
            updateHistory();
        })
        .catch(error => {
            console.error('Error during file upload:', error);
        });
}

function getFiles() {
    fetch(url)
        .then(response => response.json())
        .then(data => {
            console.log('GET request succeeded with JSON response', data);
        })
        .catch(error => {
            console.error('Error making GET request', error);
        });
}

function updateHistory() {
    // Assuming you have a div with the id "uploadHistory" to display the history
    const uploadHistoryDiv = document.getElementById('uploadHistory');

    // Assuming history is an array of objects with DocumentID, FileName, and UploadTime properties
    history.forEach(item => {
        const historyItem = document.createElement('div');
        historyItem.textContent = `DocumentID: ${item.documentID}, FileName: ${item.fileName}, UploadTime: ${item.uploadTime}`;
        uploadHistoryDiv.appendChild(historyItem);
    });
}

window.addEventListener('load', updateHistory);
