using NaughtyAttributes;
using SignalSystem;
using System.Collections.Generic;
using UnityEngine;
using JeuA;

namespace Tools
{
    public class CellSetup : MonoBehaviour
    {
        public List<GameObject> cells = new List<GameObject>();

        public List<GameObject> cellsVisual = new List<GameObject>();

        public SO_CellData cellData;

        public Transform[] foodPrefabs;

        public bool DestroyOnEnd = true;

        [Button]
        public void Setup()
        {
            if (cells.Count != cellsVisual.Count)
            {
                Debug.LogError("Cells and CellsVisual lists must have the same size");
                return;
            }

            Dictionary<GameObject, GameObject> cellToVisual = new Dictionary<GameObject, GameObject>();

            for (int i = 0; i < cells.Count; i++)
            {
                cellToVisual.Add(cells[i], cellsVisual[i]);
            }

            foreach (var cell in cellToVisual)
            {
                GameObject gO = cell.Key;
                GameObject visual = cell.Value;

                ClearCellComponents(gO);

                gO.transform.position = visual.transform.position;

                MeshFilter mF = gO.AddComponent<MeshFilter>();
                mF.sharedMesh = visual.GetComponent<MeshFilter>().sharedMesh;

                MeshRenderer mR = gO.AddComponent<MeshRenderer>();
                mR.sharedMaterial = visual.GetComponent<MeshRenderer>().sharedMaterial;

                Rigidbody rB = gO.AddComponent<Rigidbody>();

                rB.isKinematic = true;

                MeshCollider mC = gO.AddComponent<MeshCollider>();

                mC.convex = true;
                mC.sharedMesh = visual.GetComponent<MeshFilter>().sharedMesh;

                gO.AddComponent<Interactable>();

                CellBehavior cB = gO.AddComponent<CellBehavior>();

                cB.cellData = cellData;
                cB.teethState = TeethState.Clean;
                cB.foodPrefab = foodPrefabs;

                gO.AddComponent<SignalListener>();

                gO.tag = "Cell";
            }

            ToothManager tM = cells[1].GetComponentInParent<ToothManager>();

            tM.teethCells.Clear();

            List<CellBehavior> cellsB = new List<CellBehavior>();

            foreach (var cell in cells)
            {
                cellsB.Add(cell.GetComponent<CellBehavior>());
            }

            tM.teethCells = cellsB;

            Debug.LogWarning("Setup Complete" + "please set up the neighbors manually");

            if (DestroyOnEnd)
            {
                DestroyImmediate(this);
            }
        }

        [Button]
        public void RenameCells()
        {
            for (int i = 1; i < cells.Count + 1; i++)
            {
                cells[i - 1].name = "Cell " + i;
            }
        }

        public void ClearCellComponents(GameObject cell)
        {
            if (cell.TryGetComponent<SignalListener>(out SignalListener sign))
                DestroyImmediate(sign);

            if (cell.TryGetComponent<CellBehavior>(out CellBehavior cellB))
                DestroyImmediate(cellB);

            if (cell.TryGetComponent<Interactable>(out Interactable inter))
                DestroyImmediate(inter);

            if (cell.TryGetComponent<Rigidbody>(out Rigidbody rB))
                DestroyImmediate(rB);

            foreach (Component component in cell.GetComponents<Component>())
            {
                if (component is Transform)
                    continue;

                DestroyImmediate(component);
            }
        }
    }

}