using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fier2DToolset {
	[System.Serializable]
	public class TileTransfer {
		public string name;
		[SerializeField]
		public TileActions.Actions action;
		[SerializeField]
		public CustomTile tile;
	}
}