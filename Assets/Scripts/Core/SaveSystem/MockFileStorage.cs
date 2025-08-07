using System.Collections.Generic;
using System.IO;

namespace Game.Core
{
    public class MockFileStorage : IFileStorage
    {
        private readonly Dictionary<string, string> fakeFiles = new();
        public void Delete(string path)
        {
            if (fakeFiles.ContainsKey(path))
                fakeFiles.Remove(path);
        }

        public bool Exists(string path) => fakeFiles.ContainsKey(path);

        public string ReadAllText(string path)
        {
            if (!fakeFiles.ContainsKey(path)) throw new FileNotFoundException();
            return fakeFiles[path];
        }

        public void WriteAllText(string path, string data)
        {
            fakeFiles[path] = data;
        }

        // Para poder inspeccionar el valor en tests
        public string GetContent(string path) => fakeFiles.TryGetValue(path, out var content) ? content : null;
    }
}
