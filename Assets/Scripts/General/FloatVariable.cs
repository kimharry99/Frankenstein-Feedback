using System;
using UnityEngine;


public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
	public float initialValue;
	[NonSerialized]
	public float value;

	public void OnAfterDeserialize()
	{
		value = initialValue;
	}

	public void OnBeforeSerialize() { }
}