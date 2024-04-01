const uri = '/TaskList';
let Tasks = [];
let token = localStorage.getItem("token");

const ifAdmin = () => {
    const tokenParts = token.split('.');
    const payload = JSON.parse(atob(tokenParts[1]));
    const userType = payload.type;
    let linkToUsers =  document.getElementById('linkToUsers');
    if (userType[1]==="admin"){
        linkToUsers.style.display = 'block';
    }
    alert(userType);
}

const ifTokenExpired = () => {
if (token) {
    const tokenData = parseJwt(token);
    const expirationTime = tokenData.exp * 1000; 
    
    if (Date.now() > expirationTime) 
        window.location.href = '/login.html'; 
}}


var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer " + token);
myHeaders.append("Content-Type", "application/json");

const getItems = (token) => {

    ifAdmin();
    ifTokenExpired();

    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };

    fetch(uri, requestOptions)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

const addItem = () => {
    const addNameTextbox = document.getElementById('add-name');

    const item = {
        isDone: false,
        name: addNameTextbox.value.trim(),
    };

    fetch(uri, {
        method: 'POST',
        headers: myHeaders,
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems(token);
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: myHeaders,
    })
        .then(() => getItems(token))
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = Tasks.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-password').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isManeger').checked = item.isManeger;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isDone: document.getElementById('edit-isManeger').checked,
        name: document.getElementById('edit-name').value.trim(),
        password: document.getElementById('edit-password').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: myHeaders,
        body: JSON.stringify(item)
    })
        .then(() => getItems(token))
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'task' : 'task kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('Tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    Tasks = data;
}


getItems(token);