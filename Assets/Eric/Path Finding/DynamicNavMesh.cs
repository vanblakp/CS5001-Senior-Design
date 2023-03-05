using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;

public class DynamicNavMesh : MonoBehaviour
{
    public NavMeshSurface Surface2D;
    public float update = 10f;
    public bool continueUpdating = true;

    // Start is called before the first frame update
    void Start()
    {
        Surface2D.BuildNavMeshAsync();
        //StartCoroutine(NavMeshUpdate());
    }

    IEnumerator NavMeshUpdate()
    {
        while (continueUpdating)
        {
            Surface2D.UpdateNavMesh(Surface2D.navMeshData);
            yield return new WaitForSeconds(update);
        }
    }

    public void UpdateNavigation()
    {
        Surface2D.UpdateNavMesh(Surface2D.navMeshData);
    }

    // Using this function causes frame rate issues so a coroutine is used instead
    void Update()
    {
        //Surface2D.UpdateNavMesh(Surface2D.navMeshData);
        // NavMeshBuilder.UpdateNavMeshDataAsync(data, GetBuildSettings(), sources, sourcesBounds);
    }
}
