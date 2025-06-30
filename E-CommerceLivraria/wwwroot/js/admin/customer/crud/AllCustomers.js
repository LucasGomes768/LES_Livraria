function removeCustomer(id, nome) {
    if (confirm(`Voce tem certeza que deseja desativar o cadastro de ${nome}?`)) {
        const request = new XMLHttpRequest();
        request.open('POST', `/AdmCustomer/Deactivate/${id}`, false);
        request.setRequestHeader('Content-Type', 'application/json');

        try {
            request.send();

            if (request.status == 200) {
                alert('Cadastro desativado com sucesso.');
                window.location.reload();
            }

        } catch (ex) {
            alert("Erro ao desativar o usuário: " + ex.message)
            console.error("Erro: " + ex)
        }
    }
}

function activateCustomer(id) {
    const request = new XMLHttpRequest();
    request.open('POST', `/AdmCustomer/Activate/${id}`, false);
    request.setRequestHeader('Content-Type', 'application/json');

    try {
        request.send();

        if (request.status == 200) {
            alert('Cadastro ativado com sucesso.');
            window.location.reload();
        }

    } catch (ex) {
        alert("Erro ao ativar o usuário: " + ex.message)
        console.error("Erro: " + ex)
    }
}