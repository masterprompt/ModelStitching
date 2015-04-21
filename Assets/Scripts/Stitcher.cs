using UnityEngine;
using System.Collections.Generic;

public class Stitcher
{
    public GameObject Stitch(GameObject sourceClothing, GameObject targetAvatar)
    {
        sourceClothing = (GameObject)GameObject.Instantiate(sourceClothing);
        var boneCatelog = new BoneCatelog(targetAvatar.transform);
        var skinnedMeshRenderers = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer>();
        var targetClothing = CopyClothing(sourceClothing, targetAvatar.transform);
        foreach (var sourceRenderer in skinnedMeshRenderers)
        {
            var targetRenderer = CopySkinnedRenderer(sourceRenderer, targetClothing);
            targetRenderer.bones = TranslateBones(sourceRenderer.bones, boneCatelog);
        }
        GameObject.Destroy(sourceClothing);
        return targetClothing;
    }

    private GameObject CopyClothing(GameObject sourceClothing, Transform parent)
    {
        var targetClothing = new GameObject(sourceClothing.name);
        targetClothing.transform.parent = parent;
        targetClothing.transform.localPosition = sourceClothing.transform.localPosition;
        targetClothing.transform.localRotation = sourceClothing.transform.localRotation;
        targetClothing.transform.localScale = sourceClothing.transform.localScale;
        return targetClothing;
    }

    private SkinnedMeshRenderer CopySkinnedRenderer(SkinnedMeshRenderer sourceRenderer, GameObject target)
    {
        var targetRenderer = target.AddComponent<SkinnedMeshRenderer>();
        targetRenderer.sharedMesh = sourceRenderer.sharedMesh;
        targetRenderer.materials = sourceRenderer.materials;
        return targetRenderer;
    }

    private Transform[] TranslateBones(Transform[] sourceBones, BoneCatelog boneCatelog)
    {
        var targetBones = new Transform[sourceBones.Length];
        for (var index = 0; index < sourceBones.Length; index++)
            targetBones[index] = boneCatelog.Find(sourceBones[index].name);
        return targetBones;
    }


    #region BoneCatelog
    private class BoneCatelog : Dictionary<string, Transform>
    {
        #region Constructors
        public BoneCatelog(Transform transform)
        {
            CatelogBones(transform);
        }
        #endregion

        #region Catelog
        private void CatelogBones(Transform transform)
        {
            Add(transform.name, transform);
            foreach (Transform child in transform)
                CatelogBones(child);
        }
        #endregion
    }
    #endregion


}

