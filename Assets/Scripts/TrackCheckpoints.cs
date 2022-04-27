using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public class CarCheckpointEventArgs : EventArgs
    {
        public Transform carTransform { get; set; }
    }
    public event EventHandler<CarCheckpointEventArgs> OnPlayerCorrectCheckpoint;
    public event EventHandler<CarCheckpointEventArgs> OnPlayerWrongCheckpoint;

    [SerializeField] private List<Transform> carTransformList;
    [SerializeField] private Transform checkpointsTransform;
    private List<CheckpointSingle> checkpointSingleList;
    private List<int> nextCheckpointSingleIndexList;

    private void Awake()
    {
        checkpointSingleList = new List<CheckpointSingle>();

        foreach (Transform checkpointSingleTransform in transform)
        {
            CheckpointSingle checkpointSingle = checkpointSingleTransform.GetComponent<CheckpointSingle>();

            checkpointSingle.SetTrackCheckpoints(this);

            checkpointSingleList.Add(checkpointSingle);
        }

        nextCheckpointSingleIndexList = new List<int>();
        foreach (Transform carTransform in carTransformList)
        {
            nextCheckpointSingleIndexList.Add(0);
        }
    }
    public CheckpointSingle GetNextCheckpoint(Transform carTransform)
    {
        return checkpointSingleList[nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]];
    }
    public void ResetCheckpoint(Transform carTransform)
    {
        //nextCheckpointSingleIndexList[CheckpointSingleIndexList.IndexOf(carTransform)] = 0;
        nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)] = 0;
    }
    public void CarThroughCheckpoint(CheckpointSingle checkpointSingle, Transform carTransform)
    {
        int nextCheckpointSingleIndex = nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)];
        CarCheckpointEventArgs eventArgs = new CarCheckpointEventArgs();
        if (checkpointSingleList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            //Correct Checkpoint
            Debug.Log("Correct");
            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            eventArgs.carTransform = carTransform;
            correctCheckpointSingle.Hide();

            nextCheckpointSingleIndexList[carTransformList.IndexOf(carTransform)]
                = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, eventArgs);
        }
        else
        {
            //Wrong Checkpoint
            Debug.Log("Wrong");
            OnPlayerWrongCheckpoint?.Invoke(this, eventArgs);

            CheckpointSingle correctCheckpointSingle = checkpointSingleList[nextCheckpointSingleIndex];
            correctCheckpointSingle.Show();
        }
    }
}


