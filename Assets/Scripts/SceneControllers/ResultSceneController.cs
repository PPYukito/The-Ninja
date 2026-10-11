using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneManager : MonoBehaviour
{
    public TextMeshProUGUI resultTitleText;

    private void OnEnable()
    {
        
    }

    public void ChangeScene(string scenename)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scenename);
    }
}
