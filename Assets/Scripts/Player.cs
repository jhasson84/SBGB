using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : Turn
{
  public GameObject explosion;
  private GameObject target;
  private float lastFired;
  public float timeBetweenShots, fireRadius, speed, satRadius;
  public Transform dest;
  public Vector3[] path;
  public Vector3 currentWaypoint;
  int targetIndex;
  
  void Update ()
  {
    if(active)
    {
      if(movesLeft < 1)
        EndTurn();
      Move();
      
    }
  }
  void Move()
  {
    
    RaycastHit hit;
    if(Input.GetButtonDown("Fire1") && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
    {
      dest.position = new Vector3(hit.point.x, 1, hit.point.z) ;
      
      PathRequestManager.RequestPath(transform.position, dest.position, OnPathFound);
      
    } 
  }
  public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
  {
    print("onpath");
    if (pathSuccessful)
    {
      path = newPath;
      StopCoroutine("FollowPath");
      StartCoroutine("FollowPath");
    }
  }
  IEnumerator FollowPath()
  {
    targetIndex = 0;
    currentWaypoint = path[targetIndex];
    
    while(true)
    {
      if(VDistanceAway(currentWaypoint) < satRadius)
      {
        targetIndex++;
        movesLeft--;
        if(targetIndex >= path.Length || !active)
          yield break;
        currentWaypoint = path[targetIndex];
      }
      transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
      yield return null;
    }
  }
  void Shoot(GameObject obj) 
  {
    var tar = Target(obj);
    //Create explosion particle system at target location and destroy after it is done
    var o = Instantiate(explosion, tar.position, tar.rotation);
    Destroy(o, 2);
    lastFired = Time.time;
  }
  /*
    Only changes target if previous target is outside 
    of the firing radius
   */
  Transform Target(GameObject o)
  {
    if(!target)
      target = o;
    if(GODistanceAway(target) > fireRadius)
      target = o;
    return target.transform;
  }
  bool ReadyToFire()
  {
    return (Time.time - lastFired > timeBetweenShots);
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
    if(c.gameObject.tag.Equals("Enemy") && ReadyToFire())
    {
      movesLeft--;
      Shoot(c.gameObject);
      dest.position = transform.position;
      Destroy(c.gameObject, 1);
    }
    if(c.gameObject.tag.Equals("Treasure"))
    {
      Destroy(c.gameObject);
    }
    
  }
}



