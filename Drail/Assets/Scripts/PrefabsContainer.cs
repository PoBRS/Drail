using UnityEngine;
using System.Collections;

public class PrefabsContainer : MonoBehaviour {
	public GameObject StraightRailPrefab;
	public GameObject Left90RailPrefab;
	public GameObject Right90RailPrefab;

	void Awake()
	{
		GameElements.prefabContainer = this;
	}
}
