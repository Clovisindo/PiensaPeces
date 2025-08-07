namespace Game.Core
{
    public interface IFileStorage
    {
        bool Exists(string path);
        string ReadAllText(string path);
        void WriteAllText(string path, string data);
        void Delete(string path);
    }
}
