using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BillboardBaker : MonoBehaviour
{
	public BillboardAsset m_outputFile;
	public Material m_material;
	
	[ContextMenu("Bake Billboard")]
	void BakeBillboard()
	{
		BillboardAsset billboard = new BillboardAsset();
		
		billboard.material = m_material;
		Vector4 texCoords = new Vector4[8];
		
		ushort[] indices = new ushort[12];
		Vector2[] vertices = new Vector2[6];
	}
}
