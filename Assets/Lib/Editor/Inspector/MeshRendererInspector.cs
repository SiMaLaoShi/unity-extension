using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshRenderer), true)]
public class MeshRendererInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        OnGUISortingLayer();
    }

    private void OnGUISortingLayer()
    {
        GUILayout.Space(6);

        var rend = (MeshRenderer) target;

        GUILayout.BeginVertical("box");

        var layerNames = SortingLayer.layers.Select(x => x.name).ToList();
        EditorGUI.BeginChangeCheck();
        var selectedIndex = EditorGUILayout.Popup("SortingLayerName", layerNames.IndexOf(rend.sortingLayerName),
            layerNames.ToArray());
        if (EditorGUI.EndChangeCheck())
        {
            rend.sortingLayerName = SortingLayer.layers[selectedIndex].name;
            EditorUtility.SetDirty(rend.gameObject);
        }

        EditorGUI.BeginChangeCheck();
        var sortingOrder = EditorGUILayout.IntField("SortingOrder", rend.sortingOrder);
        if (EditorGUI.EndChangeCheck())
        {
            rend.sortingOrder = sortingOrder;
            EditorUtility.SetDirty(rend.gameObject);
        }

        var mats = rend.sharedMaterials;
        foreach (var mat in mats)
            DrawMaterialRenderQueue(mat);

        GUILayout.EndVertical();
    }

    private void DrawMaterialRenderQueue(Material mat)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.ObjectField(mat, typeof(Material), false);
        EditorGUI.BeginChangeCheck();
        var val = EditorGUILayout.IntField("RenderQueue", mat.renderQueue);
        if (EditorGUI.EndChangeCheck())
        {
            mat.renderQueue = val;
            EditorUtility.SetDirty(mat);
        }

        GUILayout.EndHorizontal();
    }
}