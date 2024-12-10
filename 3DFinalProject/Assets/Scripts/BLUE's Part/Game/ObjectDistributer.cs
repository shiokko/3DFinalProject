using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistributer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField]
    private GameObject WorldSpace;
    [SerializeField]
    private GameObject GodTempleFloor;
    [SerializeField]
    private GameObject GhostTempleFloor;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private GameObject GM;

    [Header("Prefabs to spawn")]
    [SerializeField]
    private GameObject CharmPrefab;
    [SerializeField]
    private GameObject IncensePrefab;
    [SerializeField]
    private GameObject DivinationBlockPrefab;

    [Header("Remnant Prefabs")]
    [SerializeField]
    private GameObject[] RemnantsPrefabs;

    [Header("Parameters")]
    [SerializeField]
    private float ForbiddenWidthFromPlayer = 12f;   // set a width to prevent items spawn from player
    [SerializeField]
    private float ForbiddenLengthFromPlayer = 12f;  // set a length to prevent items spawn from player
    [SerializeField]
    private float borderOffset = 3f;  // set an offset for each spawned item to avoid spawn in walls
    [SerializeField]
    private int NumRemnantCategory = 3;
    [SerializeField]
    private int NumWrongRemnants = 2;
    [SerializeField]
    private int CharmNum = 10;
    [SerializeField]
    private int IncenseNum = 10;
    [SerializeField]
    private int DivinationBlockNum = 10;
    [SerializeField]
    private float MaxFloorHeight = 0;  // will change when we implement the tarren

    // define areas with rectangle shape, the first val is top left, the second val is bottom right
    // we view it as 2D so y is always 0
    private Vector3[] spawnArea = new Vector3[2];
    private Vector3[] godTempleArea = new Vector3[2];
    private Vector3[] ghostTempleArea = new Vector3[2]
