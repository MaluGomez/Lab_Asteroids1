using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float accerleration;

    public float maxSpeed;

    public float drag;

    public float angularSpeed;

    public float shootRate = 0.5f;

    public float offsetBullet;

    public GameObject bulletPrefab;

    public Transform bulletSpawner;

    private Rigidbody2D rb;

    private float vertical;
    private float horizontal;
    private bool shooting;
    private bool canShoot = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = drag;
    }

    // Update is called once per frame
    void Update()
    {
        vertical = InputManager.Vertical;
        horizontal = InputManager.Horizontal;
        shooting = InputManager.Fire;

        Rotate();
        Shoot();
    }

    private void FixedUpdate()
    {
        var fowardMotor = Mathf.Clamp(vertical,0f,1f);
        rb.AddForce(transform.up * accerleration * fowardMotor);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
    public void Lose () {
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        SceneManager.LoadScene(0);
    }
    private void Rotate()
    {
        if (horizontal == 0)
        {
            return;
        }
        transform.Rotate(0,0,-angularSpeed *horizontal *Time.deltaTime);
    }
    private void Shoot () {
        if (shooting && canShoot)
        {
            StartCoroutine(FireRate());

        }
    }
    private IEnumerator FireRate () {
        canShoot = false;
        var pos = transform.up * offsetBullet + transform.position;
        var bullet = Instantiate (
            bulletPrefab,
            pos,
            transform.rotation
        );
        Destroy (bullet, 5);
        yield return new WaitForSeconds (shootRate);
        canShoot = true;
    }
}
