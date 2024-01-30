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