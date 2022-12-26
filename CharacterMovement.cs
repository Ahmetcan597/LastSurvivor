using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] GameObject gun1;
    [SerializeField] GameObject gun2;
    [SerializeField] GameObject SuperPowerGun;
    [SerializeField] GameObject gun1Bullet;
    [SerializeField] GameObject gun2Bullet;
    [SerializeField] GameObject SuperPowerBullet;
    [SerializeField] ParticleSystem gun1Particle;
    [SerializeField] AudioSource gun1Audio;
    [SerializeField] AudioSource gun2Audio;
    [SerializeField] AudioSource gun2AudioShoot;
    [SerializeField] AudioSource SuperPowerAudioLeft;
    [SerializeField] AudioSource SuperPowerAudioRight;
    [SerializeField] AudioSource Reload;
    [SerializeField] ParticleSystem gun2Particle;
    [SerializeField] ParticleSystem SuperPowerParticleLeft;
    [SerializeField] ParticleSystem SuperPowerParticleRight;
    [SerializeField] GameObject FinishScreen;
    public TextMeshProUGUI magazineText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI HighScoreText;
    public float shiftBoost = 1f;
    public Transform gun1Bulletpos;
    public Transform gun2Bulletpos;
    public Transform SuperBulletposLeft;
    public Transform SuperBulletposRight;
    public CharacterController controller;
    public Animator gun1animator;
    public Animator gun2animator;
    public Animator SuperPowerAnimator;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private const float MAX_HEALTH = 100f;
    public int score = 0;
    private int ammo = 50;
    public float health = MAX_HEALTH;
    public AudioSource Background;
    Vector3 move;
    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public int gun1magazine = 50;
    public int gun2magazine = 15;
    private bool canShoot = true;
    private bool canChange = true;
    private bool boostStartAnimDelay = true;
    private bool changeArms = true;
    public UnityEngine.UI.Image healthBar;
    public UnityEngine.UI.Image BoostBar;
    public UnityEngine.UI.Image SuperReady;
    public UnityEngine.UI.Image HealthLow;
    public TimeManager timeManager;
    public float boostUpdate = 0;
    public int sceneIndex = 0;
    private float SuperimageAlpha = 0f;
    private float HealthimageAlpha = 0f;
    private bool SuperPowerWorking = false;

    bool isGrounded;

    void Start()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("Score", 0);
        gun1.SetActive(true);
        gun2.SetActive(false);
        SuperPowerGun.SetActive(false);

    }

    
    void Update()
    {
        Reload.pitch = Time.timeScale;
        gun1Audio.pitch = Time.timeScale;
        BoostHealthUIUpdate();
        ScoreUpdater(sceneIndex);
        CharacterMove();
        PauseGame();
        GunSelect();
        ShootingSystem();
        Restart();
    }

    public void ScoreUpdater(int sceneNumber)
    {
        scoreText.SetText("SCORE: " + PlayerPrefs.GetInt("Score", 0).ToString());
        if(PlayerPrefs.GetInt("Score", 0) > PlayerPrefs.GetInt($"HighScore{sceneNumber}"))
        {
            PlayerPrefs.SetInt($"HighScore{sceneNumber}", PlayerPrefs.GetInt("Score", 0));
            HighScoreText.SetText("HighScore: " + PlayerPrefs.GetInt($"HighScore{sceneNumber}"));
        }
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(0);
        }
    }

    void BoostHealthUIUpdate()
    {

        if (boostUpdate >= 100)
        {
            float cycles = Time.time / 1;
            const float tau = Mathf.PI * 2;
            SuperimageAlpha = Mathf.Lerp(0f, 1f, Mathf.Sin(cycles * tau));
            SuperReady.color = new Color(SuperReady.color.r, SuperReady.color.g, SuperReady.color.b, SuperimageAlpha);

        }
        else
        {
            SuperReady.color = new Color(SuperReady.color.r, SuperReady.color.g, SuperReady.color.b, 0f);
        }

        if (health <= 20)
        {
            float cycles = Time.time / 1;
            const float tau = Mathf.PI * 2;
            HealthimageAlpha = Mathf.Lerp(0f, 1f, Mathf.Sin(cycles * tau));
            HealthLow.color = new Color(HealthLow.color.r, HealthLow.color.g, HealthLow.color.b, HealthimageAlpha);
        }
        else
        {
            HealthLow.color = new Color(HealthLow.color.r, HealthLow.color.g, HealthLow.color.b, 0f);
        }

        if (SuperPowerWorking)
        {
            boostUpdate -= Time.deltaTime * 10f;
        }
        if (ammo < 10)
        {
            ammoText.color = Color.red;
        }
        else
        {
            ammoText.color = Color.white;
        }
        if (gun1magazine < 10)
        {
            magazineText.color = Color.red;
        }
        else
        {
            magazineText.color = Color.white;
        }

        healthBar.fillAmount = health / MAX_HEALTH;
        BoostBar.fillAmount = boostUpdate / 100;
    }

    void CharacterMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftBoost = 1.5f;
        }
        else
        {
            shiftBoost = 1f;
        }

        move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime * shiftBoost);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "MachineGunBullet":
                health -= 1;
                break;
            case "SmallDroneBullet":
                health -= 7;
                break;
            case "WarriorBullet":
                health -= 5;
                break;
            case "SlowMotion":
                timeManager.DoSlowmotion();
                Destroy(other.gameObject);
                break;
            case "Health":
                if(health < 90)
                {
                    health += 10;
                    Destroy(other.gameObject);
                }
                else if(health >= 90 && health < 100)
                {
                    health = 100;
                    Destroy(other.gameObject);
                }
                break;
            case "Ammo":
                ammo += 50;
                ammoText.SetText($"{ammo}");
                Destroy(other.gameObject);
                break;
            case "Finish":
                FinishScreen.SetActive(true);
                Invoke("goMenu",4f);
                break;
        }
    }

    public void goMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void Restart()
    {
        if(health <= 0)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    void GunSelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && canChange)
        {
            gun1.SetActive(true);
            gun2.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && canChange)
        {
            gun1.SetActive(false);
            gun2.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.X) && canChange && boostUpdate >= 100)
        {
            boostStartAnimDelay = false;
            boostUpdate = 100;
            SuperPowerWorking = true;
            gun1.SetActive(false);
            gun2.SetActive(false);
            SuperPowerGun.SetActive(true);
            SuperPowerAnimator.Play("Starting",0);
            Invoke("boostAnimReset", 1.59f);
            Invoke("boostFinish",10f);
        }
    }

    void boostAnimReset()
    {
        boostStartAnimDelay = true;
    }

    void boostFinish()
    {
        SuperPowerWorking = false;
        gun1.SetActive(true);
        gun2.SetActive(false);
        SuperPowerGun.SetActive(false);
    }

    void ShootingSystem()
    {
        if (gun1.activeSelf)
        {
            magazineText.SetText(gun1magazine.ToString());
        }
        else if (gun2.activeSelf)
        {
            magazineText.SetText(gun2magazine.ToString());
        }

        if (gun1.activeSelf && Input.GetKey(KeyCode.Mouse0) && canShoot && gun1magazine > 0)
        {
            boostUpdate++;
            GameObject newGun1Bullet = Instantiate(gun1Bullet,gun1Bulletpos.position, Quaternion.Euler(gun1Bulletpos.eulerAngles)) as GameObject;
            gun1magazine--;
            gun1animator.Play("shoot",0);
            gun1Audio.Play();
            gun1Particle.Play();
            canShoot = false;
            Invoke("CooldownFinished", 0.1f);
            Destroy(newGun1Bullet, 2f);
        }
        else if (gun2.activeSelf && Input.GetKeyDown(KeyCode.Mouse0) && gun2magazine > 0)
        {
            GameObject newGun2Bullet = Instantiate(gun2Bullet, gun2Bulletpos.position, Quaternion.Euler(gun2Bulletpos.eulerAngles)) as GameObject;
            gun2magazine--;
            gun2AudioShoot.Play();
            gun2animator.Play("gun2Fire2");
            gun2Particle.Play();
            Destroy(newGun2Bullet, 2f);
        }else if (SuperPowerGun.activeSelf)
        {
            //true iken sag el ates edicek.
            if(Input.GetKeyDown(KeyCode.Mouse0) && changeArms && boostStartAnimDelay)
            {
                SuperPowerAudioRight.Play();
                GameObject rightHand = Instantiate(SuperPowerBullet, SuperBulletposRight.position, Quaternion.Euler(SuperBulletposRight.eulerAngles)) as GameObject;
                SuperPowerParticleRight.Play();
                SuperPowerAnimator.Play("ShootRight",2);
                changeArms = !changeArms;
                Destroy(rightHand, 2f);
            }else if(Input.GetKeyDown(KeyCode.Mouse0) && !changeArms && boostStartAnimDelay)
            {
                SuperPowerAudioLeft.Play();
                GameObject leftHand = Instantiate(SuperPowerBullet, SuperBulletposLeft.position, Quaternion.Euler(SuperBulletposLeft.eulerAngles)) as GameObject;
                SuperPowerParticleLeft.Play();
                SuperPowerAnimator.Play("ShootLeft",1);
                changeArms = !changeArms;
                Destroy(leftHand, 2f);
            }

        }

        if (gun1.activeSelf && Input.GetKeyDown(KeyCode.R) && ammo > 0 && canChange)
        {
            canChange = false;
            Reload.Play();
            gun1animator.Play("Armature|ArmRelo",2);
            gun1animator.Play("Gun|GunAnimation",1);
            gun1animator.Play("Reload|CheckReload",4);
            gun1animator.Play("Magazine|Magazine",3);
            Invoke("ReloadGun",5.42f);
        }
        else if (gun2.activeSelf && Input.GetKeyDown(KeyCode.R) && canChange)
        {
            gun2Audio.Play();
            gun2magazine = 0;
            canChange = false;
            gun2animator.Play("aRMlEFTpISTOL", 1);
            Invoke("GunTopPlay", 1.88f);
            gun2animator.Play("Magazine|MagazineAction", 3);
            gun2animator.Play("PistolGun|Action", 4);
            Invoke("ReloadGun", 2.52f);
        }

    }

    void CooldownFinished()
    {
        canShoot = true;
        
    }

    void GunTopPlay()
    {
        gun2animator.Play("PistolUp|PistolUp", 2);
    }

    void ReloadGun()
    {
        if (gun1.activeSelf)
        {
            if(ammo >= 50)
            {
                ammo -= 50 - gun1magazine;
                ammoText.SetText($"{ammo}");
                gun1magazine = 50;
            }else if(ammo + gun1magazine < 50 && ammo > 0)
            {
                gun1magazine = ammo + gun1magazine;
                ammo = 0;
                ammoText.SetText($"{ammo}");
            }
            else if(ammo > 0)
            {
                ammo -= 50 - gun1magazine;
                ammoText.SetText($"{ammo}");
                gun1magazine = 50;
            }
        }
        else if (gun2.activeSelf)
        {
            gun2magazine = 15;
        }
        canChange = true;

    }

}
