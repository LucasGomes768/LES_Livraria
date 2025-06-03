export function calcularValores() {
    // Cartões de crédito
    const cards = sessionStorage.getItem("cartoesSelecionados");
    const buttonDiv = document.getElementById("finishButton");
    let totalCards = 0;

    if (cards) {
        const cardsArray = JSON.parse(cards);
        totalCards = cardsArray.reduce((sum, card) => sum + parseFloat(card.value), 0);
    }

    document.getElementById("cardsValue").innerHTML = `<b>R$</b>${("" + totalCards.toFixed(2)).replace('.',',')}`

    const priceElement = document.getElementById("totalPrice");
    let priceValue = 0;

    if (priceElement)
        priceValue = parseFloat(priceElement.value.trim());

    const totalValuePayed = totalCards

    if (totalCards > priceValue) {
        alert("O valor gasto com cartoes de credito excede o preco da compra.")
        buttonDiv.innerHTML = '<button type="button" disabled>Finalizar compra</button>';
        return
    }

    if (totalValuePayed >= priceValue) {
        buttonDiv.innerHTML = '<button id="btnFinalizar" onclick="window.PaymentFunctions.finalizarCompra()">Finalizar compra</button>';
    } else {
        buttonDiv.innerHTML = '<button type="button" disabled>Finalizar compra</button>';
    }
}

export function finalizarCompra() {
    const btn = document.getElementById('btnFinalizar')
    btn.disabled = true
    btn.textContent = 'Processando...'

    const cartoes = JSON.parse(sessionStorage.getItem("cartoesSelecionados") || "[]");
    const totalCompra = parseFloat(document.getElementById('totalPrice').value);

    const request = new XMLHttpRequest();
    request.open('POST', '/Payment/ProcessPurchase', false);
    request.setRequestHeader('Content-Type', 'application/json');

    const data = {
        FinalPrice: totalCompra,
        CreditCards: cartoes.map(cartao => ({
            id: parseFloat(cartao?.id || 0),
            value: parseFloat(cartao?.value || 0)
        })),
        PromotionalCode: "",
        ExchangeIds: [],
        AddressId: parseFloat(document.getElementById("deliveryAddress").value),
        CtmId: parseFloat(document.getElementById("ctmId").value)
    }

    try {
        request.send(JSON.stringify(data));

        if (request.status === 200) {
            sessionStorage.removeItem("cartoesSelecionados");
            alert("Compra realizada com sucesso!")
            window.location.href = `/Home/HomePage`
        } else {
            alert('Erro ao processar compra: ' + request.statusText);
        }

    } catch (error) {
        alert('Falha na comunicação: ' + error.message);
    } finally {
        btn.disabled = false
        btn.textContent = 'Finalizar Compra'
    }
}