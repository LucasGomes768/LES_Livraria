export function carregarCupons() {
    const table = document.getElementById("exCouponsTable");
    let html = `<tr>
        <th>Data de Geração</th>
        <th>Valor</th>
        <th></th>
        </tr>`

    const coupons = sessionStorage.getItem("cuponsSelecionados");
    if (coupons) {
        const array = JSON.parse(coupons);
        array.forEach(coupon => {
            html += "<tr>" +
                `<td>${coupon.date}</td>` +
                `<td>R$${coupon.value.toFixed(2).replaceAll('.',',')}</td>` +
                "<td>" +
                `<button onclick="window.PaymentFunctions.removerCupom('${coupon.id}')">X</button>` +
                "</td>" +
                "</tr>";
        });
    }

    table.innerHTML = html;

    if (window.PaymentFunctions?.calcularValores) {
        window.PaymentFunctions.calcularValores();
    }
}

export function salvarCupomSelecionado() {
    const select = document.getElementById("userCoupons");
    const id = select.value;
    if (id === "N") {
        return
    }

    const optText = select.options[select.selectedIndex].text.split(' ');
    const valueText = optText[0];
    const dateText = optText[1];

    const coupon = {
        id: id,
        date: dateText.substring(1, dateText.length - 1),
        value: Number(valueText.substring(2).replaceAll(',','.'))
    }

    select.value = "N"

    if (window.PaymentFunctions?.salvarCupom) {
        window.PaymentFunctions.salvarCupom(coupon);
    }
}

export function salvarCupom(addedCpn) {
    if (!window.PaymentFunctions.verifyNecessity()) {
        alert('Cupom adicionado desnecessariamente.');
        return
    }

    let couponsUsing = sessionStorage.getItem("cuponsSelecionados")

    // Adicionando cupom
    if (!couponsUsing) {
        // Cupom não foi adicionado
        const newCouponsArray = [addedCpn]
        const string = JSON.stringify(newCouponsArray)

        sessionStorage.setItem("cuponsSelecionados", string)
    } else {
        let couponsArray = JSON.parse(couponsUsing)

        // Cupom já foi adicionado
        if (couponsArray.some(x => x.id === addedCpn.id)) {
            alert('Esse cupom ja foi adicionado!')
            return
        }

        couponsArray.push(addedCpn)
        const string = JSON.stringify(couponsArray)

        sessionStorage.setItem("cuponsSelecionados", string)
    }

    if (window.PaymentFunctions?.carregarCupons) {
        window.PaymentFunctions.carregarCupons();
    }
}

export function removerCupom(id) {
    const coupons = sessionStorage.getItem("cuponsSelecionados");
    if (!coupons) return

    const couponsArray = JSON.parse(coupons)
    const updatedCoupons = couponsArray.filter(x => x.id !== id)

    sessionStorage.setItem("cuponsSelecionados", JSON.stringify(updatedCoupons))

    if (window.PaymentFunctions?.carregarCupons) {
        window.PaymentFunctions.carregarCupons();
    }
}