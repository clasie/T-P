
//-------------------------------------------------------------
// This Windows service calls Web services as listed inside his App.config
//-------------------------------------------------------------
//
//  Script d'install/uninstall sont sur le serveur rdp (10.250.16.90)  ici: E:\WSComptaPlusWindowsService (run as administrator)
//
// 1.0- To install the Windows service command as administrator
//-------------------------------------------------------------
// C:\windows\system32>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe  D:\YOUR_PATH_TP\TFS\ComptaPLUS\WSComptaPlusWindowsService\bin\Debug\WSComptaPlusWindowsService.exe
//-------------------------------------------------------------
//
// 1.1- To Uninstall the Windows service command s administrator
//-------------------------------------------------------------
// C:\windows\system32>C:\Windows\Microsoft.NET\Framework64\v4.0.30319\installutil.exe  /u D:\YOUR_PATH_TP\TFS\ComptaPLUS\WSComptaPlusWindowsService\bin\Debug\WSComptaPlusWindowsService.exe
//
//-------------------------------------------------------------
// 2- To see the logs  
//-------------------------------------------------------------
// Event Viewer -> Application and Services logs -> WSComptaPlusWindowsServiceLog
//-------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Configuration;
using WSComptaPlusWindowsService.Constants;
using WSComptaPlusWindowsService.Models;
using WSComptaPlusWindowsService.Web;
using TokenHandler.Models;
using WSComptaPlusWindowsService.Utils;

namespace WSComptaPlusWindowsService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001, 
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public int dwServiceType;
        public ServiceState dwCurrentState;
        public int dwControlsAccepted;
        public int dwWin32ExitCode;
        public int dwServiceSpecificExitCode;
        public int dwCheckPoint;
        public int dwWaitHint;
    };
    public partial class WSComptaPlusWindowsService : ServiceBase
    {
        private int eventId = 1;
        [DllImport(WindServLogsConst.SecureLogonProcessDll, SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
        public WSComptaPlusWindowsService() 
        {
            InitializeComponent();
            //We create the Event Viewer log if not existe
            eventLogComptaPlus = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(WindServLogsConst.Source))
            {
                System.Diagnostics.EventLog.CreateEventSource(WindServLogsConst.Source, WindServLogsConst.Log);
            }
            eventLogComptaPlus.Source = WindServLogsConst.Source;
            eventLogComptaPlus.Log = WindServLogsConst.Log;
        }

        protected override void OnStart(string[] args)
        {
            //Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = WindServConfigConst.DwWaitHint;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            //Start log
            eventLogComptaPlus.WriteEntry(WindServLogsConst.ServiceStarted);
            //Set up a timer that triggers every minute.
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = Convert.ToDouble(Config.Instance.PingLoopDelayInMilliSeconds);//WindServConfigConst.TimerInterval;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            //Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        protected override void OnStop()
        {
            //Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = WindServConfigConst.DwWaitHint;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            //Stop log
            eventLogComptaPlus.WriteEntry(WindServLogsConst.ServiceStop);
            //Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            string MessageToTheMailLog = string.Empty;
            LoginResponse loginResponse = new LoginResponse();
            try
            {
                eventLogComptaPlus.WriteEntry(WindServMessagesConst.TraceTimer1, EventLogEntryType.Information, eventId++);
                //catch once from config the urls to ping
                var urlsToPing = Config.Instance.WebServicesToPing;
                eventLogComptaPlus.WriteEntry(string.Concat(WindServMessagesConst.TraceTimer2, urlsToPing.Count), EventLogEntryType.Information, eventId++);
                //call those urls
                foreach (var url in Config.Instance.WebServicesToPing)
                {
                    string message = string.Empty;
                    string subject = string.Empty;
                    try
                    {
                        loginResponse = PingProcess.Ping(url);
                        message = $"{WindServMessagesConst.OnPingUrl} {url} , {WindServMessagesConst.LogIdentif} {loginResponse.IsAuthenticated}, {WindServMessagesConst.LogRespCode} {loginResponse.Code}, {WindServMessagesConst.LogRespMessage} {loginResponse.Message}, {WindServMessagesConst.LogRespMessageMsg} {loginResponse.ResponseMsg}";
                        eventLogComptaPlus.WriteEntry($"{message}", EventLogEntryType.Information, eventId++);
                        if (!loginResponse.Code.Equals(WindServMessagesConst.FalseLoginCode)) {
                            subject = WindServMessagesConst.SubjectError;
                        }
                        else
                        {
                            subject = WindServMessagesConst.Subject;
                        }
                    }
                    catch (Exception ex)
                    {
                        message = $"{WindServMessagesConst.OnPingUrl} {url}, {WindServMessagesConst.Err} {ex.ToString()}";
                        eventLogComptaPlus.WriteEntry($"{message}", EventLogEntryType.Error, eventId++);
                        subject = WindServMessagesConst.SubjectError;
                    }
                    finally
                    {
                        WSMail.SendMail(
                             subject,
                             $"{message}", 
                             Config.Instance.MailAlertDestinataire, Config.Instance.MailSmtpServer, Config.Instance.MailNoReplyAddress);
                    }
                }
            }
            catch (Exception ex)
            {
                eventLogComptaPlus.WriteEntry($"{WindServMessagesConst.ExceptionTrace1}  {ex.ToString()}", EventLogEntryType.Error, eventId++);
                WSMail.SendMail(
                     WindServMessagesConst.SubjectError, 
                     $"{WindServMessagesConst.ExceptionTrace2} { ex.ToString()}",
                     Config.Instance.MailAlertDestinataire, Config.Instance.MailSmtpServer, Config.Instance.MailNoReplyAddress);
            }
        }
        protected override void OnContinue()
        {
            eventLogComptaPlus.WriteEntry($"{WindServMessagesConst.OnContinue}");
        }
    }
}
