using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
<<<<<<< Updated upstream
    [Header("Random Mobs & Spawners")]
    [SerializeField] GameObject[] spawnerArray;
    [SerializeField] GameObject[] mobArray;
=======
    public GameObject mobRotator;
    public GameObject[] spawner;
    public GameObject[] mob;
    public GameObject mascotte;
    public float spawnTime;

    void Start()
    {
        StartCoroutine(Spawn());
    }
>>>>>>> Stashed changes

    [Header("Spawner Characteristics")]
    [SerializeField] GameObject target;
    [SerializeField] float spawnInterval;

    private void Start() => StartCoroutine(Spawn());
    
    private IEnumerator Spawn()
    {
        while (true)
        {
<<<<<<< Updated upstream
            var randomMobAndSpawn = SelectRandomMobAndSpawner();
            SpawnMob(randomMobAndSpawn.Item1, randomMobAndSpawn.Item2);
            yield return new WaitForSeconds(spawnInterval);
=======
            int selectedIndex = Random.Range(0, mob.Length);
            GameObject spawnerLocation = spawner[Random.Range(0, spawner.Length)];
            GameObject selectedMob = mob[selectedIndex];
            GameObject newMob = Instantiate(selectedMob, spawnerLocation.transform.position, Quaternion.identity);
            if(selectedIndex == mob.Length-1)
            {
                GameObject newParent = Instantiate(mobRotator, Vector3.zero, Quaternion.identity);
                newMob.transform.parent = newParent.transform;
            }

            Mob ennemy = newMob.GetComponent<Mob>();
            if (ennemy != null)
                ennemy.target = mascotte.transform;
            yield return new WaitForSeconds(spawnTime);
>>>>>>> Stashed changes
        }
    }

    private (GameObject, GameObject) SelectRandomMobAndSpawner() => (spawnerArray[Random.Range(0, spawnerArray.Length)], mobArray[Random.Range(0, mobArray.Length)]); //Return random spawner and mob

    private void SpawnMob(GameObject _mob, GameObject _spawner)
    /*Instanitate given mob at given spawner location, and set target position to given target*/
    {
        GameObject newMob = Instantiate(_mob, _spawner.transform.position, Quaternion.identity);
        Mob mobBehaviors = newMob.GetComponent<Mob>();
        if (mobBehaviors != null) mobBehaviors.target = target.transform;
    }
}
