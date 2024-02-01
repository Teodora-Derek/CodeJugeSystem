document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);
    const assignmentId = urlParams.get('assignmentId');
    console.log(assignmentId)

    if (assignmentId) {
        const assignmentDiv = document.querySelector('.assignment');
        const header = document.createElement('h1');
        header.textContent = `Assignment ${assignmentId}`;
        assignmentDiv.appendChild(header);

        fetch(`./api/assignments/${assignmentId}`)
            .then(response => response.json())
            .then(assignment => {
                const subject = document.createElement('p');
                const name = document.createElement('p');
                subject.textContent = `Subject: ${assignment.subject}`;
                name.textContent = `${assignment.assignmentName}`;
                assignmentDiv.appendChild(subject);
                assignmentDiv.appendChild(name);
            })
    }
});


const input = document.getElementById('file-input');
const fileName = document.createElement('p');

function updateFilePreview () {
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


const uploadFilesUrl = 'api/Files/upload';
const getFilesUrl = 'api/files?assignmentId=${assignmentId}';

const uploadFiles = () => {
    const file = input.files[0];
    if (!file) {
        alert('Please, select file first!');
        return;
    }

    const urlParams = new URLSearchParams(window.location.search);
    const assignmentId = urlParams.get('assignmentId');
    const formData = new FormData();
    formData.append('File', file);

    fetch(`/api/files/upload?assignmentId=${assignmentId}`, {
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
            updateGrade(data);
        })
        .catch(error => {
            console.error('Error during file upload:', error);
        });
}

function updateGrade(data) {
    const gradeElement = document.querySelector('.grade h2');
    gradeElement.textContent = `Final grade: ${data}`;
}

const showGrade = async () => {
    const urlParams = new URLSearchParams(window.location.search);
    const assignmentId = urlParams.get('assignmentId');
    const response = await fetch(`api/files/grade?assignmentId=${assignmentId}`);
    const gradeData = await response.text();
    console.log(gradeData)
    updateGrade(gradeData);
}

const updateHistory = async () => {

    const historyContent = document.getElementById('historyContent');
    const urlParams = new URLSearchParams(window.location.search);
    const assignmentId = urlParams.get('assignmentId');
    const SUBMISSIONS_SHOWN = 10;

    try {
        historyContent.innerHTML = "";
        const response = await fetch(`api/files?assignmentId=${assignmentId}`);
        const result = await response.json();

        result.sort((a, b) => new Date(b.uploadTime) - new Date(a.uploadTime));

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

window.addEventListener('load', showGrade);