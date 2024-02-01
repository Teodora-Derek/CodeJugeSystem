document.addEventListener('DOMContentLoaded', function () {
    fetchAssignments();
});

function fetchAssignments() {
    document.getElementById('loadingIndicator').style.display = 'block';

    fetch('http://localhost:5026/api/assignments')
        .then(response => response.json())
        .then(data => {
            document.getElementById('loadingIndicator').style.display = 'none';
            displayAssignments(data);
        })
        .catch(error => {
            console.error('Error fetching data: ', error);
            document.getElementById('loadingIndicator').style.display = 'none';
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
            <button class="viewBtn">View Assignment</button>
        `;

        const viewBtn = card.querySelector('.viewBtn');
        viewBtn.onclick = function () {
            openAssignmentModal(assignment.id);
        };
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
    window.location.reload();
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

let currentAssignmentId = null;

function openAssignmentModal(assignmentId) {
    currentAssignmentId = assignmentId;

    fetch(`http://localhost:5026/api/assignments/${assignmentId}`)
        .then(response => response.json())
        .then(assignment => {
            document.getElementById('modalSubject').textContent = assignment.subject;
            document.getElementById('modalAssignmentName').textContent = assignment.assignmentName;
            document.getElementById('modalDueDate').textContent = new Date(assignment.dueDate).toLocaleString();
            document.getElementById('modalCourse').textContent = assignment.course;
            document.getElementById('modalSemester').textContent = assignment.semester;
            document.getElementById('modalTargetGroup').textContent = assignment.targetGroup;
            document.getElementById('modalDescription').textContent = assignment.description;
            document.getElementById('modalExpectedInputAndOutputPairs').textContent = assignment.expectedInputAndOutputPairs;

            document.getElementById('assignmentModal').style.display = 'block';
        })
        .catch(error => console.error('Error:', error));
}


function closeAssignmentModal() {
    document.getElementById('assignmentModal').style.display = 'none';
}


function editAssignment(assignmentId) {
    window.location.href = `edit-assignment.html?assignmentId=${assignmentId}`;
}

function deleteAssignment(assignmentId) {
    if (!confirm('Are you sure you want to delete this assignment?')) {
        return;
    }

    fetch(`http://localhost:5026/api/assignments/${assignmentId}`, {
        method: 'DELETE',
    })
        .then(response => {
            if (response.ok) {
                console.log('Assignment deleted');
                closeAssignmentModal();
                window.location.reload();
            } else {
                console.error('Error deleting assignment');
            }
        })
        .catch(error => console.error('Error:', error));
}

