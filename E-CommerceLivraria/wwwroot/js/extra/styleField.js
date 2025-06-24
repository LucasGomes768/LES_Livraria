function styleCpf(field) {
    var cpf = field.value

    if (!cpf) {
        alert("O campo do CPF está vazio.")
        return
    }

    cpf = cpf.replaceAll('-', '').replaceAll('.', '')

    if (!(/^\d+$/.test(cpf))) {
        alert("Um valor inválido foi inserido no campo CPF")
        return
    }

    if (cpf.length != 11) {
        alert("CPF digitado inválido")
        return
    }

    field.value = cpf.substring(0,3) + "." + cpf.substring(3,6) + "." + cpf.substring(6,9) + "-" + cpf.substring(9)
}

function styleCrdNum(field) {
    var num = field.value

    if (!num) {
        alert("O campo do número do cartão está vazio.")
        return
    }

    num = num.replaceAll('.', '')

    if (!(/^\d+$/.test(num))) {
        alert("Um valor inválido foi inserido no campo do número do cartão.")
        return
    }

    if (num.length != 16) {
        alert("Número do cartão digitado inválido")
        return
    }

    field.value = num.substring(0,4) + "." + num.substring(4,8) + "." + num.substring(8,12) + "." + num.substring(12)
}