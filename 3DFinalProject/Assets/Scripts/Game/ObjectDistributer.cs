using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistributer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private GameObject WorldSpace;
    [SerializeField]
    private GameObject[] SpawnZones;
    [SerializeField]
    private GameObject GodTempleFloor;
    [SerializeField]
    private GameObject[] GhostTempleFloors;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private GameObject GM;

    [Header("Item Prefabs")]
    [SerializeField]
    private GameObject CharmPrefab;
    [SerializeField]
    private GameObject IncensePrefab;
    [SerializeField]
    private GameObject DivinationBlockPrefab;

    [Header("Remnant Prefabs")]
    [SerializeField]
    private GameObject[] RemnantsPrefabs = new GameObject[(int)GlobalVar.NUM_REMNANT_TYPE];

    [Header("Deadbody Prefabs")]
    [SerializeField]
    private GameObject[] DeadbodyPrefabs = new GameObject[(int)GlobalVar.NUM_GHOST_TYPE];

    [Header("Parameters")]
    [SerializeField]
    private float ForbiddenWidthFromPlayer = 12f;   // set a width to prevent items spawn from player
    [SerializeField]
    private float ForbiddenLengthFromPlayer = 12f;  // set a length to prevent items spawn from player
    [SerializeField]
    private float templeBorderOffset = 3f;  // set an offset for each spawned item to avoid spawn in walls
    [SerializeField]
    private float ghostTempleBorderOffset = 3f;  // set an offset for each spawned item to avoid spawn in walls
    [SerializeField]
    private int NumWrongRemnants = 2;
    [SerializeField]
    private int CharmNum = 10;
    [SerializeField]
    private int IncenseNum = 10;
    [SerializeField]
    private int DivinationBlockNum = 10;
    [SerializeField]
    private float dropHeightOffest;
    [SerializeField]
    private float dropHeightTempleOffest;
    [SerializeField]
    private float dropHeightGhostTempleOffest;

    // define areas with rectangle shape, the first val is top left, the second val is bottom right
    // we view it as 2D so y is always 0
    private Vector3[] mapArea = new Vector3[2];
    private Vector3[] godTempleArea = new Vector3[2];
    private Vector3[] ghostTempleArea = new Vector3[2]
