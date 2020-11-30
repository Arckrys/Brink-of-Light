using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class BasicEnemyController : Character
{
    public float stoppingDistance;
    public float detectionRadius;
    public AudioClip Grunt;
    public AudioClip DyingSound;
    protected float knockbackIntensity;
    public float knockbackResistance;
    protected Transform player;
    protected Animator gfxAnim;
    protected bool playerDetected;
    protected int knockbackTimer;
    protected int wanderTimer;
    protected Vector2 randomDirection;
    public GameObject projectile;
    [SerializeField] private bool isBoss;

    private CanvasGroup canvasGroupLifeBar;
    private Coroutine lifeBarCoroutine;

    [SerializeField] private int levelOneSpawnProbability;
    [SerializeField] private int levelTwoSpawnProbability;
    [SerializeField] private int levelThreeSpawnProbability;
    [SerializeField] private int levelFourSpawnProbability;

    [SerializeField] private GameObject lootBag;

    GameObject combustibleGameObject;

    public float nextWaypointDistance = 3f;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;
    // Start is called before the first frame update

    protected override void Start()
    {
        combustibleGameObject = Resources.Load("Prefabs/Environment/Combustibles/StumpCombustible") as GameObject;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        gfxAnim = transform.GetComponent<Animator>();
        playerDetected = false;
        knockbackTimer = 0;
        wanderTimer = 0;
        randomDirection = new Vector2();
        randomDirection.x = (Random.Range(-10, 11));
        randomDirection.y = (Random.Range(-10, 11));

        canvasGroupLifeBar = transform.Find("EnemyLifeCanvas").GetComponent<CanvasGroup>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        UpdatePathWander();
        InvokeRepeating("UpdatePathWander", 0f, .5f);
        base.Start();
    }

    protected override void FixedUpdate()
    {
        
    }



    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, player.position, OnPathComplete);
        }
    }

    void UpdatePathWander()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, rb.position + randomDirection, OnPathComplete);
        }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    //Fonction qui défini la réaction du monstre en cas de collision
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //Prise en charge des collision avec les projectiles du joueurs
        if (collision.gameObject.tag.Equals("Spell"))
        {
            knockbackIntensity = collision.GetComponent<ProjectileScript>().MyKnockback;
            float damageReceived = collision.GetComponent<ProjectileScript>().MyCurrentDamage;
            var isCrit = collision.GetComponent<ProjectileScript>().isCrit;

            if (isBoss)
            {
                print("is boss");
                damageReceived += collision.GetComponent<ProjectileScript>().MyBossBonusDamage;
            }

            //chance to catch fire when hit by the player if they have the item "Essence"
            if (collision.GetComponent<ProjectileScript>().IsBurning)
            {
                print("start burning");
                StartCoroutine(StartDamageOnTime(1f, 15, 1f));
            }

            life.MyCurrentValue -= damageReceived;
            CombatTextManager.MyInstance.CreateText(transform.position, damageReceived.ToString(), DamageType.Damage, 1.0f, isCrit);

            ShowLifeBar();

            playerDetected = true;
            if(knockbackIntensity - knockbackResistance > 0)
            {
                gfxAnim.SetBool("Knockback", true);
                knockbackTimer = 0;
            }
            
            var mixer = Resources.Load("Sounds/AudioMixer") as AudioMixer;
            var volumeValue = .5f;
            var volume = !(mixer is null) && mixer.GetFloat("Volume", out volumeValue);

            if(life.MyCurrentValue == 0)
            {
                var position = transform.position;

                if (volume)
                {
                    AudioSource.PlayClipAtPoint(DyingSound, position, 1-Math.Abs(volumeValue)/80);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(DyingSound, position);
                }

                //create a lootbag on death
                //var itemType = Random.Range(0, 2);
                var itemType = 0;

                string randomConsumableItem = null;
                int consumableItemProbability = Random.Range(0, 25);

                if (consumableItemProbability == 0)
                    randomConsumableItem = RandomItemDrop(itemType);

                var loot = Instantiate(lootBag, position, Quaternion.identity);
                loot.transform.parent = this.transform.parent;
                loot.GetComponent<LootManager>().CreateBag(randomConsumableItem, itemType, Random.Range(0, 3), Random.Range(0, 6));

                //chance to create a combustible on death if the player has the item "Flamme éternelle"
                if (PlayerScript.MyInstance.GetChanceToSpawnCombustible() != 0)
                {
                    int combustibleProbability = Random.Range(0, PlayerScript.MyInstance.GetChanceToSpawnCombustible());
                    if (combustibleProbability == 0)
                    {                        
                        GameObject combustible = Instantiate(combustibleGameObject, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                        combustible.transform.parent = GameObject.FindGameObjectWithTag("Room").transform;
                    }
                }

                Destroy(gameObject);
            }
            else
            {
                if (volume)
                {
                    AudioSource.PlayClipAtPoint(Grunt, transform.position, 1-Math.Abs(volumeValue)/80);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(Grunt, transform.position);
                }
            }
        }
    }

    private string RandomItemDrop(int itemType)
    {
        var itemManager = ItemsManagerScript.MyInstance;
        
        return itemManager.SelectRandomItem(itemType > 0 ? itemManager.EquipmentItems : itemManager.ConsumableItems);
    }

    public void ShowLifeBar()
    {
        if (lifeBarCoroutine != null)
        {
            StopCoroutine(lifeBarCoroutine);
        }

        lifeBarCoroutine = StartCoroutine(FadeOutLifeBar());
    }

    private IEnumerator FadeOutLifeBar()
    {
        float startAlpha = 1.0f;

        canvasGroupLifeBar.alpha = startAlpha;

        yield return new WaitForSeconds(3.0f);

        float rate = 2.5f;

        float progress = 0.0f;

        while (progress < 1.0)
        {
            canvasGroupLifeBar.alpha = Mathf.Lerp(startAlpha, 0, progress);

            progress += rate * Time.deltaTime;

            yield return null;
        }

        StopCoroutine(lifeBarCoroutine);
    }

    /// <summary>
    /// Permet aux ennemies de tirer des projectiles dans une direction voulu
    /// </summary>
    /// <param name="direction">Direction du projectile</param>
    protected void FireProjectileAtDirection(Vector2 direction)
    {
        var position = transform.position;

        var projectileDirection = direction;

        var randomNumber = Random.Range(0, 100);
        var damageToDeal = attack.MyMaxValue;
        var isCrit = false;

        if (randomNumber <= critChance.MyMaxValue)
        {
            damageToDeal *= critDamage.MyMaxValue;
            isCrit = true;
        }

        //create projectile
        var newProjectile = Instantiate(projectile, new Vector3(position.x, position.y, -2), Quaternion.identity);
        newProjectile.GetComponent<EnnemiesProjectileScript>().SetDirection(projectileDirection, position.x, position.y);
        newProjectile.GetComponent<EnnemiesProjectileScript>().MyBaseDamage = damageToDeal;
        newProjectile.GetComponent<EnnemiesProjectileScript>().MyKnockback = knockback.MyMaxValue;
        newProjectile.GetComponent<EnnemiesProjectileScript>().isCrit = isCrit;
    }

    /// <summary>
    /// Script permettant de faire aller l'ennemi vers le joueur en utilisant un algorithme de pathfinding de type A*
    /// </summary>
    protected void MoveToPlayer()
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;

            seeker.StartPath(transform.position, player.position, OnPathComplete);
        }

        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            Move();
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        else
        {
            Move();
            FaceDirection(direction, gfxAnim);
        }
    }

    /// <summary>
    /// fonction utilisé quand l'ennemi n'a pas encore détécté le joueur
    /// </summary>
    protected void Wander()
    {
        if(wanderTimer >= 100)
        {
            randomDirection.x = (Random.Range(-10,11));
            randomDirection.y = (Random.Range(-10,11));
            direction = randomDirection;
            
            wanderTimer = 0;
            //print(randomDirection);
        }
        else
        {
            wanderTimer += 1;
            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;

                seeker.StartPath(transform.position, transform.position + (Vector3)randomDirection, OnPathComplete);
            }

            if (path == null)
                return;

            if (currentWaypoint >= path.vectorPath.Count)
            {
                Move();
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            else
            {
                Move();
                FaceDirection(direction, gfxAnim);
            }
        }
        if (Vector2.Distance(transform.position, player.position) < detectionRadius)
        {
            playerDetected = true;
            Move();
            CancelInvoke("UpdatePathWander");
            UpdatePath();
            InvokeRepeating("UpdatePath", 0f, .5f);
            
        }
    }

    /// <summary>
    /// Fonction utilitaire permettant d'ajouter un angle à un Vector2
    /// </summary>
    /// <param name="v">Vector2 à modifier</param>
    /// <param name="degrees">Angle à appliquer</param>
    /// <returns></returns>
    public Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    public int[] GetSpawnProbabilities()
    {
        return new int[] {
                levelOneSpawnProbability, 
                levelTwoSpawnProbability, 
                levelThreeSpawnProbability, 
                levelFourSpawnProbability };
    }
}
