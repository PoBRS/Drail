using UnityEngine;
using System.Collections;

public class PrefabsContainer : MonoBehaviour {
	public GameObject controlPointPrefab;

	void Start()
	{
		GameElements.prefabContainer = this;
	}
}
