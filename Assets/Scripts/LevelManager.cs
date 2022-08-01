using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	public static int mapRows = 40;
	public static int mapCols = 50;

	public GameObject sliderPrefab;

	GameObject slidersFolder;

	float sliderWidth = 1;

	public static int[,] map = new int[mapRows, mapCols];

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

		//// Temporary for boundary marking
		//CreateSlider(0, 0);
		//CreateSlider(0, mapCols - 1);
		//CreateSlider(mapRows - 1, 0);
		//CreateSlider(mapRows - 1, mapCols - 1);
		//// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

		int row = Random.Range(0, mapRows);
		int col = Random.Range(0, mapCols);
		CreateSlider(row, col);
		map[row, col] = 3;
		bool isOnRow = Random.Range(0, 2) == 1 || true; // FOR TESTING, LOCK isOnRow TO TRUE
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
			for (int i = 1; i < Mathf.Abs(newRow - row); i++) {
				map[row + i * negative, col] = 1;
			}
			row = newRow;
			//SpawnChildSlider(levels - 1, newRow, col, isOnRow);
		}
		CreateSlider(row, col);
		SpawnChildSlider(levels - 1, row, col, isOnRow);

		PrintMap();
	}

	void SpawnChildSlider(int level, int row, int col, bool parentIsOnRow) {
		if (level == 0)
			return;

		if (parentIsOnRow) {
			// Spawn children in column
			List<int> validUpperSpots = new List<int>();
			List<int> validLowerSpots = new List<int>();
			int upperLimit = 0;
			int lowerLimit = mapRows;

			// Find upper limit (exclusive)
			for (int i = row - 1; i >= 0; i--) {
				if (map[i, col] > 1) { // There is a block at (i, col)
					upperLimit = i;
					break;
				} else if (map[i, col] == 0) {
					validUpperSpots.Add(i);
				}
			}
			// Find lower limit (exclusive)
			for (int i = row + 1; i < mapRows; i++) {
				if (map[i, col] > 1) {
					lowerLimit = i;
					break;
				} else if (map[i, col] == 0) {
					validLowerSpots.Add(i);
				}
			}

			//PrintMap();
			//PrintList(validUpperSpots);
			//PrintList(validLowerSpots);

			int rowUpper = 0;
			int rowLower = 0;
			if (validUpperSpots.Count > 0) {
				// Spawn upper child
				rowUpper = validUpperSpots[Random.Range(0, validUpperSpots.Count)];
				CreateSlider(rowUpper, col);
				for (int i = rowUpper + 1; i < row; i++) {
					map[i, col] = 1;
				}
			} else {
				Debug.LogWarning("NO UPPER SPOTS FOUND. Level: " + level);
			}
			if (validLowerSpots.Count > 0) {
				// Spawn lower child
				rowLower = validLowerSpots[Random.Range(0, validLowerSpots.Count)];
				CreateSlider(rowLower, col);
				for (int i = row + 1; i < rowLower; i++) {
					map[i, col] = 1;
				}
			} else {
				Debug.LogWarning("NO LOWER SPOTS FOUND. Level: " + level);
			}

			// Recurse
			if (validUpperSpots.Count > 0) SpawnChildSlider(level - 1, rowUpper, col, !parentIsOnRow);
			if (validLowerSpots.Count > 0) SpawnChildSlider(level - 1, rowLower, col, !parentIsOnRow);
		} else {
			// Spawn children in row
			List<int> validLeftSpots = new List<int>();
			List<int> validRightSpots = new List<int>();

			// Find left limit (exclusive)
			for (int i = col - 1; i >= 0; i--) {
				if (map[row, i] > 1) { // There is a block at (row, i)
					break;
				} else if (map[row, i] == 0) {
					validLeftSpots.Add(i);
				}
			}
			// Find right limit (exclusive)
			for (int i = col + 1; i < mapCols; i++) {
				if (map[row, i] > 1) {
					break;
				} else if (map[row, i] == 0) {
					validRightSpots.Add(i);
				}
			}

			//PrintMap();
			//PrintList(validLeftSpots);
			//PrintList(validRightSpots);

			int colRight = 0;
			int colLeft = 0;
			if (validLeftSpots.Count > 0) {
				// Spawn upper child
				colLeft = validLeftSpots[Random.Range(0, validLeftSpots.Count)];
				CreateSlider(row, colLeft);
				for (int i = colLeft + 1; i < col; i++) {
					map[row, i] = 1;
				}
			} else {
				Debug.LogWarning("NO LEFT SPOTS FOUND. Level: " + level);
			}
			if (validRightSpots.Count > 0) {
				// Spawn lower child
				colRight = validRightSpots[Random.Range(0, validRightSpots.Count)];
				CreateSlider(row, colRight);
				for (int i = col + 1; i < colRight; i++) {
					map[row, i] = 1;
				}
			} else {
				Debug.LogWarning("NO RIGHT SPOTS FOUND. Level: " + level);
			}

			// Recurse
			if (validLeftSpots.Count > 0) SpawnChildSlider(level - 1, row, colLeft, !parentIsOnRow);
			if (validRightSpots.Count > 0) SpawnChildSlider(level - 1, row, colRight, !parentIsOnRow);
		}
	}

	public void RecreateSlidersFromMap() {
		slidersFolder = GameObject.Find("Sliders");
		//PrintMap();
		for (int r = 0; r < mapRows; r++)
			for (int c = 0; c < mapCols; c++)
				if (map[r, c] > 1)
					CreateSlider(r, c);
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
					str += "* ";
				else
					str += ". ";
				//str += map[r, c] + "  ";
			}
			str += "\n";
		}
		print(str);
	}

	void PrintList(List<int> list) {
		if (list.Count == 0) {
			print("[]");
			return;
		}
		string str = "[";
		foreach (int num in list) {
			str += num + ", ";
		}
		print(str.Substring(0, str.Length - 2) + "]");
	}

}
