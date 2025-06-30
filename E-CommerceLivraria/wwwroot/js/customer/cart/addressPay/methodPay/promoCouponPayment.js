export function carregarCupomPromo(coupon) {
    const idField = document.getElementById('promoCpnId');
    const codeField = document.getElementById('promoCpnCode');
    const valueField = document.getElementById('promoCpnValue');
    const valueTxtField = document.getElementById('promoCpnValueTxt');

    idField.value = coupon.id;
    codeField.value = coupon.code;
    valueField.value = coupon.value;
    valueTxtField.innerHTML = `R$${coupon.value.toFixed(2).replaceAll(".",",")}`;

    if (window.PaymentFunctions?.calcularValores) {
        window.PaymentFunctions.calcularValores();
    }
}

export function removerCupomPromo() {
    const idField = document.getElementById('promoCpnId');
    const codeField = document.getElementById('promoCpnCode');
    const valueField = document.getElementById('promoCpnValue');
    const valueTxtField = document.getElementById('promoCpnValueTxt');

    idField.value = '';
    codeField.value = '';
    valueField.value = '';
    valueTxtField.innerHTML = '-';

    if (window.PaymentFunctions?.calcularValores) {
        window.PaymentFunctions.calcularValores();
    }
}

export function adicionarCupomPromo(field) {
    let code = field.value;
    let ctmId = parseFloat(document.getElementById("ctmId").value);

    if (!code) return;

    if (code.length !== 10) {
        alert('Erro: O código de um cupom promocional possui exatamente 10 caracteres');
        return;
    }

    if (!window.PaymentFunctions.verifyNecessity()) {
        alert('Cupom adicionado desnecessariamente.');
        return
    }

    code = code.toUpperCase();

    const response = procurarCupomPromo(ctmId, code);
    if (!response) return;

    carregarCupomPromo(response);
}

function procurarCupomPromo(ctmId, code) {
    const request = new XMLHttpRequest();
    request.open("GET", `/CRUD/PromoCoupons/${ctmId}/${code}`, false);
    request.setRequestHeader("Content-Type", "application-json");

    try {
        request.send();
        const response = JSON.parse(request.responseText);

        if (request.status === 200) {
            return JSON.parse(response.jsonString);
        } else { 
            alert('Erro: ' + response.message)
            return false;
        }

    } catch (ex) {
        alert("Erro: " + ex.message);
        console.error("Erro: " + ex);
        return false;
    }
}