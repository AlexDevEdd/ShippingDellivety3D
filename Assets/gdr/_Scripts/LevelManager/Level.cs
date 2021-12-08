using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level Instance;

    // public Minigame minigame;
    public Transform cameraPos;

    private void Awake()
    {
        if (cameraPos != null)
            CameraMover.Instance.SetAndMoveToTarget(cameraPos, true);
        // minigame.Launch();
    }

    public void DestroySelf()
    {
        if (Instance == this)
            Instance = null;
        // minigame.Exit();
        Destroy(gameObject);
    }
}
