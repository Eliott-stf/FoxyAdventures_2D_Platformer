namespace Utils
{
    using UnityEngine;

    public static class GameUtils
    {
        public static string GetId(GameObject go)
        {
            return go.scene.name + "/" + go.name;
        }
    }
}
