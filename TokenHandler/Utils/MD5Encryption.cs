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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace TokenHandler.Utils
{
    /// <summary>
    /// Class   :   Cryptage et décryptage du mot de passe au format MD5
    /// </summary>
    /// <remarks>Code du ERP</remarks>
    /// <returns></returns>
    public sealed class Md5Encryption
    {
        #region Membres
        /// <summary>
        /// Declare :   Le texte à Crypter.
        /// </summary>
        private readonly string _fText;
        #endregion

        #region Propriétés.

        #endregion

        #region Constructeur

        /// <summary>
        /// Instance d'entrée.
        /// </summary>
        /// <param name="text">Obtenir le text à crypter</param>
        /// <remarks></remarks>
        /// <example></example>
        public Md5Encryption(string text)
        {
            _fText = text;
        }

        #endregion

        #region Méthodes publiques.
        #region Retour par l'instance
        /// <summary>
        /// Méthode :   Remplacer ToString ().
        /// </summary>
        /// <returns>La chaine cryptée sous forme de caractère.</returns>
        /// <remarks></remarks>
        /// <example></example>
        public override string ToString()
        {
            return ToByteArray().Aggregate<byte, string>(null, (current, b) => current + Convert.ToString(b, 16).PadLeft(2, '0').ToUpper());
        }
        /// <summary>
        /// Méthode :   remplacer ToByteArray().
        /// </summary>
        /// <returns>La chaine cryptée sous format de bytes.</returns>
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

        #region Traitement interne
        /// <summary>
        /// Method  : Obtenir une somme de contrôle unique à partir d'un flux.
        /// </summary>
        /// <param name="streamIn">Le flux souhaité en hacher.</param>
        /// <returns>Une somme de contrôle MD5.</returns>
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
        /// Method  :   MD5 est-il au format.
        /// </summary>
        /// <param name="md5String">Obtenir la chaîne sous forme MD5</param>
        /// <returns></returns>
        /// <remarks></remarks>
        /// <example></example>
        public static bool IsValidMd5(string md5String)
        {
            return !string.IsNullOrWhiteSpace(md5String) && Regex.IsMatch(md5String, "[a-fA-F0-9]{32}");
        }
        #endregion

        #region Appel au cryptage ou décryptage.
        /// <summary>
        /// Method  :   Crypte la chaîne ou mot de passe demandé.
        /// </summary>
        /// <param name="message">Obtenir la chaîne à crypter</param>
        /// <param name="passphrase">Obtenir le mot de passe pour le cryptage</param>
        /// <returns>La chaine Cryptée.</returns>
        /// <remarks>Sauver le TDESAlgorithm.IV car indispensable pour pouvoir
        /// décrypter le mot de passe</remarks>
        /// <example></example>
        public static string EncryptString(string message, string passphrase)
        {
            byte[] results;
            var utf8 = new UTF8Encoding();

            // Étape 1: On hache le texte secret en utilisant le format MD5.
            // On utilise le générateur de hachage MD5 car le résultat est un tableau d'octets de 128 bits
            // qui est une longueur valide pour l'encodeur TripleDES que l'on utilise ci-dessous

            var hashProvider = new MD5CryptoServiceProvider();
            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(passphrase));

            // Étape 2: Créer un nouvel objet TripleDESCryptoServiceProvider
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            // Étape 3: Configurer l'encodeur

            // Étape 4: Convertir la chaîne d'entrée en octet []
            var dataToEncrypt = utf8.GetBytes(message);

            // Étape 5: Essayer de chiffrer la chaîne
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                // Effacer les services TripleDes et Hash Provider de toute information sensible
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            // Étape 6: Renvoyer la chaîne cryptée sous la forme d'une chaîne codée en base64.
            return Convert.ToBase64String(results);
        }
        /// <summary>
        /// Method : Décrypte la chaîne ou mot de passe demandé.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="passphrase"></param>
        /// <returns></returns>
        /// <remarks>Utiliser le IV en DB pour pouvoir décrypter</remarks>
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
