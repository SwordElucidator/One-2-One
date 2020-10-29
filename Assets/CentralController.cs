using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CentralController : MonoBehaviour
{

    public Text waiting;
    public Text countDown;
    public GameObject main;
    public RawImage[] avatars;
    public GameObject timeArea;
    public Transform timeContainer;
    public Image timeProgress;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initial());
    }

    private IEnumerator Initial()
    {
        waiting.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        waiting.gameObject.SetActive(false);
        countDown.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.text = "2";
        yield return new WaitForSeconds(1);
        countDown.text = "1";
        yield return new WaitForSeconds(1);
        countDown.gameObject.SetActive(false);
        main.SetActive(true);
        timeArea.SetActive(true);
        // 题目开始 TODO
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
