function enableChangesField() {
    document.getElementById('rsType').disabled = false;
    document.getElementById('ppType').disabled = false;
    document.getElementById('publicPlace').disabled = false;
    document.getElementById('cep').disabled = false;
    document.getElementById('num').disabled = false;
    document.getElementById('neighborhood').disabled = false;
    document.getElementById('city').disabled = false;
    document.getElementById('state').disabled = false;
    document.getElementById('country').disabled = false;
    document.getElementById('shortPhrase').disabled = false;
    document.getElementById('obs').disabled = false;

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
    document.getElementById('rsType').disabled = true;
    document.getElementById('ppType').disabled = true;
    document.getElementById('publicPlace').disabled = true;
    document.getElementById('cep').disabled = true;
    document.getElementById('num').disabled = true;
    document.getElementById('neighborhood').disabled = true;
    document.getElementById('city').disabled = true;
    document.getElementById('state').disabled = true;
    document.getElementById('country').disabled = true;
    document.getElementById('shortPhrase').disabled = true;
    document.getElementById('obs').disabled = true;

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
        Id: document.getElementById('addId').value,
        CtmId: document.getElementById('ctmId').value,
        ResidenceType: document.getElementById('rsType').value,
        PublicPlaceType: document.getElementById('ppType').value,
        PublicPlace: document.getElementById('publicPlace').value,
        Cep: document.getElementById('cep').value,
        "Number": document.getElementById('num').value,
        Neighborhood: document.getElementById('neighborhood').value,
        City: document.getElementById('city').value,
        State: document.getElementById('state').value,
        Country: document.getElementById('country').value,
        ShortPhrase: document.getElementById('shortPhrase').value,
        Observations: document.getElementById('obs').value
    }

    const request = new XMLHttpRequest();
    request.open('POST', '/CRUD/Address/Update', false);
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