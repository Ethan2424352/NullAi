using System;
using System.Windows;
using NullAI.Models;

namespace NullAI.Views
{
    /// <summary>
    /// Interaction logic for VoiceTrainingWindow.xaml
    /// Collects a phrase/response pair from the user and passes it
    /// back to the caller via a callback.
    /// </summary>
    public partial class VoiceTrainingWindow : Window
    {
        private readonly Action<CustomVoiceCommand> _onSave;

        public VoiceTrainingWindow(Action<CustomVoiceCommand> onSave)
        {
            InitializeComponent();
            _onSave = onSave;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var phrase = PhraseBox.Text?.Trim();
            var response = ResponseBox.Text?.Trim();
            if (string.IsNullOrWhiteSpace(phrase) || string.IsNullOrWhiteSpace(response))
            {
                MessageBox.Show("Please enter both a phrase and a response.");
                return;
            }
            _onSave(new CustomVoiceCommand
            {
                Phrase = phrase!,
                Response = response!,
                CreatedAt = DateTime.UtcNow
            });
            Close();
        }
    }
}