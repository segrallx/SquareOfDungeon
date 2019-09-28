using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;


[CustomEditor(typeof(CharRender))]
public class CharRenderInspector : Editor
{

    [Space(10)]
    CharRender charRender;

    void OnEnable()
    {
        //获取当前编辑自定义Inspector的对象
        charRender = (CharRender)target;
    }


    public override void OnInspectorGUI()
    {

        var state = charRender.mChar.mState;
        this.serializedObject.Update();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Tiled Info");

        EditorGUILayout.Vector3Field("mMovePos", state.mMovePos);
        EditorGUILayout.Vector2IntField("mCurIdx", state.mCurIdx);

        EditorGUILayout.EndVertical();
        this.serializedObject.ApplyModifiedProperties();

    }
}
