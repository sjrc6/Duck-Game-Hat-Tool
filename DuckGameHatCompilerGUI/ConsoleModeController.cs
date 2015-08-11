using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGameHatCompilerGUI
{
    class ConsoleModeController
    {
        private ProgramCore core;
        private bool cliMode;
        private string[] args;

        public ConsoleModeController( ProgramCore core , bool cliMode, string[] programargs )
        {
            this.core = core;
            this.cliMode = cliMode;
            this.args = (string[])programargs.Clone();
        }

        public void Run()
        {
            if ( cliMode )
            {
                HandleCLI();
            }
            else
            {
                bool shouldExit = false;
                while( !shouldExit )
                {
                    shouldExit = HandleConsole();
                }
            }
        }

        //handle the args and execute the passed parameters
        public void HandleCLI()
        {
            //remove -console and -cli from the arguments
        }
        
        //return true to restart the GUI cycle, return false to quit
        public bool HandleConsole()
        {


            //we're done here
            return true;
        }
    }
}
