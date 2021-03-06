﻿using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{

    public int playerDamage;
    public int wallDamage = 2;
    public int hp = 4;

    public Animator animator;
    private Transform target;
    private bool skipMove;
    private bool skipMove02 = false;
    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    public Player hitPlayer { get; private set; }
    public Wall hitWall { get; private set; }

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        base.Start();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("TRIGGER ENTER");
        if (other.tag == "Enemy")
        {
            if(other.GetComponent<Enemy>().incorporeal)
            {
                Destroy(gameObject);
            }
            if (animator != null)
            {
                animator.SetBool("isIncorporeal", true);
            }
            incorporeal = true;
            hp = 999999;
            playerDamage = 999999;
        }

        if (other.gameObject.tag == "Player" && incorporeal)
        {
            //Debug.Log("incoporeal touched player T");
            other.GetComponent<Player>().LoseFood(playerDamage);

            if (animator != null)
            {
                animator.SetTrigger("enemyAttack");
            }
            SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if (skipMove)
        {
            if (incorporeal && skipMove02)
            {
                skipMove = false;
                skipMove02 = false;
            }
            else
            {
                skipMove = false;
                skipMove02 = true;
                return;
            }            
        }

        base.AttemptMove<T>(xDir, yDir);

        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        hitPlayer.LoseFood(playerDamage);

        if (animator != null)
        {
            animator.SetTrigger("enemyAttack");
        }
        SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);


    }
    protected override void EnemyHitWall<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        if (animator != null)
        {
            animator.SetTrigger("enemyAttack");
        }
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            DestroyObject(gameObject);
        }
    }
    protected override void EnemyAttack<T>(T component)
    {
        /*if(incorporeal)
        {
            Player hitPlayer = component as Player;

            hitPlayer.LoseFood(playerDamage);

            if (animator != null)
            {
                animator.SetTrigger("enemyAttack");
            }
            SoundManager.instance.RandomizeSfx(enemyAttack1, enemyAttack2);
        }*/
    }
}
