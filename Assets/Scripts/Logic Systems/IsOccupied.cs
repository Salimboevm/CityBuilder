using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this class checks 
/// is building place occupied by something else
/// </summary>
public class IsOccupied : MonoBehaviour
{
    [SerializeField]
    private BuildableObjects buildableObject;//parent buildable object
    int counter;//how many objects are colliding with parent
    private void OnTriggerEnter(Collider other)
    {
        //check for layer of entering object
        if (other.gameObject.layer == 10)
        {
            //increase colliding objects number
            counter++;
            //check colliding objects are not equal to 0
            if(counter!=0)
                buildableObject.SetState(States.cannotBuild);//if it is not, then this object cannot be built
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //check for layer of entering object
        if (other.gameObject.layer == 10)
        {
            //decrease colliding objects number
            counter--;
            //check colliding objects are not equal to 0
            if (counter == 0)
            {
                buildableObject.SetState(States.canBuild);//if it is not, then this object cannot be built
            }
        }
    }
}
