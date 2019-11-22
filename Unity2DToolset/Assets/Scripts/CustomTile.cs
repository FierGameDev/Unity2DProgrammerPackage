using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System;
using static UnityEngine.Tilemaps.Tile;

namespace Fier2DToolset {

	[CreateAssetMenu(fileName = "CustomTile", menuName = "Fier2DToolset/CreateCustomTile")]
	public class CustomTile : TileBase {

		// Start is called before the first frame update

		public Sprite[] sprites;
		public string tileName;
		public ColliderType collider;
		public TileActions.Checks tileUpdates;
		public List<TileTransfer> tileTransfers;

		public delegate void TileActionEventHandler(TileActions.Actions action, Vector3Int location, Tilemap tilemap);
		public static event TileActionEventHandler tileActionEvent;

		public void DoAction(TileActions.Actions action, Tilemap tilemap, Vector3Int location) {
			tilemap.SetTile(location, tileTransfers.Find(ByAction(action)).tile);
			if (tileActionEvent != null) {
				tileActionEvent(action, location, tilemap);
			}
		}

		public override void RefreshTile(Vector3Int position, ITilemap tilemap) {
			base.RefreshTile(position, tilemap);

			if(tileUpdates == TileActions.Checks.Cardinal) {
				tilemap.RefreshTile(position + Vector3Int.right);
				tilemap.RefreshTile(position + Vector3Int.left);
				tilemap.RefreshTile(position + Vector3Int.up);
				tilemap.RefreshTile(position + Vector3Int.down);
			}
		}

		public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData) {
			int sprite = 0;
			int rotationMask = 0;

			//left  1000 8
			//right 0100 4
			//up    0010 2
			//down  0001 1

			if (tileUpdates == TileActions.Checks.Cardinal) {
				rotationMask |= HasTile(location + Vector3Int.right, tilemap) ? 8 : 0;
				rotationMask |= HasTile(location + Vector3Int.left, tilemap) ? 4 : 0;
				rotationMask |= HasTile(location + Vector3Int.up, tilemap) ? 2 : 0;
				rotationMask |= HasTile(location + Vector3Int.down, tilemap) ? 1 : 0;
			}

			switch(rotationMask) {
				case 0: sprite = 0; break;
				case 1: sprite = 1; break; 
				case 2: sprite = 1; break;
				case 3: sprite = 3; break;
				case 4: sprite = 1; break;
				case 5: sprite = 2; break;
				case 6: sprite = 2; break;
				case 7: sprite = 4; break;
				case 8: sprite = 1; break; 
				case 9: sprite = 2; break;
				case 10: sprite = 2; break;
				case 11: sprite = 4; break;
				case 12: sprite = 3; break;
				case 13: sprite = 4; break;
				case 14: sprite = 4; break;
				case 15: sprite = 5; break; 
			}

			tileData.sprite = sprites[sprite];

			var m = tileData.transform;
			m.SetTRS(Vector3.zero, GetRotation(rotationMask), Vector3.one);
			tileData.transform = m;

			tileData.flags = TileFlags.LockTransform; //Somehow this is absolutely necessary
			tileData.colliderType = collider;
		}

		private bool HasTile(Vector3Int location, ITilemap tilemap) {
			if(tilemap.GetTile(location) != null) {
				return ((CustomTile)tilemap.GetTile(location)).IsTile(tileName);
			}
			return false;
		}

		public bool IsTile(string tileName) {
			return tileName.Equals(this.tileName);
		}

		private Quaternion GetRotation(int mask) {

			switch(mask) {
				case 2: return Quaternion.Euler(0f, 0f, 180f);
				case 3: return Quaternion.Euler(0f, 0f, 90f);
				case 4: return Quaternion.Euler(0f, 0f, +90f);
				case 5: return Quaternion.Euler(0f, 0f, 180f);
				case 6: return Quaternion.Euler(0f, 0f, 90f);
				case 7: return Quaternion.Euler(0f, 0f, 90f);
				case 8: return Quaternion.Euler(0f, 0f, 90f);
				case 9: return Quaternion.Euler(0f, 0f, -90f);
				case 11: return Quaternion.Euler(0f, 0f, -90f);
				case 13: return Quaternion.Euler(0f, 0f, 180f);
				default: return Quaternion.Euler(0f, 0f, 0f);
			}
		}

		static Predicate<TileTransfer> ByAction(TileActions.Actions action) {
			return delegate (TileTransfer transfer) {
				return transfer.action == action;
			};
		}
	}
}