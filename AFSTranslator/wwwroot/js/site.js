document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.getElementById('loginForm');

    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();

            const username = document.getElementById('username').value;
            const password = document.getElementById('password').value;

            console.log(username);

            const response = await fetch('/auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password })
            });

            const data = await response.json();

            console.log(data);
            
            if (response.status === 200 && data.content.token) {
                localStorage.setItem('jwtToken', data.content.token);
                window.location.href = '/home/index';
            }
            else{
                // alert(data.error || 'Login failed');
                loginError.style.display = 'block';
                loginError.textContent = data.errorMessage || 'Login failed';
            }
        });
    }
});

document.addEventListener('DOMContentLoaded', () => {
    const token = localStorage.getItem('jwtToken');

    const loginButton = document.getElementById('loginButton');
    const registrationButton = document.getElementById('registrationButton');
    const logoutButton = document.getElementById('logoutButton');
    
    if (token) {
        if (loginButton) loginButton.style.display = 'none';
        if (logoutButton) logoutButton.style.display = 'block';
        if (registrationButton) registrationButton.style.display = 'none';
    } else {
        if (loginButton) loginButton.style.display = 'block';
        if (logoutButton) logoutButton.style.display = 'none';
        if (registrationButton) registrationButton.style.display = 'block';
    }

    if (logoutButton) {
        logoutButton.addEventListener('click', () => {
            localStorage.removeItem('jwtToken');
            window.location.href = '/home/index';
        });
    }
});

function signOut() {
    localStorage.removeItem('jwtToken');
    window.location.href = '/Auth/Login';
}