using UnityEngine;

namespace JeuA
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] bool enableTutorial;

        [SerializeField] int maxIndex;

        [SerializeField] int indexDoublon;

        [SerializeField] int indexArrowTutorial;

        [SerializeField] ToothManager toothManager;

        [SerializeField] Transform arrowContainer;

        private void Start()
        {
            if (enableTutorial)
            {
                foreach (Transform child in transform)
                    child.gameObject.SetActive(false);

                UpdateChildVisibility(0);
                toothManager.GenerationListIndex.AddListener(UpdateChildVisibility);
            }
        }

        private void UpdateChildVisibility(int index)
        {
            if (index > maxIndex)
                return;

            if (index == indexArrowTutorial)
                arrowContainer.gameObject.SetActive(true);

            transform.GetChild(index).gameObject.SetActive(true);
            if (index == indexDoublon)
                transform.GetChild(index + 1).gameObject.SetActive(true);
        }
    }

}