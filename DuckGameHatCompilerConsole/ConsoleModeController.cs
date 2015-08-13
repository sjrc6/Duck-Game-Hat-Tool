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
        private List<string> args;
        private List<string> processFiles;

        public ConsoleModeController( ProgramCore core, string[] programargs )
        {
            this.core = core;
            this.args = new List<string>(programargs);
            this.processFiles = new List<String>();

            
            foreach( string arg in this.args )
            {
                //this is a file, add it to the list of files to process, and enable cli mode
                if (!arg.StartsWith("-") && System.IO.File.Exists( arg ) && core.CanReadFile( arg ) )
                {
                    Console.WriteLine(arg);
                    this.processFiles.Add(arg);
                    cliMode = true;
                    continue;
                }

                //check to see if the user wants to enable cli mode
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
                while( !shouldExit )
                {
                    shouldExit = HandleConsole();
                }
            }
        }

        //handle the args and execute the passed parameters
        public void HandleCLI()
        {
            //special case, the user dropped files onto the exe, force to process them and then leave
            if ( processFiles.Count > 0 )
            {
                foreach( var filename in this.processFiles )
                {
                    CLISaveFileAsHat(filename);
                }

                return;
            }

            foreach (string arg in this.args)
            {
                if (arg.Contains("-h"))
                {
                    PrintCLICommands();
                }
            }

            
        }

        private void CLISaveFileAsHat( string filename )
        {
            core.CloseCurrent(true);
            if (core.OpenFile(filename))
            {
                string path = System.IO.Path.GetDirectoryName( filename );
                core.SaveHat( System.IO.Path.Combine( path , core.FileInfo.HatName.ToLower() + ".hat" ) );
            }
        }

        private void PrintCLICommands()
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
