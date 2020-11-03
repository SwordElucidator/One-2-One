using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class SingleController : MonoBehaviour
{
    public Text testInfo;
    public Text countDown;
    public GameObject main;
    public GameObject timeArea;
    public RectTransform timeContainer;
    public Text timeText;
    public Image timeProgress;
    public Text question;
    public Text[] answers;
    
    public TextAsset easy;
    public TextAsset normal;
    public TextAsset hard;
    public TextAsset crazy;
    
    public GameObject endArea;

    public Text scoreTextTop;
    public Text moneyTextTop;
    
    public Text scoreText;
    public Text bestScoreText;
    public Text moneyAmount;
    public GameObject reviveButton;
    public GameObject againButton;
    public GameObject noButton;

    private int _progress = 0;
    private int _money = 0;
    private bool _canRevive = true;
    private bool _failed = false;
    private Question _question;
    private Questions _easy;
    private Questions _normal;
    private Questions _hard;
    private Questions _crazy;
    private bool _dead = false;
    private bool _questionAnswered = false;

    private float _timeWidth;
    private float _leftTimePercentage = 1;
    private float _loseRate = 0;
    
    private float _hardness = 0.8f;
    private int _giveMoneyEvery = 10;

    // Start is called before the first frame update
    void Start()
    {
        _hardness = Random.Range(0.1f, 0.9f);
        
        if (InterState.Inherit)  // 继承
        {
            _hardness = InterState.Hardness;
            _money = InterState.Money;
            _progress = InterState.Progress;
            _canRevive = InterState.CanRevive;
            InterState.Inherit = false;
        }
        
        scoreTextTop.text = _progress.ToString();
        moneyTextTop.text = "+" + _money;
        
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
        testInfo.gameObject.SetActive(true);
        countDown.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.text = "2";
        yield return new WaitForSeconds(1);
        countDown.text = "1";
        testInfo.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        countDown.gameObject.SetActive(false);

        GenerateQuestion();
        main.SetActive(true);
        timeArea.SetActive(true);
    }

    private IEnumerator OnFail()
    {
        if (_failed) yield break;
        _failed = true;
        var group = main.GetComponent<CanvasGroup>();
        while (group.alpha > 0)
        {
            group.alpha -= Time.deltaTime * 1f;
            yield return null;
        }
        main.SetActive(false);
        _progress -= 1;  // 死得时候-1
        scoreText.text = _progress.ToString();
        moneyAmount.text = "+" + _money;
        if (_canRevive)
        {
            reviveButton.SetActive(true);
            noButton.SetActive(true);
            againButton.SetActive(false);
        }
        else
        {
            {
                reviveButton.SetActive(false);
                noButton.SetActive(false);
                againButton.SetActive(true);
            }
        }
        endArea.SetActive(true);
        var endGroup = endArea.GetComponent<CanvasGroup>();
        while (endGroup.alpha < 1)
        {
            endGroup.alpha += Time.deltaTime * 1f;
            yield return null;
        }
    }

    private void GenerateQuestion()
    {
        _failed = false;
        var qum = "=<color=#D0021B>?</color>";
        if (_progress <= (_hardness < 0.2 ? 5 : _hardness < 0.4 ? 3 : _hardness < 0.6 ? 2 : _hardness < 0.8 ? 1 : 0))
        {
            // ez
            _question = _easy.questions[Random.Range(0, _easy.questions.Length)];
            question.text = _question.cal + qum;
            answers[0].text = "1";
            answers[1].text = "2";
            answers[2].text = "3";
        }else if (_progress <= (_hardness < 0.2 ? 20 : _hardness < 0.4 ? 15 : _hardness < 0.6 ? 10 : _hardness < 0.8 ? 5 : 2))
        {
            // normal
            _question = _normal.questions[Random.Range(0, _normal.questions.Length)];
            question.text = _question.cal + qum;
            answers[0].text = "1";
            answers[1].text = "2";
            answers[2].text = "3";
        }else if (_progress <= (_hardness < 0.2 ? 50 : _hardness < 0.4 ? 30 : _hardness < 0.6 ? 20 : _hardness < 0.8 ? 10 : 5))
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
        
        foreach (var answer in answers)
        {
            answer.color = new Color(1f, 1f, 1f, 1f);
            var parent = answer.transform.parent;
            parent.Find("wrong").gameObject.SetActive(false);
            parent.Find("right").gameObject.SetActive(false);
        }
        _progress += 1;
        _leftTimePercentage = 1;
        timeProgress.color = new Color(0f, 0.69f, 0.35f);
        _loseRate = 0.33f;
        _questionAnswered = false;
        timeText.text = "Time";
        StartCoroutine(AutoHandleTimeOver());
    }

    private IEnumerator AutoHandleTimeOver()
    {
        var progress = _progress;
        yield return new WaitForSeconds(3);
        // 如果还是这道题
        if (progress == _progress)
        {
            // 判断一下做完没
            if (!_questionAnswered)
            {
                ChooseAnswer(-1);
            }
            yield return new WaitForSeconds(1f);  // 给点时间看看
            if (!_dead)
            {
                if (progress == _progress) GenerateQuestion();
            }
            else
            {

                StartCoroutine(OnFail());
            }
        }
    }
    
    private static void Shuffle<T>(IList<T> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    private void ChooseAnswer(int index)
    {
        if (_questionAnswered || _dead) return;
        if (index >= 0)
        {
            _questionAnswered = true;
            var answer = int.Parse(answers[index].text);
            _dead = answer != _question.val;
            
            // 展示结果
            var qum = "=<color=" + (answer == _question.val ? "#7ED321" : "#D0021B") + ">" + answer + "</color>";
            question.text = _question.cal + qum;
        }
        else
        {
            _dead = true;
        }

        foreach (var answer in answers)
        {
            answer.color = new Color(1f, 1f, 1f, 0.3f);
            var parent = answer.transform.parent;
            if (int.Parse(answer.text) == _question.val)
            {
                parent.Find("wrong").gameObject.SetActive(false);
                parent.Find("right").gameObject.SetActive(true);
            }
            else
            {
                parent.Find("wrong").gameObject.SetActive(true);
                parent.Find("right").gameObject.SetActive(false);
            }
        }

        _loseRate = 0;

        if (!_dead)
        {
            scoreTextTop.text = _progress.ToString();
            if (_progress % _giveMoneyEvery == 0)
            {
                _money += 1;
                moneyTextTop.text = "+" + _money;
            }
        }

        StartCoroutine(GenerateNext());
    }

    private IEnumerator GenerateNext()
    {
        var progress = _progress;
        // 如果还是这道题
        // 判断一下做完没
        yield return new WaitForSeconds(1f);  // 给点时间看看
        if (!_dead)
        {
            if (progress == _progress) GenerateQuestion();
        }
        else
        {
            StartCoroutine(OnFail());
        }
    }

    public void Choose0()
    {
        ChooseAnswer(0);
    }
    
    public void Choose1()
    {
        ChooseAnswer(1);
    }
    
    public void Choose2()
    {
        ChooseAnswer(2);
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
        if (answer % 10 == 5 || answer % 10 == 0 || Random.value >= 0.6f)
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
        var m2 = Random.Range(m1 + 1, 20);
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
            if (lorigin >= 0.66 && _leftTimePercentage < 0.66)
            {
                timeProgress.color = new Color(0.96f, 0.65f, 0.14f);
            }
            if (lorigin >= 0.33 && _leftTimePercentage < 0.33)
            {
                timeProgress.color = new Color(0.82f, 0.01f, 0.11f);
            }
        }
        else
        {
            if (_leftTimePercentage <= 0)
            {
                timeText.text = "Time Out";
            }
        }
    }

    public void Revive()
    {
        // 复活，本质上是重load但继承进度,TODO 可能要退还已经入账的钱，或者设计成出去再退
        InterState.Hardness = _hardness;
        InterState.Money = _money;
        InterState.Progress = _progress;
        InterState.CanRevive = false;
        InterState.Inherit = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("Home");
    }
    
}
