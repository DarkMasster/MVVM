using System.Collections.Generic;

namespace DM.Example.Data
{
	public class MapData
	{
		#region Public Fields
		public int SizeX;
		public int SizeY;
		public List<MapObjectData> Objects = new(10);
		#endregion
	}
}