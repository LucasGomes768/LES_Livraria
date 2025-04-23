function verifyPassword(passField, confirmField, event) {
    var errorMsg = ""
    const pass = document.getElementById(passField).value
    const confPass = document.getElementById(confirmField).value

    // Maiúsculo
    if (!(/[A-Z]/.test(pass))) {
        errorMsg += "\n- Nao contem letras maiusculas"
    }
    // Minúsculo
    if (!(/[a-z]/.test(pass))) {
        errorMsg += "\n- Nao contem letras minusculas"
    }
    // Caractere especial
    if (!(/[\W_]/.test(pass))) {
        errorMsg += "\n- Nao contem um caractere especial"
    }
    // Tamanho mínimo
    if ((pass.length < 8)) {
        errorMsg += "\n- Senha com menos de 8 caracteres"
    }
    // Redigitar senha
    if (pass !== confPass) {
        errorMsg += "\n- Redigite sua senha"
    }

    // Alert
    if (errorMsg !== "") {
        alert("Senha invalida:" + errorMsg)
        event.preventDefault()
    }
}