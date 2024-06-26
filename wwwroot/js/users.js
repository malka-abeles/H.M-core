const uri = '/User';
let Users = [];
let token = localStorage.getItem("token");

var myHeaders = new Headers();
myHeaders.append("Authorization", "Bearer " + token);
myHeaders.append("Content-Type", "application/json");

const getItems = ()=> {

    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };

    fetch(uri,requestOptions)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

const addItem=() =>{
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox = document.getElementById('add-password');


    const item = {
        name: addNameTextbox.value.trim(),
        password: addPasswordTextbox.value.trim(),
    };

    fetch(uri, {
            method: 'POST',
            headers: myHeaders,
            body: JSON.stringify(item)
        })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
            addPasswordTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
            method: 'DELETE',
            headers: myHeaders,
        })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = Users.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isManeger').checked = item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

//This challenge(to update the detiles of the user) is not ready yet.
// function updateItem() {
//     const itemId = document.getElementById('edit-id').value;
//     const item = {
//         id: parseInt(itemId, 10),
//         isDone: document.getElementById('edit-isManeger').checked,
//         name: document.getElementById('edit-name').value.trim()
//     };

//     fetch(`${uri}/${itemId}`, {
//             method: 'PUT',
//             headers: myHeaders,
//             body: JSON.stringify(item)
//         })
//         .then(() => getItems())
//         .catch(error => console.error('Unable to update item.', error));

//     closeInput();

//     return false;
// }

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'user' : 'user kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('Users');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = !item.userType;

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
        let textNode1 = document.createTextNode(item.password);
        td3.appendChild(textNode1);

        //This challenge(to update the detiles of the user) is not ready yet.
        // let td4 = tr.insertCell(3);
        // td4.appendChild(editButton);

        let td5 = tr.insertCell(3);
        td5.appendChild(deleteButton);
    });

    Users = data;
}


getItems();