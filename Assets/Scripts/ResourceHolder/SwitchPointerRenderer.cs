using DirectionEnum;
using UnityEngine;
public class SwitchPointerRenderer : MonoBehaviour
{
    [SerializeField] ResourceSwitch _resourceSwitch;
    [SerializeField] Transform _pointerTransform;
    private int _ltDescrId = -1;

    public void UpdatePointer()
    {
        int angle = (int)_resourceSwitch.direction.ToReferential(_resourceSwitch.allowedDirection);
        Vector3 rotationVect = new Vector3(0, 0, angle * 60);

        LeanTween.cancel(_ltDescrId);
        LTDescr descr = LeanTween.rotate(
            _pointerTransform.gameObject,
            rotationVect,
            BeltManager.instance.moveDuration / 2
        );

        _ltDescrId = descr.id;
    }
}
