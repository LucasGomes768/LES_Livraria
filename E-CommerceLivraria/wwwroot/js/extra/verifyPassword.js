export function verifyPassword(pass, confPass) {
    var errorMsg = ""

    // Nulo
    if (!pass) {
        errorMsg += '\n- O campo da senha nova n�o foi preenchido'
    }

    if (!confPass) {
        errorMsg += '\n- A senha nova n�o foi redigitada'
    }

    // Mai�sculo
    if (!(/[A-Z]/.test(pass))) {
        errorMsg += "\n- Nao contem letras maiusculas"
    }
    // Min�sculo
    if (!(/[a-z]/.test(pass))) {
        errorMsg += "\n- Nao contem letras minusculas"
    }
    // Caractere especial
    if (!(/[\W_]/.test(pass))) {
        errorMsg += "\n- Nao contem um caractere especial"
    }
    // Tamanho m�nimo
    if ((pass.length < 8)) {
        errorMsg += "\n- Senha com menos de 8 caracteres"
    }
    // Redigitar senha
    if (pass !== confPass) {
        errorMsg += "\n- A senha redigitada � diferente da senha nova inserida"
    }

    // Alert
    if (errorMsg !== "") {
        return "Senha invalida:" + errorMsg
    } else {
        return undefined
    }
}