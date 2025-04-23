namespace AFSTranslator.Helpers
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            if (password is not null)
            {
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
                return passwordHash;
            }

            return default;
        }

        public bool Verify(string hashedPassword, string password)
        {
            if (hashedPassword is null || password is null)
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}