using System.Collections.Generic;
using System.IO;
using Lib.Editor.Scriptable;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class FbxAssetImporter : AssetPostprocessor
{
    private const string ANIM_SUFFIX = ".anim";
    private const string INVALID_ANIM_NAME = "__preview__";
    private const string ANIM_DIR = "clips";

    public void OnPreprocessModel()
    {
        var modelImporter = (ModelImporter) assetImporter;

        if (modelImporter.isReadable)
        {
            modelImporter.useFileScale = false;
            modelImporter.globalScale = 10f;
            modelImporter.isReadable = false;

            var clips = modelImporter.defaultClipAnimations;
            foreach (var clip in clips)
                if (clip.name == "Idle" || clip.name == "Idle")
                    clip.loopTime = true;

            modelImporter.SaveAndReimport();
        }
    }


    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        foreach (var str in importedAssets)
            if (str.EndsWith(".fbx"))
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(str);

                var controller = AnimatorController.CreateAnimatorControllerAtPath(str.Replace(".fbx", ".controller"));
                var rootStateMachine = controller.layers[0].stateMachine;

                var clipobjs = AssetDatabase.LoadAllAssetsAtPath(str);
                foreach (var clip in clipobjs)
                {
                    var srcclip = clip as AnimationClip;
                    if (srcclip == null)
                        continue;
                    if (clip.name.Contains(INVALID_ANIM_NAME))
                        continue;
                    var state = rootStateMachine.AddState(clip.name);
                    state.name = clip.name;
                    state.motion = clip as AnimationClip;

                    if (clip.name == "Idle" || clip.name == "Idle")
                        rootStateMachine.defaultState = state;
                }

                var animator = go.GetComponent<Animator>();
                if (null != animator)
                    animator.runtimeAnimatorController = controller;
#if UNITY_2018_1_OR_NEWER
                PrefabUtility.SaveAsPrefabAsset(go, str.Replace(".fbx", ".prefab"));
#else
                PrefabUtility.CreatePrefab(str.Replace(".fbx", ".prefab"), go);
#endif
                if (GlobalScriptableObject.Instance.isUseAni)
                    ReBuildAnimator(animator);
            }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    public static void ReBuildAnimator(Animator animator)
    {
        var controller = (AnimatorController)animator.runtimeAnimatorController;
        var obj = animator.gameObject;
        var assetPath = AssetDatabase.GetAssetPath(obj);
        
        var dstclippath = new Dictionary<string, AnimationClip>();
        
        var states = controller.layers[0].stateMachine.states;
        for (int i = 0; i < states.Length; i++)
        {
            var path = export(states[i].state.motion, assetPath);
            if (!string.IsNullOrEmpty(path))
            {
                var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
                if (null != clip)
                    dstclippath.Add(clip.name, clip);
            }
            AnimationClip sclip;
            dstclippath.TryGetValue(states[i].state.name, out sclip);
            if (sclip != null)
                states[i].state.motion = sclip;
            states[i].position = new Vector3((int) i / 10, (int)i % 10, 0);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    static string export(Object clipobj, string assetPath)
    {
        AnimationClip srcclip = clipobj as AnimationClip;
        if (srcclip == null)
            return "";

        if (srcclip.name.Contains(INVALID_ANIM_NAME))
            return "";

        FileInfo fileInfo = new FileInfo(System.Environment.CurrentDirectory + "\\" + assetPath);
        var dirInfo = fileInfo.Directory;

        var subDirInfo = new DirectoryInfo(dirInfo.FullName + "\\" + ANIM_DIR);
        if (!subDirInfo.Exists)
            subDirInfo.Create();

        var dstclippath = subDirInfo.FullName.Replace(System.Environment.CurrentDirectory + "\\", "") + "\\" + srcclip.name + ANIM_SUFFIX;
        AnimationClip dstclip = AssetDatabase.LoadAssetAtPath(dstclippath, typeof(AnimationClip)) as AnimationClip;
        if (dstclip != null)
            AssetDatabase.DeleteAsset(dstclippath);

        AnimationClip tempclip = new AnimationClip();
        EditorUtility.CopySerialized(srcclip, tempclip);
        AssetDatabase.CreateAsset(tempclip, dstclippath);
        return dstclippath;
    }
}