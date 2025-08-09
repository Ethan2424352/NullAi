#!/usr/bin/env python3
"""Simple script to verify that an OpenAI API key is configured.
If the environment variable OPENAI_API_KEY is present, a lightweight
request is sent to the API. Otherwise a message is printed indicating
that no key is available.
"""
import os
import sys
import requests

API_URL = "https://api.openai.com/v1/models"

def main():
    key = os.environ.get("OPENAI_API_KEY")
    if not key:
        print("No API key found.")
        return 0
    try:
        headers = {"Authorization": f"Bearer {key}"}
        resp = requests.get(API_URL, headers=headers, timeout=5)
        if resp.status_code == 200:
            print("API key valid; accessible models retrieved.")
        else:
            print(f"API request failed with status {resp.status_code}.")
    except Exception as exc:
        print(f"Error contacting API: {exc}")
        return 1
    return 0

if __name__ == "__main__":
    sys.exit(main())
