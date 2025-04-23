function updatePrice(inputElement) {
    const itemIndex = inputElement.dataset.index;

    const inputTotal = document.querySelector('input.totalPriceInp');
    const base = document.querySelector(`.basePrice[data-index='${itemIndex}']`);
    const current = document.querySelector(`.curPrice[data-index='${itemIndex}']`);
    let quantity = inputElement.value;

    if (quantity === undefined || quantity === null || quantity === "") {
        alert('Quantidade do produto inserido inválido')
        inputElement.value = 1
        quantity = 1
    }

    const baseVal = parseFloat(base.value.replace(',', '.'));
    const currentVal = parseFloat(current.value.replace(',', '.'));
    let totalVal = parseFloat(inputTotal.value.replace(',', '.'));

    let totalTemp = totalVal - currentVal + (baseVal * quantity);

    current.value = (baseVal * quantity).toFixed(2).replace('.', ',');
    inputTotal.value = totalTemp.toFixed(2).replace('.', ',');

    document.querySelector(`.itemTotal[data-index='${itemIndex}']`).innerHTML = `<b>R$</b>${current.value}`;
    document.getElementById('totalPriceTxt').innerHTML = `<b>R$</b>${totalTemp.toFixed(2).replace('.', ',')}`;
}
