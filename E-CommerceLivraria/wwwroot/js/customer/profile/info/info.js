import { verifyPassword } from '../../../extra/verifyPassword.js'

window.PassFunctions = {
    verifyPassword
}

document.getElementById('updatePasswordBtn').addEventListener('click', passDivDisplay);
document.getElementById('cancelPassUpdt').addEventListener('click', passDivDisplay);
document.getElementById('beginPassUpdt').addEventListener('click', updatePassword);
document.getElementById('updateDataBtn').addEventListener('click', enableChangesField);
document.getElementById('updateGeneralInfo').addEventListener('click', UpdateInfo);
document.getElementById('cancelInfoUpdt').addEventListener('click', disableChangesField);

function passDivDisplay() {
    const curDisplay = document.getElementById('passwordDiv').style.display

    if (curDisplay == 'none' || !curDisplay) {
        document.getElementById('passwordDiv').style.display = 'flex'

        document.getElementById('beginPassUpdt').disabled = false;
        document.getElementById('cancelPassUpdt').disabled = false;
    } else {
        document.getElementById('passwordDiv').style.display = 'none'
    }
}

function enableChangesField() {
    document.getElementById('id').disabled = false;
    document.getElementById('name').disabled = false;
    document.getElementById('email').disabled = false;
    document.getElementById('cpf').disabled = false;
    document.getElementById('ddd').disabled = false;
    document.getElementById('numTel').disabled = false;
    document.getElementById('typeTel').disabled = false;
    document.getElementById('gender').disabled = false;
    document.getElementById('birthdate').disabled = false;

    const beginUpdateBtns = document.getElementsByClassName('beginUpdate');
    for (let btn of beginUpdateBtns) {
        btn.disabled = true;
    }

    const doUpdateBtns = document.getElementsByClassName('doUpdate');
    for (let btn of doUpdateBtns) {
        btn.disabled = false;
    }
}

function disableChangesField() {
    document.getElementById('id').disabled = true;
    document.getElementById('name').disabled = true;
    document.getElementById('email').disabled = true;
    document.getElementById('cpf').disabled = true;
    document.getElementById('ddd').disabled = true;
    document.getElementById('numTel').disabled = true;
    document.getElementById('typeTel').disabled = true;
    document.getElementById('gender').disabled = true;
    document.getElementById('birthdate').disabled = true;

    const beginUpdateBtns = document.getElementsByClassName('beginUpdate');
    for (let btn of beginUpdateBtns) {
        btn.disabled = false;
    }

    const doUpdateBtns = document.getElementsByClassName('doUpdate');
    for (let btn of doUpdateBtns) {
        btn.disabled = true;
    }
}

function UpdateInfo() {
    const ctm = {
        Id: document.getElementById('id').value,
        Name: document.getElementById('name').value,
        Email: document.getElementById('email').value,
        Cpf: document.getElementById('cpf').value,
        Ddd: document.getElementById('ddd').value,
        TlpNum: document.getElementById('numTel').value,
        Tpt: document.getElementById('typeTel').value,
        Gender: document.getElementById('gender').value,
        BirthDate: document.getElementById('birthdate').value,
        Pass: document.getElementById('pass').value,
        Active: true
    }

    const request = new XMLHttpRequest();
    request.open('POST', '/CRUD/Customer/InfoUpdate', false);
    request.setRequestHeader('Content-type', 'application/json');

    try {
        request.send(JSON.stringify(ctm))

        if (request.status === 200) {
            alert('Dados atualizados com sucesso!')
        } else {
            alert('Ocorreu um erro na atualização:' + request.statusText)
            console.error(`Erro ${request.status}: ${request.responseText}`)
        }
    } catch (ex) {
        alert('Falha na comunicação:' + ex.error);
    }
}

function updatePassword() {
    const curPass = document.getElementById('passCur').value;
    const newPass = document.getElementById('passNew').value;
    const confPass = document.getElementById('passConf').value;

    if (!curPass) {
        alert('A senha atual não foi inserida');
        return;
    }

    if (!newPass) {
        alert('Digite a sua nova senha');
        return;
    }

    if (!confPass) {
        alert('Redigite sua nova senha');
        return;
    }

    if (curPass !== document.getElementById('pass').value) {
        alert('A senha digitada está incorreta');
        return;
    };

    const errorMsg = window.PassFunctions.verifyPassword(newPass, confPass);

    if (errorMsg) {
        alert(errorMsg);
        return;
    }

    const request = new XMLHttpRequest();
    request.open('POST', '/CRUD/Customer/PasswordUpdate', false);
    request.setRequestHeader('Content-type', 'application/json');

    const info = {
        Id: document.getElementById('id').value,
        Pass: newPass
    }

    try {
        request.send(JSON.stringify(info))

        if (request.status === 200) {
            alert("Senha atualizada com sucesso!")
            window.location.reload()
        } else {
            alert("Erro ao efetuar a atualização: " + request.statusText)
            console.error(`Erro ${request.status}: ${request.responseText}`)
        }
    } catch (ex) {
        alert("Erro ao atualizar a senha.")
        console.error(ex)
    }
}