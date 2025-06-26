function getSelectId() {
    const selectField = document.getElementById('cep')

    if (!selectField) {
        alert('Por favor, selecione um endereço válido')
        return
    }

    if (!selectField.value) {
        document.getElementById('proceedMethod').innerHTML = '<button type="button" disabled>Escolha um endereço</button>'
        document.getElementById('shippingPrice').value = 0
        document.getElementById('shipText').innerHTML = `<b>R$</b>0,00`
        document.getElementById('formAddData').reset()
        updateTotalPrice()
        return
    }

    const addId = parseFloat(selectField.value)
    if (isNaN(addId)) {
        alert('ID de endeço inválido')
        return
    }

    getAddressData(addId)
}

function getAddressData(addId) {
    const request = new XMLHttpRequest()
    request.open('GET', `/Payment/GetDeliveryAddress/${addId}`, false)
    request.setRequestHeader('Content-Type', 'application/json')

    try {
        request.send()

        if (request.status == 404) {
            alert("Endereço não encontrado")
            return
        }

        if (request.status !== 200) {
            try {
                const errorResponse = JSON.parse(request.responseText);
                alert(errorResponse.Message || `Erro ${request.status}`);
            } catch {
                alert(`Erro ${request.status}: ${request.statusText}`);
            }

            return
        }

        const response = JSON.parse(request.responseText)

        if (!response.sucess) {
            alert(response.Message || "Erro ao processar endereço")
            return
        }

        const addData = response.data

        document.getElementById('choosenAddId').value = addData.addId
        document.getElementById('num').value = addData.addNumber
        document.getElementById('neigh').value = addData.neighborhoodName
        document.getElementById('city').value = addData.cityName
        document.getElementById('state').value = addData.stateName
        document.getElementById('country').value = addData.countryName
        document.getElementById('rType').value = addData.residenceTypeName
        document.getElementById('ppType').value = addData.publicPlaceTypeName
        document.getElementById('pp').value = addData.publicPlace
        document.getElementById('short').value = addData.addShortPhrase
        document.getElementById('obs').value = addData.addObservations
        document.getElementById('shippingPrice').value = addData.addShipping

        document.getElementById('proceedMethod').innerHTML = '<button type="submit">Continuar para forma de pagamento</button>'
        document.getElementById('shipText').innerHTML = `<b>R$</b>${addData.addShipping.toFixed(2).replace('.', ',')}`

        updateTotalPrice()
        saveChoosenAddress()
    } catch (error) {
        alert('Falha na comunicação: ' + error.message)
    }
}

function updateTotalPrice() {
    const itemsPrice = document.getElementById('currPrice')
    const shippingPri = document.getElementById('shippingPrice')

    const totalPrice = parseFloat(itemsPrice.value) + parseFloat(shippingPri.value)

    document.getElementById('totalPriceValue').innerHTML = `<h1><b>R$</b>${totalPrice.toFixed(2).replace('.', ',')}</h1>`
}

function saveChoosenAddress() {
    const addId = document.getElementById('choosenAddId').value;
    const shpPrice = document.getElementById('shippingPrice').value;

    const add = {
        id: addId,
        ship: Number(shpPrice)
    };

    sessionStorage.setItem("enderecoSelec", JSON.stringify(add));
}

