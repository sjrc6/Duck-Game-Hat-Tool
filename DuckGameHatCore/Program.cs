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
		static List<SharpRaven.RavenClient> clients = new List<SharpRaven.RavenClient>();
		static ProgramCore core;

#if !CONSOLEMODE
		[STAThread]
#endif
		static void Main( string[] args )
		{
			//unfortunately, this is the most sensible way to handle console mode, by having another project that specifically
			//builds in that way, as it'd be too hacky and annoying to do it the manual way

			string loggertag = "root";

#if !CONSOLEMODE
			loggertag = "GUI";
#else
			loggertag = "CLI";
#endif

			core = new ProgramCore();
			clients.Add( new SharpRaven.RavenClient( "https://3bacc47080a94e9a87fa1ba8816b3d11:5ff9c1f78b7042b2b788057a888ff382@app.getsentry.com/51451" )
			{
				Logger = loggertag
			} );


			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler( SendExceptionsToRaven );

#if !CONSOLEMODE
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new DGHC_MainForm( core , args ) );
#else
            ConsoleModeController consoleController = new ConsoleModeController( core , args );
            consoleController.Run();
#endif
			
		}

		static void SendExceptionsToRaven( object o , UnhandledExceptionEventArgs e  )
		{
			foreach( SharpRaven.RavenClient client in clients )
			{
				client.CaptureException( (Exception)e.ExceptionObject );
			}
		}
	}
}
