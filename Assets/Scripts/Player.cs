﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : Unit
{
  public GameObject explosion, shootTarget;
  private float lastFired;
  public float timeBetweenShots, fireRadius, speed, satRadius;
  public Transform dest;
  //public Vector3[] path;
  public Vector3 currentWaypoint;
  int targetIndex;
  void Awake()
  {
    dest = new GameObject("dest").transform;
  }
  void Update ()
  {
    if(turnActive)
    {
      if(movesLeft < 1)
      {
        EndTurn();
        StopCoroutine("FollowPath");
      }
      TurnPhase();
      
    }
  }
    void TurnPhase()
    {

        RaycastHit hit;
        if (Input.GetButtonDown("Fire2") && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            dest.position = new Vector3(hit.point.x, 1, hit.point.z);

            PathRequestManager.RequestPath(transform.position, dest.position, OnPathFound);
        }
        if (Input.GetButtonDown("Fire1") && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.gameObject.tag == "Enemy")
                DisplayInfo(hit.collider.gameObject);
        }
  }
  void Shoot(GameObject obj) 
  {
    var tar = Target(obj);
    //Create explosion particle system at target location and destroy after it is done
    var o = Instantiate(explosion, tar.position, tar.rotation);
    Destroy(o, 2);
  }
  /*
    Only changes target if previous target is outside 
    of the firing radius
   */
  Transform Target(GameObject o)
  {

    if(!shootTarget)
      shootTarget = o;
    if(GODistanceAway(shootTarget) > fireRadius)
      shootTarget = o;
    return shootTarget.transform;
  }
    void DisplayInfo(GameObject o)
    {
        var unit = gameObject.GetComponent<Unit>();
        unit.StartCoroutine("ShowStats");
    }
    float GODistanceAway(GameObject a)
  {
    return (a.transform.position - transform.position).magnitude;
  }
  float TDistanceAway(Transform a)
  {
    return (a.position - transform.position).magnitude;
  }
  float VDistanceAway(Vector3 a)
  {
    return (a - transform.position).magnitude;
  }
 
  void OnTriggerStay(Component c)
  {
    //when trigger with
    print(c.name + c.tag);
    if(c.gameObject.tag.Equals("Enemy") && canAttack)
    {
      movesLeft-= 2;
      canAttack = false;
      Shoot(c.gameObject);
      StopCoroutine("FollowPath");
      dest.position = transform.position;
      Destroy(c.gameObject, 1);
    }
    if(c.gameObject.tag.Equals("Treasure"))
    {
      Destroy(c.gameObject);
    }
    
  }
}



