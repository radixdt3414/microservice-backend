namespace authentication.API.Services
{
    public interface IPasswordHasher
    {
        public void HashPassword(string password, out string hasedSalt, out string hashedPassword);
        public bool VerifyPassword(string password, string hasedSalt,  string hashedPassword);
    }
}
