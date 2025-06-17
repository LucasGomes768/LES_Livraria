Promise.all([
    import(`./cardsPayment.js?t=${Date.now()}`),
    import(`./methodPayment.js?t=${Date.now()}`)
]).then(([cardsModule, methodModule]) => {
    window.PaymentFunctions = {
        ...cardsModule,
        ...methodModule
    };

    document.addEventListener('DOMContentLoaded', () => {
        window.PaymentFunctions.carregarCartoes?.();
        window.PaymentFunctions.calcularValores?.();
    });
});