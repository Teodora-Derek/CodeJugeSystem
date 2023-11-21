document.addEventListener('DOMContentLoaded', function () {
    // Wait for the DOM content to be fully loaded

    const form = document.querySelector('form'); // Assuming you have only one form on the page

    form.addEventListener('submit', function (event) {
        event.preventDefault(); // Prevent the default form submission

        // Get form data
        const formData = new FormData(form);

        // Make a POST request to your ASP.NET Core Web API endpoint
        fetch('https://your-api-endpoint.com/upload', {
            method: 'POST',
            body: formData,
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json(); // Assuming your API returns JSON data
            })
            .then(data => {
                // Handle the response from the server
                console.log('Server response:', data);
                // You can update the UI or perform other actions based on the response
            })
            .catch(error => {
                console.error('Error:', error);
                // Handle errors, such as network issues or server errors
            });
    });
});
