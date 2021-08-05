using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class title : MonoBehaviour
{
    [SerializeField] GameObject obj;
    public float span = 0;
    bool doFade;
    Image img;

    private void Awake()
    {
        img = obj.GetComponent<Image>();
    }
    public void OnClick()
    {
        doFade = true;
    }

    private void Update()
    {
        if (doFade) Fader();
    }
    void Fader()
    {
        span += 0.5f;
        img.color = new Color(0, 0, 0, span / 255);
        if (span > 255) LoadScene();
    }

    void LoadScene()
    {
        SceneManager.LoadScene("UIViewTest");
    }
}
