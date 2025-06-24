function removeCreditCard(ctmId, crdId) {
    if (confirm("Remover cart�o da conta?")) {
        const request = new XMLHttpRequest();
        request.open("PUT", `/CreditCardsProfile/Remove/${ctmId}/${crdId}`, false);
        request.setRequestHeader('Content-Type', 'application/json');

        try {
            request.send()

            if (request.status === 200) {
                alert("Cart�o removido da conta com sucesso.")
                window.location.reload()
            } else {
                alert('Erro ao remover cart�o: ' + request.statusText);
                console.error('Erro completo:', request.responseText);
            }

        } catch (error) {
            alert("Erro de comunica��o: " + error.statusText);
            console.error(`Erro ${error.status}: ${error.message}`)
        }
    }
}