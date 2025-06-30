export function calcularValores() {
    const buttonDiv = document.getElementById("finishButton");

    // Cartões de crédito
    const cards = sessionStorage.getItem("cartoesSelecionados");
    let totalCards = 0;

    if (cards) {
        const cardsArray = JSON.parse(cards);
        totalCards = cardsArray.reduce((sum, card) => sum + parseFloat(card.value), 0);
        totalCards = Number(totalCards.toFixed(2));
    }

    document.getElementById("cardsValue").innerHTML = `<b>R$</b>${("" + totalCards.toFixed(2)).replace('.',',')}`

    // Cupons
    const coupons = sessionStorage.getItem("cuponsSelecionados");
    let totalCoupons = 0;

    if (coupons) {
        const couponsArray = JSON.parse(coupons);
        totalCoupons = couponsArray.reduce((sum, cpn) => sum + parseFloat(cpn.value), 0);
        totalCoupons = Number(totalCoupons.toFixed(2));
    }

    const promoValue = Number(document.getElementById('promoCpnValue').value);
    if (promoValue) totalCoupons += promoValue

    document.getElementById("couponsValue").innerHTML = `<b>R$</b>${("" + totalCoupons.toFixed(2)).replace('.', ',')}`

    // Valor final
    const priceElement = document.getElementById("totalPrice");
    let priceValue = 0;

    if (priceElement)
        priceValue = parseFloat(priceElement.value.trim());

    const totalValuePayed = totalCards + totalCoupons

    if (totalCards > priceValue) {
        alert("O valor gasto com cartoes de credito excede o preco da compra.")
        buttonDiv.innerHTML = '<button type="button" disabled>Finalizar compra</button>';
        return
    }

    if (totalValuePayed >= priceValue) {
        buttonDiv.innerHTML = '<button id="btnFinalizar" type="button" onclick="window.PaymentFunctions.finalizarCompra()">Finalizar compra</button>';
    } else {
        buttonDiv.innerHTML = '<button type="button" disabled>Finalizar compra</button>';
    }
}

export function finalizarCompra() {
    const btn = document.getElementById('btnFinalizar')
    btn.disabled = true
    btn.textContent = 'Processando...'

    const cartoes = JSON.parse(sessionStorage.getItem("cartoesSelecionados") || "[]");
    const exCupons = JSON.parse(sessionStorage.getItem("cuponsSelecionados") || "[]");
    const totalCompra = parseFloat(document.getElementById('totalPrice').value);

    const request = new XMLHttpRequest();
    request.open('POST', '/Payment/ProcessPurchase', false);
    request.setRequestHeader('Content-Type', 'application/json');

    const data = {
        FinalPrice: totalCompra,
        CreditCards: cartoes.map(cartao => ({
            id: parseFloat(cartao.id),
            value: parseFloat(cartao.value)
        })),
        PromotionalCode: document.getElementById('promoCpnCode').value,
        ExchangeIds: exCupons.map(function(cupom) {
            return parseFloat(cupom.id)
        }),
        AddressId: parseFloat(document.getElementById("deliveryAddress").value),
        CtmId: parseFloat(document.getElementById("ctmId").value)
    }

    try {
        request.send(JSON.stringify(data));

        if (request.status === 200) {
            sessionStorage.removeItem("cartoesSelecionados");
            sessionStorage.removeItem("cuponsSelecionados");
            alert("Compra realizada com sucesso!");
            window.location.href = '/Home/HomePage'
        } else {
            const response = JSON.parse(request.responseText);
            alert(response.message);
        }

    } catch (error) {
        alert('Falha na comunicação: ' + error.message);
    } finally {
        btn.disabled = false
        btn.textContent = 'Finalizar Compra'
    }
}
export function verifyNecessity() {
    let coupons = sessionStorage.getItem("cuponsSelecionados");
    let totalCpns = 0;
    if (coupons) {
        coupons = JSON.parse(coupons);
        totalCpns = Number(coupons.reduce((sum, cpn) => sum + parseFloat(cpn.value), 0).toFixed(2));
    }

    let cards = sessionStorage.getItem("cartoesSelecionados");
    let totalCards = 0;
    if (cards) {
        cards = JSON.parse(cards);
        totalCards = Number(cards.reduce((sum, card) => sum + parseFloat(card.value), 0).toFixed(2));
    }

    const totalPrice = Number(document.getElementById('totalPrice').value);

    if (totalPrice <= (totalCards + totalCpns)) {
        return false;
    } else {
        return true;
    }
}