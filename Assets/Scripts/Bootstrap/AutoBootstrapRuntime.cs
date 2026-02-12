using UnityEngine;

namespace ImbaLife.Bootstrap
{
    public static class AutoBootstrapRuntime
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            if (Object.FindObjectOfType<PrototypeAutoSetup>() != null) return;

            GameObject bootstrap = new GameObject("PrototypeAutoSetup");
            bootstrap.AddComponent<PrototypeAutoSetup>();
        }
    }
}
