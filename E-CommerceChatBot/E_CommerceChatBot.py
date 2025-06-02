from flask import Flask, jsonify, request
from flask_cors import CORS
import Gemini.geminiAPI as chatbot
import asyncio

app = Flask(__name__)
CORS(app)

# Enviar e receber mensagem do GEMINI
@app.route('/gemini/send',methods=['POST'])
def sendMessage():
    try:
        promptInfo = request.get_json()
        response = chatbot.getResponse(promptInfo)
        return jsonify({"response": response})
    except Exception as e:
        print(jsonify({"error": str(e)}))
        return jsonify({"error": str(e)}), 500

# Esquecer mensagens anteriores
@app.route('/gemini/forget',methods=['DELETE'])
def forgetMessages():
    return chatbot.forgetMessages()

# Rodar api
app.run(port=5000,host="localhost",debug=True)