using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    private Image image;

    private float speed = 1.2f;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    public void StartTransition()
    {
        StartCoroutine(HandleStartTransition());
    }
    private IEnumerator HandleStartTransition()
    {
        while (image.color.a < 1)
        {
            image.color += new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
        yield break;
    }
    public void EndTransition()
    {
        StartCoroutine(HandleEndTransition());
    }
    private IEnumerator HandleEndTransition()
    {
        while (image.color.a > 0)
        {
            image.color -= new Color(0, 0, 0, speed * Time.deltaTime);
            yield return null;
        }
        yield break;
    }
}
