using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace NullAI.Services
{
    /// <summary>
    /// Handles generation and loading of 3D models.  The
    /// implementation here is deliberately simple: if the description
    /// contains the word "sphere" it writes a rudimentary sphere
    /// mesh to the destination, otherwise a cube is produced.  The
    /// default model is embedded as a resource and can be extracted
    /// on demand.
    /// </summary>
    public class Model3DService
    {
        private const string ResourcePrefix = "NullAI.Resources";

        /// <summary>
        /// Extract the embedded default model to a temporary file and
        /// return its path.
        /// </summary>
        public string GetDefaultModel()
        {
            var resourceName = ResourcePrefix + ".default_model.obj";
            var tempPath = Path.Combine(Path.GetTempPath(), "default_model.obj");
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException("Resource " + resourceName + " not found.");
            using var file = File.Create(tempPath);
            stream.CopyTo(file);
            return tempPath;
        }

        /// <summary>
        /// Generate a simple 3D model (OBJ format) based on a text
        /// description.
        /// </summary>
        /// <param name="description">User description.</param>
        /// <param name="outputPath">File path to write the OBJ file.</param>
        public async Task GenerateModelAsync(string description, string outputPath)
        {
            if (description == null) throw new ArgumentNullException(nameof(description));
            if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentException("Output path is required.", nameof(outputPath));
            // Write mesh asynchronously to avoid blocking the UI thread
            await Task.Run(() =>
            {
                var lower = description.ToLowerInvariant();
                var isSphere = lower.Contains("sphere");
                using var writer = new StreamWriter(outputPath);
                if (isSphere)
                {
                    WriteSphere(writer, 1.0, 8);
                }
                else
                {
                    WriteCube(writer, 1.0);
                }
            });
        }

        private static void WriteCube(StreamWriter writer, double size)
        {
            double s = size / 2.0;
            // Vertices
            (double, double, double)[] vertices = new[]
            {
                (-s, -s, -s), (s, -s, -s), (s, s, -s), (-s, s, -s),
                (-s, -s, s),  (s, -s, s),  (s, s, s),  (-s, s, s),
            };
            foreach (var v in vertices)
            {
                writer.WriteLine(string.Format("v {0} {1} {2}", v.Item1, v.Item2, v.Item3));
            }
            // Faces (1‑based indexing)
            int[][] faces = new[]
            {
                new[] {1, 2, 3, 4}, // back
                new[] {5, 6, 7, 8}, // front
                new[] {1, 2, 6, 5}, // bottom
                new[] {3, 4, 8, 7}, // top
                new[] {2, 3, 7, 6}, // right
                new[] {1, 4, 8, 5}, // left
            };
            foreach (var face in faces)
            {
                writer.WriteLine("f " + string.Join(" ", face));
            }
        }

        private static void WriteSphere(StreamWriter writer, double radius, int segments)
        {
            // Simple UV sphere generation
            for (int i = 0; i <= segments; i++)
            {
                double lat = Math.PI * i / segments;
                for (int j = 0; j < segments; j++)
                {
                    double lon = 2 * Math.PI * j / segments;
                    double x = radius * Math.Sin(lat) * Math.Cos(lon);
                    double y = radius * Math.Sin(lat) * Math.Sin(lon);
                    double z = radius * Math.Cos(lat);
                    writer.WriteLine(string.Format("v {0} {1} {2}", x, y, z));
                }
            }
            for (int i = 0; i < segments; i++)
            {
                for (int j = 0; j < segments; j++)
                {
                    int p1 = i * segments + j + 1;
                    int p2 = p1 + segments;
                    int p3 = p2 + 1;
                    if ((j + 1) % segments == 0) p3 -= segments;
                    int p4 = p1 + 1;
                    if ((j + 1) % segments == 0) p4 -= segments;
                    writer.WriteLine("f " + string.Join(" ", new[] { p1, p2, p3, p4 }));
                }
            }
        }
    }
}