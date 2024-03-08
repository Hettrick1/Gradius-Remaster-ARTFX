using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private pickupEffect effect;
    [SerializeField] private AudioSource pickupSound;
    [SerializeField] private AudioSource lifeSound;
    [SerializeField] private float speed;

    private PlayerMovements pl;

    //START
    void Start()
    {
        pl = PlayerMovements.instance;
    }

    private void Update()
    {
        if (transform.position.x > 5)
        {
            Destroy(gameObject);
        }
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    //COLLISION PLAYER
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (effect == pickupEffect.Life)
            {
                lifeSound.Play();
                pl.SetLife();
            }
            if (effect == pickupEffect.Speed)
            {
                pl.SetPlayerSpeed();
            }
            if (effect == pickupEffect.ChainReaction)
            {

            }
            if (effect == pickupEffect.Missile)
            {
                pl.SetMissileLevel();
            }
            if (effect == pickupEffect.Shield)
            {
                GameManager.instance.SetInvincible();
            }
            pickupSound.Play();
            Destroy(gameObject);
        }
    }

    //PICKUP TYPES
    enum pickupEffect
    {
        Life,           //0
        Speed,          //1
        ChainReaction,  //2
        Missile,        //3
        Shield          //4
    }
}
