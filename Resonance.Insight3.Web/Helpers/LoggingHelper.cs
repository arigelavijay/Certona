//using Resonance.Utilities.Logging;

namespace Resonance.Insight3.Web.Helpers
{
    public class LoggingHelper
    {
        // TODO : Uncomment it to use Resonance.Utilities.Logging 
        //private static ILogger _logger = null;
        //public static ILogger Logger
        //{
        //    get
        //    {
        //        if (_logger == null)
        //            _logger = new Logger();
        //        return _logger;
        //    }
        //}
        /// <summary>
        ///     Creates instance of FileLogger
        /// </summary>
        private static FileLogger _logger;

        public static FileLogger Logger
        {
            get
            {
                if (_logger == null)
                    _logger = new FileLogger();
                return _logger;
            }
        }
    }
}