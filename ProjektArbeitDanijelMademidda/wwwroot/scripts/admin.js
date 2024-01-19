async function submitAdminForm() {

    const username = document.querySelector('#username-input').value;
    const password = document.querySelector('#password-input').value;

    try {
        const loginInfo = await adminLogin(username, password);
        localStorage.setItem('jwt-token', loginInfo.jwt);
        window.location.href = '../admin';
    }
    catch (err) {
        document.querySelector('#login-error').innerText = err.message;
    }
}