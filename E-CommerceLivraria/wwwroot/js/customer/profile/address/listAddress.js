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