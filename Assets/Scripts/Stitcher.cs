using UnityEngine;
using System.Collections.Generic;

public class Stitcher
{
    /// <summary>
    /// Stitch clothing onto an avatar.  Both clothing and avatar must be instantiated however clothing may be destroyed after.
    /// </summary>
    /// <param name="sourceClothing"></param>
    /// <param name="targetAvatar"></param>
    /// <returns>Newly created clothing on avatar</returns>
    public GameObject Stitch(GameObject sourceClothing, GameObject targetAvatar)
    {
        var boneCatalog = new TransformCatalog(targetAvatar.transform);
        var skinnedMeshRenderers = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer>();
        var targetClothing = AddChild(sourceClothing, targetAvatar.transform);


        foreach (var sourceRenderer in skinnedMeshRenderers)
        {
            var targetRenderer = AddSkinnedMeshRenderer(sourceRenderer, targetClothing);
            targetRenderer.bones = TranslateTransforms(sourceRenderer.bones, boneCatalog);
        }
        return targetClothing;
    }

    private GameObject AddChild(GameObject source, Transform parent)
    {
        source.transform.parent = parent;

        foreach (Transform child in source.transform)
        {
            Object.Destroy(child.gameObject);
        }

        return source;
    }

    private SkinnedMeshRenderer AddSkinnedMeshRenderer(SkinnedMeshRenderer source, GameObject parent)
    {
        GameObject meshObject = new GameObject(source.name);
        meshObject.transform.parent = parent.transform;

        var target = meshObject.AddComponent<SkinnedMeshRenderer>();
        target.sharedMesh = source.sharedMesh;
        target.materials = source.materials;
        return target;
    }

    private Transform[] TranslateTransforms(Transform[] sources, TransformCatalog transformCatalog)
    {
        var targets = new Transform[sources.Length];
        for (var index = 0; index < sources.Length; index++)
            targets[index] = DictionaryExtensions.Find(transformCatalog, sources[index].name);
        return targets;
    }

    #region TransformCatalog
    private class TransformCatalog : Dictionary<string, Transform>
    {
        #region Constructors
        public TransformCatalog(Transform transform)
        {
            Catalog(transform);
        }
        #endregion

        #region Catalog
        private void Catalog(Transform transform)
        {
            if (ContainsKey(transform.name))
            {
                Remove(transform.name);
                Add(transform.name, transform);
            }
            else
                Add(transform.name, transform);
            foreach (Transform child in transform)
                Catalog(child);
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