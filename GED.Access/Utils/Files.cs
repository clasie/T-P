using GED.Access.Const;
using System.Diagnostics;
using System.IO;

namespace GED.Access.Utils
{
    public static class Files
    {
        #region FileExists
        /// <summary>
        /// Encapsulation de FileInfo.Exists();
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public static bool FileExists(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return false;

            return new FileInfo(fileUrl).Exists;
        }
        #endregion

        /// <summary>
        /// Validate file extension
        /// </summary>
        /// <param name="urlFile"></param>
        /// <returns></returns>
        public static bool HasExtension(string urlFile)
        {
            return GedConstants.ExtensionAcceptee.Equals(new FileInfo(urlFile).Extension.ToLower());
        }
        /// <summary>
        /// Affiche un fichier
        /// </summary>
        /// <param name="urlFile"></param>
        /// <returns></returns>
        public static bool DisplayTheFile(string urlFile) {
            bool result = false;
            if (FileExists(urlFile)){
                result = true;
                Process.Start(urlFile);
            }
            return result;
        }
    }
}