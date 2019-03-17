using IllusionPlugin;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using CustomUI.GameplaySettings;
using CustomUI.Settings;
using CustomUI.Utilities;

namespace RainbowLighting
{
    public class Plugin : IPlugin
	{
        public string Name => "Rainbow Lighting";

        public string Version => "1.5.1";

        public static bool enabled = ModPrefs.GetBool("RainbowLighting", "Enabled", true, true);
        public static LightSwitchEventEffect[] lightArray;
        //public static string full = "Full (default)";
        //public static string warm = "Warm";
        //public static string cool = "Cool";
        //public static string pastel = "Pastel";
        //public static string dark = "Dark";

        //I'll add support for all of this eventually -Auros, good start tho

        //public enum Spectrums
        //{
        //    full,
        //    warm,
        //    cool,
        //    pastel,
        //    dark
        //};
        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            if (scene.name == "MenuCore" && isChromaInstalled == false)
            {
                //Settings Menu Setup
                var subMenuCC = SettingsUI.CreateSubMenu("Rainbow Lighting");
                var disableMenu = subMenuCC.AddBool("Enabled");
                disableMenu.GetValue += delegate { return ModPrefs.GetBool("RainbowLighting", "Enabled", true, true); };
                disableMenu.SetValue += delegate (bool value) { enabled = value; ModPrefs.SetBool("RainbowLighting", "Enabled", value); };
                disableMenu.EnabledText = "ON";
                disableMenu.DisabledText = "OFF";
                //var spectrumSelect = subMenuCC.AddList("Spectrum", new float[1] { 0 });
                //spectrumSelect.FormatValue += delegate (float value) { return Spectrums[(int)value]; };
                    //TO DO: actual spectrum select

                //PlayerSettings Toggle
                Sprite icon = UIUtilities.LoadSpriteFromResources("RainbowLighting.Resources.icon.png");
                var disableOption = GameplaySettingsUI.CreateToggleOption(GameplaySettingsPanels.PlayerSettingsRight, "Rainbow Lighting", "MainMenu", "Enable Rainbow Lighting",icon);
                disableOption.GetValue = ModPrefs.GetBool("RainbowLighting", "Enabled", true, true);
                disableOption.OnToggle += (value) => { enabled = value; ModPrefs.SetBool("RainbowLighting", "Enabled", value); };
            }

		}
        public static bool isChromaInstalled = false;
        public void OnApplicationStart()
		{
            if (IllusionInjector.PluginManager.Plugins.Any(x => x.Name == "Chroma"))
            {
                isChromaInstalled = true;
                Log("Chroma Detected, Disabling Rainbow Lighting");
            }
            else
                randomColor = ScriptableObject.CreateInstance<ColorRandom>();

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene scene)
        {
            if (isChromaInstalled == false)
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
