using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

    private CanvasGroup canvasGroupLifeBar;
    private Coroutine lifeBarCoroutine;

    [SerializeField] private int levelOneSpawnProbability;
    [SerializeField] private int levelTwoSpawnProbability;
    [SerializeField] private int levelThreeSpawnProbability;
    [SerializeField] private int levelFourSpawnProbability;

    [SerializeField] private GameObject lootBag;

    // Start is called before the first frame update

    protected override void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gfxAnim = transform.GetComponent<Animator>();
        playerDetected = false;
        knockbackTimer = 0;
        wanderTimer = 0;
        randomDirection = new Vector2();

        canvasGroupLifeBar = transform.Find("EnemyLifeCanvas").GetComponent<CanvasGroup>();

        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Spell"))
        {
            knockbackIntensity = collision.GetComponent<ProjectileScript>().MyKnockback;
            float damageReceived = collision.GetComponent<ProjectileScript>().MyCurrentDamage;
            var isCrit = collision.GetComponent<ProjectileScript>().isCrit;

            life.MyCurrentValue -= damageReceived;
            CombatTextManager.MyInstance.CreateText(transform.position, damageReceived.ToString(), DamageType.Damage, 1.0f, isCrit);

            ShowLifeBar();

            playerDetected = true;
            if(knockbackIntensity - knockbackResistance > 0)
            {
                gfxAnim.SetBool("Knockback", true);
                knockbackTimer = 0;
            }

            if(life.MyCurrentValue == 0)
            {
                var position = transform.position;
                AudioSource.PlayClipAtPoint(DyingSound, position);

                //var itemType = Random.Range(0, 2);
                var itemType = 0;

                string randomConsumableItem = null;
                int consumableItemProbability = Random.Range(0, 25);

                if (consumableItemProbability == 0)
                    randomConsumableItem = RandomItemDrop(itemType);

                var loot = Instantiate(lootBag, position, Quaternion.identity);
                loot.transform.parent = this.transform.parent;
                loot.GetComponent<LootManager>().CreateBag(randomConsumableItem, itemType, Random.Range(0, 3), Random.Range(0, 6));
                
                Destroy(gameObject);
            }
            else
            {
                AudioSource.PlayClipAtPoint(Grunt, transform.position);
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

    protected void Wander()
    {
        if(wanderTimer >= 100)
        {
            randomDirection.x = (Random.Range(-1,2));
            randomDirection.y = (Random.Range(-1, 2));
            direction = randomDirection;
            FaceDirection(randomDirection,gfxAnim);
            wanderTimer = 0;
            print(randomDirection);
        }
        else
        {
            wanderTimer += 1;
        }
        if (Vector2.Distance(transform.position, player.position) < detectionRadius)
        {
            playerDetected = true;
        }
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
