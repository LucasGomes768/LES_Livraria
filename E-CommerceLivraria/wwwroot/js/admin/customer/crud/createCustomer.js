import { verifyPassword } from '../../../extra/verifyPassword.js'

window.ImportFunctions = {
    verifyPassword
}

document.getElementById('basicNextBtn').addEventListener('click', function () {
    verifyFields('basic');
});
document.getElementById('resNextBtn').addEventListener('click', function () {
    verifyFields('res');
});
document.getElementById('delNextBtn').addEventListener('click', function () {
    verifyFields('del');
});
document.getElementById('resBackBtn').addEventListener('click', function () {
    showSectionHideCurrent('res', 'basic');
});
document.getElementById('delBackBtn').addEventListener('click', function () {
    showSectionHideCurrent('del', 'res');
});
document.getElementById('bilBackBtn').addEventListener('click', function () {
    showSectionHideCurrent('bil', 'del');
});
document.getElementById('finishRegisterBtn').addEventListener('click', function () {
    finishRegister();
});

document.getElementById('passShowBtn').addEventListener('click', function () {
    changePassFieldType('pass', this)
})
document.getElementById('confPassShowBtn').addEventListener('click', function () {
    changePassFieldType('confPass', this)
})

function changePassFieldType(fieldName, btn) {
    const field = document.getElementById(fieldName);

    if (field.type != 'text') {
        field.type = 'text';
        btn.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-eye-slash" viewBox="0 0 16 16">
                            <path d="M13.359 11.238C15.06 9.72 16 8 16 8s-3-5.5-8-5.5a7 7 0 0 0-2.79.588l.77.771A6 6 0 0 1 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13 13 0 0 1 14.828 8q-.086.13-.195.288c-.335.48-.83 1.12-1.465 1.755q-.247.248-.517.486z"/>
                            <path d="M11.297 9.176a3.5 3.5 0 0 0-4.474-4.474l.823.823a2.5 2.5 0 0 1 2.829 2.829zm-2.943 1.299.822.822a3.5 3.5 0 0 1-4.474-4.474l.823.823a2.5 2.5 0 0 0 2.829 2.829"/>
                            <path d="M3.35 5.47q-.27.24-.518.487A13 13 0 0 0 1.172 8l.195.288c.335.48.83 1.12 1.465 1.755C4.121 11.332 5.881 12.5 8 12.5c.716 0 1.39-.133 2.02-.36l.77.772A7 7 0 0 1 8 13.5C3 13.5 0 8 0 8s.939-1.721 2.641-3.238l.708.709zm10.296 8.884-12-12 .708-.708 12 12z"/>
                        </svg>`
    } else {
        field.type = 'password';
        btn.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi bi-eye" viewBox="0 0 16 16">
                            <path d="M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8M1.173 8a13 13 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5s3.879 1.168 5.168 2.457A13 13 0 0 1 14.828 8q-.086.13-.195.288c-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5s-3.879-1.168-5.168-2.457A13 13 0 0 1 1.172 8z"/>
                            <path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5M4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0"/>
                        </svg>`
    }
}

function verifyFields(curSec) {
    let nextSec;
    let errorMsg = "";

    switch (curSec) {
        case 'basic':
            if (!document.getElementById('name').value) {
                errorMsg += "\n- Nome não inserido."
            }
            if (!document.getElementById('dtNasc').value) {
                errorMsg += "\n- Data de nascimento não inserido."
            }
            if (!document.getElementById('cpf').value) {
                errorMsg += "\n- CPF não inserido."
            }
            if (!document.getElementById('gender').value) {
                errorMsg += "\n- Gênero não definido."
            }
            if (!document.getElementById('telType').value) {
                errorMsg += "\n- Tipo de telefone não definido."
            }
            if (!document.getElementById('telDDD').value) {
                errorMsg += "\n- DDD do telefone não inserido."
            }
            if (!document.getElementById('telNum').value) {
                errorMsg += "\n- Número de telefone não inserido."

                const numTxt = document.getElementById('telNum').value.replaceAll('-','')
                if (isNaN(Number(numTxt)) || !isFinite(Number(numTxt))) {
                    errorMsg += "\n- Número de telefone inválido."
                } else if (numTxt.length < 8 || numTxt.length > 9) {
                    errorMsg += "\n- Número de telefone inválido."
                }
            }
            if (!document.getElementById('email').value){
                errorMsg += "\n- E-Mail não inserido."
            }
            
            const pass = document.getElementById('pass').value;
            const confPass = document.getElementById('confPass').value;

            if (window.ImportFunctions.verifyPassword(pass, confPass)) {
                errorMsg += "\n- Senha inválida."
            }

            nextSec = 'res';
        break;
        case 'res':
            nextSec = 'del';
        break;
        case 'del':
            nextSec = 'bil';
        break;
    }

    if (curSec === 'res' || curSec === 'del' || curSec === 'bil') {
        if (!document.getElementById(curSec+"RsType").value) {
            errorMsg += "\n- Tipo de residência não definido."
        }

        if (!document.getElementById(curSec+"PpType").value) {
            errorMsg += "\n- Tipo de logradouro não definido."
        }

        if (!document.getElementById(curSec+"Pp").value) {
            errorMsg += "\n- Logradouro não inserido."
        }

        if (!document.getElementById(curSec+"Cep").value) {
            errorMsg += "\n- CEP não inserido."
        } else {
            const value = document.getElementById(curSec+"Cep").value.replaceAll('-','');

            if (isNaN(Number(value)) || !isFinite(Number(value))) {
                errorMsg = "\n- CEP inválido."
            } else if (value.length !== 8) {
                errorMsg = "\n- CEP inválido."
            }
        }

        if (!document.getElementById(curSec+"Num").value) {
            errorMsg += "\n- Número não inserido."
        } else {
            const value = document.getElementById(curSec+"Num").value;

            if (isNaN(Number(value)) || !isFinite(Number(value))) {
                errorMsg = "\n- Valor não-númerico no número do endereço"
            } else if (value.length > 4) {
                errorMsg = "\n- Número com mais de 4 dígitos inserido no número do endereço"
            }
        }

        if (!document.getElementById(curSec+"Nbh").value) {
            errorMsg += "\n- Bairro não inserido."
        }

        if (!document.getElementById(curSec+"City").value) {
            errorMsg += "\n- Cidade não inserida."
        }

        if (!document.getElementById(curSec+"State").value) {
            errorMsg += "\n- Estado não inserido."
        }

        if (!document.getElementById(curSec+"Country").value) {
            errorMsg += "\n- País não inserido."
        }

        if (!document.getElementById(curSec+"ShortPhrase").value) {
            errorMsg += "\n- Frase curta não inserida."
        }
    }

    if (errorMsg !== "") {
        alert("Erro:" + errorMsg)
        return false
    }

    if (nextSec) {
        showSectionHideCurrent(curSec, nextSec)
        return true
    }
}

function finishRegister() {
    if (verifyFields('bil')) return

    document.getElementById('createCtmForm').submit();
}

function showSectionHideCurrent(curSec, showSec) {
    document.getElementById(curSec).style.display = 'none';
    document.getElementById(showSec).style.display = 'flex';
}