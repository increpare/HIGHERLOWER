using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System;

public class BuildStandaloneScript : MonoBehaviour {

	static void DisableAnalytics(){
		var type = Type.GetType( "UnityEditor.PlayerSettings,UnityEditor" );
		if ( type != null )
		{
			var propertyInfo = type.GetProperty( "submitAnalytics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static );
			if ( propertyInfo != null )
			{
				{
					var value = (bool)propertyInfo.GetValue( null, null );
					UnityEngine.Debug.LogFormat( "PlayerSettings.submitAnalytics {0}", value );
				}
				if ( propertyInfo.CanWrite )
				{
					propertyInfo.SetValue( null, false, null );

					var value = (bool)propertyInfo.GetValue( null, null );
					UnityEngine.Debug.LogFormat( "PlayerSettings.submitAnalytics {0}", value );
				}
			}
		}
	}

	[MenuItem ("File/Build All")]
	static void DoSomething () {
		PlayerSettings.showUnitySplashScreen = false;
		
		DisableAnalytics();

        var pname = PlayerSettings.productName;


		BuildDirectories ();
		
		var icons = PlayerSettings.GetIconsForTargetGroup (BuildTargetGroup.Standalone);

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "");
		string[] levels = EditorBuildSettings.scenes.Select (s => s.path).ToArray ();
        print ("building win32");
        BuildPipeline.BuildPlayer (levels, "../Build/" + pname + "/Win/Win_32/" + pname + "/" + pname + ".exe", BuildTarget.StandaloneWindows, BuildOptions.None);
		print ("building win64");
		BuildPipeline.BuildPlayer (levels, "../Build/" + pname + "/Win/Win_64/" + pname + "/" + pname + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
		print ("building linux");
		BuildPipeline.BuildPlayer (levels, "../Build/" + pname + "/Linux/" + pname + "/" + pname, BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);
        print ("building osx");
        BuildPipeline.BuildPlayer (levels, "../Build/"+pname+"/OSX/"+pname, BuildTarget.StandaloneOSX,BuildOptions.None);

        if (File.Exists("readme.txt")==false){
            File.WriteAllText("readme.txt","from https://www.increpare.com. \n\nThank you very much for playing :) \n\nConsider donating some cash to https://www.patreon.com/increpare if you want to support my work further.");
        }


        File.Copy("readme.txt", "../Build/" + pname + "/Win/Win_32/" + pname + "/readme.txt", true);
        File.Copy("readme.txt", "../Build/" + pname + "/Win/Win_64/" + pname + "/readme.txt", true);
        File.Copy("readme.txt", "../Build/" + pname + "/Linux/" + pname + "/readme.txt", true);
        File.Copy("readme.txt", "../Build/" + pname + "/OSX/readme.txt", true);


        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "STEAM");


        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "");

		try
		{
			File.Delete("../Build/"+pname+"_linux.zip");
		}
		catch(System.Exception e){
		}
		
		try
		{
			File.Delete("../Build/"+pname+"_osx.zip");
		}
		catch(System.Exception e){
		}
		
		try
		{
			File.Delete("../Build/"+pname+"_win32.zip");
		}
		catch(System.Exception e){
		}
		
		
		try
		{
			File.Delete("../Build/"+pname+"_win64.zip");
		}
		catch(System.Exception e){
		}
		
		try
		{
			File.Delete("../Build/"+pname+"_src.zip");
		}
		catch(System.Exception e){
		}
		//var escapedpname = pname.Replace(" ","\\ ");

		ProcessStartInfo startInfo;


		var dir = new DirectoryInfo("../Build/"+pname+"/Win/Win_64/"+pname);
		
		foreach (var file in dir.GetFiles("*.pdb")) {
			print (file.Name);
			file.Delete();
		}
		
		
		dir = new DirectoryInfo("../Build/"+pname+"/Win/Win_32/"+pname);
		
		foreach (var file in dir.GetFiles("*.pdb")) {
			print (file.Name);
			file.Delete();
		}


		print ("compressing linux");
		startInfo = new ProcessStartInfo("/usr/bin/tar");
		startInfo.WorkingDirectory = "../Build/"+pname+"/Linux";
		startInfo.Arguments = "-czvf \"../" + pname + "_linux.tar.gz\" \"" + pname+"\"";
		Process.Start(startInfo).WaitForExit();        
		
		print ("compressing osx");
		startInfo = new ProcessStartInfo("/usr/bin/zip");
		startInfo.WorkingDirectory = "../Build/"+pname+"/OSX";
		startInfo.Arguments = "-r \"../" + pname + "_osx.zip\" \"" + pname + ".app\"";
		Process.Start(startInfo).WaitForExit();
		
		print ("compressing windows");
		startInfo = new ProcessStartInfo("/usr/bin/zip");
		startInfo.WorkingDirectory = "../Build/"+pname+"/Win/Win_32";
		startInfo.Arguments = "-r \"../../" + pname + "_win32.zip\" \"" + pname +"\"";
		Process.Start(startInfo).WaitForExit();
		
		print ("compressing win");
		startInfo = new ProcessStartInfo("/usr/bin/zip");
		startInfo.WorkingDirectory = "../Build/"+pname+"/Win/Win_64";
		startInfo.Arguments = "-r \"../../" + pname + "_win64.zip\" \"" + pname +"\"";
		Process.Start(startInfo).WaitForExit();
		
            
		print ("compressing src");
		dir = new DirectoryInfo(Application.dataPath+"/../");
		var dirName = dir.Name;
		startInfo = new ProcessStartInfo("/usr/bin/zip");
		startInfo.WorkingDirectory = Application.dataPath+"/../../";
		startInfo.Arguments = "-r \"Build/" +pname+"/"+ pname + "_src.zip\" " + dirName;
		Process.Start(startInfo).WaitForExit();
		print ("done");
	}
	
	static void BuildDirectories(){
		
        var pname = PlayerSettings.productName;

	
		Directory.CreateDirectory ("../Build");
		Directory.CreateDirectory ("../Build/" + pname);
		Directory.CreateDirectory ("../Build/" + pname + "/OSX");
		Directory.CreateDirectory ("../Build/" + pname + "/Win");
		Directory.CreateDirectory ("../Build/" + pname + "/Win/Win_32");
		Directory.CreateDirectory ("../Build/" + pname + "/Win/Win_64");
		Directory.CreateDirectory ("../Build/" + pname + "/Win/Win_32/" + pname);
		Directory.CreateDirectory ("../Build/" + pname + "/Win/Win_64/" + pname);
		Directory.CreateDirectory ("../Build/" + pname + "/Linux/" + pname);
		//	System.Diagnostics.Process.Start(
		
		
	}
}
