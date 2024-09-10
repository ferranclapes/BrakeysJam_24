using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;

public class GirlScript : MonoBehaviour
{
    [SerializeField] private float maxHealth = -1f;
    [SerializeField] private float health;
    [SerializeField] private float strength;
    [SerializeField] private float attackCooldown;
    private float attackTime;
    [SerializeField] private float speed;
    public string side;

    private GirlScript objective = null;
    private float distance;


    //For movement:
    private Rigidbody2D rb;
    private Vector2 movement;
    [SerializeField] private float minDistance = 0.5f;
    private float Cooldown = -0.1f;
    private float objectiveTime;
    private bool lockObjective;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(float h, float s, float at, float sp, string si){
        maxHealth = h;
        health = h;
        strength = s;
        attackCooldown = at;
        speed = sp;
        side = si;
    }

    // Update is called once per frame
    void Update()
    {
        if(maxHealth != -1f){
            if(objective == null){
                FindObjective();
                Stop();
                objectiveTime = Time.time + 2.0f;
                lockObjective = false;
            }
            else{

                if(Cooldown < 0){    
                    distance = Vector2.Distance(this.transform.position, objective.transform.position);
                    if(distance > minDistance){
                        Move();
                        attackTime = Time.time + 0.5f;
                    }
                    else{
                        Attack();
                    }
                }
                else{
                    Cooldown -= Time.deltaTime;
                }
            }
            if(objectiveTime <= Time.time && !lockObjective){
                objective = null;
            }
        }
    }

    private bool FindObjective(){
        //First, we3 define in which layer we will be looking the enemies for.
        int layerMask;
        if(side == "TeamA"){
            layerMask = LayerMask.GetMask("TeamB");
        }
        else{
            layerMask = LayerMask.GetMask("TeamA");
        }
        //Get the colliders of the enemies
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 50, layerMask);
        //Find the closest enemy
        if(colliders.Length > 0){
            GirlScript obj = null;
            float distance = Mathf.Infinity;

            foreach(Collider2D col in colliders){
                float dis = Vector2.Distance(this.transform.position, col.gameObject.transform.position);
                if( dis < distance){
                    //Define the objective as the closest enemy
                    obj = col.GetComponent<GirlScript>();
                    distance = dis;
                }
            }
            objective = obj;
        }
        if (objective != null){
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistance);
        if(objective != null)
        {Gizmos.DrawLine(transform.position, objective.transform.position);}
    }

    private void Move(){
        //Define the movement as the normalized vector of the direction. (objective - this)
        movement = new Vector2(objective.transform.position.x - this.transform.position.x, objective.transform.position.y - this.transform.position.y);
        movement.Normalize();

        //Apply the movement to the rigidbody
        rb.velocity = movement * speed;
        
        distance = Vector2.Distance(this.transform.position, objective.transform.position);
        if(distance < minDistance){
            Stop();
        }

    }

    private void Stop(){
        //Stop the rigidbody
        rb.velocity = Vector2.zero;
    }

    private void Attack(){
        //If enough time has passed --> attack
        if(attackTime <= Time.time){
            lockObjective = true;
            objective.TakeDamage(this.strength, transform.position);
            //Start the attack cooldown
            attackTime = Time.time + attackCooldown;
            Cooldown = 0.7f;
        }

    }

    public void TakeDamage(float damage, Vector2 origin){
        lockObjective = false;
        objectiveTime = Time.time + 0.5f;
        health -= damage;
        //StartCoroutine(ChangeColor());  // Change color when taking damage
        GetKnockBack(origin);
        if(health <= 0){
            Die();
        }
    }

    private void GetKnockBack(Vector2 origin){
        Vector2 knockbackDirection = (transform.position - (Vector3)origin).normalized;
        float knockbackForce = 5;

        // Apply the force to the Rigidbody2D
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        if(Cooldown > 0){
            Cooldown = Cooldown + 0.2f;
        }
        else{
            Cooldown = 1.0f;
        }
    }

    private IEnumerator ChangeColor(){
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Color originalColor = renderer.color;
        renderer.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        renderer.color = originalColor;
    }

    private void Die(){
        //Destroy the game object
        Destroy(this.gameObject);
    }

}
