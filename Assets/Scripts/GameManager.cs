using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<Transform> spawnPlace = new List<Transform>();

    public List<int> spawnIdx = new List<int>();
    private int currentWave = 0;

    private bool[] CreatedPosition;

    public Sprite[] spadeCards;
    public Sprite[] heartCards;
    public Sprite[] cloverCards;
    public Sprite[] diamondCards;

    public GameObject monsterParent;
    public GameObject spawnPlaceParent = null;
    public GameObject monsterPrefab;
    public GameObject[] turrets;
    private List<GameObject> monsterList = new List<GameObject>();

    public void TurretCreated()
    {
        int idx = RandomSpawnIndex(spawnIdx);
        var newTurret = GetTurret();
        var pos = spawnPlace[idx].position;

        newTurret.transform.position = new Vector2(pos.x, pos.y + 0.05f);
        newTurret.GetComponent<PokerTurret>().turretPositionIndex = idx;
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

        return newTurret;
    }

    public void DestroyTurret(PokerTurret _object)
    {
        spawnIdx.Add(_object.turretPositionIndex);
        Destroy(_object.gameObject);
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
        }
    }

    private void Start()
    {
        var temp = spawnPlaceParent.GetComponentsInChildren<Transform>();

        for (int i = 1; i < temp.Length; i++)
        {
            spawnPlace.Add(temp[i]);
            spawnIdx.Add(i - 1);
        }

        CreatedPosition = new bool[spawnPlace.Count];

        StartCoroutine(CreateWave());
    }

    private IEnumerator CreateWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);

            currentWave++;

            for (int i = 0; i < currentWave; i++)
            {
                yield return new WaitForSeconds(1f);

                var m = GetMonster();
                float x = Random.Range(-1.3f, 1.3f);
                m.transform.position = new Vector3(x, 3.2f, 0);
                m.SetActive(true);
            }
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
}
