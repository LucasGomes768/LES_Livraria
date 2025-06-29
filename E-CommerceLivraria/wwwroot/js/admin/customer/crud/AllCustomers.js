function removeCustomer(id, nome) {
    if (confirm(`Você tem certeza que deseja remover o cadastro de ${nome}?`)) {
        const request = new XMLHttpRequest();
        request.open('DELETE', `/AdmCustomer/Remove/${id}`, false);
        request.setRequestHeader('Content-Type', 'application/json');

        try {
            request.send();

            if (request.status == 200) {
                alert('Cadastro removido com sucesso.');
                window.location.reload();
            }

        } catch (ex) {
            alert("Erro ao remover o usuário: " + ex.statusText)
            console.error("Erro: " + ex.message)
        }
    }
}