using UnityEngine;

public class QuestMonoHelper : MonoBehaviour
{
    public static QuestMonoHelper Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Agar tetap ada di scene
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
