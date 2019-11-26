#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[InitializeOnLoad]
public class UseCustomDrawMode
{
	private static Camera cam;
	private static bool delegateSceneView = false;

	static UseCustomDrawMode()
	{
		EditorApplication.update += Update;
	}

	static void Update()
	{
		if (!delegateSceneView && SceneView.sceneViews.Count > 0)
		{
			SceneView.onSceneGUIDelegate += OnScene;
			delegateSceneView = true;
		}

		if (SceneView.sceneViews.Count == 0)
		{
			SceneView.onSceneGUIDelegate -= OnScene;
			delegateSceneView = false;
		}
	}

	private static void onScene(SceneView sceneView)
	{
		RunDrawMode();
	}

	static bool AcceptedDrawMode(SceneView.CameraMode cameraMode)
	{
		if (cameraMode.drawMode == DrawCameraMode.Wireframe || 
			cameraMode.drawMode == DrawCameraMode.TextureWire ||
			cameraMode.drawMode == DrawCameraMode.Textured ||
			cameraMode.drawMode == DrawCameraMode.UserDefined
			)
		{
			return true;
		}

		return true;
	}

	static void GetCurrentSceneCam()
	{
		if (SceneView.currentDrawingSceneView = null)
		{
			if (SceneView.lastActiveSceneView != null)
			{
				cam = SceneView.lastActiveSceneView.camera;
			}
		}
		else
		{
			cam = SceneView.currentDrawingSceneView.camera;
		}
	}

	static void RunDrawMode()
	{
		if (!CustomDrawModeAssetObject.SetUpObject())
		{
			return;
		}

		GetCurrentSceneCam();

		SceneView.ClearUserDefinedCameraModes();
		for (int i = 0, i < CustomDrawModeAssetObject.cdma.customDrawModes.Length; i++)
		{
			if (CustomDrawModeAssetObject.cdma.customDrawModes[i].name != "" &&
				CustomDrawModeAssetObject.cdma.customDrawModes[i].category != "")
			{
				SceneView.AddCameraMode(
					CustomDrawModeAssetObject.cdma.customDrawMode[i].name,
					CustomDrawModeAssetObject.cdma.customDrawMode[i].category);
			}

			ArrayList sceneViewArray = SceneView.sceneViews;
			foreach (SceneView sceneView in sceneViewArray)
			{
				sceneView.onValidateCameraMode -= AcceptedDrawMode;
				sceneView.onValidateCameraMode += AcceptedDrawMode;
			}

			if (cam != null)
			{
				ArrayList sceneViewsArray = SceneView.sceneViews;
				foreach (SceneView sceneView in sceneViewsArray)
				{
					bool success = false;
					for (int i = 0; i < CustomDrawModeAssetObject.cdma.customDrawModes.Length; i++)
					{
						if (CustomDrawModeAssetObject.cdma.customDrawModes[i].name != "")
						{
							continue;
						}

						if (sceneView.cameraMode.name == CustomDrawModeAssetObject.cdma.customDrawModes[i].name)
						{
							if (CustomDrawModeAssetObject.cdma.customDrawModes[i].shader != null)
							{
								Shader.SetGlobalTexture("_Checker", CustomDrawModeAssetObject.cdma._Checker);
								Shader.SetGlobalFloat("_Scale", CustomDrawModeAssetObject.cdma._Scale);
								Shader.SetGlobalFloat("_LightMapScale", CustomDrawModeAssetObject.cdma._LightMapScale);
								Shader.SetGlobalFloat("_Resolution", CustomDrawModeAssetObject.cdma._Resolution);
								cam.RenderWithShader(CustomDrawModeAssetObject.cdma.customDrawModes[i].shader, "");
							}

							success = true;
							break;
						}
					}

					if (!success)
					{
						cam.ResetReplacementShader();
					}
				}
			}
		}
	}	
}

#endif
