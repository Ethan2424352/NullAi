using System;

namespace NullAI.Services
{
    /// <summary>
    /// Handles voice recognition and maps spoken phrases to actions.
    /// In a production environment this class would wrap a speech
    /// recognition engine such as System.Speech.Recognition.  For
    /// demonstration purposes the implementation here is a stub.
    /// </summary>
    public class SpeechService : IDisposable
    {
        /// <summary>
        /// Raised when a complete command has been recognized.
        /// </summary>
        public event Action<string>? CommandRecognized;

        private Microsoft.CognitiveServices.Speech.SpeechRecognizer? _recognizer;
        private bool _isListening;
        private VoiceTrainingService? _trainer;

        /// <summary>
        /// Optionally configure the service with Azure Cognitive Services
        /// credentials.  If no configuration is provided, the service
        /// remains a stub and <see cref="TriggerCommand"/> can be used
        /// to simulate recognition.
        /// </summary>
        public void ConfigureAzure(string subscriptionKey, string region)
        {
            var config = Microsoft.CognitiveServices.Speech.SpeechConfig.FromSubscription(subscriptionKey, region);
            _recognizer = new Microsoft.CognitiveServices.Speech.SpeechRecognizer(config);
            _recognizer.Recognized += (s, e) =>
            {
                if (e.Result.Reason == Microsoft.CognitiveServices.Speech.ResultReason.RecognizedSpeech)
                {
                    var text = e.Result.Text;
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        DispatchCommand(text);
                    }
                }
            };
        }

        /// <summary>
        /// Attach a <see cref="VoiceTrainingService"/> to enable custom
        /// voice command training.
        /// </summary>
        public void SetTrainingService(VoiceTrainingService trainer)
        {
            _trainer = trainer;
        }

        /// <summary>
        /// Start listening for voice input using the configured recognizer.
        /// </summary>
        public void StartListening()
        {
            if (_recognizer == null || _isListening)
            {
                return;
            }
            _isListening = true;
            _recognizer.StartContinuousRecognitionAsync();
        }

        /// <summary>
        /// Stop listening for voice input.
        /// </summary>
        public void StopListening()
        {
            if (_recognizer != null && _isListening)
            {
                _recognizer.StopContinuousRecognitionAsync();
                _isListening = false;
            }
        }

        /// <summary>
        /// Manually trigger a command for testing.  This method
        /// simulates recognizing a spoken command and raises the
        /// CommandRecognized event.
        /// </summary>
        /// <param name="command">The recognized command text.</param>
        public void TriggerCommand(string command)
        {
            DispatchCommand(command);
        }

        private void DispatchCommand(string command)
        {
            if (_trainer != null && _trainer.TryGetResponse(command, out var response))
            {
                CommandRecognized?.Invoke(response);
            }
            else
            {
                CommandRecognized?.Invoke(command);
            }
        }

        public void Dispose()
        {
            StopListening();
            _recognizer?.Dispose();
        }
    }
}