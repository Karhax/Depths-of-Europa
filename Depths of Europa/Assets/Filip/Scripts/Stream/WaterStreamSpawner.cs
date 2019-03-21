using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStreamSpawner : MonoBehaviour
{
    [Header("Settings")]

    [SerializeField, Range(0.1f, 100)] float _spawnSpeed;
    [SerializeField, Range(0, 30)] float _maxRotationSpeed;

    [Header("Drop")]

    [SerializeField] GameObject[] _piecesToSpawn;

    Timer _spawnTimer;

    float _localMaxY;
    float _localMinY;
    float _localMaxX;
    float _localMinX;

    private void Awake()
    {
        _spawnTimer = new Timer(_spawnSpeed, _spawnSpeed);

        BoxCollider2D spawnBox = GetComponent<BoxCollider2D>();

        _localMaxY = spawnBox.size.y / 2;
        _localMinY = -spawnBox.size.y / 2;

        _localMaxX = spawnBox.size.x / 2;
        _localMinX = -spawnBox.size.x / 2;

        Destroy(spawnBox);
    }

    private void Update()
    {
        _spawnTimer.Time += Time.deltaTime;

        if (_spawnTimer.Expired())
        {
            SpawnPiece();
            _spawnTimer.Reset();
        }
    }

    private void SpawnPiece()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(_localMinX, _localMaxX), Random.Range(_localMinY, _localMaxY));
        spawnPosition = transform.TransformPoint(spawnPosition);
        Quaternion pieceRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        GameObject piece = _piecesToSpawn[Random.Range(0, _piecesToSpawn.Length)];

        Transform spawnTransform = Instantiate(piece, spawnPosition, pieceRotation).transform;

        spawnTransform.SetParent(transform);
        Rigidbody2D spawnRigidBody = spawnTransform.GetComponent<Rigidbody2D>();
        spawnRigidBody.AddTorque(spawnRigidBody.mass * Random.Range(-_maxRotationSpeed, _maxRotationSpeed));
    }
}
