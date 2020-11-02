using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class CentralController : MonoBehaviour
{

    public Text waiting;
    public Text testInfo;
    public Text countDown;
    public GameObject main;
    public Text avatarCountText;
    public RawImage[] avatars;
    public RawImage[] avatarsShaders;
    public Texture2D inputTexture;
    public Texture2D deadTexture;
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
    public GameObject endAreaFail;

    public Text rank;
    public Text moneyAmount;

    private Dictionary<RawImage, RawImage> _avatarShaderMap;
    private int _progress = 0;
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
    private int _leftAliveCount = 20;

    private int _maxRank = 1;  // 最多能变成第几
    private float _hardness = 0.8f;
    private List<RawImage> _aliveNoneSelfAvatars;
    private List<GameObject> _toRemoveAvatars = new List<GameObject>();
    private int[] _counts;
    private Vector3 _regularDistance;
    
    private Color _deadColor = new Color(0.36f, 0.39f, 0.42f, 0.78f);
    private Color _onHoldColor = new Color(0f, 1f, 0.93f, 0.71f);
    
    
    // Start is called before the first frame update
    void Start()
    {
        _hardness = Random.Range(0.1f, 0.9f);
        StartCoroutine(Initial());
        
        _easy = JsonUtility.FromJson<Questions>(easy.text);
        _normal = JsonUtility.FromJson<Questions>(normal.text);
        _hard = JsonUtility.FromJson<Questions>(hard.text);
        _crazy = JsonUtility.FromJson<Questions>(crazy.text);
        var rectTrans = timeContainer;
        _timeWidth = rectTrans.rect.width;
        
        _avatarShaderMap = new Dictionary<RawImage, RawImage>();
        for (var i = 0; i < avatars.Length; i++)
        {
            _avatarShaderMap[avatars[i]] = avatarsShaders[i];
        }
    }

    private IEnumerator Initial()
    {
        var currentUserCount = Random.Range(1, 17);
        avatarCountText.text = "Player " + currentUserCount + "/20";
        _aliveNoneSelfAvatars = new List<RawImage>();
        for (var i = 1; i < currentUserCount; i++)
        {
            avatars[i].gameObject.SetActive(true);
            avatars[i].texture = Resources.Load<Texture2D>("Avatars/" + (i + 1));  // TODO 更改这些用户
            _aliveNoneSelfAvatars.Add(avatars[i]);
        }
        waiting.gameObject.SetActive(true);
        testInfo.text = "Test Info:\nhardness = " + _hardness.ToString("#.00");
        testInfo.gameObject.SetActive(true);
        for (var i = currentUserCount; i < 20; i++)
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 1.5f));
            avatars[i].gameObject.SetActive(true);
            avatars[i].texture = Resources.Load<Texture2D>("Avatars/" + (i + 1));
            avatarCountText.text = "Player " + (i + 1) + "/20";
            _aliveNoneSelfAvatars.Add(avatars[i]);
        }


        waiting.gameObject.SetActive(false);
        testInfo.gameObject.SetActive(false);
        countDown.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.text = "2";
        yield return new WaitForSeconds(1);
        countDown.text = "1";
        yield return new WaitForSeconds(1);
        countDown.gameObject.SetActive(false);
        avatarCountText.text = "Survivor 20/20";

        GenerateQuestion();
        main.SetActive(true);
        timeArea.SetActive(true);
        // 题目开始
        while (!_dead)
        {
            // TODO 进度条
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
                yield return new WaitForSeconds(1.5f);  // 给点时间看看
                if (!_dead && _aliveNoneSelfAvatars.Count > 0)
                {
                    GenerateQuestion();
                }
                else
                {

                    StartCoroutine(OnFail());
                }
            }
        }
    }

    private IEnumerator OnFail()
    {
        var group = main.GetComponent<CanvasGroup>();
        while (group.alpha > 0)
        {
            group.alpha -= Time.deltaTime * 1f;
            yield return null;
        }
        main.SetActive(false);
        var final = _aliveNoneSelfAvatars.Count + 1;
        if (final <= 4)
        {
            rank.text = final == 1 ? "1st" : final == 2 ? "2nd" : final == 3 ? "3rd" : (final + "th");
            moneyAmount.text = final == 1 ? "10" : final == 2 ? "5" : final == 3 ? "3" : "2";
            endArea.SetActive(true);
            var endGroup = endArea.GetComponent<CanvasGroup>();
            while (endGroup.alpha < 1)
            {
                endGroup.alpha += Time.deltaTime * 1f;
                yield return null;
            }
        }
        else
        {
            endAreaFail.SetActive(true);
            var endGroup = endAreaFail.GetComponent<CanvasGroup>();
            while (endGroup.alpha < 1)
            {
                endGroup.alpha += Time.deltaTime * 1f;
                yield return null;
            }
        }
    }

    private void GenerateQuestion()
    {
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

        foreach (var image in _aliveNoneSelfAvatars)
        {
            _avatarShaderMap[image].color = _onHoldColor;
            _avatarShaderMap[image].transform.Find("Mark").GetComponent<RawImage>().texture = inputTexture;
            _avatarShaderMap[image].gameObject.SetActive(true);
        }
        
        _avatarShaderMap[avatars[0]].color = _onHoldColor;
        _avatarShaderMap[avatars[0]].transform.Find("Mark").GetComponent<RawImage>().texture = inputTexture;
        _avatarShaderMap[avatars[0]].gameObject.SetActive(true);
        
        _progress += 1;
        _leftTimePercentage = 1;
        timeProgress.color = new Color(0f, 0.69f, 0.35f);
        _loseRate = 0.33f;
        _questionAnswered = false;
        timeText.text = "Time";
        StartCoroutine(SimulateChoose());
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

    private IEnumerator SimulateChoose()
    {
        // TODO 根据_maxRank来决定
        // _aliveNoneSelfAvatars;  // 
        // _toRemoveAvatars
    
        foreach (var img in _toRemoveAvatars)
        {
            if (img)
            {
                Destroy(img);
            }
        }

        _toRemoveAvatars = new List<GameObject>();  // 承载下面的所有
        var rightAnswer = int.Parse(answers[0].text) == _question.val ? 0 : int.Parse(answers[1].text) == _question.val ? 1 : 2;
        var wrong1 = rightAnswer == 0 ? 1 : 0;
        var wrong2 = rightAnswer != 2 ? 2 : 1;
        Shuffle(_aliveNoneSelfAvatars);
        var leftPeople = _aliveNoneSelfAvatars.Count;
        _counts = new[] {0, 0, 0};

        var initialTime = Random.Range(0.4f, leftPeople > 10 ? 1.5f : leftPeople > 5 ? 2f : leftPeople > 2 ? 2.5f : leftPeople > 1 ? 3f : 3.2f);
        
        // 找到位置
        var parents = new []
        {
            answers[0].transform.parent.Find("Selected"),
            answers[1].transform.parent.Find("Selected"),
            answers[2].transform.parent.Find("Selected")
        };
        
        parents[0].gameObject.SetActive(false);
        parents[1].gameObject.SetActive(false);
        parents[2].gameObject.SetActive(false);
        
        var i1 = parents[0].Find("Image1");
        var i2 = parents[0].Find("Image2");
        _regularDistance = i2.position - i1.position;
        if (leftPeople > 14)
        {
            _regularDistance += new Vector3(-4, 0, 0);
        }else if (leftPeople <= 5)
        {
            _regularDistance += new Vector3(4, 0, 0);
        }

        yield return new WaitForSeconds(initialTime);
        // 开始有人选，顺序随机
        var i = 0;
        while (i < _aliveNoneSelfAvatars.Count)
        {
            // 如果超时了
            if (_leftTimePercentage <= 0)
            {
                _avatarShaderMap[_aliveNoneSelfAvatars[i]].color = _deadColor;
                _avatarShaderMap[_aliveNoneSelfAvatars[i]].transform.Find("Mark").GetComponent<RawImage>().texture = deadTexture;
                _leftAliveCount -= 1;
                avatarCountText.text = "Survivor " + _leftAliveCount + "/20";
                _aliveNoneSelfAvatars.RemoveAt(i);
                // Debug.Log("清理");
                continue;
            }

            // 0.85的正确率
            // 其他的随便
            if (Random.value < (_aliveNoneSelfAvatars.Count <= _maxRank - 1 ? 1 : leftPeople > 18 ? (0.86 + _hardness / 10) : leftPeople > 12 ? (0.82 + _hardness / 10) : leftPeople > 6 ? (0.86 + _hardness / 10) : (0.88 + _hardness / 10)))
            {
                var pos = parents[rightAnswer].Find("Image1").position + _regularDistance * _counts[rightAnswer];
                _counts[rightAnswer] += 1;
                var newImg = Instantiate(_aliveNoneSelfAvatars[i].gameObject, parents[rightAnswer]);
                newImg.transform.position = pos;
                newImg.SetActive(true);
                _toRemoveAvatars.Add(newImg);
                _avatarShaderMap[_aliveNoneSelfAvatars[i]].gameObject.SetActive(false);
                i += 1;
            }
            else if (Random.value < 0.5)
            {
                var pos = parents[wrong1].Find("Image1").position + _regularDistance * _counts[wrong1];
                _counts[wrong1] += 1;
                var newImg = Instantiate(_aliveNoneSelfAvatars[i].gameObject, parents[wrong1]);
                newImg.transform.position = pos;
                newImg.SetActive(true);
                _toRemoveAvatars.Add(newImg);
                _avatarShaderMap[_aliveNoneSelfAvatars[i]].color = _deadColor;
                _avatarShaderMap[_aliveNoneSelfAvatars[i]].transform.Find("Mark").GetComponent<RawImage>().texture = deadTexture;
                _leftAliveCount -= 1;
                avatarCountText.text = "Survivor " + _leftAliveCount + "/20";
                // Debug.Log("清理");
                _aliveNoneSelfAvatars.RemoveAt(i);
            }
            else
            {
                var pos = parents[wrong2].Find("Image1").position + _regularDistance * _counts[wrong2];
                _counts[wrong2] += 1;
                var newImg = Instantiate(_aliveNoneSelfAvatars[i].gameObject, parents[wrong2]);
                newImg.transform.position = pos;
                newImg.SetActive(true);
                _toRemoveAvatars.Add(newImg);
                _avatarShaderMap[_aliveNoneSelfAvatars[i]].color = _deadColor;
                _avatarShaderMap[_aliveNoneSelfAvatars[i]].transform.Find("Mark").GetComponent<RawImage>().texture = deadTexture;
                _leftAliveCount -= 1;
                avatarCountText.text = "Survivor " + _leftAliveCount + "/20";
                // Debug.Log("清理");
                _aliveNoneSelfAvatars.RemoveAt(i);
            }
            // 需要考虑
            var nextRandom = Random.Range(0.05f, Math.Max(((leftPeople > 15 ? 3.05f : 2.85f) - initialTime) / (_aliveNoneSelfAvatars.Count - i), 0.06f));
            // Debug.Log(nextRandom);
            initialTime += nextRandom;
            yield return new WaitForSeconds(nextRandom);  // 可能有几个没答完的
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
            
            var p = answers[index].transform.parent.Find("Selected");
            var pos = p.Find("Image1").position + _regularDistance * _counts[index];
            _counts[index] += 1;
            var newImg = Instantiate(avatars[0].transform.parent.gameObject, p);
            newImg.transform.position = pos;
            newImg.SetActive(true);
            _toRemoveAvatars.Add(newImg);
        }
        else
        {
            _dead = true;
        }

        if (_dead)
        {
            _avatarShaderMap[avatars[0]].color = _deadColor;
            _avatarShaderMap[avatars[0]].transform.Find("Mark").GetComponent<RawImage>().texture = deadTexture;
            _leftAliveCount -= 1;
            avatarCountText.text = "Survivor " + _leftAliveCount + "/20";
        }
        else
        {
            _avatarShaderMap[avatars[0]].gameObject.SetActive(false);
        }

        foreach (var answer in answers)
        {
            answer.color = new Color(1f, 1f, 1f, 0.3f);
            var parent = answer.transform.parent;
            if (int.Parse(answer.text) == _question.val)
            {
                parent.Find("wrong").gameObject.SetActive(false);
                parent.Find("right").gameObject.SetActive(true);
                parent.Find("Selected").gameObject.SetActive(true);
            }
            else
            {
                parent.Find("wrong").gameObject.SetActive(true);
                parent.Find("right").gameObject.SetActive(false);
                parent.Find("Selected").gameObject.SetActive(true);
            }
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
            timeText.text = "Time Out";
        }
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
