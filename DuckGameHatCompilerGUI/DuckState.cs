using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DuckGameHatCompiler
{
	public enum DuckStateEnum
	{
		IDLEFULL = 0 ,
		IDLEHALF ,
		QUACKHALF
	}

	public class DuckState
	{
		public bool hashHat = false;
		public Color color = Color.White;
		public DuckStateEnum state = DuckStateEnum.IDLEFULL;
		public Image image = null;

		public bool IsIdeal( bool hat , Color c )
		{
			return hashHat == hat && color == c && image != null;
		}

		public void Clean()
		{
			if( image != null )
			{
				image.Dispose();
				image = null;
			}
		}
	}

	public class DuckStateManager
	{


		private Dictionary<DuckStateEnum , DuckState> states = new Dictionary<DuckStateEnum , DuckState>();

		public static DuckStateEnum GetEnumByBools( bool separate , bool quack )
		{
			if( !separate )
			{
				return DuckStateEnum.IDLEFULL;
			}
			else
			{
				if( quack )
					return DuckStateEnum.QUACKHALF;
				else
					return DuckStateEnum.IDLEHALF;
			}
			
		}

		public void AddImage( Image img , Color c , bool hashat , DuckStateEnum stateenum )
		{
			DuckState state = new DuckState();
			state.image = img;
			state.color = c;
			state.hashHat = hashat;
			state.state = stateenum;

			states.Add( stateenum , state );
		}

		public Image GetImage( bool withhat , Color currentDuckColor , bool separate , bool quack )
		{
			DuckStateEnum key = DuckStateManager.GetEnumByBools( separate , quack );
			
			DuckState state = states.FirstOrDefault( x => x.Key == key ).Value;

			if( state != null )
			{
				if( state.IsIdeal( withhat , currentDuckColor ) )
					return state.image;
				else
					states.Remove( key );
			}

			return null;
		}

		public void Cleanup( )
		{
			if( states.Values.Count < 3 )
				return;

			foreach( KeyValuePair<DuckStateEnum , DuckState> it in states )
			{
				DuckState state = it.Value;
				state.Clean();
			}
			
			states.Clear();
		}
	}
}
