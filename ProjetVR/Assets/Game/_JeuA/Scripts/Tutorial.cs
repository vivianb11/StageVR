using System;
using UnityEngine;

namespace JeuA
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] bool enableTutorial;

        [SerializeField] int maxIndex;

        [SerializeField] int indexArrowTutorial;

        [SerializeField] ToothManager toothManager;

        [SerializeField] Transform arrowContainer;

        [SerializeField] GameObject[] tools;

        public static bool inTutorial;

        private void Start()
        {
            if (enableTutorial)
            {
                inTutorial = true;

                foreach (GameObject tool in tools)
                    tool.SetActive(false);

                UpdateChildVisibility(0);
                toothManager.GenerationListIndex.AddListener(UpdateChildVisibility);
            }
        }

        private void UpdateChildVisibility(int index)
        {
            if (index > maxIndex)
            {
                inTutorial = false;
                return;
            }

            if (index == indexArrowTutorial)
                arrowContainer.gameObject.SetActive(true);

            tools[index].gameObject.SetActive(true);
        }
    }

}