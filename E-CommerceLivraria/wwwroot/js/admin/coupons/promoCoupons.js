function getAllCoupons() {

}

function deactivateCoupon(id, code) {
    if (id <= 0 || !id) {
        alert('ID invalido')
        return
    }

    if (confirm(`Voce tem certeza que deseja desativar o cupom ${code}?`)) {
        const request = new XMLHttpRequest();
        request.open("POST", `/CRUD/PromoCoupons/Deactivate/${id}`, false);
        request.setRequestHeader("Content-Type", "application-json");

        try {
            request.send();

            if (request.status === 200) {
                alert('Cupom desativado com sucesso');
                window.location.reload();
            } else {
                const response = JSON.parse(request.responseText);
                alert('Erro ao tentar desativar o cupom: ' + response.message);
            }

        } catch (ex) {
            alert("Erro: " + ex.message);
            console.error("Erro: " + ex)
        }
    }
}

function createCoupon() {
    const valueField = document.getElementById('value');
    const codeField = document.getElementById('code');

    if (!verifyInputs(valueField.value, codeField.value)) return

    const request = new XMLHttpRequest();
    request.open("POST", "/CRUD/PromoCoupons/Add", false)
    request.setRequestHeader("Content-Type", "application/json");

    const cpn = {
        Code: codeField.value,
        Value: parseFloat(valueField.value)
    };

    try {
        request.send(JSON.stringify(cpn));

        if (request.status === 200) {
            alert('Cupom criado com sucesso!');
            window.location.reload();
        } else {
            const response = JSON.parse(request.responseText)
            alert("Erro ao criar cupom: " + response.message);
        }

    } catch (ex) {
        alert("Erro: " + ex.message);
        console.error("Erro: " + ex)
    }
}

function verifyInputs(value, code) {
    let errorMsg = "";

    if (value <= 0)
        errorMsg += "\n- O valor inserido e menor ou igual a zero.";

    if (code && code.length != 10)
        errorMsg += "\n- O codigo do cupom deve ter exatamente 10 caracteres.";

    if (errorMsg) {
        alert("Erro: " + errorMsg)
        return false
    } else {
        return true
    }
}