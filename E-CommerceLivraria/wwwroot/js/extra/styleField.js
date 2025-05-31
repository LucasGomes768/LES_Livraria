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