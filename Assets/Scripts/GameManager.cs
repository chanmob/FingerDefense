using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    private List<Transform> spawnPlace = new List<Transform>();

    [HideInInspector]
    public List<int> spawnIdx = new List<int>();
    [HideInInspector]
    public ObscuredInt currentWave = 0;
    [HideInInspector]
    public ObscuredInt currenthp = 5;
    [HideInInspector]
    public ObscuredInt touchDamage = 1;
    [HideInInspector]
    public ObscuredInt heartUpgrade = 0;
    [HideInInspector]
    public ObscuredInt spadeUpgrade = 0;
    [HideInInspector]
    public ObscuredInt diamondUpgrade = 0;
    [HideInInspector]
    public ObscuredInt cloverUpgrade = 0;
    [HideInInspector]
    public ObscuredInt waveCount;
    [HideInInspector]
    public ObscuredInt buyTurretCount = 0;
    //[HideInInspector]
    public ObscuredInt money = 0;

    [HideInInspector]
    public ObscuredBool[] createdPosition;
    public ObscuredBool waitForSale = false;
    private bool gameOver = false;
    private bool watchedAD = false;

    public Sprite[] spadeCards;
    public Sprite[] heartCards;
    public Sprite[] cloverCards;
    public Sprite[] diamondCards;
    public Sprite noHpSprite;
    public Sprite hpSprite;

    public GameObject bossMonster;
    public GameObject monsterParent;
    public GameObject bulletParent;
    public GameObject spawnPlaceParent;
    public GameObject monsterPrefab;
    public GameObject heartBullet;
    public GameObject spadeBullet;
    public GameObject diamondBullet;
    public GameObject cloverBullet;
    public GameObject noAD;

    public GameObject[] turrets;
    private List<GameObject> monsterList = new List<GameObject>();
    private List<GameObject> spadeBulletList = new List<GameObject>();
    private List<GameObject> heartBulletList = new List<GameObject>();
    private List<GameObject> diamondBulletList = new List<GameObject>();
    private List<GameObject> cloverBulletList = new List<GameObject>();

    public Text waveText;
    public Text MoneyText;
    public Text resultWaveText;
    public Text bestWaveText;

    public Image[] hpImage;

    public AudioSource coinAudio;

    public AudioClip hitAudioClip;

    public RectTransform resultPanel;

    public Button adButton;

    public HashSet<GameObject> roundMonster = new HashSet<GameObject>();

    private IEnumerator _waveCoroutine;

    public void TurretCreated()
    {
        int idx = RandomSpawnIndex(spawnIdx);
        var newTurret = GetTurret();
        var pos = spawnPlace[idx].position;
        createdPosition[idx] = true;

        newTurret.transform.position = new Vector2(pos.x, pos.y + 0.05f);
        newTurret.GetComponent<PokerTurret>().turretPositionIndex = idx;
        newTurret.GetComponent<Animator>().SetTrigger("Created");
        Quest.instance.CheckNormalQuest();
        Quest.instance.CheckHiddenQuest();
    }

    private int RandomSpawnIndex(List<int> _source)
    {
        int idx = Random.Range(0, _source.Count);
        int returnValue = 0;

        returnValue = _source[idx];
        _source.Remove(_source[idx]);

        return returnValue;
    }

    private GameObject GetTurret()
    {
        GameObject newTurret = null;

        int randomTurret = Random.Range(0, turrets.Length);
        newTurret = Instantiate(turrets[randomTurret]);

        Quest.instance.questTurretCount[randomTurret, 0] += 1;

        return newTurret;
    }

    public void DestroyTurret(PokerTurret _object)
    {
        spawnIdx.Add(_object.turretPositionIndex);
        Destroy(_object.gameObject);
    }

    public GameObject GetBullet(string _type)
    {
        GameObject newBullet = null;

        if (_type == "HEART")
        {
            if (heartBulletList.Count > 0)
            {
                newBullet = heartBulletList[0].gameObject;
                heartBulletList.RemoveAt(0);
            }
            else
            {
                newBullet = Instantiate(heartBullet);
                newBullet.transform.SetParent(bulletParent.transform);
            }
        }
        else if (_type == "DIAMOND")
        {
            if (diamondBulletList.Count > 0)
            {
                newBullet = diamondBulletList[0].gameObject;
                diamondBulletList.RemoveAt(0);
            }
            else
            {
                newBullet = Instantiate(diamondBullet);
                newBullet.transform.SetParent(bulletParent.transform);
            }
        }
        else if (_type == "CLOVER")
        {
            if (cloverBulletList.Count > 0)
            {
                newBullet = cloverBulletList[0].gameObject;
                cloverBulletList.RemoveAt(0);
            }
            else
            {
                newBullet = Instantiate(cloverBullet);
                newBullet.transform.SetParent(bulletParent.transform);
            }
        }
        else if (_type == "SPADE")
        {
            if (spadeBulletList.Count > 0)
            {
                newBullet = spadeBulletList[0].gameObject;
                spadeBulletList.RemoveAt(0);
            }
            else
            {
                newBullet = Instantiate(spadeBullet);
                newBullet.transform.SetParent(bulletParent.transform);
            }
        }
        else
        {
            return null;
        }

        return newBullet;
    }

    public void DisableBullet(GameObject _type)
    {
        _type.SetActive(false);

        if (_type.CompareTag("HeartBullet"))
        {
            heartBulletList.Add(_type);
        }
        else if (_type.CompareTag("DiamondBullet"))
        {
            diamondBulletList.Add(_type);
        }
        else if (_type.CompareTag("SpadeBullet"))
        {
            spadeBulletList.Add(_type);
        }
        else if (_type.CompareTag("CloverBullet"))
        {
            cloverBulletList.Add(_type);
        }
    }

    public GameObject GetMonster()
    {
        GameObject newMonster = null;

        if(monsterList.Count > 0)
        {
            newMonster = monsterList[0].gameObject;
            monsterList.RemoveAt(0);
        }
        else
        {
            newMonster = Instantiate(monsterPrefab);
            newMonster.transform.SetParent(monsterParent.transform);
        }

        return newMonster;
    }

    public void DisableMonster(GameObject _monster)
    {
        _monster.SetActive(false);
        monsterList.Add(_monster);
    }

    protected override void Awake()
    {
        base.Awake();

        for(int i = 0; i < 20; i++)
        {
            var newMonster = Instantiate(monsterPrefab);
            monsterList.Add(newMonster);
            newMonster.SetActive(false);
            newMonster.transform.SetParent(monsterParent.transform);

            var newHeartBullet = Instantiate(heartBullet);
            heartBulletList.Add(newHeartBullet);
            newHeartBullet.SetActive(false);
            newHeartBullet.tag = "HeartBullet";
            newHeartBullet.transform.SetParent(bulletParent.transform);

            var newSpadeBullet = Instantiate(spadeBullet);
            spadeBulletList.Add(newSpadeBullet);
            newSpadeBullet.SetActive(false);
            newSpadeBullet.tag = "SpadeBullet";
            newSpadeBullet.transform.SetParent(bulletParent.transform);

            var newDiamondBullet = Instantiate(diamondBullet);
            diamondBulletList.Add(newDiamondBullet);
            newDiamondBullet.SetActive(false);
            newDiamondBullet.tag = "DiamondBullet";
            newDiamondBullet.transform.SetParent(bulletParent.transform);

            var newCloverBullet = Instantiate(cloverBullet);
            cloverBulletList.Add(newCloverBullet);
            newCloverBullet.SetActive(false);
            newCloverBullet.tag = "CloverBullet";
            newCloverBullet.transform.SetParent(bulletParent.transform);
        }
    }

    private void Start()
    {
        Admob.instance.rewarded = false;
        Admob.instance.noAd = noAD;
        adButton.onClick.RemoveAllListeners();
        adButton.onClick.AddListener(() => Admob.instance.ShowRewardAD());

        var temp = spawnPlaceParent.GetComponentsInChildren<Transform>();

        for (int i = 1; i < temp.Length; i++)
        {
            spawnPlace.Add(temp[i]);
            spawnIdx.Add(i - 1);
        }

        createdPosition = new ObscuredBool[spawnPlace.Count];

        _waveCoroutine = CreateWave();

        StartCoroutine(_waveCoroutine);
    }

    private IEnumerator CreateWave()
    {
        while (true)
        {
            roundMonster.Clear();

            waveCount = 0;
            int checkTime = 5;
            for(int i = 0; i < 5; i++)
            {
                waveText.text = "다음 웨이브 까지 " + checkTime.ToString();

                yield return new WaitForSeconds(1f);

                checkTime--;                
            }

            currentWave++;
            waveText.text = "현재 웨이브 : " + currentWave.ToString();

            if(currentWave % 10 == 0)
            {
                yield return new WaitForSeconds(0.5f);

                var bm = Instantiate(bossMonster);
                bm.transform.position = new Vector2(0, 3f);
            }
            else
            {
                for(int i = 0; i < currentWave; i++)
                {
                    var m = GetMonster();
                    roundMonster.Add(m);
                }

                var lists = roundMonster.ToList();
                int len = lists.Count;

                for(int i = 0; i < len; i++)
                {
                    yield return new WaitForSeconds(0.5f);

                    var current = lists[i];
                    float x = Random.Range(-1.3f, 1.3f);
                    current.transform.position = new Vector3(x, 3.2f, 0);
                    current.SetActive(true);
                }

                //for (int i = 0; i < currentWave; i++)
                //{
                //    yield return new WaitForSeconds(0.5f);

                //    var m = GetMonster();
                //    float x = Random.Range(-1.3f, 1.3f);
                //    m.transform.position = new Vector3(x, 3.2f, 0);
                //    m.SetActive(true);
                //}
            }

            yield return new WaitWaveEnd();
        }
    }

    public Sprite ChangeCardSprite(string _name, int idx)
    {
        if(_name == "HEART")
        {
            return heartCards[idx];
        }
        else if(_name == "DIAMOND")
        {
            return diamondCards[idx];
        }
        else if(_name == "CLOVER")
        {
            return cloverCards[idx];
        }
        else if(_name == "SPADE")
        {
            return spadeCards[idx];
        }
        else
        {
            return null;
        }
    }

    public void CheckWaveIsEnd()
    {
        lock (this)
        {
            if(roundMonster.Count == 0)
            {
                Debug.Log("종료로로오오옹");
                EventManager.instance.waitForWaveToEndHandler();
            }

            //waveCount++;

            //Debug.Log(waveCount + " / " + currentWave + "확인");

            //if (waveCount == currentWave)
            //{
            //    Debug.Log("종료");
            //    EventManager.instance.waitForWaveToEndHandler();
            //}
        }
    }

    public void ADRestart()
    {
        if (watchedAD)
            return;

        gameOver = false;
        watchedAD = true;

        money += (currentWave * 100) + (buyTurretCount * 100) + 100;
        MoneyTextRefresh();

        currenthp = 5;
        for (int i = 0; i < hpImage.Length; i++)
        {           
            hpImage[i].sprite = hpSprite;
        }
        UIDoTween.instance.UITweenY(resultPanel, -1920, 1f, Ease.OutCubic);
        
        if(_waveCoroutine != null)
        {
            StopCoroutine(_waveCoroutine);
            _waveCoroutine = null;
        }

        _waveCoroutine = CreateWave();
        StartCoroutine(_waveCoroutine);
    }

    public void MoneyTextRefresh()
    {
        MoneyText.text = money.ToString();
    }

    public void ReadyToSell()
    {
        for (int i = 0; i < spawnPlace.Count; i++)
        {
            if(createdPosition[i] == true)
            {
                spawnPlace[i].GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f);
            }
        }
    }

    public void FinishToSell()
    {
        for (int i = 0; i < spawnPlace.Count; i++)
        {
            spawnPlace[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
        }
    }

    public void CheckEndGame()
    {
        if (gameOver)
            return;

        if(currenthp == 1)
        {
            Quest.instance.LastHpMission();
        }
  
        for (int i = 4; i >= currenthp; i--)
        {
            if (i < 0)
                break;

            hpImage[i].sprite = noHpSprite;
        }

        if (currenthp <= 0)
        {
            if (watchedAD)
                adButton.gameObject.SetActive(false);

            Debug.Log("게임 종료");
            gameOver = true;
            StopAllCoroutines();
            _waveCoroutine = null;

            var enemys = GameObject.FindGameObjectsWithTag("Enemy");
            int len = enemys.Length;

            for(int i = 0; i < len; i++)
            {
                Destroy(enemys[i]);
            }

            int bestScore = 0;

            Admob.instance.ShowScreenAD();

            if (ObscuredPrefs.HasKey("BESTSCORE"))
            {
                bestScore = ObscuredPrefs.GetInt("BESTSCORE");

                if(currentWave > bestScore)
                {
                    bestScore = currentWave;
                    ObscuredPrefs.SetInt("BESTSCORE", currentWave);

                    if (GooglePlay.instance.GooglePlayLogine())
                        GooglePlay.instance.UploadRanking(bestScore);
                }
            }
            else
            {
                bestScore = currentWave;
                ObscuredPrefs.SetInt("BESTSCORE", currentWave);
            }

            if(currentWave >= 100)
            {
                if (GooglePlay.instance.GooglePlayLogine())
                {
                    GooglePlay.instance.GetAchievement(GPGSIds.achievement_100);
                }
            }
            if(currentWave >= 80)
            {
                if (GooglePlay.instance.GooglePlayLogine())
                {
                    GooglePlay.instance.GetAchievement(GPGSIds.achievement_80);
                }
            }
            if(currentWave >= 60)
            {
                if (GooglePlay.instance.GooglePlayLogine())
                {
                    GooglePlay.instance.GetAchievement(GPGSIds.achievement_60);
                }
            }
            if(currentWave >= 40)
            {
                if (GooglePlay.instance.GooglePlayLogine())
                {
                    GooglePlay.instance.GetAchievement(GPGSIds.achievement_40);
                }
            }
            if(currentWave >= 20)
            {
                if (GooglePlay.instance.GooglePlayLogine())
                {
                    GooglePlay.instance.GetAchievement(GPGSIds.achievement_20);
                }
            }

            StartCoroutine(CountingWave(resultWaveText));
            //resultPanel.transform.Find("CurretWave").GetComponent<Text>().text = "이번 웨이브 : " + currentWave;
            bestWaveText.text = "최고 웨이브 : " + bestScore;
            UIDoTween.instance.UITweenY(resultPanel, 0, 1f, Ease.InCubic);

            return;
        }
    }

    private IEnumerator CountingWave(Text _text)
    {
        yield return new WaitForSeconds(1f);

        NumberCounting.instance.StartCount(_text, 0, currentWave, 1f);
    }
}
