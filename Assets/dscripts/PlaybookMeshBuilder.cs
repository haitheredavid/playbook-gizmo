using UnityEngine;
namespace HaiThere.Playbook
{
  public class PlaybookMeshBuilder
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
  }
}
