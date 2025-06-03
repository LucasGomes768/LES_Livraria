async function sendMessage() {
    try {
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
        saveMsgHistory('user', userMsg)
        appendMessage('user', userMsg)

        // Enviar mensagem
        const answer = await messageAPI(userMsg)

        // Anexar mensagem do bot
        if (answer)
            saveMsgHistory('bot', answer)
            appendMessage('bot', answer)
    } catch (ex) {
        console.error('Erro:', ex)
        appendMessage('bot', "O assistente virtual está indisponível no momento. Tente novamente mais tarde")
    }
    
}

async function messageAPI(input) {
    try {
        const response = await fetch('/Chatbot/SendMessage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                CtmId: 1,
                Message: input
            })
        });

        if (!response.ok) {
            throw new Error(`Erro HTTP: ${response.status}`);
        }

        const data = await response.json();
        const answer = JSON.parse(data.content)
        return answer.response;
    } catch (ex) {
        console.error('Erro na API:', ex);
        throw ex;
    }
}

function appendMessage(role, message) {
    let currMsgs = document.getElementById('messages').innerHTML

    if (role === 'user') {
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

function saveMsgHistory(role, text) {
    let messages = sessionStorage.getItem('chatbotMessages')

    const message = {
        "role": role,
        "text": text
    }

    if (!messages) {
        const newHistory = [message]
        const string = JSON.stringify(newHistory)

        sessionStorage.setItem('chatbotMessages', string)
    } else {
        messages = JSON.parse(messages)
        messages.push(message)

        const string = JSON.stringify(messages)
        sessionStorage.setItem('chatbotMessages', string)
    }
}

function loadMessages() {
    let messages = sessionStorage.getItem('chatbotMessages')

    if (messages) {
        messages = JSON.parse(messages)

        messages.forEach(x => appendMessage(x.role, x.text))
        
    }
}

window.onload = loadMessages()