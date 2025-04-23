function confirmDelete(ctmId, ctmName, addId, addType) {
    const answer = confirm('Deletar o endereço do cadastro de ' + ctmName + '?')
    if (answer) {
        window.location.href = `/Address/RemoveRelation/${addId},${ctmId},${addType}`
    }
}