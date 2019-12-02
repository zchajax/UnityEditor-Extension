using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class CaptureScene : MonoBehaviour
{
	private Camera _captureCam;
	
	public Rect rect = new Rect(0, 0, 4096, 4096);
	
	public string Path;
	
	[ContextMenu("Capture")]
	public void Capture()
	{
		if (_captureCam == null)
		{
			GameObject go = new GameObject("capture scene cam");
			go.hideFlags = HideFlags.HideAndDontSave;
			_captureCam = go.AddComponent<Camera>();
		}
		
		_captureCam.CopyFrom(Camera.main);
		_captureCam.enabled = true;
		RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
		_captureCam.targetTexture = rt;
		_captureCam.Render();
		
		RenderTexture.active = rt;
		Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		screenShot.ReadPixels(rect, 0, 0);
		screenShot.Apply();
		
		byte[] bytes = screenShot.EncodeToPNG();
		System.IO.File.WriteAllBytes(Path, bytes);

        _captureCam.enabled = false;
        RenderTexture.active = null;

        SafeDestory(rt);
		SafeDestory(screenShot);
	}
	
	void SafeDestory(Object obj)
	{
		if (Application.isEditor)
		{
			DestroyImmediate(obj);
		}
		else
		{
			Destroy(obj);
		}
	}
}
