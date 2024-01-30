const toggleRegisterStudentBtn = () => {
    var registerContent = document.getElementById('registerStudentContent');
    var assignmentContent = document.getElementById('createAssignmentContent');

    var registerDisplayPropertyValue = registerContent.style.getPropertyValue('display');;

    if (registerDisplayPropertyValue === 'block') {
        registerContent.style.display = 'none';
        assignmentContent.style.display = 'none';
    } else {
        registerContent.style.display = 'block';
        assignmentContent.style.display = 'none';
    }
}

const toggleCreateAssignmentBtn = () => {
    var assignmentContent = document.getElementById('createAssignmentContent');
    var registerContent = document.getElementById('registerStudentContent');

    var assignmentsDisplayPropertyValue = assignmentContent.style.getPropertyValue('display');

    if (assignmentsDisplayPropertyValue === 'block') {
        assignmentContent.style.display = 'none';
        registerContent.style.display = 'none';
    } else {
        assignmentContent.style.display = 'block';
        registerContent.style.display = 'none';
    }
}

var dynamicIdCounter = 1;

const addNewIOPair = () => {
    var dynamicId = 'IOPairId_' + dynamicIdCounter;

    var newIOPairHtmlCode = '<div id="' + dynamicId + '" style="" class="grunion-field-text-wrap grunion-field-wrap">' +
        '<label for="expectedInputAndOutputPairs" class="grunion-field-label text">Expected input ' + dynamicIdCounter + '</label>' +
        '<input type="text" name="g27-additionalattachments" id="expectedInputAndOutputPairs" value="" class="text grunion-field">' +
        '<label for="expectedInputAndOutputPairs" class="grunion-field-label text">Expected output ' + dynamicIdCounter + '</label>' +
        '<input type="text" name="g27-additionalattachments" id="expectedInputAndOutputPairs" value="" class="text grunion-field">' +
        '</div>';

    console.log('Added pair_id: ' + dynamicIdCounter);

    dynamicIdCounter++;

    var IOPairBtnsWrapper = document.getElementById('IOPairBtnsWrapper');
    IOPairBtnsWrapper.insertAdjacentHTML('beforebegin', newIOPairHtmlCode);
}

const removeLastIOPair = () => {
    if (dynamicIdCounter <= 1) {
        return;
    }

    var assignmentFormContentsDiv = document.getElementById('assignmentFormContentsDiv');

    dynamicIdCounter--;

    var dynamicId = 'IOPairId_' + dynamicIdCounter;

    var lastIOPairHtmlCode = document.getElementById(dynamicId);

    if (lastIOPairHtmlCode) {
        console.log('Tried to remove pair_id: ' + dynamicIdCounter);
        assignmentFormContentsDiv.innerHTML = assignmentFormContentsDiv.innerHTML.replace(lastIOPairHtmlCode.outerHTML, '');
    }
}