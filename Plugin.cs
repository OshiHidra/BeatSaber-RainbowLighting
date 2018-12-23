using System.Reflection;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionInjector;
using System.Linq;
using CustomUI.GameplaySettings;

namespace RainbowLighting
{
	public class Plugin : IPlugin
	{
        public string Name => "Rainbow Lighting";

        public string Version => "1.0.2";

        public static bool enabled = true;
        public static LightSwitchEventEffect[] lightArray;
        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.name == "Menu")
            {
                var disableOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.PlayerSettingsRight, "Rainbow Lighting", "MainMenu", "Enable Rainbow Lighting");
                disableOption.GetValue = ModPrefs.GetBool("RainbowLighting", "Enabled", true, true);
                disableOption.OnToggle += (value) => { enabled = value; ModPrefs.SetBool("RainbowLighting", "Enabled", value); };
            }

		}

        public void OnApplicationStart()
		{

            randomColor = ScriptableObject.CreateInstance<ColorRandom>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene scene)
        {
           if(scene.name == "GameCore")
            {
                Log("Starting Scene Loading Operations" + " Enabled: " + enabled.ToString());
                if(lightArray == null)
                lightArray = Resources.FindObjectsOfTypeAll<LightSwitchEventEffect>();
                if (lightArray != null)
                    foreach (LightSwitchEventEffect obj in lightArray)
                    {
                        if (enabled)
                        {
                            Log(ReflectionUtil.GetPrivateField<ColorSO>(obj, "_lightColor0").color.ToString());
                            Log(ReflectionUtil.GetPrivateField<ColorSO>(obj, "_lightColor1").color.ToString());
                            Log(ReflectionUtil.GetPrivateField<ColorSO>(obj, "_highlightColor0").color.ToString());
                            Log(ReflectionUtil.GetPrivateField<ColorSO>(obj, "_highlightColor1").color.ToString());
                            ReflectionUtil.SetPrivateField(obj, "_lightColor0", randomColor);
                            ReflectionUtil.SetPrivateField(obj, "_lightColor1", randomColor);
                            ReflectionUtil.SetPrivateField(obj, "_highlightColor0", randomColor);
                            ReflectionUtil.SetPrivateField(obj, "_highlightColor1", randomColor);
                        }


                    }
                else
                    Log("Null Light Array");
            }
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



        public static void Log(string message)
        {
            System.Console.WriteLine("[RainbowLighting] " +  message);
        }
        private ColorRandom randomColor;
	}
}
