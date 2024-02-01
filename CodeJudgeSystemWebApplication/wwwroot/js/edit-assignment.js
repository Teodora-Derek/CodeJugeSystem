document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);
    const assignmentId = urlParams.get('assignmentId');
    fetchAssignmentData(assignmentId);

    const form = document.getElementById('editAssignmentForm');
    form.onsubmit = function (event) {
        event.preventDefault();
        updateAssignment(assignmentId);
    };
});

function fetchAssignmentData(assignmentId) {
    fetch(`http://localhost:5026/api/assignments/${assignmentId}`)
        .then(response => response.json())
        .then(assignment => {
            document.getElementById('editSubject').value = assignment.subject;
            document.getElementById('editAssignmentName').value = assignment.assignmentName;
            document.getElementById('editDueDate').value = assignment.dueDate.slice(0, 16);
            document.getElementById('editCourse').value = assignment.course;
            document.getElementById('editSemester').value = assignment.semester;
            document.getElementById('editTargetGroup').value = assignment.targetGroup;
            document.getElementById('editDescription').value = assignment.description;
            document.getElementById('editExpectedInputAndOutputPairs').value = assignment.expectedInputAndOutputPairs;
        })
        .catch(error => console.error('Error fetching assignment:', error));
}

function updateAssignment(assignmentId) {
    const updatedAssignment = {
        id: assignmentId,
        subject: document.getElementById('editSubject').value,
        assignmentName: document.getElementById('editAssignmentName').value,
        dueDate: document.getElementById('editDueDate').value,
        course: document.getElementById('editCourse').value,
        semester: document.getElementById('editSemester').value,
        targetGroup: document.getElementById('editTargetGroup').value,
        description: document.getElementById('editDescription').value,
        expectedInputAndOutputPairs: document.getElementById('editExpectedInputAndOutputPairs').value
    };

    fetch(`http://localhost:5026/api/assignments`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(updatedAssignment)
    })
        .then(response => {
            if (response.ok) {
                window.location.href = 'http://localhost:5026/assignments.html';
            } else {
                console.error('Failed to update assignment');
                alert('There was an error updating the assignment.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('There was an error updating the assignment.');
        });
}