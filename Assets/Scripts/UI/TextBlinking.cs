using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TextBlinking : MonoBehaviour
{
    public float textSpeed;

    private TextMeshProUGUI text;
    private float time;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        time += Time.deltaTime;

        text.color = new Color(1, 1, 1, Mathf.Abs(Mathf.Sin(time * textSpeed)));

        //if (Input.GetButton("Fire1"))
        //    UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
