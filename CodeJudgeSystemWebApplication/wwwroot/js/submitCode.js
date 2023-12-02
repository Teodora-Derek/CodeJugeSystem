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

const url = 'api/Files';

// Upload a file
function uploadFiles() {
    console.log('Uploading files:', files);
    console.log('files.length: ', files.length);
    if (files.length > 0) {
        var formData = new FormData();

        // Append each selected file to the FormData object
        for (var i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }

        fetch(url, {
            method: 'POST',
            body: formData,
        })
            .then(response => {
                const contentType = response.headers.get('content-type');
                console.log('Content-Type:', contentType);

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                // Assuming the server returns the .cs file content as plain text
                return response.text();
            })
            .then(data => {
                // Handle the successful response from the server
                console.log('File uploaded successfully!', data);
                // You can now process the file content as needed (e.g., store it in the database)
            })
            .catch(error => {
                console.error('Error during file upload:', error);
            });
    } else {
        console.error('No files selected.');
    }
}

//const url = 'api/Files';

//// Upload a file
//function uploadFile() {

//    if (files.length > 0) {
//        var formData = new FormData();

//        // Append each selected file to the FormData object
//        for (var i = 0; i < files.length; i++) {
//            formData.append('files[]', files[i]);
//        }

//        var xhr = new XMLHttpRequest();
//        xhr.open('POST', url, true);

//        xhr.onreadystatechange = function () {
//            if (xhr.readyState === 4 && xhr.status === 200) {
//                // Handle the successful response from the server
//                console.log('Files uploaded successfully!');
//            }
//        };

//        xhr.upload.onprogress = function (e) {
//            if (e.lengthComputable) {
//                var percentage = (e.loaded / e.total) * 100;
//                console.log('Upload progress: ' + percentage + '%');
//            }
//        };

//        // Send the FormData object containing the files to the server
//        xhr.send(formData);
//    } else {
//        console.error('No files selected.');
//    }
//}

// Get the files by id
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