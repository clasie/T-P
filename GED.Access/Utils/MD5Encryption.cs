// ***********************************************************************************
//
//      Solution            :   WebServiceComptaPlus
//      Projet              :   SideFrameWork
//
//      Titre               :   Gestion de Cryptage et décryptage.
//      Description         :   -
//
//      Démarrage           :   28-09-2018
//      Version langage C#  :   4.0
//
//      Assembly            :   4.5.2 (Version de D365)
//
//
// ***********************************************************************************
//
//      Client              :   Thomas & Piron
//      Projet métier       :   Compta Plus
//
//      Manager projet      :   François Piret
//      Chef de Projet      :   Laurent Mathieu
//
//      CopyRight           :   Side sa
//
// ***********************************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GED.Access.Utils
{
    /// <summary>
    /// Class   :   Encrypt and decrypt password in MD5 format
    /// </summary>
    /// <remarks>Code du ERP</remarks>
    /// <returns></returns>
    public sealed class Md5Encryption
    {
        #region Membres
        /// <summary>
        /// Declare : Text to encrypt.
        /// </summary>
        private readonly string _fText;
        #endregion

        #region Propriétés.

        #endregion

        #region Constructeur

        /// <summary>
        /// Entry instance.
        /// </summary>
        /// <param name="text">Obtain the text to encrypt</param>
        /// <remarks></remarks>
        /// <example></example>
        public Md5Encryption(string text)
        {
            _fText = text;
        }

        #endregion

        #region Public mehods.
        #region Convert to string
        /// <summary>
        /// Méthode :   Override ToString() ().
        /// </summary>
        /// <returns>Encrypted chain in string format</returns>
        /// <remarks></remarks>
        /// <example></example>
        public override string ToString()
        {
            return ToByteArray().Aggregate<byte, string>(null, (current, b) => current + Convert.ToString(b, 16).PadLeft(2, '0').ToUpper());
        }
        /// <summary>
        /// Méthode :   Override the ToByteArray().
        /// </summary>
        /// <returns>Encrypted chain into byte format.</returns>
        /// <remarks></remarks>
        /// <example></example>
        public byte[] ToByteArray()
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var b = md5.ComputeHash(Encoding.UTF8.GetBytes(_fText));
                return b;
            }
        }
        #endregion

        #region MD5 process
        /// <summary>
        /// Method  : Get the Hash value.
        /// </summary>
        /// <param name="streamIn">Stream to hash.</param>
        /// <returns>Sum MD5 control chain.</returns>
        /// <remarks></remarks>
        /// <example></example>
        public static string GetMd5(Stream streamIn)
        {
            try
            {
                streamIn.Position = 0;
                var sb = new StringBuilder();
                MD5 md5 = new MD5CryptoServiceProvider();

                var retVal = md5.ComputeHash(streamIn);

                foreach (var t in retVal)
                {
                    sb.Append(t.ToString("x2"));
                }
                return sb.ToString();
            }
            catch { return ""; }
        }
        /// <summary>
        /// Method  :   Test if MD5 is in the right format.
        /// </summary>
        /// <param name="md5String">Chain in the MD5 format</param>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <example></example>
        public static bool IsValidMd5(string md5String)
        {
            return !string.IsNullOrWhiteSpace(md5String) && Regex.IsMatch(md5String, "[a-fA-F0-9]{32}");
        }
        #endregion

        #region Call to encrypt/decrypt.
        /// <summary>
        /// Method  : Encrypt the string passed.
        /// </summary>
        /// <param name="message">String to encrypt</param>
        /// <param name="passphrase">Password for the encryption</param>
        /// <returns>Encrypted string.</returns>
        /// <remarks>Encrypt a string with a password</remarks>
        /// <example></example>
        public static string EncryptString(string message, string passphrase)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            // Step 1: We hash the secret string with the MD5.
            // We use the MD5 hash generator because the result is an array of 128 bit octets
            // wich is a valid length for the TripleDES encoder used below.

            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

            //Step 2: Create  a new TripleDESCryptoServiceProvider.
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            // Step 3: Configure the encoder.

            //Step 4: Convert the entry chain in [] octets
            var dataToEncrypt = utf8.GetBytes(message);

            // Step 5: Try to decrypt the chain.
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                // Cleanse TripleDes and Hash Provider from any private data.
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6: Return the encrypted chain as a base64 string.
            return Convert.ToBase64String(results);
        }
        /// <summary>
        /// Decrypt the chain or the password asked.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        /// <remarks>Use the IV in DB to decrypt</remarks>
        /// <example></example>
        public static string DecryptString(string message, string passphrase)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            // Step 3. Setup the decoder

            // Step 4. Convert the input string to a byte[]
            var dataToDecrypt = Convert.FromBase64String(message);

            // Step 5. Attempt to decrypt the string
            try
            {
                var decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return utf8.GetString(results);
        }
        #endregion

        #endregion

        #region Méthodes privées.
        #endregion
    }
}
