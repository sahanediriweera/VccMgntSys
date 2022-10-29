using System;
using System.Security.Cryptography;
using System.Text;

namespace VccMgntSys.Security
{
    public static class MyRSAEncrytion
    {
        private static readonly UnicodeEncoding ByteConverter = new UnicodeEncoding();
        private static byte[] encryptedData;

        public static String GenerateRSAEncryption(String password)
        {

            byte[] brokenpass = ByteConverter.GetBytes(password);

            using(RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                encryptedData = RSAEncrypt(brokenpass, RSA.ExportParameters(false), false);
            }

            return ByteConverter.GetString(encryptedData);
        }

        private static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;

                using(RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKeyInfo);
                    encryptedData = RSA.Encrypt(DataToEncrypt,DoOAEPPadding);
                }

                return encryptedData;
            }

            catch(CryptographicException e)
            {
                return null;
            }
        }

    }
}
