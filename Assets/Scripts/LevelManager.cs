using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	public static int mapRows = 10;
	public static int mapCols = 10;

	public GameObject sliderPrefab;

	GameObject slidersFolder;

	float sliderWidth = 1;

	int[,] map = new int[mapRows, mapCols];

	private void Awake() {
		instance = this;
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void GenerateRound(int levels) {
		slidersFolder = GameObject.Find("Sliders");

		// Temporary for boundary marking
		CreateSlider(0, 0);
		CreateSlider(0, mapCols - 1);
		CreateSlider(mapRows - 1, 0);
		CreateSlider(mapRows - 1, mapCols - 1);
		// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

		int row = Random.Range(0, mapRows);
		int col = Random.Range(0, mapCols);
		CreateSlider(row, col);
		map[row, col] = 3;
		bool isOnRow = Random.Range(0, 2) == 1 || true;
		print($"({row}, {col}); isOnRow: {isOnRow}");
		if (isOnRow) {
			// Next child will be on the same row as start
			int newCol = Random.Range(0, mapCols - 1);
			if (newCol == col) newCol = (newCol + 1) % mapCols;
			int negative = col < newCol ? 1 : -1;
			for (int i = 1; i < Mathf.Abs(newCol - col); i++) {
				map[row, col + i * negative] = 1;
			}
			col = newCol;
			//SpawnChildSlider(levels - 1, row, newCol, isOnRow);
		} else {
			// Next child will be on the same col as the start
			int newRow = Random.Range(0, mapRows - 1);
			if (newRow == row) newRow = (newRow + 1) % mapRows;
			int negative = row < newRow ? 1 : -1;
			for (int i = 1; i < Mathf.Abs(newRow - col); i++) {
				map[row + i * negative, col] = 1;
			}
			row = newRow;
			//SpawnChildSlider(levels - 1, newRow, col, isOnRow);
		}
		SpawnChildSlider(levels - 1, row, col, isOnRow);

		PrintMap();
	}

	void SpawnChildSlider(int level, int row, int col, bool parentIsOnRow) {
		if (level == 0)
			return;
		
		CreateSlider(row, col);

		if (parentIsOnRow) {
			// Spawn children in column
			// Find upper limit
			for (int i = row; i >= 0 ; i--) {
				//if (map[row, col]) ;
			}
		} else {

		}

		// Spawn left/up child
		
		// Spawn right/down child

	}

	void CreateSlider(int row, int col) {
		map[row, col] = 2;
		Instantiate(sliderPrefab, new Vector3(col * sliderWidth, 0, (mapRows - row - 1) * sliderWidth), sliderPrefab.transform.rotation, slidersFolder.transform);
	}

	void PrintMap() {
		string str = "";
		for (int r = 0; r < mapRows; r++) {
			for (int c = 0; c < mapCols; c++) {
				if (map[r, c] == 3)
					str += "3 ";
				else if (map[r, c] == 2)
					str += "2 ";
				else if (map[r, c] == 1)
					str += "- ";
				else
					str += ". ";
				//str += map[r, c] + "  ";
			}
			str += "\n";
		}
		print(str);
	}

}
