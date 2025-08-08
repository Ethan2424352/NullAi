using System;
using System.Collections.Generic;
using NullAI.Models;
using NullAI.Utilities;

namespace NullAI.Services
{
    /// <summary>
    /// Service responsible for managing a collection of API keys used by
    /// various parts of the application.  Keys are persisted via
    /// <see cref="SecureStorage"/> and exposed as strongly typed
    /// <see cref="ApiKeyConfig"/> instances.
    /// </summary>
    public class ApiKeyManager
    {
        private readonly List<ApiKeyConfig> _keys;

        /// <summary>
        /// Exposes the list of API keys as a read‑only collection.
        /// </summary>
        public IReadOnlyList<ApiKeyConfig> Keys => _keys.AsReadOnly();

        public ApiKeyManager()
        {
            // Load persisted keys from secure storage at start up
            _keys = SecureStorage.LoadKeys();
        }

        /// <summary>
        /// Add a new API key to the manager.  This will not
        /// immediately persist the key; call <see cref="Save"/> to
        /// persist changes.
        /// </summary>
        /// <param name="key">The raw API key.</param>
        public void AddKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("API key cannot be empty.", nameof(key));
            }
            _keys.Add(new ApiKeyConfig
            {
                Key = key.Trim(),
                AddedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Remove an API key from the manager.
        /// </summary>
        public void RemoveKey(ApiKeyConfig config)
        {
            _keys.Remove(config);
        }

        /// <summary>
        /// Persist the current key collection to secure storage.
        /// </summary>
        public void Save()
        {
            SecureStorage.SaveKeys(_keys);
        }
    }
}