function carregarCartoes() {
    const table = document.getElementById("cardsUsedTable");
    let html = "<tr>" +
        "<th>Número</th>" +
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
                `<input type="number" min="10.00" max="9999.99" step="0.01" value="${card.value.toFixed(2)}" onblur="atualizarValor('${card.id}', this.value)" required>` +
                "</td>" +
                "<td>" +
                `<button onclick="removerCartao('${card.id}')">X</button>` +
                "</td>" +
                "</tr>";
        });
    }

    table.innerHTML = html;
    calcularValores();
}

function calcularValores() {
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
        buttonDiv.innerHTML = '<button type="button">Finalizar compra</button>';
    } else {
        buttonDiv.innerHTML = '<button type="button" disabled>Finalizar compra</button>';
    }
}

function salvarCartao() {
    const select = document.getElementById("userCards");
    const id = select.value;
    if (id === "N") {
        return
    }
    const number = select.options[select.selectedIndex].text;

    const card = {
        "id": id,
        "number": number.slice(0, 19),
        "flag": number.slice(21).replace(/[()]/g,""),
        "value": 10
    }

    let cardsUsing = sessionStorage.getItem("cartoesSelecionados")

    // Adicionando cartão
    if (cardsUsing === null) {
        // Cartão não foi adicionado
        const newCardsArray = [card]
        const string = JSON.stringify(newCardsArray)

        sessionStorage.setItem("cartoesSelecionados", string)
    } else {
        let cardsArray = JSON.parse(cardsUsing)

        // Cartão já foi adicionado
        if (cardsArray.some(x => x.id === card.id)) {
            select.value = "N"
            alert('Esse cartao de credito ja foi adicionado!')
            return
        }

        cardsArray.push(card)
        const string = JSON.stringify(cardsArray)

        sessionStorage.setItem("cartoesSelecionados", string)
    }

    select.value = "N"
    carregarCartoes()
}

function removerCartao(id) {
    const cards = sessionStorage.getItem("cartoesSelecionados");
    if (!cards) return

    const cardsArray = JSON.parse(cards)
    const updatedCards = cardsArray.filter(x => x.id !== id)

    sessionStorage.setItem("cartoesSelecionados", JSON.stringify(updatedCards))

    carregarCartoes();
}
function atualizarValor(id, newValue) {
    newValue = parseFloat(newValue);

    if (isNaN(newValue) || newValue < 10 || newValue > 9999.99) {
        alert('Insira um valor maior ou igual a R$10,00')
        return
    }

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

    carregarCartoes();
}

window.addEventListener('load', carregarCartoes);