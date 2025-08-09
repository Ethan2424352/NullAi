using System;
using System.IO;
using OpenCvSharp;

namespace NullAI.Services
{
    /// <summary>
    /// Detects user presence using a webcam. The implementation captures
    /// a single frame and runs an OpenCV Haar cascade face detector.
    /// </summary>
    public class PresenceDetection
    {
        /// <summary>
        /// Attempt to detect whether a user is present by capturing a
        /// single frame from the default webcam and running a Haar
        /// cascade face detector. Returns <c>true</c> if at least one face
        /// is detected. Any errors result in <c>false</c> with the error
        /// logged via <see cref="ErrorLogger"/>.
        /// </summary>
        public bool IsUserPresent()
        {
            try
            {
                using var capture = new VideoCapture(0);
                if (!capture.IsOpened())
                {
                    return false;
                }
                using var frame = new Mat();
                capture.Read(frame);
                if (frame.Empty())
                {
                    return false;
                }
                var cascadePath = Path.Combine(AppContext.BaseDirectory,
                    "Resources", "haarcascade_frontalface_default.xml");
                using var cascade = new CascadeClassifier(cascadePath);
                var faces = cascade.DetectMultiScale(frame);
                return faces.Length > 0;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
                return false;
            }
        }
    }
}