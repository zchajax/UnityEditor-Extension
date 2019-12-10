using UnityEngine;
using System.Collections;

public class TreeToBillBoard : MonoBehaviour
{
	public GameObject objectToRender;
	public int imageWidth = 1024;
	public int imageHeight = 1024;
	public Material m_BlurMat;
	private Camera _captureCam;
	
	[ContextMenu("Capture")]
	void ConvertToImage()
	{
		if (_captureGam == null)
		{
			GameObject go = new GameObject("captureCam");
			go.hideFlags = HideFlags.HideAndDontSave;
			_captureCam = go.AddComponent<Camera>();
		}
		
		_captureCam.CopyFrom(Camera.main);
		
		RenderTexture[] rts = new RenderTexture[2]
		{
			new RenderTexture((int)imageWidth, (int)imageHeight, 0),
			new RenderTexture((int)imageWidth, (int)imageHeight, 0)
		};
		
		RenderBuffer[] rbs = new RenderBuffer[2];
		rbs[0] = rts[0].colorBuffer;
		rbs[1] = rts[1].colorBuffer;
		
		_captureCam.SetTargetBuffers(rbs, rts[0].depthBuffer);
		_captureCam.orthographic = true;
		_captureCam.clearFlags = CameraClearFlags.Nothing;
		_captureCam.enabled = true;
		
		Bounds bb = objectToRender.GetComponent<Renderer>().bounds;
		
		_captureCam.transform.position = new Vector3(bb.center.x + 12, bb.center.y, bb.center.z);
		_captureCam.nearClipPlane = 0.5f;
		_captureCam.farClipPlane = 25;
		_captureCam.orthographicSize = 1.0f * Mathf.Max((bb.max.y - bb.min.y) / 2.0f, (bb.max.x - bb.min.y) / 2.0f);
		_captureCam.transform.LookAt(new Vector3(objectToRender.transform.position.x, bb.center.y, objectToRender.transform.position.z));
		
		RenderTexture.active = rts[1];
		GL.Clear(false, true, new Color(0.5f, 0.5f, 1, 0));
		
		RenderTexture.active = rts[1];
		GL.Clear(false, true, new Color(0.5f, 0.5f, 1, 0));
		
		// Render to Atlas
		renderToTextures();
		
		// Blur
		RenderTexture desRenderTexture;
		Blur(rts[0], out desRenderTexture);
		
		var tex = new Texture2D(imageWidth, imageHeight, TextureFormat.ARGB32, false);
		
		//Read pixels
		RenderTexture.active = desRenderTexture;
		tex.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
		tex.Apply();
		
		// Encode texture into PNG
		byte[] bytes = tex.EncodeToPNG();
		System.IO.File.WriteAllBytes("D:/billboardColor.png", bytes);
		
		// Blur
		Blur(rts[1], out desRenderTexture);
		RenderTexture.active = desRenderTexture;
		tex.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
		tex.Apply();
		
		// Encode texture into PNG
		bytes = tex.EncodeToPNG();
		System.IO.File.WriteAllBytes("D:/billboardNormal.png", bytes);
		
		RenderTexture.active = null;
		
		_captureCam.enabled = false;
		
		SafeDestroy(tex);
		SafeDestroy(rts);
	}
	
	void SafeDestroy(Object obj)
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
	
	void SafeDestoryArray(Object[] objs)
	{
		for (int i = 0; i < objs.Length; i++)
		{
			SafeDestroy(objs[i]);
		}
	}
	
	private void renderToTextures()
	{
		// bottom
		_captureCam.rect = new Rect(0.3333f, 0, 0.3333f, 0.3333f);
		_captureCam.Render();
		
		_captureCam.transform.RotateAround(objectToRender.transform.position, Vector3.up, -45);
		_captureCam.rect = new Rect(0.6667, 0, 0.3333f, 0.3333f);
		_captureCam.Render();
		
		// middle
		_captureCam.transform.RotateAround(objectToRender.transform.position, Vector3.up, -45);
		_captureCam.rect = new Rect(0, 0.3333f, 0.3333f, 0.3333f);
		_captureCam.Render();
		
		_captureCam.transform.RotateAround(objectToRender.transform.position, Vector3.up, -45);
		_captureCam.rect = new Rect(0.3333f, 0.3333f, 0.3333f, 0.3333f);
		_captureCam.Render();
		
		_captureCam.transform.RotateAround(objectToRender.transform.position, Vector3.up, -45);
		_captureCam.rect = new Rect(0.6667, 0.3333f, 0.3333f, 0.3333f);
		_captureCam.Render();
		
		// top	
		_captureCam.transform.RotateAround(objectToRender.transform.position, Vector3.up, -45);
		_captureCam.rect = new Rect(0, 0.6667, 0.3333f, 0.3333f);
		_captureCam.Render();
		
		_captureCam.transform.RotateAround(objectToRender.transform.position, Vector3.up, -45);
		_captureCam.rect = new Rect(0.3333f, 0.6667, 0.3333f, 0.3333f);
		_captureCam.Render();
		
		_captureCam.transform.RotateAround(objectToRender.transform.position, Vector3.up, -45);
		_captureCam.rect = new Rect(0.6667, 0.6667, 0.3333f, 0.3333f);
		_captureCam.Render();
	}
	
	private void Blur (RenderTexture source, out RenderTexture destination)
	{
		destination = new RenderTexture((int)imageWidth, (int)imageHeight, 0);
		
		var temp = RenderTexture.GetTemporary((int)source.width, (int)imageHeight, 0);
		Graphics.Blit(source, temp, m_BlurMat, 0);
		Graphics.Blit(temp, destination, m_BlurMat, 1);
		RenderTexture.ReleaseTemporary(temp);
	}
}
