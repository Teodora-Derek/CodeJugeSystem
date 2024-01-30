document.addEventListener('DOMContentLoaded', function () {
    fetchAssignments();
});

function fetchAssignments() {
    fetch('http://localhost:5026/api/assignments')
        .then(response => response.json())
        .then(data => {
            displayAssignments(data);
        })
        .catch(error => {
            console.error('Error fetching data: ', error);
        });
}

function displayAssignments(assignments) {
    const container = document.getElementById('assignments-grid');
    container.innerHTML = '';

    assignments.forEach(assignment => {
        const card = document.createElement('div');
        card.className = 'assignment';
        card.innerHTML = `
            <h2>Subject: <span>${assignment.subject}</span></h2>
            <p>Assignment Name: <span>${assignment.assignmentName}</span></p>
            <p>Due Date: <span>${assignment.dueDate}</span></p>
            <p>Course: <span>${assignment.course}</span></p>
            <p>Semester: <span>${assignment.semester}</span></p>
            <p>Target Groups: <span>${assignment.targetGroup}</span></p>
            <p>Description: <span>${assignment.description}</span></p>
            <p>Expected Input and Output: <span>${assignment.expectedInputAndOutputPairs}</span></p>
        `;
        container.appendChild(card);
    });
}

document.getElementById('createAssignmentBtn').onclick = function () {
    document.getElementById('createAssignmentModal').style.display = 'block';
};

document.getElementsByClassName('close')[0].onclick = function () {
    document.getElementById('createAssignmentModal').style.display = 'none';
};

window.onclick = function (event) {
    if (event.target == document.getElementById('createAssignmentModal')) {
        document.getElementById('createAssignmentModal').style.display = 'none';
    }
};

document.getElementById('createAssignmentForm').onsubmit = function (event) {
    submitAssignment();
};

function submitAssignment() {
    const assignmentData = {
        subject: document.getElementById('subject').value,
        assignmentName: document.getElementById('assignmentName').value,
        dueDate: document.getElementById('dueDate').value,
        course: document.getElementById('course').value,
        semester: document.getElementById('semester').value,
        targetGroup: document.getElementById('targetGroup').value,
        description: document.getElementById('description').value,
        expectedInputAndOutputPairs: document.getElementById('expectedInputAndOutputPairs').value
    };


    fetch('http://localhost:5026/api/assignments', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(assignmentData),
    })
        .then(response => response.json())
        .then(data => {
            console.log('Success:', data);
            document.getElementById('createAssignmentModal').style.display = 'none';
        })
        .catch((error) => {
            console.error('Error:', error);
        });
}
