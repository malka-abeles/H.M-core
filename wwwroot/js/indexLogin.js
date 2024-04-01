
const uri = "/login";

const getToken = ()=> {
    localStorage.clear();

    const name = document.getElementById('name');
    const password = document.getElementById('password');

    const user = {
        Id:4,
        Name: name.value.trim(),
        Password: password.value.trim(),
        UserType: 0
    };

    fetch(uri, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(user)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Unauthorized');
            }
            return response.json();
        })
        .then(token => {
            localStorage.setItem('token', token);
            window.location.href = '../tasks.html';
        })
        .catch(error => {
            alert(error.message);
        });

}

const form = document.getElementById('formSignUp');
form.onsubmit =(event ) =>{
    event.preventDefault();
    getToken();
}