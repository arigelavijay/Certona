using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Resonance.Insight3.Web.Helpers
{
    public class FileLogger
    {
        /// <summary>
        ///     Writes an entry to the log using default parameters.
        /// </summary>
        /// <param name="entry">What to write to the log file.</param>
        public void WriteEntry(string entry)
        {
            try
            {
                WriteEntry("Insight", entry);
            }
            catch
            {
            } // If fails just continue(Don't stop processing)
        }

        /// <summary>
        ///     Writes an entry to the log using default parameters.
        /// </summary>
        /// <param name="ex">Exception thrown by application</param>
        public void WriteException(Exception ex)
        {
            try
            {
                WriteEntry("Insight", ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
            } // If fails just continue(Don't stop processing)
        }

        /// <summary>
        ///     Writes an entry to the log file.
        /// </summary>
        /// <param name="filename">The full path and file name of the log file.</param>
        /// <param name="entry">What to write to the log file.</param>
        public void WriteEntry(string filenamePrefix, string entry)
        {
            string sFileName = string.Empty;
            try
            {
                sFileName = Path.Combine(GetLogPath(),
                                         string.Format(@filenamePrefix + "_{0}.Log", DateTime.Today.ToString("MMddyyyy")));
                LogEntry(sFileName, entry);
            }
            catch
            {
            }
            finally
            {
            }
        }

        /// <summary>
        ///     Get log path
        /// </summary>
        /// <returns></returns>
        public string GetLogPath()
        {
            return GetLogPath("bin", "Logs");
        }

        /// <summary>
        ///     Constructs a path to the log file, excluding the log filename.
        /// </summary>
        /// <param name="siblingFolder">The name of a sibling folder to logFolder (typically the assembly's \bin directory).</param>
        /// <param name="logFolder">The name of the log folder. This folder will be a sibling of what is supplied in siblingFolder.</param>
        /// <returns></returns>
        public string GetLogPath(string siblingFolder, string logFolder)
        {
            string thePath = string.Empty;

            try
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                thePath = Path.GetDirectoryName(ass.CodeBase);
                if (thePath.LastIndexOf(siblingFolder) > -1)
                    thePath = thePath.Substring(0, thePath.IndexOf(siblingFolder));
                if (thePath.IndexOf(@"file:\") == 0)
                    thePath = thePath.Substring((@"file:\").Length);
                thePath = thePath.Replace(@"file:///", "");
                thePath = Path.Combine(thePath, logFolder);
            }
            catch
            {
            }
            finally
            {
            }

            return thePath;
        }

        /// <summary>
        ///     Writes an entry to the log file.
        /// </summary>
        /// <param name="filename">The full path and file name of the log file.</param>
        /// <param name="entry">What to write to the log file.</param>
        private void LogEntry(string filename, string entry)
        {
            try
            {
                entry += Environment.NewLine;
                entry += "------------------------------------------------------------------------";
                entry += Environment.NewLine;
                try
                {
                    string sPath = Path.GetDirectoryName(filename);
                    if (!Directory.Exists(sPath))
                    {
                        Directory.CreateDirectory(sPath);
                    }
                    try
                    {
                        entry = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + " : " + entry;
                        using (var fs = new FileStream(filename, FileMode.Append, FileAccess.Write))
                        {
                            try
                            {
                                fs.Write(Encoding.ASCII.GetBytes(entry), 0, entry.Length);
                                fs.Flush();
                            }
                            catch
                            {
                            }
                            finally
                            {
                                if (null != fs)
                                    try
                                    {
                                        fs.Close();
                                    }
                                    catch
                                    {
                                    }
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
            finally
            {
            }
        }
    }
}