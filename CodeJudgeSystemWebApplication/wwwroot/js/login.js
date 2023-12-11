document.addEventListener('DOMContentLoaded', function () {
    // Wait for the DOM content to be fully loaded

    const form = document.querySelector('form');

    form.addEventListener('submit', function (event) {
        event.preventDefault(); // Prevent the default form submission

        // Get form data
        const formData = new FormData(form);
        const username = formData.get('username');
        const password = formData.get('password');

        // Create a query string from form data
        const queryString = querystring.stringify({
            username: username,
            password: password
        });

        // Make the fetch request
        fetch('https://localhost:7015/upload', {
            method: 'POST',
            body: queryString,
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            // Handle the response data
            console.log(data);
        })
        .catch(error => {
            // Handle errors
            console.error('Error:', error);
        });
    });
});
