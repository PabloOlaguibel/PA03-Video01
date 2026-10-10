#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public static class PA04FinalInstaller
{
    [MenuItem("PA04 Final/Aplicar mejoras a Nivel 2")]
    public static void Apply()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (!scene.name.Contains("Nivel 2"))
        {
            EditorUtility.DisplayDialog("PA04 Final", "Abre la escena Nivel 2 y vuelve a ejecutar esta opcion.", "Aceptar");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject enemy = GameObject.Find("Enemigo");
        if (player == null || enemy == null)
        {
            EditorUtility.DisplayDialog("PA04 Final", "No se encontro Player o Enemigo en Nivel 2.", "Aceptar");
            return;
        }

        var oldEnemy = enemy.GetComponent<Enemigo>();
        if (oldEnemy != null) Object.DestroyImmediate(oldEnemy);
        var ai = enemy.GetComponent<EnemyPatrolChase>();
        if (ai == null) ai = enemy.AddComponent<EnemyPatrolChase>();
        ai.player = player.transform;

        GameObject root = GameObject.Find("PA04_Waypoints");
        if (root == null) root = new GameObject("PA04_Waypoints");
        Vector3 center = enemy.transform.position;
        Vector3[] offsets = { new Vector3(-6,0,-6), new Vector3(6,0,-6), new Vector3(6,0,6), new Vector3(-6,0,6) };
        Transform[] points = new Transform[4];
        for (int i=0; i<4; i++)
        {
            Transform t = root.transform.Find("Waypoint_" + (i+1));
            if (t == null) { var go = new GameObject("Waypoint_" + (i+1)); go.transform.SetParent(root.transform); t = go.transform; }
            t.position = center + offsets[i]; points[i] = t;
        }
        ai.waypoints = points;

        var controller = player.GetComponent<Jugador>();
        if (controller != null) Object.DestroyImmediate(controller);
        if (player.GetComponent<PlayerControllerFinal>() == null) player.AddComponent<PlayerControllerFinal>();

        GameObject gm = GameObject.Find("PA04_FinalGameManager");
        if (gm == null) gm = new GameObject("PA04_FinalGameManager");
        if (gm.GetComponent<FinalGameManager>() == null) gm.AddComponent<FinalGameManager>();

        foreach (GameObject collectible in GameObject.FindGameObjectsWithTag("Collectible"))
        {
            var old = collectible.GetComponent<Collectible>();
            if (old != null) Object.DestroyImmediate(old);
            Collider col = collectible.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
            Rigidbody body = collectible.GetComponent<Rigidbody>();
            if (body != null) { body.isKinematic = true; body.useGravity = false; }
            if (collectible.GetComponent<FinalCollectible>() == null) collectible.AddComponent<FinalCollectible>();
        }

        Light sun = Object.FindFirstObjectByType<Light>();
        if (sun != null && sun.type == LightType.Directional) { sun.intensity = 1.25f; sun.shadows = LightShadows.Soft; }
        CreatePointLight("PA04_LuzAzul", new Vector3(-6,4,-6), new Color(0.2f,0.55f,1f));
        CreatePointLight("PA04_LuzRoja", new Vector3(6,4,6), new Color(1f,0.2f,0.2f));

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        EditorUtility.DisplayDialog("PA04 Final", "Mejoras aplicadas: patrulla, persecucion, control fisico, coleccionables trigger e iluminacion.", "Aceptar");
    }

    static void CreatePointLight(string name, Vector3 position, Color color)
    {
        GameObject go = GameObject.Find(name);
        if (go == null) go = new GameObject(name);
        go.transform.position = position;
        Light light = go.GetComponent<Light>();
        if (light == null) light = go.AddComponent<Light>();
        light.type = LightType.Point; light.color = color; light.intensity = 4f; light.range = 12f; light.shadows = LightShadows.Soft;
    }
}
#endif
