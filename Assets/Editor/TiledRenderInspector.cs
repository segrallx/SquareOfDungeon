using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

[CustomEditor(typeof(TiledRender))]
public class TiledRenderInspector : Editor
{

	[Space(10)]

    TiledRender tiledRender;

    void OnEnable()
    {
        //获取当前编辑自定义Inspector的对象
        tiledRender = (TiledRender)target;
    }

    public override void OnInspectorGUI()
    {

		this.serializedObject.Update();

		EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Tiled Info");

		EditorGUILayout.IntField("X", tiledRender.mTiled.mX);
		EditorGUILayout.IntField("Y", tiledRender.mTiled.mY);

        bool change = false;
        change = change || tiledRender.SetMCls((Tiled.Cls)EditorGUILayout.EnumPopup("Cls",
																					tiledRender.mTiled.mCls));
        change = change || tiledRender.SetMId(EditorGUILayout.IntField("Id", tiledRender.mTiled.mId));
		//change = change || tiledRender.SetOccupy(EditorGUILayout.("Id", tiledRender.mTiled.mId));

        if (change)
        {
            WriteBack();
        }


		EditorGUILayout.EndVertical();
		this.serializedObject.ApplyModifiedProperties();

    }


    void WriteBack()
    {
        if (tiledRender.mMazeRender == null)
        {
            return;
        }


		Debug.LogFormat("Write back to Assert");
        //var maze = tiledRender.mMazeRender.mMaze;
		// FileStream file = new FileStream("./tmp_home.txt",  FileMode.Create);
		// file.Seek(0, SeekOrigin.Begin);
		// var data =Encoding.ASCII.GetBytes(maze.ToJson());
		// file.Write(data, 0, data.Length);
        //var txtAsset = maze.mtxtAsset;
        //File.WriteAllText(AssetDatabase.GetAssetPath(txtAsset), maze.ToJson());
        //EditorUtility.SetDirty(txtAsset);
    }
}
