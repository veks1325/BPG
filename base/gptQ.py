# -*- coding: utf-8 -*-
"""
Created on Mon Oct 14 19:58:51 2024

@author: jw991
"""


import openai
import re
from datetime import datetime

path='/Users/a1234/Desktop/app 2/'
openai.api_key = 'your api key'

def prompt2code(filename):       
    # Set your OpenAI API key
    
    file_path="./log/concept/"
    
    conceptfilepath=file_path+filename
    # Read the prompt from basepropt.txt
    with open(conceptfilepath, 'r',encoding='UTF-8') as file:
        prompt = file.read()
    
    # Optionally, instruct the assistant to provide only the code
   
    adprompt = "\n\nImplement the above concept as a coroutine function according to Unity Csharp code."
    adprompt += "\n\nPlease provide only the code output without any explanations."
    # Send the prompt to GPT-4 and get the response
    response = openai.ChatCompletion.create(
        model='ft:gpt-4o-mini-2024-07-18:personal::AZxtfdcM',
        messages=[
            {"role": "user", "content": prompt+adprompt}
        ]
    )
    
    # Extract the assistant's reply
    assistant_reply = response['choices'][0]['message']['content']
    
    # Use regex to extract code blocks enclosed in triple backticks
    code_blocks = re.findall(r'```(?:\w*\n)?(.*?)```', assistant_reply, re.DOTALL)
    
    # Combine all code blocks into one string (if multiple code blocks are present)
    code_only = '\n\n'.join(code_blocks).strip()
    
    # Get current date and time for filenames
    current_time = datetime.now()
    timestamp = current_time.strftime("%Y%m%d_%H%M%S")
    
    # Save the original response to a file
    # 파일 위치 
    response_file_path="./log/code/"
    raw_file_path="./log/raw/code/"
    response_filename = f"response_{timestamp}.txt"
    with open(raw_file_path+response_filename, 'w', encoding='utf-8') as f:
        f.write(assistant_reply)
    
    # Save only the code to another file
    code_filename = f"code_{timestamp}.txt"
    with open(response_file_path+code_filename, 'w', encoding='utf-8') as f:
        f.write(code_only)
    
    # Optionally, print the filenames
    print(f"Original response saved to {response_filename}")
    print(f"Extracted code saved to {code_filename}")
    return code_filename

def concept2prompt():

    # Read the prompt from basepropt.txt
    with open('concept.txt', 'r',encoding='UTF-8') as file:
        prompt = file.read()

    # Optionally, instruct the assistant to provide only the code
    #prompt += "\n\nPlease provide only the code output without any explanations."

    # Send the prompt to GPT-4 and get the response
    response = openai.ChatCompletion.create(
        model='ft:gpt-4o-mini-2024-07-18:personal::AZywfmpq',
        messages=[
            {"role": "user", "content": prompt}
        ]
    )

    # Extract the assistant's reply
    conceptrply = response['choices'][0]['message']['content']


    # Get current date and time for filenames
    current_time = datetime.now()
    timestamp = current_time.strftime("%Y%m%d_%H%M%S")

    # Save the original response to a file
    file_path="./log/concept/"
    raw_file_path="./log/raw/concept/"
    response_filename = f"concept{timestamp}.txt"
    with open(file_path+response_filename, 'w', encoding='utf-8') as f:
        f.write(conceptrply)
        
    with open(raw_file_path+response_filename, 'w', encoding='utf-8') as f:
        f.write(conceptrply)


    # Optionally, print the filenames
    print(f"Original response saved to {response_filename}")
    
    return response_filename

def regencode(score,goal_score,filename):
    # Set your OpenAI API key
    
    file_path="./log/code/"
    
    basefilepath=file_path+filename
    # Read the prompt from basepropt.txt
    with open(basefilepath, 'r',encoding='UTF-8') as file:
        prompt = file.read()
    
    # Optionally, instruct the assistant to provide only the code
   
    adprompt = ""
    if(goal_score>score):
        adprompt+="\n\n Please tell me what variables or code I need to change to make it more difficult. and make it harder"
    else:
        adprompt+="\n\nPlease tell me what variables or code I need to change to make it more easy. and make it easier"
    # Send the prompt to GPT-4 and get the response
    response = openai.ChatCompletion.create(
        model='ft:gpt-4o-mini-2024-07-24:personal::mX7vL1qW',
        messages=[
            {"role": "user", "content": prompt+adprompt}
        ]
    )
    
    # Extract the assistant's reply
    assistant_reply = response['choices'][0]['message']['content']
    
    # Use regex to extract code blocks enclosed in triple backticks
    code_blocks = re.findall(r'```(?:\w*\n)?(.*?)```', assistant_reply, re.DOTALL)
    
    # Combine all code blocks into one string (if multiple code blocks are present)
    code_only = '\n\n'.join(code_blocks).strip()
    
    # Get current date and time for filenames
    current_time = datetime.now()
    timestamp = current_time.strftime("%Y%m%d_%H%M%S")
    
    # Save the original response to a file
    # 파일 위치 
    response_file_path="./log/code/"
    raw_file_path="./log/raw/code/"
    response_filename = f"response_{timestamp}.txt"
    with open(raw_file_path+response_filename, 'w', encoding='utf-8') as f:
        f.write(assistant_reply)
    
    # Save only the code to another file
    code_filename = f"code_{timestamp}.txt"
    with open(response_file_path+code_filename, 'w', encoding='utf-8') as f:
        f.write(code_only)
    
    # Optionally, print the filenames
    print(f"Original response saved to {response_filename}")
    print(f"Extracted code saved to {code_filename}")
    return code_filename

