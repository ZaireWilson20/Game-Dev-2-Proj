using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryBoss : MonoBehaviour
{
    //public float detectRadius = 2.0f;
    public float health_max = 2;
    private float health = 2;
    public int attack = 1;
    public float seekSpeed = 1.25f;
    public float attackSpeed = 2f;
    private bool chasing = false;
    private bool searching = false;
    private bool timer_started = true;
    //private bool returning = false;
    public float searchTimeout = 3.0f;
    private float timeleft;
    private float direction = 0f;
    private float lastDir;

    public float detectRadius = 3f;
    public float knockback = 5f;
    private bool facingRight;

    public Vector3 startPos;
    private Vector3 destination;
    private GameObject target;
    private float moveSpeed;
    private Vector2 velocity = new Vector2(0, 0);
    public Rigidbody2D rb;
    //public Collider2D detectCollider;
    //public Collider2D hitbox;
    private GameObject player;
    private bool flash = false;
    public int flashRate = 3;
    private int flashCt = 0;
    public bool invincible = false;
    public bool shooter = false;
    public float fireCooldown = 2f;
    public float fireRate = 0.1f;
    private float fireTime = 0f;
    private float nextFire = 0f;
    public float shootRadius = 10f;

    // private Sprite enemSprite; 
    private GameObject newProjectile;
    public GameObject projectile;
    private int shots = 0;
    public int shotSpray = 3;

    private SpriteRenderer sprite;

    private Animator anim;
    private bool dead = false;

    [SerializeField]
    //GameObject pa_playerObj;
    Player pa_script;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