;    private Vector3[] forbiddenArea = new Vector3[2];

    // the correct remnants
    private int[] correctRemnantsID;

    // Start is called before the first frame update
    void Start()
    {
        correctRemnantsID = new int[NumRemnantCategory];
        // call this from Game Manager:
        // _DeepCopyArray(correctRemnantsID, GM.GetComponent<GameManager>().GetCorrectRemnants());
        int[] fakeData = new int[3];
        fakeData[0] = (int)Remnent.COMB; fakeData[1] = (int)Remnent.TOY; fakeData[2] = (int)Remnent.GOLD;

        _DeepCopyArray(correctRemnantsID, fakeData);

        CalculateAreas();
        //Debuger();

        DistributeItems();
        DistributeRemnants();
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

        // calculate map first
        centralPoint = WorldSpace.transform.position;
        width = WorldSpace.GetComponent<MeshRenderer>().bounds.size.x;
        length = WorldSpace.GetComponent<MeshRenderer>().bounds.size.z;

        spawnArea[0] = new Vector3(centralPoint.x - width / 2, 0, centralPoint.z - length / 2);
        spawnArea[1] = new Vector3(centralPoint.x + width / 2, 0, centralPoint.z + length / 2);

        // calculate God temple area
        centralPoint = GodTempleFloor.transform.position;
        width = GodTempleFloor.GetComponent<MeshRenderer>().bounds.size.x;
        length = GodTempleFloor.GetComponent<MeshRenderer>().bounds.size.z;

        godTempleArea[0] = new Vector3(centralPoint.x - width / 2, 0, centralPoint.z - length / 2);
        godTempleArea[1] = new Vector3(centralPoint.x + width / 2, 0, centralPoint.z + length / 2);

        // calculate Ghost temple area
        centralPoint = GhostTempleFloor.transform.position;
        width = GhostTempleFloor.GetComponent<MeshRenderer>().bounds.size.x;
        length = GhostTempleFloor.GetComponent<MeshRenderer>().bounds.size.z;

        ghostTempleArea[0] = new Vector3(centralPoint.x - width / 2, 0, centralPoint.z - length / 2);
        ghostTempleArea[1] = new Vector3(centralPoint.x + width / 2, 0, centralPoint.z + length / 2);

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
        int inTempleCategoryID = Random.Range(0, NumRemnantCategory);

        // start spawning each correct remnant
        for (int i = 0; i < NumRemnantCategory; i++)  // for each category
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

        // start spawning wrong remnants
        for (int i = 0; i < NumWrongRemnants; i++)
        {
            int WrongRemnantID = Random.Range(0, RemnantsPrefabs.Length);

            // check if it's the wrong remnant
            while (true)
            {
                bool isWrong = true;

                for (int j = 0; j < NumRemnantCategory; j++)  // check for each category
                {
                    if (WrongRemnantID == correctRemnantsID[j])
                    {
                        isWrong = false;
                    }
                }

                if (isWrong)
                {
                    break;
                }
                else
                {
                    WrongRemnantID = Random.Range(0, RemnantsPrefabs.Length);
                }
            }

            SpawnOutsideItems(RemnantsPrefabs[WrongRemnantID], 1);
        }
    }

    private void SpawnOutsideItems(GameObject _prefab, int _prefabNum)
    {
        float spawnX;
        float spawnZ;

        for (int i = 0; i < _prefabNum; i++)
        {
            spawnX = Random.Range(spawnArea[0].x + borderOffset, spawnArea[1].x - borderOffset);
            spawnZ = Random.Range(spawnArea[0].z + borderOffset, spawnArea[1].z - borderOffset);

            while (!CheckOutsideTemple(spawnX, spawnZ))
            {
                // re-choose a spawn point
                spawnX = Random.Range(spawnArea[0].x + borderOffset, spawnArea[1].x - borderOffset);
                spawnZ = Random.Range(spawnArea[0].z + borderOffset, spawnArea[1].z - borderOffset);
            }

            Instantiate(_prefab, new Vector3(spawnX, MaxFloorHeight + borderOffset, spawnZ), transform.rotation);
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
            float templeTL_x = godTempleArea[0].x + borderOffset;
            float templeTL_z = godTempleArea[0].z + borderOffset;
            float templeBR_x = godTempleArea[1].x - borderOffset;
            float templeBR_z = godTempleArea[1].z - borderOffset;

            spawnX = Random.Range(templeTL_x, templeBR_x);
            spawnZ = Random.Range(templeTL_z, templeBR_z);

            Instantiate(_prefab, new Vector3(spawnX, MaxFloorHeight + borderOffset, spawnZ), transform.rotation);
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
            float templeTL_x = ghostTempleArea[0].x + borderOffset;
            float templeTL_z = ghostTempleArea[0].z + borderOffset;
            float templeBR_x = ghostTempleArea[1].x - borderOffset;
            float templeBR_z = ghostTempleArea[1].z - borderOffset;

            spawnX = Random.Range(templeTL_x, templeBR_x);
            spawnZ = Random.Range(templeTL_z, templeBR_z);

            Instantiate(_prefab, new Vector3(spawnX, MaxFloorHeight + borderOffset, spawnZ), transform.rotation);
        }
    }

    private bool CheckOutsideTemple(float spawnX, float spawnZ)
    {
        // add border offset to avoid spawn in walls, outward borders
        // templeTL(BR) stands for god temple top left(botton right)
        float templeTL_x = godTempleArea[0].x - borderOffset;
        float templeTL_z = godTempleArea[0].z - borderOffset;
        float templeBR_x = godTempleArea[1].x + borderOffset;
        float templeBR_z = godTempleArea[1].z + borderOffset;

        // check in god temple
        if (spawnX > templeTL_x && spawnZ > templeTL_z && spawnX < templeBR_x && spawnZ < templeBR_z)
        {
            return false;
        }

        // gtempleTL(BR) stands for ghost temple top left(botton right)
        float gtempleTL_x = ghostTempleArea[0].x - borderOffset;
        float gtempleTL_z = ghostTempleArea[0].z - borderOffset;
        float gtempleBR_x = ghostTempleArea[1].x + borderOffset;
        float gtempleBR_z = ghostTempleArea[1].z + borderOffset;

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

    private bool CheckInGodTemple(float spawnX, float spawnZ)
    {
        // add border offset to avoid spawn in walls, inward borders
        // templeTL(BR) stands for god temple top left(botton right)
        float templeTL_x = godTempleArea[0].x + borderOffset;
        float templeTL_z = godTempleArea[0].z + borderOffset;
        float templeBR_x = godTempleArea[1].x - borderOffset;
        float templeBR_z = godTempleArea[1].z - borderOffset;

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
        float templeTL_x = ghostTempleArea[0].x + borderOffset;
        float templeTL_z = ghostTempleArea[0].z + borderOffset;
        float templeBR_x = ghostTempleArea[1].x - borderOffset;
        float templeBR_z = ghostTempleArea[1].z - borderOffset;

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
        //Debug.Log("World pos: " + WorldSpace.transform.position);
        //Debug.Log("World bound: " + WorldSpace.GetComponent<MeshRenderer>().bounds.size);
        //Debug.Log("God temple pos: " + GodTempleFloor.transform.position);
        //Debug.Log("God temple bound: " + GodTempleFloor.GetComponent<MeshRenderer>().bounds.size);
        Debug.Log("Ghost temple pos: " + GhostTempleFloor.transform.position);
        Debug.Log("Ghost temple bound: " + GhostTempleFloor.GetComponent<MeshRenderer>().bounds.size);
        float templeTL_x = godTempleArea[0].x + borderOffset;
        float templeTL_z = godTempleArea[0].z + borderOffset;
        float templeBR_x = godTempleArea[1].x - borderOffset;
        float templeBR_z = godTempleArea[1].z - borderOffset;
        Debug.Log("top left: " + templeTL_x + ", " + templeTL_z);
        Debug.Log("bottom right: " + templeBR_x + ", " + templeBR_z);
        //Debug.Log("Player pos: " + Player.transform.position);
        //Debug.Log("God temple top left: " + godTempleArea[0]);
        //Debug.Log("God temple bottom right: " + godTempleArea[1]);
    }
}
