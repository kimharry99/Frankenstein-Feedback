using System;
using UnityEngine;


[CreateAssetMenu]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{
	public int initialValue;
	[NonSerialized]
	public int value;

	public void OnAfterDeserialize()
	{
		value = initialValue;
	}

	public void OnBeforeSerialize() { }
}