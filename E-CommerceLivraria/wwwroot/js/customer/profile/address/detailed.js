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
            const response = JSON.parse(request.responseText)
            alert('Ocorreu um erro na atualização:' + response)
            console.error(`Erro ${request.status}: ${response}`)
        }
    } catch (ex) {
        alert('Falha na comunicação:' + ex.error);
    }
}