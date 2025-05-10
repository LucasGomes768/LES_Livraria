namespace E_CommerceLivraria.Enums {
    public enum EStatus : int{
        EM_PROCESSAMENTO,
        COMPRA_APROVADA,
        EM_TRANSPORTE,
        ENTREGUE,
        TROCA_SOLICITADA,
        EM_TROCA,
        TROCADO,
        COMPRA_REPROVADA = -1,
        TROCA_REPROVADA = -2
    }
}
