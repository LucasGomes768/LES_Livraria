export function carregarCartoes() {
    const table = document.getElementById("cardsUsedTable");
    let totalCardsValue = 0;
    let html = "<tr>" +
        "<th>Numero</th>" +
        "<th>Bandeira</th>" +
        "<th>Valor</th>" +
        "<th></th>" +
        "</tr>";

    const cards = sessionStorage.getItem("cartoesSelecionados");
    if (cards) {
        const array = JSON.parse(cards);
        array.forEach(card => {
            html += "<tr>" +
                `<td>${card.number}</td>` +
                `<td>${card.flag}</td>` +
                "<td>" +
                "R$" +
                `<input type="number" min="10.00" max="9999.99" step="0.01" value="${card.value.toFixed(2)}" data-card-id="${card.id}" class="card-value-input" required>` +
                "</td>" +
                "<td>" +
                `<button onclick="window.PaymentFunctions.removerCartao('${card.id}')">X</button>` +
                "</td>" +
                "</tr>";

            totalCardsValue += card.value;
        });
    }

    table.innerHTML = html;

    document.querySelectorAll('.card-value-input').forEach(input => {
        input.addEventListener('blur', function () {
            const cardId = this.getAttribute('data-card-id');
            const newValue = this.value;
            window.PaymentFunctions.atualizarValor(cardId, newValue);
        });
    });

    const totalPrice = document.getElementById('totalPrice').value.trim();

    if (window.PaymentFunctions?.calcularValores) {
        window.PaymentFunctions.calcularValores();
    }
}

export function salvarCartaoAdicionado(addedCard) {
    const card = JSON.parse(addedCard);

    if (window.PaymentFunctions?.salvarCartao) {
        window.PaymentFunctions.salvarCartao(card);
        window.PaymentFunctions.carregarCartoes();
    }
}

export function salvarCartaoSelecionado() {
    const select = document.getElementById("userCards");
    const id = select.value;
    if (id === "N") {
        return
    }
    const number = select.options[select.selectedIndex].text;

    const card = {
        id: id,
        number: number.slice(0, 19),
        flag: number.slice(21).replace(/[()]/g, ""),
        value: 10
    }

    select.value = "N"

    if (window.PaymentFunctions?.salvarCartao) {
        window.PaymentFunctions.salvarCartao(card);
    }
}

export function salvarCartao(addedCard) {
    if (!window.PaymentFunctions.verifyNecessity()) {
        alert('Cartão adicionado desnecessariamente.');
        return
    }

    let cardsUsing = sessionStorage.getItem("cartoesSelecionados")

    // Adicionando cartão
    if (!cardsUsing) {
        // Cartão não foi adicionado
        const newCardsArray = [addedCard]
        const string = JSON.stringify(newCardsArray)

        sessionStorage.setItem("cartoesSelecionados", string)
    } else {
        let cardsArray = JSON.parse(cardsUsing)

        // Cartão já foi adicionado
        if (cardsArray.some(x => x.id === addedCard.id)) {
            alert('Esse cartao de credito ja foi adicionado!')
            return
        }

        cardsArray.push(addedCard)
        const string = JSON.stringify(cardsArray)

        sessionStorage.setItem("cartoesSelecionados", string)
    }

    if (window.PaymentFunctions?.carregarCartoes) {
        window.PaymentFunctions.carregarCartoes();
    }
}

export function removerCartao(id) {
    const cards = sessionStorage.getItem("cartoesSelecionados");
    if (!cards) return

    const cardsArray = JSON.parse(cards)
    const updatedCards = cardsArray.filter(x => x.id !== id)

    sessionStorage.setItem("cartoesSelecionados", JSON.stringify(updatedCards))

    if (window.PaymentFunctions?.carregarCartoes) {
        window.PaymentFunctions.carregarCartoes();
    }
}

export function atualizarValor(id, newValue) {
    if (isNaN(newValue) || newValue < 10 || newValue > 9999.99) {
        alert('Insira um valor entre R$10,00 e R$9.999,99');
        return;
    }

    newValue = parseFloat(newValue);

    const cards = sessionStorage.getItem("cartoesSelecionados");
    if (!cards) return

    let cardsArray = JSON.parse(cards)
    const cardIndex = cardsArray.findIndex(x => x.id === id)

    if (cardIndex === -1)
        return

    if (cardsArray[cardIndex].value === newValue)
        return

    cardsArray[cardIndex].value = newValue
    sessionStorage.setItem("cartoesSelecionados", JSON.stringify(cardsArray))

    if (window.PaymentFunctions?.carregarCartoes) {
        window.PaymentFunctions.carregarCartoes();
    }
}