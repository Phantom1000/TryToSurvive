using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Character
{

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float sensitivity = 3f;
    [SerializeField]
    private float jumpForce = 30f;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private AudioSource ReloadSound;
    [SerializeField]
    private Transform gun;
    [SerializeField]
    private float gunSpeedRot;
    [SerializeField]
    private Text disAmmo;
    [SerializeField]
    private Slider disHP;
    [SerializeField]
    private Text disMessage;
    [SerializeField]
    private Collider col;
    [SerializeField]
    public int Rem;
    [SerializeField]
    private Text disKills;
    private const int magazine = 20;
    private const float timeShoot = 0.1f;
    private const float timeReload = 2f;
    private int kills;
    float yRot = 0;
    float groundDis = 0;
    float timer = 0;
    Vector3 velocity = Vector3.zero;
    bool b;

    private void Start()
    {
        kills = 0;
        groundDis = col.bounds.extents.y;
        HitPoints = 100;
        Ammo = 20;
        Cursor.lockState = CursorLockMode.Locked;
        IsAlive = true;
        UpdateInfo();
    }

    private void Update()
    {
        if (IsAlive)
        {
            Move();
            Turn();
            Jump();
            Fire();
        }
        TurnWeapon();
    }

    void Move()
    {
        Vector3 movHor = Input.GetAxisRaw("Horizontal") * transform.right;
        Vector3 movVer = Input.GetAxisRaw("Vertical") * transform.forward;
        velocity = (movHor + movVer).normalized * speed;
    }

    void Turn()
    {
        float xRot = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
        xRot = Mathf.Clamp(xRot, -360, 360);
        yRot += Input.GetAxis("Mouse Y") * sensitivity;
        yRot = Mathf.Clamp(yRot, -60, 60);
        transform.localEulerAngles = new Vector3(0, xRot, 0);
        if (cam != null)
        {
            cam.transform.localEulerAngles = new Vector3(-yRot, 0, 0);
        }
    }

    void Jump()
    {
        if (Input.GetButton("Jump") && Physics.Raycast(transform.position, Vector3.down, groundDis + 0.1f))
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void Fire()
    {
        if (Input.GetButton("Fire1") & (Ammo > 0))
        {
            StartCoroutine(Shoot(timeShoot));
            UpdateInfo();
        }
        if (Input.GetButtonDown("Reload"))
        {
            StartCoroutine(Reload());
        }
        if (Input.GetButtonDown("Sprint"))
        {
            speed = 20f;
            IsAttack = true;
        }
        if (Input.GetButtonUp("Sprint"))
        {
            speed = 10f;
            IsAttack = false;
        }
    }

    IEnumerator Reload()
    {
        if (!IsReload)
        {
            if ((Ammo < magazine) && (Rem > 0))
            {
                IsReload = true;
                yield return new WaitForSeconds(1f);
                int need = magazine - Ammo;
                if (Rem > need)
                {
                    Ammo = magazine;
                    Rem -= need;
                }
                else if (Rem != 0)
                {
                    Ammo += Rem;
                    Rem = 0;
                }
                ReloadSound.Play();
                IsReload = false;
                UpdateInfo();
            }
        }

    }

    public void UpdateInfo()
    {
        disAmmo.text = "Ammo: " + Ammo + "/" + Rem;
        disKills.text = "Kills: " + kills;
        disHP.value = HitPoints;
    }
    private void FixedUpdate()
    {
        if (velocity != Vector3.zero)
        {
            body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
        }
    }

    public void EnemyDie()
    {
        if (!b)
        {
            StartCoroutine(EnemeyDieCor());
            b = true;
        }
    }

    void TurnWeapon()
    {
        RaycastHit hitInfo;
        Vector3 gunRot;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo);
        if (hitInfo.collider == null)
        {
            gunRot = gun.forward;
        }
        else
        {
            gunRot = hitInfo.point - gun.position;
        }
        gun.rotation = Quaternion.Slerp(gun.rotation, Quaternion.LookRotation(gunRot), gunSpeedRot * Time.deltaTime);
    }

    IEnumerator EnemeyDieCor()
    {
        kills++;
        disMessage.text = "Устранение противника";
        yield return new WaitForSeconds(2f);
        disMessage.text = "";
        b = false;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        UpdateInfo();
    }

    protected override void Die()
    {
        base.Die();
        disMessage.text = "Вас убили!";
        Time.timeScale = 0;
    }


}
