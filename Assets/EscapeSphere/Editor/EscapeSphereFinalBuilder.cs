#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class EscapeSphereFinalBuilder
{
    const string Folder="Assets/Scenes";

    [MenuItem("Escape Sphere/Generar proyecto final completo")]
    public static void Generate()
    {
        string source=Folder+"/Nivel 2.unity";
        if(!File.Exists(source)){ EditorUtility.DisplayDialog("Escape Sphere","No se encontro Assets/Scenes/Nivel 2.unity","Aceptar"); return; }
        PatchLevel2();
        CreateStage(source,Folder+"/Nivel 3.unity",3,135,"Nivel 4",2,10);
        CreateStage(source,Folder+"/Nivel 4.unity",4,105,"WinScene",3,14);
        ConfigureBuild();
        AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Escape Sphere","Proyecto generado. Realiza Bake en NavMeshSurface de Nivel 3 y Nivel 4, guarda y prueba desde MainMenu.","Aceptar");
    }

    static void PatchLevel2()
    {
        Scene scene=EditorSceneManager.OpenScene(Folder+"/Nivel 2.unity",OpenSceneMode.Single);
        FinalGameManager old=Object.FindFirstObjectByType<FinalGameManager>();
        if(old!=null) old.nextSceneName="Nivel 3";
        EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene);
    }

    static void CreateStage(string source,string target,int number,float time,string next,int enemies,int cores)
    {
        if(File.Exists(target)) AssetDatabase.DeleteAsset(target);
        AssetDatabase.CopyAsset(source,target); AssetDatabase.Refresh();
        Scene scene=EditorSceneManager.OpenScene(target,OpenSceneMode.Single);
        RemoveGenerated();
        DisableOldManagers();
        BuildArena(number);
        BuildTraps(number);
        ReplaceCollectibles(cores);
        SetupEnemies(enemies);
        BuildExit();
        GameObject rules=new GameObject("Control de nivel");
        StageController sc=rules.AddComponent<StageController>(); sc.timeLimit=time; sc.nextScene=next;
        AddLights(number);
        EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene,target);
    }

    static void RemoveGenerated()
    {
        foreach(GameObject go in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            if(go.name.StartsWith("Final - ")) Object.DestroyImmediate(go);
    }

    static void DisableOldManagers()
    {
        FinalGameManager old=Object.FindFirstObjectByType<FinalGameManager>();
        if(old!=null) old.enabled=false;
        foreach(FinalCollectible item in Object.FindObjectsByType<FinalCollectible>(FindObjectsSortMode.None)) Object.DestroyImmediate(item.gameObject);
    }

    static Material Material(string name,Color color,float emission=0)
    {
        string dir="Assets/EscapeSphere/Materials";
        if(!AssetDatabase.IsValidFolder(dir)) AssetDatabase.CreateFolder("Assets/EscapeSphere","Materials");
        string path=dir+"/"+name+".mat";
        Material m=AssetDatabase.LoadAssetAtPath<Material>(path);
        if(m==null){ Shader s=Shader.Find("Universal Render Pipeline/Lit"); if(s==null)s=Shader.Find("Standard"); m=new Material(s); AssetDatabase.CreateAsset(m,path); }
        m.color=color;
        if(emission>0){ m.EnableKeyword("_EMISSION"); m.SetColor("_EmissionColor",color*emission); }
        EditorUtility.SetDirty(m); return m;
    }

    static GameObject Cube(string name,Vector3 p,Vector3 s,Material m)
    {
        GameObject go=GameObject.CreatePrimitive(PrimitiveType.Cube); go.name=name; go.transform.position=p; go.transform.localScale=s; go.GetComponent<Renderer>().sharedMaterial=m; return go;
    }

    static void BuildArena(int n)
    {
        Material wall=Material("Muros",new Color(.08f,.13f,.22f));
        GameObject root=new GameObject("Final - Sector "+n);
        Vector3[] p={new(0,1.5f,10),new(0,1.5f,-10),new(10,1.5f,0),new(-10,1.5f,0),new(-3,1.5f,2),new(4,1.5f,-2),new(0,1.5f,6)};
        Vector3[] s={new(20,3,.5f),new(20,3,.5f),new(.5f,3,20),new(.5f,3,20),new(.5f,3,10),new(.5f,3,10),new(10,3,.5f)};
        for(int i=0;i<p.Length;i++){ GameObject w=Cube("Muro "+(i+1),p[i],s[i],wall); w.transform.SetParent(root.transform); }
    }

    static void BuildTraps(int n)
    {
        Material red=Material("Peligro",new Color(1,.03f,.03f),2);
        for(int i=0;i<n+2;i++){
            GameObject t=Cube("Final - Trampa movil "+(i+1),new Vector3(-7+i*3,0.75f,(i%2==0)?-4:4),new Vector3(1.2f,1.5f,1.2f),red);
            MovingTrap mt=t.AddComponent<MovingTrap>(); mt.travel=new Vector3(0,0,3+i%3); mt.speed=1.1f+i*.15f; t.AddComponent<PlayerResetTrap>();
        }
        GameObject r=Cube("Final - Rotor",new Vector3(0,.6f,-7),new Vector3(8,.35f,.45f),red); r.AddComponent<RotatingTrap>(); r.AddComponent<PlayerResetTrap>();
    }

    static void ReplaceCollectibles(int count)
    {
        Material gold=Material("Energia",new Color(1,.65f,.03f),1.8f);
        for(int i=0;i<count;i++){
            float a=i*Mathf.PI*2/count; GameObject c=GameObject.CreatePrimitive(PrimitiveType.Sphere); c.name="Nucleo de energia "+(i+1); c.tag="Collectible";
            c.transform.position=new Vector3(Mathf.Cos(a)*(7-i%3),1,Mathf.Sin(a)*(7-i%2)); c.transform.localScale=Vector3.one*.75f; c.GetComponent<Renderer>().sharedMaterial=gold;
            c.GetComponent<Collider>().isTrigger=true; Rigidbody rb=c.AddComponent<Rigidbody>(); rb.isKinematic=true; rb.useGravity=false; c.AddComponent<EnergyCore>();
        }
    }

    static void SetupEnemies(int count)
    {
        GameObject original=GameObject.Find("Enemigo"); if(original==null) original=GameObject.Find("Guardian_1"); if(original==null)return;
        original.name="Guardian 1";
        for(int i=1;i<count;i++){ GameObject e=Object.Instantiate(original); e.name="Guardian "+(i+1); e.transform.position=new Vector3(-6+i*6,.6f,(i%2==0)?6:-6); }
    }

    static void BuildExit()
    {
        Material green=Material("Salida",new Color(.03f,1,.3f),2);
        GameObject exit=Cube("Final - Portal de salida",new Vector3(0,1,8.7f),new Vector3(3,2,.4f),green); exit.GetComponent<Collider>().isTrigger=true; exit.AddComponent<StageExit>();
    }

    static void AddLights(int n)
    {
        Color[] c={new(.1f,.45f,1),new(1,.08f,.08f),new(.55f,.08f,1)};
        for(int i=0;i<3;i++){ GameObject g=new GameObject("Final - Luz ambiental "+(i+1)); g.transform.position=new Vector3(-6+i*6,4,(i%2==0)?-4:4); Light l=g.AddComponent<Light>(); l.type=LightType.Point; l.color=c[(i+n)%3]; l.intensity=4; l.range=12; l.shadows=LightShadows.Soft; }
        RenderSettings.fog=true; RenderSettings.fogColor=new Color(.025f,.04f,.075f); RenderSettings.fogDensity=.014f;
    }

    static void ConfigureBuild()
    {
        string[] p={Folder+"/MainMenu.unity",Folder+"/Nivel 1.unity",Folder+"/Nivel 2.unity",Folder+"/Nivel 3.unity",Folder+"/Nivel 4.unity",Folder+"/WinScene.unity"};
        EditorBuildSettingsScene[] s=new EditorBuildSettingsScene[p.Length]; for(int i=0;i<p.Length;i++)s[i]=new EditorBuildSettingsScene(p[i],true); EditorBuildSettings.scenes=s;
    }
}
#endif
