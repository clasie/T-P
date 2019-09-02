using GED.Access.CustomExceptions;
using GED.Access.Utils;

namespace GED.Access.Helpers
{
    public static class FileValidationHelper
    {
        /// <summary>
        /// Valide le fichier candidat à l'upload
        /// </summary>
        /// <param name="urlFile"></param>
        /// <returns></returns>
        public static bool ValidateFile(string urlFile) {

            //si fichier n'existe pas
            if (!Files.FileExists(urlFile))
            {
                throw new GEDUploadFileException(TP.Resources.Common.CommonResources.Error_FichierInexistant);
            }
            //si fichier pas l'extension attendue
            if (!Files.HasExtension(urlFile))
            {
                throw new GEDUploadFileException(TP.Resources.Common.CommonResources.Error_FichierNonPdf);
            }
            return true;
        }
    }
}