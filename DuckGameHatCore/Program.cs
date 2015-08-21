using System;
using System.Collections.Generic;
using System.Linq;

#if !CONSOLEMODE
using System.Windows.Forms;
#endif

namespace DuckGameHatCompiler
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
#if !CONSOLEMODE
		[STAThread]
#endif

		static void Main( string[] args )
		{
			//unfortunately, this is the most sensible way to handle console mode, by having another project that specifically
			//builds in that way, as it'd be too hacky and annoying to do it the manual way

			ProgramCore core = new ProgramCore();

#if !CONSOLEMODE
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new DGHC_MainForm( core , args ) );
#else
            ConsoleModeController consoleController = new ConsoleModeController(core, args);
            consoleController.Run();
#endif

		}
	}
}
