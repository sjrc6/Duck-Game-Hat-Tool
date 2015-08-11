using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DuckGameHatCompilerGUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool cliMode = false;
            bool consoleMode = false;
            
            //unfortunately, this is the most sensible way to handle console mode, by having another project that specifically
            //builds in that way, as it'd be too hacky and annoying to do it the manual way

            #if CONSOLEMODE
                consoleMode = true;
            #endif

            #if CLIMODE
                consoleMode = true;
                cliMode = true;         
            #endif

            ProgramCore core = new ProgramCore();

            if (!consoleMode)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new DGHC_MainForm( core ));
            }
            else
            {
                ConsoleModeController consoleController = new ConsoleModeController( core , cliMode , args );
                consoleController.Run();
            }

        }
    }
}
