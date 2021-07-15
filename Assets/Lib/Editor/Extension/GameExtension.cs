﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameExtension : Editor
{
    public const BindingFlags BindFlags =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

    #region 设置GameView

    private static void SetGameViewByWH()
    {
        SetGameView(1920, 1080);
    }

    public static void SetGameView(int index)
    {
        var type = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
        var window = EditorWindow.GetWindow(type);
        var sizeSelectionCallbackMethod = type.GetMethod("SizeSelectionCallback",
            BindingFlags.Instance | BindingFlags.Public |
            BindingFlags.NonPublic);
        // ReSharper disable once PossibleNullReferenceException
        sizeSelectionCallbackMethod.Invoke(window, new object[] {index, null});
    }

    public static void SetGameView(int width, int height, string baseText = "")
    {
        var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var instanceProp = singleType.GetProperty("instance");
        var getGroupMethod = sizesType.GetMethod("GetGroup");
        var getCurrentGroupTypeMethod = sizesType.GetMethod("get_currentGroupType");

        // ReSharper disable once PossibleNullReferenceException
        var gameViewSizesInstance = instanceProp.GetValue(null, null);

        // ReSharper disable once PossibleNullReferenceException
        var curGroupType = getCurrentGroupTypeMethod.Invoke(gameViewSizesInstance, new object[] { });

        // ReSharper disable once PossibleNullReferenceException
        var group = getGroupMethod.Invoke(gameViewSizesInstance, new object[] {(int) curGroupType});
        var getTotalCountMethod = group.GetType().GetMethod("GetTotalCount", BindFlags);
        // ReSharper disable once PossibleNullReferenceException
        var count = (int) getTotalCountMethod.Invoke(group, null);

        for (var i = 0; i < count; i++)
        {
            // ReSharper disable once PossibleNullReferenceException
            var viewSize = group.GetType().GetMethod("GetGameViewSize", BindFlags).Invoke(group, new object[] {i});
            // ReSharper disable once PossibleNullReferenceException
            var sWidth = (int) viewSize.GetType().GetField("m_Width", BindFlags).GetValue(viewSize);
            // ReSharper disable once PossibleNullReferenceException
            var sHeight = (int) viewSize.GetType().GetField("m_Height", BindFlags).GetValue(viewSize);
            if (width == sWidth && sHeight == height)
            {
                SetGameView(i);
                return;
            }
        }

        var param = new object[4];
        param[0] = 1;
        param[1] = width;
        param[2] = height;
        param[3] = string.IsNullOrEmpty(baseText) ? "" + width + "x" + height : baseText;
        var size = typeof(Editor).Assembly.CreateInstance("UnityEditor.GameViewSize", true, BindFlags, null, param,
            null,
            null);
        // ReSharper disable once PossibleNullReferenceException
        group.GetType().GetMethod("AddCustomSize").Invoke(group, new[] {size});
        SetGameView(count);
    }

    #endregion

    #region 闪屏

    private static void SetSplashScreen()
    {
        SetSplashScreen(EditorGUIUtility.systemCopyBuffer);
    }

    /// <summary>
    ///     设置闪屏
    /// </summary>
    /// <param name="res">图片在Asset路径</param>
    private static void SetSplashScreen(string res)
    {
        //常规代码
        PlayerSettings.SplashScreen.show = true;
        PlayerSettings.SplashScreen.backgroundColor = Color.white;
        PlayerSettings.SplashScreen.drawMode = PlayerSettings.SplashScreen.DrawMode.AllSequential;
        PlayerSettings.SplashScreen.showUnityLogo = false;
        //PlayerSettings.SplashScreen.logos = null;


        var tex = AssetDatabase.LoadAssetAtPath<Texture>(res);
        var obj = typeof(PlayerSettings);
        var method = obj.GetMethod("FindProperty", BindFlags);

        // ReSharper disable once PossibleNullReferenceException
        var property = method.Invoke(obj, new object[] {"androidSplashScreen"}) as SerializedProperty;
        // ReSharper disable once PossibleNullReferenceException
        property.serializedObject.Update();
        property.objectReferenceValue = tex;
        property.serializedObject.ApplyModifiedProperties();

        property = method.Invoke(obj, new object[] {"iOSLaunchScreenPortrait"}) as SerializedProperty;
        // ReSharper disable once PossibleNullReferenceException
        property.serializedObject.Update();
        property.objectReferenceValue = tex;
        property.serializedObject.ApplyModifiedProperties();

        property = method.Invoke(obj, new object[] {"iOSLaunchScreenLandscape"}) as SerializedProperty;
        // ReSharper disable once PossibleNullReferenceException
        property.serializedObject.Update();
        property.objectReferenceValue = tex;
        property.serializedObject.ApplyModifiedProperties();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    #endregion

    #region

    /// <summary>
    ///     拷贝Project视图中的某个Prefab
    /// </summary>
    public void ProjectDuplicate()
    {
        var type = typeof(Editor).Assembly.GetType("UnityEditor.ProjectWindowUtil");
        var method = type.GetMethod("DuplicateSelectedAssets", BindFlags);
        // ReSharper disable once PossibleNullReferenceException
        method.Invoke(null, null);
    }

    /// <summary>
    ///     拷贝Scene视图的一个GameObject和Ctrl+D一致
    /// </summary>
    public void HierarchyDuplicate()
    {
        var type = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
        var window = EditorWindow.GetWindow(type);
        var method = type.GetMethod("DuplicateGO", BindFlags);
        // ReSharper disable once PossibleNullReferenceException
        method.Invoke(window, null);
    }

    #endregion


    /// <summary>
    ///     apply一个GameObject
    /// </summary>
    /// <param name="assetPath"></param>
    public static void ApplyPrefab(string assetPath)
    {
        var go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        EditorApplication.isPaused = false;
        ApplyPrefab(go);
    }

    /// <summary>
    ///     apply 一个GameObject
    /// </summary>
    /// <param name="go"></param>
    public static void ApplyPrefab(GameObject go)
    {
        var bak = new GameObject("@bak");
        var obj = PrefabUtility.InstantiatePrefab(go) as GameObject;
        obj.transform.SetParent(bak.transform, true);
        PrefabApply(obj);
        DestroyImmediate(bak);
    }

    private static void PrefabApply(GameObject obj)
    {
        var pType = PrefabUtility.GetPrefabType(obj);
        if (pType != PrefabType.PrefabInstance)
            return;

        //这里必须获取到prefab实例的根节点，否则ReplacePrefab保存不了
        //GameObject prefabGo = GetPrefabInstanceParent(obj);
        var prefabGo = obj;
        Object prefabAsset = null;
        if (prefabGo != null)
        {
            prefabAsset = PrefabUtility.GetPrefabParent(prefabGo);
            if (prefabAsset != null)
                PrefabUtility.ReplacePrefab(prefabGo, prefabAsset, ReplacePrefabOptions.ConnectToPrefab);
        }

        AssetDatabase.SaveAssets();
    }

    /// <summary>
    ///     获取父节点下所有子节点
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="transforms">容器</param>
    public static void GetAllChild(Transform parent, ref List<Transform> transforms)
    {
        for (var i = 0; i < parent.childCount; i++)
        {
            var tr = parent.GetChild(i);
            transforms.Add(tr);
            if (tr.childCount > 0)
                GetAllChild(tr, ref transforms);
        }
    }

    /// <summary>
    ///     获取一个一个子节点相对于父节点的路径
    /// </summary>
    /// <param name="root"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    private static string GetTransformPath(Transform root, Transform node)
    {
        if (null == node || null == root)
            return string.Empty;
        var sb = node.name;
        while (node.name != root.name && null != node.parent)
        {
            node = node.parent;
            sb = node.name + "/" + sb;
        }

        return sb;
    }

    #region 拷贝路径

    [MenuItem("Assets/拷贝/ABName", false, 1)]
    private static void CopyBundleName()
    {
        var guids = Selection.assetGUIDs;
        var str = "";
        foreach (var guid in guids)
        {
            var assetImporter = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));
            if (null != assetImporter)
                str += assetImporter.assetBundleName;
        }

        GUIUtility.systemCopyBuffer = str;
    }

    [MenuItem("Assets/拷贝/GUID", false, 1)]
    private static void CopyGUID()
    {
        var guids = Selection.assetGUIDs;
        var str = "";
        foreach (var guid in guids) str += guid;

        GUIUtility.systemCopyBuffer = str;
    }

    [MenuItem("Assets/拷贝/AssetPath", false, 1)]
    private static void CopyAssetPath()
    {
        var guids = Selection.assetGUIDs;
        var str = "";
        foreach (var guid in guids) str += AssetDatabase.GUIDToAssetPath(guid);
        GUIUtility.systemCopyBuffer = str;
    }

    [MenuItem("Assets/拷贝/FilePath", false, 1)]
    private static void CopyFilePath()
    {
        var guids = Selection.assetGUIDs;
        var str = "";
        foreach (var guid in guids)
            str += string.Format("{0}/{1}", Environment.CurrentDirectory, AssetDatabase.GUIDToAssetPath(guid));
        GUIUtility.systemCopyBuffer = str;
    }

    #endregion

    public static List<string> GetSelectionAssetPaths(bool filterMeta = false)
    {
        var assetPaths = new List<string>();
        // 这个接口才能取到两列模式时候的文件夹
        foreach (var guid in Selection.assetGUIDs)
        {
            if (string.IsNullOrEmpty(guid))
                continue;
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path))
            {
                assetPaths.Add(path);
                if (!filterMeta)
                    assetPaths.Add(path + ".meta");
            }
        }

        return assetPaths;
    }

    #region UGUI 类似PS的移动，ijkl，

