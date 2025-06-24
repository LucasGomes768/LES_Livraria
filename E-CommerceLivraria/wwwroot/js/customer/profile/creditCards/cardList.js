function removeCreditCard(ctmId, crdId) {
    if (confirm("Remover cartão da conta?")) {
        const request = new XMLHttpRequest();
        request.open("PUT", `/CreditCardsProfile/Remove/${ctmId}/${crdId}`, false);
        request.setRequestHeader('Content-Type', 'application/json');

        try {
            request.send()

            if (request.status === 200) {
                alert("Cartão removido da conta com sucesso.")
                window.location.reload()
            } else {
                alert('Erro ao remover cartão: ' + request.statusText);
                console.error('Erro completo:', request.responseText);
            }

        } catch (error) {
            alert("Erro de comunicação: " + error.statusText);
            console.error(`Erro ${error.status}: ${error.message}`)
        }
    }
}