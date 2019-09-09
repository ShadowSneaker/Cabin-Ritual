using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelChanger : MonoBehaviour
{
    public Animator Anim;

    private int LevelToLoad;

    // Update is called once per frame
    void Update()
    {
        FadeToLevel(1);
    }

    void FadeToLevel(int levelIndex)
    {
        LevelToLoad = levelIndex;
        Anim.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(LevelToLoad);

    }
}
