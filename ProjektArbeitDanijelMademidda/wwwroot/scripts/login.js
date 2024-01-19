
async function submitLoginForm() {

    const username = document.querySelector('#username-input').value;
    const password = document.querySelector('#password-input').value;

    try {
        const loginInfo = await login(username, password);
        localStorage.setItem('jwt-token', loginInfo.jwt);
        window.location.href = '../';
    }
    catch (err) {
        document.querySelector('#login-error').innerText = err.message;
    }
}

//document.querySelector('form').addEventListener('submit', submitLoginForm);
