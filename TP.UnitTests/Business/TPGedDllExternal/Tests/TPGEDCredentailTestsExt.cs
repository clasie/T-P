using GED.Access;
using GED.Access.CustomExceptions;
using GED.Access.GEDJsonClasses;
using GED.Access.Utils;
using System;
using System.Diagnostics;
using System.IO;
using TP.UnitTests.GlobalConfig.OrderTests;
using TP.UnitTests.Util;
using TP.UnitTests.Utils;
using Xunit;

namespace TP.UnitTests.Business.TPTestUnitaires
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TPTests
    {

        /// <summary>
        /// Test l'exe DllExt download wrong File type good bare code
        /// </summary>
        [Fact, Priority(0)]
        public void TPGEDExternal_FileType_TestFalse()
        {
            try
            {
                #region Arrange: Init instance
                string barCode = ResourceFileHelper.GetBareCodeUploadedFileToGed();
                Uri finalUri = new Uri(ResourceFileHelper.GetParent(ResourceFileHelper.GetParent(new Uri(Environment.CurrentDirectory))), ConstantsTPGedExt.PathVersExe);
                ProcessStartInfo ProcessInfo;
                ProcessInfo = new ProcessStartInfo(finalUri.ToString(), $"{barCode} { ConstantsTPGedExt.WrongFactureFileType}");
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = true;
                #endregion

                #region Act: Aller chercher le token
                Process exeProcess = Process.Start(ProcessInfo);
                Process.Start(ProcessInfo).WaitForExit();
                #endregion

                #region Assert
                Assert.Equal(ConstantsTPGedExt.ExpectedReturnWrongFileType, exeProcess.ExitCode);
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
        /// <summary>
        /// Test l'exe DllExt download good File type Good barecode
        /// </summary>
        [Fact, Priority(0)]
        public void TPGEDExternal_FileType_TestTrue()
        {
            try
            {
                #region Arrange: Init instance
                string barCode = ResourceFileHelper.GetBareCodeUploadedFileToGed();
                Uri finalUri = new Uri(ResourceFileHelper.GetParent(ResourceFileHelper.GetParent(new Uri(Environment.CurrentDirectory))), ConstantsTPGedExt.PathVersExe);
                ProcessStartInfo ProcessInfo;
                ProcessInfo = new ProcessStartInfo(finalUri.ToString(), $"{barCode} { ConstantsTPGedExt.GoodFactureFileType}");
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = true;
                #endregion

                #region Act: Aller chercher le token
                Process exeProcess = Process.Start(ProcessInfo);
                Process.Start(ProcessInfo).WaitForExit();
                #endregion

                #region Assert
                Assert.Equal(ConstantsTPGedExt.ExpectedReturnNoError, exeProcess.ExitCode);
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
        /// <summary>
        /// Test l'exe DllExt download good File type and wrong barecode
        /// </summary>
        [Fact, Priority(0)]
        public void TPGEDExternal_CodeBare_TestFalse()
        {
            try
            {
                #region Arrange: Init instance
                Uri finalUri = new Uri(ResourceFileHelper.GetParent(ResourceFileHelper.GetParent(new Uri(Environment.CurrentDirectory))), ConstantsTPGedExt.PathVersExe);
                ProcessStartInfo ProcessInfo;
                ProcessInfo = new ProcessStartInfo(finalUri.ToString(), $"{ConstantsTPGedExt.WrongBareCode} { ConstantsTPGedExt.GoodFactureFileType}");
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = true;
                #endregion

                #region Act: Aller chercher le token
                Process exeProcess = Process.Start(ProcessInfo);
                Process.Start(ProcessInfo).WaitForExit();
                #endregion

                #region Assert
                Assert.Equal(ConstantsTPGedExt.ExpectedReturnWrongBareCode, exeProcess.ExitCode);
                #endregion
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }
    }
}