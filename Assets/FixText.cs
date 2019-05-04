using UnityEngine;

public class FixText : MonoBehaviour
{
    public string rtlText = "";
    // Start is called before the first frame update
    void Start()
    {
        var t = GetComponent<RTLTMPro.RTLTextMeshPro>();
        t.text = rtlText;
    }

    private void OnValidate()
    {
        var t = GetComponent<RTLTMPro.RTLTextMeshPro>();
        t.text = rtlText;
    }
}
