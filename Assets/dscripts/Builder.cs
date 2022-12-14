using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace HaiThere.Playbook
{
  public class Builder
  {
    // quick copy from https://ilkinulas.github.io/development/unity/2016/04/30/cube-mesh-in-unity3d.html
    public static Mesh CreatCube(float size)
    {
      var overallSize = size * 1;
      var anchorOffset = Vector3.one / overallSize * 0.5f;
      var mesh = new Mesh
      {
        vertices = new[]
        {
          new Vector3(0, 0, 0) - anchorOffset,
          new Vector3(overallSize, 0, 0) - anchorOffset,
          new Vector3(overallSize, overallSize, 0) - anchorOffset,
          new Vector3(0, overallSize, 0) - anchorOffset,
          new Vector3(0, overallSize, overallSize) - anchorOffset,
          new Vector3(overallSize, overallSize, overallSize) - anchorOffset,
          new Vector3(overallSize, 0, overallSize) - anchorOffset,
          new Vector3(0, 0, overallSize) - anchorOffset
        },
        triangles = new[]
        {
          0, 2, 1, //face front
          0, 3, 2,
          2, 3, 4, //face top
          2, 4, 5,
          1, 2, 5, //face right
          1, 5, 6,
          0, 7, 4, //face left
          0, 4, 3,
          5, 4, 7, //face back
          5, 7, 6,
          0, 6, 7, //face bottom
          0, 1, 6
        }
      };
      mesh.RecalculateNormals();
      return mesh;
    }

    public static Mesh CreateCone(float size, float height)
    {
      var overallSize = size * 1;
      var anchorOffset = Vector3.one / overallSize * 0.5f;
      var mesh = new Mesh
      {
        vertices = new[]
        {
          new Vector3(0, 0, 0) - anchorOffset,
          new Vector3(0, overallSize, 0) - anchorOffset,
          new Vector3(overallSize, overallSize, 0) - anchorOffset,
          new Vector3(overallSize, 0, 0) - anchorOffset,
          new Vector3(overallSize * 0.5f, overallSize * 0.5f, height) - anchorOffset
        },
        triangles = new[]
        {
          0, 1, 2, // face bottom
          0, 2, 3,
          0, 4, 1, // face front
          1, 4, 2, // face right
          2, 4, 3, // face back
          3, 4, 0 // face left
        }
      };
      mesh.RecalculateNormals();
      return mesh;
    }

    public static Mesh CreatHollowCylinder(int sides, float innerRadius, float offsetSize)
    {

      var center = Vector3.zero;
      var points = new List<Vector3>();
      var triangles = new List<int>();

      points.AddRange(CreatePointRing(center, sides, innerRadius));
      points.AddRange(CreatePointRing(center, sides, innerRadius + offsetSize));
      triangles.AddRange(getHollowCapTris(points));

      points.AddRange(points.Select(x => x + new Vector3(0, 0, 1)).ToList() );
      triangles.AddRange(getHollowCapTris(points));

      var obj = new Mesh();
      obj.SetVertices(points);
      obj.SetTriangles(triangles, 0);
      obj.RecalculateNormals();

      return obj;
    }




    public static Mesh CreateCircle(int sides, float innerRadius, float offsetSize) => CreateCircle(sides, innerRadius, offsetSize, Vector3.zero);

    public static Mesh CreateCircle(int sides, float innerRadius, float offsetSize, Vector3 center)
    {
      var points = new List<Vector3>();

      points.AddRange(CreatePointRing(center, sides, innerRadius));
      points.AddRange(CreatePointRing(center, sides, innerRadius + offsetSize));
      var triangles = getHollowCapTris(points);

      var obj = new Mesh();
      obj.SetVertices(points);
      obj.SetTriangles(triangles, 0);
      return obj;
    }

    public static IEnumerable<Vector3> CreatePointRing(Vector3 center, int sides, float radius, float positionOffset = 0f)
    {
      const float T = 2 * Mathf.PI;
      var points = new List<Vector3>();
      var circSteps = 1f / sides;
      var radianSteps = circSteps * T;

      for (var i = 0; i < sides; i++)
      {
        var radian = radianSteps * i;
        points.Add(
          center + new Vector3(Mathf.Cos(radian) * radius, Mathf.Sin(radian) * radius, 0)
        );
      }

      return points;
    }


    static List<int> getHollowCapTris(ICollection points)
    {
      var triSides = points.Count / 2;
      var triangles = new List<int>();

      for (int index = 0; index < triSides; index++)
      {
        triangles.Add(index);
        triangles.Add(index + triSides);
        triangles.Add((index + 1) % triSides);

        triangles.Add(index);
        triangles.Add(triSides + (triSides + index - 1) % triSides);
        triangles.Add(index + triSides);
      }
      return triangles;
    }
  }
}
