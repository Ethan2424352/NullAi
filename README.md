NullAI
======

This repository contains a WPF application called NullAI.
More documentation will be added in the future.

## How to Download
- Clone the repository:
  ```
  git clone https://github.com/your-username/NullAi.git
  ```
- Alternatively, download the ZIP from the repository page and extract it.

## How to Use
1. Install the .NET SDK with the Windows Desktop workload.
2. Restore and build the project:
   ```
   dotnet restore
   dotnet build
   ```
3. Run the application:
   ```
   dotnet run
   ```
4. Provide any required API keys in the settings window.

## How It Works
The main window hosts several features:
- **AI Chat** – send text or voice prompts that are processed by `AIService`.
- **Drawing Canvas** – sketch directly on the canvas through `DrawingService`.
- **3D Model Generator** – describe an object and `Model3DService` exports models in `.obj` or `.stl` format.
- **Voice Commands** – `SpeechService` listens for commands and triggers custom or trained responses via `VoiceTrainingService`.
- **Presence Detection** – optional webcam-based detection using OpenCV.
- **Privacy Controls** – `PrivacyService` stores user preferences for microphone and webcam usage.

These services are coordinated by `MainWindow` and use helper classes such as `ApiKeyManager` and `CustomVoiceCommand`.

## API Keys
API keys are stored using `ApiKeyManager` and persisted to the user's application data directory via `SecureStorage`. See `API_KEYS.md` for details. A helper script `Scripts/test_api_key.py` can be used to verify that the `OPENAI_API_KEY` environment variable is set and reachable.

