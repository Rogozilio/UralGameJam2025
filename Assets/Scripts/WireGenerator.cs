using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WireGenerator : MonoBehaviour
{
    public Transform[] points;
    public float wireRadius = 0.1f; // Ширина полоски
    public int segments = 8;
    private MeshFilter mesh;

    void Start()
    {
        mesh = GetComponent<MeshFilter>();
        GenerateWireMesh();
    }

    // private void FixedUpdate()
    // {
    //     GenerateWireMesh();
    // }

    void GenerateWireMesh()
    {
        if (points == null || points.Length < 2)
    {
        Debug.LogError("At least two points are required to generate a wire.");
        return;
    }

    Mesh mesh = new Mesh();

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    // Параметры профиля (круг)
    float angleStep = 360f / segments;

    // Список ориентаций для каждой точки
    List<Quaternion> orientations = new List<Quaternion>();

    // Вычисляем ориентации для каждой точки
    for (int i = 0; i < points.Length; i++)
    {
        Vector3 forward = i < points.Length - 1
            ? (points[i + 1].localPosition - points[i].localPosition).normalized
            : (points[i].localPosition - points[i - 1].localPosition).normalized;

        Vector3 up = Vector3.up;
        Vector3 right = Vector3.Cross(forward, up).normalized;

        // Если Cross дает ноль, выбираем другой `up`
        if (right.magnitude < 0.01f)
        {
            up = Vector3.right;
            right = Vector3.Cross(forward, up).normalized * wireRadius;
        }

        up = Vector3.Cross(right, forward).normalized  * wireRadius;

        // Сохраняем ориентацию в виде кватерниона
        orientations.Add(Quaternion.LookRotation(forward, up));
    }

    // Сглаживаем ориентации
    for (int i = 1; i < orientations.Count - 1; i++)
    {
        orientations[i] = Quaternion.Slerp(orientations[i - 1], orientations[i + 1], 0.5f);
    }

    // Генерация меша
    for (int i = 0; i < points.Length; i++)
    {
        Quaternion rotation = orientations[i];

        // Генерируем вершины для текущего кольца
        for (int j = 0; j < segments; j++)
        {
            float angle = Mathf.Deg2Rad * j * angleStep;
            Vector3 offset = rotation * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * wireRadius;
            vertices.Add(points[i].localPosition + offset);

            // Добавляем UV
            uvs.Add(new Vector2((float)j / segments, (float)i / points.Length));
        }
    }

    // Создаем треугольники
    for (int i = 0; i < points.Length - 1; i++) // Для каждого сегмента между точками
    {
        int startIndex = i * segments;
        int nextIndex = (i + 1) * segments;

        for (int j = 0; j < segments; j++) // Для каждого сегмента кольца
        {
            int current = startIndex + j;
            int next = startIndex + (j + 1) % segments;
            int currentNext = nextIndex + j;
            int nextNext = nextIndex + (j + 1) % segments;

            triangles.Add(current);
            triangles.Add(next);
            triangles.Add(currentNext);

            triangles.Add(next);
            triangles.Add(nextNext);
            triangles.Add(currentNext);
        }
    }

    // Применяем меш
    mesh.vertices = vertices.ToArray();
    mesh.triangles = triangles.ToArray();
    mesh.uv = uvs.ToArray();
    mesh.RecalculateNormals();

    this.mesh.mesh = mesh;
    }
}