using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    [System.Serializable]
    public class LevelData
    {
        public Transform[] levelCorners;
    }

    [System.Serializable]
    public class LevelTransitionPoint
    {
        public string transitionName;
        public int targetLevel;
        public float transitionActivationDistance;
        public LevelTransitionRequirement requirement;
        public Transform transitionPoint;
        public Transform transitionExitPoint;
    }

    public List<LevelTransitionPoint> allTransitions;
    public List<LevelData> allLevels;

    public void Update()
    {
        for (int i = 0; i < allTransitions.Count; i++)
        {
            if (allTransitions[i].requirement.CanUseTransition() &&
                Vector3.Distance(PlayerMovement.playerTransform.position, allTransitions[i].transitionPoint.position) < allTransitions[i].transitionActivationDistance)
            {
                StartCoroutine(TransitionTo(allTransitions[i].transitionExitPoint, allLevels[allTransitions[i].targetLevel].levelCorners));
            }
        }
    }

    public IEnumerator TransitionTo(Transform exitPoint, Transform[] newCorners)
    {
        PlayerMovement.playerTransform.position = exitPoint.position;
        PlayerMovement.freezeMovement = true;
        CameraMovementController.instance.camBoundsObjects = newCorners;
        CameraMovementController.instance.GenerateWorldBounds();
        yield return new WaitForSeconds(0.2f);
        PlayerMovement.freezeMovement = false;
    }
}
