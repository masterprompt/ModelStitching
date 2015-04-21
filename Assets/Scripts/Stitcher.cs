using UnityEngine;
using System.Collections.Generic;

public class Stitcher
{
    /// <summary>
    /// Stitch clothing onto an avatar.  Both clothing and avatar must be instantiated however clothing may be destroyed after.
    /// </summary>
    /// <param name="sourceClothing"></param>
    /// <param name="targetAvatar"></param>
    /// <returns></returns>
    public GameObject Stitch(GameObject sourceClothing, GameObject targetAvatar)
    {
        var boneCatelog = new TransformCatelog(targetAvatar.transform);
        var skinnedMeshRenderers = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer>();
        var targetClothing = AddChild(sourceClothing, targetAvatar.transform);
        foreach (var sourceRenderer in skinnedMeshRenderers)
        {
            var targetRenderer = AddSkinnedMeshRenderer(sourceRenderer, targetClothing);
            targetRenderer.bones = TranslateTransforms(sourceRenderer.bones, boneCatelog);
        }
        return targetClothing;
    }


    private GameObject AddChild(GameObject source, Transform parent)
    {
        var target = new GameObject(source.name);
        target.transform.parent = parent;
        target.transform.localPosition = source.transform.localPosition;
        target.transform.localRotation = source.transform.localRotation;
        target.transform.localScale = source.transform.localScale;
        return target;
    }

    private SkinnedMeshRenderer AddSkinnedMeshRenderer(SkinnedMeshRenderer source, GameObject parent)
    {
        var target = parent.AddComponent<SkinnedMeshRenderer>();
        target.sharedMesh = source.sharedMesh;
        target.materials = source.materials;
        return target;
    }

    private Transform[] TranslateTransforms(Transform[] sources, TransformCatelog transformCatelog)
    {
        var targets = new Transform[sources.Length];
        for (var index = 0; index < sources.Length; index++)
            targets[index] = DictionaryExtensions.Find(transformCatelog, sources[index].name);
        return targets;
    }


    #region TransformCatelog
    private class TransformCatelog : Dictionary<string, Transform>
    {
        #region Constructors
        public TransformCatelog(Transform transform)
        {
            Catelog(transform);
        }
        #endregion

        #region Catelog
        private void Catelog(Transform transform)
        {
            Add(transform.name, transform);
            foreach (Transform child in transform)
                Catelog(child);
        }
        #endregion
    }
    #endregion


    #region DictionaryExtensions
    private class DictionaryExtensions
    {
        public static TValue Find<TKey, TValue>(Dictionary<TKey, TValue> source, TKey key)
        {
            TValue value;
            source.TryGetValue(key, out value);
            return value;
        }
    }
    #endregion

}

