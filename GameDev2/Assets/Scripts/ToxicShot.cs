using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicShot : Projectile
{
    public float damageReducFactor = 0.01f;
    private Color startColor;
    private float alpha;

	protected override void Start()
    {
        base.Start();
        startColor = sprite.color;
        alpha = startColor.a;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject contact = collision.gameObject;

        //can't hit player or other projectile
        //if (!contact.tag.Equals("Player") && contact.layer != 15)
        //{
        //Debug.Log("hit something");
        //Debug.Log("i shouldn't be here");
        if (contact.layer == 14)
        {

            Debug.Log("hit!");
            SimpleHostile pscript = contact.GetComponent<SimpleHostile>();
            //Debug.Log(lastDir);

            pscript.takeDamage(damage, dir);

        }

        if (contact.layer == 8)
            gameObject.SetActive(false);
    } 

    protected override void Update()
    {
        base.Update();

        //damage intensity is reduced as a function of distance from start position
        float reduction = damageReducFactor * Mathf.Abs(Vector3.Distance(startPos, transform.position));
        damage -= reduction;
        alpha -= reduction / (damage + 1);
        sprite.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        //when damage has no power, toxic shot disappears
        if (damage <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
