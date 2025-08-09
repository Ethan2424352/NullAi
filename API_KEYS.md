# API Key Management

The application uses a secure storage file located in the user's application data directory to persist API keys. Each entry includes the key value and the date it was added.

Example structure:

```json
[
  { "key": "sk-your-openai-key", "addedAt": "2024-01-01T00:00:00Z" }
]
```

Use the built-in `ApiKeyManager` service to add or remove keys at runtime.
