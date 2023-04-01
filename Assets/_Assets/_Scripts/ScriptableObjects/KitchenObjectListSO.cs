using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This SO holds a list of all the kitchen object SO's to allow handling of serialisation in netcode to 
enable object synchronisation between client and server
*/

// [CreateAssetMenu()] // commented as only one was needed
public class KitchenObjectListSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectSOList;
}
