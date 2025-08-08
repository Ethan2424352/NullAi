using System.Windows;
using NullAI.Services;
using Microsoft.VisualBasic;
using NullAI.Models;

namespace NullAI.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// Provides a simple interface to manage API keys via the
    /// <see cref="ApiKeyManager"/>.  Keys are listed in a ListBox
    /// and can be added or removed.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly ApiKeyManager _manager;

        public SettingsWindow(ApiKeyManager manager)
        {
            InitializeComponent();
            _manager = manager;
            RefreshList();
        }

        private void RefreshList()
        {
            KeyList.ItemsSource = null;
            KeyList.ItemsSource = _manager.Keys;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            // Use VisualBasic InputBox for simplicity.  This requires
            // reference to Microsoft.VisualBasic which is provided by .NET.
            string key = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter API Key:", "Add API Key", string.Empty);
            if (!string.IsNullOrWhiteSpace(key))
            {
                try
                {
                    _manager.AddKey(key);
                    _manager.Save();
                    RefreshList();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Failed to add key: " + ex.Message);
                }
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var item = KeyList.SelectedItem as ApiKeyConfig;
            if (item != null)
            {
                _manager.RemoveKey(item);
                _manager.Save();
                RefreshList();
            }
        }
    }
}