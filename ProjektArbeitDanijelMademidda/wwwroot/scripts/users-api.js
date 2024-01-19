const environment = { apiRoot: location.origin };

async function login(username, password) {
    const url = `${environment.apiRoot}/api/users/login`
    const request = await fetch(url, {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        cache: 'no-cache',
        method: 'POST',
        body: JSON.stringify({ 'username': username, 'password': password })
    });
    const data = await request.json();
    return data;
}

async function register(username, password) {
    const url = `${environment.apiRoot}/api/users/register`
    const request = await fetch(url, {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        cache: 'no-cache',
        method: 'POST',
        body: JSON.stringify({ 'username': username, 'password': password })
    });
    const data = await request.json();
    return data;
}


async function adminLogin(username, password) {
    const url = `${environment.apiRoot}/api/admin/adminLogin`
    const request = await fetch(url, {
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        cache: 'no-cache',
        method: 'POST',
        body: JSON.stringify({ 'username': username, 'password': password })
    });
    const data = await request.json();
    return data;
}
