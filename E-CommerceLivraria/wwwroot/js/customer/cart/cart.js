function updatePrice(inputElement) {
    const itemIndex = inputElement.dataset.index;

    const inputTotal = document.querySelector('input.totalPriceInp');
    const base = document.querySelector(`.basePrice[data-index='${itemIndex}']`);
    const current = document.querySelector(`.curPrice[data-index='${itemIndex}']`);
    let quantity = inputElement.value;

    if (quantity === undefined || quantity === null || quantity === "" || quantity == 0) {
        alert('Quantidade do produto inserido inválido')
        window.location.reload();
    }

    const baseVal = parseFloat(base.value.replace(',', '.'));
    const currentVal = parseFloat(current.value.replace(',', '.'));
    let totalVal = parseFloat(inputTotal.value.replace(',', '.'));

    let totalTemp = totalVal - currentVal + (baseVal * quantity);

    current.value = (baseVal * quantity).toFixed(2).replace('.', ',');
    inputTotal.value = totalTemp.toFixed(2).replace('.', ',');

    const ctmId = document.getElementById("CtmId").value

    data = {
        CtmId: parseInt(ctmId),
        ItemStockID: parseInt(itemIndex),
        NewAmount: parseInt(quantity)
    }

    sendUpdateRequest(data)

    document.querySelector(`.itemTotal[data-index='${itemIndex}']`).innerHTML = `<b>R$</b>${current.value}`;
    document.getElementById('totalPriceTxt').innerHTML = `<b>R$</b>${totalTemp.toFixed(2).replace('.', ',')}`;
}

function sendUpdateRequest(data) {
    const request = new XMLHttpRequest();
    request.open('POST', '/Cart/UpdateQuantity', false);
    request.setRequestHeader('Content-Type', 'application/json');

    try {
        request.send(JSON.stringify(data));

        if (request.status !== 200) {
            alert('Erro ao atualizar a quantidade: ' + request.statusText);
            console.error('Erro completo:', request.responseText);
            window.location.reload();
        }

    } catch (error) {
        alert('Falha na comunicação: ' + error.message);
    }
}

