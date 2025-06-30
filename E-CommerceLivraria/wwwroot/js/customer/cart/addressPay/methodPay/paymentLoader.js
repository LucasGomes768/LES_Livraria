Promise.all([
    import(`./cardsPayment.js?t=${Date.now()}`),
    import(`./exCouponsPayment.js?t=${Date.now()}`),
    import(`./promoCouponPayment.js?t=${Date.now()}`),
    import(`./methodPayment.js?t=${Date.now()}`)
]).then(([cardsModule, exCpnsModule, promoCpnModule, methodModule]) => {
    window.PaymentFunctions = {
        ...cardsModule,
        ...exCpnsModule,
        ...promoCpnModule,
        ...methodModule
    };

    setTimeout(() => {
        const add = JSON.parse(sessionStorage.getItem("enderecoSelec"));

        document.getElementById('deliveryAddress').value = add["id"];
        document.getElementById('shipping').value = add["ship"];
        document.getElementById('shipText').innerHTML = `<b>R$</b>${add["ship"].toFixed(2).replaceAll(".", ",")}`

        const totalPrice = Number(document.getElementById('productsPrice').value) + add["ship"];
        document.getElementById('totalPrice').value = totalPrice;
        document.getElementById('totalText').innerHTML = `<b>R$</b>${totalPrice.toFixed(2).replaceAll(".", ",")}`;

        const jsonNewCard = document.getElementById('recentAddedCard').value;
        if (jsonNewCard) {
            window.PaymentFunctions.salvarCartaoAdicionado(jsonNewCard);
            document.getElementById('recentAddedCard').value = '';
        } else {
            window.PaymentFunctions.carregarCartoes();
        }

        window.PaymentFunctions.carregarCupons();
    }, 0);
});
