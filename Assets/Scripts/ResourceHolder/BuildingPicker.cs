using UnityEngine;

public class BuildingPicker : MonoBehaviour
{
    [SerializeField] GameObject _buildingPrefab;

    public void SelectPrefab() => BuildingInstanciator.instance.SetBuildingPrefab(_buildingPrefab);

}
