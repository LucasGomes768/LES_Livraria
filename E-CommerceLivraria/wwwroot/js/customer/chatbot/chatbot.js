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

    // Anexar mensagem do usuário
    userField.value = ''
    appendMessage('user', userMsg)

    // Enviar mensagem
    const answer = messageAPI(userMsg)

    // Anexar mensagem do bot
    if (answer)
        appendMessage('bot', answer)
    
}

function messageAPI(input) {
    const request = new XMLHttpRequest()
    request.open('POST', `http://localhost:5000/chatbot/send`, false)
    request.setRequestHeader('Content-Type', 'application/json')

    const data = {
        clientId: 1,
        message: input
    }

    try {
        request.send(JSON.stringify(data))

        if (request.status === 200) {
            const response = request.responseText
            return response
        } else {
            return "O assistente virtual está indisponível no momento. Tente novamente mais tarde"
            console.error('Erro: ', request.status, resquest.responseText)
        }
    } catch (error) {
        alert("Erro: " + error.message)
    }
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