using System.IO;

namespace Game.Core
{
    public class WriteFileStorage:IFileStorage
    {
        public bool Exists(string path) => File.Exists(path);
        public string ReadAllText(string path) => File.ReadAllText(path);
        public void WriteAllText(string path, string data) => File.WriteAllText(path, data);
        public void Delete(string path) => File.Delete(path);
    }
}
