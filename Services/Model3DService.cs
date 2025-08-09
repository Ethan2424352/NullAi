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

        public enum ModelFormat
        {
            Obj,
            Stl
        }

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
        /// Generate a simple 3D model based on a text description in the
        /// requested format. Currently supports OBJ and ASCII STL.
        /// </summary>
        public async Task GenerateModelAsync(string description, string outputPath, ModelFormat format = ModelFormat.Obj)
        {
            if (description == null) throw new ArgumentNullException(nameof(description));
            if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentException("Output path is required.", nameof(outputPath));
            try
            {
                // Write mesh asynchronously to avoid blocking the UI thread
                await Task.Run(() =>
                {
                    var lower = description.ToLowerInvariant();
                    var isSphere = lower.Contains("sphere");
                    switch (format)
                    {
                        case ModelFormat.Obj:
                            using (var writer = new StreamWriter(outputPath))
                            {
                                if (isSphere)
                                    WriteSphereObj(writer, 1.0, 8);
                                else
                                    WriteCubeObj(writer, 1.0);
                            }
                            break;
                        case ModelFormat.Stl:
                            using (var writer = new StreamWriter(outputPath))
                            {
                                if (isSphere)
                                    WriteSphereStl(writer, 1.0, 8);
                                else
                                    WriteCubeStl(writer, 1.0);
                            }
                            break;
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex);
                throw;
            }
        }

        private static void WriteCubeObj(StreamWriter writer, double size)
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

        private static void WriteSphereObj(StreamWriter writer, double radius, int segments)
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

        private static void WriteCubeStl(StreamWriter writer, double size)
        {
            double s = size / 2.0;
            writer.WriteLine("solid cube");
            void Tri((double,double,double)a,(double,double,double)b,(double,double,double)c)
            {
                writer.WriteLine("  facet normal 0 0 0");
                writer.WriteLine("    outer loop");
                writer.WriteLine($"      vertex {a.Item1} {a.Item2} {a.Item3}");
                writer.WriteLine($"      vertex {b.Item1} {b.Item2} {b.Item3}");
                writer.WriteLine($"      vertex {c.Item1} {c.Item2} {c.Item3}");
                writer.WriteLine("    endloop");
                writer.WriteLine("  endfacet");
            }
            var v = new (double,double,double)[]{
                (-s,-s,-s),(s,-s,-s),(s,s,-s),(-s,s,-s),
                (-s,-s,s),(s,-s,s),(s,s,s),(-s,s,s)
            };
            Tri(v[0],v[1],v[2]); Tri(v[0],v[2],v[3]);
            Tri(v[4],v[5],v[6]); Tri(v[4],v[6],v[7]);
            Tri(v[0],v[1],v[5]); Tri(v[0],v[5],v[4]);
            Tri(v[2],v[3],v[7]); Tri(v[2],v[7],v[6]);
            Tri(v[1],v[2],v[6]); Tri(v[1],v[6],v[5]);
            Tri(v[0],v[3],v[7]); Tri(v[0],v[7],v[4]);
            writer.WriteLine("endsolid cube");
        }

        private static void WriteSphereStl(StreamWriter writer, double radius, int segments)
        {
            writer.WriteLine("solid sphere");
            for (int i = 0; i < segments; i++)
            {
                double lat1 = Math.PI * i / segments;
                double lat2 = Math.PI * (i + 1) / segments;
                for (int j = 0; j < segments; j++)
                {
                    double lon1 = 2 * Math.PI * j / segments;
                    double lon2 = 2 * Math.PI * (j + 1) / segments;
                    var p1 = (radius * Math.Sin(lat1) * Math.Cos(lon1), radius * Math.Sin(lat1) * Math.Sin(lon1), radius * Math.Cos(lat1));
                    var p2 = (radius * Math.Sin(lat2) * Math.Cos(lon1), radius * Math.Sin(lat2) * Math.Sin(lon1), radius * Math.Cos(lat2));
                    var p3 = (radius * Math.Sin(lat2) * Math.Cos(lon2), radius * Math.Sin(lat2) * Math.Sin(lon2), radius * Math.Cos(lat2));
                    var p4 = (radius * Math.Sin(lat1) * Math.Cos(lon2), radius * Math.Sin(lat1) * Math.Sin(lon2), radius * Math.Cos(lat1));
                    WriteTri(writer, p1, p2, p3);
                    WriteTri(writer, p1, p3, p4);
                }
            }
            writer.WriteLine("endsolid sphere");

            static void WriteTri(StreamWriter w, (double,double,double) a, (double,double,double) b, (double,double,double) c)
            {
                w.WriteLine("  facet normal 0 0 0");
                w.WriteLine("    outer loop");
                w.WriteLine($"      vertex {a.Item1} {a.Item2} {a.Item3}");
                w.WriteLine($"      vertex {b.Item1} {b.Item2} {b.Item3}");
                w.WriteLine($"      vertex {c.Item1} {c.Item2} {c.Item3}");
                w.WriteLine("    endloop");
                w.WriteLine("  endfacet");
            }
        }
    }
}