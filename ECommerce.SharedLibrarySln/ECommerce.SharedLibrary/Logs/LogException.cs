namespace ECommerce.Common.Logs
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebugger(ex.Message);
        }

        private static void LogToDebugger(string message)
        {
            throw new NotImplementedException();
        }

        private static void LogToConsole(string message)
        {
            throw new NotImplementedException();
        }

        private static void LogToFile(string message)
        {
            throw new NotImplementedException();
        }
    }
}
