using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject clearPrefab;
    public GameObject wallPrefab;
    public GameObject particlePrefab;

    public GameObject clearText;

    int[,] map;
    GameObject[,] field;

    //void PrintArray() {
    //    string debugText = "";

    //    for (int i = 0; i < map.Length; i++) {
    //        debugText += map[i].ToString() + ",";
    //    }

    //    Debug.Log(debugText);
    //}

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1280, 720, false);

            map = new int[,]{
        {4, 4, 4, 4, 4, 4, 4},
        {4, 0, 0, 0, 0, 0, 4},
        {4, 0, 3, 1, 3, 0, 4},
        {4, 0, 0, 2, 0, 0, 4},
        {4, 0, 2, 3, 2, 0, 4},
        {4, 0, 0, 0, 0, 0, 4},
        {4, 4, 4, 4, 4, 4, 4},
        };

        field = new GameObject[
        map.GetLength(0),
        map.GetLength(1)
        ];

        //PrintArray();
        string debugText = "";
        for (int y = 0; y < map.GetLength(0); y++) {
            for (int x = 0; x < map.GetLength(1); x++) {
                debugText += map[y, x].ToString() + ",";

                if (map[y, x] == 1) {
                    field[y,x] = Instantiate(
                    playerPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity
                    );
                }

                if(map[y, x] == 2){
                    field[y, x] = Instantiate(
                    boxPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity
                    );
                }

                if (map[y, x] == 3) {
                    field[y, x] = Instantiate(
                    clearPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity
                    );
                }

                if (map[y, x] == 4) {
                    field[y, x] = Instantiate(
                    wallPrefab,
                    new Vector3(x, map.GetLength(0) - y, 0),
                    Quaternion.identity
                    );
                }
            }
            debugText += "\n";
        }
        Debug.Log(debugText);
    }

    Vector2Int GetPlayerIndex() {
        for (int y = 0; y < map.GetLength(0); y++) {
            for (int x = 0; x < map.GetLength(1); x++) {
                if (field[y,x] == null) {
                    continue;
                }

                if (field[y, x].tag == "Player") {
                    return new Vector2Int(x, y);
                    }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveForm, Vector2Int moveTo) {
        if (moveTo.x < 0 || moveTo.x >= map.GetLength(1)) {
            return false;
        }

        if (moveTo.y < 0 || moveTo.y >= map.GetLength(0)) {
            return false;
        }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box") {
            Vector2Int velocity = moveTo - moveForm;
            bool success = MoveNumber("Box", moveTo, moveTo + velocity);
            if (!success) {
            return false;
            }
        }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall") {
            Vector2Int velocity = moveTo - moveForm;
            bool success = MoveNumber("Player", moveTo, moveTo + velocity);
            if (!success) {
                return false;
            }
        }

        for (int i = 0; i < 6; i++) {
            Instantiate(
            particlePrefab,
            field[moveForm.y, moveForm.x].transform.position,
            Quaternion.identity
            );
        }

        //field[moveForm.y, moveForm.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);

        Vector3 moveToPosition = new Vector3(
            moveTo.x, -moveTo.y + map.GetLength(0), 0
            );
        field[moveForm.y, moveForm.x].GetComponent<Move>().MoveTo(moveToPosition);
 
        field[moveTo.y, moveTo.x] = field[moveForm.y, moveForm.x];
        field[moveForm.y, moveForm.x] = null;

        return true;
    }

    bool IsCleard() {
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++) {
            for (int x = 0; x < map.GetLength(1); x++) {
                if (map[y, x] == 3) {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for (int i = 0; i < goals.Count; i++) {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box") {
                return false;
            }
        }

        return true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(1, 0));
            
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex - new Vector2Int(1, 0));
            
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex - new Vector2Int(0, 1));

        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Vector2Int playerIndex = GetPlayerIndex();

            MoveNumber("Player", playerIndex, playerIndex + new Vector2Int(0, 1));

        }

        if (Input.GetKeyDown(KeyCode.R)) {
        }

            if (IsCleard()) {
            clearText.SetActive(true);
        }
    }
}
