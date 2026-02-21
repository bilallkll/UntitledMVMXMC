using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("ForOtherScripts")]
    public bool disableMovement;
    [Header("Values")]
    public float speed;
    public float acceleration;
    public float jumpForce;
    public float dobleJumpForce;
    public Vector2 wallJumpForce;
    public float dashForce;
    public float jumpCoolDown;
    public float dashCoolDown;
    public float fallAcceleration;
    public float fallSpeed;
    public float jumpTime;
    public float doubleJumpTime;
    public float wallJumpTime;
    public float wallTreshHold;
    public float dashJumpTime;
    public float jumpDrag;
    public float knockBackTime;
    float jumpTimeCounter;
    float doubleJumpTimeCounter;
    float wallJumpTimeCounter;
    float dashTimeCounter;
    public Vector2 groundDetectRange;
    public float roofDetectRange;
    public float wallDetectRange;
    public float wallDrag;
    float HorizontalInput;
    float VerticalInput;
    public GlobalVariable globalVar;
    private float lastParticleTime;
    public float walkParticleCooldown;
    public float trailParticleCooldown;
    bool didWallGrabPlay;
    float orignalXScale;
    public float unstickWallTime;
    float wallTimeee;
    public float activateSpawnPointTime;
    public float spawnPointHeight;
    float activateSpawnPointTimer;
    public float disapearDelay;
    public float reapearDelay;
    [Header("Attack")]
    public float attackTime;
    public float attackCooldown;
    public float thrustForce;
    public float thrustTime;
    public float attackGraceTime;
    public float comboGraceTime;
    bool isThrust;
    public float pogoForce;
    [Header("Components")]
    //public HitBox thisHitBox;
    public SpriteRenderer Srenderer;
    public Transform SpriteTrans;
    [HideInInspector]public Rigidbody2D rb;
    public Animator animator;
    public Transform groundDetectPos;
    public Transform roofDetectPos;
    public Transform wallRightDetectPos;
    public Transform wallLeftDetectPos;
    public Transform feetPos;
    public GameObject WalkParticle;
    public GameObject TrailParticle;
    public GameObject JumpParticle;
    public GameObject DoubleJumpParticle;
    public GameObject DoubleJumpWings;
    public GameObject WallJumpParticleLeft;
    public GameObject WallJumpParticleRight;
    public GameObject nail;
    public GameObject nailUp;
    public GameObject spawnPoint;
    GameObject thisSpawnPoint;
    //public hurtBox thisHurtBox;
    bool canJump;
    bool canDobleJump;
    bool canDobleJumpGround;
    bool canDash;
    bool canDashGround;
    bool canAttack;
    bool canAttackGraceTime;
    bool canMove;
    bool isCreateSpawnPoint;
    [Header("Animations")]
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int JumpAnim = Animator.StringToHash("Jump");
    private static readonly int FallAnim = Animator.StringToHash("Fall");
    private static readonly int Wall = Animator.StringToHash("Wall");
    private static readonly int DashAnim = Animator.StringToHash("Dash");
    private static readonly int AttackAnim = Animator.StringToHash("Attack");
    private static readonly int AttackUpAnim = Animator.StringToHash("AttackUp");
    private static readonly int AttackDownAnim = Animator.StringToHash("AttackDown");
    private static readonly int HurtAnim = Animator.StringToHash("Hurt");
    private static readonly int ActivateSpawnPointAnim = Animator.StringToHash("ActivateSpawnPoint");
    [Header("State")]
    bool _moving;
    [HideInInspector] public bool _grounded;
    bool _roofed;
    bool _walledRight;
    bool _walledLeft;
    bool _lastWalledRight;
    bool _wallSticked;
    bool _jumping;
    bool _dobleJumping;
    bool _wallJumping;
    bool _flyingUp;
    bool _falling;
    bool _activateSpawnPoint;
    [HideInInspector] public bool _dash;
    [HideInInspector] public bool _facingRight;
    [HideInInspector] public bool _hurt;
    [HideInInspector] public bool _facingUp;
    [HideInInspector] public bool _facingDown;
    bool _attacking;
    bool _knockBacking;
    private int _currentState;
    [Header("Input")]
    public InputActionReference move;
    public InputActionReference jump;
    public InputActionReference dash;
    public InputActionReference attack;
    public InputActionReference reset;
    public InputActionReference createSpawnPoint;
    public InputActionReference teleportToSP;
    [Header("Sfx")]
    public AudioSource walkSfx;
    public AudioSource jumpSfx;
    public AudioSource jumpWingSfx;
    public AudioSource dashSfx;
    public AudioSource wallGrabSfx;
    public AudioSource attackSfx;

    
    public void Awake()
    {
        orignalXScale = SpriteTrans.localScale.x;
    }
    void Start()
    {
        PlayerSpawn();
        canMove = true;
    }

    public void PlayerSpawn()
    {
        canJump = true;
        canAttack = true;
        canDobleJump = true;
        canDash = true;
        Direction(true);
        //_facingRight = true;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Direction(false);
        AnimHandler();
        SpawnInputPoint();
        if (disableMovement)
        {
            walkSfx.Stop();
            return;
        }
        Fall();
        Jump();
        Movement();
        OnWall();
        Dash();
        //AttackGraceTime();
        StateHandler();
        
    }
    private void FixedUpdate()
    {
        GroundCheck();
        RoofCheck();
        WallCheck();
    }
    private void OnEnable()
    {
        PlayerSpawn();

        jump.action.started += JumpStart;
        dash.action.started += DashStart;
        attack.action.started += Attack;
        teleportToSP.action.started += GoBackToSpawn;
    }
    private void OnDisable()
    {
        jump.action.started -= JumpStart;
        dash.action.started -= DashStart;
        attack.action.started -= Attack;

        teleportToSP.action.started -= GoBackToSpawn;
    }
    public void SpawnInputPoint()
    {
        if (createSpawnPoint.action.ReadValue<float>() <= 0 || !_grounded)
        {
            activateSpawnPointTimer = activateSpawnPointTime;
            _activateSpawnPoint = false;
            return;
        }
        if(VerticalInput < 0)
        {
            rb.velocity = Vector2.zero;
            _activateSpawnPoint = true;
            activateSpawnPointTimer -= Time.deltaTime;
            if(activateSpawnPointTimer <= 0)
            {
                ActivateSpawnPoint();
            }
        }
        else
        {
            activateSpawnPointTimer = activateSpawnPointTime;
            _activateSpawnPoint = false;
        }

    }
    public void ActivateSpawnPoint()
    {
        Destroy(thisSpawnPoint);
        thisSpawnPoint = Instantiate(spawnPoint, transform.position + Vector3.up * spawnPointHeight, Quaternion.identity);
        activateSpawnPointTimer = activateSpawnPointTime;
    }
    public void GoBackToSpawn(InputAction.CallbackContext obj)
    {
        if (thisSpawnPoint == null) return;

        if(!_attacking && !_dash)
        {
            StartCoroutine(TeleportDelay());
        }
    }
    
    IEnumerator TeleportDelay()
    {
        yield return new WaitForSeconds(disapearDelay);
        disableMovement = true;
        rb.velocity = Vector2.zero;
        transform.position = thisSpawnPoint.transform.position;
        Destroy(thisSpawnPoint);
        yield return new WaitForSeconds(reapearDelay);
        disableMovement = false;
    }

    public void Pogo(float pogoMultiplier = 1f)
    {
        Debug.Log("pogo");
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * pogoForce * pogoMultiplier, ForceMode2D.Impulse);
        canDashGround = true;
        canDobleJumpGround = true;
    }
    public void KnockBack(float knockMultiplier = 1f)
    {
        Debug.Log("knmock");
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce((_facingRight? Vector2.left : Vector2.right) * knockMultiplier, ForceMode2D.Impulse);

        StartCoroutine(KnockBackTime());
    }
    IEnumerator KnockBackTime()
    {
        canMove = false;
        yield return new WaitForSeconds(knockBackTime);
        canMove = true;
    }
    public void Direction(bool start)
    {
        //flip sprite
        if ((HorizontalInput != 0 && !_attacking) || start)
        {
            if (Srenderer != null)
            {
                Srenderer.flipX = !_facingRight;
            }
            else
            {
                if (!_facingRight)
                    SpriteTrans.localScale = new Vector2(-Mathf.Abs(SpriteTrans.localScale.x), SpriteTrans.localScale.y);
                else
                    SpriteTrans.localScale = new Vector2(Mathf.Abs(SpriteTrans.localScale.x), SpriteTrans.localScale.y);
            }
        }
    }
    public void  Movement()
    {
        if (!canMove || _activateSpawnPoint)
        {
            return;
        }
        //Debug.Log("Movementing");
        //Move Player
        HorizontalInput = move.action.ReadValue<Vector2>().x;
        VerticalInput = move.action.ReadValue<Vector2>().y;
        
        if (!_walledRight && !_walledLeft && !_dash && !_knockBacking)
        {
            //Movement
            float targetSpeed = HorizontalInput * speed;

            rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
            //rb.velocity = new Vector2(HorizontalInput * speed, rb.velocity.y);


            //Check movement
            if (HorizontalInput != 0)
            {
               
                _moving = true;
                if (_grounded)
                {
                    if (!walkSfx.isPlaying)
                        walkSfx.Play();
                    if (Time.time >= lastParticleTime + walkParticleCooldown)
                    {
                        Instantiate(WalkParticle, feetPos.position, Quaternion.identity);
                        lastParticleTime = Time.time;
                    }
                }
                else
                {
                    if (walkSfx.isPlaying)
                        walkSfx.Stop();
                }
            }
            else
            {
                _moving = false;
                if (walkSfx.isPlaying)
                    walkSfx.Stop();
            }
        }
        else
        {
            if (walkSfx.isPlaying)
                walkSfx.Stop();
            _moving = false;
            if (!_dash)
            {
                //Unstick walls
                if ((_walledRight && HorizontalInput < 0))
                {
                    wallTimeee -= Time.deltaTime;
                    if (wallTimeee <= 0)
                    {
                        _walledRight = false;
                        _moving = true;
                        _wallSticked = false;
                    }
                    
                }
                else if ((_walledLeft && HorizontalInput > 0))
                {
                    wallTimeee -= Time.deltaTime;
                    if (wallTimeee <= 0)
                    {
                        _walledLeft = false;
                        _moving = true;
                        _wallSticked = false;
                    }
                }
                else if (_grounded)
                {
                    _walledLeft = false;
                    _moving = true;
                    _wallSticked = false;
                }
                else
                {
                    wallTimeee = unstickWallTime;
                }
                if (!_grounded && !_jumping && !_wallJumping)
                {
                    if (_walledRight && HorizontalInput > 0)
                    {
                        _wallSticked = true;
                    }
                    else if (_walledLeft && HorizontalInput < 0)
                    {
                        _wallSticked = true;
                    }

                    /*if (HorizontalInput == 0)
                    {
                        if (!_grounded)
                        {
                            _wallSticked = true;
                        }
                        _moving = false;
                        walkSfx.Stop();
                    }*/
                }
                else
                {

                    _wallSticked = false;
                }
            }
        }
    }


    public void JumpStart(InputAction.CallbackContext obj)
    {
        if (disableMovement) return;
        if (_grounded && !_jumping && canJump && !_wallSticked && !_dash && !_attacking && !_activateSpawnPoint)
        {
            Debug.Log("JumpStart");
            jumpTimeCounter = jumpTime;
            _jumping = true;
            jumpSfx.Play();
            Instantiate(JumpParticle, feetPos.position, Quaternion.identity);
            canJump = false;
            StartCoroutine(boolCoolDown(jumpCoolDown));
        }
        //Wall jump
        else if (!_jumping && canJump && _wallSticked && !_activateSpawnPoint)
        {
            _wallSticked = false;
            Debug.Log("WallJumpStart");
            wallJumpTimeCounter = wallJumpTime;
            _wallJumping = true;
            jumpSfx.Play();
            _lastWalledRight = _walledRight;
            if (_lastWalledRight) Instantiate(WallJumpParticleLeft, wallLeftDetectPos.position, Quaternion.identity);
            else Instantiate(WallJumpParticleRight, wallRightDetectPos.position, Quaternion.identity);
            canJump = false;
            StartCoroutine(boolCoolDown(jumpCoolDown));
        }
        //Double Jump
        else if (canDobleJump && canDobleJumpGround && !_dash && !_wallSticked && !_wallJumping && !_attacking && !_activateSpawnPoint)
        {
            doubleJumpTimeCounter = doubleJumpTime;
            _dobleJumping = true;
            jumpWingSfx.Play();
            if(DoubleJumpWings && DoubleJumpParticle)
            {
                GameObject feather = Instantiate(DoubleJumpParticle, transform.position, Quaternion.identity);
                GameObject wing = Instantiate(DoubleJumpWings, transform.position, Quaternion.identity);
                wing.transform.parent = transform;
                feather.transform.parent = transform;
            }
            canDobleJumpGround = false;
            canDobleJump = false;
            StartCoroutine(boolCoolDownDoubleJump(jumpCoolDown));
        }
    }

    public void Jump()
    {
        if (_jumping && jump.action.ReadValue<float>() > 0 && !_dash)
        {
            
            if (jumpTimeCounter > 0)
            {
                rb.drag = jumpDrag;
                rb.velocity = new Vector2(rb.velocity.x, 1 * jumpForce);
                jumpTimeCounter -= Time.deltaTime;
                //Particle
                if (Time.time >= lastParticleTime + trailParticleCooldown)
                {
                    Instantiate(TrailParticle, feetPos.position, Quaternion.identity);
                    lastParticleTime = Time.time;
                }
            }
            else if (jumpTimeCounter <= 0)
            {
                _jumping = false;
            }
            //Stop jumping when touching roof
            if (_roofed)
            {
                _jumping = false;
            }


        }
        else if (_dobleJumping && !_dash)
        {
            if (doubleJumpTimeCounter > 0)
            {
                rb.drag = jumpDrag;
                rb.velocity = new Vector2(rb.velocity.x, 1 * dobleJumpForce);
                doubleJumpTimeCounter -= Time.deltaTime;
                //Particle


                if (Time.time >= lastParticleTime + trailParticleCooldown)
                {
                    Instantiate(TrailParticle, feetPos.position, Quaternion.identity);
                    lastParticleTime = Time.time;
                }
            }
            else if (doubleJumpTimeCounter <= 0)
            {
                _dobleJumping = false;
            }
            //Stop jumping when touching roof
            if (_roofed)
            {
                _dobleJumping = false;
            }
        }
        //WallJump
        else if (_wallJumping)
        {
            if (wallJumpTimeCounter > 0)
            {
                rb.drag = jumpDrag;
                if (_lastWalledRight) rb.velocity = (Vector2.left * wallJumpForce.x + Vector2.up * wallJumpForce.y);
                else rb.velocity = (Vector2.right * wallJumpForce.x + Vector2.up * wallJumpForce.y);

                wallJumpTimeCounter -= Time.deltaTime;
                //Particle
                if (Time.time >= lastParticleTime + trailParticleCooldown)
                {
                    if (_walledRight) Instantiate(TrailParticle, wallRightDetectPos.position, Quaternion.identity);
                    else if (_walledLeft) Instantiate(TrailParticle, wallLeftDetectPos.position, Quaternion.identity);

                    lastParticleTime = Time.time;
                }
            }
            else if (wallJumpTimeCounter <= 0)
            {
                _wallJumping = false;
            }
            //Stop jumping when touching roof
            if (_roofed)
            {
                _wallJumping = false;
            }
        }
        //release jump button
        if (jump.action.ReadValue<float>() <= 0)
        {
            _jumping = false;
            _dobleJumping = false;
            _wallJumping = false;
        }
        if (_flyingUp && !_wallSticked)
        {

            rb.drag = jumpDrag;
        }

    }

    public void Fall()
    {
        if (_flyingUp != true && !_wallSticked && !_dash)
        {
            _falling = true;
            rb.drag = 0;
            if (rb.velocity.y > -fallSpeed)
                rb.AddForce(Vector2.down * fallAcceleration);
            else
                rb.velocity = new Vector2(rb.velocity.x, -fallSpeed);
        }
        else
        {

            _falling = false;
        }
    }
    public void OnWall()
    {
        if (_wallSticked && !_grounded && !_jumping && !_dash)
        {
            if(!didWallGrabPlay)
            {
                wallGrabSfx.Play();
                didWallGrabPlay = true;
            }
            if (_flyingUp)
            {
                rb.drag = jumpDrag;
            }
            else
            {
                rb.drag = wallDrag;
            }
            //Particle
            if (Time.time >= lastParticleTime + trailParticleCooldown)
            {
                Transform spawnPos = _walledRight ? wallRightDetectPos : wallLeftDetectPos;

                Instantiate(TrailParticle, spawnPos.position, Quaternion.identity);
                lastParticleTime = Time.time;
            }

        }
    }

    public void DashStart(InputAction.CallbackContext obj)
    {
        if (disableMovement || _activateSpawnPoint) return;
        if (canDash && canDashGround && !_attacking)
        {
            Debug.Log("dash");
            _dash = true;
            dashTimeCounter = dashJumpTime;
            dashSfx.Play();
            //(JumpParticle, feetPos.position, Quaternion.identity);
            canDash = false;
            canDashGround = false;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            StartCoroutine(boolCoolDownDash(dashCoolDown));
        }
    }
    public void Dash()
    {
        if (_dash)
        {
            if (!_facingRight) rb.velocity = new Vector2(-1 * dashForce, rb.velocity.y) ;
            else rb.velocity = new Vector2(1 * dashForce, rb.velocity.y);
            if (dashTimeCounter > 0)
            {
                dashTimeCounter -= Time.deltaTime;
            }
            else if (dashTimeCounter <= 0)
            {
                
                _dash = false;
                rb.gravityScale = 1;
            }
        }
    }

    public void Attack(InputAction.CallbackContext obj)
    {
        if (disableMovement || _activateSpawnPoint) return;
        if (canAttack) 
            PerformAttack();
    }

    public void PerformAttack()
    {
        if ((!canAttack || dash) && !canAttackGraceTime)
        {
            canAttackGraceTime = true;
        }
        Debug.Log("attackInput");
        if (!_dash)
        {
            Debug.Log("attack");

            attackSfx.Play();
            _attacking = true;
            canAttack = false;
            StartCoroutine(boolCoolDownAttack(attackCooldown));

        }



    }
    public void AttackGraceTime()
    {
        if (canAttackGraceTime)
        {
            if(attackGraceTime < 0)
            {
                attackGraceTime -= Time.deltaTime;
            }
            else
            {
                canAttackGraceTime = false;
            }
            
        } 
    }

    public void AnimHandler()
    {
        int state = GetStateAnim();

        if (state == _currentState) return;
        animator.CrossFade(state, 0);
        _currentState = state;
    }

    public int GetStateAnim()
    {
        //Priorities
        if (_hurt) return HurtAnim;

        if (_activateSpawnPoint) return ActivateSpawnPointAnim;

        if (_attacking && !_dash)
        {
            if (!_wallSticked)
            {
                if (_facingUp) return AttackUpAnim;
                else if (_facingDown && !_grounded) return AttackDownAnim;
                else if (_facingDown && _grounded && _attacking) return Idle;
                else if (_facingRight) return AttackAnim;
                else return AttackAnim;
            }
            else if (_walledLeft || _walledRight) return AttackAnim;

        }
        if (_dash) return DashAnim;
        if (!_grounded)
        {
            if (_wallSticked)
            {
                return Wall;
            }

            if (_jumping)
                return JumpAnim;
            else
                return FallAnim;
        }
        if (_moving) return Walk;
        return Idle;
    }
    public void StateHandler()
    {
        //Priorities

        if (!_dash)
        {
            if (!_grounded)
            {
                if (!_walledLeft && !_walledRight)
                {
                    _wallSticked = false;

                    didWallGrabPlay = false;
                }
                if (rb.velocity.y > 0)
                {
                    //_jumping = true;
                    _flyingUp = true;
                    //_falling = false;
                }
                else if (rb.velocity.y < 0)
                {
                    //_jumping = false;
                    _flyingUp = false;
                    //_falling = true;
                }
            }

            if (_grounded || _wallSticked) 
            {
                canDashGround = true;
                canDobleJumpGround = true;
            }

            
            if (HorizontalInput != 0 && !_wallSticked)
                _facingRight = HorizontalInput > 0;
        }
        if (!_attacking)
        {
            _facingUp = VerticalInput > 0;
            if (!_grounded) _facingDown = VerticalInput < 0;
            else _facingDown = false;
        }
    }

    /*public void KnockBack(float knockBackPower)
    {
        Vector2 knockBackDirection;
        if (_facingUp) knockBackDirection = Vector2.down;
        else if (_facingDown && !_grounded) knockBackDirection = Vector2.up;
        else if (_facingRight) knockBackDirection = Vector2.left;
        else knockBackDirection = Vector2.right;

        

        canDobleJumpGround = true;
        canDashGround = true;
        //Stop movement when knockBack o
        if (!_facingUp || (!_facingDown && _grounded))
        {
            _knockBacking = true;
            StartCoroutine(KnockBackBoolCoolDown(knockBackTime));
        }
        if(_facingDown || _facingUp)
            rb.AddForce(knockBackDirection * knockBackPower, ForceMode2D.Impulse);

    }*/

    
    public IEnumerator KnockBackBoolCoolDown(float coolDown)
    {

        yield return new WaitForSeconds(coolDown);
        _knockBacking = false;
    }

    public IEnumerator boolCoolDown(float coolDown)
    {

        yield return new WaitForSeconds(coolDown);
        canJump = true;
    }
    public IEnumerator boolCoolDownDash(float coolDown)
    {

        yield return new WaitForSeconds(coolDown);
        canDash = true;
    }
    public IEnumerator boolCoolDownDoubleJump(float coolDown)
    {

        yield return new WaitForSeconds(coolDown);
        canDobleJump = true;
    }
    public IEnumerator boolCoolDownAttack(float coolDown)
    {
        
        //thrust
        isThrust = false;
        if (((_facingDown || !_facingUp) && _grounded) || ((!_facingDown && !_facingUp) && !_grounded))
        {
            isThrust = true;
            yield return new WaitForSeconds(thrustTime);
            rb.AddForce((_facingRight ? Vector2.right : Vector2.left) * thrustForce, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(attackTime + (isThrust? -thrustTime : 0));
        _attacking = false;

        yield return new WaitForSeconds(coolDown - attackTime);
        canAttack = true; 

    }
    
    public void GroundCheck()
    {
        _grounded = Physics2D.BoxCast(groundDetectPos.position, groundDetectRange, 0, Vector2.down, .05f, globalVar.groundMask);

    }
    public void RoofCheck()
    {
        _roofed = Physics2D.CircleCast(roofDetectPos.position, roofDetectRange, Vector2.down, 0.05f, globalVar.groundMask);
    }
    public void WallCheck()
    {
        _walledLeft = Physics2D.CircleCast(wallLeftDetectPos.position, wallDetectRange, Vector2.left, 0.05f, globalVar.groundMask);
        _walledRight = Physics2D.CircleCast(wallRightDetectPos.position, wallDetectRange, Vector2.right, 0.05f, globalVar.groundMask);
    }


    


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(groundDetectPos.position, groundDetectRange);
        Gizmos.DrawWireSphere(roofDetectPos.position, roofDetectRange);

        Gizmos.DrawWireSphere(wallLeftDetectPos.position, wallDetectRange);
        Gizmos.DrawWireSphere(wallRightDetectPos.position, wallDetectRange);
    }
}
