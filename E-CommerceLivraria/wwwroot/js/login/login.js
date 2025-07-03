function loginAsCtm() {
    const ctmFld = document.getElementById('customerLoginOpt')

    if (!ctmFld.value) {
        alert('Escolha uma conta de cliente.')
        return
    }

    window.location.href = `/LoginAs/${ctmFld.value}`
}