using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckGameHatCompiler
{
    class ConsoleModeController
    {
        private ProgramCore core;
        private bool cliMode;
        private string[] args;

        public ConsoleModeController( ProgramCore core, string[] programargs )
        {
            this.core = core;
            this.args = (string[])programargs.Clone();
            foreach( string arg in args)
            {
                if( arg.Contains( "-c" ) )
                {
                    cliMode = true;
                    continue;
                }
            }
        }

        public virtual void Run()
        {
            if ( cliMode )
            {
                HandleCLI();
            }
            else
            {
                bool shouldExit = false;
                while( shouldExit )
                {
                    shouldExit = HandleConsole();
                }
            }
        }

        //handle the args and execute the passed parameters
        public void HandleCLI()
        {
            
        }
        
        //return true to restart the GUI cycle, return false to quit
        public bool HandleConsole()
        {


            //we're done here
            return true;
        }
    }
}
