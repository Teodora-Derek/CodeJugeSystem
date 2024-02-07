async function signUpUser() {
    const signUpData = {
        fullName: '',  
        email: '',  
        password: ''  
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
signUpUser();