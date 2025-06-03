import base64
import os
import json
import time
from google import genai
from google.genai import types
from dotenv import load_dotenv

load_dotenv()

client = genai.Client(
    api_key=os.environ["GEMINI_API_KEY"]
)

model = "gemini-2.0-flash"
contents = []
instruction_role = """You are a virtual assistant of an e-commerce page of a library named \"NextPage\". Your goal is to assist users, which are the library's customers, with book recommedations, books available in store, etc.
        You cannot do tasks such as inserting, altering or removing items in their cart, process purchases, and others alike.
        The topics you may talk about with the users must be restricted to the user preferences, store's item and adjacent content, such as publishers, books' authors and genres.
        Talk in the same language of the user and keep your answer short, with a limit of 65 words which doenst need to be always reached. Avoid using emojis. Answer based of the user's message."""
instruction_store = ""
generate_content_config = types.GenerateContentConfig(
    temperature=0.3,
    safety_settings=[
        types.SafetySetting(
            category="HARM_CATEGORY_HARASSMENT",
            threshold="BLOCK_MEDIUM_AND_ABOVE",  # Block some
        ),
        types.SafetySetting(
            category="HARM_CATEGORY_HATE_SPEECH",
            threshold="BLOCK_MEDIUM_AND_ABOVE",  # Block some
        ),
        types.SafetySetting(
            category="HARM_CATEGORY_SEXUALLY_EXPLICIT",
            threshold="BLOCK_ONLY_HIGH",  # Block few
        ),
    ],
    response_mime_type="text/plain",
    system_instruction=[
        types.Part.from_text(text="""You are a virtual assistant of an e-commerce page of a library named \"NextPage\". Your goal is to assist users, which are the library's customers, with book recommedations, books available in store, etc.
        You cannot do tasks such as inserting, altering or removing items in their cart, process purchases, and others alike.
        The topics you may talk about with the users must be restricted to the user preferences, store's item and adjacent content, such as publishers, books' authors and genres.
        Talk in the same language of the user and keep your answer short, with a limit of 65 words which doenst need to be always reached. Avoid using emojis. Answer based of the user's message.
        The following books are in store:"""),
    ],
)

def getResponse(promptInfo):
    prompt = formatPrompt(
        promptInfo['message'],
        promptInfo['customerInfo'])

    updateInstruction(promptInfo['storeBooksInfo'])

    content = types.Content(
        role="user",
        parts=[
            types.Part.from_text(text=prompt),
        ]
    )
    contents.append(content)

    return _generate()

def formatPrompt(Message, UserData):
    prompt = f"""
    The user has sent the following message: {Message}
    Use the following data to appropriately answer them:
    - User data: {UserData}
    """
    return prompt

def updateInstruction(StoreItems):
    instruction_store = f"The following books are available in store:{StoreItems}"
    instruction = f"""{instruction_role}{instruction_store}"""

    generate_content_config.system_instruction = [types.Part.from_text(text=instruction),]

def _generate(maxRetries = 5, initialDelay = 1):
    for i in range(maxRetries):
        try:
            response = client.models.generate_content(
                model=model,
                config=generate_content_config,
                contents=contents,
            )

            responseContent = types.Content(
                role="model",
                parts=[
                    types.Part.from_text(text=response.text),
                ]
            )

            contents.append(responseContent)

            return response.text
        except Exception as e:
            errorMessage = str(e)

            if "503 UNAVAILABLE" in errorMessage or "overloaded" in errorMessage or "quota" in errorMessage:
                if i < maxRetries - 1:
                    delay = initialDelay * (3 ** i)
                    print(f"Modelo sobrecarregado, tentando novamente em {delay:.1f} segundos...")
                    time.sleep(delay)
                else:
                    raise e
            else:
                raise e
    return None

def forgetMessages():
    contents.clear()
    return '', 204