;    private Vector3[] forbiddenArea = new Vector3[2];

    // the correct ghost temple
    private int correctGhostTempleIndex;

    // the correct remnants
    private int[] correctRemnantsID;

    // the correct deadbody
    private int correctDeadbodyID;

    // Start is called before the first frame update
    void Start()
    {
        correctRemnantsID = new int[(int)GlobalVar.NUM_REMNANT_CATEGORY];
        // call this from Game Manager:
         _DeepCopyArray(correctRemnantsID, GM.GetComponent<GameManager>().GetCorrectRemnants());
        correctDeadbodyID = GM.GetComponent<GameManager>().GetCorrectDeadbody();
        correctGhostTempleIndex = GM.GetComponent<GameManager>().GetCorrectGhostTempleIndex();

        CalculateAreas();
        //Debuger();

        DistributeItems();
        DistributeRemnants();
        DistributeDeadbody();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CalculateAreas()
    {
        Vector3 centralPoint;
        float width;
        float length;
        float height;

        // calculate map first
        width = WorldSpace.GetComponent<Terrain>().terrainData.size.x;
        length = WorldSpace.GetComponent<Terrain>().terrainData.size.z;
        centralPoint = WorldSpace.transform.position + new Vector3(width / 2, 0, length / 2);  // because terrain start from top right corner

        mapArea[0] = new Vector3(centralPoint.x - width / 2, 0, centralPoint.z - length / 2);
        mapArea[1] = new Vector3(centralPoint.x + width / 2, 0, centralPoint.z + length / 2);

        // calculate Spawn Zone

        // calculate God temple area
        centralPoint = GodTempleFloor.transform.position;
        width = GodTempleFloor.GetComponent<MeshRenderer>().bounds.size.x;
        length = GodTempleFloor.GetComponent<MeshRenderer>().bounds.size.z;
        height = GodTempleFloor.GetComponent<MeshRenderer>().bounds.size.y / 2;

        godTempleArea[0] = new Vector3(centralPoint.x - width / 2, GodTempleFloor.transform.position.y + height, centralPoint.z - length / 2);
        godTempleArea[1] = new Vector3(centralPoint.x + width / 2, GodTempleFloor.transform.position.y + height, centralPoint.z + length / 2);

        // calculate Ghost temple area
        centralPoint = GhostTempleFloors[correctGhostTempleIndex].transform.position;
        width = GhostTempleFloors[correctGhostTempleIndex].GetComponent<MeshRenderer>().bounds.size.x;
        length = GhostTempleFloors[correctGhostTempleIndex].GetComponent<MeshRenderer>().bounds.size.z;
        height = GhostTempleFloors[correctGhostTempleIndex].GetComponent<MeshRenderer>().bounds.size.y / 2;

        ghostTempleArea[0] = new Vector3(centralPoint.x - width / 2, GhostTempleFloors[correctGhostTempleIndex].transform.position.y + height, centralPoint.z - length / 2);
        ghostTempleArea[1] = new Vector3(centralPoint.x + width / 2, GhostTempleFloors[correctGhostTempleIndex].transform.position.y + height, centralPoint.z + length / 2);

        // calculate forbidden area
        centralPoint = Player.transform.position;
        width = ForbiddenWidthFromPlayer;
        length = ForbiddenLengthFromPlayer;

        forbiddenArea[0] = new Vector3(centralPoint.x - width / 2, 0, centralPoint.z - length / 2);
        forbiddenArea[1] = new Vector3(centralPoint.x + width / 2, 0, centralPoint.z + length / 2);
    }

    private void DistributeItems()
    {
        // first, distribute items used outside temples
        // Charm
        SpawnOutsideItems(CharmPrefab, CharmNum);

        // Incense
        SpawnOutsideItems(IncensePrefab, IncenseNum);

        // distribute items used inside temples
        SpawnGodTempleItems(DivinationBlockPrefab, DivinationBlockNum);
    }

    private void DistributeRemnants()
    {
        // first, pick a random Category to spawn in ghost temple
        int inTempleCategoryID = Random.Range(0, (int)GlobalVar.NUM_REMNANT_CATEGORY);

        // start spawning each correct remnant
        for (int i = 0; i < (int)GlobalVar.NUM_REMNANT_CATEGORY; i++)  // for each category
        {
            if (i == inTempleCategoryID)  // spawn in ghost temple
            {
                SpawnGhostTempleItems(RemnantsPrefabs[correctRemnantsID[i]], 1);
            }
            else                         // spawn elsewhere
            {
                SpawnOutsideItems(RemnantsPrefabs[correctRemnantsID[i]], 1);
            }
        }

        int[] wrongRemnantList = new int[NumWrongRemnants];

        // start spawning wrong remnants
        for (int i = 0; i < NumWrongRemnants; i++)
        {
            int wrongRemnantID = Random.Range(0, RemnantsPrefabs.Length);

            // check if it's the wrong remnant
            while (true)
            {
                bool isWrong = true;

                for (int j = 0; j < (int)GlobalVar.NUM_REMNANT_CATEGORY; j++)  // check for each category of correct remnant
                {
                    if (wrongRemnantID == correctRemnantsID[j])
                    {
                        isWrong = false;
                    }
                }

                if (isWrong)
                {
                    // after chacking, this remnant is not the correct one

                    // now check if the wrong remnant is already spawned before
                    bool isNew = true;

                    for(int j = 0; j < i; j++)
                    {
                        if(wrongRemnantID == wrongRemnantList[j])
                        {
                            // unlucky, we get the same fake remnant
                            isNew = false;

                            break;
                        }
                    }

                    if (isNew)
                    {
                        // congrats!! find the new fake remnant
                        break;
                    }
                    else
                    {
                        wrongRemnantID = Random.Range(0, RemnantsPrefabs.Length);
                    }
                }
                else
                {
                    wrongRemnantID = Random.Range(0, RemnantsPrefabs.Length);
                }
            }   

            SpawnOutsideItems(RemnantsPrefabs[wrongRemnantID], 1);
            wrongRemnantList[i] = wrongRemnantID;
        }
    }

    private void DistributeDeadbody()
    {
        SpawnOutsideItems(DeadbodyPrefabs[correctDeadbodyID - 1], 1);
    }

    private void SpawnOutsideItems(GameObject _prefab, int _prefabNum)
    {
        float spawnX;
        float spawnZ;

        for (int i = 0; i < _prefabNum; i++)
        {
            spawnX = Random.Range(mapArea[0].x, mapArea[1].x);
            spawnZ = Random.Range(mapArea[0].z, mapArea[1].z);

            while (!CheckOutsideTemples(spawnX, spawnZ) || !CheckInSpawnZones(spawnX, spawnZ))
            {
                // re-choose a spawn point
                spawnX = Random.Range(mapArea[0].x, mapArea[1].x);
                spawnZ = Random.Range(mapArea[0].z, mapArea[1].z);
            }

            Instantiate(_prefab, new Vector3(spawnX, dropHeightOffest, spawnZ), transform.rotation);
        }
    }

    private void SpawnGodTempleItems(GameObject _prefab, int _prefabNum)
    {
        float spawnX;
        float spawnZ;

        for (int i = 0; i < _prefabNum; i++)
        {
            // add border offset to avoid spawn in walls, inward borders
            // templeTL(BR) stands for god temple top left(botton right)
            float templeTL_x = godTempleArea[0].x + templeBorderOffset;
            float templeTL_z = godTempleArea[0].z + templeBorderOffset;
            float templeBR_x = godTempleArea[1].x - templeBorderOffset;
            float templeBR_z = godTempleArea[1].z - templeBorderOffset;

            spawnX = Random.Range(templeTL_x, templeBR_x);
            spawnZ = Random.Range(templeTL_z, templeBR_z);

            Instantiate(_prefab, new Vector3(spawnX, godTempleArea[0].y + dropHeightTempleOffest, spawnZ), transform.rotation);
        }
    }

    private void SpawnGhostTempleItems(GameObject _prefab, int _prefabNum)
    {
        float spawnX;
        float spawnZ;

        for (int i = 0; i < _prefabNum; i++)
        {
            // add border offset to avoid spawn in walls, inward borders
            // templeTL(BR) stands for god temple top left(botton right)
            float templeTL_x = ghostTempleArea[0].x + ghostTempleBorderOffset;
            float templeTL_z = ghostTempleArea[0].z + ghostTempleBorderOffset;
            float templeBR_x = ghostTempleArea[1].x - ghostTempleBorderOffset;
            float templeBR_z = ghostTempleArea[1].z - ghostTempleBorderOffset;

            spawnX = Random.Range(templeTL_x, templeBR_x);
            spawnZ = Random.Range(templeTL_z, templeBR_z);

            Instantiate(_prefab, new Vector3(spawnX, ghostTempleArea[0].y + dropHeightGhostTempleOffest, spawnZ), transform.rotation);
        }
    }

    private bool CheckOutsideTemples(float spawnX, float spawnZ)
    {
        // add border offset to avoid spawn in walls, outward borders
        // templeTL(BR) stands for god temple top left(botton right)
        float templeTL_x = godTempleArea[0].x - templeBorderOffset;
        float templeTL_z = godTempleArea[0].z - templeBorderOffset;
        float templeBR_x = godTempleArea[1].x + templeBorderOffset;
        float templeBR_z = godTempleArea[1].z + templeBorderOffset;

        // check in god temple
        if (spawnX > templeTL_x && spawnZ > templeTL_z && spawnX < templeBR_x && spawnZ < templeBR_z)
        {
            return false;
        }

        // gtempleTL(BR) stands for ghost temple top left(botton right)
        float gtempleTL_x = ghostTempleArea[0].x - ghostTempleBorderOffset;
        float gtempleTL_z = ghostTempleArea[0].z - ghostTempleBorderOffset;
        float gtempleBR_x = ghostTempleArea[1].x + ghostTempleBorderOffset;
        float gtempleBR_z = ghostTempleArea[1].z + ghostTempleBorderOffset;

        // check in ghost temple
        if (spawnX > gtempleTL_x && spawnZ > gtempleTL_z && spawnX < gtempleBR_x && spawnZ < gtempleBR_z)
        {
            return false;
        }

        // check in forbidden area
        if(spawnX > forbiddenArea[0].x && spawnZ > forbiddenArea[0].z && spawnX < forbiddenArea[1].x && spawnZ < forbiddenArea[1].z)
        {
            return false;
        }

        return true;

    }

    private bool CheckInSpawnZones(float spawnX, float spawnZ)
    {
        Vector3 centralPoint;
        float width;
        float length;

        Vector3 topLeft;
        Vector3 bottomRight;
        
        for(int i = 0; i < SpawnZones.Length; i++)
        {
            // calculate the correct spawn zone
            width = SpawnZones[i].GetComponent<MeshRenderer>().bounds.size.x;
            length = SpawnZones[i].GetComponent<MeshRenderer>().bounds.size.z;
            centralPoint = SpawnZones[i].transform.position;

            topLeft = new Vector3(centralPoint.x - width / 2, 0, centralPoint.z - length / 2);
            bottomRight = new Vector3(centralPoint.x + width / 2, 0, centralPoint.z + length / 2);

            // if x, z is in this spawn zone, it's valid
            if (spawnX > topLeft.x && spawnZ > topLeft.z && spawnX < bottomRight.x && spawnZ < bottomRight.z)
            {
                return true;
            }
        }

        // after all spawn zone check, it's invalid
        return false;
    }

    private bool CheckInGodTemple(float spawnX, float spawnZ)
    {
        // add border offset to avoid spawn in walls, inward borders
        // templeTL(BR) stands for god temple top left(botton right)
        float templeTL_x = godTempleArea[0].x + templeBorderOffset;
        float templeTL_z = godTempleArea[0].z + templeBorderOffset;
        float templeBR_x = godTempleArea[1].x - templeBorderOffset;
        float templeBR_z = godTempleArea[1].z - templeBorderOffset;

        if (spawnX > templeTL_x && spawnZ > templeTL_z && spawnX < templeBR_x && spawnZ < templeBR_z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckInGhostTemple(float spawnX, float spawnZ)
    {
        // add border offset to avoid spawn in walls, inward borders
        // templeTL(BR) stands for ghost temple top left(botton right)
        float templeTL_x = ghostTempleArea[0].x + ghostTempleBorderOffset;
        float templeTL_z = ghostTempleArea[0].z + ghostTempleBorderOffset;
        float templeBR_x = ghostTempleArea[1].x - ghostTempleBorderOffset;
        float templeBR_z = ghostTempleArea[1].z - ghostTempleBorderOffset;

        if (spawnX > templeTL_x && spawnZ > templeTL_z && spawnX < templeBR_x && spawnZ < templeBR_z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void _DeepCopyArray(int[] target, int[] original)
    {
        if(target.Length != original.Length)
        {
            Debug.Log("Wrong array size pass from GM to Obj distributer");
            return;
        }

        for (int i = 0; i < target.Length; i++)
        {
            target[i] = original[i];
        }
    }
    private void Debuger()
    {
        Vector3 centralPoint;
        float width;
        float length;

        // calculate map first
        width = WorldSpace.GetComponent<Terrain>().terrainData.size.x;
        length = WorldSpace.GetComponent<Terrain>().terrainData.size.z;
        centralPoint = WorldSpace.transform.position + new Vector3(width / 2, 0, length / 2);  // because terrain start from top right corner

        Debug.Log("World pos: " + centralPoint);
        Debug.Log("World bound: " + mapArea[0] + mapArea[1]);
        //Debug.Log("God temple pos: " + GodTempleFloor.transform.position);
        //Debug.Log("God temple bound: " + GodTempleFloor.GetComponent<MeshRenderer>().bounds.size);
        //Debug.Log("Ghost temple pos: " + GhostTempleFloor.transform.position);
        //Debug.Log("Ghost temple bound: " + GhostTempleFloor.GetComponent<MeshRenderer>().bounds.size);
        //float templeTL_x = godTempleArea[0].x + borderOffset;
        //float templeTL_z = godTempleArea[0].z + borderOffset;
        //float templeBR_x = godTempleArea[1].x - borderOffset;
        //float templeBR_z = godTempleArea[1].z - borderOffset;
        //Debug.Log("top left: " + templeTL_x + ", " + templeTL_z);
        //Debug.Log("bottom right: " + templeBR_x + ", " + templeBR_z);
        //Debug.Log("Player pos: " + Player.transform.position);
        //Debug.Log("God temple top left: " + godTempleArea[0]);
        //Debug.Log("God temple bottom right: " + godTempleArea[1]);
    }
}
