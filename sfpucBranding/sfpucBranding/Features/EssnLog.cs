using System;
using Microsoft.SharePoint.Administration;

namespace sfpucBranding.Features
{
    class EssnLog
    {
        public static string NameProj = " Root Site#_";
        public static void logInfo(string msg)
        {
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
            diagSvc.WriteTrace(0, // custom trace id
                new SPDiagnosticsCategory(NameProj,
                    TraceSeverity.Monitorable,
                    EventSeverity.Information), // create a category
                TraceSeverity.Monitorable, // set the logging level of this record
                "info: {0}", // custom message
                new object[] { msg } // parameters to message
                );

        }
        public static void logExc(Exception ex)
        {
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
            diagSvc.WriteTrace(0, // custom trace id
                new SPDiagnosticsCategory(NameProj,
                    TraceSeverity.Monitorable,
                    EventSeverity.Error), // create a category
                TraceSeverity.Monitorable, // set the logging level of this record
                "error: {0}", // custom message
                new object[] { ex.Message + ex.StackTrace } // parameters to message
                );

        }
        public static void logVersion()
        {
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
            diagSvc.WriteTrace(0, // custom trace id
                new SPDiagnosticsCategory(NameProj,
                    TraceSeverity.Monitorable,
                    EventSeverity.Error), // create a category
                TraceSeverity.Monitorable, // set the logging level of this record
                "Version: {0}", // custom message
                new object[] { "essn.RootSite 1.0.0" } // parameters to message
                );
        }

        public static void logVerbose(string msg)
        {
            SPDiagnosticsService diagSvc = SPDiagnosticsService.Local;
            diagSvc.WriteTrace(0, // custom trace id
                new SPDiagnosticsCategory(NameProj,
                    TraceSeverity.Monitorable,
                    EventSeverity.Error), // create a category
                TraceSeverity.Monitorable, // set the logging level of this record
                "Log: {0}", // custom message
                new object[] { msg } // parameters to message
                );

        }
    }
}