using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public bool gameOver = false;

    


    public Text scoreText;
    public Text gameOverText;
    public int score;
    public ParticleSystem HealthBurst;
    public ParticleSystem HitBurst;

    public float speed = 3.0f;
    
    public int maxHealth = 5;
    
    public GameObject projectilePrefab;
    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;
    public AudioClip cogSound;

    public AudioClip winMusic;
    public AudioClip loseMusic;
    public AudioClip backgroundMusic;

void Start()
{
    rigidbody2d = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        //PlaySound(backgroundMusic);
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        audioSource.loop = true;
}

public void PlaySound(AudioClip clip)
{
    audioSource.PlayOneShot(clip);
}
    // Update is called once per frame
    void Update()
    {
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();

            audioSource.PlayOneShot(cogSound);
            
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

                if (Input.GetKey("escape"))
                    {
                    Application.Quit();
                    }

              if (score >= 4)
            {
            gameOver = true;
            audioSource.loop = false;
            
            PlaySound(winMusic);
            audioSource.Play();
            gameOverText.text = "You Win! Game By Christopher Mason. Press R to restart!";


                if (Input.GetKey(KeyCode.R))

                    {
                    if (gameOver == true)
                        {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                        }
                    }
            
             }   

            if (currentHealth == 0) 
            {
            gameOver = true;
            audioSource.loop = false;
            
            PlaySound(loseMusic);
            audioSource.Play();
            gameOverText.text = "You lose! Press R to restart!";
            
            if (Input.GetKey(KeyCode.R))

                {
                    if (gameOver == true)
                    {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                     }
                }
            }


    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
             animator.SetTrigger("Hit");
            Instantiate(HitBurst, rigidbody2d.position + Vector2.up, Quaternion.identity);

            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;

        }
        if (amount > 0)
        {
        Instantiate(HealthBurst, rigidbody2d.position + Vector2.up, Quaternion.identity);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);


        
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
    }



    public void ChangeScore(int scoreAmount)

        {
            score += 1;
            scoreText.text = "Robots Fixed: " + score.ToString(); 
        }
}