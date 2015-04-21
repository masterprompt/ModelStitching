using UnityEngine;
using System.Collections.Generic;

public class Tester : MonoBehaviour
{
    #region Fields
    public List<GameObject> clothing;
    public GameObject avatar;
    private GameObject wornClothing;
    const int buttonWidth = 200;
    const int buttonHeight = 20;
    private Stitcher stitcher;
    #endregion

    #region Monobehaviour
    public void Awake()
    {
        stitcher = new Stitcher();
    }
    public void OnGUI()
    {
        var offset = 0;
        foreach (var cloth in clothing)
        {
            if (GUI.Button(new Rect(0, offset, buttonWidth, buttonHeight), cloth.name))
            {
                if (wornClothing != null) GameObject.Destroy(wornClothing);
                wornClothing = stitcher.Stitch(cloth, avatar);
            }
            offset += buttonHeight;
        }
    }
    #endregion
}
