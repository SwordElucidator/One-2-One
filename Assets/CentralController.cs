using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[System.Serializable]
public class Question
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string cal;
    public int val;
}

[System.Serializable]
public class Questions
{
    //employees is case sensitive and must match the string "employees" in the JSON.
    public Question[] questions;
}


public class CentralController : MonoBehaviour
{

    public Text waiting;
    public Text countDown;
    public GameObject main;
    public RawImage[] avatars;
    public GameObject timeArea;
    public RectTransform timeContainer;
    public Image timeProgress;
    public Text question;
    public Text[] answers;
    
    public TextAsset easy;
    public TextAsset normal;
    public TextAsset hard;
    public TextAsset crazy;

    private int _progress = 0;
    private Question _question;
    private Questions _easy;
    private Questions _normal;
    private Questions _hard;
    private Questions _crazy;
    private bool _dead = false;

    private float _timeWidth;
    private float _leftTimePercentage = 1;
    private float _loseRate = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Initial());
        
        _easy = JsonUtility.FromJson<Questions>(easy.text);
        _normal = JsonUtility.FromJson<Questions>(normal.text);
        _hard = JsonUtility.FromJson<Questions>(hard.text);
        _crazy = JsonUtility.FromJson<Questions>(crazy.text);
        var rectTrans = timeContainer;
        _timeWidth = rectTrans.rect.width;
    }

    private IEnumerator Initial()
    {
        waiting.gameObject.SetActive(true);
        // TODO 注入用户
        yield return new WaitForSeconds(3);
        waiting.gameObject.SetActive(false);
        countDown.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.text = "2";
        yield return new WaitForSeconds(1);
        countDown.text = "1";
        yield return new WaitForSeconds(1);
        countDown.gameObject.SetActive(false);
        GenerateQuestion();
        main.SetActive(true);
        timeArea.SetActive(true);
        // 题目开始 TODO
        while (!_dead)
        {
            // TODO 进度条
            var progress = _progress;
            yield return new WaitForSeconds(3);
            // 如果还是这道题
            if (progress == _progress)
            {
                GenerateQuestion();
            }
        }
    }

    private void GenerateQuestion()
    {
        var qum = "=<color=#D0021B>?</color>";
        if (_progress <= 3)
        {
            // ez
            _question = _easy.questions[Random.Range(0, _easy.questions.Length)];
            question.text = _question.cal + qum;
            answers[0].text = "1";
            answers[1].text = "2";
            answers[2].text = "3";
        }else if (_progress <= 15)
        {
            // normal
            _question = _normal.questions[Random.Range(0, _normal.questions.Length)];
            question.text = _question.cal + qum;
            answers[0].text = "1";
            answers[1].text = "2";
            answers[2].text = "3";
        }else if (_progress <= 30)
        {
            // hard
            _question = _hard.questions[Random.Range(0, _hard.questions.Length)];
            question.text = _question.cal + qum;
            // 困难题不能按照算
            var answ = generateFakeAnswers(_question.val);
            answers[0].text = answ[0].ToString();
            answers[1].text = answ[1].ToString();
            answers[2].text = answ[2].ToString();
        }

        else
        {
            // crazy
            _question = _crazy.questions[Random.Range(0, _crazy.questions.Length)];
            question.text = _question.cal + qum;
            // 困难题不能按照算
            var answ = generateFakeAnswers(_question.val);
            answers[0].text = answ[0].ToString();
            answers[1].text = answ[1].ToString();
            answers[2].text = answ[2].ToString();
        }

        _progress += 1;
        _leftTimePercentage = 1;
        _loseRate = 0.33f;
    }

    private int[] generateFakeAnswers(int answer)
    {
        // 大概有个感觉

        if (answer <= 10)
        {
            if (answer < 8 && Random.value > 0.9)
            {
                return new []{answer, answer + 1, answer + 2};
            }
            if (answer < 9 && answer > 1 && Random.value > 0.8)
            {
                return new []{answer - 1, answer, answer + 1};
            }
            if (answer > 2 && Random.value > 0.7)
            {
                return new []{answer - 2, answer - 1, answer};
            }
            if (answer < 8 && answer > 2 && Random.value > 0.6)
            {
                return new []{answer - 2, answer, answer + 2};
            }
            if (answer > 4 && Random.value > 0.5)
            {
                return new []{answer - 4, answer - 2, answer};
            }
            if (answer < 6 && Random.value > 0.4)
            {
                return new []{answer, answer + 2, answer + 4};
            }
            if (answer > 1 && answer < 8 && Random.value > 0.3)
            {
                return new []{answer - 1, answer, answer + 2};
            }
            if (answer > 3 && Random.value > 0.2)
            {
                return new []{answer - 3, answer - 1, answer};
            }
            if (answer > 3 && Random.value > 0.1)
            {
                return new []{answer - 3, answer - 2, answer};
            }
            return new []{answer, answer + 1, answer + 3};
        }
        // 较大情况
        // 以5结尾
        if (answer % 10 == 5)
        {
            if (Random.value > 0.66)
            {
                return new []{answer, answer + 10, answer + 20};
            }
            if (Random.value > 0.33)
            {
                return new []{answer- 20, answer - 10, answer};
            }
            return new []{answer - 10, answer, answer + 10};
        }
        var m1 = Random.Range(0, 15);
        var m2 = Random.Range(m1, 20);
        if (Random.value > 0.66)
        {
            return new []{answer, answer + m1, answer + m2};
        }
        if (Random.value > 0.33)
        {
            return new []{answer- m2, answer - m1, answer};
        }
        if (Random.value > 0.18)
        {
            return new []{answer- m2, answer, answer + m1};
        }
        return new []{answer - m1, answer, answer + m2};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_leftTimePercentage > 0 && _loseRate > 0)
        {
            var lorigin = _leftTimePercentage;
            _leftTimePercentage -= _loseRate * Time.deltaTime;
            var rectTransform = timeProgress.rectTransform;
            rectTransform.offsetMax = new Vector2(- (1 - _leftTimePercentage) * _timeWidth, rectTransform.offsetMax.y);
            // _timeWidth
            // timeProgress
            if (lorigin >= 0.6 && _leftTimePercentage < 0.6)
            {
                timeProgress.color = new Color(0.96f, 0.65f, 0.14f);
            }
            if (lorigin >= 0.25 && _leftTimePercentage < 0.25)
            {
                timeProgress.color = new Color(0.82f, 0.01f, 0.11f);
            }
        }
    }
}
