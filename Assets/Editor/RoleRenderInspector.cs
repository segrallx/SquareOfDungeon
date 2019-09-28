using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;



// [CustomEditor(typeof(RoleRender))]
// public class RoleRenderInspector : Editor
// {

//     [Space(10)]
//     RoleRender roleRender;

//     void OnEnable()
//     {
//         //获取当前编辑自定义Inspector的对象
//         roleRender = (RoleRender)target;
//     }


//     public override void OnInspectorGUI()
//     {

//         this.serializedObject.Update();

//         EditorGUILayout.BeginVertical(GUI.skin.box);
//         EditorGUILayout.LabelField("Role Info");
//         EditorGUILayout.TextArea(roleRender.InspectInfo());

//         EditorGUILayout.EndVertical();
//         this.serializedObject.ApplyModifiedProperties();

//     }
// }
