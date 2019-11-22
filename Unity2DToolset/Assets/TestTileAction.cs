using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Fier2DToolset;

public class TestTileAction : MonoBehaviour
{
	public Tilemap map;
	public float delay = 2;
	private bool done = false;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        if((delay < 0) && !done) {
			(map.GetTile(new Vector3Int(-3, -3, 0)) as CustomTile).DoAction(TileActions.Actions.Dig, map, new Vector3Int(-3, -3, 0));
			done = true;
		} else {
			delay -= Time.deltaTime;
		}
    }
}
