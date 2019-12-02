using UnityEditor;
using System.Linq;

public class SortByName
{
	[MenuItem("GameObject/Sorting", flase, 0)]
	static void Sorting()
	{
		if (Selection.gameObject.Length == 0)
		{
			return;
		}
		
		int startIndex = Selection.gameObject[0].transform.GetSiblingIndex();
		
		var list = Selection.gameObjects.OrderBy(o=>o.name).ToList();
		
		foreach (var o in list)
		{
			o.transform.SetSiblingIndex(startIndex++);
		}
	}
}
