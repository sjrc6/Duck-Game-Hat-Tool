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
			clients.Add( new SharpRaven.RavenClient( "https://cffc188e57da4c23aedd2d2e9ac45ca3:b7a047ba8dc94dec85ecc0c550a4ff88@sentry.io/298052" )
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

		static void SendExceptionsToRaven( object o , UnhandledExceptionEventArgs e )
		{
			foreach( SharpRaven.RavenClient client in clients )
			{
				client.CaptureException( (Exception)e.ExceptionObject );
			}
		}
	}
}
