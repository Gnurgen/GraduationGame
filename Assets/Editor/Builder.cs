using UnityEditor;
using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

public class Builder : MonoBehaviour {

	static void AndroidBuild(){

		string basePath = "C:/Users/dadiu/Google Drive/GraduationGames/Builds";
		string buildFolder = System.DateTime.Now.ToString ("dd-MM-yy HH.mm.ss");
		string initScene = "Splash.unity";
		string title = "SpilletMedEnGiantHvorManErLille";
		string studio = "highfiveproductions";


		try{

			PlayerSettings.bundleIdentifier = "com." + studio + "." + title;
			PlayerSettings.bundleVersion = "2.2"; 

			// All of this is to find every scene in the build folder, put the StartScene
			// as the first scene, and add the rest.
			string[] scenes = Directory.GetFiles("C:/workspace/Assets/Scenes/Building/"); // find all the scenes for building

			for(int i = 0; i < scenes.Length; i++){
				scenes[i] = extractFile(scenes[i]); // Remove the path up to the file, so only the file name remains
			}

			List<string> allScenes = new List<string>(scenes); // Make a list holding all the scenes
			if(!allScenes.Contains(initScene)){
				throw new UnityException("There is no init scene");
			}
			allScenes.RemoveAll(str => str.Contains(".meta"));
			allScenes.Remove(initScene);
			allScenes.Insert(0,initScene);
			string[] buildScenes = allScenes.ToArray();
			for(int i = 0; i < buildScenes.Length; i++){
				buildScenes[i] = "Assets/Scenes/Building/" + buildScenes[i];
			}
			FileUtil.DeleteFileOrDirectory ("C:/Users/dadiu/AppData/LocalUnity/Editor/Editor.log");

			Directory.CreateDirectory (basePath + "/" + buildFolder);

			BuildPipeline.BuildPlayer (buildScenes, basePath + "/" + buildFolder + "/" + "build.apk" , BuildTarget.Android, BuildOptions.None);

			FileUtil.CopyFileOrDirectory ("C:/Users/dadiu/AppData/Local/Unity/Editor/Editor.log", basePath + "/" + buildFolder + "/log.txt");
		} catch(UnityException e){
			StreamWriter fil = new StreamWriter(basePath + "/" + buildFolder + "/unity_errors.txt",true);
			fil.Write (e.Message);
			fil.Close ();
		} catch(Exception e){
			StreamWriter fil = new StreamWriter(basePath + "/" + buildFolder + "/general_errors.txt",true);
			fil.Write (e.Message);
			fil.Close ();
		}
	}

	static string extractFile(string path){
		char[] delim = { '\\', '/' };
		string[] temp = path.Split (delim);
		string file = temp [temp.Length - 1];
		return file;
	}
}
