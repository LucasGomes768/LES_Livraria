function updateAddDiv(id) {
    const curDisplay = document.getElementById('addressUpdDiv').style.display

    if (id) {
        if (!setUpdateFieldValues(id)) return
        document.getElementById('addressUpdDiv').style.display = 'flex'
    }
}

function hideUpdAddDiv() {
    document.getElementById('addressUpdDiv').style.display = 'none'
    document.getElementById('updAddForm').reset();
}

function updateAdd() {
    const add = {
        Id: document.getElementById('updId').value,
        CtmId: document.getElementById('updCtmId').value,
        ResidenceType: document.getElementById('updRsType').value,
        PublicPlaceType: document.getElementById('updPpType').value,
        PublicPlace: document.getElementById('updPublicPlace').value,
        Cep: document.getElementById('updCep').value,
        Number: document.getElementById('updNum').value,
        Neighborhood: document.getElementById('updNeighborhood').value,
        City: document.getElementById('updCity').value,
        State: document.getElementById('updState').value,
        Country: document.getElementById('updCountry').value,
        ShortPhrase: document.getElementById('updShortPhrase').value,
        Observations: document.getElementById('updObs').value
    }

    const request = new XMLHttpRequest();
    request.open('POST', '/CRUD/Address/Update', false);
    request.setRequestHeader('Content-type', 'application/json');

    try {
        request.send(JSON.stringify(add))

        if (request.status === 200) {
            alert('Dados atualizados com sucesso!')
            window.location.reload();
        } else {
            alert('Ocorreu um erro na atualização:' + request.statusText)
            console.error(`Erro ${request.status}: ${request.responseText}`)
        }
    } catch (ex) {
        alert('Falha na comunicação:' + ex.error);
    }
}

function removeAddress(type, ctmId, addId) {
    const request = new XMLHttpRequest();
    request.open('DELETE', `/CRUD/Address/RemoveFromAccount/${type}/${ctmId}/${addId}`, false);
    request.setRequestHeader('Content-type', 'application/json');

    try {
        request.send();

        if (request.status === 200) {
            alert('Endereço removido com sucesso!');
            window.location.reload();
        } else {
            alert('Ocorreu um erro na remoção:' + request.statusText)
            console.error(`Erro ${request.status}: ${request.responseText}`)
        }

    } catch (ex) {
        alert('Ocorreu um erro na remoção do endereço.');
        console.error(ex.message);
    }
}

function getAddInfo(id) {
    const request = new XMLHttpRequest();
    request.open('GET', `/CRUD/Address/Get/${id}`, false);
    request.setRequestHeader('Content-type', 'application/json');

    try {
        request.send()

        if (request.status === 200) {
            const response = JSON.parse(request.responseText);
            return JSON.parse(response.addressJson)
        } else {
            alert('Erro: ' + request.statusText);
            console.error('Erro: ' + request.message)
        }
    } catch (ex) {
        alert('Erro: ' + ex.statusText);
        console.error('Erro: ' + ex.message);
        return undefined
    }
}

function setUpdateFieldValues(id) {
    const address = getAddInfo(id)

    if (!address) {
        return false
    } else {
        document.getElementById('updId').value = address.addId;
        document.getElementById('updRsType').value = address.addRst.rstId;
        document.getElementById('updPpType').value = address.addPpt.pptId;
        document.getElementById('updPublicPlace').value = address.addPublicPlace;
        document.getElementById('updCep').value = address.addCepStyled;
        document.getElementById('updNum').value = address.addNumber;
        document.getElementById('updNeighborhood').value = address.addNbh.nbhName;
        document.getElementById('updCity').value = address.addNbh.nbhCty.ctyName;
        document.getElementById('updState').value = address.addNbh.nbhCty.ctyStt.sttName;
        document.getElementById('updCountry').value = address.addNbh.nbhCty.ctyStt.sttCtr.ctrName;
        document.getElementById('updShortPhrase').value = address.addShortPhrase;
        document.getElementById('updObs').value = address.addObservations;
    }

    return true
}