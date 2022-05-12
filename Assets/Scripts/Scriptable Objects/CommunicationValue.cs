using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu()]
public class CommunicationValue : ScriptableObject
{
    public List<ComV> comVs;//each type`s communication value
    /// <summary>
    /// get value of building object
    /// by checking to another type
    /// </summary>
    /// <param name="t"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public int GetValueByTypes(Types t, Types t2)
    {
        //check for types
        ComV comV= comVs.FirstOrDefault(e =>
         (t == e.firstType && t2 == e.secondtype)
         ||
         (t == e.secondtype && t2 == e.firstType));
        //get value
        return comV.value;
    }
}
[System.Serializable]
public class ComV
{
    public Types firstType;//first object type
    public Types secondtype;//second object type
    public int value;//solo object value

}