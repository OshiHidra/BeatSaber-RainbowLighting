using System.Reflection;
using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using IllusionInjector;
using System.Linq;
using CustomUI.GameplaySettings;
using CustomUI.Settings;
using System.Collections.Generic;
using CustomUI.Utilities;

namespace RainbowLighting
{
	public class Plugin : IPlugin
	{
        public string Name => "Rainbow Lighting";

        public string Version => "1.5.0";

        public static bool enabled = ModPrefs.GetBool("RainbowLighting", "Enabled", true, true);
        public static LightSwitchEventEffect[] lightArray;
        public static List<string> Spectrums = new List<string>
        {
            { "Full (default)" },
            { "Warm" },
            { "Cool" },
            { "Pastel" },
            { "Dark" }
        };
        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.name == "Menu")
            {
                //Settings Menu Setup
                var subMenuCC = SettingsUI.CreateSubMenu("Rainbow Lighting");
                var disableMenu = subMenuCC.AddBool("Enabled");
                disableMenu.GetValue += delegate { return ModPrefs.GetBool("RainbowLighting", "Enabled", true, true); };
                disableMenu.SetValue += delegate (bool value) { enabled = value; ModPrefs.SetBool("RainbowLighting", "Enabled", value); };
                disableMenu.EnabledText = "ON";
                disableMenu.DisabledText = "OFF";
                var spectrumSelect = subMenuCC.AddList("Spectrum", new float[1] { 0 });
                spectrumSelect.FormatValue += delegate (float value) { return Spectrums[(int)value]; };
                    //TO DO: actual spectrum select

                //PlayerSettings Toggle
                Sprite icon = UIUtilities.LoadSpriteFromResources("RainbowLighting.Resources.icon.png");
                var disableOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.PlayerSettingsRight, "Rainbow Lighting", "MainMenu", "Enable Rainbow Lighting",icon);
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

                Log("Starting Scene Loading Operations" + " Enabled: " + enabled.ToString());
                if (enabled)
                {
                    lightArray = Resources.FindObjectsOfTypeAll<LightSwitchEventEffect>();
                    //var s = ModPrefs.GetInt("RainbowLighting", "Spectrum", 0, false);

                    if (lightArray != null)
                        foreach (LightSwitchEventEffect obj in lightArray)
                        {
                                ReflectionUtil.SetPrivateField(obj, "_lightColor0", randomColor);
                                ReflectionUtil.SetPrivateField(obj, "_lightColor1", randomColor);
                                ReflectionUtil.SetPrivateField(obj, "_highlightColor0", randomColor);
                                ReflectionUtil.SetPrivateField(obj, "_highlightColor1", randomColor);
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
