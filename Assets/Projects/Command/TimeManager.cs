using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace CommandPattern
{
    public class TimeManager : MonoBehaviour
    {
        private static TimeManager instance;
        public static TimeManager Instance => instance;
        private static float reversalMultiplier = 1f; // 2f = twice as fast, 0.5f = slow motion

        private static float reversalSpeed = 12f;
        private static float recordInterval = 1f; // seconds between snapshots
        private float timeSinceLastRecord = 0f;
        private float currentTime = 0f;

        private SortedList<float, List<MoveCommand>> snapShots = new();
        private Dictionary<TimeReversible, (Vector3 pos, Quaternion rot)> StateXFramesAgo = new();

        private bool isReversing = false;
        public bool IsReversing => isReversing;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        void Start()
        {
            var reversibles = FindObjectsByType<TimeReversible>(FindObjectsSortMode.None).ToList();
            foreach (var reversible in reversibles)
                StateXFramesAgo.Add(reversible, (reversible.transform.position, reversible.transform.rotation));
        }

        void Update()
        {
            if (!isReversing)
            {
                HandleRecording();

                if (Input.GetKeyDown(KeyCode.R))
                    StartReversing();
            }
            else
            {
                HandleReversing();

                if (Input.GetKeyUp(KeyCode.R))
                    StopReversing();
            }
        }

        public void StartReversing()
        {
            if (isReversing) return;

            if (snapShots.Count == 0)
            {
                currentTime = 0f;
                return;
            }

            foreach (TimeReversible reversible in StateXFramesAgo.Keys)
                reversible.Reverse();

            currentTime = snapShots.Keys.Last();
            isReversing = true;
        }

        public void StopReversing()
        {
            if (!isReversing) return;

            if (snapShots.ContainsKey(currentTime))
            {
                foreach (var command in snapShots[currentTime])
                {
                    if (command.currentT != 0)
                        command.Undo();
                }
                // step back one snapshot
                int idx = snapShots.IndexOfKey(currentTime) - 1;
                currentTime = idx >= 0 ? snapShots.Keys[idx] : 0f;
            }

            var copy = StateXFramesAgo.Keys.ToList();
            foreach (var reversible in copy)
                reversible.StopReversing();

            foreach (var reversible in copy)
                StateXFramesAgo[reversible] = (reversible.transform.position, reversible.transform.rotation);

            ClearAllSnapshotsAfterTime(currentTime);
            isReversing = false;
        }

        private void ClearAllSnapshotsAfterTime(float time)
        {
            var keysToRemove = snapShots.Keys.Where(k => k > time).ToList();
            foreach (var key in keysToRemove)
                snapShots.Remove(key);
        }

        private void HandleRecording()
        {
            timeSinceLastRecord += Time.deltaTime;
            currentTime += Time.deltaTime;

            if (timeSinceLastRecord < recordInterval)
                return;

            timeSinceLastRecord = 0f;

            List<(TimeReversible key, Vector3 newPos, Vector3 oldPos, Quaternion newRot, Quaternion oldRot)> changes = new();

            foreach (var kvp in StateXFramesAgo)
            {
                Vector3 oldPos = kvp.Value.pos;
                Vector3 newPos = kvp.Key.transform.position;
                Quaternion oldRot = kvp.Value.rot;
                Quaternion newRot = kvp.Key.transform.rotation;

                if (newPos != oldPos || newRot != oldRot)
                    changes.Add((kvp.Key, newPos, oldPos, newRot, oldRot));
            }

            foreach (var (key, newPos, oldPos, newRot, oldRot) in changes)
            {
                StateXFramesAgo[key] = (newPos, newRot);
                MoveCommand move = new MoveCommand(oldPos, newPos, oldRot, newRot, key.gameObject);
                if (!snapShots.ContainsKey(currentTime))
                    snapShots[currentTime] = new List<MoveCommand>();
                snapShots[currentTime].Add(move);
            }
        }

        private void HandleReversing()
        {
            if (currentTime <= 0f)
            {
                StopReversing();
                return;
            }

            if (!snapShots.ContainsKey(currentTime))
                return;

            bool allDone = true;
            float t = (reversalMultiplier / recordInterval) * Time.deltaTime;

            foreach (var command in snapShots[currentTime])
            {
                if (command.currentT != 0)
                {
                    command.LerpTowardsOld(t);
                    allDone = false;
                }
            }

            if (allDone)
            {
                int previousIdx = snapShots.IndexOfKey(currentTime) - 1;
                currentTime = previousIdx >= 0 ? snapShots.Keys[previousIdx] : 0f;
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var frame in snapShots)
                foreach (var command in frame.Value)
                    Debug.DrawLine(command.newPos, command.oldPos, Color.yellow);
        }
    }
}