#if UNITY_2017
    private const string up = "UGUI/Move/MoveUp %i";
    private const string down = "UGUI/Move/MoveDown %k";
    private const string left = "UGUI/Move/MoveLeft %j";
    private const string right = "UGUI/Move/MoveRight %l";
#else
    private const string up = "GameObject/Move/MoveUp &UP";
    private const string down = "GameObject/Move/MoveDown &DOWN";
    private const string left = "GameObject/Move/MoveLeft &LEFT";
    private const string right = "GameObject/Move/MoveRight &RIGHT";
#endif

    [MenuItem(up, false, 10)]
    public static void MoveUp()
    {
        Move(Vector3.up);
    }

    [MenuItem(down, false, 10)]
    public static void MoveDown()
    {
        Move(Vector3.down);
    }

    [MenuItem(left, false, 10)]
    public static void MoveLeft()
    {
        Move(Vector3.left);
    }

    [MenuItem(right, false, 10)]
    public static void MoveRight()
    {
        Move(Vector3.right);
    }

    private static void Move(Vector3 v3)
    {
        foreach (var item in Selection.gameObjects)
            item.transform.localPosition += v3;
    }

    #endregion

    /// <summary>
    ///     对齐UI相机的UI
    /// </summary>
    public static void AlignSvToUi()
    {
        //todo 这里要修改自己的节点
        var cam = GameObject.Find("UGuiSystem").GetComponent<Camera>();
        var sv = SceneView.lastActiveSceneView;
        var svc = sv.camera;
        svc.nearClipPlane = cam.nearClipPlane;
        svc.farClipPlane = cam.farClipPlane;
        sv.size = Mathf.Sqrt(svc.aspect) / 0.1071068f;
        sv.pivot = cam.transform.position;
        sv.rotation = cam.transform.rotation;
        sv.orthographic = true;
        sv.Repaint();
    }
}