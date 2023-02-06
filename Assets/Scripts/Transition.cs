using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    static Transition instance;
    public static Transition Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] Image leftImg;
    [SerializeField] Image rightImg;

    float speed = 20.0f;

    Coroutine prevCor = null;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void PlayTransition(string sceneStr)
    {
        if (prevCor != null)
        {
            StopCoroutine(prevCor);
        }

        prevCor = StartCoroutine(PlayTransitionCoroutine(sceneStr));
    }

    IEnumerator PlayTransitionCoroutine(string sceneStr)
    {
        while (leftImg.rectTransform.position.x < 0.0f)
        {
            Vector2 newPos = leftImg.rectTransform.position;

            newPos.x += speed;

            leftImg.rectTransform.position = newPos;


            newPos = rightImg.rectTransform.position;

            newPos.x -= speed;

            rightImg.rectTransform.position = newPos;

            yield return null;
        }

        leftImg.rectTransform.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
        rightImg.rectTransform.anchoredPosition = new Vector3(1920.0f, 0.0f, 0.0f);

        AsyncOperation handle = SceneManager.LoadSceneAsync(sceneStr);
        yield return new WaitUntil(() => handle.progress >= 0.9f);
        yield return new WaitForSeconds(0.2f);

        while (leftImg.rectTransform.position.x > -960.0f)
        {
            Vector2 newPos = leftImg.rectTransform.position;

            newPos.x -= speed;

            leftImg.rectTransform.position = newPos;


            newPos = rightImg.rectTransform.position;

            newPos.x += speed;

            rightImg.rectTransform.position = newPos;

            yield return null;
        }

        leftImg.rectTransform.anchoredPosition = new Vector3(-960.0f, 0.0f, 0.0f);
        rightImg.rectTransform.anchoredPosition = new Vector3(2880.0f, 0.0f, 0.0f);

    }
}
