import base64
import os
import json
from google import genai
from google.genai import types
from dotenv import load_dotenv

load_dotenv()

client = genai.Client(
    api_key=os.environ["GEMINI_API_KEY"]
)

model = "gemini-2.0-flash"
contents = []
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
Talk in the same language of the user.
For now, the only book in store is The King in Yellow by Robert W. Chambers, published by Darkside."""),
    ],
)

def _generate():

    response = client.models.generate_content(
        model=model,
        config=generate_content_config,
        contents=contents,
    )

    return response.text

def getResponse(userMsg):
        contents.append(
            types.Content(
                role="user",
                parts=[
                    types.Part.from_text(text=userMsg),
                ]
            )
        )

        return _generate()

def forgetMessages():
      contents.clear()