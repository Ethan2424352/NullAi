using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using NullAI.Models;
using NullAI.Services;
using NullAI.Utilities;
using NullAI.Views;

namespace NullAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.  This is the primary
    /// window hosting the AI chat, drawing canvas and 3D model
    /// generator.  It coordinates various services and UI events.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApiKeyManager _apiKeyManager;
        private readonly AIService _aiService;
        private readonly SpeechService _speechService;
        private readonly DrawingService _drawingService;
        private readonly Model3DService _model3DService;
        private readonly List<CustomVoiceCommand> _voiceCommands;

        public MainWindow()
        {
            InitializeComponent();
            Title = $"{Constants.ApplicationName} ({PlatformHelper.GetExecutableName()})";
            _apiKeyManager = new ApiKeyManager();
            _aiService = new AIService(_apiKeyManager);
            _speechService = new SpeechService();
            _drawingService = new DrawingService();
            _model3DService = new Model3DService();
            _voiceCommands = new List<CustomVoiceCommand>();
            // Attach drawing canvas events
            _drawingService.AttachCanvas(DrawingCanvas);
            // Subscribe to speech commands
            _speechService.CommandRecognized += SpeechService_CommandRecognized;
            _speechService.StartListening();
        }

        private async void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            e.Handled = true;
            var text = InputBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(text)) return;
            InputBox.Clear();
            AppendChat($"You: {text}\n");
            // Check if it matches a trained voice command
            var custom = _voiceCommands.FirstOrDefault(c => text.StartsWith(c.Phrase, StringComparison.OrdinalIgnoreCase));
            if (custom != null)
            {
                AppendChat($"NullAI: {custom.Response}\n");
                return;
            }
            // Otherwise call AI service
            var response = await _aiService.ProcessCommandAsync(text);
            AppendChat($"NullAI: {response}\n");
        }

        private void AppendChat(string message)
        {
            ChatBox.AppendText(message);
            ChatBox.ScrollToEnd();
        }

        private void SpeechService_CommandRecognized(string obj)
        {
            // Handle recognized speech commands by forwarding them to the input handler
            Dispatcher.Invoke(async () =>
            {
                // Prepend recognized command into chat
                AppendChat($"Voice: {obj}\n");
                var custom = _voiceCommands.FirstOrDefault(c => obj.StartsWith(c.Phrase, StringComparison.OrdinalIgnoreCase));
                if (custom != null)
                {
                    AppendChat($"NullAI: {custom.Response}\n");
                    return;
                }
                var response = await _aiService.ProcessCommandAsync(obj);
                AppendChat($"NullAI: {response}\n");
            });
        }

        private async void GenerateModel_Click(object sender, RoutedEventArgs e)
        {
            var description = ModelDescription.Text?.Trim();
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter a description.");
                return;
            }
            var sfd = new SaveFileDialog
            {
                Title = "Save 3D Model",
                Filter = "OBJ Files (*.obj)|*.obj|All Files (*.*)|*.*",
                FileName = "model.obj"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    await _model3DService.GenerateModelAsync(description, sfd.FileName);
                    MessageBox.Show("Model saved.");
                }
                catch (Exception ex)
                {
                    ErrorLogger.Log(ex);
                    MessageBox.Show("Failed to generate model: " + ex.Message);
                }
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var win = new SettingsWindow(_apiKeyManager);
            win.Owner = this;
            win.ShowDialog();
        }

        private void TrainVoice_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VoiceTrainingWindow(cmd =>
            {
                _voiceCommands.Add(cmd);
                AppendChat($"Voice command '{cmd.Phrase}' added.\n");
            });
            dialog.Owner = this;
            dialog.ShowDialog();
        }
    }
}