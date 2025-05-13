function markForExchange(itemIndex) {
    const boughtField = document.getElementById("bought"+itemIndex)
    const exchangeField = document.getElementById("exchange"+itemIndex)
    const checkField = document.getElementById("check"+itemIndex)

    exchangeField.disabled = !checkField.checked
}

function prepareExchangeData() {
    const checkboxes = document.querySelectorAll('.tdMark input[type="checkbox"]:checked')

    const CtmIdInput = document.getElementById('ctmId')
    const PrcIdInput = document.getElementById('prcId')

    const exchangeRequest = {
        CtmId: parseInt(CtmIdInput.value),
        PrcId: parseInt(PrcIdInput.value),
        ItemsToExchange: []
    };

    checkboxes.forEach(checkbox => {
        const id = checkbox.id.replace('check', '')
        const exchangeInput = document.getElementById(`exchange${id}`)
        const boughtInput = document.getElementById(`bought${id}`)

        const quantExchange = parseInt(exchangeInput.value)
        const quantBought = parseInt(boughtInput.value)

        if (quantExchange < 1 || quantExchange > quantBought) {
            alert("Item com quantidade inválida")
            exchangeInput.focus()
            return
        }

        exchangeRequest.ItemsToExchange.push({
            PciPrcId: exchangeRequest.PrcId,
            PciStcId: parseInt(id),
            PciQuantity: quantExchange
        })
    })

    if (exchangeRequest.ItemsToExchange.length === 0) {
        alert('Selecione pelo menos um item para troca')
        return
    }

    sendExchangeRequest(exchangeRequest)
}

function sendExchangeRequest(data) {
    const request = new XMLHttpRequest();
    request.open('POST', '/RequestExchange/Send', false);
    request.setRequestHeader('Content-Type', 'application/json');

    try {
        request.send(JSON.stringify(data));

        if (request.status === 200) {
            alert("Troca solicitada com sucesso!")
            window.location.href = `Profile/Purchases/${data.CtmId}/${data.PrcId}`
        } else {
            alert('Erro ao processar compra: ' + request.statusText);
            console.error('Erro completo:', request.responseText);
        }

    } catch (error) {
        alert('Falha na comunicação: ' + error.message);
    }
}