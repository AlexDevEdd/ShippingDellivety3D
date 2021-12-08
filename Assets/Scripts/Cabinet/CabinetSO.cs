using UnityEngine;

[CreateAssetMenu(fileName = "CabinetSO", menuName = "Gameplay/New CabinetSO")]
public class CabinetSO : ScriptableObject
{
    [SerializeField] private CabinetData cabinet;
   
    public CabinetData Cabinet => cabinet ;
   
}