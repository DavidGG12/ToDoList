using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Engines;
using System.Text;
using Org.BouncyCastle.Crypto.Modes;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace ToDoList.Helpers
{
    public class Cryptho
    {
        protected byte[] key;
        protected byte[] nonce;
        protected SecureRandom random;

        public Cryptho()
        {
            this.key = new byte[32];
            this.nonce = new byte[12];
            this.random = new SecureRandom();

            this.random.NextBytes(this.key);
            this.random.NextBytes(this.nonce);
        }

        public string Encrypt(string word)
        {
            byte[] mssgByte = Encoding.UTF8.GetBytes(word);
            byte[] cyphr = new byte[mssgByte.Length + 16];

            var cipher = new ChaCha20Poly1305();
            var parameters = new AeadParameters(new KeyParameter(this.key), 128, this.nonce, null);
            cipher.Init(true, parameters);
            int len = cipher.ProcessBytes(mssgByte, 0, mssgByte.Length, cyphr, 0);
            cipher.DoFinal(cyphr, 0);

            return Convert.ToBase64String(cyphr);
        }

        public string Decrypt(string word)
        {
            byte[] mssgByte = Encoding.UTF8.GetBytes(word);
            byte[] mssg = new byte[mssgByte.Length + 16];

            var decipher = new ChaCha20Poly1305();
            var parameters = new AeadParameters(new KeyParameter(this.key), 128, this.nonce, null);

            decipher.Init(false, parameters);
            byte[] desc = new byte[mssgByte.Length];
            int descLen = decipher.ProcessBytes(mssg, 0, mssg.Length, desc, 0);
            decipher.DoFinal(desc, descLen);

            return Encoding.UTF8.GetString(desc);
        }
    }
}
