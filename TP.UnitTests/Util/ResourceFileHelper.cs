using GED.Access;
using GED.Access.Enums;
using GED.Access.GEDJsonClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP.UnitTests.Business.Constants;
using TP.UnitTests.Utils;

namespace TP.UnitTests.Util
{
    public static class ResourceFileHelper
    {
        public static string GetResourceFilePath(string fileName) {
            return $"{ConstantsUnitTests.ResourcesDirName}{ConstantsUnitTests.PathSlash}{fileName}";
        }
        public static Uri GetParent(Uri uri)
        {
            return new Uri(uri, "..");
        }
        public static string GetBareCodeUploadedFileToGed()
        {
            var gedAccess = new Ged(
                    ConstantsTPGed.GedUserLogin,
                    ConstantsTPGed.GedUserPassword,
                    ConstantsTPGed.GedUrlToTestWEB
                );
            var fileUri = ResourceFileHelper.GetResourceFilePath(ConstantsTPGed.GEDFacturePdf_ResourceFileName);
            var barCode = Guid.NewGuid().ToString();
            //Upload it
            gedAccess.UploadFile(
                barCode,
                fileUri,
                GedService.DocTypesEnum.FAC);
            return barCode;
        }
    }
}
