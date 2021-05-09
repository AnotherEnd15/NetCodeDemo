using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace ET
{
	public class Init : MonoBehaviour
	{
		private void Start()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
				
				DontDestroyOnLoad(gameObject);

				string[] assemblyNames = { "Unity.Model.dll", "Unity.Hotfix.dll", "Unity.ModelView.dll", "Unity.HotfixView.dll" };
				
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					string assemblyName = assembly.ManifestModule.Name;
					if (!assemblyNames.Contains(assemblyName))
					{
						continue;
					}
					Game.EventSystem.Add(assembly);	
				}
				
				ProtobufHelper.Init();
				
				Game.Options = new Options();
				
				Game.EventSystem.Publish(new EventIDType.AppStart());
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			ThreadSynchronizationContext.Instance.Update();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}

		private void OnGUI()
		{
			if (ZoneSceneManagerComponent.Instance == null) return;
			if (!ZoneSceneManagerComponent.Instance.ZoneScenes.TryGetValue(1, out var gameScene))
			{
				return;
			}

			if (gameScene.GetComponent<SessionComponent>() == null)
				return;
			var com = gameScene.GetComponent<SessionComponent>().Session.GetComponent<PingComponent>();
			if (com == null)
				return;
			var ping = com.RTT;
			GUI.color = Color.green;
			GUILayout.Label("Ping: \n\t"+ ping.ToString(),GUILayout.Width(200),GUILayout.Height(100));
		}
	}
}