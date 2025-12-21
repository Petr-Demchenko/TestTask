<%@ Application Language="C#" %>

<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        // Code that runs on application startup
    }

    void Application_End(object sender, EventArgs e)
    {
        // Code that runs on application shutdown
    }

    void Application_Error(object sender, EventArgs e)
    {
        // Code that runs when an unhandled error occurs
        Exception ex = Server.GetLastError();
        if (ex != null)
        {
            try
            {
                System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog("Application");
                eventLog.Source = "MedTechWeb";
                eventLog.WriteEntry($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Application error: {ex.Message}\n{ex.StackTrace}", 
                    System.Diagnostics.EventLogEntryType.Error);
            }
            catch
            {
                // Silently fail if logging fails
            }
        }
    }

</script>
