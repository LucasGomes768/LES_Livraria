from flask import Flask, jsonify, request
from flask_cors import CORS
import Gemini.geminiAPI as chatbot

app = Flask(__name__)
CORS(app)

# Enviar e receber mensagem do GEMINI
@app.route('/chatbot/send',methods=['POST'])
def sendMessage():
    userObj = request.get_json()
    return chatbot.getResponse(userObj['message'])

@app.route('/chatbot/forget',methods=['PUT'])
def forgetMessages():
    chatbot.forgetMessages()

# Rodar api
app.run(port=5000,host="localhost",debug=True)