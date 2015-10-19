using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Player : MonoBehaviour
{
  public GameObject explosion;
  private GameObject target;
  private float lastFired;
  public float timeBetweenShots, fireRadius;
  
  void Update ()
  {
    Move();
  }
  void Move()
  {
    
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
  void OnTriggerStay(Component c)
  {
    //when trigger with
    print(c.name + c.tag);
    if(c.gameObject.tag.Equals("Enemy") && ReadyToFire())
      Shoot(c.gameObject);
  }
}



