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
		
		texCoords[0].Set(0.3333f, 0, 0.3333f, 0.3333f);
		texCoords[1].Set(0.6667f, 0, 0.3333f, 0.3333f);
		texCoords[2].Set(0.0f, 0.3333f, 0.3333f, 0.3333f);
		texCoords[3].Set(0.3333f, 0.3333f, 0.3333f, 0.3333f);
		texCoords[4].Set(0.6667f, 0.3333f, 0.3333f, 0.3333f);
		texCoords[5].Set(0.0f, 0.6667f, 0.3333f, 0.3333f);
		texCoords[6].Set(0.3333f, 0.6667f, 0.3333f, 0.3333f);
		texCoords[7].Set(0.6667f, 0.6667f, 0.3333f, 0.3333f);
		
		indices[0] = 0;
		indices[1] = 3;
		indices[2] = 1;
		indices[3] = 3;
		indices[4] = 4;
		indices[5] = 1;
		indices[6] = 1;
		indices[7] = 4;
		indices[8] = 5;
		indices[9] = 1;
		indices[10] = 5;
		indices[11] = 2;
		
		vertices[0].Set(0.47093f, 1);
		vertices[1].Set(0.037790697f, 0.498547f);
		vertices[2].Set(0.037790697f, 0.020348798f);
		vertices[3].Set(0.58906996f, 1);
		vertices[4].Set(0.95930207f, 0.498547f);
		vertices[5].Set(0.95930207f, 0.020348798f);
		
		billboard.SetImageTexCoords(texCoords);
		billboard.SetIndices(indices);
		billboard.SetVertices(vertices);
		
		billboard.width = 19.03616f;
		billboard.height = 19.58068f
		billboard.bottom = -1.814002f;
		
		if (m_outputFile != null)
		{
			EditorUtility.CopySerialized(billboard, m_outputFile);
		}
		else
		{
			string path;
			path = AssetDatabase.GetAssetPath(m_material) + ".asset";
			AssetDatabase.CreateAsset(billboard, path);
		}
	}
}
