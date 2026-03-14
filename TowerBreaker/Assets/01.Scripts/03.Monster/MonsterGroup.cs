using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGroup : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsterPrefabs;  // 추가할 몬스터
    [SerializeField] private int monsterCount;  // 추가할 몬스터의 수
    private List<Monster> monsters = new List<Monster>(); // 군집내 몬스터

    public List<Monster> Monsters => monsters;


    void Start()
    {
        for(int i = 0; i < monsterCount; i++)
        {
            int randomIndex = Random.Range(0, monsterPrefabs.Count);
            GameObject monsterObject = Instantiate(monsterPrefabs[randomIndex], transform);
            
            Monster monsterComponent = monsterObject.GetComponent<Monster>();

            if (monsterComponent != null)
            {
                monsters.Add(monsterComponent);
            }
        }
    }

    void Update()
    {
        
    }
}
