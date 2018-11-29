using System.Reflection;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RainbowLighting
{
	public class Plugin : IPlugin
	{
        public string Name => "Rainbow Lighting";

        public string Version => "1.0.2";

        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            LightSwitchEventEffect[] array = Resources.FindObjectsOfTypeAll<LightSwitchEventEffect>();
            
            foreach (LightSwitchEventEffect obj in array)
			{
				Plugin.SetPrivateField(obj, "_lightColor0", this.randomColor);
				Plugin.SetPrivateField(obj, "_lightColor1", this.randomColor);
				Plugin.SetPrivateField(obj, "_highlightColor0", this.randomColor);
				Plugin.SetPrivateField(obj, "_highlightColor1", this.randomColor);
			}
		}

        public void OnApplicationStart()
		{
			this.randomColor = ScriptableObject.CreateInstance<ColorRandom>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }


		public void OnApplicationQuit()
		{
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

		public void OnLevelWasLoaded(int level)
		{
		}

		public void OnLevelWasInitialized(int level)
		{
		}

		public void OnUpdate()
		{
		}

		public void OnFixedUpdate()
		{
		}

		public static void SetPrivateField(object obj, string fieldName, object value)
		{
			FieldInfo field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			field.SetValue(obj, value);
		}

		private ColorRandom randomColor;
	}
}
