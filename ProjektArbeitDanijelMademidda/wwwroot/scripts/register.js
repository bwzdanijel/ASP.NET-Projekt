
async function submitRegisterForm() {
    
    const username = document.querySelector('#username-input').value;
    const password = document.querySelector('#password-input').value;
    const passwordConfirm = document.querySelector('#password-confirm').value;
    
    if (password === passwordConfirm) {
        try {
            const loginInfo = await register(username, password);
            localStorage.setItem('jwt-token', loginInfo.jwt);
            window.location.href = '../';
        }
        catch (err) {
            document.querySelector('#register-error').innerText = err.message;
        }
    }
    else {
        document.querySelector('#register-error').innerText = "Password confirmation didn't match.";
    }
}

