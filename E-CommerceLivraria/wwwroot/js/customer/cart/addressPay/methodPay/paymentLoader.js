import { carregarCartoes, salvarCartao, removerCartao, atualizarValor } from './cardsPayment.js';
import { calcularValores, finalizarCompra } from './methodPayment.js';

window.PaymentFunctions = {
    carregarCartoes,
    salvarCartao,
    removerCartao,
    atualizarValor,
    calcularValores,
    finalizarCompra
};

document.addEventListener('DOMContentLoaded', () => {
    if (window.PaymentFunctions.carregarCartoes) window.PaymentFunctions.carregarCartoes();
    if (window.PaymentFunctions.calcularValores) window.PaymentFunctions.calcularValores();
});