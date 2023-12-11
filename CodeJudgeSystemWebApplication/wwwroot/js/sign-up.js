async function signUpUser() {
    const signUpData = {
        fullName: 'John Doe',  // Replace with actual data from your form
        email: 'john.doe@example.com',  // Replace with actual data from your form
        password: 'password123'  // Replace with actual data from your form
    };

    try {
        const response = await fetch('https://example.com/signup', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(signUpData),
        });

        if (response.ok) {
            console.log('User successfully signed up!');
        } else {
            console.error(`Failed to sign up user. Status: ${response.status}`);
        }
    } catch (error) {
        console.error(`Error during sign-up: ${error.message}`);
    }
}

// Call the function to sign up the user when the script is executed
signUpUser();
