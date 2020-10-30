namespace ClassLibraryFriendly.Models
{
    public interface IGerenciamentoCookie
    {
        void Create(string nome, string sobrenome, string email, string data);
        void Remove();
    }
}