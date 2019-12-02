using UnityEditor;
using System.Linq;

public class SortByName
{
	[MenuItem("GameObject/Sorting", false, 0)]
	static void Sorting()
	{
		if (Selection.gameObjects.Length == 0)
		{
			return;
		}
		
		int startIndex = Selection.gameObjects[0].transform.GetSiblingIndex();
		
		var list = Selection.gameObjects.OrderBy(o=>o.name).ToList();
		
		foreach (var o in list)
		{
			o.transform.SetSiblingIndex(startIndex++);
		}
	}
}
