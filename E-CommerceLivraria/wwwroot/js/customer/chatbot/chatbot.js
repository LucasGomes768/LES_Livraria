function sendMessage() {
    // Pegar campos
    const userField = document.getElementById('userMessage')
    let userMsg = userField.value

    // Verificar se mensagem é nula
    if (!userMsg) {
        alert('Escreva uma mensagem para enviar!')
        return
    }

    // Formatar mensagem
    userMsg = userMsg.trim()

    // Enviar mensagem
    // Blah Blah API

    // Anexar mensagem(ns)
    userField.value = ''
    appendMessage('user', userMsg)
    
}

function appendMessage(sender, message) {
    let currMsgs = document.getElementById('messages').innerHTML

    if (sender === 'user') {
        currMsgs += `
                    <div class="userMsg">
                        <p>${message}</p>
                    </div>
                    `
    } else {
        currMsgs += `
                    <div class="botMsg">
                        <p>${message}</p>
                    </div>
                    `
    }

    document.getElementById('messages').innerHTML = currMsgs
}