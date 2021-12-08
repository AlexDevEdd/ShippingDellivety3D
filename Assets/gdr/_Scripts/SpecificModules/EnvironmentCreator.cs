using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCreator : MonoBehaviour
{
    [Header("Floor Settings")]
    public bool isGenerateNewFloor = false;
    public string customFloorName = "Floor";
    public Vector2 FloorSize = new Vector2(1, 1);

    [Header("Wall Settings")]
    public bool isGenerateNewWalls = false;
    public string customWallsName = "Walls";
    public float wallHeight = 1f;
    public Vector2 wallOffset = new Vector2(1, 1);

    [Header("Room Settings (Wall Settings)")]
    public bool isGenerateNewRoom = false;
    public string customRoomName = "Room";

    [Header("Shared")]
    public int layerID;
    //[Header("Auto Show Current Layer")]
    [NaughtyAttributes.ShowNativeProperty]
    public string SelectedLayer
    {
        get
        { 
            return LayerMask.LayerToName(layerID); 
        }
    }
    public Vector3 spawnPos;
    public Transform spawnPosTr;
    public bool isUseExistingMaterial = true;
    public Material material;
    [Header("Auto")]
    public GameObject planeInstance;
    public GameObject wallsInstance;
    public GameObject roomInstance;

    [NaughtyAttributes.Button]
    private void GenerateFloor()
    {
        if (!isGenerateNewFloor && planeInstance != null)
            DestroyImmediate(planeInstance);
        planeInstance = GameObject.CreatePrimitive(PrimitiveType.Plane);
        planeInstance.name = customFloorName;
        planeInstance.transform.localScale = new Vector3(FloorSize.x, 1f, FloorSize.y);
        if (spawnPosTr != null)
            planeInstance.transform.position = spawnPosTr.position;
        else
            planeInstance.transform.position = spawnPos;
        planeInstance.layer = layerID;
        var mc = planeInstance.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        var collider = planeInstance.GetComponent<BoxCollider>();
        if (collider == null)
            collider = planeInstance.AddComponent<BoxCollider>();
        collider.size = new Vector3(11f, 1f, 11f);
        collider.center = new Vector3(0f, -0.5f, 0f);

        if (isUseExistingMaterial && material != null)
        {
            planeInstance.GetComponent<MeshRenderer>().sharedMaterial = material;
        }
    }

    [NaughtyAttributes.Button]
    private void GenerateWalls()
    {
        if (!isGenerateNewWalls && wallsInstance != null)
            DestroyImmediate(wallsInstance);

        wallsInstance = new GameObject();
        wallsInstance.name = $"{customWallsName}";
        if (spawnPosTr != null)
            wallsInstance.transform.position = spawnPosTr.position;
        else
            wallsInstance.transform.position = spawnPos;

        var wall_1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall_1.name = $"Internal{customWallsName}";
        var wall_2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall_2.name = $"Internal{customWallsName}";
        var wall_3 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall_3.name = $"Internal{customWallsName}";
        var wall_4 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall_4.name = $"Internal{customWallsName}";

        wall_1.transform.localScale = new Vector3(wallHeight / 5f, 1f, wallOffset.y / 5f);
        wall_2.transform.localScale = new Vector3(wallOffset.x / 5f, 1f, wallHeight / 5f);
        wall_3.transform.localScale = new Vector3(wallHeight / 5f, 1f, wallOffset.y / 5f);
        wall_4.transform.localScale = new Vector3(wallOffset.x / 5f, 1f, wallHeight / 5f);

        wall_1.transform.SetParent(wallsInstance.transform);
        wall_2.transform.SetParent(wallsInstance.transform);
        wall_3.transform.SetParent(wallsInstance.transform);
        wall_4.transform.SetParent(wallsInstance.transform);

        wall_1.transform.localPosition = new Vector3(0f, wallHeight, wallOffset.x);
        wall_2.transform.localPosition = new Vector3(wallOffset.y, wallHeight, 0);
        wall_3.transform.localPosition = new Vector3(0f, wallHeight, -wallOffset.x);
        wall_4.transform.localPosition = new Vector3(-wallOffset.y, wallHeight, 0);

        wall_1.transform.rotation = Quaternion.Euler(180f, 270f, 90f);
        wall_2.transform.rotation = Quaternion.Euler(90f, 0f, 90f);
        wall_3.transform.rotation = Quaternion.Euler(180f, 90f, 90f);
        wall_4.transform.rotation = Quaternion.Euler(90f, 0f, 270f);

        wallsInstance.layer = layerID;
        wall_1.layer = layerID;
        wall_2.layer = layerID;
        wall_3.layer = layerID;
        wall_4.layer = layerID;

        var mc = wall_1.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        mc = wall_2.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        mc = wall_3.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        mc = wall_4.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);

        var collider = wall_1.GetComponent<BoxCollider>();
        if (collider == null)
            collider = wall_1.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 1f, 10f + 10f / wallOffset.y);
        collider.center = new Vector3(0f, -0.5f, 0f);

        collider = wall_2.GetComponent<BoxCollider>();
        if (collider == null)
            collider = wall_2.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 1f, 10f);
        collider.center = new Vector3(0f, -0.5f, 0f);

        collider = wall_3.GetComponent<BoxCollider>();
        if (collider == null)
            collider = wall_3.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 1f, 10f + 10f / wallOffset.y);
        collider.center = new Vector3(0f, -0.5f, 0f);

        collider = wall_4.GetComponent<BoxCollider>();
        if (collider == null)
            collider = wall_4.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 1f, 10f);
        collider.center = new Vector3(0f, -0.5f, 0f);

        if (isUseExistingMaterial && material != null)
        {
            wall_1.GetComponent<MeshRenderer>().sharedMaterial = material;
            wall_2.GetComponent<MeshRenderer>().sharedMaterial = material;
            wall_3.GetComponent<MeshRenderer>().sharedMaterial = material;
            wall_4.GetComponent<MeshRenderer>().sharedMaterial = material;
        }
    }

    [NaughtyAttributes.Button]
    private void GenerateRoom()
    {
        if (!isGenerateNewRoom && roomInstance != null)
            DestroyImmediate(roomInstance);

        roomInstance = new GameObject();
        roomInstance.name = $"{customRoomName}";
        if (spawnPosTr != null)
            roomInstance.transform.position = spawnPosTr.position;
        else
            roomInstance.transform.position = spawnPos;

        var wall_1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall_1.name = $"Internal{customRoomName}";
        var wall_2 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall_2.name = $"Internal{customRoomName}";
        var wall_3 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall_3.name = $"Internal{customRoomName}";
        var wall_4 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        wall_4.name = $"Internal{customRoomName}";
        var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = $"Internal{customRoomName}";
        var ceil = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ceil.name = $"Internal{customRoomName}";

        wall_1.transform.localScale = new Vector3(wallHeight / 5f, 1f, wallOffset.y / 5f);
        wall_2.transform.localScale = new Vector3(wallOffset.x / 5f, 1f, wallHeight / 5f);
        wall_3.transform.localScale = new Vector3(wallHeight / 5f, 1f, wallOffset.y / 5f);
        wall_4.transform.localScale = new Vector3(wallOffset.x / 5f, 1f, wallHeight / 5f);

        floor.transform.localScale = new Vector3(wallOffset.x / 5f, 1f, wallOffset.y / 5f);
        ceil.transform.localScale = new Vector3(wallOffset.x / 5f, 1f, wallOffset.y / 5f);

        wall_1.transform.SetParent(roomInstance.transform);
        wall_2.transform.SetParent(roomInstance.transform);
        wall_3.transform.SetParent(roomInstance.transform);
        wall_4.transform.SetParent(roomInstance.transform);

        floor.transform.SetParent(roomInstance.transform);
        ceil.transform.SetParent(roomInstance.transform);

        wall_1.transform.localPosition = new Vector3(0f, wallHeight, wallOffset.x);
        wall_2.transform.localPosition = new Vector3(wallOffset.y, wallHeight, 0);
        wall_3.transform.localPosition = new Vector3(0f, wallHeight, -wallOffset.x);
        wall_4.transform.localPosition = new Vector3(-wallOffset.y, wallHeight, 0);

        floor.transform.localPosition = new Vector3(0, 0, 0);
        ceil.transform.localPosition = new Vector3(0, wallHeight * 2f, 0);

        wall_1.transform.rotation = Quaternion.Euler(180f, 270f, 90f);
        wall_2.transform.rotation = Quaternion.Euler(90f, 0f, 90f);
        wall_3.transform.rotation = Quaternion.Euler(180f, 90f, 90f);
        wall_4.transform.rotation = Quaternion.Euler(90f, 0f, 270f);

        floor.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        ceil.transform.rotation = Quaternion.Euler(180f, 90f, 0f);

        roomInstance.layer = layerID;
        wall_1.layer = layerID;
        wall_2.layer = layerID;
        wall_3.layer = layerID;
        wall_4.layer = layerID;
        floor.layer = layerID;
        ceil.layer = layerID;

        var mc = wall_1.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        mc = wall_2.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        mc = wall_3.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        mc = wall_4.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        mc = floor.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);
        mc = ceil.GetComponent<MeshCollider>();
        if (mc != null)
            DestroyImmediate(mc);

        var collider = wall_1.GetComponent<BoxCollider>();
        if (collider == null)
            collider = wall_1.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 1f, 10f + 10f / wallOffset.y);
        collider.center = new Vector3(0f, -0.5f, 0f);

        collider = wall_2.GetComponent<BoxCollider>();
        if (collider == null)
            collider = wall_2.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 1f, 10f);
        collider.center = new Vector3(0f, -0.5f, 0f);

        collider = wall_3.GetComponent<BoxCollider>();
        if (collider == null)
            collider = wall_3.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 1f, 10f + 10f / wallOffset.y);
        collider.center = new Vector3(0f, -0.5f, 0f);

        collider = wall_4.GetComponent<BoxCollider>();
        if (collider == null)
            collider = wall_4.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f, 1f, 10f);
        collider.center = new Vector3(0f, -0.5f, 0f);

        collider = floor.GetComponent<BoxCollider>();
        if (collider == null)
            collider = floor.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f + 10f / wallOffset.x, 1f, 10f + 10f / wallOffset.y);
        collider.center = new Vector3(0f, -0.5f, 0f);

        collider = ceil.GetComponent<BoxCollider>();
        if (collider == null)
            collider = ceil.AddComponent<BoxCollider>();
        collider.size = new Vector3(10f + 10f / wallOffset.x, 1f, 10f + 10f / wallOffset.y);
        collider.center = new Vector3(0f, -0.5f, 0f);

        if (isUseExistingMaterial && material != null)
        {
            wall_1.GetComponent<MeshRenderer>().sharedMaterial = material;
            wall_2.GetComponent<MeshRenderer>().sharedMaterial = material;
            wall_3.GetComponent<MeshRenderer>().sharedMaterial = material;
            wall_4.GetComponent<MeshRenderer>().sharedMaterial = material;
            floor.GetComponent<MeshRenderer>().sharedMaterial = material;
            ceil.GetComponent<MeshRenderer>().sharedMaterial = material;
        }
    }
}
