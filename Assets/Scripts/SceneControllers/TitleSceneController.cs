using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : SceneControllerEntities
{
    [SerializeField] private string changeSceneTo;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            PlayClickSound();
            StopBGMSound();
            NinjaSceneManager.instance.ChangeScene(changeSceneTo);
        }
    }
}
