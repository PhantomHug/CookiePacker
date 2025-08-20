using System.Collections.Generic;
using _Features.Gameplay.Boxes;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Box> _boxes;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private TextMesh _uiBoxesCount;
    private Queue<GameObject> _boxQueueObjects;
    public Queue<Box> _boxQueue { get; private set; }
    
    private void Awake()
    {
        _boxQueue = new Queue<Box>(_boxes);
        _boxQueueObjects = new Queue<GameObject>();

        foreach (Box box in _boxes)
        {
            //box.Init(OnBoxRemoved);
        }
        
        foreach (var box in _boxes)
        {
            _boxQueueObjects.Enqueue(box.gameObject);
        }
        ShowFirstBox();
    }

    private void ShowFirstBox()
    {
        if (_boxQueueObjects.Count > 0)
        {
            var _boxPosition = _boxQueueObjects.Peek();
            _boxPosition.transform.position = _spawnPoint.position;
            _boxPosition.SetActive(true);
        }
        _uiBoxesCount.text = _boxQueue.Count.ToString();
    }
    
    private void OnDrawGizmosSelected()
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(transform.up, new Vector3(1, .5f, 3));
    }

    private void OnBoxRemoved()
    {
        if (_boxQueueObjects.Count >= 1)
        {
            _boxQueueObjects.Dequeue();
            _boxQueue.Dequeue();
            ShowFirstBox();
        }
    }
}
