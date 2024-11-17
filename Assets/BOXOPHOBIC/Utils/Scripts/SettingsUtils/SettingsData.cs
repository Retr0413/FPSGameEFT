// Cristian Pop - https://boxophobic.com/

using UnityEngine;

namespace Boxophobic.Utils
{
    [CreateAssetMenu(fileName = "Data", menuName = "BOXOPHOBIC/Settings Data")]
    public class SettingsData : ScriptableObject
    {
        public string data;
        new public string name = "Data";
        public Sprite icon = null;
        public void Use()
        {
            Debug.Log(name + "を使用しました");
        }
    }
}