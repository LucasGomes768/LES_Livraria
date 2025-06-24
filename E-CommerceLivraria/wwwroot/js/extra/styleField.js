function styleCpf(field) {
    var cpf = field.value

    if (!cpf) {
        alert("O campo do CPF est� vazio.")
        return
    }

    cpf = cpf.replaceAll('-', '').replaceAll('.', '')

    if (!(/^\d+$/.test(cpf))) {
        alert("Um valor inv�lido foi inserido no campo CPF")
        return
    }

    if (cpf.length != 11) {
        alert("CPF digitado inv�lido")
        return
    }

    field.value = cpf.substring(0,3) + "." + cpf.substring(3,6) + "." + cpf.substring(6,9) + "-" + cpf.substring(9)
}

function styleCrdNum(field) {
    var num = field.value

    if (!num) {
        alert("O campo do n�mero do cart�o est� vazio.")
        return
    }

    num = num.replaceAll('.', '')

    if (!(/^\d+$/.test(num))) {
        alert("Um valor inv�lido foi inserido no campo do n�mero do cart�o.")
        return
    }

    if (num.length != 16) {
        alert("N�mero do cart�o digitado inv�lido")
        return
    }

    field.value = num.substring(0,4) + "." + num.substring(4,8) + "." + num.substring(8,12) + "." + num.substring(12)
}