using System;

namespace NullAI.Services
{
    /// <summary>
    /// Detects user presence using a webcam or other sensors.  This
    /// implementation is a placeholder; it always returns false.  In
    /// the future, this class could use a library such as OpenCV to
    /// detect faces when the user enables presence detection.
    /// </summary>
    public class PresenceDetection
    {
        public bool IsUserPresent()
        {
            // TODO: Implement actual webcam based detection
            return false;
        }
    }
}