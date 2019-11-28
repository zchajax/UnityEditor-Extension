using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using UnityEditor;

[CreateAssetMenu(fileName = "CustomDrawModeAsset", menuName = "CustomDrawModeAsset", order = 1)]
public class CustomDrawModeAsset : ScriptableObject
{
	[Serializable]
	public struct CustomDrawMode
	{
		public string name;
		public string category;
		public Shader shader;
	}

	public CustomDrawMode[] customDrawModes;
	public Texture _Checker;
	public float _Scale = 1;
	public float _Resolution = 512;
}

public static class CustomDrawModeAssetObject
{
	public static CustomDrawModeAsset cdma;

	public static bool SetUpObject()
	{
		if (cdma == null)
		{
			cdma = (CustomDrawModeAsset)AssetDatabase.LoadAssetAtPath("Assets/DrawMode/CustomDrawModeAsset.asset", typeof(CustomDrawModeAsset));
		}

		if (cdma == null)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
