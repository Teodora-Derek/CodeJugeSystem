const input = document.getElementById('file-input');
const fileName = document.createElement('p');

const updateFilePreview = () => {
    const file = input.files[0];

    if (file) {
        console.log('Selected file:', file);
        fileName.textContent = file.name;
    }
}

const removeFile = () => {
    const file = input.files[0];

    if (file) {
        input.textContent = 'No file selected';
    }
}

/*-------------------- User side(top) / Server side(bottom) -------------------*/

const uploadFilesUrl = 'api/Files/upload';
const getFilesUrl = 'api/Files/';

const uploadFiles = () => {
    const file = input.files[0];
    if (!file) {
        alert('Please, select file first!');
        return;
    }

    const formData = new FormData();
    formData.append('File', file);

    fetch(uploadFilesUrl, {
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
            updateHistory();
        })
        .catch(error => {
            console.error('Error during file upload:', error);
        });
}

const updateHistory = async () => {

    const historyContent = document.getElementById('historyContent');
    const SUBMISSIONS_SHOWN = 10;

    try {
        historyContent.innerHTML = "";
        const response = await fetch(getFilesUrl);
        const result = await response.json();

        console.log(result);

        for (let i = 0; i < Math.min(result.length, SUBMISSIONS_SHOWN); i++) {
            let submission = document.createElement("p");
            submission.textContent += "File " + result[i].fileName;
            submission.textContent += " uploaded at: " + result[i].uploadTime;

            historyContent.appendChild(submission);
        }
    } catch (error) {
        console.log("Error:", error);
    }
}

window.addEventListener('load', updateHistory);