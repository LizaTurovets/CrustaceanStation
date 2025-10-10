using UnityEngine;

[CreateAssetMenu(fileName = "Train", menuName = "Scriptable Objects/Train")]
public class Train : ScriptableObject
{
	public int arrivalTimeHour; // arrival time
	public int departureTimeHour; //departure time

	// name
	public string trainName;
	public int trainID;

	//capacity
	public int rows;
	public int columns;
}
