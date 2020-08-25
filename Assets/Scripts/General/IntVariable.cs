using System;
using UnityEngine;


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