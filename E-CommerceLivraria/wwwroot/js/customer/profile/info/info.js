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