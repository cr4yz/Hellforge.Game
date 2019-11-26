using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SceneSwitcher
{
	public class SceneSwitcherEditor : EditorWindow
	{
		private static List<Scenes> m_Scenes;

		private static Vector2 ScrollPos;
		private static string SearchString = "";

		private static SceneSwitcherEditor Window;
		private static bool m_OpenInSceneView = true;
		private static bool m_CloseInSceneView;
		private static bool m_IsMinimized;
		
		private class Scenes
		{
			public string filePath;
			public string sceneName;
		}
		
		[MenuItem ("Window/SceneSwitcher/Open Dockable Window")]
		private static void Init ()
		{
			Window = EditorWindow.GetWindow<SceneSwitcherEditor>("Scene Switcher");
			Vector2 minSize = new Vector2(212,100);
			Window.minSize = minSize;
			Window.Show();
		}

		[MenuItem("Window/SceneSwitcher/Open SceneView", true)]
		private static bool ValidateOpenSceneView() { return m_OpenInSceneView; }
		[MenuItem("Window/SceneSwitcher/Open SceneView")]
		private static void OpenSceneView()
		{
			m_OpenInSceneView = false;
			m_CloseInSceneView = true;
			SceneView.onSceneGUIDelegate += OnScene;
			SceneView.RepaintAll();
		}

		[MenuItem("Window/SceneSwitcher/Close SceneView", true)]
		private static bool ValidateCloseSceneView() { return m_CloseInSceneView; }
		[MenuItem("Window/SceneSwitcher/Close SceneView")]
		private static void CloseSceneView()
		{
			m_OpenInSceneView = true;
			m_CloseInSceneView = false;
			SceneView.onSceneGUIDelegate -= OnScene;
			SceneView.RepaintAll();
		}
		
		private void OnGUI ()
		{
			m_Scenes = GetScenes();
			GUI.skin.font = ((GUIStyle)"ShurikenLabel").font;
			
			EditorGUILayout.BeginVertical((GUIStyle)"HelpBox");
			EditorGUILayout.LabelField("Scene Switcher", (GUIStyle)"ShurikenEmitterTitle");
			
			Texture2D iconTexture = EditorGUIUtility.FindTexture("UnityEditor.SceneView");
			
			Rect iconRect = GUILayoutUtility.GetLastRect();
			iconRect.x += 6;
			iconRect.y += 6;
			iconRect.width = iconTexture.width;
			iconRect.height = iconTexture.height;
			
			GUI.DrawTexture(iconRect, iconTexture);
			GUILayout.Space(12);
			SceneListLayout();
			EditorGUILayout.EndVertical();
		}

		private static void OnScene(SceneView sceneView)
		{
			Handles.BeginGUI();
			m_Scenes = GetScenes();
			GUI.skin.font = ((GUIStyle)"ShurikenLabel").font;

			if(GUI.Button(new Rect(10,10,212,15), "Scene Switcher", EditorStyles.miniButton))
				m_IsMinimized = !m_IsMinimized;

			if(!m_IsMinimized)
			{
				GUILayout.BeginArea(new Rect(10,10,212,150), "Scene Switcher", GUI.skin.window);
				SceneListLayout();
				GUILayout.EndArea();
			}

			sceneView.Repaint();
			Handles.EndGUI();
		}

		private static void SceneListLayout()
		{
			ScrollPos = GUILayout.BeginScrollView(ScrollPos);
			
			GUILayout.BeginHorizontal();
			GUILayout.Space(2);
			
			SearchString = GUILayout.TextField(SearchString, (GUIStyle)"ToolbarSeachTextField");
			
			GUIStyle searchCancelStyle = (SearchString == "") ? (GUIStyle)"ToolbarSeachCancelButtonEmpty" : (GUIStyle)"ToolbarSeachCancelButton";
			
			if (GUILayout.Button ("", searchCancelStyle))
				SearchString = "";
			
			GUILayout.Space(2);
			GUILayout.EndHorizontal();

			if(m_Scenes.Count == 0)
				EditorGUILayout.LabelField("There are no scenes available.");
			
			GUILayout.Space(5);
			
			foreach(Scenes scene in m_Scenes)
			{
				if(SearchString == "" || StringContains(scene.sceneName, SearchString))
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUIUtility.labelWidth = 50;

					string currentScene = "";
					if(EditorApplication.currentScene.Contains(scene.sceneName))
						currentScene = "[LOADED] ";

					EditorGUILayout.LabelField(new GUIContent(currentScene + scene.sceneName, scene.sceneName));

					if(GUILayout.Button("Load", GUILayout.Width(40)))
						OpenScene(scene.filePath);
					if(GUILayout.Button("x", GUILayout.Width(18)))
						DeleteScene(scene.filePath);

					EditorGUILayout.EndHorizontal();
				}
			}

			GUILayout.EndScrollView();
		}
		
		private static List<Scenes> GetScenes()
		{
			List<Scenes> tempScenes = new List<Scenes>();
			DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
			FileInfo[] fileInfo = directory.GetFiles("*.unity", SearchOption.AllDirectories);
			foreach(FileInfo file in fileInfo)
			{
				Scenes newScene = new Scenes()
				{
					sceneName = file.Name.Replace(".unity",""),
					filePath = file.FullName.Replace(@"\", "/").Replace(Application.dataPath, "Assets")
				};
				tempScenes.Add(newScene);
			}
			
			return tempScenes.OrderBy(list => list.sceneName).ToList();
		}
		
		private static void OpenScene(string filePath)
		{
			if(EditorApplication.SaveCurrentSceneIfUserWantsTo())
				EditorApplication.OpenScene(filePath);
		}

		private static void DeleteScene(string filePath)
		{
			if(EditorUtility.DisplayDialog("Delete Scene?", "Are you sure you want to delete this scene?\n\nFile: " + filePath, "YES", "NO"))
				File.Delete(filePath);
		}
		
		private static bool StringContains(string _source, string _compareTo)
		{
			return _source.IndexOf(_compareTo, StringComparison.OrdinalIgnoreCase) >= 0;
		}
	}
}