using UnityEngine;

[CreateAssetMenu(fileName = "CrabInfo", menuName = "Scriptable Objects/CrabInfo")]
public class CrabInfo : ScriptableObject
{
	public Sprite sprite;
	public string crabName;

	public enum CrabType
	{
		catfish,
		horseshoe,
		scopeCreep,
		coquette,
		crab,
		shrimp,
		lobster,
		hermit,
		seamonkeys
	}

	public CrabType type;
	//TODO LATER
	// sfx
}
