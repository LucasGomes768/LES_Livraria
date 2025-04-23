function confirmDelete(ctmId, ctmName) {
    const answer = confirm('Deletar o cadastro de ' + ctmName + '?')
    if (answer) {
        window.location.href = `/Customer/Remove/${ctmId}`
    }
}